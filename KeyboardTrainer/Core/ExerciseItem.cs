using System;
using System.Windows.Forms;

namespace Twidlle.KeyboardTrainer.Core
{
    /// <summary> Одна позиция (один элемент) тестовой строки. </summary>
    /// <remarks>Immutable class.</remarks>
    public class ExerciseItem
    {
        protected ExerciseItem(String displayText, String voiceText, String voiceLanguage)
        {
            DisplayText   = displayText;
            VoiceText     = voiceText;
            VoiceLanguage = voiceLanguage;
        }

        public String DisplayText { get; }

        public String VoiceText { get; }

        public String VoiceLanguage { get; }

        public override string ToString()
        {
            return $"{{{DisplayText}, {VoiceText}, {VoiceLanguage}}}";
        }
    }


    /// <summary> Элемент тестовой строки - символ. </summary>
    /// <remarks>Immutable class.</remarks>
    public class CharacterItem : ExerciseItem
    {
        public CharacterItem(Char c, Boolean local, String voiceText, String voiceLanguage) 
            : base (new String(c, 1), voiceText, voiceLanguage)
        {
            IsLocalCharacter = local;
            Character        = c;
        }

        public Boolean IsLocalCharacter { get; }

        public Char Character { get; }

        public override string ToString()
        {
            return $"{{{base.ToString()}, {Character}, {IsLocalCharacter}}}";
        }
    }


    /// <summary> Элемент тестовой строки - функциональная клавиша. </summary>
    /// <remarks>Immutable class.</remarks>
    public class FuncKeyItem : ExerciseItem
    {
        public FuncKeyItem(Keys   key, 
                           String displayText, 
                           String voiceText, 
                           String voiceLanguage) 
            : base(displayText, voiceText, voiceLanguage)
        {
            Key  = key;
        }

        public Keys Key { get; }

        public override string ToString()
        {
            return $"{{{base.ToString()}, {Key}}}";
        }
    }
}
