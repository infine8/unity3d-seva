using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.Core.Model
{
    [XmlType("profile")]
    public sealed class Profile : UserDataSectionBase
    {
        private List<SubGameLevelTagUserData> _subGameLevelTagUserDataList; 

        [XmlAttribute("is-sub-game-priority-enabled")]
		public bool IsSubGamePriorityEnabled;

        [XmlAttribute("current-sub-game-name")]
		public string CurrentSubGameName;

        [XmlAttribute("current-level-id")]
		public int CurrentLevelId;

        [XmlAttribute("is-game-finished")]
		public bool IsGameFinished;
        
		[XmlArray("sub-game-level-tag-data")]
		public SubGameLevelTagUserData[] SubGameLevelTagArray;
		
		[XmlArray("sub-game-data")]
		public SubGameBase[] SubGameDataArray;

        [XmlIgnore]
        public User User { get { return CoreContext.UserData.Common.User; } }

        public Profile()
        {
            SubGameDataArray = new SubGameBase[0];
            SubGameLevelTagArray = new SubGameLevelTagUserData[0];
        }

    }

}
