using System;
using System.IO;
using System.Windows.Forms;
using Twidlle.Infrastructure;

namespace Twidlle.KeyboardTrainer.Core
{
    public class Workout
    {
        /// <summary> Creating new document. </summary>
        public Workout(IWorkoutView view, 
                       String       localLanguageCode, 
                       String       workoutTypeCode)
            : this(view, 
                   localLanguageCode, 
                   workoutTypeCode, 
                    workoutState: new WorkoutState(), 
                   startDateTime: DateTime.Now, 
                        filePath: null)
        {}


        /// <summary> Loading from file. </summary>
        public static Workout Load(IWorkoutView view, String filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            var workoutFile = XmlFile.Read<WorkoutFile>(filePath);

            return new Workout(view, 
                               workoutFile.LocalLanguageCode,
                               workoutFile.WorkoutTypeCode, 
                               workoutFile.WorkoutState,
                               workoutFile.StartDateTime, 
                               filePath);
        }


        /// <summary> Creating new document. </summary>
        private Workout(IWorkoutView view,
                        String       localLanguageCode,
                        String       workoutTypeCode,
                        WorkoutState workoutState,
                        DateTime     startDateTime,
                        String       filePath)
        {
            _view          = view ?? throw new ArgumentNullException(nameof(view));
            _localLanguage = AppConfiguration.GetLocalLanguage(localLanguageCode ?? throw new ArgumentNullException(nameof(localLanguageCode)) );
            _workoutType   = AppConfiguration.GetWorkoutType(   workoutTypeCode  ?? throw new ArgumentNullException(nameof(workoutTypeCode)));
            _workoutState  = workoutState ?? throw new ArgumentNullException(nameof(workoutState));
            _startDateTime = startDateTime;
            _filePath      = filePath;
            _exercise      = new Exercise(_localLanguage, _workoutType);
        }


        /// <summary> Saving with current file path. </summary>
        public void Save()
        {
            Save(FilePath);
        }


        /// <summary> Saving with new file path. </summary>
        public void Save(String filePath)
        {
            var workoutFile = new WorkoutFile
                              {
                                    LocalLanguageCode     = _localLanguage.Code, 
                                    WorkoutTypeCode       = _workoutType.Code, 
                                    StartDateTime         = _startDateTime,
                                    LastPerformedDateTime = DateTime.Now,
                                    WorkoutState          = _workoutState
                              };
            XmlFile.Write(workoutFile, filePath);

            _filePath = filePath;
            _changed  = false;
            _exercise = new Exercise(_localLanguage, _workoutType);
            _view.WorkoutSaved();
        }


        public DateTime StartDateTime => _startDateTime;

        public Language LocalLanguage => _localLanguage;

        public WorkoutType WorkoutType  => _workoutType;

        public WorkoutState WorkoutState => _workoutState;

        public Exercise Exercise => _exercise;

        public Boolean Changed => _changed;

        public Boolean IsFilePathSpecified => _filePath != null;


        public String FilePath
        {
            get
            {
                if (_filePath == null)
                    throw new InvalidOperationException("File path is not specified.");

                return _filePath;
            }
        }


        public String FormatStateString(String runningStatusString, 
                                        String fileNonameString,
                                        String applicationTitle)
        { 
            var running = _exercise.Running ? "(" + runningStatusString + ")" : "";
            var changed = _changed ? "*" : "";
            var fileName = Path.GetFileNameWithoutExtension(IsFilePathSpecified ? _filePath : fileNonameString);

            return $"{fileName}{changed} {running} - {applicationTitle}";
        }


        public Pronouncing GetNewExercisePronouncing()
        {
            return AppConfiguration.GetPronouncing("NewExercise", LocalLanguage.Code);    
        }


        public void ResetExercise()
        {
            _exercise = new Exercise(_localLanguage, _workoutType);
            _view.ExerciseChanged();
        }


        public void ResetWorkout()
        {
            _workoutState = new WorkoutState();
            _exercise = new Exercise(_localLanguage, _workoutType);
            _changed = true;
            _view.WorkoutChanged();
        }


        public Boolean OnKeyPressed(Keys keyPressed)
        {
            if (!_exercise.KeyPressed(keyPressed))
                return false;

            ProcessExerciseStep();
            return true;
        }


        public void OnCharPressed(Char charPressed)
        {
            _exercise.CharPressed(charPressed);
            ProcessExerciseStep();
        }

        //----------------------

        public static String DefaultFileName => DateTime.Now.ToString("yyyy-MM-dd_HHmm." + FileExtention);


        public static String FileExtention => "ktr";

        //----------------------

        private void ProcessExerciseStep()
        {
            _view.ExerciseChanged();

            int charPerMinute = 0, errorCount = 0;
            if (_exercise.CheckFinished(ref charPerMinute, ref errorCount))
            {
                _workoutState.SetExerciseFinished(charPerMinute, (double)errorCount / _workoutType.ExerciseLength);
                _changed = true;
                _exercise = new Exercise(_localLanguage, _workoutType);
                _view.WorkoutChanged();
            }
        }


        private String       _filePath;
        private WorkoutState _workoutState;
        private Exercise     _exercise;
        private Boolean      _changed; 

        private readonly DateTime      _startDateTime;
        private readonly Language      _localLanguage;
        private readonly WorkoutType   _workoutType;
        private readonly IWorkoutView  _view;
    }
}