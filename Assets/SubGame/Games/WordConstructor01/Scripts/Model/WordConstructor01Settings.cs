using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.WordConstructor01.Model
{
    public sealed class WordConstructor01Settings : SubGameSettingsBase<WordConstructor01Level, WordConstructor01Stage>
    {

    }

    [XmlType("level")]
    public sealed class WordConstructor01Level : Level<WordConstructor01Stage>
    {
        [XmlAttribute("lang-library-name-sequence")]
        public string LangLibraryNameSequence;

        [XmlAttribute("lang-outline-library-name-sequence")]
        public string LangOutlineLibraryNameSequence;
    }

    [XmlType("stage")]
    public sealed class WordConstructor01Stage : Stage
    {
        [XmlAttribute("showed-word")]
        public string ShowedWord;

        [XmlText]
        public string GuessingWord;
    }
}