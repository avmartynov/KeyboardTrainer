using System.Xml.Serialization;

namespace Twidlle.KeyboardTrainer.ExeciseStringDrawing
{
    [XmlRoot("Appearance")]
    public class ExerciseStringAppearance
    {
        public int    FontSize           { get; set; }
        public string FontName           { get; set; }

        public string TextColor          { get; set; }
        public string BackgrColor        { get; set; }
        public string LocalTextColor     { get; set; }

        public string HeadColor          { get; set; }
        public string CurrentCharColor   { get; set; }
        public string IncorrectCharColor { get; set; }
        public string TailColor          { get; set; }
    }
}
