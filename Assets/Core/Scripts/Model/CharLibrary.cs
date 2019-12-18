using System.Xml.Serialization;

namespace UniKid.Core.Model
{
    [XmlType("char-library")]
    public sealed class CharLibrary
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("sequence")]
        public string Sequence;
    }
}