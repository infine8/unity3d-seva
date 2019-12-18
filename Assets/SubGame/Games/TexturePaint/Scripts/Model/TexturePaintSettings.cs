using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.TexturePaintGame.Model
{
    public sealed class TexturePaintSettings : SubGameSettingsBase<TexturePaintLevel, TexturePaintStage>
    {

    }


    [XmlType("level")]
    public class TexturePaintLevel : Level<TexturePaintStage>
    {
        
    }

    [XmlType("stage")]
    public class TexturePaintStage : Stage
    {
        [XmlAttribute("paint-char-name")]
        public string PaintCharName { get; set; }
    }
}