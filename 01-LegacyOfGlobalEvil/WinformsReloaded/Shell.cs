using System;
using System.Windows.Forms;

using Microsoft.Extensions.Logging;
using WinformsReloaded.Abstractions;

namespace WinformsReloaded
{
    public partial class Shell : Form, IShell
    {
        private readonly ILogger logger;

        public Shell(ILogger<Shell> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InitializeComponent();
        }

        public Form MainForm => this;

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.logger.LogInformation("User changed tab to {selectedTabName}", this.tabControl1.SelectedTab?.Name);
        }
    }
}
