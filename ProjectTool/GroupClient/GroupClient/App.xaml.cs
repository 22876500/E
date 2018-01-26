using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GroupClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        }


        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
             {
                 CommonUtils.Log("未捕获的UI线程异常！", e.Exception);
                 e.Handled = true;
             }
             catch (Exception ex)
             {
                 CommonUtils.Log("不可恢复的UI线程全局异常", ex);
                 MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
             }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
             {
                 var exception = e.ExceptionObject as Exception;
                 if (exception != null)
                 {
                     CommonUtils.Log("非UI线程全局异常！", exception);
                 }
             }
             catch (Exception)
             {
                 //CommonUtils.Log("不可恢复的非UI线程全局异常！", ex);
                 MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
             }
        }

    }
}
