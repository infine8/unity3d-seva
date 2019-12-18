using System.Xml.Serialization;

namespace UniKid.Core.Model
{
    [XmlType("user")]
    public sealed class User : UserDataSectionBase
    {
        [XmlAttribute("login")]
		public string Login;

        [XmlAttribute("facebook-id")]
		public string FacebookId;
    }
}