using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinformsReloaded
{
    public partial class Shell : Form
    {
        public Shell()
        {
            InitializeComponent();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("User changed tab to {0}", this.tabControl1.SelectedTab?.Name);
        }
    }
}
