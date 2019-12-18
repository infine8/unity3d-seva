using System.Collections.Generic;
using System.Xml.Serialization;

namespace UniKid.SubGame.Model
{
    public abstract class SubGameQueueUserDataBase : SubGameUserDataBase
    {
		[XmlArray("queue-data")] 
		public QueueInfo[] QueueInfoArray;

        protected SubGameQueueUserDataBase()
        {
            QueueInfoArray = new QueueInfo[0];
        }
    }

    [XmlType("queue-info")]
    public class QueueInfo
    {
		[XmlAttribute("id")] public int Id;

		[XmlAttribute("is-passed")] public bool IsPassed;

		[XmlAttribute("attempt-num")] public int AttemptNum;
		
		[XmlArray("queue")] 
		public QueueItem[] Queue;
        
        public QueueInfo()
        {
            Queue = new QueueItem[0];
        }
    }

    [XmlType("item")]
    public class QueueItem
    {
        [XmlAttribute("id")] public int Id;

        [XmlAttribute("is-found")] public bool? IsFound;
        
		[XmlAttribute("attempt-num")] public int AttemptNum;
    }
}

