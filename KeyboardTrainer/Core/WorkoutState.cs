
namespace Twidlle.KeyboardTrainer.Core
{
    public class WorkoutState
    {
        public int  BestCharPerMinute { get; set; }
                      
        public double BestErrorCount { get; set; }
                      
        public int  LastCharPerMinute { get; set; }
                      
        public double LastErrorCount { get; set; }

        public double AverageCharPerMinute { get; set; }

        public double AverageErrorCount { get; set; }

        public int  ExerciseCount { get; set; }
    }

    public static class WorkoutStateEx
    {
        public static void SetExerciseFinished(this WorkoutState workoutState, int charPerMinute, double errorCount)
        {
            workoutState.LastCharPerMinute = charPerMinute;
            workoutState.LastErrorCount    = errorCount;

            if (workoutState.BestCharPerMinute < workoutState.LastCharPerMinute)
            {
                workoutState.BestCharPerMinute = workoutState.LastCharPerMinute;
                workoutState.BestErrorCount    = workoutState.LastErrorCount;
            }

            workoutState.AverageCharPerMinute = (workoutState.AverageCharPerMinute * workoutState.ExerciseCount + charPerMinute) / (workoutState.ExerciseCount + 1);
            workoutState.AverageErrorCount    = (workoutState.AverageErrorCount    * workoutState.ExerciseCount + errorCount   ) / (workoutState.ExerciseCount + 1);

            workoutState.ExerciseCount++;
        }
    }
}
