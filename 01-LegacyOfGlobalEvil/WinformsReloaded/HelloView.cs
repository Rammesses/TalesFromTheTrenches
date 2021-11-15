using System;
using System.Windows.Forms;
using WinformsReloaded.Abstractions;

namespace WinformsReloaded
{
    public partial class HelloView : UserControl
    {
        private readonly IShell shell;
        private readonly Lazy<IShell> shellLazy;

        private IShell Shell => shellLazy.Value;

        // This does NOT work!
        // public HelloView() :
        //     this(ServiceLocator.GetRequired<IShell>())
        // {
        // }

        public HelloView()
        {
            this.shellLazy = new Lazy<IShell>(() => this.shell ?? ServiceLocator.GetRequired<IShell>());

            InitializeComponent();
        }

        public HelloView(IShell shell) :
            this()
        {
            this.shell = shell ?? throw new ArgumentNullException(nameof(shell));            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Shell.Close();
        }
    }
}
