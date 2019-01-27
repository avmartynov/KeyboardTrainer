using System;
using System.Globalization;
using System.Speech.Synthesis;
using System.Windows.Forms;
using Twidlle.KeyboardTrainer.WinFormsApp;

namespace Articulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _synthesizer.SelectVoiceByHints(VoiceGender.NotSet, VoiceAge.Adult, 0, CultureInfo.CreateSpecificCulture("ru-Ru"));
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonArticulate_Click(object sender, EventArgs e)
        {
            Speaker.Speak(textBoxLang.Text, textBoxSsml.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _synthesizer.SelectVoiceByHints(VoiceGender.NotSet, VoiceAge.Adult, 0, CultureInfo.CreateSpecificCulture("ru-Ru"));            
            _synthesizer.Speak("Заглавное: W");

            // _synthesizer.SelectVoiceByHints(VoiceGender.NotSet, VoiceAge.Adult, 0, CultureInfo.CreateSpecificCulture("ru-Ru"));            
            // _synthesizer.Speak("D");

            // var synthesizer = new SpeechSynthesizer();
            // var installedVoices = synthesizer.GetInstalledVoices();
        }

        private static readonly SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
    }
}
