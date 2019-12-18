using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.Core.Model
{
    public abstract class SettingsBase
    {
        [XmlElement("default-audio-controller-snapshot")]
		public Audio.AudioControllerSnapshot DefaultAudioControllerSnapshot;

        [XmlIgnore]
        public List<SubGameBase> SubGameSettingsList { get; set; }

        [XmlElement("game-scene-list")]
        public GameSceneArray GameSceneArray;

        [XmlArray("char-library-list")]
        public CharLibrary[] CharLibraryArray;

        [XmlArray("sub-game-level-tag-data")]
        public SubGameLevelTag[] SubGameLevelTagArray;

        [XmlIgnore]
        public abstract List<SubGameBaseInfo> SubGameBaseInfoList { get; }
    }

}
