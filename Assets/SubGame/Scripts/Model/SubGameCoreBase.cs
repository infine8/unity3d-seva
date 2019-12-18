using System;
using System.Collections.Generic;
using System.Linq;
using UniKid.Core;
using UniKid.Core.Model;
using UniKid.SubGame.Controller;
using UnityEngine;

namespace UniKid.SubGame.Model
{
    public abstract class SubGameCoreBase<TSettings, TUserData> : SubGameCoreBase<TSettings, TUserData, Level<Stage>, Stage>
        where TSettings : SubGameSettingsBase<Level<Stage>, Stage>, new()
        where TUserData : SubGameQueueUserDataBase, new()
    {

    }

    public abstract class SubGameCoreBase
    {
        public static Action<LevelBaseInfo, bool> StartLevelAction { get; set; }
        public static Action GameFinishedAction { get; set; }

        protected static bool CurrentLevelIsPassed = false; 

        private static int _currentLevelId;
        private static int _previosLevelId;

        public abstract SubGameName SubGameName { get; }

        public int PreviousLevelId
        {
            get { return _previosLevelId; }
            set { _previosLevelId = value; }
        }

        public int CurrentLevelId
        {
            get { return _currentLevelId; }
            set { _currentLevelId = value; }
        }

        public int CurrentStageId { get; protected set; }

        protected SubGameCoreBase()
        {
            _previosLevelId = -1;
            _currentLevelId = 0;
        }

        public abstract void ForceMoveNextLevel();

        public abstract void ForceMovePrevLevel();

        public static void StartLevel()
        {
            var availableLevelList = GetAvailableLivelList();

            if (availableLevelList.Count < 1)
            {
                if (GameFinishedAction != null && CurrentLevelIsPassed) GameFinishedAction();
                return;
            }

            availableLevelList.Shuffle();

            var nextLevel = availableLevelList[0];


            if (!CoreContext.PlayParticularGame)
            {
                CoreContext.UserData.CurrentProfile.CurrentSubGameName = nextLevel.SubGameName.ToString();
                CoreContext.UserData.CurrentProfile.CurrentLevelId = nextLevel.Id;
            }

            if (StartLevelAction != null) StartLevelAction(nextLevel, CurrentLevelIsPassed);

            CurrentLevelIsPassed = false;
        }

        private static List<LevelBaseInfo> GetAvailableLivelList()
        {
            var levelList = new List<LevelBaseInfo>();

            CoreContext.Settings.SubGameBaseInfoList.Where(x => x.IsEnabled && x.Category == SubGameCategory.School && (!CoreContext.PlayParticularGame || x.Name == CoreContext.LoadedSubGameName))
                .Select(x => x.LevelList).ToList().ForEach(levelList.AddRange);

            if (levelList.Count < 1) return levelList;

            levelList.Sort((obj1, obj2) => obj1.Id - obj2.Id);

            var levelNum = CoreContext.PlayParticularGame ? _currentLevelId : -1;
            
            var validLevelList = new List<LevelBaseInfo>();
            var currentTagKey = GetCurrentTagKey();

            foreach (var level in levelList)
            {
                if (CoreContext.PlayParticularGame ||
                    (
                    !level.IsPassed && !string.IsNullOrEmpty(level.TagSequence)
                    && (!CoreContext.UserData.CurrentProfile.IsSubGamePriorityEnabled || level.TagSequence.ToUpper().Contains(currentTagKey.ToUpper()))
                    ))
                {
                    if (levelNum < 0) levelNum = level.Id;
                    if (level.Id == levelNum) validLevelList.Add(level);
                }
            }

            if (CoreContext.UserData.CurrentProfile.IsSubGamePriorityEnabled && !CoreContext.PlayParticularGame)  _currentLevelId = levelNum;

            return validLevelList;
        }

        public static string GetCurrentTagKey()
        {
            var levelList = new List<LevelBaseInfo>();
            
            CoreContext.Settings.SubGameBaseInfoList.Where(x => x.IsEnabled).Select(x => x.LevelList).ToList().ForEach(levelList.AddRange);

            CoreContext.UserData.CurrentProfile.SubGameLevelTagArray.Sort((obj1, obj2) => obj1.Priority - obj2.Priority);

            foreach (var tag in CoreContext.UserData.CurrentProfile.SubGameLevelTagArray)
            {
                if (!tag.IsEnabled) continue;

                var tagLevelCount = levelList.Count(x => !string.IsNullOrEmpty(x.TagSequence) && x.TagSequence.ToUpper().Contains(tag.Key.ToUpper()) && !x.IsPassed);
                if (tagLevelCount > 0) return tag.Key;
            }

            return "UNDEFINED";
        }

    }

    public abstract class SubGameCoreBase<TSettings, TUserData, TSettingsLevel, TSettingsStage> : SubGameCoreBase
        where TSettings : SubGameSettingsBase<TSettingsLevel, TSettingsStage>, new()
        where TUserData : SubGameQueueUserDataBase, new()
        where TSettingsLevel : Level<TSettingsStage>, new()
        where TSettingsStage : Stage, new()
    {

        public TSettings Settings { get; private set; }
        public TUserData UserData { get; private set; }

        public TSettingsLevel CurrentLevel { get; private set; }
        public TSettingsStage CurrentStage { get; private set; }

        public int CurrentLevelAttemptNum { get; private set; }

        public List<SubGameLevelTagUserData> CurrentLevelTagList { get; private set; }

        public IQueueList<TSettingsStage, LeitnerQueue<TSettingsStage>> LevelQueue { get; private set; }
        
        public SubGameBaseInfo SubGameBaseInfo
        {
            get
            {
                if (_subGameBaseInfo != null) return _subGameBaseInfo;

                _subGameBaseInfo = CoreContext.Settings.SubGameBaseInfoList.FirstOrDefault(x => x.Name == SubGameName);

                if (_subGameBaseInfo == null) throw new Exception("GameInfo is not found: " + Settings.Name);

                return _subGameBaseInfo;
            }
        }

        private SubGameBaseInfo _subGameBaseInfo;


        protected SubGameCoreBase()
        {
            var defaultUserData = new TUserData { Name = SubGameName };


            var savedUserData = CoreContext.UserData.CurrentProfile.SubGameDataArray.FirstOrDefault(x => x.Name == SubGameName);

            UserData = (SubGameUserDataBase)(savedUserData ?? defaultUserData) as TUserData;

            if (savedUserData == null) CoreContext.UserData.CurrentProfile.SubGameDataArray = CoreContext.UserData.CurrentProfile.SubGameDataArray.Add(UserData);

            if (UserData == null) throw new Exception("UserData is null");

            Settings = CoreContext.Settings.SubGameSettingsList.Find(x => x.Name == SubGameName) as TSettings;

            if (Settings == null) throw new Exception("Settings is null");


            LevelQueue = new QueueList<TSettingsStage, LeitnerQueue<TSettingsStage>>();

            LevelQueue.Init(UserData);

            for (var i = 0; i < Settings.LevelArray.Length; i++) LevelQueue.AddQueue(i, Settings.LevelArray[i].StageArray);

            LevelQueue.ResetQueueList();

            LevelQueue.QueueIsFinishedFirstTime += LevelIsFinished;
            LevelQueue.QueueIsPassed += LevelIsPassed;

            if (!CoreContext.PlayParticularGame) CurrentLevelId = UserData.CurrentProfile.CurrentLevelId;

            LevelQueue.MoveToQueue(CurrentLevelId);

            MoveNextStage();

            CurrentLevelIsPassed = false;
        }

        public bool MoveNextStage(bool isPreviosFound = true)
        {
            CurrentStage = LevelQueue.GetNextItem(CurrentLevelId, isPreviosFound);

            if (CurrentStage == null) return false;
			
            CurrentStageId = LevelQueue.CurrentQueue.CurrentItemIndex;

            CurrentLevel = Settings.LevelArray[CurrentLevelId];
			
            CurrentLevelAttemptNum = LevelQueue.CurrentQueue.AttemptNum;

            CurrentLevelTagList = new List<SubGameLevelTagUserData>();

            foreach (var key in CurrentLevel.TagSequence.SplitSequence(true))
            {
                var tag = CoreContext.UserData.CurrentProfile.SubGameLevelTagArray.FirstOrDefault(x => x.Key.ToUpper().Equals(key.ToUpper()));
                if (tag == null) continue; //throw new Exception("TagUserData is not found: " + key);

                CurrentLevelTagList.Add(tag);
            }

            if (PreviousLevelId != CurrentLevelId)
            {
                AddSpentTimeData();
                PreviousLevelId = CurrentLevelId;
            }


            OnNextStageIsMoved();

            return true;
        }

        public override void  ForceMoveNextLevel()
        {
            if (CurrentLevelId == CurrentLevel.StageArray.Length - 1) return;
            
            CurrentLevelId++;

            StartLevel();

        }

        public override void ForceMovePrevLevel()
        {
            if (CurrentLevelId < 1) return;

            CurrentLevelId--;
            
            StartLevel();
        }

        private void LevelIsPassed(object sender, EventArgs e)
        {
            CurrentLevelIsPassed = SubGameBaseInfo.LevelList[CurrentLevelId].IsPassed = true;

            CurrentLevelId++;

            LevelIsFinished(sender, e);
        }

        private void LevelIsFinished(object sender, EventArgs e)
        {
            foreach (var tag in CurrentLevelTagList)
            {
                tag.SpentTimeArray[tag.SpentTimeArray.Length - 1].To = DateTime.UtcNow;
            }
            
            LevelQueue.ResetQueueList();
            
            StartLevel();
        }
        
        private void AddSpentTimeData()
        {
            foreach (var tag in CurrentLevelTagList)
            {
                tag.SpentTimeArray = tag.SpentTimeArray.Add(new SpentTimeItem { From = DateTime.UtcNow });
            } 
        }


        protected virtual void OnNextStageIsMoved() { }

    }
}