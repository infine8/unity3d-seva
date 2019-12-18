using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.SeaGuessing.Model
{
    public sealed class SeaGuessingSettings : SubGameSettingsBase<SeaGuessinggLevel, SeaGuessingStage>
    {

    }

    [XmlType("level")]
    public sealed class SeaGuessinggLevel : Level<SeaGuessingStage>
    {

    }

    [XmlType("stage")]
    public sealed class SeaGuessingStage : Stage
    {
        [XmlAttribute("char-library-name-sequence")]
        public string CharLibraryNameSequence { get; set; }

        [XmlAttribute("possible-char-sequence")]
        public string PossibleCharSequence { get; set; }

        [XmlText]
        public string CharName;
    }
}