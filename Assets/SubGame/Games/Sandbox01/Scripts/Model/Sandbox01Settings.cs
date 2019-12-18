using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.Sandbox01.Model
{
    public sealed class Sandbox01Settings : SubGameSettingsBase<Sandbox01Level, Sandbox01Stage>
    {

    }

    [XmlType("level")]
    public sealed class Sandbox01Level : Level<Sandbox01Stage>
    {

    }

    [XmlType("stage")]
    public sealed class Sandbox01Stage : Stage
    {
        [XmlAttribute("thing-name-sequence")]
        public string ThingNameSequence { get; set; }
    }
}