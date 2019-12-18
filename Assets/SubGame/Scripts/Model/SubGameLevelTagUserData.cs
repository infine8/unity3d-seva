using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UniKid.Core;
using UniKid.Core.Model;

namespace UniKid.SubGame.Model
{
    [XmlType("level-tag")]
    public sealed class SubGameLevelTagUserData : UserDataSectionBase
    {
        [XmlAttribute("is-enabled")]
		public bool IsEnabled;

        [XmlAttribute("key")]
		public string Key;

        [XmlAttribute("priority")]
		public int Priority;
		
		[XmlArray("spent-time-data")]
		public SpentTimeItem[] SpentTimeArray;
        
        [XmlIgnore]
        public Profile CurrentProfile { get { return CoreContext.UserData.CurrentProfile; } }

        public SubGameLevelTagUserData()
        {
            IsEnabled = true;

            SpentTimeArray = Enumerable.Empty<SpentTimeItem>().ToArray();
        }
    }
}