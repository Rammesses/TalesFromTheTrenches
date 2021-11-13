using System;
using System.Windows.Forms;

namespace WinformsReloaded
{
    public partial class HelloView : UserControl
    {
        public HelloView()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GodObject.MainForm.Close();
        }
    }
}
