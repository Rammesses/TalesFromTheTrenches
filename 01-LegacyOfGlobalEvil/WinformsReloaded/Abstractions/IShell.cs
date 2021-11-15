using System;
using System.Windows.Forms;

namespace WinformsReloaded.Abstractions
{
    public interface IShell
    {
        public Form MainForm { get; }

        public void Close();

        public event EventHandler Closed;
    }
}
