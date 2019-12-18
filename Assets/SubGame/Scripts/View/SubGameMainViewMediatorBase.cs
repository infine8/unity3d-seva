using System;
using System.Collections;
using UniKid.Core;
using UniKid.Core.Controller;
using UniKid.Core.Model;
using UniKid.Core.Service;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;
using UniKid.SubGame.View.Character;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

namespace UniKid.SubGame.View
{
    public abstract class SubGameMainViewMediatorBase<TSettings, TUserData> : SubGameMainViewMediatorBase<TSettings, TUserData, Level<Stage>, Stage>
        where TSettings : SubGameSettingsBase<Level<Stage>, Stage>, new()
        where TUserData : SubGameQueueUserDataBase, new()
    {
        protected abstract SubGameCoreBase<TSettings, TUserData> SubGameCoreSimple { get; }

        protected override SubGameCoreBase<TSettings, TUserData, Level<Stage>, Stage> SubGameCore { get { return SubGameCoreSimple; } }
    }

    public abstract class SubGameMainViewMediatorBase<TSettings, TUserData, TSettingsLevel, TSettingsStage> : EventMediatorBase
        where TSettings : SubGameSettingsBase<TSettingsLevel, TSettingsStage>, new()
        where TUserData : SubGameQueueUserDataBase, new()
        where TSettingsLevel : Level<TSettingsStage>, new()
        where TSettingsStage : Stage, new()
    {
        [Inject]
        public SubGameIsPassedSignal SubGameIsPassedSignal { get; set; }

        [Inject]
        public MoveNextLevelSignal MoveNextLevelSignal { get; set; }

        [Inject]
        public IDbService DbService { get; set; }
        

        [Inject]
        public StagePassedSignal StagePassedSignal { get; set; }

        [Inject(ContextKeys.CROSS_CONTEXT_DISPATCHER)]
        public IEventDispatcher crossContextDispatcher { get; set; }

        
        public abstract SubGameMainViewBase SubGameViewBase { get; }

        protected abstract SubGameCoreBase<TSettings, TUserData, TSettingsLevel, TSettingsStage> SubGameCore { get; }

        public override void OnRegister()
        {
            base.OnRegister();

            SubGameCoreBase.StartLevelAction += OnLevelPassed;
            SubGameCoreBase.GameFinishedAction += GameIsFinishedAction;
            
            StagePassedSignal.AddListener(OnStagePassed);
        }

        public override void OnRemove()
        {
            base.OnRemove();

            SubGameCoreBase.StartLevelAction -= OnLevelPassed;
            SubGameCoreBase.GameFinishedAction -= GameIsFinishedAction;
            
            StagePassedSignal.RemoveListener(OnStagePassed);
        }

        protected override void UpdateListeners(bool toAdd)
        {
            SubGameViewBase.dispatcher.UpdateListener(toAdd, SubGameMainViewBase.SubGameViewEvent.ExitToMainMenu, Exit);
            SubGameViewBase.dispatcher.UpdateListener(toAdd, SubGameMainViewBase.SubGameViewEvent.GameIsPassed, GameIsPassed);
        }

        private void OnStagePassed()
        {
            if (SubGameCore.MoveNextStage()) StartCoroutine(LoadNextStage(true, null));
        }

        private void OnLevelPassed(LevelBaseInfo nextLevelInfo, bool isLastLevelPassed, Action moveNextLevelAct)
        {
            StartCoroutine(LoadNextStage(isLastLevelPassed, moveNextLevelAct));
        }

        protected virtual IEnumerator LoadNextStage(bool isLastLevelPassed, Action moveNextLevelAct)
        {
            yield return new WaitForSeconds(SubGameViewBase.SecondsBeforeLoadNewLevel);

            if (moveNextLevelAct != null) moveNextLevelAct();

            SubGameViewBase.LoadStage();
        }

        protected void Exit()
        {
            dispatcher.Dispatch(SubGameEventType.Exit);
        }

        private void GameIsPassed()
        {
            SubGameIsPassedSignal.Dispatch(SubGameCore.SubGameName);
        }

        private void OnLevelPassed(LevelBaseInfo nextLevelInfo, bool isLastLevelPassed)
        {
            Debug.Log("MoveNextLevel - " + nextLevelInfo.UniqueId + " - " + isLastLevelPassed);

            MoveNextLevelSignal.Dispatch(nextLevelInfo);

            var moveLevelAct = new Action(() =>
            {
                if (nextLevelInfo.SubGameName == SubGameCore.Settings.Name)
                {
                    SubGameCore.MoveNextStage();
                }
                else
                {
                    dispatcher.Dispatch(SubGameEventType.Exit, nextLevelInfo.SubGameName);
                }
            });


            crossContextDispatcher.Dispatch(CoreEventType.DbUpdateUserData);

            OnLevelPassed(nextLevelInfo, isLastLevelPassed, moveLevelAct);
        }

        private void GameIsFinishedAction()
        {
            Debug.Log("GameIsFinishedAction");

            if (!CoreContext.PlayParticularGame) CoreContext.UserData.CurrentProfile.IsGameFinished = true;

            dispatcher.Dispatch(SubGameEventType.Exit);
        }
    }
}