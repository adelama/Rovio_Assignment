using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rovio.TapMatch.WindowsApp
{
    internal static class Program
    {
        private static RemoteController remoteController;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var remoteForm = new RemoteForm();
            remoteController = new RemoteController(remoteForm);
            Application.Run(remoteForm);
        }
    }
}
