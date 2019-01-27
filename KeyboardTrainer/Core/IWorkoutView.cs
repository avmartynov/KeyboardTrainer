namespace Twidlle.KeyboardTrainer.Core
{
    public interface IWorkoutView
    {
        /// <summary> Notification about workout perform </summary>
        void WorkoutChanged();

        /// <summary> Notification about exercise execution </summary>
        void ExerciseChanged();

        /// <summary> Notification about workout have been saved </summary>
        void WorkoutSaved();
    }
}
