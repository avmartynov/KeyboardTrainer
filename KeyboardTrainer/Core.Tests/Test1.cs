using System;
using System.IO;
using Moq;
using NUnit.Framework;

namespace Twidlle.KeyboardTrainer.Core.Tests
{
    public class Prime_TestFixture
    {
        [Test]
        public void Basic_Test()
        {
            const int COUNT_OF_EXERCISES = 12;

            var viewMock = new Mock<IWorkoutView>();
            viewMock.Setup(a => a.ExerciseChanged());
            viewMock.Setup(a => a.WorkoutChanged ());

            var workout = new Workout(view: viewMock.Object,
                         localLanguageCode: "ru", 
                           workoutTypeCode: "EnglishLetters");

            for (var i = 0; i < COUNT_OF_EXERCISES; i++)
            {
                foreach (var item in workout.Exercise.ExeciseString)
                {
                    if (item is CharacterItem keyItem)
                        workout.OnCharPressed(keyItem.Character);
                    else
                    {
                        if (!(item is FuncKeyItem funcKeyItem))
                            throw new InvalidOperationException("Invalid Exercise item type");

                        var success = workout.OnKeyPressed(funcKeyItem.Key);
                        Assert.That(success, Is.True);
                        throw new InvalidOperationException("Invalid Exercise item type");
                    }
                }
            }
            Assert.That(workout.Changed, Is.True);
            Assert.That(workout.WorkoutState.ExerciseCount, Is.EqualTo(COUNT_OF_EXERCISES));

            Assert.Catch(typeof (InvalidOperationException), () => workout.Save());

            const string KTR_FILE_PATH = @"c:\Temp\x.ktr";
            workout.Save(KTR_FILE_PATH);

            var workout2 = Workout.Load(viewMock.Object, KTR_FILE_PATH);
            Assert.That(workout2.WorkoutState.ExerciseCount, Is.EqualTo(COUNT_OF_EXERCISES));

            File.Delete(KTR_FILE_PATH);
        }


        [Test]
        public void PronouncingData_Test()
        {
            Assert.That(AppConfiguration.GetPronouncing("в", "ru").Text, Is.EqualTo("вэ"));
            Assert.That(AppConfiguration.GetPronouncing("с", "ru").Text, Is.EqualTo("эс"));
        }
    }
}
