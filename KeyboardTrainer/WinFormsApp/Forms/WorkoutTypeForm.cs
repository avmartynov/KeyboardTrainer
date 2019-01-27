using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Twidlle.Infrastructure;
using Twidlle.KeyboardTrainer.Core;

namespace Twidlle.KeyboardTrainer.WinFormsApp.Forms
{
    public partial class WorkoutTypeForm : Form
    {
        private readonly WorkoutType[] _allWorkoutTypes;

        public WorkoutTypeForm(String currentWorkoutTypeCode, IEnumerable<WorkoutType> allWorkoutTypes)
        {
            InitializeComponent();

            _allWorkoutTypes = allWorkoutTypes.ToArray();

            workoutTypesListBox.Items.AddRange(_allWorkoutTypes.Select(i => i.Name).Cast<object>().ToArray());

            workoutTypesListBox.SelectedItem = _allWorkoutTypes.SingleOrDefault(i => i.Code == currentWorkoutTypeCode)?.Name;
        }


        public String WorkoutTypeCode => _allWorkoutTypes[workoutTypesListBox.SelectedIndex].Code;


        private void testsListBox_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }


        private void testsListBox_SelectedIndexChanged(Object sender, EventArgs e)
        {
            textBox.Text = _allWorkoutTypes[workoutTypesListBox.SelectedIndex].Description.JoinLines();
        }
    }
}
