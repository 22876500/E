using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OKCoinClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application//,  ISingleInstanceApp
    {
        public static Logger Log = new Logger();

        App()
        {
            Log.Init();
        }
        //private const string Unique = "9305fb25-3111-4bf7-a100-11e1e08a1ddc";

        //[STAThread]
        //public static void Main()
        //{
        //    if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
        //    {
        //        var application = new App();
        //        application.InitializeComponent();
        //        application.Run();

        //        // Allow single instance code to perform cleanup operations
        //        SingleInstance<App>.Cleanup();
        //    }
        //}

        //#region ISingleInstanceApp Members
        //public bool SignalExternalCommandLineArgs(IList<string> args)
        //{
        //    // Bring window to foreground
        //    if (this.MainWindow.WindowState == WindowState.Minimized)
        //    {
        //        this.MainWindow.WindowState = WindowState.Normal;
        //    }

        //    this.MainWindow.Activate();

        //    // Handle command line arguments of second instance
        //    return true;
        //}
        //#endregion
    }
}
