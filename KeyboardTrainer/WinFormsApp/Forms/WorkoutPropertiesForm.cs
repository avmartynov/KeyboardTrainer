using System;
using System.Windows.Forms;
using Twidlle.Infrastructure;
using Twidlle.KeyboardTrainer.Core;
using Twidlle.KeyboardTrainer.WinFormsApp.Properties;

namespace Twidlle.KeyboardTrainer.WinFormsApp.Forms
{
    public partial class WorkoutPropertiesForm : Form
    {
        private readonly Workout _document;

        public WorkoutPropertiesForm(Workout document)
        {
            InitializeComponent();

            _document = document;

            this.bestCharPerMinuteLabel.Text    = document.WorkoutState.BestCharPerMinute.ToString();
            this.bestErrorsLabel.Text           = (document.WorkoutState.BestErrorCount / 100).ToString("f2");

            this.averageCharPerMinuteLabel.Text = document.WorkoutState.AverageCharPerMinute.ToString("f2");
            this.averageErrorsLabel.Text        = (document.WorkoutState.AverageErrorCount / 100).ToString("f2");

            this.lastCharPerMinuteLabel.Text    = document.WorkoutState.LastCharPerMinute.ToString();
            this.lastErrorsLabel.Text           = (document.WorkoutState.LastErrorCount /100).ToString("f2");

            this.exerciseCountLabel.Text        = document.WorkoutState.ExerciseCount.ToString();
            this.workoutTypeLabel.Text          = document.WorkoutType.Name;
            this.languageLabel.Text             = document.LocalLanguage.Name;
        }


        private void copyButton_Click(object sender, EventArgs e)
        {
            var s = Resources.ResultFormat.Format(DateTime.Now, 
                _document.WorkoutType.Name,                  _document.LocalLanguage.Name,
                _document.WorkoutState.ExerciseCount, 
                _document.WorkoutState.BestCharPerMinute,    _document.WorkoutState.BestErrorCount,
                _document.WorkoutState.AverageCharPerMinute, _document.WorkoutState.AverageErrorCount,
                _document.WorkoutState.LastCharPerMinute,    _document.WorkoutState.LastErrorCount);

            Clipboard.SetText(s);
        }
    }
}
