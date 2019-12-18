using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.HideSeekCards.Model
{
    public sealed class HideSeekCardsSettings : SubGameSettingsBase<HideSeekCardsLevel, HideSeekCardsStage>
    {

    }

    [XmlType("level")]
    public sealed class HideSeekCardsLevel : Level<HideSeekCardsStage>
    {
    }

    [XmlType("stage")]
    public sealed class HideSeekCardsStage : Stage
    {
        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}