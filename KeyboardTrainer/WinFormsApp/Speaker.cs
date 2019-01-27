using System;
using System.Media;
using System.Speech.Synthesis;

namespace Twidlle.KeyboardTrainer.WinFormsApp
{
    public static class Speaker
    {
        public static void SpeakAsync(String languageCode, String text)
        {
            SpeakSsml(languageCode, text, async: true);
        }


        public static void Speak(String languageCode, String text)
        {
            SpeakSsml(languageCode, text);
        }


        public static void PlayAsterisk()
        {
            _synthesizer.SpeakAsyncCancelAll();
            SystemSounds.Asterisk.Play();
        }


        private static void SpeakSsml(String  languageCode, 
                                      String  content,
                                      Boolean async = false)
        {
            languageCode = languageCode ?? "en-US";

            var ssml = "<speak version=\"1.0\" " +
                              "xmlns=\"http://www.w3.org/2001/10/synthesis\" " +
                              "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                              "xsi:schemaLocation=\"http://www.w3.org/2001/10/synthesis " +
                              "http://www.w3.org/TR/speech-synthesis/synthesis.xsd\" " +
                             $"xml:lang=\"{languageCode}\" >" +
                            $"{content}" +
                       "</speak>";

            _synthesizer.SpeakAsyncCancelAll();

            if (async)
                _synthesizer.SpeakSsmlAsync(ssml);
            else
                _synthesizer.SpeakSsml(ssml);
        }


        private static readonly SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
    }
}
