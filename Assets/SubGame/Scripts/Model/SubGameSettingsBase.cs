using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.SubGame.Controller;

namespace UniKid.SubGame.Model
{
    public abstract class SubGameSettingsBase : SubGameSettingsBase<Level<Stage>, Stage>
    {
        
    }
    public abstract class SubGameSettingsBase<T, TZ> : SubGameBase where T : Level<TZ>, new() where TZ : Stage, new()
    {
		[XmlArray("level-list")]
		public T[] LevelArray;


        [XmlAttribute("category")] 
        public SubGameCategory Category { get; set; }

        protected SubGameSettingsBase()
        {
            LevelArray = new T[0];
        }
    }

    [XmlType("level")]
    public class Level<T> where T : Stage
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tag-sequence")]
        public string TagSequence { get; set; }

		[XmlArray("stage-list")]
		public T[] StageArray;
        
        
        public Level()
        {
            StageArray = new T[0];
        }
    }
    
    [XmlType("stage")]
    public class Stage
    {

    }
}
