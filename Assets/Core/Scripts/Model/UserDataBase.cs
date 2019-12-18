using System.Xml.Serialization;

namespace UniKid.Core.Model
{
    public abstract class UserDataBase
    {
        public event System.EventHandler SaveAttempted;
        public event System.EventHandler Loaded;
        
        public abstract void Save();

		[XmlIgnore]
        public abstract Profile CurrentProfile { get; set; }

		
		[XmlElement("common")]
		public UserDataCommonSnapshot Common;
    }
}