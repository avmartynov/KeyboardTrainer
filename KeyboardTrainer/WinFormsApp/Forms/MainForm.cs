using System;
using System.IO;
using System.Windows.Forms;
using Twidlle.Infrastructure;
using Twidlle.Infrastructure.WinForms;
using Twidlle.KeyboardTrainer.WinFormsApp.Properties;
using Twidlle.KeyboardTrainer.Core;
using Twidlle.KeyboardTrainer.WinFormsApp.ExerciseDrawing;

namespace Twidlle.KeyboardTrainer.WinFormsApp.Forms
{
    public partial class MainForm : Form, IWorkoutView
    {
        public MainForm(String workoutFilePath = null)
        {
            _startupWorkoutFilePath = workoutFilePath;

            FormRestoreManager.Initialize(this, Settings.Default, s => s.MainForm);

            InitializeComponent();
        }


        /// <summary> IView.WorkoutChanged </summary>
        public void WorkoutChanged()
        {
            lastResultToolStripStatusLabel.Text    = _workout.WorkoutState.LastCharPerMinute.ToString();
            bestResultToolStripStatusLabel.Text    = _workout.WorkoutState.BestCharPerMinute.ToString();
            exerciseCountToolStripStatusLabel.Text = (_workout.WorkoutState.ExerciseCount + 1).ToString();

            if (Settings.Default.Option_Voice)
                SpeakNewExercise();

            ExerciseChanged();
        }


        /// <summary> IView.ExerciseChanged </summary>
        public void ExerciseChanged()
        {
            Text = _workout.FormatStateString(runningStatusString: Resources.RunningState,
                                                 fileNonameString: Resources.NoNameFileName,
                                                 applicationTitle: ApplicationInfo.ProductName);
            this._testPanel.Invalidate();

            if (Settings.Default.Option_Voice)
                SpeakNextChar();
        }


        /// <summary> IView.WorkoutSaved </summary>
        public void WorkoutSaved()
        {
            ExerciseChanged();
        }

        //////////////////////////////////////////////////////

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(_startupWorkoutFilePath))
                    LoadFile(_startupWorkoutFilePath);
                else
                {
                    if (Settings.Default.Option_OpenLastFile)
                        LoadFile(Settings.Default.LastFile);
                    else
                        NewFile();
                }
            }
            catch (Exception x)
            {
                Program.ShowExceptionDialog(this, x);
                NewFile();
            }
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_workout.Changed)
            {
                if (!AskAndSaveFile())
                    e.Cancel = true;
            }
        }


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Save();
        }


        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            _workout.OnCharPressed(e.KeyChar);
        }


        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = _workout.OnKeyPressed(e.KeyCode);
        }


        private void testStringPanel_Paint(object sender, PaintEventArgs e)
        {
            _painter.Draw(e.Graphics, _testPanel.ClientRectangle, _workout.Exercise);
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_workout.Changed)
                {
                    if (! AskAndSaveFile())
                        return;
                }

                NewFile();
            }
            catch (Exception x)
            {
                Program.ShowExceptionDialog(this, x);
            }
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workout.Changed)
            {
                if (! AskAndSaveFile())
                    return;
            }

            AskPathAndLoadFile();
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_workout.IsFilePathSpecified)
                _workout.Save();
            else
            {
                var dir = String.IsNullOrEmpty(Settings.Default.LastFile) 
                            ? null 
                            : Path.GetDirectoryName(Settings.Default.LastFile);
                dir = dir ?? DefaultFileDirectory;

                var workoutFilePath = Path.Combine(dir, DefaultFileName);

                AskPathAndSaveFile(workoutFilePath);
            }
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskPathAndSaveFile(Settings.Default.LastFile);
        }


        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new WorkoutPropertiesForm(_workout).ShowDialog(this);
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void newExerciseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _workout.ResetExercise();
        }


        private void resetWorkoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _workout.ResetWorkout();
        }


        private void workoutTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new WorkoutTypeForm(Settings.Default.Option_WorkoutType, AppConfiguration.WorkoutTypes);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

            Settings.Default.Option_WorkoutType = dlg.WorkoutTypeCode;
        }


        private void localLanguageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var dlg = new LocalLanguageForm(Settings.Default.Option_LocalLanguage, AppConfiguration.LocalLanguages);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

            Settings.Default.Option_LocalLanguage = dlg.LocalLanguageCode;
        }


        private void preferencesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var dlg = new OptionsForm(Settings.Default.Option_OpenLastFile, Settings.Default.Option_Voice);
            if (dlg.ShowDialog(this) != DialogResult.OK)
                    return;

            Settings.Default.Option_OpenLastFile = dlg.OpenLastFile;
            Settings.Default.Option_Voice        = dlg.VoiceEnable;
        }


        private void homePageToolStripMenuItem_Click(object sender, EventArgs e)
            => new Uri(Resources.HelpUrl).NavigateTo();


        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
            => new AboutForm(Resources.AppIcon).ShowDialog(this);


        //////////////////////////////////////////////////////

        private void NewFile()
        {
            Settings.Default.LastFile = null;
            _workout = new Workout(this,
                                    Settings.Default.Option_LocalLanguage,
                                    Settings.Default.Option_WorkoutType);
            WorkoutChanged();
        }


        private void LoadFile(String filePath)
        {
            Settings.Default.LastFile = filePath;
            _workout = Workout.Load(this, filePath);
            WorkoutChanged();
        }


        private void SaveFile(String filePath)
        {
            Settings.Default.LastFile = filePath;
            _workout.Save(filePath);
        }


        private Boolean AskAndSaveFile()
        {
            var result = MessageBox.Show(owner: this,
                                          text: Resources.AskSaveFile,
                                       caption: "",
                                       buttons: MessageBoxButtons.YesNoCancel,
                                          icon: MessageBoxIcon.Question);

            if (result == DialogResult.Cancel)
                return false;

            if (result == DialogResult.Yes)
            {
                if (_workout.IsFilePathSpecified)
                    _workout.Save();
                else
                    AskPathAndSaveFile(DefaultFileName);
            }
            return true;
        }


        private void AskPathAndSaveFile(String path)
        {
            var dlg = new SaveFileDialog
                      {
                                    Filter = Resources.WorkoutFileFilter,
                               FilterIndex = 1,
                              AddExtension = true,
                           CheckPathExists = true,
                                DefaultExt = Workout.FileExtention,
                          InitialDirectory = Path.GetDirectoryName(path),
                                     Title = Resources.SaveWorkoutFile,
                                  FileName = Path.GetFileName(path),
                      };
            if (dlg.ShowDialog() == DialogResult.OK)
                SaveFile(dlg.FileName);
        }


        private void AskPathAndLoadFile()
        {
            var dlg = new OpenFileDialog
                      {
                                    Filter = Resources.WorkoutFileFilter,
                               FilterIndex = 1,
                              AddExtension = true,
                           CheckPathExists = true,
                           CheckFileExists = true,
                                DefaultExt = Workout.FileExtention,
                          InitialDirectory = Path.GetDirectoryName(Settings.Default.LastFile),
                                     Title = Resources.OpenWorkoutFile,
                                  FileName = Path.GetFileName(Settings.Default.LastFile),
                      };

            if (dlg.ShowDialog() == DialogResult.OK)
                LoadFile(dlg.FileName);
        }


        private static String DefaultFileName => DateTime.Now.ToString("yyyy-MM-dd_HHmm.") + Workout.FileExtention;


        private static String DefaultFileDirectory
        {
            get
            {
                var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                // C:\Users\<user>\Documents\Keyboard Workouts
                var dir = Path.Combine(myDocs, "Keyboard Workouts"); 
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                return dir;
            }
        }


        private void SpeakNewExercise()
        {
            var newExercisePrononcing = _workout.GetNewExercisePronouncing();
            Speaker.Speak(newExercisePrononcing.Language, newExercisePrononcing.Text);            
        }


        private void SpeakNextChar()
        {
            if (_workout.Exercise.WrongTyping)
                Speaker.PlayAsterisk();
            
            var item = _workout.Exercise.CurrentItem;
            if (item == null)
                return;

            // Speaker.SpeakAsync(item.VoiceLanguage, item.VoiceText);
            Speaker.SpeakAsync(_workout.LocalLanguage.Code, item.VoiceText);
        }


        private Workout _workout;
        private readonly String _startupWorkoutFilePath;

        private static readonly ExercisePainter        _painter     = new ExercisePainter(AppConfiguration.Appearance);
    }
}



