using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DataComparision
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            DispatcherUnhandledException += Current_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }


        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            CommonUtils.Log("未捕获的UI线程异常！", e.Exception);
            //MessageBox.Show("未捕获的异常，详细信息请查看日志。", "意外的操作", MessageBoxButton.OK, MessageBoxImage.Information);//这里通常需要给用户一些较为友好的提示，并且后续可能的操作
            //if (e.Exception.StackTrace.ToUpper().Contains("OPENCLIPBOARD FAILED"))
            //{
            //    MessageBox.Show("剪贴板被占用，请使用导出功能！");
            //}
            e.Handled = true;//使用这一行代码告诉运行时，该异常被处理了，不再作为UnhandledException抛出了。
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            CommonUtils.Log("未捕获的UI线程异常！" + e.ExceptionObject.ToJson());
        } 
    }
}
