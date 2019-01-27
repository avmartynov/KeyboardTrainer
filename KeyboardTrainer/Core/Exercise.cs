using System;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace Twidlle.KeyboardTrainer.Core
{
    public class Exercise
    {
        public Exercise(Language localLanguage, WorkoutType workoutType)
        {
            if (localLanguage == null)
                throw new ArgumentNullException(nameof(localLanguage));

            if (workoutType == null)
                throw new ArgumentNullException(nameof(workoutType));

            _execiseString = new ExerciseString(localLanguage, workoutType);
        }


        public ExerciseString ExeciseString => _execiseString;

        public bool Running => _startDate != default(DateTime);

        public Int32 CurrentPosition => _currentPosition;

        public ExerciseItem CurrentItem => _currentPosition < ExeciseString.Count() ? ExeciseString[_currentPosition] : null;

        public Boolean WrongTyping => _wrongTyping;


        public Boolean CheckFinished(ref Int32 charPerMinute, ref Int32 errorCount)
        {
            if (_finishDate == default(DateTime))
                return false;

            charPerMinute = (Int32)(ExeciseString.Count() / (_finishDate - _startDate).TotalMinutes);
            errorCount = _errorCount;
            return true;
        }


        public Boolean KeyPressed(Keys keyPressed)
        {
            if (!Running)
                _startDate = DateTime.Now;

            var currentKeyItem = ExeciseString[_currentPosition];
            if (currentKeyItem is CharacterItem)
                return false;

            if (! (currentKeyItem is FuncKeyItem))
                throw new InvalidOperationException("Invalid Exercise item type");

            var keyItem = currentKeyItem as FuncKeyItem;
            _wrongTyping = keyItem.Key != keyPressed;

            if (WrongTyping)
                _errorCount++;
            else
            {
                _currentPosition++;
                if (CurrentPosition == ExeciseString.Count())
                {
                    _finishDate = DateTime.Now;
                }
            }
            return true;
        }


        public void CharPressed(Char charPressed)
        {
            if (_startDate == default(DateTime))
                _startDate = DateTime.Now;

            _wrongTyping = ExeciseString.GetCharacter(CurrentPosition) != charPressed;

            if (WrongTyping)
                _errorCount++;
            else
            {
                _currentPosition++;
                if (CurrentPosition == ExeciseString.Count())
                {
                    _finishDate = DateTime.Now;
                }
            }
        }


        private DateTime _startDate;
        private DateTime _finishDate;
        private Int32    _errorCount;
        private Int32    _currentPosition;
        private Boolean  _wrongTyping;

        private readonly ExerciseString _execiseString;
    }
}
