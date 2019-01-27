using System;
using System.Xml.Serialization;

namespace Twidlle.KeyboardTrainer.Core
{
    public class WorkoutType
    {
        /// <summary> Идентификатор типа тренировки </summary>
        [XmlAttribute("id")]
        public string Code { get; set; } = "EnglishLetters";

        /// <summary> Имя плана тренировки </summary>
        [XmlElement]
        public string Name { get; set; } = "English letters";

        /// <summary> Подробное описание плана тренировки </summary>
        [XmlElement]
        public string Description { get; set; } = String.Empty;

        /// <summary> Длина одного упражнения (тестового текста). </summary>
        [XmlAttribute("exerciseLength")]
        public int ExerciseLength { get; set; } = 30;

        /// <summary> Минимальная длина слова (1 или больше)</summary>
        [XmlAttribute("minWordLength")]
        public int MinWordLength { get; set; } = 2;

        /// <summary> Максимальная длина слова </summary>
        [XmlAttribute("maxWordLength")]
        public int MaxWordLength { get; set; } = 6;


        // Особые 

        /// <summary> Символ десятичной точки </summary>
        [XmlAttribute("decimalPointCharacter")]
        public string DecimalPointCharacter { get; set; } = ".";

        /// <summary> Произвольный набор символов </summary>
        [XmlAttribute("randomSymbols")]
        public String RandomSymbols { get; set; } = String.Empty;

        /// <summary> Произвольный набор локальных символов </summary>
        [XmlAttribute("localRandomSymbols")]
        public String LocalRandomSymbols { get; set; } = String.Empty;


        /// Слова

        /// <summary> Процент слов в местной раскладке </summary>
        [XmlAttribute("localWordPercent")]
        public Int32 LocalWordPercent { get; set; }

        /// <summary> Процент слов с заглавной буквы </summary>
        [XmlAttribute("capitalizedWordPercent")]
        public Int32 CapitalizedWordPercent { get; set; }

        /// <summary> Процент слов, набранных в режиме CapsLock среди слов не с заглавной буквы </summary>
        [XmlAttribute("capsLockWordPercent")]
        public Int32 CapsLockWordPercent { get; set; }

        /// <summary> Процент чисел с десятичной точкой </summary>
        [XmlAttribute("decimalPointNumberPercent")]
        public Int32 DecimalPointNumberPercent { get; set; }

        /// <summary> Процент слов-управляющих и функциональных клавиш </summary>
        [XmlAttribute("functionalKeysPercent")]
        public Int32 FunctionalKeysPercent { get; set; }


        /// Состав слов (Буквы, цыфры, пунктуация, спецсимволы)

        /// <summary> Доля цифр </summary>
        [XmlAttribute("digitsPercent")]
        public Int32 DigitsPercent { get; set; }

        /// <summary> Доля знаков препинания </summary>
        [XmlAttribute("punctuationPercent")]
        public Int32 PunctuationPercent { get; set; }

        /// <summary> Доля спец-символов </summary>
        [XmlAttribute("specialSymbolPercent")]
        public Int32 SpecialSymbolPercent { get; set; }

        /// <summary> Доля символов из произвольного множества </summary>
        [XmlAttribute("randomSymbolPercent")]
        public Int32 RandomSymbolPercent { get; set; }
    }
}
