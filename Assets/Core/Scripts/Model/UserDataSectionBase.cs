using System;
using System.Xml.Serialization;

namespace UniKid.Core.Model
{
    public abstract class UserDataSectionBase
    {
        [XmlAttribute("name")]
		public string Name;

        [XmlAttribute("db-id")]
		public string DbId;

        [XmlAttribute("last-sync-date")]
		public DateTime LastSyncDate;

    }
}