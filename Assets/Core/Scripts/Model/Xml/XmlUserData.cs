using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;
using UnityEngine;
using System.Linq;

namespace UniKid.Core.Model.Xml
{
    [XmlType("userdata")]
    public sealed class XmlUserData : UserDataBase
    {
        public event EventHandler SaveAttempted = null;
        public event EventHandler Loaded = null;

        public const string ENCRYPTION_CHECK_STRING = "Bad boys!";

        private const string FILENAME = "userdata.xml";
        
        private static List<SubGameBaseInfo> _subGameInfoList;
        
        private static XmlUserData _userData = null;

        public static XmlUserData Create()
        {

#if !UNITY_WEBPLAYER || UNITY_EDITOR
            _userData = XmlResource.LoadSavingFromFile<XmlUserData>(FILENAME, CoreContext.USE_ENCRYPTION);
#endif
            if (_userData == null)
            {
                _userData = new XmlUserData();
                _userData.Common = new UserDataCommonSnapshot();
                _userData.ProfileDataArray = new Profile[0];
            }


            if (_userData.Loaded != null) _userData.Loaded(_userData, null);
            
            _userData.CurrentProfile = _userData.ProfileDataArray.FirstOrDefault(x => x.Name == _userData.Common.LastUsedProfileName);
            if (_userData.CurrentProfile == null)
            {
                _userData.CurrentProfile = new Profile { Name = "TestProfile", IsSubGamePriorityEnabled = true };
                _userData.ProfileDataArray = _userData.ProfileDataArray.Add(_userData.CurrentProfile);
                _userData.Common.LastUsedProfileName = _userData.CurrentProfile.Name;
            }

            //foreach (var s in _userData.CurrentProfile.SubGameDataList) s.SubGameName = (SubGameName)Enum.Parse(typeof(SubGameName), s.Name);

            for (var i = 0; i < CoreContext.Settings.SubGameLevelTagArray.Length; i++)
            {
                var tagSettings = CoreContext.Settings.SubGameLevelTagArray[i];

                var tagUserData = _userData.CurrentProfile.SubGameLevelTagArray.FirstOrDefault(x => x.Key == tagSettings.Key);

                if (tagUserData != null) continue;
 

                var tagPriority = _userData.CurrentProfile.SubGameLevelTagArray.FirstOrDefault(x => x.Priority == i);

                _userData.CurrentProfile.SubGameLevelTagArray = 
                    _userData.CurrentProfile.SubGameLevelTagArray.Add(new SubGameLevelTagUserData
                                                                        {
                                                                            IsEnabled = true,
                                                                            Name = tagSettings.Name,
                                                                            Key = tagSettings.Key,
                                                                            Priority = tagPriority == null ? i : _userData.CurrentProfile.SubGameLevelTagArray.Length
                                                                        });

            }


            return _userData;
        }

        public static void UpdateSubGameBaseInfoList(List<SubGameBaseInfo> subGameBaseInfoList)
        {

            if (subGameBaseInfoList == null || subGameBaseInfoList.Count < 1) return;

#if UNITY_WEBPLAYER && !UNITY_EDITOR
            return;
#endif
            var doc = XmlResource.LoadSavingFromFile<XmlDocument>(FILENAME, CoreContext.USE_ENCRYPTION);

            if (doc == null) return;

            var subGameNodeList = doc.SelectNodes(string.Format("userdata/profile-data/profile['{0}']/sub-game-data/sub-game", _userData.CurrentProfile.Name));

            if (subGameNodeList == null) return;

            foreach (XmlNode subGameNode in subGameNodeList)
            {
                if (subGameNode == null || subGameNode.Attributes == null) continue;

                var gameName = (SubGameName)Enum.Parse(typeof(SubGameName), subGameNode.Attributes["name"].Value);
                var gameInfo = subGameBaseInfoList.FirstOrDefault(x => x.Name == gameName);

                if (gameInfo == null) continue;// throw new Exception("Game settings is not found: " + gameName);

                var levelNodeList = subGameNode.SelectNodes("queue-data/queue-info");

                if (levelNodeList == null) continue;

                foreach(XmlNode levelNode in levelNodeList)
                {
                    if (levelNode == null || levelNode.Attributes == null) continue;

                    var levelId = int.Parse(levelNode.Attributes["id"].Value);
                    var isPassed = bool.Parse(levelNode.Attributes["is-passed"].Value);
                    var attemptNum = int.Parse(levelNode.Attributes["attempt-num"].Value);

                    gameInfo.LevelList[levelId].AttemptNum = attemptNum;
                    gameInfo.LevelList[levelId].IsPassed = isPassed;
                }
            }
        }

        public override void Save()
        {
            if (SaveAttempted != null) SaveAttempted(this, null);

            XmlResource.SaveToFile(FILENAME, this, CoreContext.USE_ENCRYPTION);
        }



        [XmlArray("profile-data")] 
		public Profile[] ProfileDataArray;


        [XmlIgnore] public override Profile CurrentProfile { get; set; }
    }
}
