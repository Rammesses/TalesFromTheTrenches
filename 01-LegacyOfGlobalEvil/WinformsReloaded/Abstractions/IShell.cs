using System;
using System.Windows.Forms;

namespace WinformsReloaded.Abstractions
{
    public interface IShell
    {
        public Form MainForm { get; }

        public event EventHandler Closed;
    }
}
