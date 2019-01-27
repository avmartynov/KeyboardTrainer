using System;
using System.Xml.Serialization;

namespace Twidlle.KeyboardTrainer.Core
{
    [XmlRoot("Appearance")]
    public class ExerciseAppearance
    {
        public Int32  FontSize { get; set; }

        public String FontName { get; set; }

        public String TextColor { get; set; }

        public String BackgrColor { get; set; }

        public String LocalTextColor { get; set; }

        public String HeadColor { get; set; }

        public String CurrentCharColor { get; set; }

        public String IncorrectCharColor { get; set; }

        public String TailColor { get; set; }

        public String NewExerciseText { get; set; }

        public String SpaceCharacterText { get; set; }

        public String UpperPrefixText { get; set; }
    }
}
