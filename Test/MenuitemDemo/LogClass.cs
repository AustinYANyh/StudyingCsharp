using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogClassHelper
{
    [XmlRoot("Menu")]
    public class Menu
    {
        [XmlElement("MenuItem")]
        public List<Year> Year { get; set; }

        public Menu()
        {
            Year = new List<Year>();
        }
    }

    public class Year
    {
        [XmlElement("MenuItem")]
        public List<Date> date { get; set; }

        [XmlAttribute("Data")]
        public string data { get; set; }
        public Year()
        {
            date = new List<Date>();
            data = string.Empty;
        }
    }
    public class Date
    {

        [XmlElement("MenuItem")]
        public List<Time> time { get; set; }

        [XmlAttribute("Data")]
        public string data { get; set; }
        public Date()
        {
            data = string.Empty;
            time = new List<Time>();
        }
    }

    public class Time
    {
        [XmlAttribute("Data")]
        public string data { get; set; }

        public Time()
        {
            data = string.Empty;
        }
    }
}
