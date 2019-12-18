using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.BubbleGuessing.Model
{
    public sealed class BubbleGuessingSettings : SubGameSettingsBase<BubbleGuessingLevel, BubbleGuessingStage>
    {

    }

    [XmlType("level")]
    public sealed class BubbleGuessingLevel : Level<BubbleGuessingStage>
    {
        [XmlElement("spawn-settings")]
        public SpawnSettingsElement SpawnSettings;

        [XmlType("spawn-settings")]
        public sealed class SpawnSettingsElement
        {
            [XmlElement("velocity-range")]
            public VelocityRangeElement VelocityRange;

            [XmlElement("force-range")]
            public ForceRangeElement ForceRange;

            [XmlElement("spawn-period-range")]
            public SpawnPeriodRangeElement SpawnPeriodRange;

            [XmlElement("max-char-on-screen")]
            public MaxCharOnScreenRangeElement MaxCharOnScreenRange;

            [XmlType("velocity-range")]
            public sealed class VelocityRangeElement
            {
                [XmlAttribute("from")]
                public float From;
                [XmlAttribute("to")]
                public float To;
            }

            [XmlType("force-range")]
            public sealed class ForceRangeElement
            {
                [XmlAttribute("xFrom")]
                public float XFrom;
                [XmlAttribute("xTo")]
                public float XTo;
                [XmlAttribute("yFrom")]
                public float YFrom;
                [XmlAttribute("yTo")]
                public float YTo;
            }

            [XmlType("spawn-period-range")]
            public sealed class SpawnPeriodRangeElement
            {
                [XmlAttribute("from")]
                public int From;
                [XmlAttribute("to")]
                public int To;
            }

            [XmlType("max-char-on-screen")]
            public sealed class MaxCharOnScreenRangeElement
            {
                [XmlAttribute("from")]
                public int From;
                [XmlAttribute("to")]
                public int To;
            }

        }

    }

    [XmlType("stage")]
    public sealed class BubbleGuessingStage : Stage
    {
        [XmlAttribute("max-error-count")]
        public int MaxErrorCount;
        [XmlAttribute("max-missed-char-count")]
        public int MaxMissedCharCount;
        [XmlAttribute("max-missclick-count")]
        public int MaxMissclickCount;
        [XmlAttribute("no-help")]
        public bool NoHelp;
        [XmlAttribute("lang")]
        public string Lang;
        [XmlAttribute("char-library-name-sequence")]
        public string CharLibraryNameSequence;
        [XmlAttribute("char-sequence")]
        public string CharSequence;
        [XmlElement("exp")]
        public ExpressionElement Expression;
		
		
        [XmlArray("char-library-list")]
		public CharLibrary[] CharLibraryArray;

        [XmlType("char-library")]
        public sealed class CharLibrary
        {
            [XmlAttribute("name")]
            public string Name;
        }

        [XmlType("exp")]
        public sealed class ExpressionElement
        {
            [XmlText]
            public string Text;
            [XmlAttribute("template")]
            public string Template;
        }
    }


}