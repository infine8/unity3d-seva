using System;
using System.Xml.Serialization;

namespace UniKid.SubGame.Model
{
    [XmlType("spent-time")]
    public class SpentTimeItem
    {
		[XmlAttribute("from")] public DateTime From;
		[XmlAttribute("to")] public DateTime To;
    }
}