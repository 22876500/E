using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.ServiceModel;
using System.ComponentModel;
using Server.Adapter;

namespace Server
{


    public partial class DbDataSet
    {

        public void Load()
        {
            this.额度分配.Load();

            this.交易日志.Load();

            this.已平仓订单.Load();

            this.风控分配.Load();

            this.MAC地址分配.Load();

            this.平台用户.Load();

            this.订单.Load();

            this.券商帐户.Load();

            this.已发委托.Load();

            this.已处理成交.Load();

            this.电子币帐户.Load();

            this.可用资金.Load();

            this.交易账户关联.Load();
        }

        public 平台用户DataTable QueryJyBelongFK(string FKUserName)
        {
            Server.DbDataSet.平台用户Row 风控员 = this.平台用户.Get平台用户(FKUserName);
            Server.DbDataSet.平台用户DataTable 交易员DataTable = this.平台用户.QueryUserInRoles(new int[] { (int)角色.交易员 }, 风控员.分组);


            if (风控员.角色 == (int)角色.普通风控员)
            {
                Server.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                foreach (Server.DbDataSet.平台用户Row 交易员Row1 in 交易员DataTable)
                {
                    if (this.风控分配.Exists(交易员Row1.用户名, 风控员.用户名))
                    {
                        平台用户DataTable1.ImportRow(交易员Row1);
                    }
                }
                return 平台用户DataTable1;
            }
            else if (风控员.角色 == (int)角色.超级风控员)
            {
                Server.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                foreach (Server.DbDataSet.平台用户Row 交易员Row1 in 交易员DataTable)
                {
                    平台用户DataTable1.ImportRow(交易员Row1);
                }
                return 平台用户DataTable1;
            }
            else
            {
                throw new FaultException("非风控用户");
            }
        }

        public 平台用户DataTable QueryJyNotBelongFK(string FKUserName)
        {
            Server.DbDataSet.平台用户Row 风控员 = this.平台用户.Get平台用户(FKUserName);
            Server.DbDataSet.平台用户DataTable 交易员DataTable = this.平台用户.QueryUserInRoles(new int[] { (int)角色.交易员 }, 风控员.分组);
            if (风控员.角色 == (int)角色.普通风控员)
            {
                Server.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                foreach (Server.DbDataSet.平台用户Row 交易员Row1 in 交易员DataTable)
                {
                    if (!this.风控分配.Exists(交易员Row1.用户名, 风控员.用户名))
                    {
                        平台用户DataTable1.ImportRow(交易员Row1);
                    }
                }

                return 平台用户DataTable1;
            }
            else if (风控员.角色 == (int)角色.超级风控员)
            {
                Server.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                return 平台用户DataTable1;
            }
            else
            {
                throw new FaultException("非风控用户");
            }
        }


        partial class 电子币帐户DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["名称"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (电子币帐户 accItem in db.电子币帐户)
                        {
                            var 电子币帐户Row1 = this.New电子币帐户Row();
                            电子币帐户Row1.名称 = accItem.名称;
                            电子币帐户Row1.启用 = accItem.启用;
                            电子币帐户Row1.交易平台 = accItem.交易平台;
                            电子币帐户Row1.登录帐号 = accItem.登录帐号;
                            电子币帐户Row1.ApiKey = Cryptor.MD5Decrypt(accItem.ApiKey);
                            电子币帐户Row1.SecretKey = Cryptor.MD5Decrypt(accItem.SecretKey);

                            this.Add电子币帐户Row(电子币帐户Row1);
                        }
                    }

                    this.电子币帐户RowChanging += 电子币帐户_电子币帐户RowChanging;
                    this.电子币帐户RowDeleting += 电子币帐户_电子币帐户RowChanging;
                }
            }

            private void 电子币帐户_电子币帐户RowChanging(object sender, 电子币帐户RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            电子币帐户 accItem = new 电子币帐户();
                            accItem.交易平台 = e.Row.交易平台;
                            accItem.名称 = e.Row.名称;
                            accItem.启用 = e.Row.启用;
                            accItem.登录帐号 = e.Row.登录帐号;
                            accItem.ApiKey = Cryptor.MD5Encrypt(e.Row.ApiKey);
                            accItem.SecretKey = Cryptor.MD5Encrypt(e.Row.SecretKey);
                            db.电子币帐户.Add(accItem);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            var item = db.电子币帐户.Find(e.Row.名称);
                            db.电子币帐户.Remove(item);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Change:
                            电子币帐户 accItem1 = db.电子币帐户.Find(e.Row.名称);
                            accItem1.交易平台 = e.Row.交易平台;
                            accItem1.启用 = e.Row.启用;
                            accItem1.登录帐号 = e.Row.登录帐号;
                            accItem1.ApiKey = Cryptor.MD5Encrypt(e.Row.ApiKey);
                            accItem1.SecretKey = Cryptor.MD5Encrypt(e.Row.SecretKey);
                            db.电子币帐户.Add(accItem1);
                            db.Entry(accItem1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }
                }
            }

            public bool Exists(string Name)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(r => r.名称 == Name);
                }
            }

            public 电子币帐户DataTable QueryQsAccount()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    电子币帐户DataTable 电子币帐户DataTable1 = new 电子币帐户DataTable();
                    foreach (电子币帐户Row 电子币帐户Row1 in this)
                    {
                        电子币帐户DataTable1.ImportRow(电子币帐户Row1);
                    }
                    return 电子币帐户DataTable1;
                }
            }

            public void EnableQSAccount(string Name, bool Enabled)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.电子币帐户Row 电子币帐户Row1 = this.First(r => r.名称 == Name);
                    电子币帐户Row1.EnableQSAccount(Enabled);
                }
            }

            public void AddQSAccount(bool Enabled, string Name, string 交易平台, string 登录帐号, string apiKey, string secretKey)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    电子币帐户Row row = this.New电子币帐户Row();
                    row.ApiKey = apiKey;
                    row.SecretKey = secretKey;
                    row.交易平台 = 交易平台;
                    row.名称 = Name;
                    row.启用 = Enabled;
                    row.登录帐号 = 登录帐号;
                    this.Add电子币帐户Row(row);
                }
            }

            public void UpdateQSAccount(string Name, string 交易平台, string 登录帐号, string ApiKey, string SecretKey)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    电子币帐户Row row = this.First(r => r.名称 == Name);
                    row.UpdateQSAccount(交易平台, 登录帐号, ApiKey, SecretKey);
                }
            }

            public void DeleteQSAccount(string Name)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.电子币帐户Row row = this.First(r => r.名称 == Name);
                    row.Stop();
                    Program.db.电子币帐户.Remove电子币帐户Row(row);
                }

            }

            public void Start()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (Server.DbDataSet.电子币帐户Row 电子币帐户Row1 in this)
                    {
                        if (!电子币帐户Row1.IsBusy)
                        {
                            电子币帐户Row1.Start();
                        }
                    }
                }
            }

            public void Stop()
            {
                Program.logger.LogInfo("开始停止电子币帐户...");
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (Server.DbDataSet.电子币帐户Row 电子币帐户Row1 in this)
                    {
                        if (电子币帐户Row1.IsBusy)
                        {
                            电子币帐户Row1.Stop();
                        }
                    }
                }
            }

            public void SendOrder(string 组合号, int 买卖类别, string 证券代码, decimal 委托价格, decimal 委托数量, string 交易员, out string Result, out string ErrInfo)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    电子币帐户Row row = this.FirstOrDefault(r => r.名称 == 组合号);
                    row.SendOrder(证券代码, 委托价格, 委托数量, 买卖类别, 交易员, out Result, out ErrInfo);
                }
            }

            public void CancelOrder(string Zqdm, string 组合号, string orderID, out string Result, out string ErrInfo)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.电子币帐户Row row = this.FirstOrDefault(r => r.名称 == 组合号);
                    if (row == null)
                    {
                        Result = string.Empty;
                        ErrInfo = string.Format("{0}不存在", 组合号);
                        return;
                    }
                    row.CancelOrder(Zqdm, orderID, out Result, out ErrInfo);
                }
            }

            public List<AccounCoin> QueryAccountCoin(string 组合号, List<string> listCoin = null)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    电子币帐户Row row = this.FirstOrDefault(r => r.名称 == 组合号);
                    return row.QueryAccountCoin(listCoin);
                }
            }

            public decimal QueryAccountCoin(string 组合号, string coin)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    电子币帐户Row row = this.FirstOrDefault(r => r.名称 == 组合号);
                    return row.QueryAccountCoin(coin);
                }
            }

            public DbDataSet DbDataSet
            {
                get
                {
                    return this.DataSet as DbDataSet;
                }
            }
        }

        partial class 电子币帐户Row
        {
            IAdapter Adapter;

            private BackgroundWorker backgroundWorker1 = new BackgroundWorker();

            public int ClientID = -1;
            public object SendOrderObject = new object();
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();
            public JyDataSet.成交DataTable 帐户成交DataTable = new JyDataSet.成交DataTable();
            public JyDataSet.委托DataTable 帐户委托DataTable = new JyDataSet.委托DataTable();

            public void UpdateQSAccount(string 交易平台, string 登录帐号, string ApiKey, string SecretKey)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.交易平台 = 交易平台;
                    this.ApiKey = ApiKey;
                    this.登录帐号 = 登录帐号;
                    this.SecretKey = SecretKey;
                }
            }
            public void EnableQSAccount(bool Enabled)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.启用 = Enabled;
                }
            }

            public void Start()
            {
                this.backgroundWorker1.WorkerSupportsCancellation = true;
                this.backgroundWorker1.WorkerReportsProgress = true;
                this.backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
                this.backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
                this.backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
                this.backgroundWorker1.RunWorkerAsync();
            }

            public void Stop()
            {
                this.backgroundWorker1.CancelAsync();
                while (this.backgroundWorker1.IsBusy)
                {
                    Thread.Sleep(100);
                }
            }

            public bool IsBusy
            {
                get
                {
                    return this.backgroundWorker1.IsBusy;
                }
            }

            void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
            {
                while (!this.backgroundWorker1.CancellationPending)
                {
                    if (this.Safe启用)
                    {
                        if (Adapter == null || !Adapter.IsInited)
                        {
                            #region 登录
                            this.backgroundWorker1.ReportProgress(0, "登录中...");

                            string Msg = this.Logon();

                            this.backgroundWorker1.ReportProgress(0, Msg);
                            #endregion

                            Thread.Sleep(1500);
                        }
                        else
                        {
                            try
                            {
                                #region 工作
                                var queryInfo = this.QueryData();
                                this.帐户委托DataTable.Deal();
                                this.backgroundWorker1.ReportProgress(0, queryInfo);
                                Thread.Sleep(100);
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                Program.logger.LogInfo("电子币帐户{0}后台线程异常:{1}", this.名称, ex.Message);

                                Thread.Sleep(3000);
                            }
                        }
                    }
                    else
                    {
                        #region 未启用
                        if (this.ClientID != -1)
                        {
                            this.backgroundWorker1.ReportProgress(0, "注销中...");

                            this.Logoff();

                            this.backgroundWorker1.ReportProgress(0, "已注销");
                        }

                        this.backgroundWorker1.ReportProgress(0, "未启用");

                        Thread.Sleep(1000);

                        #endregion
                    }
                }
            }

            void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                switch (e.ProgressPercentage)
                {
                    case 0:
                        Program.mainForm.BeginInvoke((Action)delegate ()
                        {
                            UIDataSet.交易帐户Row 交易帐户Row1 = Program.UIdataSet.交易帐户.FirstOrDefault(r => r.组合号 == this.名称);
                            if (交易帐户Row1 == null)
                            {
                                交易帐户Row1 = Program.UIdataSet.交易帐户.New交易帐户Row();
                                交易帐户Row1.组合号 = this.名称;
                                交易帐户Row1.查询耗时 = e.UserState as string;
                                Program.UIdataSet.交易帐户.Add交易帐户Row(交易帐户Row1);
                            }
                            else
                            {
                                交易帐户Row1.查询耗时 = e.UserState as string;
                            }
                        });
                        break;
                    default:
                        break;
                }

            }

            void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Error == null)
                {
                    Program.mainForm.BeginInvoke((Action)delegate ()
                    {
                        UIDataSet.交易帐户Row 交易帐户Row2 = Program.UIdataSet.交易帐户.FirstOrDefault(r => r.组合号 == this.名称);
                        if (交易帐户Row2 != null)
                        {
                            Program.UIdataSet.交易帐户.Remove交易帐户Row(交易帐户Row2);
                        }
                    });
                }
                else
                {
                    Program.mainForm.BeginInvoke((Action)delegate ()
                    {
                        UIDataSet.交易帐户Row 交易帐户Row1 = Program.UIdataSet.交易帐户.FirstOrDefault(r => r.组合号 == this.名称);
                        if (交易帐户Row1 == null)
                        {
                            交易帐户Row1 = Program.UIdataSet.交易帐户.New交易帐户Row();
                            交易帐户Row1.组合号 = this.名称;
                            交易帐户Row1.查询耗时 = e.Error.Message;
                            Program.UIdataSet.交易帐户.Add交易帐户Row(交易帐户Row1);
                        }
                        else
                        {
                            交易帐户Row1.查询耗时 = e.Error.Message;
                        }
                    });
                }
            }

            public bool Safe启用
            {
                get
                {
                    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                    {
                        return this.启用;
                    }
                }
            }

            public string Logon()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    switch (this.交易平台)
                    {
                        case "币安":

                            BinanceAdapter adapter = new BinanceAdapter();
                            try
                            {
                                var dt = Program.db.已发委托.Get已发委托(DateTime.Today, this.名称);
                                adapter.Init(this.ApiKey, this.SecretKey, this.名称, this.登录帐号, dt);
                                Adapter = adapter;
                                this.ClientID = 1;
                                return "登录成功";
                            }
                            catch (Exception ex)
                            {
                                var errMsg = (ex.InnerException ?? ex).Message;
                                Program.logger.LogInfo("{0} Init 异常：{1}", this.名称, errMsg);
                                if (errMsg.Contains("Invalid API-key, IP, or permissions"))
                                {
                                    this.启用 = false;
                                    Program.logger.LogInfo("{0} API登陆验证失败,将自动停止", this.名称, errMsg);
                                }
                                Thread.Sleep(60000);
                                return "登录异常：" + errMsg;
                            }
                        default:
                            return "不支持该交易平台账户！";
                    }
                }

            }

            public void Logoff()
            {
                //接口如何退出？或者不需要退出，关闭程序即可
                if (Adapter != null)
                {
                    this.ClientID = -1;
                    Adapter = null;
                }
            }

            public string QueryData()
            {
                DateTime dtStart = DateTime.Now;
                try
                {
                    JyDataSet.成交DataTable 查到的成交Result = null;
                    JyDataSet.委托DataTable 查到的委托Result = null;
                    Adapter.QueryData(out 查到的委托Result, out 查到的成交Result);

                    //CheckLostOrder(查到的委托Result);

                    帐户委托DataTable = 查到的委托Result;
                    帐户成交DataTable = 查到的成交Result;

                    Program.帐户成交DataTable[this.名称] = this.帐户成交DataTable.Copy() as JyDataSet.成交DataTable;
                    Program.帐户委托DataTable[this.名称] = this.帐户委托DataTable.Copy() as JyDataSet.委托DataTable;
                    Thread.Sleep(200);
                    return (DateTime.Now - dtStart).TotalSeconds.ToString();
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                    return string.Format("QueryData 发生异常：{0}", (ex.InnerException ?? ex).Message);
                }
            }

            //void CheckLostOrder(JyDataSet.委托DataTable 查到的委托DataTable)
            //{
            //    if (查到的委托DataTable == null)
            //    {
            //        return;
            //    }
            //    var orderList = Program.db.已发委托.GetOrderIDList(DateTime.Today, this.名称);
            //    if (orderList.Count < 查到的委托DataTable.Rows.Count)
            //    {
            //        foreach (var item in 查到的委托DataTable)
            //        {
            //            if (!orderList.Contains(item.委托编号))
            //            {
            //                Program.db.已发委托.Add(DateTime.Today, this.名称, item.委托编号, item.交易员, "委托补漏成功", 0, item.证券代码, item.证券名称, item.买卖方向, item.成交价格, item.成交数量, item.委托价格, item.委托数量, item.撤单数量);
            //                Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), item.交易员, this.名称, item.证券代码, item.证券代码, item.委托编号, item.买卖方向, item.委托数量, item.委托价格, "下单补漏成功");
            //            }
            //        }
            //    }
            //}

            public void SendOrder(string zqdm, decimal price, decimal quantity, int bsFlag, string trader, out string orderID, out string errInfo)
            {
                lock (this.SendOrderObject)
                {
                    switch (this.交易平台)
                    {
                        case "币安":
                            Adapter.SendOrder(zqdm, bsFlag, quantity, price, trader, out orderID, out errInfo);
                            break;
                        default:
                            orderID = string.Empty;
                            errInfo = "目前只支持币安接口！";
                            break;
                    }
                }


            }

            public void CancelOrder(string zqdm, string hth, out string result, out string errInfo)
            {
                switch (this.交易平台)
                {
                    case "币安":
                        Adapter.CancelOrder(zqdm, hth, out result, out errInfo);
                        break;
                    default:
                        throw new NotImplementedException("目前只支持币安接口！");
                }
            }

            public List<AccounCoin> QueryAccountCoin(List<string> list)
            {
                return Adapter == null ? null : Adapter.QueryAccountCoin(list);
            }

            public decimal QueryAccountCoin(string coin)
            {
                return Adapter == null ? 0 : Adapter.QueryAccountCoin(coin);
            }
        }



        partial class 券商帐户DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();


            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["名称"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (券商帐户 券商帐户1 in db.券商帐户)
                        {
                            券商帐户Row 券商帐户Row1 = this.New券商帐户Row();
                            券商帐户Row1.启用 = 券商帐户1.启用;
                            券商帐户Row1.名称 = 券商帐户1.名称;
                            券商帐户Row1.券商 = 券商帐户1.券商;
                            券商帐户Row1.类型 = 券商帐户1.类型;
                            券商帐户Row1.交易服务器 = 券商帐户1.交易服务器;
                            券商帐户Row1.版本号 = 券商帐户1.版本号;
                            券商帐户Row1.营业部代码 = 券商帐户1.营业部代码;
                            券商帐户Row1.登录帐号 = 券商帐户1.登录帐号;
                            券商帐户Row1.交易帐号 = 券商帐户1.交易帐号;
                            券商帐户Row1.交易密码 = Cryptor.MD5Decrypt(券商帐户1.交易密码);
                            券商帐户Row1.通讯密码 = Cryptor.MD5Decrypt(券商帐户1.通讯密码);
                            券商帐户Row1.上海股东代码 = 券商帐户1.上海股东代码;
                            券商帐户Row1.深圳股东代码 = 券商帐户1.深圳股东代码;
                            券商帐户Row1.查询间隔时间 = 券商帐户1.查询间隔时间;
                            this.Add券商帐户Row(券商帐户Row1);
                        }
                    }



                    this.券商帐户RowChanging += 券商帐户_券商帐户RowChanging;
                    this.券商帐户RowDeleting += 券商帐户_券商帐户RowChanging;
                }
            }


            void 券商帐户_券商帐户RowChanging(object sender, DbDataSet.券商帐户RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            券商帐户 券商帐户1 = new 券商帐户();
                            券商帐户1.启用 = e.Row.启用;
                            券商帐户1.名称 = e.Row.名称;
                            券商帐户1.券商 = e.Row.券商;
                            券商帐户1.类型 = e.Row.类型;
                            券商帐户1.交易服务器 = e.Row.交易服务器;
                            券商帐户1.版本号 = e.Row.版本号;
                            券商帐户1.营业部代码 = e.Row.营业部代码;
                            券商帐户1.登录帐号 = e.Row.登录帐号;
                            券商帐户1.交易帐号 = e.Row.交易帐号;
                            券商帐户1.交易密码 = Cryptor.MD5Encrypt(e.Row.交易密码);
                            券商帐户1.通讯密码 = Cryptor.MD5Encrypt(e.Row.通讯密码);
                            券商帐户1.上海股东代码 = e.Row.上海股东代码;
                            券商帐户1.深圳股东代码 = e.Row.深圳股东代码;
                            券商帐户1.查询间隔时间 = e.Row.查询间隔时间;
                            db.券商帐户.Add(券商帐户1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            券商帐户1 = db.券商帐户.Find(e.Row.名称);
                            db.券商帐户.Remove(券商帐户1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Change:
                            券商帐户1 = db.券商帐户.Find(e.Row.名称);
                            券商帐户1.启用 = e.Row.启用;
                            券商帐户1.名称 = e.Row.名称;
                            券商帐户1.券商 = e.Row.券商;
                            券商帐户1.类型 = e.Row.类型;
                            券商帐户1.交易服务器 = e.Row.交易服务器;
                            券商帐户1.版本号 = e.Row.版本号;
                            券商帐户1.营业部代码 = e.Row.营业部代码;
                            券商帐户1.登录帐号 = e.Row.登录帐号;
                            券商帐户1.交易帐号 = e.Row.交易帐号;
                            券商帐户1.交易密码 = Cryptor.MD5Encrypt(e.Row.交易密码);
                            券商帐户1.通讯密码 = Cryptor.MD5Encrypt(e.Row.通讯密码);
                            券商帐户1.上海股东代码 = e.Row.上海股东代码;
                            券商帐户1.深圳股东代码 = e.Row.深圳股东代码;
                            券商帐户1.查询间隔时间 = e.Row.查询间隔时间;
                            db.Entry(券商帐户1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }
                }
            }




            public bool Exists(string Name)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(r => r.名称 == Name);
                }
            }



            public 券商帐户DataTable QueryQsAccount()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    券商帐户DataTable 券商帐户DataTable1 = new 券商帐户DataTable();
                    foreach (券商帐户Row 券商帐户Row1 in this)
                    {
                        券商帐户DataTable1.ImportRow(券商帐户Row1);
                    }
                    return 券商帐户DataTable1;
                }
            }


            public void EnableQSAccount(string Name, bool Enabled)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.券商帐户Row 券商帐户Row1 = this.First(r => r.名称 == Name);
                    券商帐户Row1.EnableQSAccount(Enabled);
                }
            }


            public void AddQSAccount(bool Enabled, string Name, string QS, string Type, string 交易服务器, string Version, short YybID, string Account, string TradeAccount, string JyPassword, string TxPassword, string SHGDDM, string SZGDDM, int 查询间隔时间)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.券商帐户Row 券商帐户Row1 = this.New券商帐户Row();
                    券商帐户Row1.启用 = Enabled;
                    券商帐户Row1.名称 = Name;
                    券商帐户Row1.券商 = QS;
                    券商帐户Row1.类型 = Type;
                    券商帐户Row1.交易服务器 = 交易服务器;
                    券商帐户Row1.版本号 = Version;
                    券商帐户Row1.营业部代码 = YybID;
                    券商帐户Row1.登录帐号 = Account;
                    券商帐户Row1.交易帐号 = TradeAccount;
                    券商帐户Row1.交易密码 = JyPassword;
                    券商帐户Row1.通讯密码 = TxPassword;
                    券商帐户Row1.上海股东代码 = SHGDDM;
                    券商帐户Row1.深圳股东代码 = SZGDDM;
                    券商帐户Row1.查询间隔时间 = 查询间隔时间;
                    this.Add券商帐户Row(券商帐户Row1);
                }
            }


            public void UpdateQSAccount(string Name, string 交易服务器, string Version, string 交易密码, string 通讯密码, int 查询间隔时间)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.券商帐户Row 券商帐户Row1 = this.First(r => r.名称 == Name);
                    券商帐户Row1.UpdateQSAccount(交易服务器, Version, 交易密码, 通讯密码, 查询间隔时间);
                }

            }


            public void DeleteQSAccount(string Name)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.券商帐户Row 券商帐户Row1 = this.First(r => r.名称 == Name);
                    券商帐户Row1.Stop();
                    Program.db.券商帐户.Remove券商帐户Row(券商帐户Row1);
                }

            }

            public void Start()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (Server.DbDataSet.券商帐户Row 券商帐户Row1 in this)
                    {
                        if (!券商帐户Row1.IsBusy)
                        {
                            券商帐户Row1.Start();
                        }
                    }
                }
            }

            public void Stop()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (Server.DbDataSet.券商帐户Row 券商帐户Row1 in this)
                    {
                        if (券商帐户Row1.IsBusy)
                        {
                            券商帐户Row1.Stop();
                        }
                    }
                }
            }

            public void SendOrder(string 组合号, int 买卖类别, byte 市场, string 证券代码, decimal 委托价格, decimal 委托数量, OrderCacheEntity orderCacheObj, out string Result, out string ErrInfo, out bool hasOrderNo)
            {
                Result = string.Empty;
                ErrInfo = string.Empty;

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == 组合号);


                    券商帐户1.SendOrder(买卖类别, 市场, 证券代码, 委托价格, 委托数量, orderCacheObj, out Result, out ErrInfo, out hasOrderNo);

                }
            }

            public void CancelOrder(string Zqdm, string 组合号, byte Market, string hth, out string Result, out string ErrInfo)
            {
                Result = string.Empty;
                ErrInfo = string.Empty;

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == 组合号);
                    if (券商帐户1 == null)
                    {
                        ErrInfo = string.Format("{0}不存在", 组合号);
                        return;
                    }


                    券商帐户1.CancelOrder(Zqdm, Market, hth, out Result, out ErrInfo);
                }
            }

            public string AccountRepay(string group, decimal amount)
            {
                StringBuilder result = new StringBuilder(1024);
                StringBuilder ErrMsg = new StringBuilder(1024);
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == group);
                    if (券商帐户1 == null)
                    {
                        return string.Format("{0}不存在", group);
                    }
                    券商帐户1.Repay(Math.Round(amount, 3).ToString(), result, ErrMsg);
                }
                return ErrMsg.Length == 0 ? result.ToString() : ErrMsg.ToString();
            }

            public DbDataSet DbDataSet
            {
                get
                {
                    return this.DataSet as DbDataSet;
                }
            }
        }

        partial class 券商帐户Row
        {
            public static object LoginObject = new object();
            object SendOrderObject = new object();
            BackgroundWorker backgroundWorker1 = new BackgroundWorker();

            public int ClientID = -1;
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();
            public JyDataSet.成交DataTable 帐户成交DataTable = new JyDataSet.成交DataTable();
            public JyDataSet.委托DataTable 帐户委托DataTable = new JyDataSet.委托DataTable();

            public void UpdateQSAccount(string 交易服务器, string Version, string 交易密码, string 通讯密码, int 查询间隔时间)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.交易服务器 = 交易服务器;
                    this.版本号 = Version;
                    this.交易密码 = 交易密码;
                    this.通讯密码 = 通讯密码;
                    this.查询间隔时间 = 查询间隔时间;
                }
            }
            public void EnableQSAccount(bool Enabled)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.启用 = Enabled;
                }
            }

            public void Start()
            {
                this.backgroundWorker1.WorkerSupportsCancellation = true;
                this.backgroundWorker1.WorkerReportsProgress = true;
                this.backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
                this.backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
                this.backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
                this.backgroundWorker1.RunWorkerAsync();
            }


            public void Stop()
            {
                this.backgroundWorker1.CancelAsync();
                while (this.backgroundWorker1.IsBusy)
                {
                    Thread.Sleep(100);
                }
            }

            public bool IsBusy
            {
                get
                {
                    return this.backgroundWorker1.IsBusy;
                }
            }

            void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
            {
                while (!this.backgroundWorker1.CancellationPending)
                {
                    if (this.Safe启用)
                    {
                        if (DateTime.Parse(Program.appConfig.GetValue("开始查询时间", "8:15")) <= DateTime.Now && DateTime.Now <= DateTime.Parse(Program.appConfig.GetValue("结束查询时间", "15:30")))
                        {
                            #region 交易时段
                            if (this.ClientID == -1)
                            {
                                #region 登录
                                this.backgroundWorker1.ReportProgress(0, "登录中...");


                                string Msg;
                                lock (LoginObject)
                                {
                                    Msg = this.Logon();
                                }

                                this.backgroundWorker1.ReportProgress(0, Msg);
                                #endregion

                                Thread.Sleep(1500);
                            }
                            else
                            {
                                try
                                {
                                    #region 工作
                                    var queryCosts = this.QueryData();
                                    this.帐户委托DataTable.Deal();
                                    this.backgroundWorker1.ReportProgress(0, queryCosts);
                                    Thread.Sleep(100);
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    Program.logger.LogInfo("券商帐户 {0} 线程异常:{1} {2}", this.名称, ex.Message, ex.StackTrace);

                                    Thread.Sleep(3000);
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 非交易时段
                            if (this.ClientID != -1)
                            {
                                this.backgroundWorker1.ReportProgress(0, "注销中...");

                                this.Logoff();

                                this.backgroundWorker1.ReportProgress(0, "已注销");
                            }



                            this.backgroundWorker1.ReportProgress(0, "非交易时段");

                            Thread.Sleep(1000);
                            if (Program.CancelOrders.Count > 0)
                            {
                                Program.CancelOrders = new ConcurrentBag<string>();
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        #region 未启用
                        if (this.ClientID != -1)
                        {
                            this.backgroundWorker1.ReportProgress(0, "注销中...");

                            this.Logoff();

                            this.backgroundWorker1.ReportProgress(0, "已注销");
                        }

                        this.backgroundWorker1.ReportProgress(0, "未启用");

                        Thread.Sleep(1000);

                        #endregion
                    }
                }
            }

            void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                switch (e.ProgressPercentage)
                {
                    case 0:
                        Program.mainForm.BeginInvoke((Action)delegate ()
                        {
                            UIDataSet.交易帐户Row 交易帐户Row1 = Program.UIdataSet.交易帐户.FirstOrDefault(r => r.组合号 == this.名称);
                            if (交易帐户Row1 == null)
                            {
                                交易帐户Row1 = Program.UIdataSet.交易帐户.New交易帐户Row();
                                交易帐户Row1.组合号 = this.名称;
                                交易帐户Row1.查询耗时 = e.UserState as string;
                                Program.UIdataSet.交易帐户.Add交易帐户Row(交易帐户Row1);
                            }
                            else
                            {
                                交易帐户Row1.查询耗时 = e.UserState as string;
                            }
                        });
                        break;
                    default:
                        break;
                }

            }

            void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Error == null)
                {
                    Program.mainForm.BeginInvoke((Action)delegate ()
                    {
                        UIDataSet.交易帐户Row 交易帐户Row2 = Program.UIdataSet.交易帐户.FirstOrDefault(r => r.组合号 == this.名称);
                        if (交易帐户Row2 != null)
                        {
                            Program.UIdataSet.交易帐户.Remove交易帐户Row(交易帐户Row2);
                        }
                    });
                }
                else
                {
                    Program.mainForm.BeginInvoke((Action)delegate ()
                    {
                        UIDataSet.交易帐户Row 交易帐户Row1 = Program.UIdataSet.交易帐户.FirstOrDefault(r => r.组合号 == this.名称);
                        if (交易帐户Row1 == null)
                        {
                            交易帐户Row1 = Program.UIdataSet.交易帐户.New交易帐户Row();
                            交易帐户Row1.组合号 = this.名称;
                            交易帐户Row1.查询耗时 = e.Error.Message;
                            Program.UIdataSet.交易帐户.Add交易帐户Row(交易帐户Row1);
                        }
                        else
                        {
                            交易帐户Row1.查询耗时 = e.Error.Message;
                        }
                    });
                }
            }

            public bool Safe启用
            {
                get
                {
                    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                    {
                        return this.启用;
                    }
                }
            }

            public string Logon()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    string[] JyServerInfo = this.交易服务器.Split(new char[] { ':' });
                    StringBuilder ErrInfo = new StringBuilder(256);
                    //this.ClientID = TdxApi.Logon(JyServerInfo[1], short.Parse(JyServerInfo[2]), this.版本号, this.营业部代码, this.登录帐号, this.交易帐号, this.交易密码, this.通讯密码, ErrInfo);
                    //登录

                    //OrderCallBack = OrderCallBackFunc;
                    //TradeCallBack = TradeCallBackFunc;

                    //KFApi.register_rtn_order_callback(OrderCallBackFunc);
                    //KFApi.register_rtn_trade_callback(TradeCallBackFunc);


                    this.ClientID = 0;
                    // connect 是否成功，最好有返回值，否则不知道是否应该重新连接。
                    if (ErrInfo.ToString() != string.Empty)
                    {
                        return "登录失败:" + ErrInfo.ToString();
                    }
                    else
                    {
                        return "登录成功";
                    }
                }
            }

            //private void OrderCallBackFunc(int orderID, Server.KFApi.KFRtnOrder order)
            //{
            //    //收到委托信息时，如何处理
            //    Program.logger.LogInfo("收到委托回报:{0}", order.ToJson());
            //}

            //private void TradeCallBackFunc(int orderID, Server.KFApi.KFRtnTrade trade)
            //{
            //    Program.logger.LogInfo("收到委托回报:{0}", trade.ToJson());
            //}

            public void Logoff()
            {
                //TdxApi.Logoff(this.ClientID);
                //KFApi.disconnect();
                this.ClientID = -1;
            }

            public string QueryData()
            {
                double timeCost = 0;
                DataTable 查到的成交Result = null;
                string 查到的成交ErrInfo = null;

                DataTable 查到的委托Result = null;
                string 查到的委托ErrInfo = null;
                QueryDataLocal(ref timeCost, ref 查到的成交Result, ref 查到的成交ErrInfo, ref 查到的委托Result, ref 查到的委托ErrInfo);

                DateTime dtMid = DateTime.Now;


                if (查到的成交ErrInfo == string.Empty)
                {
                    if (查到的成交Result != null && 查到的成交Result.Rows.Count > 0)
                    {
                        this.帐户成交DataTable = this.Get规范成交(查到的成交Result);
                    }
                }


                if (查到的委托ErrInfo == string.Empty)
                {
                    if (查到的委托Result != null && 查到的委托Result.Rows.Count > 0)
                    {
                        this.帐户委托DataTable = this.Get规范委托(查到的委托Result);
                    }
                }

                Program.帐户成交DataTable[this.名称] = this.帐户成交DataTable.Copy() as JyDataSet.成交DataTable;
                Program.帐户委托DataTable[this.名称] = this.帐户委托DataTable.Copy() as JyDataSet.委托DataTable;
                Thread.Sleep(200);
                return timeCost.ToString();
            }

            private void QueryDataLocal(ref double timeCost, ref DataTable 查到的成交Result, ref string 查到的成交ErrInfo, ref DataTable 查到的委托Result, ref string 查到的委托ErrInfo)
            {
                var dt = DateTime.Now;
                //StringBuilder Result = new StringBuilder(1024 * 1024);
                //StringBuilder ErrInfo = new StringBuilder(1024);
                //可以考虑用于保持心跳连接

                //TdxApi.QueryData(this.ClientID, 3, result, errInfo);
                //查到的成交Result = Tool.ChangeDataStringToTable(result.ToString());
                //查到的成交ErrInfo = errInfo.ToString();
                Thread.Sleep(this.查询间隔时间 / 2);

                //TdxApi.QueryData(this.ClientID, 2, result, errInfo);

                //查到的委托Result = Tool.ChangeDataStringToTable(result.ToString());
                //查到的委托ErrInfo = errInfo.ToString();
                Thread.Sleep(this.查询间隔时间 / 2);

                timeCost = (DateTime.Now - dt).TotalSeconds;
            }

            JyDataSet.成交DataTable Get规范成交(DataTable 查到的成交DataTable)
            {
                bool hasCancelSymbol = 查到的成交DataTable.Columns.Contains("撤单标志");
                bool hasSuccessPrice = 查到的成交DataTable.Columns.Contains("成交价格");
                bool hasSuccessQty = 查到的成交DataTable.Columns.Contains("成交数量");

                string sucPriceCol = "";
                if (hasSuccessPrice)
                {
                    sucPriceCol = "成交价格";
                }
                else if (查到的成交DataTable.Columns.Contains("成交均价"))
                {
                    sucPriceCol = "成交均价";
                }
                JyDataSet.成交DataTable 规范的成交DataTable = new JyDataSet.成交DataTable();
                try
                {
                    for (int i = 查到的成交DataTable.Rows.Count - 1; i >= 0; i--)
                    {
                        if (hasSuccessPrice && decimal.Parse(查到的成交DataTable.Rows[i]["成交价格"] as string) == 0)
                        {
                            //查到的成交DataTable.Rows.RemoveAt(i);
                        }
                        else if (hasSuccessQty && decimal.Parse(查到的成交DataTable.Rows[i]["成交数量"] as string) <= 0)
                        {
                            //查到的成交DataTable.Rows.RemoveAt(i);
                        }
                        else
                        {
                            DataRow DataRow0 = 查到的成交DataTable.Rows[i];
                            string 委托编号 = this.GetDataRow委托编号(DataRow0);
                            Server.DbDataSet.已发委托Row 已发委托Row1 = this.table券商帐户.DbDataSet.已发委托.Get已发委托(DateTime.Today, this.名称, 委托编号);
                            if (已发委托Row1 != null)
                            {
                                JyDataSet.成交Row 成交Row1 = 规范的成交DataTable.New成交Row();
                                成交Row1.交易员 = 已发委托Row1.交易员;
                                成交Row1.组合号 = this.名称;
                                成交Row1.证券代码 = 已发委托Row1.证券代码;
                                成交Row1.证券名称 = 已发委托Row1.证券名称;
                                成交Row1.委托编号 = 已发委托Row1.委托编号;
                                成交Row1.买卖方向 = 已发委托Row1.买卖方向;
                                成交Row1.市场代码 = 已发委托Row1.市场代码;
                                成交Row1.成交编号 = 规范的成交DataTable.Count.ToString();//此券商不提供成交编号.  不能更改，成交编号必须这么定义为序号，防止重复

                                #region 各个券商
                                if ((this.券商 == "招商证券") ||
                                    (this.券商 == "银河证券") ||
                                    (this.券商 == "广发证券") ||
                                    (this.券商 == "光大证券") ||
                                    (this.券商 == "中信证券") ||
                                    (this.券商 == "东方证券") ||
                                    (this.券商 == "国信证券") ||
                                    (this.券商 == "国海证券" && this.类型 == "普通") ||
                                    (this.券商 == "国泰君安" && this.类型 == "信用") ||
                                    (this.券商 == "民族证券" && this.类型 == "普通") ||
                                    (this.券商 == "兴业证券" && this.类型 == "普通") ||
                                    (this.券商 == "中泰证券" && this.类型 == "信用")
                                    )
                                {
                                    成交Row1.成交时间 = DataRow0["成交时间"] as string;
                                    if (!string.IsNullOrEmpty(sucPriceCol))
                                    {
                                        成交Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                                    }

                                    成交Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                                    成交Row1.成交金额 = decimal.Parse(DataRow0["成交金额"] as string);
                                }
                                else if (this.券商 == "华泰证券" && (this.类型 == "信用" || this.类型 == "普通"))
                                {
                                    成交Row1.成交时间 = "9:30";
                                    成交Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                                    成交Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                                    成交Row1.成交金额 = decimal.Parse(DataRow0["成交金额"] as string);
                                }
                                else
                                {
                                    throw new Exception(string.Format("不支持 {0}{1} 帐户", this.券商, this.类型));
                                }
                                #endregion


                                规范的成交DataTable.Add成交Row(成交Row1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    Program.logger.LogInfo("Get规范成交异常:{1} {2}", this.名称, ex.Message, ex.StackTrace);
                }

                return 规范的成交DataTable;
            }

            JyDataSet.委托DataTable Get规范委托(DataTable 查到的委托DataTable)
            {
                JyDataSet.委托DataTable 规范的委托DataTable = new JyDataSet.委托DataTable();
                bool EnableRepair = 查到的委托DataTable.Columns.Contains("委托价格")
                                 && 查到的委托DataTable.Columns.Contains("委托数量")
                                 && 查到的委托DataTable.Columns.Contains("证券代码")
                                 && 查到的委托DataTable.Columns.Contains("买卖标志");
                var needRepairList = CommonUtils.OrderCacheQueue.Where(_ => _.GroupName == this.名称 && _.IsTimeOutError == "1" && (DateTime.Now - _.SendTime).TotalSeconds < 30 && string.IsNullOrEmpty(_.OrderID)).ToList();

                foreach (DataRow DataRow0 in 查到的委托DataTable.Rows)
                {
                    string 委托编号 = this.GetDataRow委托编号(DataRow0);
                    if (规范的委托DataTable.Any(r => r.委托编号 == 委托编号))
                    {
                        ReCalculateTradeNumber(规范的委托DataTable, 委托编号);
                        ReCalculateTradePrice(规范的委托DataTable, 委托编号);
                        continue;
                    }



                    Server.DbDataSet.已发委托Row 已发委托Row1 = this.table券商帐户.DbDataSet.已发委托.Get已发委托(DateTime.Today, this.名称, 委托编号);
                    if (已发委托Row1 != null)
                    {
                        JyDataSet.委托Row 委托Row1 = 规范的委托DataTable.New委托Row();
                        委托Row1.交易员 = 已发委托Row1.交易员;
                        委托Row1.组合号 = this.名称;
                        委托Row1.证券代码 = 已发委托Row1.证券代码;
                        委托Row1.证券名称 = 已发委托Row1.证券名称;
                        委托Row1.委托价格 = 已发委托Row1.委托价格;
                        委托Row1.委托数量 = 已发委托Row1.委托数量;
                        委托Row1.委托编号 = 已发委托Row1.委托编号;
                        委托Row1.买卖方向 = 已发委托Row1.买卖方向;
                        委托Row1.市场代码 = 已发委托Row1.市场代码;

                        #region 各个券商
                        DataStandard(DataRow0, 委托编号, 委托Row1);
                        #endregion

                        规范的委托DataTable.Add委托Row(委托Row1);
                    }
                    else if (EnableRepair && needRepairList.Count > 0)
                    {
                        //30秒内，没有已下单记录，同时 股票代码，买卖方向，委托价格，委托数量
                        var needAddOrd = needRepairList.FirstOrDefault(_ => _.Zqdm == DataRow0["证券代码"].ToString()
                            && _.Quantity == decimal.Parse(DataRow0["委托数量"] + "")
                            && _.Price == decimal.Parse(DataRow0["委托价格"] + "")
                            && CommonUtils.IsTdxBuy(_.Category) == CommonUtils.IsTdxBuy(int.Parse(DataRow0["买卖标志"] + "")));
                        if (needAddOrd != null)
                        {
                            //加入委托表和交易日志表。
                            Program.db.已发委托.Add(DateTime.Today, this.名称, 委托编号, needAddOrd.Trader, "委托成功", needAddOrd.Market, needAddOrd.Zqdm, needAddOrd.ZqName, needAddOrd.Category, 0m, 0m, needAddOrd.Price, needAddOrd.Quantity, 0m);
                            string Msg = needAddOrd.IsRiskControl ? string.Format("风控员{0}下单成功", needAddOrd.Sender) : "下单成功";
                            Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), needAddOrd.Trader, this.名称, needAddOrd.Zqdm, needAddOrd.ZqName, 委托编号, needAddOrd.Category, needAddOrd.Quantity, needAddOrd.Price, Msg);
                            needAddOrd.OrderID = 委托编号;

                            JyDataSet.委托Row 委托Row1 = 规范的委托DataTable.New委托Row();

                            委托Row1.交易员 = needAddOrd.Trader;
                            委托Row1.组合号 = this.名称;
                            委托Row1.证券代码 = needAddOrd.Zqdm;
                            委托Row1.证券名称 = needAddOrd.ZqName;
                            委托Row1.委托价格 = needAddOrd.Price;
                            委托Row1.委托数量 = needAddOrd.Quantity;
                            委托Row1.委托编号 = 委托编号;
                            委托Row1.买卖方向 = needAddOrd.Category;
                            委托Row1.市场代码 = needAddOrd.Market;

                            DataStandard(DataRow0, 委托编号, 委托Row1);

                            规范的委托DataTable.Add委托Row(委托Row1);
                        }
                    }
                }

                return 规范的委托DataTable;
            }

            //修正异常成交价格
            private void ReCalculateTradePrice(JyDataSet.委托DataTable 规范的委托DataTable, string 委托编号)
            {
                var standardOrder = 规范的委托DataTable.First(r => r.委托编号 == 委托编号);
                if (standardOrder.成交数量 > 0 && standardOrder.成交价格 == 0)
                {
                    var order = Program.帐户成交DataTable[this.名称].FirstOrDefault(_ =>
                        _.委托编号 == 委托编号
                        && _.交易员 == standardOrder.交易员
                        && _.证券代码 == standardOrder.证券代码
                        && _.买卖方向 == standardOrder.买卖方向);
                    if (order != null && order.成交价格 != standardOrder.成交价格)
                    {
                        standardOrder.成交价格 = order.成交价格;
                    }
                    else
                    {
                        standardOrder.成交价格 = standardOrder.委托价格;
                    }
                }
            }

            //修正异常成交或者撤单数据
            private void ReCalculateTradeNumber(JyDataSet.委托DataTable 规范的委托DataTable, string 委托编号)
            {
                var standardOrder = 规范的委托DataTable.First(r => r.委托编号 == 委托编号);
                if (standardOrder.成交数量 > 0 && standardOrder.成交价格 == 0)//修正成交价格
                {
                    var standardTrade = this.帐户成交DataTable.FirstOrDefault(_ => _.委托编号 == 委托编号);
                    if (standardTrade != null)
                    {
                        standardOrder.成交价格 = standardTrade.成交金额 / standardTrade.成交数量;
                    }
                }
                else if (standardOrder.撤单数量 + standardOrder.成交数量 > standardOrder.委托数量)
                {
                    //修正成交数量或撤单数量
                    if (standardOrder.成交数量 > standardOrder.委托数量
                     && standardOrder.撤单数量 <= standardOrder.委托数量)
                    {
                        standardOrder.成交数量 = standardOrder.委托数量 - standardOrder.撤单数量;
                    }
                    else if (standardOrder.成交数量 > 0
                          && standardOrder.成交数量 < standardOrder.委托数量
                          && standardOrder.委托数量 == standardOrder.撤单数量)
                    {
                        standardOrder.撤单数量 = standardOrder.委托数量 - standardOrder.成交数量;
                    }
                }
            }

            private void DataStandard(DataRow DataRow0, string 委托编号, JyDataSet.委托Row 委托Row1)
            {
                decimal 成交数量, 成交价格;
                switch (this.券商)
                {
                    case "模拟测试":
                        委托Row1.委托时间 = DataRow0["委托时间"] as string;
                        委托Row1.成交价格 = DataRow0["成交均价"] as string == string.Empty ? 0 : decimal.Parse(DataRow0["成交均价"] as string);
                        委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                        委托Row1.状态说明 = DataRow0["状态说明"] as string;
                        委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                        break;
                    case "招商证券":
                        if (this.类型 == "普通")
                        {
                            委托Row1.委托时间 = DataRow0["委托时间"] as string;
                            委托Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                            委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                            委托Row1.状态说明 = DataRow0["状态说明"] as string;
                            //委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                            委托Row1.撤单数量 = 委托Row1.计算撤单数量();
                        }
                        else
                        {
                            委托Row1.委托时间 = DataRow0["委托时间"] as string;
                            委托Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                            委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                            委托Row1.状态说明 = DataRow0["状态说明"] as string;
                            委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                        }
                        break;
                    case "东方证券":
                        委托Row1.委托时间 = DataRow0["委托时间"] as string;
                        委托Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                        委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                        委托Row1.状态说明 = DataRow0["状态说明"] as string;
                        委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                        break;
                    case "银河证券":
                        if (this.类型 == "普通")
                        {
                            if (this.营业部代码 == 8888)
                            {
                                委托Row1.委托时间 = DataRow0["委托时间"] as string;
                                if (DataRow0.Table.Columns.Contains("成交价格"))
                                {
                                    委托Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                                }
                                else
                                {
                                    委托Row1.成交价格 = decimal.Parse(DataRow0["成交均价"] as string);
                                }
                                委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                                if (DataRow0.Table.Columns.Contains("结果说明"))
                                {
                                    委托Row1.状态说明 = DataRow0["结果说明"] as string;
                                }
                                else
                                {
                                    委托Row1.状态说明 = DataRow0["委托状态"] as string;
                                }
                                委托Row1.撤单数量 = 委托Row1.计算撤单数量();

                            }
                            else
                            {
                                委托Row1.委托时间 = DataRow0["委托时间"] as string;
                                委托Row1.成交价格 = decimal.Parse(DataRow0["成交均价"] as string);
                                委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                                委托Row1.状态说明 = DataRow0["委托状态"] as string;
                                委托Row1.撤单数量 = 委托Row1.计算撤单数量();
                            }
                        }
                        else if (this.类型 == "信用")
                        {
                            委托Row1.委托时间 = DataRow0["委托时间"] as string;
                            委托Row1.成交价格 = decimal.Parse(DataRow0["成交均价"] as string);
                            委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                            委托Row1.状态说明 = DataRow0["状态说明"] as string;
                            委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                        }
                        break;
                    case "广发证券":
                    case "兴业证券":
                    case "国海证券":
                    case "华泰证券":
                    case "民族证券":
                        委托Row1.委托时间 = DataRow0["委托时间"] as string;
                        委托Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                        委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                        委托Row1.状态说明 = DataRow0["状态说明"] as string;
                        委托Row1.撤单数量 = 委托Row1.计算撤单数量();
                        break;
                    case "光大证券":
                        委托Row1.委托时间 = DataRow0["委托时间"] as string;
                        委托Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                        委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                        委托Row1.状态说明 = DataRow0["委托状态"] as string;
                        委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                        break;
                    case "中信证券":
                        委托Row1.委托时间 = DataRow0["委托时间"] as string;
                        委托Row1.成交价格 = decimal.Parse(DataRow0["成交价格"] as string);
                        委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                        委托Row1.状态说明 = DataRow0["委托状态"] as string;
                        委托Row1.撤单数量 = decimal.Parse(DataRow0["已撤数量"] as string);
                        if (委托Row1.状态说明 != null && 委托Row1.状态说明.Contains("废单") && 委托Row1.撤单数量 <= 0)
                            委托Row1.撤单数量 = 委托Row1.委托数量;
                        break;
                    case "国信证券":
                        this.Get成交Info(委托编号, out 成交数量, out 成交价格);
                        委托Row1.委托时间 = DataRow0["委托时间"] as string;
                        委托Row1.成交价格 = 成交价格;
                        委托Row1.成交数量 = 成交数量;
                        委托Row1.状态说明 = DataRow0["隔夜委托标识"] as string;
                        委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                        break;
                    case "国泰君安":
                        this.Get成交Info(委托编号, out 成交数量, out 成交价格);
                        委托Row1.委托时间 = DataRow0["委托时间"] as string;
                        委托Row1.成交价格 = 成交价格;
                        委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                        委托Row1.状态说明 = DataRow0["状态说明"] as string;
                        委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                        if (委托Row1.成交数量 > 0 && 委托Row1.成交价格 == 0)
                            委托Row1.成交价格 = 委托Row1.委托价格;
                        break;
                    case "中泰证券":
                        委托Row1.委托时间 = DataRow0["委托时间"] as string;
                        委托Row1.成交价格 = decimal.Parse(DataRow0["成交均价"] as string);
                        委托Row1.成交数量 = decimal.Parse(DataRow0["成交数量"] as string);
                        委托Row1.状态说明 = DataRow0["状态说明"] as string;
                        委托Row1.撤单数量 = decimal.Parse(DataRow0["撤单数量"] as string);
                        break;
                    default:
                        throw new Exception(string.Format("不支持 {0}{1} 帐户", this.券商, this.类型));
                }
            }

            void Get成交Info(string 委托编号, out decimal 成交数量, out decimal 成交价格)
            {
                if (this.帐户成交DataTable.Any(r => r.委托编号 == 委托编号))
                {
                    var rows = this.帐户成交DataTable.Where(r => r.委托编号 == 委托编号 && r.成交数量 > 0 && r.成交金额 == 0).ToList();
                    if (rows.Count > 0)
                    {
                        rows.ForEach(_ =>
                        {
                            if (_.成交价格 > 0)
                            {
                                _.成交金额 = _.成交价格 * _.成交数量;
                            }
                        });
                    }
                    decimal 成交金额 = this.帐户成交DataTable.Where(r => r.委托编号 == 委托编号).Sum(r => r.成交金额);
                    成交数量 = this.帐户成交DataTable.Where(r => r.委托编号 == 委托编号).Sum(r => r.成交数量);
                    成交价格 = Math.Round(成交金额 / 成交数量, 3, MidpointRounding.AwayFromZero);
                }
                else
                {
                    成交数量 = 0;
                    成交价格 = 0;
                }
            }


            public string GetDataRow委托编号(DataRow DataRow0)
            {
                if (this.券商 == "银河证券" && this.类型 == "信用")
                {
                    return DataRow0["合同编号"] as string;
                }
                else
                {
                    if (DataRow0.Table.Columns.Contains("委托编号"))
                    {
                        return DataRow0["委托编号"] as string;
                    }
                    else if (DataRow0.Table.Columns.Contains("contractNum"))
                    {
                        return DataRow0["contractNum"] as string;
                    }
                    else
                    {
                        return "0";
                    }
                }
            }

            ConcurrentDictionary<string, DateTime> dictOrderSend = new ConcurrentDictionary<string, DateTime>();
            ConcurrentDictionary<string, DateTime> dictOrderCancel = new ConcurrentDictionary<string, DateTime>();

            public void SendOrder(int Category, byte Market, string Zqdm, decimal Price, decimal Quantity, OrderCacheEntity orderCacheObj, out string Result, out string ErrInfo, out bool hasOrderNo)
            {
                char directory = Category % 2 == 0 ? '0' : '1';
                string market = Market == 0 ? "" : "SZE";

                orderCacheObj.GroupName = this.名称;
                orderCacheObj.Market = Market;
                hasOrderNo = true;//只有CATS接口会用到这个参数，用于区别普通A股下单。因为此接口返回的是客户端自己生成的id，在一段事件后才会有接口生成的id。
                if (!Tool.IsSendOrderTimeFit())
                {
                    ErrInfo = string.Format("下单时限为9:00-15:00, 当前时间{0}超出下单时限", DateTime.Now);
                    Result = string.Empty;
                    return;
                }

                Result = string.Empty;
                ErrInfo = string.Empty;

                if (this.ClientID == -1)
                {
                    ErrInfo = string.Format("{0}未登录到券商交易服务器", this.名称);
                    return;
                }

                lock (this.SendOrderObject)
                {
                    Result = string.Empty;
                    ErrInfo = string.Empty;

                    if (this.ClientID == -1)
                    {
                        ErrInfo = string.Format("{0}未登录到券商交易服务器", this.名称);
                        return;
                    }

                    //int orderID = KFApi.insert_market_order(Zqdm, market, Math.Round((double)Price, 2), (int)Quantity, directory, 'N');

                    //if (orderID > 0)
                    //{
                    //    Result = orderID.ToString();
                    //}
                }

            }

            public void CancelOrder(string Zqdm, byte Market, string hth, out string Result, out string ErrInfo)
            {
                Result = string.Empty;
                ErrInfo = string.Empty;

                if (this.ClientID == -1)
                {
                    ErrInfo = string.Format("{0}未登录到券商交易服务器", this.名称);
                    return;
                }

                StringBuilder Result1 = new StringBuilder(1024 * 1024);
                StringBuilder ErrInfo1 = new StringBuilder(256);

                //string ExchangeID = null;
                //if (this.券商 == "招商证券" && this.类型 == "普通")
                //{
                //    ExchangeID = (Market == 1 ? "1" : "2");
                //}
                //else
                //{
                //    ExchangeID = Market.ToString();
                //}

                //TdxApi.CancelOrder(ClientID, ExchangeID.ToString(), hth, Result1, ErrInfo1);


                //ErrInfo = ErrInfo1.ToString();

                //KFApi.cancel_order(int.Parse(hth));
                if (ErrInfo == string.Empty)
                {
                    Result = "撤单成功";
                }
                else
                {
                    Result = string.Empty;
                }
            }

            public void Repay(string amount, StringBuilder Result, StringBuilder ErrInfo)
            {
                if (this.ClientID > -1)
                {
                    //TdxApi.Repay(this.ClientID, amount, Result, ErrInfo);
                }
                else
                {
                    ErrInfo.AppendFormat("{0}: ClientID为-1", this.名称);
                }
            }
        }


        partial class 订单DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["交易员"], this.Columns["组合号"], this.Columns["证券代码"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (订单 订单1 in db.订单)
                        {
                            订单Row 订单Row1 = this.New订单Row();
                            订单Row1.交易员 = 订单1.交易员;
                            订单Row1.组合号 = 订单1.组合号;
                            订单Row1.证券代码 = 订单1.证券代码;
                            订单Row1.证券名称 = 订单1.证券名称;
                            订单Row1.开仓时间 = 订单1.开仓时间;
                            订单Row1.开仓类别 = 订单1.开仓类别;
                            订单Row1.已开数量 = 订单1.已开数量;
                            订单Row1.已开金额 = 订单1.已开金额;
                            订单Row1.开仓价位 = 订单1.开仓价位;
                            订单Row1.当前价位 = 订单1.当前价位;
                            订单Row1.浮动盈亏 = 订单1.浮动盈亏;
                            订单Row1.平仓时间 = 订单1.平仓时间;
                            订单Row1.平仓类别 = 订单1.平仓类别;
                            订单Row1.已平数量 = 订单1.已平数量;
                            订单Row1.已平金额 = 订单1.已平金额;
                            订单Row1.平仓价位 = 订单1.平仓价位;
                            订单Row1.市场代码 = 订单1.市场代码;
                            this.Add订单Row(订单Row1);
                        }
                    }

                    this.订单RowChanging += 订单_订单RowChanging;
                    this.订单RowDeleting += 订单_订单RowChanging;

                }
            }

            void 订单_订单RowChanging(object sender, DbDataSet.订单RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:

                            订单 订单1 = new 订单();
                            订单1.交易员 = e.Row.交易员;
                            订单1.组合号 = e.Row.组合号;
                            订单1.证券代码 = e.Row.证券代码;
                            订单1.证券名称 = e.Row.证券名称;
                            订单1.开仓时间 = e.Row.开仓时间;
                            订单1.开仓类别 = e.Row.开仓类别;
                            订单1.已开数量 = e.Row.已开数量;
                            订单1.已开金额 = e.Row.已开金额;
                            订单1.开仓价位 = e.Row.开仓价位;
                            订单1.当前价位 = e.Row.当前价位;
                            订单1.浮动盈亏 = e.Row.浮动盈亏;
                            订单1.平仓时间 = e.Row.平仓时间;
                            订单1.平仓类别 = e.Row.平仓类别;
                            订单1.已平数量 = e.Row.已平数量;
                            订单1.已平金额 = e.Row.已平金额;
                            订单1.平仓价位 = e.Row.平仓价位;
                            订单1.市场代码 = e.Row.市场代码;
                            db.订单.Add(订单1);
                            db.SaveChanges();



                            Program.订单表Changed[e.Row.交易员] = true;
                            break;
                        case DataRowAction.Delete:
                            订单1 = db.订单.Find(e.Row.交易员, e.Row.组合号, e.Row.证券代码);
                            db.订单.Remove(订单1);
                            db.SaveChanges();


                            Program.订单表Changed[e.Row.交易员] = true;

                            break;
                        case DataRowAction.Change:
                            订单1 = db.订单.First(r => r.交易员 == e.Row.交易员 && r.组合号 == e.Row.组合号 && r.证券代码 == e.Row.证券代码);
                            订单1.交易员 = e.Row.交易员;
                            订单1.组合号 = e.Row.组合号;
                            订单1.证券代码 = e.Row.证券代码;
                            订单1.证券名称 = e.Row.证券名称;
                            订单1.开仓时间 = e.Row.开仓时间;
                            订单1.开仓类别 = e.Row.开仓类别;
                            订单1.已开数量 = e.Row.已开数量;
                            订单1.已开金额 = e.Row.已开金额;
                            订单1.开仓价位 = e.Row.开仓价位;
                            订单1.当前价位 = e.Row.当前价位;
                            订单1.浮动盈亏 = e.Row.浮动盈亏;
                            订单1.平仓时间 = e.Row.平仓时间;
                            订单1.平仓类别 = e.Row.平仓类别;
                            订单1.已平数量 = e.Row.已平数量;
                            订单1.已平金额 = e.Row.已平金额;
                            订单1.平仓价位 = e.Row.平仓价位;
                            订单1.市场代码 = e.Row.市场代码;
                            db.Entry(订单1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            Program.订单表Changed[e.Row.交易员] = true;


                            break;
                        default:
                            break;
                    }
                }
            }

            public 订单Row Get订单(string 交易员, string 组合号, string 证券代码)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.订单Row 订单1 = this.FirstOrDefault(r => r.交易员 == 交易员 && r.组合号 == 组合号 && r.证券代码 == 证券代码);
                    if (订单1 == null)
                    {
                        return null;
                    }



                    订单DataTable 订单DataTable1 = new 订单DataTable();
                    订单DataTable1.ImportRow(订单1);
                    return 订单DataTable1[0];
                }
            }

            public 订单DataTable Query订单BelongJy(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    订单DataTable 订单DataTable1 = new 订单DataTable();
                    foreach (订单Row 订单Row1 in this.Where(r => r.交易员 == UserName))
                    {
                        订单DataTable1.ImportRow(订单Row1);
                    }
                    return 订单DataTable1;
                }
            }

            public bool Exists(JyDataSet.成交Row 成交Row1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(r => r.交易员 == 成交Row1.交易员 && r.组合号 == 成交Row1.组合号 && r.证券代码 == 成交Row1.证券代码);
                }
            }

            public void Add(JyDataSet.成交Row 成交Row1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    订单Row 订单1 = this.New订单Row();
                    订单1.交易员 = 成交Row1.交易员;
                    订单1.组合号 = 成交Row1.组合号;
                    订单1.证券代码 = 成交Row1.证券代码;
                    订单1.证券名称 = 成交Row1.证券名称;
                    订单1.开仓时间 = DateTime.Now;
                    订单1.开仓类别 = 成交Row1.买卖方向;
                    订单1.已开数量 = 成交Row1.成交数量;
                    订单1.已开金额 = 成交Row1.成交金额;
                    订单1.开仓价位 = 成交Row1.成交价格;
                    订单1.当前价位 = 成交Row1.成交价格;
                    订单1.浮动盈亏 = 0;
                    订单1.平仓时间 = DateTime.MaxValue.Date;

                    //if (订单1.开仓类别 == 3)
                    //{
                    //    订单1.平仓类别 = 0;
                    //}
                    //else if (订单1.开仓类别 == 2)
                    //{
                    //    订单1.平仓类别 = 1;
                    //}
                    订单1.平仓类别 = 1 - 订单1.开仓类别 % 2;

                    订单1.已平数量 = 0;
                    订单1.已平金额 = 0;
                    订单1.平仓价位 = 0;
                    订单1.市场代码 = 成交Row1.市场代码;
                    this.Add订单Row(订单1);
                }



            }

            public void Update(JyDataSet.成交Row 成交Row1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.订单Row 订单1 = this.First(r => r.交易员 == 成交Row1.交易员 && r.组合号 == 成交Row1.组合号 && r.证券代码 == 成交Row1.证券代码);

                    //Program.logger.LogInfoDetail("订单计算: 当前订单, 已开数量{0}, 已开金额{1}, 开仓价位{2}; 新增成交 成交数量{3}, 成交金额{4}, 成交价格{5}",
                    //    订单1.已开数量, 订单1.已平金额, 订单1.开仓价位, 成交Row1.成交数量, 成交Row1.成交金额, 成交Row1.成交价格);

                    if (订单1.开仓类别 % 2 == 成交Row1.买卖方向 % 2)
                    {
                        订单1.已开数量 += 成交Row1.成交数量;
                        订单1.已开金额 += 成交Row1.成交金额;
                        if (订单1.已开数量 <= 0)
                        {
                            订单1.开仓价位 = 成交Row1.成交价格;
                        }
                        else
                        {
                            订单1.开仓价位 = Math.Round(订单1.已开金额 / 订单1.已开数量, 3, MidpointRounding.AwayFromZero);
                        }

                    }
                    else
                    {
                        订单1.已平数量 += 成交Row1.成交数量;
                        订单1.已平金额 += 成交Row1.成交金额;
                        if (订单1.已平数量 <= 0)
                        {
                            订单1.平仓价位 = Math.Round(成交Row1.成交价格, 3, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            订单1.平仓价位 = Math.Round(订单1.已平金额 / 订单1.已平数量, 3, MidpointRounding.AwayFromZero);
                        }

                    }


                    订单1.Deal();



                    this.AcceptChanges();


                }
            }

            public DbDataSet DbDataSet
            {
                get
                {
                    return this.DataSet as DbDataSet;
                }
            }
        }

        partial class 订单Row
        {
            public void Deal()
            {
                #region 挨个处理订单

                if (this.已开数量 > this.已平数量 && this.已平数量 > 0)
                {
                    #region 对冲1
                    decimal 平掉数量 = this.已平数量;
                    decimal 平掉金额 = this.已平金额;
                    //订单Row1.证券代码 = Zqdm;
                    //订单Row1.证券名称 = Zqmc;
                    //订单Row1.开仓日期 = DateTime.Today;
                    //订单Row1.开仓类别 = Mmfx;
                    this.已开数量 -= 平掉数量;
                    this.已开金额 = this.开仓价位 * this.已开数量;
                    //this.开仓价位 = Math.Round(this.已开金额 / this.已开数量, 3, MidpointRounding.AwayFromZero);
                    //订单Row1.当前价位 = Cjjg;
                    //订单Row1.浮动盈亏 = 0;
                    //订单Row1.平仓日期 = DateTime.Today;
                    //订单Row1.平仓类别 = 1 - 订单Row1.开仓类别;
                    this.已平数量 = 0;
                    this.已平金额 = 0;
                    this.平仓价位 = 0;







                    平台用户Row AASUser1 = this.table订单.DbDataSet.平台用户.Get平台用户(this.交易员);



                    this.table订单.DbDataSet.已平仓订单.Add(平掉数量, 平掉金额, this, AASUser1);





                    #endregion


                }
                else if (this.已开数量 < this.已平数量 && this.已开数量 > 0)
                {

                    #region 对冲2
                    decimal 平掉数量 = this.已开数量;
                    decimal 平掉金额 = Math.Round(this.已平金额 * this.已开数量 / this.已平数量, 2, MidpointRounding.AwayFromZero);
                    DateTime Old开仓时间 = this.开仓时间;
                    decimal Old开仓价位 = this.开仓价位;

                    //订单Row1.证券代码 = Zqdm;
                    //订单Row1.证券名称 = Zqmc;
                    this.开仓时间 = DateTime.Now;
                    this.开仓类别 = this.平仓类别;
                    this.已开数量 = this.已平数量 - 平掉数量;
                    this.已开金额 = this.已平金额 - 平掉金额;
                    this.开仓价位 = Math.Round(this.已开金额 / this.已开数量, 3, MidpointRounding.AwayFromZero);
                    //订单Row1.当前价位 = Cjjg;
                    //订单Row1.浮动盈亏 = 0;
                    //订单Row1.平仓日期 = DateTime.Today;
                    this.平仓类别 = 1 - this.开仓类别;
                    this.已平数量 = 0;
                    this.已平金额 = 0;
                    this.平仓价位 = 0;





                    平台用户Row AASUser1 = this.table订单.DbDataSet.平台用户.Get平台用户(this.交易员);


                    this.table订单.DbDataSet.已平仓订单.Add(平掉数量, 平掉金额, Old开仓时间, Old开仓价位, this, AASUser1);





                    #endregion

                }
                else if (this.已开数量 == this.已平数量 && this.已开数量 > 0)
                {
                    #region 对冲3

                    平台用户Row AASUser1 = this.table订单.DbDataSet.平台用户.Get平台用户(this.交易员);


                    this.table订单.DbDataSet.已平仓订单.Add(this, AASUser1);


                    this.Delete();

                    #endregion



                }
                else
                {

                }
                #endregion
            }

        }


        partial class 已平仓订单DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();


            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (已平仓订单 已平仓订单1 in db.已平仓订单)
                        {
                            if (已平仓订单1.平仓日期 == DateTime.Today)
                            {
                                已平仓订单Row 已平仓订单Row1 = this.New已平仓订单Row();
                                已平仓订单Row1.交易员 = 已平仓订单1.交易员;
                                已平仓订单Row1.组合号 = 已平仓订单1.组合号;
                                已平仓订单Row1.证券代码 = 已平仓订单1.证券代码;
                                已平仓订单Row1.证券名称 = 已平仓订单1.证券名称;
                                已平仓订单Row1.开仓时间 = 已平仓订单1.开仓时间;
                                已平仓订单Row1.开仓类别 = 已平仓订单1.开仓类别;
                                已平仓订单Row1.已开数量 = 已平仓订单1.已开数量;
                                已平仓订单Row1.已开金额 = 已平仓订单1.已开金额;
                                已平仓订单Row1.开仓价位 = 已平仓订单1.开仓价位;
                                已平仓订单Row1.平仓类别 = 已平仓订单1.平仓类别;
                                已平仓订单Row1.已平数量 = 已平仓订单1.已平数量;
                                已平仓订单Row1.已平金额 = 已平仓订单1.已平金额;
                                已平仓订单Row1.平仓价位 = 已平仓订单1.平仓价位;
                                已平仓订单Row1.平仓时间 = 已平仓订单1.平仓时间;
                                已平仓订单Row1.毛利 = 已平仓订单1.毛利;

                                this.Add已平仓订单Row(已平仓订单Row1);
                            }

                        }
                    }

                    this.已平仓订单RowChanging += 已平仓订单_已平仓订单RowChanging;
                    this.已平仓订单RowDeleting += 已平仓订单_已平仓订单RowChanging;



                }
            }


            public void Load(List<string> JyList, DateTime StartDate, DateTime EndDate)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (已平仓订单 已平仓订单1 in db.已平仓订单.Where(r => JyList.Contains(r.交易员)))
                        {
                            if (StartDate <= 已平仓订单1.平仓日期 && 已平仓订单1.平仓日期 <= EndDate)
                            {
                                Server.DbDataSet.已平仓订单Row 已平仓订单Row1 = this.New已平仓订单Row();
                                已平仓订单Row1.交易员 = 已平仓订单1.交易员;
                                已平仓订单Row1.组合号 = 已平仓订单1.组合号;
                                已平仓订单Row1.证券代码 = 已平仓订单1.证券代码;
                                已平仓订单Row1.证券名称 = 已平仓订单1.证券名称;
                                已平仓订单Row1.开仓时间 = 已平仓订单1.开仓时间;
                                已平仓订单Row1.开仓类别 = 已平仓订单1.开仓类别;
                                已平仓订单Row1.已开数量 = 已平仓订单1.已开数量;
                                已平仓订单Row1.已开金额 = 已平仓订单1.已开金额;
                                已平仓订单Row1.开仓价位 = 已平仓订单1.开仓价位;
                                已平仓订单Row1.平仓类别 = 已平仓订单1.平仓类别;
                                已平仓订单Row1.已平数量 = 已平仓订单1.已平数量;
                                已平仓订单Row1.已平金额 = 已平仓订单1.已平金额;
                                已平仓订单Row1.平仓价位 = 已平仓订单1.平仓价位;
                                已平仓订单Row1.平仓时间 = 已平仓订单1.平仓时间;
                                已平仓订单Row1.毛利 = 已平仓订单1.毛利;

                                this.Add已平仓订单Row(已平仓订单Row1);
                                if (已平仓订单1.证券代码.Length > 6)
                                {
                                    已平仓订单Row1.证券代码 = 已平仓订单1.证券代码.Trim();
                                    Program.logger.LogInfo("已平仓订单出现长度大于6的证券代码,证券代码{0},开仓时间{1}.", 已平仓订单1.证券代码, 已平仓订单1.开仓时间);
                                }
                            }
                        }
                    }
                }
            }



            void 已平仓订单_已平仓订单RowChanging(object sender, DbDataSet.已平仓订单RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    if (e.Action == DataRowAction.Add)
                    {
                        已平仓订单 已平仓订单1 = new 已平仓订单();
                        已平仓订单1.交易员 = e.Row.交易员;
                        已平仓订单1.组合号 = e.Row.组合号;
                        已平仓订单1.证券代码 = e.Row.证券代码;
                        已平仓订单1.证券名称 = e.Row.证券名称;
                        已平仓订单1.开仓时间 = e.Row.开仓时间;
                        已平仓订单1.开仓类别 = e.Row.开仓类别;
                        已平仓订单1.已开数量 = e.Row.已开数量;
                        已平仓订单1.已开金额 = e.Row.已开金额;
                        已平仓订单1.开仓价位 = e.Row.开仓价位;
                        已平仓订单1.平仓类别 = e.Row.平仓类别;
                        已平仓订单1.已平数量 = e.Row.已平数量;
                        已平仓订单1.已平金额 = e.Row.已平金额;
                        已平仓订单1.平仓价位 = e.Row.平仓价位;
                        已平仓订单1.平仓时间 = e.Row.平仓时间;
                        已平仓订单1.毛利 = e.Row.毛利;


                        db.已平仓订单.Add(已平仓订单1);
                        db.SaveChanges();

                        Program.已平仓订单表Changed[e.Row.交易员] = true;

                    }
                }
            }








            public decimal Get当日已平仓亏损(string UserName)
            {

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    decimal 当日已平仓毛利 = 0;
                    foreach (已平仓订单Row 已平仓订单Row1 in this.Where(r => r.交易员 == UserName))
                    {
                        if (已平仓订单Row1.平仓日期 == DateTime.Today)
                        {
                            当日已平仓毛利 += 已平仓订单Row1.毛利;
                        }
                    }
                    return -当日已平仓毛利;
                }

            }



            public 已平仓订单DataTable Query已平仓订单BelongJy(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.已平仓订单DataTable 已平仓订单DataTable1 = new DbDataSet.已平仓订单DataTable();
                    foreach (Server.DbDataSet.已平仓订单Row 已平仓订单Row1 in this.Where(r => r.交易员 == UserName))
                    {
                        if (已平仓订单Row1.平仓日期 == DateTime.Today)
                        {
                            已平仓订单DataTable1.ImportRow(已平仓订单Row1);
                        }
                    }
                    return 已平仓订单DataTable1;
                }
            }








            public void Add(decimal 平掉数量, decimal 平掉金额, 订单Row 订单Row1, 平台用户Row AASUser1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    已平仓订单Row 已平仓订单Row1 = this.New已平仓订单Row();
                    已平仓订单Row1.交易员 = 订单Row1.交易员;
                    已平仓订单Row1.组合号 = 订单Row1.组合号;
                    已平仓订单Row1.证券代码 = 订单Row1.证券代码;
                    已平仓订单Row1.证券名称 = 订单Row1.证券名称;
                    已平仓订单Row1.开仓时间 = 订单Row1.开仓时间;
                    已平仓订单Row1.开仓类别 = 订单Row1.开仓类别;
                    已平仓订单Row1.已开数量 = 平掉数量;
                    已平仓订单Row1.已开金额 = 订单Row1.开仓价位 * 平掉数量;
                    已平仓订单Row1.开仓价位 = 订单Row1.开仓价位;
                    已平仓订单Row1.平仓类别 = 订单Row1.平仓类别;
                    已平仓订单Row1.已平数量 = 平掉数量;
                    已平仓订单Row1.已平金额 = 平掉金额;
                    已平仓订单Row1.平仓价位 = Math.Round(平掉金额 / 平掉数量, 3, MidpointRounding.AwayFromZero);
                    已平仓订单Row1.平仓时间 = DateTime.Now;
                    已平仓订单Row1.毛利 = 已平仓订单Row1.计算毛利();

                    this.Add已平仓订单Row(已平仓订单Row1);
                }



            }

            public void Add(decimal 平掉数量, decimal 平掉金额, DateTime Old开仓时间, decimal Old开仓价位, 订单Row 订单Row1, 平台用户Row AASUser1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    已平仓订单Row 已平仓订单Row1 = this.New已平仓订单Row();
                    已平仓订单Row1.交易员 = 订单Row1.交易员;
                    已平仓订单Row1.组合号 = 订单Row1.组合号;
                    已平仓订单Row1.证券代码 = 订单Row1.证券代码;
                    已平仓订单Row1.证券名称 = 订单Row1.证券名称;
                    已平仓订单Row1.开仓时间 = Old开仓时间;
                    已平仓订单Row1.开仓类别 = 订单Row1.平仓类别;
                    已平仓订单Row1.已开数量 = 平掉数量;
                    已平仓订单Row1.已开金额 = Old开仓价位 * 平掉数量;
                    已平仓订单Row1.开仓价位 = Old开仓价位;
                    已平仓订单Row1.平仓类别 = 订单Row1.开仓类别;
                    已平仓订单Row1.已平数量 = 平掉数量;
                    已平仓订单Row1.已平金额 = 平掉金额;
                    已平仓订单Row1.平仓价位 = Math.Round(平掉金额 / 平掉数量, 3, MidpointRounding.AwayFromZero);
                    已平仓订单Row1.平仓时间 = DateTime.Now;
                    已平仓订单Row1.毛利 = 已平仓订单Row1.计算毛利();


                    this.Add已平仓订单Row(已平仓订单Row1);
                }




            }


            public void Add(订单Row 订单Row1, 平台用户Row AASUser1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    已平仓订单Row 已平仓订单Row1 = this.New已平仓订单Row();

                    已平仓订单Row1.交易员 = 订单Row1.交易员;
                    已平仓订单Row1.组合号 = 订单Row1.组合号;
                    已平仓订单Row1.证券代码 = 订单Row1.证券代码;
                    已平仓订单Row1.证券名称 = 订单Row1.证券名称;
                    已平仓订单Row1.开仓时间 = 订单Row1.开仓时间;
                    已平仓订单Row1.开仓类别 = 订单Row1.开仓类别;
                    已平仓订单Row1.已开数量 = 订单Row1.已开数量;
                    已平仓订单Row1.已开金额 = 订单Row1.已开金额;
                    已平仓订单Row1.开仓价位 = Math.Round(订单Row1.已开金额 / 订单Row1.已开数量, 3, MidpointRounding.AwayFromZero);
                    已平仓订单Row1.平仓类别 = 订单Row1.平仓类别;
                    已平仓订单Row1.已平数量 = 订单Row1.已平数量;
                    已平仓订单Row1.已平金额 = 订单Row1.已平金额;
                    已平仓订单Row1.平仓价位 = Math.Round(订单Row1.已平金额 / 订单Row1.已平数量, 3, MidpointRounding.AwayFromZero);
                    已平仓订单Row1.平仓时间 = DateTime.Now;
                    已平仓订单Row1.毛利 = 已平仓订单Row1.计算毛利();




                    this.Add已平仓订单Row(已平仓订单Row1);
                }



            }


        }

        partial class 已平仓订单Row
        {

            public DateTime 平仓日期
            {
                get
                {
                    return this.平仓时间.Date;
                }
            }


            public decimal 计算毛利()
            {
                return this.开仓类别 == 0 ? this.已平金额 - this.已开金额 : this.已开金额 - this.已平金额;

            }
        }


        partial class 已处理成交DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["日期"], this.Columns["组合号"], this.Columns["委托编号"], this.Columns["成交编号"] };



                    using (AASDbContext db = new AASDbContext())
                    {
                        var yesterday = DateTime.Today.AddDays(-1);
                        foreach (已处理成交 已处理成交1 in db.已处理成交.Where(r => r.日期 >= yesterday))
                        {
                            已处理成交Row 已处理成交Row1 = this.New已处理成交Row();
                            已处理成交Row1.日期 = 已处理成交1.日期;
                            已处理成交Row1.组合号 = 已处理成交1.组合号;
                            已处理成交Row1.委托编号 = 已处理成交1.委托编号;
                            已处理成交Row1.成交编号 = 已处理成交1.成交编号;
                            this.Add已处理成交Row(已处理成交Row1);
                        }
                    }


                    this.已处理成交RowChanging += 已处理成交_已处理成交RowChanging;
                    this.已处理成交RowDeleting += 已处理成交_已处理成交RowChanging;
                }
            }


            void 已处理成交_已处理成交RowChanging(object sender, DbDataSet.已处理成交RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            已处理成交 已处理成交1 = new 已处理成交();
                            已处理成交1.日期 = e.Row.日期;
                            已处理成交1.组合号 = e.Row.组合号;
                            已处理成交1.委托编号 = e.Row.委托编号;
                            已处理成交1.成交编号 = e.Row.成交编号;
                            db.已处理成交.Add(已处理成交1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            已处理成交1 = db.已处理成交.Find(e.Row.日期, e.Row.组合号, e.Row.委托编号, e.Row.成交编号);
                            db.已处理成交.Remove(已处理成交1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Change:
                            已处理成交1 = db.已处理成交.Find(e.Row.日期, e.Row.组合号, e.Row.委托编号, e.Row.成交编号);
                            已处理成交1.日期 = e.Row.日期;
                            已处理成交1.组合号 = e.Row.组合号;
                            已处理成交1.委托编号 = e.Row.委托编号;
                            已处理成交1.成交编号 = e.Row.成交编号;
                            db.Entry(已处理成交1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }




                }
            }



            public bool Exists(JyDataSet.成交Row 成交Row1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(r => r.日期 == DateTime.Today && r.组合号 == 成交Row1.组合号 && r.委托编号 == 成交Row1.委托编号 && r.成交编号 == 成交Row1.成交编号);
                }
            }

            public void Add(JyDataSet.成交Row 成交Row1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.已处理成交Row 已处理成交1 = this.New已处理成交Row();
                    已处理成交1.日期 = DateTime.Today;
                    已处理成交1.组合号 = 成交Row1.组合号;
                    已处理成交1.成交编号 = 成交Row1.成交编号;
                    已处理成交1.委托编号 = 成交Row1.委托编号;
                    this.Add已处理成交Row(已处理成交1);
                }
            }
        }


        partial class 已发委托DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["日期"], this.Columns["组合号"], this.Columns["委托编号"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        var yesterday = DateTime.Today.AddDays(-1);
                        foreach (已发委托 已发委托1 in db.已发委托.Where(r => r.日期 >= yesterday))
                        {
                            已发委托Row 已发委托Row1 = this.New已发委托Row();
                            已发委托Row1.日期 = 已发委托1.日期;
                            已发委托Row1.组合号 = 已发委托1.组合号;
                            已发委托Row1.委托编号 = 已发委托1.委托编号;
                            已发委托Row1.交易员 = 已发委托1.交易员;
                            已发委托Row1.状态说明 = 已发委托1.状态说明;
                            已发委托Row1.市场代码 = 已发委托1.市场代码;
                            已发委托Row1.证券代码 = 已发委托1.证券代码;
                            已发委托Row1.证券名称 = 已发委托1.证券名称;
                            已发委托Row1.买卖方向 = 已发委托1.买卖方向;
                            已发委托Row1.成交价格 = 已发委托1.成交价格;
                            已发委托Row1.成交数量 = 已发委托1.成交数量;
                            已发委托Row1.委托价格 = 已发委托1.委托价格;
                            已发委托Row1.委托数量 = 已发委托1.委托数量;
                            已发委托Row1.撤单数量 = 已发委托1.撤单数量;
                            this.Add已发委托Row(已发委托Row1);
                        }
                    }

                    this.已发委托RowChanging += 已发委托_已发委托RowChanging;
                    this.已发委托RowDeleting += 已发委托_已发委托RowChanging;

                }
            }

            public void Load(List<string> JyList, DateTime StartDate, DateTime EndDate)
            {

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["日期"], this.Columns["组合号"], this.Columns["委托编号"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (已发委托 已发委托1 in db.已发委托.Where(r => JyList.Contains(r.交易员) && StartDate <= r.日期 && r.日期 <= EndDate))
                        {
                            已发委托Row 已发委托Row1 = this.New已发委托Row();
                            已发委托Row1.日期 = 已发委托1.日期;
                            已发委托Row1.组合号 = 已发委托1.组合号;
                            已发委托Row1.委托编号 = 已发委托1.委托编号;
                            已发委托Row1.交易员 = 已发委托1.交易员;
                            已发委托Row1.状态说明 = 已发委托1.状态说明;
                            已发委托Row1.市场代码 = 已发委托1.市场代码;
                            已发委托Row1.证券代码 = 已发委托1.证券代码;
                            已发委托Row1.证券名称 = 已发委托1.证券名称;
                            已发委托Row1.买卖方向 = 已发委托1.买卖方向;
                            已发委托Row1.成交价格 = 已发委托1.成交价格;
                            已发委托Row1.成交数量 = 已发委托1.成交数量;
                            已发委托Row1.委托价格 = 已发委托1.委托价格;
                            已发委托Row1.委托数量 = 已发委托1.委托数量;
                            已发委托Row1.撤单数量 = 已发委托1.撤单数量;
                            this.Add已发委托Row(已发委托Row1);
                            if (已发委托1.证券代码.Length > 6)
                            {
                                已发委托Row1.证券代码 = 已发委托1.证券代码.Trim();
                                Program.logger.LogInfo("出现长度大于6的证券代码,{0}.", 已发委托1.证券代码);
                            }
                        }
                    }

                }

            }

            //public void LoadToday()
            //{
            //    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
            //    {
            //        this.PrimaryKey = new DataColumn[] { this.Columns["日期"], this.Columns["组合号"], this.Columns["委托编号"] };


            //        using (AASDbContext db = new AASDbContext())
            //        {
            //            var today = DateTime.Today;
            //            foreach (已发委托 已发委托1 in db.已发委托.Where(r => r.日期 >= today))
            //            {
            //                已发委托Row 已发委托Row1 = this.New已发委托Row();
            //                已发委托Row1.日期 = 已发委托1.日期;
            //                已发委托Row1.组合号 = 已发委托1.组合号;
            //                已发委托Row1.委托编号 = 已发委托1.委托编号;
            //                已发委托Row1.交易员 = 已发委托1.交易员;
            //                已发委托Row1.状态说明 = 已发委托1.状态说明;
            //                已发委托Row1.市场代码 = 已发委托1.市场代码;
            //                已发委托Row1.证券代码 = 已发委托1.证券代码;
            //                已发委托Row1.证券名称 = 已发委托1.证券名称;
            //                已发委托Row1.买卖方向 = 已发委托1.买卖方向;
            //                已发委托Row1.成交价格 = 已发委托1.成交价格;
            //                已发委托Row1.成交数量 = 已发委托1.成交数量;
            //                已发委托Row1.委托价格 = 已发委托1.委托价格;
            //                已发委托Row1.委托数量 = 已发委托1.委托数量;
            //                已发委托Row1.撤单数量 = 已发委托1.撤单数量;
            //                this.Add已发委托Row(已发委托Row1);
            //            }
            //        }

            //    }
            //}

            void 已发委托_已发委托RowChanging(object sender, DbDataSet.已发委托RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            已发委托 已发委托1 = new 已发委托();
                            已发委托1.日期 = e.Row.日期;
                            已发委托1.组合号 = e.Row.组合号;
                            已发委托1.委托编号 = e.Row.委托编号;
                            已发委托1.交易员 = e.Row.交易员;
                            已发委托1.状态说明 = e.Row.状态说明;
                            已发委托1.市场代码 = e.Row.市场代码;
                            已发委托1.证券代码 = e.Row.证券代码;
                            已发委托1.证券名称 = e.Row.证券名称;
                            已发委托1.买卖方向 = e.Row.买卖方向;
                            已发委托1.成交价格 = e.Row.成交价格;
                            已发委托1.成交数量 = e.Row.成交数量;

                            已发委托1.委托价格 = e.Row.委托价格;
                            已发委托1.委托数量 = e.Row.委托数量;
                            已发委托1.撤单数量 = e.Row.撤单数量;

                            db.已发委托.Add(已发委托1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            已发委托1 = db.已发委托.Find(e.Row.日期, e.Row.组合号, e.Row.委托编号);
                            db.已发委托.Remove(已发委托1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Change:
                            已发委托1 = db.已发委托.Find(e.Row.日期, e.Row.组合号, e.Row.委托编号);
                            已发委托1.日期 = e.Row.日期;
                            已发委托1.组合号 = e.Row.组合号;
                            已发委托1.委托编号 = e.Row.委托编号;
                            已发委托1.交易员 = e.Row.交易员;
                            已发委托1.状态说明 = e.Row.状态说明;
                            已发委托1.市场代码 = e.Row.市场代码;
                            已发委托1.证券代码 = e.Row.证券代码;
                            已发委托1.证券名称 = e.Row.证券名称;
                            已发委托1.买卖方向 = e.Row.买卖方向;
                            已发委托1.成交价格 = e.Row.成交价格;
                            已发委托1.成交数量 = e.Row.成交数量;

                            已发委托1.委托价格 = e.Row.委托价格;
                            已发委托1.委托数量 = e.Row.委托数量;
                            已发委托1.撤单数量 = e.Row.撤单数量;

                            db.Entry(已发委托1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }
                }
            }


            public List<string> GetOrderIDList(DateTime 日期, string 组合号)
            {
                List<string> arrID = null;
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    arrID = this.Where(r => r.日期 == 日期 && r.组合号 == 组合号).Select(_ => _.委托编号).ToList();
                }
                return arrID;
            }

            public JyDataSet.委托DataTable Get风控平仓委托DataTable()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    JyDataSet.委托DataTable 委托DataTable1 = new JyDataSet.委托DataTable();

                    foreach (Server.DbDataSet.已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.状态说明 == "风控平仓"))//已发委托 已发委托1 in db.已发委托.Where(r => r.交易员 == UserName && r.日期 == DateTime.Today  && r.证券代码 == 证券代码))
                    {
                        JyDataSet.委托Row 委托Row1 = 委托DataTable1.New委托Row();
                        委托Row1.交易员 = 已发委托Row1.交易员;
                        委托Row1.组合号 = 已发委托Row1.组合号;
                        委托Row1.委托时间 = "00:00:00";
                        委托Row1.证券代码 = 已发委托Row1.证券代码;
                        委托Row1.证券名称 = 已发委托Row1.证券名称;
                        委托Row1.委托价格 = 已发委托Row1.委托价格;
                        委托Row1.委托数量 = 已发委托Row1.委托数量;
                        委托Row1.成交价格 = 已发委托Row1.委托价格;
                        委托Row1.成交数量 = 已发委托Row1.委托数量;
                        委托Row1.撤单数量 = 0;
                        委托Row1.状态说明 = 已发委托Row1.状态说明;
                        委托Row1.委托编号 = 已发委托Row1.委托编号;
                        委托Row1.买卖方向 = 已发委托Row1.买卖方向;
                        委托Row1.市场代码 = 已发委托Row1.市场代码;
                        委托DataTable1.Add委托Row(委托Row1);
                    }

                    return 委托DataTable1;
                }
            }

            public 已发委托Row Get已发委托(DateTime 日期, string 组合号, string 委托编号)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.已发委托Row 已发委托Row1 = this.FirstOrDefault(r => r.日期 == DateTime.Today && r.组合号 == 组合号 && r.委托编号 == 委托编号);
                    if (已发委托Row1 == null)
                    {
                        return null;
                    }

                    已发委托DataTable 已发委托DataTable1 = new 已发委托DataTable();
                    已发委托DataTable1.ImportRow(已发委托Row1);
                    return 已发委托DataTable1[0];
                }
            }

            public 已发委托DataTable Get已发委托(DateTime 日期, string 组合号)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    已发委托DataTable 已发委托DataTable1 = null;
                    var rows = this.Where(r => r.日期 == DateTime.Today && r.组合号 == 组合号);
                    if (rows != null && rows.Count() > 0)
                    {
                        已发委托DataTable1 = new 已发委托DataTable();
                        foreach (var item in rows)
                        {
                            已发委托DataTable1.ImportRow(item);
                        }
                    }
                    return 已发委托DataTable1;
                }

            }

            public 已发委托DataTable GetNotFinished已发委托(DateTime 日期, string 组合号, string userName,int bsFlag)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    已发委托DataTable 已发委托DataTable1 = null;
                    var rows = this.Where(r => r.日期 == DateTime.Today && r.组合号 == 组合号&& r.交易员 == userName && r.买卖方向 == bsFlag  && r.委托数量> r.成交数量 + r.撤单数量);
                    if (rows != null && rows.Count() > 0)
                    {
                        已发委托DataTable1 = new 已发委托DataTable();
                        foreach (var item in rows)
                        {
                            已发委托DataTable1.ImportRow(item);
                        }
                    }
                    return 已发委托DataTable1;
                }
            }

            public void Add(DateTime 日期, string 组合号, string 委托编号, string 交易员, string 状态说明, byte 市场代码, string 证券代码, string 证券名称, int 买卖方向, decimal 成交价格, decimal 成交数量, decimal 委托价格, decimal 委托数量, decimal 撤单数量)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.已发委托Row 已发委托Row1 = this.New已发委托Row();
                    已发委托Row1.日期 = 日期;////如果隔夜委托，这个日期就不能设为今天，待解决
                    已发委托Row1.组合号 = 组合号;
                    已发委托Row1.委托编号 = 委托编号;
                    已发委托Row1.交易员 = 交易员;
                    已发委托Row1.状态说明 = 状态说明;
                    已发委托Row1.市场代码 = 市场代码;
                    已发委托Row1.证券代码 = 证券代码;
                    已发委托Row1.证券名称 = 证券名称;
                    已发委托Row1.买卖方向 = 买卖方向;
                    已发委托Row1.成交价格 = 成交价格;
                    已发委托Row1.成交数量 = 成交数量;
                    已发委托Row1.委托价格 = 委托价格;
                    已发委托Row1.委托数量 = 委托数量;
                    已发委托Row1.撤单数量 = 撤单数量;
                    this.Add已发委托Row(已发委托Row1);
                }
            }

            public void Update(DateTime 日期, string 组合号, string 委托编号, string 状态说明)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.已发委托Row 已发委托Row1 = this.First(r => r.日期 == DateTime.Today && r.组合号 == 组合号 && r.委托编号 == 委托编号);
                    if (委托编号.Length == 36)
                        已发委托Row1.状态说明 = 状态说明;//cats接口专用废单说明。
                    else
                        已发委托Row1.状态说明 = "废单";
                }
            }

            public void Update(DateTime 日期, string 组合号, string 委托编号, decimal 成交价格, decimal 成交数量)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.已发委托Row 已发委托Row1 = this.First(r => r.日期 == DateTime.Today && r.组合号 == 组合号 && r.委托编号 == 委托编号);

                    已发委托Row1.成交价格 = 成交价格;
                    已发委托Row1.成交数量 = 成交数量;
                }
            }

            public void Update(DateTime 日期, string 组合号, string 委托编号, decimal 撤单数量)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.已发委托Row 已发委托Row1 = this.First(r => r.日期 == DateTime.Today && r.组合号 == 组合号 && r.委托编号 == 委托编号);

                    已发委托Row1.撤单数量 = 撤单数量;
                }
            }




            public decimal Get交易费用(string JyUserName, decimal 手续费率)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    decimal 交易费用 = 0;
                    foreach (已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.交易员 == JyUserName))
                    {
                        交易费用 += 已发委托Row1.Get交易费用(手续费率);
                    }
                    return 交易费用;
                }
            }

            public decimal Get交易费用(DbDataSet.平台用户Row user)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    var limits = Program.db.额度分配.QueryTradeLimitBelongJy(user.用户名);
                    decimal 交易费用 = 0;
                    foreach (已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.交易员 == user.用户名))
                    {
                        var limit = limits.FirstOrDefault(_ => _.交易员 == user.用户名 && _.组合号 == 已发委托Row1.组合号 && _.证券代码 == 已发委托Row1.证券代码);
                        交易费用 += 已发委托Row1.Get交易费用(limit == null ? user.手续费率 : limit.手续费率);
                    }
                    return 交易费用;
                }
            }

            public void Get已买卖股数(string JyUserName, string 证券代码, out decimal 已买股数, out decimal 已卖股数)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    已买股数 = 0;
                    已卖股数 = 0;

                    foreach (Server.DbDataSet.已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.交易员 == JyUserName && r.证券代码 == 证券代码))//已发委托 已发委托1 in db.已发委托.Where(r => r.交易员 == UserName && r.日期 == DateTime.Today  && r.证券代码 == 证券代码))
                    {
                        //修正 成交数量+撤单数量 != 委托数量的问题。
                        //经观察，成交数量>0，且符合上述条件的，应按成交数量来走，撤单数量=委托数量-成交数量，委托中撤单数量有问题。
                        if (已发委托Row1.买卖方向 % 2 == 0)
                        {
                            //注释是因为，使用委托数量 -撤单数量 ，会在未成交时将已买或已卖数量计算进去
                            if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 <= 已发委托Row1.委托数量 && (已发委托Row1.委托数量 != 已发委托Row1.撤单数量 + 已发委托Row1.成交数量))
                            {
                                已买股数 += 已发委托Row1.成交数量;
                            }
                            else
                            {
                                已买股数 += 已发委托Row1.委托数量 - 已发委托Row1.撤单数量;
                            }
                            //已买股数 += 已发委托Row1.成交数量;
                        }
                        else
                        {
                            if (已发委托Row1.成交数量 > 0
                                && (已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                                && (已发委托Row1.委托数量 != 已发委托Row1.撤单数量 + 已发委托Row1.成交数量))
                            {
                                已卖股数 += 已发委托Row1.成交数量;
                            }
                            else
                            {
                                已卖股数 += 已发委托Row1.委托数量 - 已发委托Row1.撤单数量;
                            }
                            //已卖股数 += 已发委托Row1.成交数量;
                        }
                    }
                }
            }

            public void Get已买卖股票(string JyUserName, string 证券代码, List<string> 已买卖委托, out decimal 已买股数, out decimal 已卖股数)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    已买股数 = 0;
                    已卖股数 = 0;
                    //已发委托 已发委托1 in db.已发委托.Where(r => r.交易员 == UserName && r.日期 == DateTime.Today  && r.证券代码 == 证券代码))
                    var wtList = this.Where(r => r.日期 == DateTime.Today && r.交易员 == JyUserName && r.证券代码 == 证券代码);
                    foreach (var 已发委托Row1 in wtList)
                    {
                        //修正 成交数量+撤单数量 != 委托数量的问题。
                        //经观察，成交数量>0，且符合上述条件的，应按成交数量来走，撤单数量=委托数量-成交数量，委托中撤单数量有问题。
                        if (已发委托Row1.买卖方向 % 2 == 0)
                        {
                            //注释是因为，使用委托数量 -撤单数量 ，会在未成交时将已买或已卖数量计算进去
                            if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                            {
                                已买股数 += 已发委托Row1.成交数量;
                            }
                            else
                            {
                                已买股数 += 已发委托Row1.委托数量 - 已发委托Row1.撤单数量;
                            }
                        }
                        else
                        {
                            if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                            {
                                已卖股数 += 已发委托Row1.成交数量;
                            }
                            else
                            {
                                已卖股数 += 已发委托Row1.委托数量 - 已发委托Row1.撤单数量;
                            }
                        }
                        已买卖委托.Add(已发委托Row1.委托编号);
                    }
                }
            }

            public void GetBuySaleNum(string JyUserName, string 证券代码, List<string> lstOrderID, out Dictionary<string, decimal> buyDict, out Dictionary<string, decimal> saleDict)
            {
                buyDict = new Dictionary<string, decimal>();
                saleDict = new Dictionary<string, decimal>();
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    var wtList = this.Where(r => r.日期 == DateTime.Today && r.交易员 == JyUserName && r.证券代码 == 证券代码);

                    foreach (var 已发委托Row1 in wtList)
                    {
                        if (!buyDict.ContainsKey(已发委托Row1.组合号))
                        {
                            buyDict.Add(已发委托Row1.组合号, 0);
                        }
                        if (!saleDict.ContainsKey(已发委托Row1.组合号))
                        {
                            saleDict.Add(已发委托Row1.组合号, 0);
                        }

                        if (已发委托Row1.买卖方向 % 2 == 0)
                        {
                            if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                            {
                                buyDict[已发委托Row1.组合号] += 已发委托Row1.成交数量;
                            }
                            else
                            {
                                buyDict[已发委托Row1.组合号] += 已发委托Row1.委托数量 - 已发委托Row1.撤单数量;
                            }
                        }
                        else
                        {
                            if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                            {
                                saleDict[已发委托Row1.组合号] += 已发委托Row1.成交数量;
                            }
                            else
                            {
                                saleDict[已发委托Row1.组合号] += 已发委托Row1.委托数量 - 已发委托Row1.撤单数量;
                            }
                        }
                        lstOrderID.Add(已发委托Row1.委托编号);
                    }
                }
            }

            //已用资金数
            public decimal Get已用仓位(string JyUserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Dictionary<string, decimal> 证券仓位 = new Dictionary<string, decimal>();

                    foreach (Server.DbDataSet.已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.交易员 == JyUserName))
                    {
                        if (!证券仓位.ContainsKey(已发委托Row1.证券代码))
                        {
                            证券仓位[已发委托Row1.证券代码] = 0;
                        }
                        //修正 成交数量+撤单数量 != 委托数量的问题。
                        //经观察，成交数量>0，且符合上述条件的，应按成交数量来走，撤单数量=委托数量-成交数量，委托中撤单数量有问题。
                        if (已发委托Row1.买卖方向 == 0)
                        {
                            if (已发委托Row1.成交数量 > 0
                                && (已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                                && (已发委托Row1.委托数量 != 已发委托Row1.撤单数量 + 已发委托Row1.成交数量))
                            {
                                证券仓位[已发委托Row1.证券代码] += 已发委托Row1.委托价格 * 已发委托Row1.成交数量;
                            }
                            else
                            {
                                证券仓位[已发委托Row1.证券代码] += 已发委托Row1.委托价格 * (已发委托Row1.委托数量 - 已发委托Row1.撤单数量);
                            }
                        }
                        else
                        {
                            if (已发委托Row1.成交数量 > 0
                                && (已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                                && (已发委托Row1.委托数量 != 已发委托Row1.撤单数量 + 已发委托Row1.成交数量))
                            {
                                证券仓位[已发委托Row1.证券代码] -= 已发委托Row1.委托价格 * 已发委托Row1.成交数量;
                            }
                            else
                            {
                                证券仓位[已发委托Row1.证券代码] -= 已发委托Row1.委托价格 * (已发委托Row1.委托数量 - 已发委托Row1.撤单数量);
                            }
                        }
                    }


                    decimal 已用仓位 = 0;
                    foreach (string ZqdmKey in 证券仓位.Keys)
                    {
                        已用仓位 += Math.Abs(证券仓位[ZqdmKey]);
                    }
                    return 已用仓位;
                }
            }
        }

        partial class 已发委托Row
        {
            public decimal Get交易费用(decimal 手续费率)
            {

                if (this.成交数量 > 0)
                {
                    decimal 成交金额 = this.成交价格 * this.成交数量;
                    decimal 佣金 = Math.Max(5, Math.Round(成交金额 * 手续费率, 2, MidpointRounding.AwayFromZero));
                    decimal 印花税 = this.买卖方向 == 0 ? 0 : Math.Round(成交金额 * 0.001m, 2, MidpointRounding.AwayFromZero);
                    decimal 过户费 = this.市场代码 == 1 ? Math.Round(this.成交数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;
                    return 佣金 + 印花税 + 过户费;
                }
                else
                {
                    return 0;
                }
            }
        }


        partial class 额度分配DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["交易员"], this.Columns["证券代码"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (额度分配 额度分配1 in db.额度分配)
                        {
                            额度分配Row 额度分配Row1 = this.New额度分配Row();
                            额度分配Row1.交易员 = 额度分配1.交易员;
                            额度分配Row1.组合号 = 额度分配1.组合号;
                            额度分配Row1.证券代码 = 额度分配1.证券代码;
                            额度分配Row1.市场 = 额度分配1.市场;
                            额度分配Row1.证券名称 = 额度分配1.证券名称;
                            额度分配Row1.拼音缩写 = 额度分配1.拼音缩写;
                            额度分配Row1.买模式 = (int)额度分配1.买模式;
                            额度分配Row1.卖模式 = (int)额度分配1.卖模式;
                            额度分配Row1.交易额度 = 额度分配1.交易额度;
                            额度分配Row1.手续费率 = (decimal)额度分配1.手续费率;
                            this.Add额度分配Row(额度分配Row1);
                        }
                    }


                    this.额度分配RowChanging += 额度分配_额度分配RowChanging;
                    this.额度分配RowDeleting += 额度分配_额度分配RowChanging;


                }
            }

            public void LoadToday()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["交易员"], this.Columns["证券代码"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (额度分配 额度分配1 in db.额度分配)
                        {
                            额度分配Row 额度分配Row1 = this.New额度分配Row();
                            额度分配Row1.交易员 = 额度分配1.交易员;
                            额度分配Row1.组合号 = 额度分配1.组合号;
                            额度分配Row1.证券代码 = 额度分配1.证券代码;
                            额度分配Row1.市场 = 额度分配1.市场;
                            额度分配Row1.证券名称 = 额度分配1.证券名称;
                            额度分配Row1.拼音缩写 = 额度分配1.拼音缩写;
                            额度分配Row1.买模式 = (int)额度分配1.买模式;
                            额度分配Row1.卖模式 = (int)额度分配1.卖模式;
                            额度分配Row1.交易额度 = 额度分配1.交易额度;
                            额度分配Row1.手续费率 = (decimal)额度分配1.手续费率;
                            this.Add额度分配Row(额度分配Row1);
                        }
                    }
                }
            }
            void 额度分配_额度分配RowChanging(object sender, DbDataSet.额度分配RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            额度分配 额度分配1 = new 额度分配();
                            额度分配1.交易员 = e.Row.交易员;
                            额度分配1.组合号 = e.Row.组合号;
                            额度分配1.证券代码 = e.Row.证券代码;
                            额度分配1.市场 = e.Row.市场;
                            额度分配1.证券名称 = e.Row.证券名称;
                            额度分配1.拼音缩写 = e.Row.拼音缩写;
                            额度分配1.买模式 = (买模式)e.Row.买模式;
                            额度分配1.卖模式 = (卖模式)e.Row.卖模式;
                            额度分配1.交易额度 = e.Row.交易额度;
                            额度分配1.手续费率 = e.Row.手续费率;
                            db.额度分配.Add(额度分配1);
                            db.SaveChanges();

                            Program.额度分配表Changed[e.Row.交易员] = true;
                            break;
                        case DataRowAction.Delete:
                            额度分配1 = db.额度分配.Find(e.Row.交易员, e.Row.证券代码);
                            db.额度分配.Remove(额度分配1);
                            db.SaveChanges();

                            Program.额度分配表Changed[e.Row.交易员] = true;
                            break;
                        case DataRowAction.Change:
                            额度分配1 = db.额度分配.Find(e.Row.交易员, e.Row.证券代码);
                            额度分配1.交易员 = e.Row.交易员;
                            额度分配1.组合号 = e.Row.组合号;
                            额度分配1.证券代码 = e.Row.证券代码;
                            额度分配1.市场 = e.Row.市场;
                            额度分配1.证券名称 = e.Row.证券名称;
                            额度分配1.拼音缩写 = e.Row.拼音缩写;
                            额度分配1.买模式 = (买模式)e.Row.买模式;
                            额度分配1.卖模式 = (卖模式)e.Row.卖模式;
                            额度分配1.交易额度 = e.Row.交易额度;
                            额度分配1.手续费率 = e.Row.手续费率;
                            db.Entry(额度分配1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            Program.额度分配表Changed[e.Row.交易员] = true;
                            break;
                        default:
                            break;

                    }
                }
            }






            public 额度分配Row Get额度分配(string 交易员, string 证券代码)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();

                    额度分配Row 额度分配Row1 = this.FirstOrDefault(r => r.交易员 == 交易员 && r.证券代码 == 证券代码);
                    if (额度分配Row1 == null)
                    {
                        return null;
                    }

                    额度分配DataTable1.ImportRow(额度分配Row1);


                    return 额度分配DataTable1[0];
                }
            }

            public IEnumerable<额度分配Row> GetAllLimi(string 交易员, string 证券代码)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Where(r => r.交易员 == 交易员 && r.证券代码 == 证券代码);
                }
            }

            public Server.DbDataSet.额度分配DataTable QueryTradeLimit()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
                    Server.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();

                    if (平台用户Row.分组 == (int)分组.ALL)
                    {
                        foreach (Server.DbDataSet.额度分配Row 额度分配Row1 in this)
                        {
                            额度分配DataTable1.ImportRow(额度分配Row1);

                        }
                    }
                    else
                    {
                        Server.DbDataSet.平台用户DataTable 平台用户DataTable1 = new 平台用户DataTable();
                        平台用户DataTable1.LoadToday();
                        foreach (Server.DbDataSet.额度分配Row 额度分配Row1 in this)
                        {
                            foreach (Server.DbDataSet.平台用户Row 交易员Row in 平台用户DataTable1.Where(r => r.用户名 == 额度分配Row1.交易员))
                            {
                                if (交易员Row != null && 交易员Row.分组 == 平台用户Row.分组)
                                {
                                    额度分配DataTable1.ImportRow(额度分配Row1);
                                }
                            }

                        }
                    }

                    return 额度分配DataTable1;
                }

            }

            public Server.DbDataSet.额度分配DataTable QueryTradeLimitBelongJy(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();

                    foreach (Server.DbDataSet.额度分配Row 额度分配Row1 in this.Where(r => r.交易员 == UserName))
                    {
                        额度分配DataTable1.ImportRow(额度分配Row1);

                    }

                    return 额度分配DataTable1;
                }

            }

            #region Limit Manager Functions
            public Server.DbDataSet.额度分配DataTable LimitManagerServiceQeury(string guid)
            {
                if (CommonUtils.LimitServiceID == guid)
                {
                    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                    {
                        Server.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();

                        foreach (Server.DbDataSet.额度分配Row 额度分配Row1 in this)
                        {
                            额度分配DataTable1.ImportRow(额度分配Row1);
                        }
                        return 额度分配DataTable1;
                    }
                }
                return null;
            }

            public int LimitManagerserviceImport(string guid, DbDataSet.额度分配DataTable dt, out string msg)
            {
                int excetptionCount = 0;
                int updateCount = 0;
                int addCount = 0;

                if (CommonUtils.LimitServiceID == guid)
                {
                    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                    {
                        foreach (var limitRow in dt)
                        {
                            try
                            {
                                额度分配Row row = this.FirstOrDefault(_ => _.交易员 == limitRow.交易员 && _.证券代码 == limitRow.证券代码);
                                if (Program.db.平台用户.ExistsUserRole(limitRow.交易员, 角色.交易员))
                                {
                                    if (row == null)
                                    {
                                        row = this.New额度分配Row();
                                        row.交易员 = limitRow.交易员;
                                        row.证券代码 = limitRow.证券代码;
                                        this.Add额度分配Row(row);
                                        addCount++;
                                    }
                                    else
                                    {
                                        updateCount++;
                                    }
                                    row.组合号 = limitRow.组合号;
                                    row.市场 = limitRow.市场;
                                    row.证券名称 = limitRow.证券名称;
                                    row.拼音缩写 = limitRow.拼音缩写;
                                    row.买模式 = limitRow.买模式;
                                    row.卖模式 = limitRow.卖模式;
                                    row.交易额度 = limitRow.交易额度;
                                    row.手续费率 = limitRow.手续费率;
                                }
                            }
                            catch (Exception ex)
                            {
                                excetptionCount++;
                                Program.logger.LogInfoDetail("额度分配DataTable.LimitManagerserviceImport Exception {0}", ex.Message);
                            }
                        }
                    }
                    msg = string.Format("新增额度{0}条, 修改数据{1}条, 异常数据{2}条", addCount, updateCount, excetptionCount);
                    if (excetptionCount > 0)
                    {
                        msg += "，异常数据多于0条，请联系管理员！";
                    }
                    return updateCount + addCount;
                }
                else
                {
                    msg = "LimitManagerservice 访问接口验证异常，请联系管理员！";
                    return 0;
                }
            }
            #endregion

            public void AddTradeLimit(string 交易员, string 证券代码, string 组合号, byte 市场代码, string 证券名称, string 拼音缩写, 买模式 买模式1, 卖模式 卖模式1, decimal 交易额度, decimal 手续费率)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.额度分配Row TradeLimit1 = this.New额度分配Row();
                    TradeLimit1.交易员 = 交易员;
                    TradeLimit1.组合号 = 组合号;
                    TradeLimit1.证券代码 = 证券代码;
                    TradeLimit1.市场 = 市场代码;
                    TradeLimit1.证券名称 = 证券名称;
                    TradeLimit1.拼音缩写 = 拼音缩写;
                    TradeLimit1.买模式 = (int)买模式1;
                    TradeLimit1.卖模式 = (int)卖模式1;
                    TradeLimit1.交易额度 = 交易额度;
                    TradeLimit1.手续费率 = 手续费率;
                    this.Add额度分配Row(TradeLimit1);
                }



            }

            public void UpdateTradeLimit(string 交易员, string 证券代码, string 组合号, byte 市场代码, string 证券名称, string 拼音缩写, 买模式 买模式1, 卖模式 卖模式1, decimal 交易额度, decimal 手续费率)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.额度分配Row TradeLimit1 = this.First(r => r.交易员 == 交易员 && r.证券代码 == 证券代码);

                    TradeLimit1.组合号 = 组合号;
                    TradeLimit1.市场 = 市场代码;
                    TradeLimit1.证券名称 = 证券名称;
                    TradeLimit1.拼音缩写 = 拼音缩写;
                    TradeLimit1.买模式 = (int)买模式1;
                    TradeLimit1.卖模式 = (int)卖模式1;
                    TradeLimit1.交易额度 = 交易额度;
                    TradeLimit1.手续费率 = 手续费率;
                }




            }

            public void DeleteTradeLimit(string 交易员, string 证券代码)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.额度分配Row TradeLimit1 = this.First(r => r.交易员 == 交易员 && r.证券代码 == 证券代码);


                    this.Remove额度分配Row(TradeLimit1);
                }



            }

            public void DeleteAllTradeLimitByUser(string 交易员)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    for (int i = this.Rows.Count - 1; i >= 0; i--)
                    {
                        Server.DbDataSet.额度分配Row TradeLimit1 = this.Rows[i] as Server.DbDataSet.额度分配Row;
                        if (TradeLimit1.交易员.Equals(交易员))
                        {
                            this.Remove额度分配Row(TradeLimit1);
                        }

                    }
                }
            }

        }

        partial class 额度分配Row
        {
            public int Get券商帐户买卖类别(int 买卖方向)
            {
                if (买卖方向 == 0)
                {
                    if (this.买模式 == (int)Server.买模式.现金买入)
                    {
                        return 0;
                    }
                    else if (this.买模式 == (int)Server.买模式.融资买入)
                    {
                        return 2;
                    }
                    else if (this.买模式 == (int)Server.买模式.买券还券)
                    {
                        return 4;
                    }
                    else if (this.买模式 == (int)Server.买模式.担保品买入)
                    {
                        return 7;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    if (this.卖模式 == (int)Server.卖模式.现券卖出)
                    {
                        return 1;
                    }
                    else if (this.卖模式 == (int)Server.卖模式.融券卖出)
                    {
                        return 3;
                    }
                    else if (this.卖模式 == (int)Server.卖模式.卖券还款)
                    {
                        return 5;
                    }
                    else if (this.卖模式 == (int)Server.卖模式.担保品卖出)
                    {
                        return 8;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

        }


        partial class 风控分配DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["交易员"], this.Columns["风控员"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (风控分配 风控分配1 in db.风控分配)
                        {
                            风控分配Row 风控分配Row1 = this.New风控分配Row();
                            风控分配Row1.交易员 = 风控分配1.交易员;
                            风控分配Row1.风控员 = 风控分配1.风控员;
                            this.Add风控分配Row(风控分配Row1);
                        }
                    }

                    this.风控分配RowChanging += 风控分配_风控分配RowChanging;
                    this.风控分配RowDeleting += 风控分配_风控分配RowChanging;
                }
            }


            void 风控分配_风控分配RowChanging(object sender, DbDataSet.风控分配RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            风控分配 风控分配1 = new 风控分配();
                            风控分配1.交易员 = e.Row.交易员;
                            风控分配1.风控员 = e.Row.风控员;
                            db.风控分配.Add(风控分配1);
                            db.SaveChanges();

                            Program.风控分配表Changed[e.Row.风控员] = true;

                            break;
                        case DataRowAction.Delete:
                            风控分配1 = db.风控分配.Find(e.Row.交易员, e.Row.风控员);
                            db.风控分配.Remove(风控分配1);
                            db.SaveChanges();

                            Program.风控分配表Changed[e.Row.风控员] = true;

                            break;
                        case DataRowAction.Change:
                            风控分配1 = db.风控分配.Find(e.Row.交易员, e.Row.风控员);
                            风控分配1.交易员 = e.Row.交易员;
                            风控分配1.风控员 = e.Row.风控员;
                            db.Entry(风控分配1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            Program.风控分配表Changed[e.Row.风控员] = true;

                            break;
                        default:
                            break;

                    }
                }
            }


            public Server.DbDataSet.风控分配DataTable Query风控分配BelongFK(string FKUserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.风控分配DataTable 风控分配DataTable1 = new Server.DbDataSet.风控分配DataTable();
                    foreach (Server.DbDataSet.风控分配Row 风控分配Row1 in this)
                    {
                        if (风控分配Row1.风控员 == FKUserName)
                        {
                            风控分配DataTable1.ImportRow(风控分配Row1);
                        }
                    }

                    return 风控分配DataTable1;
                }
            }

            public void Add(string JyUserName, string FkUserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.风控分配Row 风控分配Row1 = this.New风控分配Row();
                    风控分配Row1.风控员 = FkUserName;
                    风控分配Row1.交易员 = JyUserName;
                    this.Add风控分配Row(风控分配Row1);
                }
            }

            public void Delete(string JyUserName, string FkUserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.风控分配Row 风控分配Row1 = this.First(r => r.交易员 == JyUserName && r.风控员 == FkUserName);
                    this.Remove风控分配Row(风控分配Row1);
                }
            }

            public bool Exists(string JyUserName, string FkUserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(r => r.交易员 == JyUserName && r.风控员 == FkUserName);
                }
            }
        }

        partial class 平台用户DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();
            public ConcurrentDictionary<string, object> SendOrderSyncDict = new ConcurrentDictionary<string, object>();
            public static object Sync = new object();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["用户名"] };



                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (平台用户 平台用户1 in db.平台用户)
                        {
                            平台用户Row 平台用户Row1 = this.New平台用户Row();
                            平台用户Row1.用户名 = 平台用户1.用户名;
                            平台用户Row1.密码 = Cryptor.MD5Decrypt(平台用户1.密码);
                            平台用户Row1.角色 = (int)平台用户1.角色;
                            平台用户Row1.仓位限制 = 平台用户1.仓位限制;
                            平台用户Row1.亏损限制 = 平台用户1.亏损限制;
                            平台用户Row1.手续费率 = 平台用户1.手续费率;
                            平台用户Row1.允许删除碎股订单 = 平台用户1.允许删除碎股订单;
                            平台用户Row1.分组 = (int)平台用户1.分组;
                            this.Add平台用户Row(平台用户Row1);
                        }
                    }


                    this.平台用户RowChanging += 平台用户_平台用户RowChanging;
                    this.平台用户RowDeleting += 平台用户_平台用户RowChanging;
                }
            }
            public void LoadToday()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["用户名"] };



                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (平台用户 平台用户1 in db.平台用户)
                        {
                            平台用户Row 平台用户Row1 = this.New平台用户Row();
                            平台用户Row1.用户名 = 平台用户1.用户名;
                            平台用户Row1.密码 = Cryptor.MD5Decrypt(平台用户1.密码);
                            平台用户Row1.角色 = (int)平台用户1.角色;
                            平台用户Row1.仓位限制 = 平台用户1.仓位限制;
                            平台用户Row1.亏损限制 = 平台用户1.亏损限制;
                            平台用户Row1.手续费率 = 平台用户1.手续费率;
                            平台用户Row1.允许删除碎股订单 = 平台用户1.允许删除碎股订单;
                            平台用户Row1.分组 = (int)平台用户1.分组;
                            this.Add平台用户Row(平台用户Row1);
                        }
                    }
                }
            }
            void 平台用户_平台用户RowChanging(object sender, DbDataSet.平台用户RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            平台用户 平台用户1 = new 平台用户();
                            平台用户1.用户名 = e.Row.用户名;
                            平台用户1.密码 = Cryptor.MD5Encrypt(e.Row.密码);
                            平台用户1.角色 = (角色)e.Row.角色;
                            平台用户1.仓位限制 = e.Row.仓位限制;
                            平台用户1.亏损限制 = e.Row.亏损限制;
                            平台用户1.手续费率 = e.Row.手续费率;
                            平台用户1.允许删除碎股订单 = e.Row.允许删除碎股订单;
                            平台用户1.分组 = (分组)e.Row.分组;
                            db.平台用户.Add(平台用户1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            平台用户1 = db.平台用户.Find(e.Row.用户名);
                            db.平台用户.Remove(平台用户1);
                            db.SaveChanges();




                            break;
                        case DataRowAction.Change:
                            平台用户1 = db.平台用户.Find(e.Row.用户名);
                            平台用户1.用户名 = e.Row.用户名;
                            平台用户1.密码 = Cryptor.MD5Encrypt(e.Row.密码);
                            平台用户1.角色 = (角色)e.Row.角色;
                            平台用户1.仓位限制 = e.Row.仓位限制;
                            平台用户1.亏损限制 = e.Row.亏损限制;
                            平台用户1.手续费率 = e.Row.手续费率;
                            平台用户1.允许删除碎股订单 = e.Row.允许删除碎股订单;
                            平台用户1.分组 = (分组)e.Row.分组;
                            db.Entry(平台用户1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();


                            Program.平台用户表Changed[e.Row.用户名] = true;

                            break;
                        default:
                            break;
                    }
                }
            }

            public Server.DbDataSet.平台用户DataTable QueryUserInRoles(int[] 角色Array, int 分组1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                    foreach (Server.DbDataSet.平台用户Row 平台用户Row1 in this)
                    {
                        if (角色Array.Contains(平台用户Row1.角色) && (分组1 == (int)分组.ALL || 平台用户Row1.分组 == 分组1))
                        {
                            平台用户DataTable1.ImportRow(平台用户Row1);
                        }

                    }
                    foreach (Server.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
                    {
                        平台用户Row1.密码 = string.Empty;
                    }
                    return 平台用户DataTable1;
                }
            }

            public Server.DbDataSet.平台用户Row Get平台用户(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.平台用户Row AASUser1 = this.First(r => r.用户名 == UserName);
                    Server.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                    平台用户DataTable1.ImportRow(AASUser1);
                    foreach (Server.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
                    {
                        平台用户Row1.密码 = string.Empty;
                    }
                    return 平台用户DataTable1[0];
                }
            }
            public Server.DbDataSet.平台用户DataTable QuerySingleUser(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                    foreach (Server.DbDataSet.平台用户Row 平台用户Row1 in this.Where(r => r.用户名 == UserName))
                    {
                        平台用户DataTable1.ImportRow(平台用户Row1);
                    }
                    foreach (Server.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
                    {
                        平台用户Row1.密码 = string.Empty;
                    }
                    return 平台用户DataTable1;
                }
            }

            public object GetSendOrderSyncObj(string userName)
            {
                lock (Sync)
                {
                    if (SendOrderSyncDict.Count == 0)
                    {
                        foreach (var item in this)
                        {
                            SendOrderSyncDict[item.用户名] = new object();
                        }
                    }
                    if (!SendOrderSyncDict.ContainsKey(userName))
                    {
                        SendOrderSyncDict[userName] = new object();
                    }
                }
                return SendOrderSyncDict[userName];
            }

            public bool ExistsUser(string UserName, string Password)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(r => r.用户名 == UserName && r.密码 == Password);
                }
            }


            public bool ExistsUserRole(string UserName, 角色 角色1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(r => r.用户名 == UserName && r.角色 == (int)角色1);
                }
            }


            public bool ExistsRole(角色 角色1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(r => r.角色 == (int)角色1);
                }
            }

            public void ResetPassword(string UserName, string Password)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.平台用户Row AASUser1 = this.First(r => r.用户名 == UserName);
                    AASUser1.密码 = Password;
                }
            }


            public void AddUser(string UserName, string Password, 角色 Role, decimal 仓位限制, decimal 亏损限制, decimal 手续费率, bool 允许删除碎股订单, 分组 Region)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.平台用户Row AASUser2 = this.New平台用户Row();
                    AASUser2.用户名 = UserName;
                    AASUser2.密码 = Password;
                    AASUser2.角色 = (int)Role;
                    AASUser2.仓位限制 = 仓位限制;
                    AASUser2.亏损限制 = 亏损限制;
                    AASUser2.手续费率 = 手续费率;
                    AASUser2.允许删除碎股订单 = 允许删除碎股订单;
                    AASUser2.分组 = (int)Region;
                    this.Add平台用户Row(AASUser2);
                }

            }


            public void UpdateUser(string UserName, decimal 仓位限制, decimal 亏损限制, decimal 手续费率, bool 允许删除碎股订单, 分组 分组1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.平台用户Row AASUser1 = this.First(r => r.用户名 == UserName);
                    AASUser1.仓位限制 = 仓位限制;
                    AASUser1.亏损限制 = 亏损限制;
                    AASUser1.手续费率 = 手续费率;
                    AASUser1.允许删除碎股订单 = 允许删除碎股订单;
                    AASUser1.分组 = (int)分组1;
                }
            }

            public void DeleteUser(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.平台用户Row AASUser1 = this.First(r => r.用户名 == UserName);
                    this.Remove平台用户Row(AASUser1);
                }
            }
        }

        partial class MAC地址分配DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["用户名"], this.Columns["MAC"] };



                    using (AASDbContext db = new AASDbContext())
                    {

                        foreach (MAC地址分配 MAC地址分配1 in db.MAC地址分配)
                        {
                            MAC地址分配Row MAC地址分配Row1 = this.NewMAC地址分配Row();
                            MAC地址分配Row1.用户名 = MAC地址分配1.用户名;
                            MAC地址分配Row1.MAC = MAC地址分配1.MAC;
                            this.AddMAC地址分配Row(MAC地址分配Row1);
                        }

                    }


                    this.MAC地址分配RowChanging += MAC地址分配_MAC地址分配RowChanging;
                    this.MAC地址分配RowDeleting += MAC地址分配_MAC地址分配RowChanging;
                }
            }

            void MAC地址分配_MAC地址分配RowChanging(object sender, DbDataSet.MAC地址分配RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            MAC地址分配 MAC地址分配1 = new MAC地址分配();
                            MAC地址分配1.用户名 = e.Row.用户名;
                            MAC地址分配1.MAC = e.Row.MAC;
                            db.MAC地址分配.Add(MAC地址分配1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            MAC地址分配1 = db.MAC地址分配.Find(e.Row.用户名, e.Row.MAC);
                            db.MAC地址分配.Remove(MAC地址分配1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Change:
                            MAC地址分配1 = db.MAC地址分配.Find(e.Row.用户名, e.Row.MAC);
                            MAC地址分配1.用户名 = e.Row.用户名;
                            MAC地址分配1.MAC = e.Row.MAC;
                            db.Entry(MAC地址分配1).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }
                }
            }

            public Server.DbDataSet.MAC地址分配DataTable QueryMACBelongUser(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.MAC地址分配DataTable MAC地址分配DataTable1 = new DbDataSet.MAC地址分配DataTable();
                    foreach (Server.DbDataSet.MAC地址分配Row MAC地址分配Row1 in this)
                    {
                        if (MAC地址分配Row1.用户名 == UserName)
                        {
                            MAC地址分配DataTable1.ImportRow(MAC地址分配Row1);
                        }

                    }

                    return MAC地址分配DataTable1;
                }
            }

            public void AddMAC(string UserName, string MAC)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.MAC地址分配Row MAC地址分配1 = this.NewMAC地址分配Row();
                    MAC地址分配1.用户名 = UserName;
                    MAC地址分配1.MAC = MAC;
                    this.AddMAC地址分配Row(MAC地址分配1);
                }
            }

            public void DeleteMAC(string UserName, string MAC)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.MAC地址分配Row MAC地址分配1 = this.First(r => r.用户名 == UserName && r.MAC == MAC);

                    this.RemoveMAC地址分配Row(MAC地址分配1);
                }


            }

            public bool IsMacValid(string UserName, string MAC)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    if (!this.Any(r => r.用户名 == UserName))
                    {
                        return true;
                    }
                    else
                    {
                        if (this.Any(r => r.用户名 == UserName && r.MAC == MAC))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

        }

        partial class 交易日志DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    using (AASDbContext db = new AASDbContext())
                    {
                        var yesterday = DateTime.Today.AddDays(-1);
                        foreach (交易日志 交易日志1 in db.交易日志.Where(r => r.日期 >= yesterday))
                        {
                            交易日志Row 交易日志Row1 = this.New交易日志Row();
                            交易日志Row1.日期 = 交易日志1.日期;
                            交易日志Row1.时间 = 交易日志1.时间;
                            交易日志Row1.交易员 = 交易日志1.交易员;
                            交易日志Row1.组合号 = 交易日志1.组合号;
                            交易日志Row1.证券代码 = 交易日志1.证券代码;
                            交易日志Row1.证券名称 = 交易日志1.证券名称;
                            交易日志Row1.委托编号 = 交易日志1.委托编号;
                            交易日志Row1.买卖方向 = 交易日志1.买卖方向;
                            交易日志Row1.委托数量 = 交易日志1.委托数量;
                            交易日志Row1.委托价格 = 交易日志1.委托价格;
                            交易日志Row1.信息 = 交易日志1.信息;
                            this.Add交易日志Row(交易日志Row1);
                        }

                    }


                    this.交易日志RowChanging += 交易日志_交易日志RowChanging;
                    this.交易日志RowDeleting += 交易日志_交易日志RowChanging;
                }
            }

            void 交易日志_交易日志RowChanging(object sender, DbDataSet.交易日志RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    if (e.Action == DataRowAction.Add)
                    {
                        交易日志 交易日志1 = new 交易日志();
                        交易日志1.日期 = e.Row.日期;
                        交易日志1.时间 = e.Row.时间;
                        交易日志1.交易员 = e.Row.交易员;
                        交易日志1.组合号 = e.Row.组合号;
                        交易日志1.证券代码 = e.Row.证券代码;
                        交易日志1.证券名称 = e.Row.证券名称;
                        交易日志1.委托编号 = e.Row.委托编号;
                        交易日志1.买卖方向 = e.Row.买卖方向;
                        交易日志1.委托数量 = e.Row.委托数量;
                        交易日志1.委托价格 = e.Row.委托价格;
                        交易日志1.信息 = e.Row.信息;
                        db.交易日志.Add(交易日志1);
                        db.SaveChanges();
                    }
                }
            }

            public void Add(DateTime 日期, string 时间, string 交易员, string 组合号, string 证券代码, string 证券名称, string 委托编号, int 买卖方向, decimal 委托数量, decimal 委托价格, string 信息)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    Server.DbDataSet.交易日志Row 交易日志Row1 = this.New交易日志Row();
                    交易日志Row1.日期 = 日期;
                    交易日志Row1.时间 = 时间;
                    交易日志Row1.交易员 = 交易员;
                    交易日志Row1.组合号 = 组合号;
                    交易日志Row1.证券代码 = 证券代码;
                    交易日志Row1.证券名称 = 证券名称;
                    交易日志Row1.委托编号 = 委托编号;
                    交易日志Row1.买卖方向 = 买卖方向;
                    交易日志Row1.委托数量 = 委托数量;
                    交易日志Row1.委托价格 = 委托价格;
                    交易日志Row1.信息 = 信息;
                    this.Add交易日志Row(交易日志Row1);
                }
            }

            public void Add(JyDataSet.成交Row 成交Row1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    交易日志Row 交易日志Row1 = this.New交易日志Row();
                    交易日志Row1.日期 = DateTime.Today;
                    交易日志Row1.时间 = 成交Row1.成交时间;
                    交易日志Row1.交易员 = 成交Row1.交易员;
                    交易日志Row1.组合号 = 成交Row1.组合号;
                    交易日志Row1.证券代码 = 成交Row1.证券代码;
                    交易日志Row1.证券名称 = 成交Row1.证券名称;
                    交易日志Row1.委托编号 = 成交Row1.委托编号;
                    交易日志Row1.买卖方向 = 成交Row1.买卖方向;
                    交易日志Row1.委托数量 = 成交Row1.成交数量;
                    交易日志Row1.委托价格 = 成交Row1.成交价格;
                    交易日志Row1.信息 = "成交";
                    this.Add交易日志Row(交易日志Row1);
                }
            }

            public Server.DbDataSet.交易日志DataTable QueryTodayJyLog(List<string> JyList)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    Server.DbDataSet.交易日志DataTable 交易日志DataTable1 = new DbDataSet.交易日志DataTable();
                    foreach (Server.DbDataSet.交易日志Row 交易日志Row1 in this.Where(r => JyList.Contains(r.交易员) && r.日期 == DateTime.Today))
                    {
                        交易日志DataTable1.ImportRow(交易日志Row1);
                    }
                    return 交易日志DataTable1;
                }

            }

        }

        partial class 可用资金DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["交易员"], this.Columns["币种"] };

                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (var item in db.可用资金)
                        {
                            var row = this.New可用资金Row();
                            row.交易员 = item.交易员;
                            row.币种 = item.币种;
                            row.可用数量 = item.可用数量;
                            this.Add可用资金Row(row);
                        }
                    }


                    this.可用资金RowChanging += 可用资金_可用资金RowChanging;
                    this.可用资金RowDeleting += 可用资金_可用资金RowChanging;
                }
            }

            void 可用资金_可用资金RowChanging(object sender, DbDataSet.可用资金RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            var row = new 可用资金();
                            row.交易员 = e.Row.交易员;
                            row.币种 = e.Row.币种;
                            row.可用数量 = e.Row.可用数量;
                            db.可用资金.Add(row);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            row = db.可用资金.Find(e.Row.交易员, e.Row.币种);
                            db.可用资金.Remove(row);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Change:
                            row = db.可用资金.Find(e.Row.交易员, e.Row.币种);
                            row.可用数量 = e.Row.可用数量;
                            db.Entry(row).State = System.Data.Entity.EntityState.Modified;
                            //Program.平台用户表Changed[e.Row.交易员] = true;
                            db.SaveChanges();
                            break;
                        default:
                            break;
                    }
                }
            }

            public bool Exists(string UserName, string 币种)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    可用资金Row row = this.FirstOrDefault(r => r.交易员 == UserName && r.币种 == 币种);
                    return row != null;
                }
            }

            public 可用资金DataTable Get可用资金(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    可用资金DataTable dt = new 可用资金DataTable();
                    foreach (var item in this)
                    {
                        if (item.交易员 == UserName) dt.Add可用资金Row(item);
                    }
                    return dt;
                }
            }

            public decimal Get可用资金(string UserName, string 币种)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    可用资金Row row = this.FirstOrDefault(r => r.交易员 == UserName && 币种.Equals(r.币种, StringComparison.CurrentCultureIgnoreCase));
                    return row == null ? 0 : row.可用数量;
                }
            }


            public void Add(string UserName, string 币种, decimal 仓位限制)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    可用资金Row row = this.New可用资金Row();
                    row.交易员 = UserName;
                    row.币种 = 币种.ToUpper();
                    row.可用数量 = 仓位限制;
                    this.Add可用资金Row(row);
                }
            }

            public void Update(string UserName, string 币种, decimal 仓位限制)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    var row = this.First(_ => _.交易员 == UserName && _.币种 == 币种);
                    row.可用数量 = 仓位限制;
                }
            }

            public void Update(string 交易员, string 证券代码, int 买卖方向, decimal 未处理成交数量, decimal 未处理成交价格)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    var kv = CommonUtils.GetBinanceCoinInfo(证券代码);
                    var rowCoinTrade = this.FirstOrDefault(_ => _.交易员 == 交易员 && _.币种 == kv.Key);
                    if (rowCoinTrade == null)
                    {
                        rowCoinTrade = this.New可用资金Row();
                        rowCoinTrade.交易员 = 交易员;
                        rowCoinTrade.币种 = kv.Key;
                        this.Add可用资金Row(rowCoinTrade);
                    }
                    rowCoinTrade.可用数量 += (买卖方向 == 0 ? 未处理成交数量 : (0 - 未处理成交数量));

                    var rowBasic = this.FirstOrDefault(_ => _.交易员 == 交易员 && _.币种 == kv.Value);
                    if (rowBasic == null)
                    {
                        rowBasic = this.New可用资金Row();
                        rowBasic.交易员 = 交易员;
                        rowBasic.币种 = kv.Key;
                        this.Add可用资金Row(rowCoinTrade);
                    }
                    rowBasic.可用数量 += (买卖方向 == 0 ? 未处理成交数量 : (0 - 未处理成交数量)) * 未处理成交价格;
                }
            }

            public void Delete(string UserName, string coinType)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    var row = this.FirstOrDefault(_ => _.交易员 == UserName && _.币种 == coinType);
                    this.Remove可用资金Row(row);
                }
            }

            public 可用资金DataTable Query可用资金(string userName, string coinType)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    可用资金DataTable table = new 可用资金DataTable();
                    foreach (var item in this)
                    {
                        if ((string.IsNullOrEmpty(userName) || item.交易员 == userName) && (string.IsNullOrEmpty(coinType) || item.币种 == coinType))
                        {
                            table.ImportRow(item);
                        }
                    }
                    return table;
                }
            }

            public 可用资金DataTable QueryAll可用资金()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    可用资金DataTable table = new 可用资金DataTable();
                    foreach (var item in this)
                    {
                        table.ImportRow(item);
                    }
                    return table;
                }
            }


        }

        partial class 交易账户关联DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["交易员"] };
                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (var item in db.交易账户关联)
                        {
                            var row = this.New交易账户关联Row();
                            row.交易员 = item.交易员;
                            row.电子币账户 = item.电子币账户;
                            this.Add交易账户关联Row(row);
                        }
                    }

                    this.交易账户关联RowChanging += 交易账户关联表_关联表RowChanging;
                    this.交易账户关联RowDeleting += 交易账户关联表_关联表RowChanging;
                }
            }

            void 交易账户关联表_关联表RowChanging(object sender, DbDataSet.交易账户关联RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            var row = new 交易账户关联();
                            row.交易员 = e.Row.交易员;
                            row.电子币账户 = e.Row.电子币账户;
                            db.交易账户关联.Add(row);
                            db.SaveChanges();
                            //Program.交易账户关联Changed[e.Row.交易员] = true;
                            break;
                        case DataRowAction.Delete:
                            row = db.交易账户关联.Find(e.Row.交易员);
                            db.交易账户关联.Remove(row);
                            db.SaveChanges();

                            //Program.交易账户关联Changed[e.Row.交易员] = true;
                            break;
                        case DataRowAction.Change:
                            row = db.交易账户关联.Find(e.Row.交易员);
                            row.电子币账户 = e.Row.电子币账户;
                            db.Entry(row).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();

                            //Program.交易账户关联Changed[e.Row.交易员] = true;
                            break;
                        default:
                            break;

                    }
                }
            }

            public bool Exists(string 交易员)
            {
                using (ReadWriteLock readWriteLock = new ReadWriteLock(readerWriterLockSlim, ReadWriteMode.Read))
                {
                    return this.Any(_ => _.交易员 == 交易员);
                }
            }

            public 交易账户关联Row GetRelation(string 交易员)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    交易账户关联DataTable table = new 交易账户关联DataTable();
                    var row = this.FirstOrDefault(_ => _.交易员 == 交易员);
                    if (row != null)
                    {
                        table.ImportRow(row);
                        return table[0];
                    }
                    return null;
                }
            }

            public void UpdateRelation(string 交易员, string 电子币账户)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    var row = this.First(_ => _.交易员 == 交易员);
                    row.电子币账户 = 电子币账户;
                }
            }

            public void AddRelation(string 交易员, string 电子币账户)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    var row = this.New交易账户关联Row();
                    row.交易员 = 交易员;
                    row.电子币账户 = 电子币账户;
                    this.Add交易账户关联Row(row);
                }
            }

            public void DeleteRelation(string 交易员)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    var row = this.First(_ => _.交易员 == 交易员);
                    this.Remove交易账户关联Row(row);
                }
            }

            public DbDataSet.交易账户关联DataTable QeuryAccountRelation()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    交易账户关联DataTable table = new 交易账户关联DataTable();
                    foreach (var item in this)
                    {
                        table.ImportRow(item);
                    }
                    return table;
                }
            }
        }
    }
}

