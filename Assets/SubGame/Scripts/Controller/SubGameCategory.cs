using System.Xml.Serialization;

namespace UniKid.SubGame.Controller
{
    public enum SubGameCategory
    {
        [XmlEnum]
        School,
        [XmlEnum]
        Sandbox,
        [XmlEnum]
        Library
    }
}