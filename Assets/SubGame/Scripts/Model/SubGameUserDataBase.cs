using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.Core;
using UniKid.Core.Model;

namespace UniKid.SubGame.Model
{
    public abstract class SubGameUserDataBase : SubGameBase
    {
        [XmlIgnore]
        public Profile CurrentProfile { get { return CoreContext.UserData.CurrentProfile; } }
        
        [XmlAttribute("is-enabled")]
		public bool IsEnabled;

        [XmlAttribute("db-id")]
		public string DbId;

        [XmlAttribute("last-sync-date")]
		public double LastSyncDate;

        protected SubGameUserDataBase()
        {
            IsEnabled = true;
        }
    }

}