using System;
using System.Windows.Forms;

namespace Twidlle.KeyboardTrainer.WinFormsApp.Forms
{
    public partial class OptionsForm : Form
    {
        public OptionsForm(Boolean openLastFile, Boolean voiceEnable)
        {
            InitializeComponent();

            this.openLastFileCheckBox.Checked = openLastFile;
            this.voiceCheckBox.Checked        = voiceEnable;
        }

        public Boolean OpenLastFile => this.openLastFileCheckBox.Checked;
        public Boolean VoiceEnable  => this.voiceCheckBox.Checked;
    }
}
