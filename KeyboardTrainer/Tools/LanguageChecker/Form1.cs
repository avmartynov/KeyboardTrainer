using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LanguageChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lettersButton_Click(object sender, EventArgs e)
        {
            var letters = this.textBox.Text;
            foreach (var letter in letters)
            {
                if (!Char.IsLetter(letter))
                    MessageBox.Show(this, letter.ToString());
            }
        }

        private void punctuationButton_Click(object sender, EventArgs e)
        {
            var punctuationChars = this.textBox.Text;
            foreach (var c in punctuationChars)
            {
                if (!Char.IsPunctuation(c))
                    MessageBox.Show(this, c.ToString());
            }
        }

        private void symbolButton_Click(object sender, EventArgs e)
        {
            var symbols = this.textBox.Text;
            foreach (var s in symbols)
            {
                if (!Char.IsSymbol(s))
                    MessageBox.Show(this, s.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var symbols = this.textBox.Text;
            foreach (var s in symbols)
            {
                if (!Char.IsDigit(s))
                    MessageBox.Show(this, s.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var symbols = this.textBox.Text;
            foreach (var s in symbols)
            {
                if (!Char.IsSeparator(s))
                    MessageBox.Show(this, s.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var symbols = this.textBox.Text;
            foreach (var s in symbols)
            {
                if (!Char.IsNumber(s))
                    MessageBox.Show(this, s.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var symbols = this.textBox.Text;
            foreach (var s in symbols)
            {
                if (!Char.IsWhiteSpace(s))
                    MessageBox.Show(this, s.ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var symbols = this.textBox.Text;
            foreach (var s in symbols)
            {
                if (!Char.IsControl(s))
                    MessageBox.Show(this, s.ToString());
            }
        }
    }
}
