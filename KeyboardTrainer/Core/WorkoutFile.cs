using System;
using System.Xml.Serialization;

namespace Twidlle.KeyboardTrainer.Core
{
    [XmlRoot("Workout")]
    public class WorkoutFile
    {
        public String LocalLanguageCode { get; set; }

        public String WorkoutTypeCode { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime LastPerformedDateTime { get; set; }

        public WorkoutState WorkoutState { get; set; }
    }
}
