using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.WordConstructor02.Model
{
    public sealed class WordConstructor02Settings : SubGameSettingsBase<WordConstructor02Level, WordConstructor02Stage>
    {

    }

    [XmlType("level")]
    public sealed class WordConstructor02Level : Level<WordConstructor02Stage>
    {
        [XmlAttribute("picture-library-name-sequence")]
        public string PictureLibraryNameSequence;
    }

    [XmlType("stage")]
    public sealed class WordConstructor02Stage : Stage
    {
        [XmlAttribute("char-name")]
        public string CharName;

        [XmlAttribute("syllable-sequence")]
        public string SyllableSequence;

        [XmlAttribute("possible-syllable-sequence")]
        public string PossibleSyllableSequence;
    }
}