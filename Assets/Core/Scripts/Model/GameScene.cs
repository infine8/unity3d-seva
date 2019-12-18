using System.Xml.Serialization;

namespace UniKid.Core.Model
{
    [XmlType("game-scene-list")]
    public class GameSceneArray
    {
        [XmlAttribute("start-scene-name")] public string StartSceneName;

        [XmlElement("scene")] public GameScene[] GamesScenes;
    }

    public class GameScene
    {
        [XmlAttribute("name")]
        public string Name;
    }
}
