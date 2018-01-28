using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASServer
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

        public static string Version = "201711100001";

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
        public static ConcurrentQueue<JyDataSet.委托Row> 废单通知 = new ConcurrentQueue<AASServer.JyDataSet.委托Row>();
        public static ConcurrentQueue<JyDataSet.成交Row> 成交通知 = new ConcurrentQueue< JyDataSet.成交Row>();
        public static ConcurrentQueue<风控操作> 风控操作 = new ConcurrentQueue<风控操作>();
        public static DbDataSet db = new DbDataSet();


        
        public static ConcurrentDictionary<string, JyDataSet.成交DataTable> 帐户成交DataTable = new ConcurrentDictionary<string, JyDataSet.成交DataTable>();
        public static ConcurrentDictionary<string, JyDataSet.委托DataTable> 帐户委托DataTable = new ConcurrentDictionary<string, JyDataSet.委托DataTable>();
        


       
        public static ConcurrentDictionary<string, JyDataSet.成交DataTable> 交易员成交DataTable = new ConcurrentDictionary<string, JyDataSet.成交DataTable>();
        public static ConcurrentDictionary<string, JyDataSet.委托DataTable> 交易员委托DataTable = new ConcurrentDictionary<string, JyDataSet.委托DataTable>();



        public static ConcurrentDictionary<IClient, string> ClientUserName = new ConcurrentDictionary<IClient, string>();

        /// <summary>
        /// 委托信息缓存，用于修正已获取委托编号，未获取委托详情时，向客户端回发。
        /// </summary>
        public static ConcurrentQueue<已发委托> QueueConsignmentCache = new ConcurrentQueue<已发委托>();

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
            
            mainForm = new MainForm();
            Application.Run(mainForm);
            

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

        /// <summary>
        /// 缓存下单信息，在获取到委托号但查询不到详细订单状态时，修正客户端订单信息显示。
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="证券代码"></param>
        /// <param name="买卖方向"></param>
        /// <param name="委托数量"></param>
        /// <param name="委托价格"></param>
        /// <param name="委托编号"></param>
        /// <param name="TradeLimit1"></param>
        public static void AddConsignmentCache(string UserName, string 证券代码, int 买卖方向, decimal 委托数量, decimal 委托价格, string 委托编号, string 组合号, string 证券名称, byte 市场)
        {
            if (!string.IsNullOrEmpty(委托编号))
            {
                try
                {
                    QueueConsignmentCache.Enqueue(new 已发委托()
                    {
                        交易员 = UserName,
                        组合号 = 组合号,
                        证券代码 = 证券代码,
                        证券名称 = 证券名称,
                        买卖方向 = 买卖方向,
                        委托价格 = 委托价格,
                        委托数量 = 委托数量,
                        撤单数量 = 0,
                        成交价格 = 0,
                        成交数量 = 0,
                        状态说明 = "已报",
                        委托编号 = 委托编号,
                        市场代码 = 市场,
                        日期 = DateTime.Now
                    });
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("下单完成后自动加入缓存异常\r\n  Message:{0}\r\n  StackTrace:{1}", ex.Message, ex.StackTrace);
                }
            }
        }
    }

    public class PublicStockOrder
    {
        public DateTime TradeTime { get; set; }

        public string Trader { get; set; }

        public string StockCode { get; set; }

        public string StockName { get; set; }

        public string OrderID { get; set; }

        /// <summary>
        /// 市场（0 深圳，1 沪市）
        /// </summary>
        public byte Market { get; set; }

        /// <summary>
        /// 买卖方向
        /// </summary>
        public int Side { get; set; }

        /// <summary>
        /// 委托价格
        /// </summary>
        public decimal OrderPrice { get; set; }

        /// <summary>
        /// 委托数量
        /// </summary>
        public decimal OrderQuantity { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal KnockDownPrice { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public decimal KnockDownQuantity { get; set; }

        /// <summary>
        /// 撤单数量
        /// </summary>
        public decimal CancelQuantity { get; set; }
    }
}
