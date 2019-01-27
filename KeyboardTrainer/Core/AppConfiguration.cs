using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Twidlle.Infrastructure;

namespace Twidlle.KeyboardTrainer.Core
{
    public static class AppConfiguration
    {
        public static ExerciseAppearance Appearance => _appearance;


        public static IEnumerable<WorkoutType> WorkoutTypes => _workoutTypes;


        public static IEnumerable<Language> LocalLanguages => _localLanguages;

             
        public static WorkoutType GetWorkoutType(String workoutTypeCode)
        {
            return  _workoutTypes.SingleOrDefault(i => i.Code == workoutTypeCode) ?? new WorkoutType();
        }


        public static Language GetLocalLanguage(String languageCode)
        {
            return _localLanguages.SingleOrDefault(i => i.Code == languageCode) ?? new Language();    
        }


        public static Pronouncing GetPronouncing(String itemName, String localLanguage)
        {
            try
            {
                List<Pronouncing> prons;
                if (!_pronouncings.TryGetValue(itemName, out prons))
                    return new Pronouncing {Name = itemName, Language = localLanguage, Text = itemName};

                var pron = prons.SingleOrDefault(i => i.Language == localLanguage);
                if (pron != null)
                    return new Pronouncing {Name = itemName, Language = localLanguage, Text = pron.Text};
            
                pron = prons.SingleOrDefault(i => i.Language == "en");
                return pron != null ? 
                    new Pronouncing {Name = itemName, Language = pron.Language, Text = pron.Text} : 
                    new Pronouncing {Name = itemName, Language = localLanguage, Text = itemName};
            }
            catch (Exception x)
            {
                throw new InvalidOperationException("Incorrect pronouncing config for item '{itemName}', '{localLanguage}'", x);
            }
        }


        static AppConfiguration ()
        {
            _appearance     = ReadFile<ExerciseAppearance>("Appearance");
            _workoutTypes   = ReadFile<List<WorkoutType>>("WorkoutTypes");
            _localLanguages = ReadFile<List<Language>>("Languages").OrderBy(lang => lang.Name)
                                                                  .ToList();

            var pronouncings = ReadFile<List<Pronouncing>>("Pronouncings");
            _pronouncings = pronouncings.Select(i => i.Name)
                                        .Distinct()
                                        .ToDictionary(name => name, 
                                                      name => pronouncings.Where(j => j.Name == name)
                                                                         .ToList());
        }


        private static T ReadFile<T>(String fileName)
        {
            var dir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) ?? "";
            var filePath = Path.ChangeExtension(Path.Combine(dir, "Config", fileName), "xml");
            try
            {
                return XmlFile.Read<T>(filePath, rootTag: fileName);
            }
            catch (Exception x)
            {
                throw new InvalidOperationException($"Can't read config file: {filePath}", x);
            }
        }


        static readonly ExerciseAppearance _appearance;

        static readonly IEnumerable<WorkoutType> _workoutTypes;

        static readonly IEnumerable<Language> _localLanguages;

        static readonly IDictionary<String /* itemname */, List<Pronouncing>> _pronouncings;

        static readonly TraceSource _trace = new TraceSource(typeof(AppConfiguration).Namespace ?? "");
    }
}
