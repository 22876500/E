using AASClient.AASServiceReference;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class RiskControlMainForm : Form
    {
        private delegate void FlushClient();//代理

        DateTime LastFetchDay = DateTime.MinValue.Date;
        private ShareLimitGroupItem[] ShareLimitGroups = null;

        public RiskControlMainForm()
        {
            InitializeComponent();
            tabControlMain.TabPages.Remove(tabPage额度共享);
        }



        private void RiskControlMainForm_Load(object sender, EventArgs e)
        {
            var uri = Program.AASServiceClient.Endpoint.Address.Uri;
            string serverName = "";
            foreach (var item in LoginForm.ServerIPDict)
            {
                if (item.Value.Contains(uri.Host))
                {
                    serverName = item.Key;
                }
            }

            this.Text = string.Format("{0} {1} {2} ", ((角色)Program.Current平台用户.角色).ToString(), Program.Current平台用户.用户名, serverName);

            this.bindingSource运行日志.DataSource = Program.uiDataSet;
            this.bindingSource运行日志.DataMember = "运行日志";
            this.dataGridView运行日志.DataSource = this.bindingSource运行日志;

            this.dataGridView运行日志.Columns["时间"].Width = 70;
            this.dataGridView运行日志.Columns["时间"].DefaultCellStyle.Format = "HH:mm:ss";
            this.dataGridView运行日志.Columns["信息"].Width = 540;



            //----------------------------------------


            this.dataGridView交易员.AutoGenerateColumns = false;

            this.bindingSource交易员.DataSource = Program.serverDb;
            this.bindingSource交易员.DataMember = "平台用户";
            this.dataGridView交易员.DataSource = this.bindingSource交易员;
            



            this.bindingSource交易额度.DataSource = Program.serverDb;
            this.bindingSource交易额度.DataMember = "额度分配";
            this.dataGridView交易额度.DataSource = this.bindingSource交易额度;
            this.dataGridView交易额度.Columns["拼音缩写"].Visible = false;
            //this.dataGridView交易额度.Columns["组合号"].Visible = false;
            this.dataGridView交易额度.Columns["市场"].Visible = false;
            this.dataGridView交易额度.Columns["买模式"].Visible = false;
            this.dataGridView交易额度.Columns["卖模式"].Visible = false;

            this.dataGridView交易额度.Columns.Add("已卖股数", "已卖股数");
            this.dataGridView交易额度.Columns.Add("可卖股数", "可卖股数");


            this.dataGridView交易额度.Columns["交易额度"].DefaultCellStyle.Format = "f0";
            this.dataGridView交易额度.Columns["已卖股数"].DefaultCellStyle.Format = "f0";
            this.dataGridView交易额度.Columns["可卖股数"].DefaultCellStyle.Format = "f0";


            //-----------------------------------------

            this.bindingSource当前仓位.DataSource = Program.serverDb.订单;
            //this.bindingSource当前仓位.DataMember = "订单";
            this.dataGridView当前仓位.DataSource = this.bindingSource当前仓位;
            //this.dataGridView当前仓位.Columns["组合号"].Visible = false;
            this.dataGridView当前仓位.Columns["市场代码"].Visible = false;
            this.dataGridView当前仓位.Columns["已开金额"].Visible = false;
            this.dataGridView当前仓位.Columns["平仓时间"].Visible = false;
            this.dataGridView当前仓位.Columns["平仓类别"].Visible = false;
            this.dataGridView当前仓位.Columns["已平数量"].Visible = false;
            this.dataGridView当前仓位.Columns["已平金额"].Visible = false;
            this.dataGridView当前仓位.Columns["平仓价位"].Visible = false;

            this.dataGridView当前仓位.Columns["开仓时间"].DefaultCellStyle.Format = "HH:mm:ss";
            this.dataGridView当前仓位.Columns["已开数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView当前仓位.Columns["已平数量"].DefaultCellStyle.Format = "f0";

            //-------------------------------------------------------------------
            this.bindingSource当前委托.DataSource = Program.jyDataSet;
            this.bindingSource当前委托.DataMember = "委托";
            this.bindingSource当前委托.Filter = "委托数量 > 成交数量+撤单数量";
            this.dataGridView当前委托.DataSource = this.bindingSource当前委托;


            this.dataGridView当前委托.Columns["市场代码"].Visible = false;
            this.dataGridView当前委托.Columns["委托时间"].DisplayIndex = 11;
            this.dataGridView当前委托.Columns["委托数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView当前委托.Columns["成交数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView当前委托.Columns["撤单数量"].DefaultCellStyle.Format = "f0";

            //---------------------------------------------------------------------
            //this.bindingSource当前成交.DataSource = Program.jyDataSet;
            //this.bindingSource当前成交.DataMember = "成交";
            //this.dataGridView当前成交.DataSource = this.bindingSource当前成交;
            //this.dataGridView当前成交.Columns["市场代码"].Visible = false;
            //this.dataGridView当前成交.Columns["成交时间"].DisplayIndex = 8;
            //this.dataGridView当前成交.Columns["成交数量"].DefaultCellStyle.Format = "f0";

            //----------------------------------------------------------------------
            //this.bindingSource当日统计.DataSource = Program.serverDb;
            //this.bindingSource当日统计.DataMember = "已平仓订单";
            //this.dataGridView当日平仓.DataSource = this.bindingSource当日统计;
            ////this.dataGridView当日平仓.Columns["组合号"].Visible = false;
            //this.dataGridView当日平仓.Columns["平仓类别"].Visible = false;
            //this.dataGridView当日平仓.Columns["开仓时间"].DefaultCellStyle.Format = "HH:mm:ss";
            //this.dataGridView当日平仓.Columns["平仓时间"].DefaultCellStyle.Format = "HH:mm:ss";

            //this.dataGridView当日平仓.Columns["已开数量"].DefaultCellStyle.Format = "f0";
            //this.dataGridView当日平仓.Columns["已平数量"].DefaultCellStyle.Format = "f0";

            //this.dataGridView当日平仓.Columns["毛利"].DefaultCellStyle.Format = "f2";
            //this.dataGridView当日平仓.Columns["已开金额"].DefaultCellStyle.Format = "f2";
            //this.dataGridView当日平仓.Columns["已平金额"].DefaultCellStyle.Format = "f2";
            //----------------------------------------------------------------------
            //this.bindingSource委托记录.DataSource = Program.jyDataSet;
            //this.bindingSource委托记录.DataMember = "委托";
            //this.dataGridView委托记录.DataSource = this.bindingSource委托记录;

            //this.dataGridView委托记录.Columns["市场代码"].Visible = false;
            //this.dataGridView委托记录.Columns["委托时间"].DisplayIndex = 11;
            //this.dataGridView委托记录.Columns["委托数量"].DefaultCellStyle.Format = "f0";
            //this.dataGridView委托记录.Columns["成交数量"].DefaultCellStyle.Format = "f0";
            //this.dataGridView委托记录.Columns["撤单数量"].DefaultCellStyle.Format = "f0";

            //--------------------------------------------------------------------

            this.dataGridView组合仓位.AutoGenerateColumns = false;
            this.bindingSource组合仓位.DataSource = Program.组合订单;
            this.dataGridView组合仓位.DataSource = this.bindingSource组合仓位;

            this.dataGridView组合仓位.Columns["Column5"].DefaultCellStyle.Format = "f0";//已开数量列

            //----------------------------------------------------------


            this.tabControl1_SelectedIndexChanged(null, null);


            Thread thread = new Thread(new ThreadStart(Refresh当前仓位)) { IsBackground = true };
            thread.Start();

            InitSync();

            //this.bindingSource额度共享股票.DataSource = null;
        }

        private void Refresh当前仓位()
        {
            Thread.Sleep(5000);
            while (true)
            {
                if (this.IsDisposed)
                {
                    break;
                }
                try
                {
                    if (DateTime.Now.Hour < 9 || DateTime.Now.Hour >= 15)
                    {
                        var dt = Program.AASServiceClient.QueryAll订单();
                        Tool.RefreshDrcjDataTable(Program.serverDb.订单, dt, new string[] { "交易员", "组合号", "证券代码" });

                        Thread.Sleep(60000);
                    }
                    else if(Program.AASServiceClient.State == CommunicationState.Opened)
                    {
                        var dt = Program.AASServiceClient.QueryAll订单();
                        dataGridView当前仓位.Invoke(new Action(() =>
                        {
                            Refresh订单UI(dt);
                        }));
                        Thread.Sleep(5000);
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("Refresh当前仓位 exception:" + ex.Message);
                }
            }
        }

        private static void Refresh订单UI(DbDataSet.订单DataTable dt)
        {
            if (dt != null && dt.Count > 0)
            {
                List<AASClient.AASServiceReference.DbDataSet.订单Row> lstRowsRemove = new List<DbDataSet.订单Row>();
                if (Program.serverDb.订单.Count > 0)
                {
                    foreach (var item in Program.serverDb.订单)
                    {
                        var sameKeyItem = dt.FirstOrDefault(_ => _.交易员 == item.交易员 && _.组合号 == item.组合号 && _.证券代码 == item.证券代码);
                        if (sameKeyItem == null)
                        {
                            lstRowsRemove.Add(item);
                        }
                        else
                        {
                            item.开仓价位 = sameKeyItem.开仓价位;
                            item.开仓类别 = sameKeyItem.开仓类别;
                            item.开仓时间 = sameKeyItem.开仓时间;
                            item.平仓价位 = sameKeyItem.平仓价位;
                            item.平仓类别 = sameKeyItem.平仓类别;
                            item.平仓时间 = sameKeyItem.平仓时间;
                            item.已开金额 = sameKeyItem.已开金额;
                            item.已开数量 = sameKeyItem.已开数量;
                            item.已平金额 = sameKeyItem.已平金额;
                            item.已平数量 = sameKeyItem.已平数量;
                            item.刷新浮动盈亏();
                            dt.Remove订单Row(sameKeyItem);
                        }
                    }
                    foreach (var item in lstRowsRemove)
                    {
                        Program.serverDb.订单.Remove订单Row(item);
                    }
                }

                foreach (var item in dt)
                {
                    Program.serverDb.订单.ImportRow(item);
                }

            }
            else if (Program.serverDb.订单 != null && Program.serverDb.订单.Count > 0)
            {
                Program.serverDb.订单.Rows.Clear();
            }
        }

        private void InitSync()
        {
            Task.Run(() => {
                while (Program.HqServer == null)
                {
                    try
                    {
                        Program.HqServer = Program.AASServiceClient.Get行情服务器();
                    }
                    catch (Exception) { }
                    Thread.Sleep(200);
                }
                if (this.IsDisposed)
                {
                    return;
                }

                this.BeginInvoke(new Action(() =>
                {
                    this.backgroundWorker报价.RunWorkerAsync();
                    if (TDFData.DataSourceConfig.IsUseTDXData)
                    {
                        this.backgroundWorker行情.RunWorkerAsync();
                    }
                    if (TDFData.DataSourceConfig.IsUseTDFData)
                    {
                        TDFData.DataCache.GetInstance().Start();
                    }
                    //InitShareLimit();
                }));
            });
        }

        private void InitShareLimit()
        {
            Task.Run(() =>
            {
                try
                {
                    var groups = Program.AASServiceClient.ShareGroupQuery();
                    if (groups != null && groups.Length > 0)
                    {
                        this.BeginInvoke(new Action(()=> {
                            comboBoxShareLimitGroup.Items.Clear();

                            groups.ToList().ForEach(_ => comboBoxShareLimitGroup.Items.Add(_.GroupName));
                            comboBoxShareLimitGroup.SelectedIndex = 0;

                            this.bindingSource额度共享股票.DataSource = groups.First().GroupStockList;
                            dataGridView额度分配股票.DataSource = bindingSource额度共享股票;
                            dataGridView额度分配股票.AutoGenerateColumns = false;

                            this.bindingSource额度共享交易员.DataSource = groups.First().GroupTraderList;
                            dataGridView额度分配交易员.DataSource = bindingSource额度共享交易员;
                            dataGridView额度分配交易员.AutoGenerateColumns = false;
                        }));
                        
                    }
                    ShareLimitGroups = groups;
                }
                catch (Exception) { }
            });
            
        }


        private void RiskControlMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.logger.LogRunning("正在关闭连接...");
            try
            {
                if (Program.AASServiceClient.State == CommunicationState.Opened)
                {
                    Program.AASServiceClient.Close();
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning(ex.Message);
            }




            Program.logger.LogRunning("正在停止线程...");
            this.backgroundWorker报价.CancelAsync();
            this.backgroundWorker行情.CancelAsync();
            while (this.backgroundWorker报价.IsBusy || this.backgroundWorker行情.IsBusy)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }



            Program.logger.LogRunning("程序已退出");
        }

        private void dataGridView交易员_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "角色")
            {
                int int1 = (int)e.Value;

                角色 角色1 = (角色)Enum.Parse(typeof(角色), int1.ToString(), false);
                e.Value = 角色1.ToString();
            }
        }




        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                        Program.AASServiceClient.Fectch名下交易员Table();//命令服务器发出风控分配表Changed
                        this.LastFetchDay = DateTime.Today;
                    }


                    风控分配DataTableChanged 风控分配DataTableChanged1;
                    if (Program.风控分配通知.TryDequeue(out 风控分配DataTableChanged1))
                    {
                        Program.交易员成交DataTable.Clear();
                        Program.交易员委托DataTable.Clear();
                        Program.交易员平台用户DataTable.Clear();
                        Program.交易员额度分配DataTable.Clear();
                        Program.交易员订单DataTable.Clear();
                        Program.交易员已平仓订单DataTable.Clear();



                        foreach (AASClient.AASServiceReference.DbDataSet.平台用户Row 交易员Row1 in 风控分配DataTableChanged1.名下交易员DataTable)
                        {
                            Program.交易员平台用户DataTable[交易员Row1.用户名] = new AASServiceReference.DbDataSet.平台用户DataTable();
                            Program.交易员平台用户DataTable[交易员Row1.用户名].ImportRow(交易员Row1);
                            Program.AASServiceClient.FectchAllTable(交易员Row1.用户名);
                        }

                    }










                    成交DataTableChanged 成交DataTableChanged1;
                    if (Program.成交表通知.TryDequeue(out 成交DataTableChanged1))
                    {
                        Program.交易员成交DataTable[成交DataTableChanged1.UserName] = 成交DataTableChanged1.TableChanged as AASClient.AASServiceReference.JyDataSet.成交DataTable;



                        AASClient.AASServiceReference.JyDataSet.成交DataTable 合并的成交DataTable = new JyDataSet.成交DataTable();
                        foreach (JyDataSet.成交DataTable 成交DataTable1 in Program.交易员成交DataTable.Values)
                        {
                            合并的成交DataTable.Merge(成交DataTable1);
                        }
                        this.backgroundWorker报价.ReportProgress(2, 合并的成交DataTable);
                    }






                    委托DataTableChanged 委托DataTableChanged1;
                    if (Program.委托表通知.TryDequeue(out 委托DataTableChanged1))
                    {
                        Program.交易员委托DataTable[委托DataTableChanged1.UserName] = 委托DataTableChanged1.TableChanged as AASClient.AASServiceReference.JyDataSet.委托DataTable;



                        AASClient.AASServiceReference.JyDataSet.委托DataTable 合并的委托DataTable = new JyDataSet.委托DataTable();
                        foreach (JyDataSet.委托DataTable 委托DataTable1 in Program.交易员委托DataTable.Values)
                        {
                            合并的委托DataTable.Merge(委托DataTable1);
                        }
                        this.backgroundWorker报价.ReportProgress(3, 合并的委托DataTable);
                    }







                    平台用户DataTableChanged 平台用户DataTableChanged1;
                    if (Program.平台用户表通知.TryDequeue(out 平台用户DataTableChanged1))
                    {
                        AASClient.AASServiceReference.DbDataSet.平台用户DataTable Recv平台用户DataTable = 平台用户DataTableChanged1.TableChanged as AASClient.AASServiceReference.DbDataSet.平台用户DataTable;


                        if (平台用户DataTableChanged1.UserName != Program.Current平台用户.用户名)
                        {
                            Program.交易员平台用户DataTable[平台用户DataTableChanged1.UserName] = Recv平台用户DataTable;



                            AASClient.AASServiceReference.DbDataSet.平台用户DataTable 合并的平台用户DataTable = new AASServiceReference.DbDataSet.平台用户DataTable();
                            foreach (AASClient.AASServiceReference.DbDataSet.平台用户DataTable 平台用户DataTable1 in Program.交易员平台用户DataTable.Values)
                            {
                                合并的平台用户DataTable.Merge(平台用户DataTable1);
                            }
                            this.backgroundWorker报价.ReportProgress(6, 合并的平台用户DataTable);
                        }
                        else
                        {
                            Program.Current平台用户 = Recv平台用户DataTable[0];
                        }
                    }






                    额度分配DataTableChanged 额度分配DataTableChanged1;
                    if (Program.额度分配表通知.TryDequeue(out 额度分配DataTableChanged1))
                    {
                        Program.交易员额度分配DataTable[额度分配DataTableChanged1.UserName] = 额度分配DataTableChanged1.TableChanged as AASClient.AASServiceReference.DbDataSet.额度分配DataTable;



                        AASClient.AASServiceReference.DbDataSet.额度分配DataTable 合并的额度分配DataTable = new AASServiceReference.DbDataSet.额度分配DataTable();
                        foreach (AASClient.AASServiceReference.DbDataSet.额度分配DataTable 额度分配DataTable1 in Program.交易员额度分配DataTable.Values)
                        {
                            合并的额度分配DataTable.Merge(额度分配DataTable1);
                        }
                        this.backgroundWorker报价.ReportProgress(7, 合并的额度分配DataTable);
                    }







                    订单DataTableChanged 订单DataTableChanged1;
                    if (Program.订单表通知.TryDequeue(out 订单DataTableChanged1))
                    {
                        Program.交易员订单DataTable[订单DataTableChanged1.UserName] = 订单DataTableChanged1.TableChanged as AASClient.AASServiceReference.DbDataSet.订单DataTable;


                        AASClient.AASServiceReference.DbDataSet.订单DataTable 合并的订单DataTable = new AASClient.AASServiceReference.DbDataSet.订单DataTable();
                        foreach (AASClient.AASServiceReference.DbDataSet.订单DataTable 订单DataTable1 in Program.交易员订单DataTable.Values)
                        {
                            合并的订单DataTable.Merge(订单DataTable1);
                        }
                        foreach (AASClient.AASServiceReference.DbDataSet.订单Row 订单Row1 in 合并的订单DataTable)
                        {
                            if (Program.HqDataTable.ContainsKey(订单Row1.证券代码))
                            {
                                DataTable DataTable1 = Program.HqDataTable[订单Row1.证券代码];
                                DataRow DataRow1 = DataTable1.Rows[0];
                                decimal XJ = decimal.Parse((DataRow1["现价"] as string));
                                decimal ZS = decimal.Parse((DataRow1["昨收"] as string));
                                订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), L2Api.Get精度(订单Row1.证券代码), MidpointRounding.AwayFromZero);

                                AASClient.AASServiceReference.DbDataSet.平台用户Row 订单交易员平台用户Row = Program.serverDb.平台用户.FirstOrDefault(r => r.用户名 == 订单Row1.交易员);

                                if (订单交易员平台用户Row != null)
                                {
                                    //订单Row1.刷新浮动盈亏(订单交易员平台用户Row.手续费率);
                                    订单Row1.刷新浮动盈亏();
                                }
                            }
                        }


                        this.backgroundWorker报价.ReportProgress(4, 合并的订单DataTable);


                        AASClient.AASServiceReference.DbDataSet.订单DataTable 组合订单DataTable = new AASClient.AASServiceReference.DbDataSet.订单DataTable();
                        foreach (AASClient.AASServiceReference.DbDataSet.订单Row 订单Row1 in 合并的订单DataTable)
                        {
                            if (!组合订单DataTable.Any(r => r.组合号 == 订单Row1.组合号 && r.证券代码 == 订单Row1.证券代码))
                            {
                                组合订单DataTable.ImportRow(订单Row1);
                            }
                            else
                            {
                                AASClient.AASServiceReference.DbDataSet.订单Row 订单Row2 = 组合订单DataTable.First(r => r.组合号 == 订单Row1.组合号 && r.证券代码 == 订单Row1.证券代码);
                                if (订单Row1.开仓类别 == 订单Row2.开仓类别)
                                {
                                    订单Row2.已开数量 += 订单Row1.已开数量;
                                    订单Row2.已开金额 += 订单Row1.已开金额;
                                    订单Row2.开仓价位 = Math.Round(订单Row2.已开金额 / 订单Row2.已开数量, 3, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    if (订单Row1.已开数量 < 订单Row2.已开数量)
                                    {
                                        订单Row2.已开数量 -= 订单Row1.已开数量;
                                        订单Row2.已开金额 -= 订单Row1.已开金额;
                                        订单Row2.开仓价位 = Math.Round(订单Row2.已开金额 / 订单Row2.已开数量, 3, MidpointRounding.AwayFromZero);
                                    }
                                    else if (订单Row1.已开数量 > 订单Row2.已开数量)
                                    {
                                        订单Row2.开仓类别 = 订单Row1.开仓类别;
                                        订单Row2.已开数量 = 订单Row1.已开数量 - 订单Row2.已开数量;
                                        订单Row2.已开金额 = 订单Row1.已开金额 - 订单Row2.已开金额;
                                        订单Row2.开仓价位 = Math.Round(订单Row2.已开金额 / 订单Row2.已开数量, 3, MidpointRounding.AwayFromZero);
                                    }
                                    else
                                    {
                                        组合订单DataTable.Remove订单Row(订单Row2);
                                    }
                                }
                            }
                        }
                        this.backgroundWorker报价.ReportProgress(8, 组合订单DataTable);
                    }






                    已平仓订单DataTableChanged 已平仓订单DataTableChanged1;
                    if (Program.已平仓订单表通知.TryDequeue(out 已平仓订单DataTableChanged1))
                    {
                        Program.交易员已平仓订单DataTable[已平仓订单DataTableChanged1.UserName] = 已平仓订单DataTableChanged1.TableChanged as AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable;

                        AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable 合并的已平仓订单DataTable = new AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable();
                        foreach (AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable 已平仓订单DataTable1 in Program.交易员已平仓订单DataTable.Values)
                        {
                            合并的已平仓订单DataTable.Merge(已平仓订单DataTable1);
                        }
                        this.backgroundWorker报价.ReportProgress(5, 合并的已平仓订单DataTable);
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("报价线程异常:{0}", ex.Message);

                    Thread.Sleep(1000);
                }
            }
        }

        #region backgroundWorker报价_ProgressChanged 更新
        private void backgroundWorker报价_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                switch (e.ProgressPercentage)
                {
                    case 0:
                        this.toolStripStatusLabel时间.Text = DateTime.Now.ToString("HH:mm:ss");
                        break;
                    case 2:
                        Refresh成交(e);
                        break;
                    case 3:
                        RefreshOrder(e);
                        break;
                    case 4:
                        Refresh订单(e);
                        break;
                    case 5:
                        Represh已平仓订单(e);
                        break;
                    case 6:
                        DbDataSet.平台用户DataTable 平台用户DataTable1 = e.UserState as DbDataSet.平台用户DataTable;
                        Tool.RefreshDrcjDataTable(Program.serverDb.平台用户, 平台用户DataTable1, new string[] { "用户名" });
                        break;
                    case 7:
                        AASClient.AASServiceReference.DbDataSet.额度分配DataTable TradeLimitDataTable = e.UserState as AASClient.AASServiceReference.DbDataSet.额度分配DataTable;
                        Tool.RefreshDrcjDataTable(Program.serverDb.额度分配, TradeLimitDataTable, new string[] { "交易员", "证券代码", "组合号" });
                        break;
                    case 8:
                        AASClient.AASServiceReference.DbDataSet.订单DataTable 组合订单DataTable1 = e.UserState as AASClient.AASServiceReference.DbDataSet.订单DataTable;
                        Tool.RefreshDrcjDataTable(Program.组合订单, 组合订单DataTable1, new string[] { "组合号", "证券代码" });
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("backgroundWorker报价_ProgressChanged Exception, ProgressPercentage {0}, Message {1}", e.ProgressPercentage, ex.Message );
            }
        }

        

        private void Represh已平仓订单(ProgressChangedEventArgs e)
        {
            DbDataSet.已平仓订单DataTable 已平仓订单DataTable1 = e.UserState as DbDataSet.已平仓订单DataTable;

            if (queueClosedTrades == null)
            {
                queueClosedTrades = new ConcurrentQueue<DbDataSet.已平仓订单DataTable>();
                var threadRefreshTrades = new Thread(new ThreadStart(TradesClosedUpdateMain)) { IsBackground = true };
                threadRefreshTrades.Start();
            }
            queueClosedTrades.Enqueue(已平仓订单DataTable1);
        }
        ConcurrentQueue<DbDataSet.已平仓订单DataTable> queueClosedTrades = null;
        private void TradesClosedUpdateMain()
        {
            while (true)
            {
                if (this.IsDisposed)
                {
                    break;
                }
                else
                {
                    try
                    {
                        DbDataSet.已平仓订单DataTable dt = null;
                        while (queueClosedTrades.Count > 0)
                        {
                            queueClosedTrades.TryDequeue(out dt);
                        }
                        if (dt != null)
                        {
                            Program.serverDb.已平仓订单.Rows.Clear();
                            Program.serverDb.已平仓订单.Merge(dt);
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfo("TradesClosedUpdateMain Exception: " + ex.Message);
                    }
                    Thread.Sleep(3000);
                }
            }
        }

        private void Refresh成交(ProgressChangedEventArgs e)
        {
            JyDataSet.成交DataTable 成交DataTable1 = e.UserState as JyDataSet.成交DataTable;

            if (queueTrades == null)
            {
                queueTrades = new ConcurrentQueue<JyDataSet.成交DataTable>();
                var TradeUpdateThread = new Thread(new ThreadStart(TradeUpdateMain)) { IsBackground = true };
                TradeUpdateThread.Start();
            }
            if (成交DataTable1 != null)
            {
                queueTrades.Enqueue(成交DataTable1);
            }
        }
        ConcurrentQueue<JyDataSet.成交DataTable> queueTrades = null;
        private void TradeUpdateMain()
        {
            while (true)
            {
                if (this.IsDisposed)
                {
                    break;
                }
                else
                {
                    try
                    {
                        JyDataSet.成交DataTable dt = null;
                        while (queueTrades.Count > 0)
                            queueTrades.TryDequeue(out dt);

                        if (dt != null)
                            Tool.RefreshDrcjDataTable(Program.jyDataSet.成交, dt, new string[] { "组合号", "委托编号", "成交编号" });
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfo("TradeUpdateMain Exception: " + ex.Message);
                    }
                    Thread.Sleep(3000);
                }
               
            }
        }

        private void RefreshOrder(ProgressChangedEventArgs e)
        {
            JyDataSet.委托DataTable 委托DataTable1 = e.UserState as JyDataSet.委托DataTable;

            Dictionary<string, List<string>> dictOrder = new Dictionary<string, List<string>>();

            var dt = 委托DataTable1.Copy() as JyDataSet.委托DataTable;
            dt.Clear();
            foreach (var item in 委托DataTable1)
            {
                if (dictOrder.ContainsKey(item.组合号))
                {
                    if (dictOrder[item.组合号].Contains(item.委托编号))
                    {
                        continue;
                    }
                    else
                    {
                        dictOrder[item.组合号].Add(item.委托编号);
                        dt.ImportRow(item);
                    }
                }
                else
                {
                    dictOrder.Add(item.组合号, new List<string>());
                    dictOrder[item.组合号].Add(item.委托编号);
                    dt.ImportRow(item);
                }
            }

            try
            {
                Tool.RefreshDrcjDataTable(Program.jyDataSet.委托, dt, new string[] { "组合号", "委托编号" });
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("backgroundWorker报价_ProgressChanged 委托更新异常，异常信息{0}", ex.Message);
            }

        }

        private void Refresh订单(ProgressChangedEventArgs e)
        {
            //var 订单DataTable1 = e.UserState as AASClient.AASServiceReference.DbDataSet.订单DataTable;
            //Tool.RefreshDrcjDataTable(Program.serverDb.订单, 订单DataTable1, new string[] { "交易员", "组合号", "证券代码" });
        }

        #endregion

        private void backgroundWorker报价_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("报价线程异常结束:" + e.Error.Message + "\r\n" + e.Error.StackTrace);
            }
        }

        private void dataGridView交易员_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView当前委托_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //撤单

            AASClient.AASServiceReference.JyDataSet.委托Row DataRow1 = (this.bindingSource当前委托.Current as DataRowView).Row as AASClient.AASServiceReference.JyDataSet.委托Row;



            string Ret = Program.AASServiceClient.CancelOrder(DataRow1.交易员, DataRow1.组合号, DataRow1.证券代码, DataRow1.证券名称, DataRow1.市场代码, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格);
            string[] Data = Ret.Split('|');
            if (Data[1] != string.Empty)
            {
                MessageBox.Show(Data[1], "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            MessageBox.Show("撤单成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }






        bool isOpen = false;
        private void backgroundWorker行情_DoWork(object sender, DoWorkEventArgs e)
        {

            StringBuilder Result = new StringBuilder(1024 * 1024);
            StringBuilder ErrInfo = new StringBuilder(256);

            //string[] N = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

            int HqServerIndex = int.Parse(Program.accountDataSet.参数.GetParaValue("行情服务器", "0"));
            bool IsConnected = false;

            while (!this.backgroundWorker行情.CancellationPending)
            {
                if (Program.HqServer == null)
                {
                    try
                    {
                        Program.HqServer = Program.AASServiceClient.Get行情服务器();
                    }
                    catch (Exception) { }
                    Thread.Sleep(1000);
                }
                if (TDFData.DataSourceConfig.IsUseTDFData && TDFData.DataCache.GetInstance().IsConnected)
                {
                    Thread.Sleep(5000);
                    continue;
                }
                if (!isOpen)
                {
                    isOpen = L2Api.OpenTdx(ErrInfo);
                    if (!isOpen)
                    {
                        Program.logger.LogInfo("OpenTdx Fail: " + ErrInfo.ToString());
                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        Program.logger.LogInfo("OpenTdx Success!");
                    }
                }

                if (!IsConnected)
                {
                    #region 连接
                    Program.logger.LogRunning("程序正在连接行情服务器...");
                    if (Program.HqServer == null)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    string HqServerString = Program.HqServer[HqServerIndex];
                    string[] HqServerInfo = HqServerString.Split(new char[] { ':' });

                    if (isOpen)
                    {
                        IsConnected = L2Api.TdxL2Hq_Connect(HqServerInfo[1], int.Parse(HqServerInfo[2]), Result, ErrInfo);
                    }


                    if (!IsConnected)
                    {
                        Program.logger.LogRunning("连接到{0}失败:{1}", HqServerString, ErrInfo);

                        HqServerIndex = (HqServerIndex + 1) % Program.HqServer.Length;
                    }
                    else
                    {
                        Program.logger.LogRunning(L2Api.ChangeDataTableToString(L2Api.ChangeDataStringToTable(Result.ToString())));
                    }
                    Thread.Sleep(1000);
                    #endregion
                }
                else
                {
                    #region 获取证券代码
                    List<string> 十档证券代码 = new List<string>();

                    try
                    {
                        var positions = Program.serverDb.订单.Select(_ => _.证券代码).ToList();
                        if (positions.Count > 0)
                        {
                            positions.ForEach(_ => { if (!十档证券代码.Contains(_)) 十档证券代码.Add(_); });
                        }
                    }
                    catch { }

                    if (Program.TempZqdm != null)
                    {
                        if (!十档证券代码.Contains(Program.TempZqdm))
                        {
                            十档证券代码.Add(Program.TempZqdm);
                        }
                    }

                    for (int i = 0; i < 十档证券代码.Count; i++)
                    {
                        string Zqdm1 = 十档证券代码[i];
                        if (Zqdm1.Length != 6)
                        {
                            continue;
                        }

                        short Count = 1;
                        IsConnected = L2Api.TdxL2Hq_GetSecurityQuotes(new byte[] { L2Api.GetMarket(Zqdm1) }, new string[] { Zqdm1 }, ref Count, Result, ErrInfo);
                        if (IsConnected && Count == 1)
                        {
                            Program.HqDataTable[Zqdm1] = L2Api.ChangeDataStringToTable(Result.ToString());
                        }
                        else
                        {
                            IsConnected = false;
                            Program.logger.LogRunning("获取{0}十档报价失败:{1}", Zqdm1, ErrInfo);
                        }
                    }
                    #endregion


                    Thread.Sleep(1000);
                }
            }


            if (IsConnected)
            {
                L2Api.TdxL2Hq_Disconnect();
            }
        }

        private void backgroundWorker行情_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker行情_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("行情线程异常结束:" + e.Error.Message + "\r\n" + e.Error.StackTrace);
            }
        }

        private void 行情服务器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetHqServerForm SetHqServerForm1 = new SetHqServerForm();
            SetHqServerForm1.ShowDialog();
        }

        private void dataGridView当前仓位_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e == null || e.ColumnIndex < 0)
            {
                return;
            }

            try
            {
                if (this.dataGridView当前仓位.Columns[e.ColumnIndex].Name == "开仓类别" || this.dataGridView当前仓位.Columns[e.ColumnIndex].Name == "平仓类别")
                {
                    int int1 = (int)e.Value;
                    switch (int1)
                    {
                        case 0://买入
                        case 2://融资买入
                        case 69:
                            e.Value = "多";
                            break;
                        case 1://卖出
                        case 3://融券卖出
                        case 70:
                            e.Value = "空";
                            break;
                        default:
                            e.Value = "X";
                            break;
                    }
                }
            }
            catch (Exception)
            {
                //Program.logger.LogRunning("dataGridView当前仓位_CellFormatting Exception, e.Value {0}, e.Value.Type, Info {1}", e.Value, e.Value.GetType().Name, ex.Message);
            }
        }



        private void dataGridView交易员_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AASClient.AASServiceReference.DbDataSet.平台用户Row DataRow1 = (this.bindingSource交易员.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.平台用户Row;


            this.bindingSource交易额度.Filter = string.Format("交易员='{0}'", DataRow1.用户名);
        }

        private void 显示全部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.bindingSource交易额度.Filter = null;
        }


        private void dataGridView交易额度_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //下单
            AASClient.AASServiceReference.DbDataSet.额度分配Row DataRow1 = (this.bindingSource交易额度.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.额度分配Row;
            string 交易员 = DataRow1.交易员;
            string 证券代码 = DataRow1.证券代码;
            string 证券名称 = DataRow1.证券名称;

            SendOrderForm SendOrderForm1 = new SendOrderForm(交易员, 证券代码, 证券名称);
            SendOrderForm1.ShowDialog();
        }

        private void 登录密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModifyPasswordForm ModifyPasswordForm1 = new ModifyPasswordForm();
            ModifyPasswordForm1.ShowDialog();
        }

        private void 刷新交易日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DbDataSet.交易日志DataTable dt = null;
            //Task.Run(() =>
            //{
            //    try
            //    {
            //        dt = Program.AASServiceClient.QueryJyLogBelongFK(Program.Current平台用户.用户名);
            //        if (dt != null && !this.IsDisposed)
            //        {
            //            this.Invoke(new Action(() =>
            //            {
            //                this.dataGridView交易日志.DataSource = dt;

            //                if (this.dataGridView交易日志.DataSource != null)
            //                {
            //                    this.dataGridView交易日志.Columns["日期"].Visible = false;
            //                    this.dataGridView交易日志.Columns["委托数量"].DefaultCellStyle.Format = "f0";
            //                }

            //                if (this.dataGridView交易日志.Rows.Count > 0)
            //                {
            //                    this.dataGridView交易日志.FirstDisplayedScrollingRowIndex = this.dataGridView交易日志.Rows.Count - 1;
            //                }
            //            }));
            //        }
            //    }
            //    catch (Exception) { }
            //});
        }

        private void button统计_Click(object sender, EventArgs e)
        {
            AASClient.AASServiceReference.JyDataSet.业绩统计DataTable 业绩统计DataTable1 = null;
            try
            {
                业绩统计DataTable1 = Program.AASServiceClient.Query业绩BelongFK(Program.Current平台用户.用户名, this.dateTimePicker开始日期.Value.Date, this.dateTimePicker结束日期.Value.Date);
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("风控统计业绩问题：{0}", ex.Message);
                //MessageBox.Show("出错了，请联系管理员！");
                return;
            }




            Dictionary<string, List<JyDataSet.业绩统计Row>> 业绩统计RowList = new Dictionary<string, List<JyDataSet.业绩统计Row>>();

            foreach (AASClient.AASServiceReference.JyDataSet.业绩统计Row 业绩统计Row1 in 业绩统计DataTable1)
            {
                string Key1 = this.radioButton按交易员.Checked ? 业绩统计Row1.交易员 : 业绩统计Row1.组合号;

                if (!业绩统计RowList.ContainsKey(Key1))
                {
                    业绩统计RowList[Key1] = new List<JyDataSet.业绩统计Row>();
                }

                业绩统计RowList[Key1].Add(业绩统计Row1);
            }






            AASClient.AASServiceReference.JyDataSet.业绩统计DataTable 业绩统计DataTable2 = new JyDataSet.业绩统计DataTable();

            foreach (string Key in 业绩统计RowList.Keys)
            {
                foreach (AASClient.AASServiceReference.JyDataSet.业绩统计Row 业绩统计Row1 in 业绩统计RowList[Key])
                {
                    业绩统计DataTable2.ImportRow(业绩统计Row1);
                }

                AASClient.AASServiceReference.JyDataSet.业绩统计Row 业绩统计Row2 = 业绩统计DataTable2.New业绩统计Row();
                业绩统计Row2.交易员 = "合计";
                业绩统计Row2.组合号 = string.Empty;
                业绩统计Row2.证券代码 = string.Empty;
                业绩统计Row2.证券名称 = string.Empty;
                业绩统计Row2.买入数量 = 业绩统计RowList[Key].Sum(r => r.买入数量);
                业绩统计Row2.买入金额 = 业绩统计RowList[Key].Sum(r => r.买入金额);
                业绩统计Row2.买入均价 = 业绩统计Row2.买入数量 != 0 ? Math.Round(业绩统计Row2.买入金额 / 业绩统计Row2.买入数量, 3, MidpointRounding.AwayFromZero) : 0;
                业绩统计Row2.卖出数量 = 业绩统计RowList[Key].Sum(r => r.卖出数量);
                业绩统计Row2.卖出金额 = 业绩统计RowList[Key].Sum(r => r.卖出金额);
                业绩统计Row2.卖出均价 = 业绩统计Row2.卖出数量 != 0 ? Math.Round(业绩统计Row2.卖出金额 / 业绩统计Row2.卖出数量, 3, MidpointRounding.AwayFromZero) : 0;
                业绩统计Row2.毛利 = 业绩统计RowList[Key].Sum(r => r.毛利);
                业绩统计Row2.净利润 = 业绩统计RowList[Key].Sum(r => r.净利润);
                业绩统计Row2.交易费用 = 业绩统计RowList[Key].Sum(r => r.交易费用);
                业绩统计DataTable2.Add业绩统计Row(业绩统计Row2);

            }






            if (业绩统计DataTable1.Any())
            {
                AASClient.AASServiceReference.JyDataSet.业绩统计Row 业绩统计Row2 = 业绩统计DataTable2.New业绩统计Row();
                业绩统计Row2.交易员 = "总计";
                业绩统计Row2.组合号 = string.Empty;
                业绩统计Row2.证券代码 = string.Empty;
                业绩统计Row2.证券名称 = string.Empty;
                业绩统计Row2.买入数量 = 业绩统计DataTable1.Sum(r => r.买入数量);
                业绩统计Row2.买入金额 = 业绩统计DataTable1.Sum(r => r.买入金额);
                业绩统计Row2.买入均价 = 业绩统计Row2.买入数量 != 0 ? Math.Round(业绩统计Row2.买入金额 / 业绩统计Row2.买入数量, 3, MidpointRounding.AwayFromZero) : 0;
                业绩统计Row2.卖出数量 = 业绩统计DataTable1.Sum(r => r.卖出数量);
                业绩统计Row2.卖出金额 = 业绩统计DataTable1.Sum(r => r.卖出金额);
                业绩统计Row2.卖出均价 = 业绩统计Row2.卖出数量 != 0 ? Math.Round(业绩统计Row2.卖出金额 / 业绩统计Row2.卖出数量, 3, MidpointRounding.AwayFromZero) : 0;
                业绩统计Row2.毛利 = 业绩统计DataTable1.Sum(r => r.毛利);
                业绩统计Row2.净利润 = 业绩统计DataTable1.Sum(r => r.净利润);
                业绩统计Row2.交易费用 = 业绩统计DataTable1.Sum(r => r.交易费用);
                业绩统计DataTable2.Add业绩统计Row(业绩统计Row2);
            }



            this.dataGridView业绩统计.DataSource = 业绩统计DataTable2;


            foreach (DataGridViewColumn DataGridViewColumn1 in this.dataGridView业绩统计.Columns)
            {

                DataGridViewColumn1.SortMode = DataGridViewColumnSortMode.NotSortable;
            }



            this.dataGridView业绩统计.Columns["买入数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView业绩统计.Columns["卖出数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView业绩统计.Columns["买入金额"].DefaultCellStyle.Format = "f2";
            this.dataGridView业绩统计.Columns["卖出金额"].DefaultCellStyle.Format = "f2";

        }

        private void dataGridView业绩统计_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            if (this.dataGridView业绩统计.Columns[e.ColumnIndex].Name == "开仓类别" || this.dataGridView业绩统计.Columns[e.ColumnIndex].Name == "平仓类别")
            {
                int int1 = (int)e.Value;
                switch (int1)
                {
                    case 0:
                        e.Value = "买";
                        break;
                    case 1:
                        e.Value = "卖";
                        break;
                    case 2:
                        e.Value = "融资买入";
                        break;
                    case 3:
                        e.Value = "融券卖出";
                        break;
                    default:
                        e.Value = "X";
                        break;
                }
            }
        }

        //private void dataGridView当日平仓_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (e.ColumnIndex < 0)
        //    {
        //        return;
        //    }

        //    if (this.dataGridView当日平仓.Columns[e.ColumnIndex].Name == "开仓类别" || this.dataGridView当日平仓.Columns[e.ColumnIndex].Name == "平仓类别")
        //    {
        //        int int1 = (int)e.Value;
        //        switch (int1)
        //        {
        //            case 0:
        //                e.Value = "多";
        //                break;
        //            case 1:
        //                e.Value = "空";
        //                break;
        //            case 2:
        //                e.Value = "融资买入";
        //                break;
        //            case 3:
        //                e.Value = "融券卖出";
        //                break;
        //            default:
        //                e.Value = "X";
        //                break;
        //        }
        //    }
        //}

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task.Run(() => {
                RefreshTrades();

                try
                {
                    decimal 当日委托交易费用 = 0;
                    decimal 毛利 = Program.serverDb.已平仓订单.Get毛利();

                    JyDataSet.委托DataTable wtCopy = Program.jyDataSet.委托.Copy() as JyDataSet.委托DataTable;
                    当日委托交易费用 = wtCopy.Where(r => r.成交数量 > 0).Sum(_ => _.Get交易费用());

                    if (!this.IsDisposed)
                    {
                        this.Invoke(new Action(() => { RefreshDataGridView(当日委托交易费用, 毛利, wtCopy); }));
                    }
                }
                catch (Exception) { }
            });
        }

        private void RefreshDataGridView(decimal 当日委托交易费用, decimal 毛利, JyDataSet.委托DataTable wtCopy)
        {
            try
            {
                foreach (DataGridViewRow 交易额度row in this.dataGridView交易额度.Rows)
                {
                    string Jyy = 交易额度row.Cells["交易员"].Value as string;
                    string Zqdm = 交易额度row.Cells["证券代码"].Value as string;
                    decimal 交易额度 = (decimal)交易额度row.Cells["交易额度"].Value;
                    decimal 已买股数, 已卖股数;

                    wtCopy.Get已买卖股数(Jyy, Zqdm, out 已买股数, out 已卖股数);

                    decimal 可用股数 = 交易额度 - 已卖股数;
                    交易额度row.Cells["已卖股数"].Value = 已卖股数;
                    交易额度row.Cells["可卖股数"].Value = 可用股数;
                }

                //this.label市值合计.Text = Program.serverDb.订单.Get市值合计().ToString();
                //this.label浮动盈亏.Text = Program.serverDb.订单.Get浮动盈亏().ToString();
                //this.label实现盈亏.Text = (毛利 - 当日委托交易费用).ToString();
            }
            catch (Exception) { }
            
        }

        private static void RefreshTrades()
        {
            foreach (AASClient.AASServiceReference.DbDataSet.订单Row 订单Row1 in Program.serverDb.订单)
            {
                try
                {
                    decimal XJ, ZS;

                    if (TDFData.DataSourceConfig.IsUseTDFData && TDFData.DataCache.GetInstance().IsConnected)
                    {
                        if (TDFData.DataCache.GetInstance().MarketNewDict.ContainsKey(订单Row1.证券代码))
                        {
                            var marketData = TDFData.DataCache.GetInstance().MarketNewDict[订单Row1.证券代码];
                            XJ = (decimal)marketData.Match / 10000;
                            ZS = (decimal)marketData.PreClose / 10000;
                            订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), L2Api.Get精度(订单Row1.证券代码), MidpointRounding.AwayFromZero);
                            订单Row1.刷新浮动盈亏();
                        }
                        else if (TDFData.DataSourceConfig.IsUseTDFData)
                        {
                            if (TDFData.DataCache.GetInstance().UpdateMarketData == null)
                                TDFData.DataCache.GetInstance().UpdateMarketData += (obj) => { };
                            TDFData.DataCache.GetInstance().AddSub(订单Row1.证券代码);
                        }
                    }
                    else if (TDFData.DataSourceConfig.IsUseTDXData && Program.HqDataTable.ContainsKey(订单Row1.证券代码))
                    {
                        DataTable DataTable1 = Program.HqDataTable[订单Row1.证券代码];
                        DataRow DataRow1 = DataTable1.Rows[0];
                        XJ = decimal.Parse((DataRow1["现价"] as string));
                        ZS = decimal.Parse((DataRow1["昨收"] as string));
                        订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), L2Api.Get精度(订单Row1.证券代码), MidpointRounding.AwayFromZero);

                        AASClient.AASServiceReference.DbDataSet.平台用户Row 订单交易员平台用户Row = Program.serverDb.平台用户.FirstOrDefault(r => r.用户名 == 订单Row1.交易员);

                        if (订单交易员平台用户Row != null)
                            订单Row1.刷新浮动盈亏();
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("RefreshTrades Exception, Info {0}", ex.Message);
                }
            }
        }

        private void dataGridView业绩统计_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            string fdyk = (string)this.dataGridView业绩统计["交易员", e.RowIndex].Value;

            if (fdyk == "合计")
            {
                this.dataGridView业绩统计.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
            }
            else if (fdyk == "总计")
            {
                this.dataGridView业绩统计.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Gold;
            }

        }

        private void dataGridView当前仓位_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!Program.Current平台用户.允许删除碎股订单)
            {
                MessageBox.Show("无权限平仓碎股");
                return;
            }



            AASClient.AASServiceReference.DbDataSet.订单Row 订单Row1 = (this.bindingSource当前仓位.Current as DataRowView).Row as AASClient.AASServiceReference.DbDataSet.订单Row;

            //if (订单Row1.已开数量 < 100)
            //{
            CloseOrderForm CloseOrderForm1 = new CloseOrderForm(订单Row1);
            CloseOrderForm1.ShowDialog();
            // }

        }

        private void dataGridView交易额度_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "市场")
            {
                byte byte1 = (byte)e.Value;


                e.Value = byte1 == 0 ? "深圳" : "上海";
            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "买模式")
            {
                买模式 买模式1 = (买模式)e.Value;
                e.Value = 买模式1.ToString();
            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "卖模式")
            {
                卖模式 卖模式1 = (卖模式)e.Value;
                e.Value = 卖模式1.ToString();
            }
        }

        private void dataGridView组合仓位_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            if (this.dataGridView组合仓位.Columns[e.ColumnIndex].HeaderText == "开仓类别" || this.dataGridView组合仓位.Columns[e.ColumnIndex].HeaderText == "平仓类别")
            {
                int int1 = (int)e.Value;
                switch (int1)
                {
                    case 0:
                    case 2:
                        e.Value = "多";
                        break;
                    case 1:
                    case 3:
                        e.Value = "空";
                        break;
                    default:
                        e.Value = "X";
                        break;
                }
            }

        }


        private void dataGridView当前仓位_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                decimal fdyk = (decimal)this.dataGridView当前仓位["浮动盈亏", e.RowIndex].Value;

                if (fdyk > 0)
                {
                    this.dataGridView当前仓位.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                }
                else if (fdyk < 0)
                {
                    this.dataGridView当前仓位.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    this.dataGridView当前仓位.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                }
            }
            catch (Exception)
            {
                //Program.logger.LogRunning("当前仓位界面PrePaint异常，异常信息 {0}", ex.Message);
            }

        }

        private void dataGridView当前委托_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int mmfx = (int)this.dataGridView当前委托["买卖方向", e.RowIndex].Value;
            if (mmfx == 0)
            {
                this.dataGridView当前委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
            else if (mmfx == 1)
            {
                this.dataGridView当前委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.dataGridView当前委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void dataGridView当前成交_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            //int mmfx = (int)this.dataGridView当前成交["买卖方向", e.RowIndex].Value;
            //if (mmfx == 0)
            //{
            //    this.dataGridView当前成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            //}
            //else if (mmfx == 1)
            //{
            //    this.dataGridView当前成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            //}
            //else
            //{
            //    this.dataGridView当前成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            //}
        }

        //private void dataGridView委托记录_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        //{
        //    try
        //    {
        //        int mmfx = (int)this.dataGridView委托记录["买卖方向", e.RowIndex].Value;
        //        Color bg = mmfx == 0 ? System.Drawing.Color.Red : (mmfx == 1 ? System.Drawing.Color.Blue : Color.Black);
        //        this.dataGridView委托记录.Rows[e.RowIndex].DefaultCellStyle.ForeColor = bg;
        //        //if (mmfx == 0)
        //        //{
        //        //    this.dataGridView委托记录.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
        //        //}
        //        //else if (mmfx == 1)
        //        //{
        //        //    this.dataGridView委托记录.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
        //        //}
        //        //else
        //        //{
        //        //    this.dataGridView委托记录.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
        //        //}
        //    }
        //    catch (Exception) { }
        //}

        private void dataGridView组合仓位_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int mmfx = (int)this.dataGridView组合仓位["Column4", e.RowIndex].Value;//开仓类别 列
            if (mmfx == 0)
            {
                this.dataGridView组合仓位.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
            else if (mmfx == 1)
            {
                this.dataGridView组合仓位.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.dataGridView组合仓位.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        //private void dataGridView当日平仓_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        //{
        //    decimal fdyk = (decimal)this.dataGridView当日平仓["毛利", e.RowIndex].Value;

        //    if (fdyk > 0)
        //    {
        //        this.dataGridView当日平仓.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
        //    }
        //    else if (fdyk < 0)
        //    {
        //        this.dataGridView当日平仓.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
        //    }
        //    else
        //    {
        //        this.dataGridView当日平仓.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
        //    }
        //}

        private void dataGridView交易日志_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "买卖方向")
            {
                int int1 = (int)e.Value;

                switch (int1)
                {
                    case 0:
                        e.Value = "买入";
                        break;
                    case 1:
                        e.Value = "卖出";
                        break;
                    case 2:
                        e.Value = "融资买入";
                        break;
                    case 3:
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;

                }

            }
        }

        private void dataGridView当前委托_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "买卖方向")
            {
                int int1 = (int)e.Value;

                switch (int1)
                {
                    case 0:
                        e.Value = "买入";
                        break;
                    case 1:
                        e.Value = "卖出";
                        break;
                    case 2:
                        e.Value = "融资买入";
                        break;
                    case 3:
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;

                }

            }
        }

        private void dataGridView当前成交_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "买卖方向")
            {
                int int1 = (int)e.Value;

                switch (int1)
                {
                    case 0:
                        e.Value = "买入";
                        break;
                    case 1:
                        e.Value = "卖出";
                        break;
                    case 2:
                        e.Value = "融资买入";
                        break;
                    case 3:
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;

                }

            }
        }

        private void dataGridView委托记录_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var header = (sender as DataGridView).Columns[e.ColumnIndex].HeaderText;
            if (header == "买卖方向")
            {
                int int1 = (int)e.Value;

                switch (int1)
                {
                    case 0:
                        e.Value = "买入";
                        break;
                    case 1:
                        e.Value = "卖出";
                        break;
                    case 2:
                        e.Value = "融资买入";
                        break;
                    case 3:
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;

                }

            }
            else if (header == "委托编号")
            {
                var match = Regex.Match(e.Value as string, "^[0-9_]+_([0-9]+)$");
                if (match.Success)
                {
                    e.Value = match.Groups[1].Value;
                }
            }
        }


        private void timerConnector_Tick(object sender, EventArgs e)
        {
            Task.Run(() => {
                try
                {
                    string status = "";

                    if (Program.AASServiceClient.State == CommunicationState.Opened && Program.AASServiceClient.QuerySingleUser(Program.Version) != null)
                    {
                        status = "连接状态：已连接";
                    }
                    else if (Program.AASServiceClient.State == CommunicationState.Created)
                    {
                        status = "连接状态：已创建";
                    }
                    else
                    {
                        status = "连接状态：未连接";
                        bool isConnected =   Program.AutoReLogin();
                        if (isConnected)
                        {
                            status = "连接状态：连接中断";
                        }
                    }
                    if (!this.IsDisposed)
                    {
                        this.Invoke(new Action(() => { toolStripStatusLabelConnect.Text = status; }));
                    }

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

        private void buttonSearchShareLimit_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView额度分配股票_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "买方式")
            {
                var buyType = e.Value.ToString();

                switch (buyType)
                {
                    case "0":
                        e.Value = "买入";
                        break;
                    case "1":
                        e.Value = "融资买入";
                        break;
                    default:
                        break;
                }

            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "卖方式")
            {
                var saleType = e.Value.ToString();

                switch (saleType)
                {
                    case "0":
                        e.Value = "卖出";
                        break;
                    case "1":
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;
                }
            }
        }

        private void dataGridView当前仓位_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //Program.logger.LogInfo("dataGridView当前仓位_DataError: " + e.Exception.Message);
            e.ThrowException = false;
            e.Cancel = true;
        }

        private void 当日成交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new TradeDetailForm() { StartPosition = FormStartPosition.CenterScreen };
            win.Show(this);
        }

        private void 委托记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new RiskControlDetail.OrderLogDetail();
            win.Show(this);
        }

        private void 当日平仓ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new RiskControlDetail.TodayCloseTrade();
            win.Show(this);
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                try
                {
                    var result = Program.AASServiceClient.QueryAll订单();
                    dataGridView当前仓位.Invoke(new Action(() =>
                    { 
                        Refresh订单UI(result); 
                    }));
                }
                catch (Exception)
                {
                    
                }
                
            });
            
        }
    }
}
