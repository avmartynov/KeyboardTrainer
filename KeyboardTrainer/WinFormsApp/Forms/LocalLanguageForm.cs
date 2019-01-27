using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Twidlle.KeyboardTrainer.Core;

namespace Twidlle.KeyboardTrainer.WinFormsApp.Forms
{
    public partial class LocalLanguageForm : Form
    {
        private readonly Language[] _languages;

        public LocalLanguageForm(string languageCode, IEnumerable<Language> localLanguages)
        {
            InitializeComponent();

            _languages = localLanguages.ToArray();

            localLanguagesListBox.Items.AddRange(_languages.Select(i => i.Name).Cast<object>().ToArray());

            localLanguagesListBox.SelectedItem = _languages.SingleOrDefault(i=> i.Code == languageCode)?.Name;
        }


        public string LocalLanguageCode => _languages[localLanguagesListBox.SelectedIndex].Code;


        private void testsListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
