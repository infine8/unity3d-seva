using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.TestSubGame.Model
{
    public sealed class TestSubGameSettings : SubGameSettingsBase<Level<Stage>, Stage>
    {


    }

    public abstract class TestSubGameSettingsBase : SubGameSettingsBase
    {
        protected TestSubGameSettingsBase() : base()
        {
            
        }
    }

    [XmlType("level")]
    public sealed class TestSubGameLevel : Level<TestSubGamegStage>
    {

    }

    [XmlType("stage")]
    public sealed class TestSubGamegStage : Stage
    {
    }
}