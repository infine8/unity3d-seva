using System.Collections.Generic;
using System.Xml.Serialization;
using UniKid.SubGame.Model;

namespace UniKid.SubGame.Games.BubbleGuessing.Model
{
    public sealed class BubbleGuessingUserData : SubGameQueueUserDataBase
    {
        [XmlAttribute("error-count")]
        public int ErrorCount;
        [XmlAttribute("missed-char-count")]
        public int MissedCharCount;
        [XmlAttribute("missclick-count")]
        public int MissclickCount;
        
        public BubbleGuessingUserData()
        {
            
        }

        [XmlType("exp-id")]
        public class FoundExpression
        {
            [XmlText]
            public int ExpressionId;
        }
    }


}