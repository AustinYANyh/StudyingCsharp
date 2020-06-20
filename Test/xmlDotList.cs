    [XmlRoot("List")]
    public class List
    {

        [XmlElement("DutyCycleCurve")]
        public List<DutyCycleDotStruct> DutyCycleCurve { get; set; }

        [XmlElement("FrequencyCurve")]
        public List<DutyCycleDotStruct> FrequencyCurve { get; set; }
        public List()
        {
            DutyCycleCurve = new List<DutyCycleDotStruct>();
            DutyCycleDotStruct ds = new DutyCycleDotStruct();
            DutyCycleCurve.Add(ds);
        }
    }

    public class DutyCycleDotStruct
    {
        public List<DutyCycleDot> DutyCycleDotList { get; set; }
        public DutyCycleDotStruct()
        {
            DutyCycleDotList = new List<DutyCycleDot>();
            DutyCycleDotList.Add(new DutyCycleDot());
            DutyCycleDotList.Add(new DutyCycleDot() { Speed = "30", Ratip = "30" });
            DutyCycleDotList.Add(new DutyCycleDot() { Speed = "50", Ratip = "50" });
        }
    }
    public class DutyCycleDot
    {
        [XmlElement("Speed")]
        public string Speed { get; set; }

        [XmlElement("Ratip")]
        public string Ratip { get; set; }

        public DutyCycleDot()
        {
            Speed = "10";
            Ratip = "10";
        }
    }

    public class FreDot
    {

    }