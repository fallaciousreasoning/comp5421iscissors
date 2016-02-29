using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IScissors
{
    public partial class Blurrer : Form
    {
        public Action<int> Completed;

        public Blurrer()
        {
            InitializeComponent();
        }

        private void blurRadiusSlider_Scroll(object sender, EventArgs e)
        {
            blurRadiusLabel.Text = blurRadiusSlider.Value.ToString();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void okayButton_Click(object sender, EventArgs e)
        {
            Hide();
            Completed?.Invoke(blurRadiusSlider.Value);
        }
    }
}
