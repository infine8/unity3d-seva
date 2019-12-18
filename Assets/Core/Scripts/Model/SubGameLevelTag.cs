using System.Xml.Serialization;

namespace UniKid.Core.Model
{
    [XmlType("tag")]
    public sealed class SubGameLevelTag
    {
		[XmlAttribute("key")] public string Key;

        [XmlText] public string Name;
    }
}