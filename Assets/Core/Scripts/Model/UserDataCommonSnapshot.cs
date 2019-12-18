using System.Xml.Serialization;
using UniKid.Core.Model.Audio;

namespace UniKid.Core.Model
{
    [XmlType("common")]
    public sealed class UserDataCommonSnapshot
    {
        [XmlElement("audio-controller")]
        public Audio.AudioControllerSnapshot AudioControllerSnapshot;

        [XmlAttribute("last-used-profile-name")]
		public string LastUsedProfileName;

        [XmlElement("user")]
		public User User;

        public UserDataCommonSnapshot()
        {
            User = new User();
            AudioControllerSnapshot = new AudioControllerSnapshot();
        }
    }
}