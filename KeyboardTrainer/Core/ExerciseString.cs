using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Twidlle.Infrastructure;

namespace Twidlle.KeyboardTrainer.Core
{
    public class ExerciseString : IEnumerable<ExerciseItem>
    {
        public ExerciseString(Language localLanguage, WorkoutType workoutType)
        {
            _items = new List<ExerciseItem>();

            while (_items.Count < workoutType.ExerciseLength)
            {
                var maxWordLength = workoutType.ExerciseLength - _items.Count;
                if (maxWordLength < workoutType.MinWordLength)
                    return;

                maxWordLength = Math.Min(maxWordLength, workoutType.MaxWordLength);

                if (_items.Any())
                {
                     var spacePronounce = AppConfiguration.GetPronouncing(" ", localLanguage.Code);

                    _items.Add(new CharacterItem(' ', false, spacePronounce.Text, spacePronounce.Language));
                }

                if (_rand.Next(99) < workoutType.FunctionalKeysPercent)
                    _items.Add(NextFunctionalKey(localLanguage));
                else
                    _items.AddRange(NextWord(localLanguage, workoutType, maxWordLength));
            }

            _trace.Info("New exercise:");    
            foreach (var item in _items)
            {
                _trace.Info($"{item}");    
            }
            _trace.Info(".");    
        }


        public ExerciseItem this[Int32 index] => _items[index];

        public IEnumerator<ExerciseItem> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();


        public Char GetCharacter(Int32 position)
        {
            var currentKeyItem = _items[position];
            var item = currentKeyItem as CharacterItem;
            if (item != null)
                return item.Character;

            throw new InvalidOperationException("Excercise item is not character.");
        }


        private static IEnumerable<ExerciseItem> NextWord(Language localLanguage, WorkoutType workoutType, Int32 maxWordLength)
        {
            var localWord       = _rand.Next(99) < workoutType.LocalWordPercent;
            var capitalizedWord = _rand.Next(99) < workoutType.CapitalizedWordPercent;
            var capsLockWord    = _rand.Next(99) < workoutType.CapsLockWordPercent;
            var wordWithPoint   = _rand.Next(99) < workoutType.DecimalPointNumberPercent;

            var wordLen = _rand.Next(workoutType.MinWordLength, maxWordLength + 1);

            var pointPosition = -1;
            if (wordWithPoint && 3 <= wordLen)
                pointPosition = _rand.Next(1, wordLen - 2);

            var position = 0;
            var wordLanguage = localWord ? localLanguage : _englishUs;

            var firstKey = NextKeyItem(wordLanguage, workoutType, localLanguage, capsLockWord || capitalizedWord);

            var wordKeys = new List<ExerciseItem> { firstKey };
            while (wordKeys.Count < wordLen)
            {
                position++;
                var decimalPointCharacter = workoutType.DecimalPointCharacter.Substring(0, 1)[0];
                // var decimalPointPronounce = AppConfiguration.GetPronouncing(workoutType.DecimalPointCharacter, wordLanguage.Code);
                var decimalPointPronounce = AppConfiguration.GetPronouncing(workoutType.DecimalPointCharacter, localLanguage.Code);
                wordKeys.Add(position == pointPosition
                    ? new CharacterItem(decimalPointCharacter, localWord, decimalPointPronounce.Text, decimalPointPronounce.Language)
                    : NextKeyItem(wordLanguage, workoutType, localLanguage, capsLockWord));
            }

            return wordKeys;
        }


        private static ExerciseItem NextKeyItem(Language wordLanguage, WorkoutType workoutType, Language localLanguage, Boolean upperChar)
        {
            var digitBorder       = workoutType.DigitsPercent;
            var punctuationBorder = workoutType.PunctuationPercent   + digitBorder;
            var specSymbolBorder  = workoutType.SpecialSymbolPercent + punctuationBorder;
            var randSymbolBorder  = workoutType.RandomSymbolPercent  + punctuationBorder;

            var randSymbols = wordLanguage.Code == localLanguage.Code 
                                ? (workoutType.LocalRandomSymbols ?? "") 
                                : (workoutType.RandomSymbols      ?? "");

            var typeValue = _rand.Next(99);
            var ch = typeValue < digitBorder       ?               DIGITS[_rand.Next(              DIGITS.Length)]
                   : typeValue < punctuationBorder ? wordLanguage.Punctuation[_rand.Next(wordLanguage.Punctuation.Length)]
                   : typeValue < specSymbolBorder  ?     wordLanguage.Symbols[_rand.Next(    wordLanguage.Symbols.Length)]
                   : typeValue < randSymbolBorder  ?              randSymbols[_rand.Next(             randSymbols.Length)] 
                   :                                     wordLanguage.Letters[_rand.Next(    wordLanguage.Letters.Length)];
            ch = upperChar ? Char.ToUpper(ch) : ch;
            var chStr = new String(ch, 1);
            var chPronounce = AppConfiguration.GetPronouncing(chStr.ToLower(), localLanguage.Code);
            var upperPronounce = AppConfiguration.GetPronouncing("UpperPrefix", localLanguage.Code);
            var voiceText = (Char.IsUpper(ch) ? upperPronounce.Text + " " : "") + chPronounce.Text;
            return new CharacterItem(ch,  wordLanguage.Code == localLanguage.Code, voiceText, chPronounce.Language);
        }


        private static ExerciseItem NextFunctionalKey(Language localLanguage)
        {
            var funcKeys    = _functionalKeyNames.Keys.ToArray();
            var key         = funcKeys[_rand.Next(funcKeys.Length)];
            var displayText = _functionalKeyNames[key];
            var pronounce   = AppConfiguration.GetPronouncing(displayText, localLanguage.Code);
            return new FuncKeyItem(key, displayText, pronounce.Text, pronounce.Language);
        }


        private readonly List<ExerciseItem> _items;

        private static readonly Random _rand = new Random();

        private static readonly Language _englishUs = new Language();

        private static readonly Dictionary<Keys, String> _functionalKeyNames = new Dictionary<Keys, String> 
        {
            { Keys.F1,         "F1"       },
            { Keys.F2,         "F2"       },
            { Keys.F3,         "F3"       },
            { Keys.F4,         "F4"       },
            { Keys.F5,         "F5"       },
            { Keys.F6,         "F6"       },
            { Keys.F7,         "F7"       },
            { Keys.F8,         "F8"       },
            { Keys.F9,         "F9"       },
            { Keys.F10,        "F10"      },
            { Keys.F11,        "F11"      },
            { Keys.F12,        "F12"      },

            { Keys.Escape,     "Escape"   },
            { Keys.Tab,        "Tab"      },
            { Keys.CapsLock,   "CapsLock" },
            { Keys.ControlKey, "Ctrl"     },

            { Keys.Back,       "Back"     },
            { Keys.Enter,      "Enter"    },
            { Keys.Apps,       "Menu"     },

            { Keys.Insert,     "Insert"   },
            { Keys.Delete,     "Delete"   },
            { Keys.Home,       "Home"     },
            { Keys.End,        "End"      },
            { Keys.PageDown,   "PageDown" },
            { Keys.PageUp,     "PageUp"   },

            { Keys.Scroll,     "Scroll"   },
            { Keys.Pause,      "Pause"    },
            { Keys.NumLock,    "NumLock"  },

//          { Keys.PrintScreen, "<PrintScreen> " },
//          { Keys.Windows,     "<Windows>"      },
//          { Keys.Alt,         "<Alt>"          },
//          { Keys.Shift,       "<Shift>"        },
        };

        private const String DIGITS = "0123456789";

        private static readonly TraceSource _trace = new TraceSource(typeof(ExerciseString).Namespace?? "");
    }
}

