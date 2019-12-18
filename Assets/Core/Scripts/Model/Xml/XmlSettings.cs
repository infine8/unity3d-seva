using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Model;
using UnityEngine;

namespace UniKid.Core.Model.Xml
{
    [XmlType("settings")]
    public class XmlSettings : SettingsBase
    {
        private static string FILENAME = "settings";
        private static XmlSettings _settings;

        private static List<SubGameBaseInfo> _subGameInfoList;

        public static XmlSettings Create(string settings = null, Dictionary<SubGameName, string> subGameSettingsDict = null)
        {
            if (_settings != null) return _settings;

            if (settings == null)
            {
                if (CoreContext.IS_STANDALONE_BUILD)  FILENAME = FILENAME + ".xml";

                _settings = CoreContext.IS_STANDALONE_BUILD ? XmlResource.LoadFromFile<XmlSettings>(FILENAME) : XmlResource.LoadFromResources<XmlSettings>(FILENAME);

                _settings.SubGameSettingsList = new List<SubGameBase>();

                foreach (var name in Enum.GetValues(typeof(SubGameName)))
                {
                    _settings.SubGameSettingsList.Add(
                        CoreContext.IS_STANDALONE_BUILD ? XmlResource.LoadFromFile<SubGameBase>(name + "/" + FILENAME) : 
                        XmlResource.LoadFromResources<SubGameBase>(name + "/" + FILENAME)
                        );
                }

                LoadSubGameBaseInfoList();

                return _settings;
            }

            using (var sr = new StringReader(settings))
            {
                var xmlSerializer = new XmlSerializer(typeof(XmlSettings));
                _settings = xmlSerializer.Deserialize(sr) as XmlSettings;
            }


            return _settings;
        }

        private static void LoadSubGameBaseInfoList()
        {
            _subGameInfoList = new List<SubGameBaseInfo>();

            foreach (var name in Enum.GetValues(typeof(SubGameName)))
            {
                var doc = CoreContext.IS_STANDALONE_BUILD ? XmlResource.LoadFromFile<XmlDocument>(name + "/" + FILENAME)
                    : XmlResource.LoadFromResources<XmlDocument>(name + "/" + FILENAME);

                var gameNode = doc.SelectSingleNode("sub-game");

                if (gameNode == null) throw new Exception("There are no game!");
                
                if (gameNode == null || gameNode.Attributes == null) continue;

                var gameInfo = new SubGameBaseInfo { Name = (SubGameName)Enum.Parse(typeof(SubGameName), gameNode.Attributes["name"].Value) };

                var isEnabledAtt = gameNode.Attributes["is-enabled"];
                gameInfo.IsEnabled = isEnabledAtt == null || bool.Parse(isEnabledAtt.Value);
                gameInfo.Category = (SubGameCategory)Enum.Parse(typeof(SubGameCategory), gameNode.Attributes["category"].Value);

                var levelList = gameNode.SelectNodes("level-list/level");
                if (levelList == null) continue;

                for (var i = 0; i < levelList.Count; i++)
                {
                    var levelNode = levelList[i];

                    if (levelNode == null || levelNode.Attributes == null) continue;

                    var level = new LevelBaseInfo { Id = i, SubGameName = gameInfo.Name };

                    var nameAtt = levelNode.Attributes["name"];
                    var tagsAtt = levelNode.Attributes["tag-sequence"];

                    if (nameAtt != null) level.Name = nameAtt.Value;
                    if (tagsAtt != null) level.TagSequence = tagsAtt.Value;

                    var stageList = levelNode.SelectNodes("stage-list/stage");
                    if (stageList == null) continue;

                    foreach (XmlNode stageNode in stageList)
                    {
                        if (stageNode == null) continue;
                        level.StageArray = level.StageArray.Add(new Stage());
                    }

                    gameInfo.LevelList.Add(level);
                }

                _subGameInfoList.Add(gameInfo);

            }

        }
        

        [XmlIgnore]
        public override List<SubGameBaseInfo> SubGameBaseInfoList { get { return _subGameInfoList; } }


    }
}