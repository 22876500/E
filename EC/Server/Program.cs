using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    //加上默认值，避免修改后与原数据库中数据不一致，方便后面增加新角色。
    public enum 角色 
    { 
        超级管理员 = 0, 
        普通管理员 = 1, 
        超级风控员 = 2, 
        普通风控员 = 3, 
        交易员 = 4, 

    } 
    public enum 分组
    {
        ALL = 0,
        XM = 1,
        CQ = 2,
        XA = 3,
        SH = 4,
        T1 = 5,
        BY1 = 6,
        BY2 = 7,
        FK = 8,
        NB = 9,
    }
    static class Program
    {
        public static bool IsServiceUpdating = false;

        public static AppConfig appConfig = new AppConfig();

        public static string Version = "201801070003";

        public static Logger logger = new Logger();

        public static UIDataSet UIdataSet = new UIDataSet();


        public static ConcurrentDictionary<string, bool> 风控分配表Changed = new ConcurrentDictionary<string, bool>();


        public static ConcurrentBag<string> CancelOrders = new ConcurrentBag<string>();

        public static ConcurrentDictionary<string, bool> 成交表Changed = new ConcurrentDictionary<string, bool>();
        public static ConcurrentDictionary<string, bool> 委托表Changed = new ConcurrentDictionary<string, bool>();
        public static ConcurrentDictionary<string, bool> 平台用户表Changed = new ConcurrentDictionary<string, bool> ();
        public static ConcurrentDictionary<string, bool> 额度分配表Changed = new ConcurrentDictionary<string, bool>();
        public static ConcurrentDictionary<string, bool> 订单表Changed = new ConcurrentDictionary<string, bool> ();
        public static ConcurrentDictionary<string, bool> 已平仓订单表Changed = new ConcurrentDictionary<string, bool>();
        public static ConcurrentQueue<JyDataSet.委托Row> 废单通知 = new ConcurrentQueue<Server.JyDataSet.委托Row>();
        public static ConcurrentQueue<JyDataSet.成交Row> 成交通知 = new ConcurrentQueue< JyDataSet.成交Row>();
        public static ConcurrentQueue<风控操作> 风控操作 = new ConcurrentQueue<风控操作>();
        public static DbDataSet db = new DbDataSet();


        
        public static ConcurrentDictionary<string, JyDataSet.成交DataTable> 帐户成交DataTable = new ConcurrentDictionary<string, JyDataSet.成交DataTable>();
        public static ConcurrentDictionary<string, JyDataSet.委托DataTable> 帐户委托DataTable = new ConcurrentDictionary<string, JyDataSet.委托DataTable>();
        


       
        public static ConcurrentDictionary<string, JyDataSet.成交DataTable> 交易员成交DataTable = new ConcurrentDictionary<string, JyDataSet.成交DataTable>();
        public static ConcurrentDictionary<string, JyDataSet.委托DataTable> 交易员委托DataTable = new ConcurrentDictionary<string, JyDataSet.委托DataTable>();



        public static ConcurrentDictionary<IClient, string> ClientUserName = new ConcurrentDictionary<IClient, string>();

        public static MainForm mainForm;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            //UI线程异常
            Application.ThreadException += Application_ThreadException;

            //处理非UI线程异常   
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //处理未捕获的异常   
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TdxApi.OpenTdx();

#if Release
            LoginForm LoginForm1 = new LoginForm();
            DialogResult DialogResult1 = LoginForm1.ShowDialog();
            if (DialogResult1 == DialogResult.OK)
            {
                mainForm = new MainForm();
                Application.Run(mainForm);
            }
#else
            mainForm = new MainForm();
            Application.Run(mainForm);
#endif

            TdxApi.CloseTdx();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            //MessageBox.Show("系统出现未知异常，请重启系统！");
            if (ex != null)
            {
                //Program.logger.LogInfo("服务器发生未捕获的线程异常，Message:{0} \n  StackTrace:{1}", ex.Message, ex.StackTrace);
                Program.logger.Init();
                Program.logger.LogInfo("服务器发生未捕获的线程异常，Message:{0}  StackTrace:{1}", ex.Message, ex.StackTrace);
            }
            
        }


        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.Exception.InnerException == null ? e.Exception : e.Exception.InnerException;

                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            catch (Exception)
            {
                Program.logger.LogInfo("服务器发生未捕获的线程异常 ", e.ToJson());
            }
        }
    }

}
