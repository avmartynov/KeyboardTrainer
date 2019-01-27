using System.Windows.Forms;

namespace Twidlle.KeyboardTrainer.WinFormsApp.ExerciseDrawing
{
    public class ExercisePanel : Panel
    {
        public ExercisePanel()
        {
            this.SetStyle(ControlStyles.UserPaint,             true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint,  true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw,          true);
            this.UpdateStyles();            
        }
    }
}
