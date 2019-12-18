using System.Xml.Serialization;
using UniKid.SubGame.Controller;
using UniKid.SubGame.Games.BubbleGuessing.Model;
using UniKid.SubGame.Games.HideSeekCards.Model;
using UniKid.SubGame.Games.LibrarySlider.Model;
using UniKid.SubGame.Games.Sandbox01.Model;
using UniKid.SubGame.Games.SeaGuessing.Model;
using UniKid.SubGame.Games.TestSubGame.Model;
using UniKid.SubGame.Games.TexturePaintGame.Model;
using UniKid.SubGame.Games.WordConstructor01.Model;
using UniKid.SubGame.Games.WordConstructor02.Model;

namespace UniKid.SubGame.Model
{
    [XmlType("sub-game")]
    [XmlInclude(typeof(TestSubGameSettings)), XmlInclude(typeof(TestSubGameUserData))]
    [XmlInclude(typeof(SeaGuessingSettings)), XmlInclude(typeof(SeaGuessingUserData))]
    [XmlInclude(typeof(BubbleGuessingSettings)), XmlInclude(typeof(BubbleGuessingUserData))]
    [XmlInclude(typeof(TexturePaintSettings)), XmlInclude(typeof(TexturePaintUserData))]
    [XmlInclude(typeof(HideSeekCardsSettings)), XmlInclude(typeof(HideSeekCardsUserData))]
    [XmlInclude(typeof(Sandbox01Settings)), XmlInclude(typeof(Sandbox01UserData))]
    [XmlInclude(typeof(LibrarySliderSettings)), XmlInclude(typeof(LibrarySliderUserData))]
    [XmlInclude(typeof(WordConstructor01Settings)), XmlInclude(typeof(WordConstructor01UserData))]
    [XmlInclude(typeof(WordConstructor02Settings)), XmlInclude(typeof(WordConstructor02UserData))]
    public class SubGameBase
    {
        [XmlAttribute("name")]
		public SubGameName Name;
    }
}
