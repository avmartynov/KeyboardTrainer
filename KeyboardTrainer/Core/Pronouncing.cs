using System;
using System.Xml.Serialization;


namespace Twidlle.KeyboardTrainer.Core
{
    public class Pronouncing
    {
        [XmlAttribute("name")]
        public String Name { get; set; }

        [XmlAttribute("lang")]
        public String Language { get; set; }

        [XmlText]    
        public String Text { get; set; }
    }
}
