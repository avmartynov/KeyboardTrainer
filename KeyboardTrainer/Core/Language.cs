using System;

namespace Twidlle.KeyboardTrainer.Core
{
    public class Language
    {
        public String Code { get; set; } = "en-US";

        public String Name { get; set; } = "English(US)";

        public String Letters { get; set; } = "abcdefghijklmnopqrstuvwxyz";

        public String Punctuation { get; set; } = @".,;:?!`'""-()";

        public String Symbols { get; set; } = @"~@#$%^&*_+=[]{}|\/<>";
    }
}
