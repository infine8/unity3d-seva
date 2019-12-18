using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.LibrarySlider.Model
{
    public sealed class LibrarySliderSettings : SubGameSettingsBase<LibrarySliderLevel, LibrarySliderStage>
    {

    }

    [XmlType("level")]
    public sealed class LibrarySliderLevel : Level<LibrarySliderStage>
    {
        [XmlAttribute("picture-library-name-sequence")] 
        public string PictureLibraryNameSequence;

        [XmlAttribute("lang-library-name-sequence")]
        public string LangLibraryNameSequence;
    }

    [XmlType("stage")]
    public sealed class LibrarySliderStage : Stage
    {
		[XmlArray("pic-list")]
		public Picture[] PictureArray;
        
        [XmlType("pic")]
        public class Picture
        {
            [XmlAttribute("char-name")]
            public string CharName;

            [XmlText]
            public string DisplayName;
        }
    }
}