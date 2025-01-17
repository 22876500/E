﻿using AASClient.AASServiceReference;
using AASClient.Manager;
using AASTrader.Model.DataModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace AASClient
{
    public partial class TradeMainForm : Form
    {

        public DateTime ErrorTime { get; set; }
        public int hqFormCount;
        public HqForm[] hqForm;
        string uiFile;
        int hqFormIndex = 0;
        public LogForm logForm = new LogForm();

        CancelWTForm cancelWTForm = new CancelWTForm();
        CJForm cJForm = new CJForm();
        WTForm wtForm = new WTForm();
        OpenTradeForm openTradeForm = new OpenTradeForm();
        ClosedTradeForm closedTradeForm = new ClosedTradeForm();
        业绩统计Form 交易统计Form = new 业绩统计Form();

        public ConcurrentDictionary<string, AccounCoin> dictAccountCoin = new ConcurrentDictionary<string, AccounCoin>();
        DateTime LastFetchDay = DateTime.MinValue.Date;
        DateTime lastFetchCoinInfo = DateTime.MinValue;
        bool isQueryCoin = false;

        public TradeMainForm()
        {
            InitializeComponent();
            try
            {
                this.hqFormCount = (int)decimal.Parse(Program.accountDataSet.参数.GetParaValue("报价窗口数目", "4"));
                Program.logger.LogInfo(string.Format("2.hqFormCount Init finished, count {0}", hqFormCount));

                this.hqForm = new HqForm[this.hqFormCount];
                for (int i = 0; i < this.hqFormCount; i++)
                {
                    this.hqForm[i] = new HqForm(i, this);
                }

                this.uiFile = Path.Combine(Application.StartupPath, Program.Current平台用户.用户名 + "\\DockUI.xml");
                string[] 主窗体位置 = Program.accountDataSet.参数.GetParaValue("主窗体位置", "300|10|1000|618").Split('|');
                this.Location = new System.Drawing.Point(int.Parse(主窗体位置[0]), int.Parse(主窗体位置[1]));
                this.Size = new System.Drawing.Size(int.Parse(主窗体位置[2]), int.Parse(主窗体位置[3]));
                this.FormClosing += TradeMainForm_FormClosing;
                this.FormClosed += TradeMainForm_FormClosed;
                formInit();
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo(string.Format("3.交易员主界面加载异常！错误信息{0}", ex.Message));
            }

            
        }
                
        private void TradeMainForm_Load(object sender, EventArgs e)
        {
            formInit();
        }

        private void formInit()
        {
            if (File.Exists(this.uiFile))
            {
                try
                {
                    DeserializeDockContent ddContent = new DeserializeDockContent(GetContentFromPersistString);

                    this.dockPanel1.LoadFromXml(uiFile, ddContent);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("加载界面布局文件失败:" + ex.Message);

                    this.ShowAllChildWindow();
                }
            }
            else
            {
                try
                {
                    this.ShowAllChildWindow();
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo(string.Format("ShowAllChildWindow Exception: {0}", ex.Message));
                }

            }


            this.Text = string.Format("{0} {1} (版本号：{2}, 端口：{3})", ((角色)Program.Current平台用户.角色).ToString(), Program.Current平台用户.用户名, Program.Version, Program.Port);


            this.backgroundWorker报价.RunWorkerAsync();
        }

        private void TradeMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pnlWaiting.Visible = true;
            lblWaiting.Text = "正在退出系统……";
            
            Program.logger.LogRunning("正在关闭连接...");
            try
            {
                if (Program.AASServiceClient.State == CommunicationState.Opened)
                {
                    Program.AASServiceClient.Close();
                }
            }
            catch(Exception ex)
            {
                Program.logger.LogRunning(ex.Message);
            }


            Program.logger.LogRunning("正在保存布局文件...");
            if (this.hqFormCount == (int)decimal.Parse(Program.accountDataSet.参数.GetParaValue("报价窗口数目", "4")))
            {
                this.dockPanel1.SaveAsXml(this.uiFile);

                Program.accountDataSet.参数.SetParaValue("主窗体位置", string.Format("{0}|{1}|{2}|{3}", this.Location.X, this.Location.Y, this.Width, this.Height));

            }
            else
            {
                File.Delete(this.uiFile);

                Program.accountDataSet.参数.DeletePara("主窗体位置");
            }

            Program.logger.LogRunning("正在停止线程...");
            this.backgroundWorker报价.CancelAsync();
            this.backgroundWorker报价.RunWorkerCompleted -= this.BackgroundWorker报价_RunWorkerCompleted;

            for (int i = 0; i < 10; i++)
            {
                if (this.backgroundWorker报价.IsBusy)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }
                else
                {
                    break;
                }
            }

            Program.logger.LogRunning("程序已退出");
        }

        void TradeMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(HqForm).ToString())
            {
                return this.hqForm[this.hqFormIndex++];
            }
            else if (persistString == typeof(LogForm).ToString())
            {
                return this.logForm;
            }
            else if (persistString == typeof(CancelWTForm).ToString())
            {
                return this.cancelWTForm;
            }
            else if (persistString == typeof(CJForm).ToString())
            {
                return this.cJForm;
            }
            else if (persistString == typeof(WTForm).ToString())
            {
                return this.wtForm;
            }
            else if (persistString == typeof(OpenTradeForm).ToString())
            {
                return this.openTradeForm;
            }
            else if (persistString == typeof(ClosedTradeForm).ToString())
            {
                return this.closedTradeForm;
            }
            else if (persistString == typeof(业绩统计Form).ToString())
            {
                return this.交易统计Form;
            }
            else
            {
                return null;
            }
        }

        private void backgroundWorker报价_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!this.backgroundWorker报价.CancellationPending)
            {
                this.backgroundWorker报价.ReportProgress(0);
                
                try
                {
                    if (DateTime.Now >= this.LastFetchDay.AddDays(1).AddHours(1))
                    {
                        Program.AASServiceClient.FectchAllTable(Program.Current平台用户.用户名);
                        this.LastFetchDay = DateTime.Today;
                    }
                    
                    成交DataTableChanged 成交DataTableChanged1;
                    if (Program.成交表通知.TryDequeue(out 成交DataTableChanged1))
                    {
                        AASClient.AASServiceReference.JyDataSet.成交DataTable 成交DataTable1 = 成交DataTableChanged1.TableChanged as AASClient.AASServiceReference.JyDataSet.成交DataTable;
                        this.backgroundWorker报价.ReportProgress(4, 成交DataTable1);
                    }


                    委托DataTableChanged 委托DataTableChanged1;
                    if (Program.委托表通知.TryDequeue(out 委托DataTableChanged1))
                    {
                        AASClient.AASServiceReference.JyDataSet.委托DataTable 委托DataTable1 = 委托DataTableChanged1.TableChanged as AASClient.AASServiceReference.JyDataSet.委托DataTable;
                        this.backgroundWorker报价.ReportProgress(5, 委托DataTable1);
                    }

                    平台用户DataTableChanged 平台用户DataTableChanged1;
                    if (Program.平台用户表通知.TryDequeue(out 平台用户DataTableChanged1))
                    {
                        AASClient.AASServiceReference.DbDataSet.平台用户DataTable 平台用户DataTable1 = 平台用户DataTableChanged1.TableChanged as AASClient.AASServiceReference.DbDataSet.平台用户DataTable;
                        this.backgroundWorker报价.ReportProgress(1, 平台用户DataTable1);
                    }

                    额度分配DataTableChanged 额度分配DataTableChanged1;
                    if (Program.额度分配表通知.TryDequeue(out 额度分配DataTableChanged1))
                    {
                        AASClient.AASServiceReference.DbDataSet.额度分配DataTable 额度分配DataTable1 = 额度分配DataTableChanged1.TableChanged as AASClient.AASServiceReference.DbDataSet.额度分配DataTable;
                        this.backgroundWorker报价.ReportProgress(2, 额度分配DataTable1);
                    }

                    订单DataTableChanged 订单DataTableChanged1;
                    if (Program.订单表通知.TryDequeue(out 订单DataTableChanged1))
                    {
                        AASClient.AASServiceReference.DbDataSet.订单DataTable 订单DataTable1 = 订单DataTableChanged1.TableChanged as AASClient.AASServiceReference.DbDataSet.订单DataTable;
                        this.backgroundWorker报价.ReportProgress(6, 订单DataTable1);
                    }

                    已平仓订单DataTableChanged 已平仓订单DataTableChanged1;
                    if (Program.已平仓订单表通知.TryDequeue(out 已平仓订单DataTableChanged1))
                    {
                        AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable 已平仓订单DataTable1 = 已平仓订单DataTableChanged1.TableChanged as AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable;
                        this.backgroundWorker报价.ReportProgress(7, 已平仓订单DataTable1);
                    }


                  



                    Notify Notify1;
                    if (Program.交易通知.TryDequeue(out Notify1))
                    {
                        Program.logger.LogJy(Notify1.操作员, Notify1.证券代码, Notify1.证券名称, Notify1.委托编号, Notify1.买卖方向, Notify1.委托数量, Notify1.委托价格, Notify1.信息);
                    }




                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("报价线程异常:{0} {1}", ex.Message, ex.StackTrace);

                    Thread.Sleep(1000);
                }


               
            }
        }

        private void backgroundWorker报价_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            switch (e.ProgressPercentage)
            {
                case 0:
                    this.toolStripStatusLabel时间.Text = DateTime.Now.ToString("HH:mm:ss");
                    break;
                case 1:
                    AASClient.AASServiceReference.DbDataSet.平台用户DataTable 平台用户DataTable1 = e.UserState as AASClient.AASServiceReference.DbDataSet.平台用户DataTable;
                    Tool.RefreshDrcjDataTable(Program.serverDb.平台用户, 平台用户DataTable1, new string[] { "用户名" });
                    Program.Current平台用户 = Program.serverDb.平台用户.First(r => r.用户名 == Program.Current平台用户.用户名);
                    break;
                case 2:
                    AASClient.AASServiceReference.DbDataSet.额度分配DataTable TradeLimitDataTable = e.UserState as AASClient.AASServiceReference.DbDataSet.额度分配DataTable;
                    Tool.RefreshDrcjDataTable(Program.serverDb.额度分配, TradeLimitDataTable, new string[] { "证券代码" });

                    foreach (HqForm HqForm1 in this.hqForm)
                    {
                        HqForm1.comboBox代码.AutoCompleteCustomSource.Clear();

                        foreach (AASClient.AASServiceReference.DbDataSet.额度分配Row 交易额度Row1 in Program.serverDb.额度分配)
                        {
                            HqForm1.comboBox代码.AutoCompleteCustomSource.Add(string.Format("{0}", 交易额度Row1.证券代码));
                        }
                    }
                    break;
                case 3:
                  
                    break;
                case 4:
                    AASClient.AASServiceReference.JyDataSet.成交DataTable 成交DataTable1 = e.UserState as AASClient.AASServiceReference.JyDataSet.成交DataTable;
                    Tool.RefreshDrcjDataTable(Program.jyDataSet.成交, 成交DataTable1, new string[] { "组合号", "委托编号", "成交编号" });
                    break;
                case 5:
                    RefreshOrder(e);
                    
                    break;
                case 6:
                    AASClient.AASServiceReference.DbDataSet.订单DataTable 订单DataTable1 = e.UserState as AASClient.AASServiceReference.DbDataSet.订单DataTable;
                    foreach (AASClient.AASServiceReference.DbDataSet.订单Row 订单Row1 in 订单DataTable1)
                    {
                        if (Program.HqDataTable.ContainsKey(订单Row1.证券代码))
                        {
                            DataTable DataTable1 = Program.HqDataTable[订单Row1.证券代码];
                            DataRow DataRow1 = DataTable1.Rows[0];
                            decimal XJ = decimal.Parse((DataRow1["现价"] as string));
                            decimal ZS = decimal.Parse((DataRow1["昨收"] as string));
                            订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), 8, MidpointRounding.AwayFromZero);


                            //订单Row1.刷新浮动盈亏(Program.Current平台用户.手续费率);
                            订单Row1.刷新浮动盈亏();
                        }
                    }
                    Tool.RefreshDrcjDataTable(Program.serverDb.订单, 订单DataTable1, new string[] { "组合号", "证券代码" });
                    break;
                case 7:
                    AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable 已平仓订单DataTable1 = e.UserState as AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable;
                    Program.serverDb.已平仓订单.Clear();
                    Program.serverDb.已平仓订单.Merge(已平仓订单DataTable1);

                       
                    AASClient.AASServiceReference.JyDataSet.业绩统计DataTable 业绩统计DataTable2 = this.Generate业绩统计();
                    Tool.RefreshDrcjDataTable(Program.jyDataSet.业绩统计, 业绩统计DataTable2, new string[] { "证券代码" });
                    break;

                default:
                    break;
            }
        }

        private void RefreshOrder(ProgressChangedEventArgs e)
        {
            AASClient.AASServiceReference.JyDataSet.委托DataTable 委托DataTable1 = e.UserState as AASClient.AASServiceReference.JyDataSet.委托DataTable;
            

            if (委托DataTable1 == null)
            {
                Program.logger.LogRunning("委托更新错误，委托列表为空，不能执行更新！");
                return;
            }

            //List<string> lstID = new List<string>();
            Dictionary<string, List<string>> dictOrder = new Dictionary<string, List<string>>();
            var newDt = 委托DataTable1.Copy();
            newDt.Clear();
            for (int i = 0; i < 委托DataTable1.Rows.Count; i++)
            {
                var row = 委托DataTable1.Rows[i];

                var orderID = row["委托编号"] as string;
                var groupID = row["组合号"] as string;
                if (dictOrder.ContainsKey(groupID) && dictOrder[groupID].Contains(orderID))
                {
                    continue;
                }
                else
                {
                    if (dictOrder.ContainsKey(groupID))
                    {
                        dictOrder[groupID].Add(orderID);
                    }
                    else
                    {
                        dictOrder.Add(groupID, new List<string>() { orderID });
                    }
                }

                newDt.ImportRow(row);
            }

            Tool.RefreshDrcjDataTable(Program.jyDataSet.委托, newDt, new string[] { "组合号", "委托编号" });

            AASClient.AASServiceReference.JyDataSet.业绩统计DataTable 业绩统计DataTable1 = this.Generate业绩统计();
            Tool.RefreshDrcjDataTable(Program.jyDataSet.业绩统计, 业绩统计DataTable1, new string[] { "证券代码" });
        }


       

        private void BackgroundWorker报价_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("报价线程异常结束:" + e.Error.Message + "\r\n" + e.Error.StackTrace);
            }
        }



        public AASClient.AASServiceReference.JyDataSet.业绩统计DataTable Generate业绩统计()
        {
            AASClient.AASServiceReference.JyDataSet.业绩统计DataTable 业绩统计DataTable1 = new AASClient.AASServiceReference.JyDataSet.业绩统计DataTable();
            foreach (AASClient.AASServiceReference.JyDataSet.委托Row 委托Row1 in Program.jyDataSet.委托.Where(r => r.成交数量 > 0))
            {
                decimal 交易费用 = 委托Row1.Get交易费用();

                if (!业绩统计DataTable1.Any(r => r.证券代码 == 委托Row1.证券代码))
                {
                    #region 生成业绩统计Row
                    AASClient.AASServiceReference.JyDataSet.业绩统计Row 业绩统计RowNew = 业绩统计DataTable1.New业绩统计Row();
                    业绩统计RowNew.交易员 = 委托Row1.交易员;
                    业绩统计RowNew.组合号 = 委托Row1.组合号;
                    业绩统计RowNew.证券代码 = 委托Row1.证券代码;
                    业绩统计RowNew.证券名称 = 委托Row1.证券名称;



                    if (委托Row1.买卖方向 == 0)
                    {
                        #region 买单
                        业绩统计RowNew.买入数量 = 委托Row1.成交数量;
                        业绩统计RowNew.买入金额 = 委托Row1.成交价格 * 委托Row1.成交数量;
                        业绩统计RowNew.买入均价 = 委托Row1.成交价格;


                        业绩统计RowNew.卖出数量 = 0;
                        业绩统计RowNew.卖出金额 = 0;
                        业绩统计RowNew.卖出均价 = 0;

                        #endregion

                    }
                    else
                    {
                        #region 卖单
                        业绩统计RowNew.买入数量 = 0;
                        业绩统计RowNew.买入金额 = 0;
                        业绩统计RowNew.买入均价 = 0;

                        业绩统计RowNew.卖出数量 = 委托Row1.成交数量;
                        业绩统计RowNew.卖出金额 = 委托Row1.成交价格 * 委托Row1.成交数量;
                        业绩统计RowNew.卖出均价 = 委托Row1.成交价格;


                        #endregion
                    }


                    业绩统计RowNew.毛利 = 0;
                    业绩统计RowNew.交易费用 = 交易费用;

                    业绩统计RowNew.净利润 = 业绩统计RowNew.毛利 - 业绩统计RowNew.交易费用;
                    业绩统计DataTable1.Add业绩统计Row(业绩统计RowNew);

                    #endregion
                }
                else
                {
                    #region 修改业绩统计Row
                    AASClient.AASServiceReference.JyDataSet.业绩统计Row 业绩统计Row1 = 业绩统计DataTable1.First(r => r.证券代码 == 委托Row1.证券代码);


                    if (委托Row1.买卖方向 == 0)
                    {
                        #region 买单
                        业绩统计Row1.买入数量 += 委托Row1.成交数量;
                        业绩统计Row1.买入金额 += 委托Row1.成交价格 * 委托Row1.成交数量;
                        业绩统计Row1.买入均价 = Math.Round(业绩统计Row1.买入金额 / 业绩统计Row1.买入数量, 3, MidpointRounding.AwayFromZero);
                        #endregion
                    }
                    else
                    {
                        #region 卖单
                        业绩统计Row1.卖出数量 += 委托Row1.成交数量;
                        业绩统计Row1.卖出金额 += 委托Row1.成交价格 * 委托Row1.成交数量;
                        业绩统计Row1.卖出均价 = Math.Round(业绩统计Row1.卖出金额 / 业绩统计Row1.卖出数量, 3, MidpointRounding.AwayFromZero);
                        #endregion
                    }



                    业绩统计Row1.毛利 += 0;
                    业绩统计Row1.交易费用 += 交易费用;
                    业绩统计Row1.净利润 = 业绩统计Row1.毛利 - 业绩统计Row1.交易费用;
                    #endregion
                }
            }



            foreach (AASClient.AASServiceReference.JyDataSet.业绩统计Row 业绩统计Row1 in 业绩统计DataTable1)
            {
                if (Program.serverDb.已平仓订单.Any(r => r.证券代码 == 业绩统计Row1.证券代码))
                {
                    业绩统计Row1.毛利 = Program.serverDb.已平仓订单.Where(r => r.证券代码 == 业绩统计Row1.证券代码).Sum(r => r.毛利);
                    业绩统计Row1.净利润 = 业绩统计Row1.毛利 - 业绩统计Row1.交易费用;
                }
            }


            return 业绩统计DataTable1;
        }
        
        private void 显示所有窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowAllChildWindow();
        }

        private void ShowAllChildWindow()
        {
            Program.logger.LogRunning("1.ShowAllChildWindow Start……");
            this.logForm.Show(this.dockPanel1);
            Program.logger.LogRunning("2.logForm Show Finished");

            
            foreach (HqForm HqForm1 in this.hqForm)
            {
                HqForm1.Show(this.dockPanel1);
            }
            Program.logger.LogRunning("3.HqForm Show Finished");

            this.openTradeForm.Show(this.dockPanel1);
            Program.logger.LogRunning("4.openTradeForm Show Finished");

            this.closedTradeForm.Show(this.dockPanel1);
            Program.logger.LogRunning("5.closedTradeForm Show Finished");

            this.cancelWTForm.Show(this.dockPanel1);
            Program.logger.LogRunning("6.cancelWTForm Show Finished");

            this.cJForm.Show(this.dockPanel1);
            Program.logger.LogRunning("7.cJForm Show Finished");

            this.wtForm.Show(this.dockPanel1);
            Program.logger.LogRunning("8.wtForm Show Finished");

            this.交易统计Form.Show(this.dockPanel1);
            Program.logger.LogRunning("9.交易统计Form Show Finished");

        }
        
        private void 行情服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetHqServerForm SetHqServerForm1 = new SetHqServerForm();
            SetHqServerForm1.ShowDialog();
        }

        private void 交易额度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TradeLimitForm TradeLimitForm1 = new TradeLimitForm();
            //TradeLimitForm1.ShowDialog();
            TradeLimitForm1.Show(this);
        }

        private void 交易快捷键ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetShortcutForm SetShortcutForm1 = new SetShortcutForm();
            SetShortcutForm1.ShowDialog();
        }

        private void 登录密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifyPasswordForm ModifyPasswordForm1 = new ModifyPasswordForm();
            ModifyPasswordForm1.ShowDialog();
        }

        private void 报价窗口数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetHqFormCount SetHqFormCount1 = new SetHqFormCount();
            SetHqFormCount1.ShowDialog();
        }

        private void 价格提示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            RemindPriceForm RemindPriceForm1 = new RemindPriceForm();
            RemindPriceForm1.ShowDialog();
            this.timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            foreach(AASClient.AccountDataSet.价格提示Row 价格提示Row1 in Program.accountDataSet.价格提示)
            {
                if (价格提示Row1.启用)
                {
                    if (Program.HqDataTable.ContainsKey(价格提示Row1.证券代码))
                    {
                        DataTable DataTable1 = Program.HqDataTable[价格提示Row1.证券代码];
                        DataRow DataRow1 = DataTable1.Rows[0];
                        decimal Price = decimal.Parse((DataRow1["现价"] as string));
                        if ((价格提示Row1.提示类型 == (int)提示类型.涨到 && Price >= 价格提示Row1.提示价格) ||
                            (价格提示Row1.提示类型 == (int)提示类型.跌到 && Price != 0 && Price <= 价格提示Row1.提示价格))
                        {
                            PopupForm PopupForm1 = new PopupForm(价格提示Row1, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 400, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100 - PopupForm.PopupFormCount * 90);

                            PopupForm1.Show();
                           

                            价格提示Row1.启用 = false;
                        }
                    }
                }
            }

            if ((DateTime.Now - lastFetchCoinInfo).TotalSeconds > 3 && ! isQueryCoin)
            {
                isQueryCoin = true;
                RefreshCoinQty();
            }
        }

        public void RefreshCoinQty()
        {
            lastFetchCoinInfo = DateTime.Now;
            Task.Run(() =>
            {
                List<string> coins = new List<string>(hqForm.Length * 2) { "BNB", "BTC", "ETH", "USDT" };
                foreach (var item in hqForm)
                {
                    var kv = MarketAdapter.BinanceUtils.GetBinanceCoinInfo(item.Zqdm);
                    coins.Add(kv.Key);
                }

                try
                {
                    var arrResult = Program.AASServiceClient.QueryAccountCoin(Program.Current平台用户.用户名, coins.Distinct().ToArray());
                    if (arrResult != null && arrResult.Length > 0)
                    {
                        foreach (var item in arrResult)
                        {
                            dictAccountCoin[item.Assetk__BackingField] = item;
                        }
                    }
                }
                catch (Exception) { }
                isQueryCoin = false;
            });
        }

        private void 界面设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = false;
            //RemindPriceForm RemindPriceForm1 = new RemindPriceForm();
            //RemindPriceForm1.ShowDialog();
            var uiSettingForm = new UISettingForm();
            uiSettingForm.ShowDialog();
            this.timer1.Enabled = true;
        }

        private void 订阅模式设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SubConfigForm();
            form.ShowDialog();
        }

        private void 价格提示设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new PrewarningListForm();
            frm.ShowDialog();
        }

        private void TimerTestConnect_Tick(object sender, EventArgs e)
        {
            Task.Run(() => {
                try
                {
                    string status = "";

                    if (Program.AASServiceClient.State == CommunicationState.Opened)
                    {
                        if (Program.AASServiceClient.QuerySingleUser(Program.Version) != null)
                        {
                            status = "连接状态：已连接";
                            ErrorTime = DateTime.MinValue;
                        }
                        else
                        {
                            status = "连接状态：连接异常，开始自动连接";
                            Program.AutoReLogin();
                        }
                    }
                    else if (Program.AASServiceClient.State == CommunicationState.Created)
                    {
                        ErrorTime = DateTime.MinValue;
                        status = "连接状态：已连接";
                        Program.Current平台用户 = Program.AASServiceClient.QuerySingleUser(Program.Version)[0];
                    }
                    else
                    {
                        status = "连接状态：未连接";
                        Program.AutoReLogin();
                    }
                    this.Invoke(new Action(() => { toolStripStatusLabelConnect.Text = status; }));
                    
                }
                catch (Exception ex)
                {
                    if (!this.IsDisposed)
                    {
                        Program.logger.LogRunning("连接出错，出错信息：{0}", ex.Message);
                        this.Invoke(new Action(() => { toolStripStatusLabelConnect.Text = "连接状态：连接出错"; }));
                    }
                }
            });
        }

        private void 共享额度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var shareLimitWin = new ShareLimitListForm();
            shareLimitWin.Show();
        }


    }
}
