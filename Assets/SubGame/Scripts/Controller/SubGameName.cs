using System.Xml.Serialization;

namespace UniKid.SubGame.Controller
{
    public enum SubGameName
    {
        [XmlEnum]
        TestSubGame,
        SeaGuessing,
        TexturePaint,
        HideSeekCards,
        BubbleGuessing,
        Sandbox01,
        LibrarySlider,
        WordConstructor01,
        WordConstructor02
    }
}
