using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using System.ServiceModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using AASServer.CATSEntity;





namespace AASServer
{


    public partial class DbDataSet
    {
        partial class 顶点账户DataTable
        {
        }


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

            this.恒生帐户.Load();
        }




        public AASServer.DbDataSet.平台用户DataTable QueryJyBelongFK(string FKUserName)
        {
            AASServer.DbDataSet.平台用户Row 风控员 = this.平台用户.Get平台用户(FKUserName);
            AASServer.DbDataSet.平台用户DataTable 交易员DataTable = this.平台用户.QueryUserInRoles(new int[] { (int)角色.交易员 }, 风控员.分组);


            if (风控员.角色 == (int)角色.普通风控员)
            {
                AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                foreach (AASServer.DbDataSet.平台用户Row 交易员Row1 in 交易员DataTable)
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
                AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                foreach (AASServer.DbDataSet.平台用户Row 交易员Row1 in 交易员DataTable)
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

        public AASServer.DbDataSet.平台用户DataTable QueryJyNotBelongFK(string FKUserName)
        {
            AASServer.DbDataSet.平台用户Row 风控员 = this.平台用户.Get平台用户(FKUserName);
            AASServer.DbDataSet.平台用户DataTable 交易员DataTable = this.平台用户.QueryUserInRoles(new int[] { (int)角色.交易员 }, 风控员.分组);
            if (风控员.角色 == (int)角色.普通风控员)
            {
                AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                foreach (AASServer.DbDataSet.平台用户Row 交易员Row1 in 交易员DataTable)
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
                AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                return 平台用户DataTable1;
            }
            else
            {
                throw new FaultException("非风控用户");
            }
        }








        partial class 恒生帐户DataTable
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public void Load()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["名称"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        foreach (恒生帐户 恒生帐户1 in db.恒生帐户)
                        {
                            恒生帐户Row 恒生帐户Row1 = this.New恒生帐户Row();
                            恒生帐户Row1.启用 = 恒生帐户1.启用;
                            恒生帐户Row1.名称 = 恒生帐户1.名称;
                            恒生帐户Row1.IP = 恒生帐户1.IP;
                            恒生帐户Row1.端口 = 恒生帐户1.端口;
                            恒生帐户Row1.基金编码 = 恒生帐户1.基金编码;
                            恒生帐户Row1.资产单元编号 = 恒生帐户1.资产单元编号;
                            恒生帐户Row1.组合编号 = 恒生帐户1.组合编号;
                            恒生帐户Row1.操作员用户名 = 恒生帐户1.操作员用户名;
                            恒生帐户Row1.登录IP = 恒生帐户1.登录IP;
                            恒生帐户Row1.MAC = 恒生帐户1.MAC;
                            恒生帐户Row1.HDD = 恒生帐户1.HDD;
                            恒生帐户Row1.操作员密码 = Cryptor.MD5Decrypt(恒生帐户1.操作员密码);
                            恒生帐户Row1.查询间隔时间 = 恒生帐户1.查询间隔时间;
                            this.Add恒生帐户Row(恒生帐户Row1);
                        }
                    }



                    this.恒生帐户RowChanging += 恒生帐户DataTable_恒生帐户RowChanging;
                    this.恒生帐户RowDeleting += 恒生帐户DataTable_恒生帐户RowChanging;
                }
            }

            void 恒生帐户DataTable_恒生帐户RowChanging(object sender, 恒生帐户RowChangeEvent e)
            {
                using (AASDbContext db = new AASDbContext())
                {
                    switch (e.Action)
                    {
                        case DataRowAction.Add:
                            恒生帐户 恒生帐户1 = new 恒生帐户();
                            恒生帐户1.启用 = e.Row.启用;
                            恒生帐户1.名称 = e.Row.名称;
                            恒生帐户1.IP = e.Row.IP;
                            恒生帐户1.端口 = e.Row.端口;
                            恒生帐户1.基金编码 = e.Row.基金编码;
                            恒生帐户1.资产单元编号 = e.Row.资产单元编号;
                            恒生帐户1.组合编号 = e.Row.组合编号;
                            恒生帐户1.操作员用户名 = e.Row.操作员用户名;
                            恒生帐户1.操作员密码 = Cryptor.MD5Encrypt(e.Row.操作员密码);
                            恒生帐户1.登录IP = e.Row.登录IP;
                            恒生帐户1.MAC = e.Row.MAC;
                            恒生帐户1.HDD = e.Row.HDD;
                            恒生帐户1.查询间隔时间 = e.Row.查询间隔时间;
                            db.恒生帐户.Add(恒生帐户1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            恒生帐户1 = db.恒生帐户.Find(e.Row.名称);
                            db.恒生帐户.Remove(恒生帐户1);
                            db.SaveChanges();
                            break;
                        case DataRowAction.Change:
                            恒生帐户1 = db.恒生帐户.Find(e.Row.名称);
                            恒生帐户1.启用 = e.Row.启用;
                            恒生帐户1.名称 = e.Row.名称;
                            恒生帐户1.IP = e.Row.IP;
                            恒生帐户1.端口 = e.Row.端口;
                            恒生帐户1.基金编码 = e.Row.基金编码;
                            恒生帐户1.资产单元编号 = e.Row.资产单元编号;
                            恒生帐户1.组合编号 = e.Row.组合编号;
                            恒生帐户1.操作员用户名 = e.Row.操作员用户名;
                            恒生帐户1.操作员密码 = Cryptor.MD5Encrypt(e.Row.操作员密码);
                            恒生帐户1.登录IP = e.Row.登录IP;
                            恒生帐户1.MAC = e.Row.MAC;
                            恒生帐户1.HDD = e.Row.HDD;
                            恒生帐户1.查询间隔时间 = e.Row.查询间隔时间;
                            db.Entry(恒生帐户1).State = System.Data.Entity.EntityState.Modified;
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


            public 恒生帐户DataTable QueryHsAccount()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    恒生帐户DataTable 恒生帐户DataTable1 = new 恒生帐户DataTable();
                    foreach (恒生帐户Row 恒生帐户Row1 in this)
                    {
                        恒生帐户DataTable1.ImportRow(恒生帐户Row1);
                    }
                    return 恒生帐户DataTable1;
                }
            }


            public void EnableHsAccount(string Name, bool Enabled)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.恒生帐户Row 恒生帐户Row1 = this.First(r => r.名称 == Name);
                    恒生帐户Row1.EnableHsAccount(Enabled);
                }
            }



            public void AddHsAccount(bool 启用, string 名称, string IP, short 端口, string 基金编码, string 资产单元编号, string 组合编号, string 操作员用户名, string 操作员密码, string 登录IP, string MAC, string HDD, int 查询间隔时间)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    恒生帐户Row 恒生帐户Row1 = this.New恒生帐户Row();
                    恒生帐户Row1.启用 = 启用;
                    恒生帐户Row1.名称 = 名称;
                    恒生帐户Row1.IP = IP;
                    恒生帐户Row1.端口 = 端口;
                    恒生帐户Row1.基金编码 = 基金编码;
                    恒生帐户Row1.资产单元编号 = 资产单元编号;
                    恒生帐户Row1.组合编号 = 组合编号;
                    恒生帐户Row1.操作员用户名 = 操作员用户名;
                    恒生帐户Row1.操作员密码 = 操作员密码;
                    恒生帐户Row1.登录IP = 登录IP;
                    恒生帐户Row1.MAC = MAC;
                    恒生帐户Row1.HDD = HDD;
                    恒生帐户Row1.查询间隔时间 = 查询间隔时间;
                    this.Add恒生帐户Row(恒生帐户Row1);
                }
            }


            public void UpdateHsAccount(string 名称, string IP, short 端口, string 基金编码, string 资产单元编号, string 组合编号, string 操作员用户名, string 操作员密码, string 登录IP, string MAC, string HDD, int 查询间隔时间)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    恒生帐户Row 恒生帐户Row1 = this.First(r => r.名称 == 名称);
                    恒生帐户Row1.UpdateHsAccount(IP, 端口, 基金编码, 资产单元编号, 组合编号, 操作员用户名, 操作员密码, 登录IP, MAC, HDD, 查询间隔时间);
                }
            }


            public void DeleteHsAccount(string Name)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.恒生帐户Row 恒生帐户Row1 = this.First(r => r.名称 == Name);
                    恒生帐户Row1.Stop();
                    Program.db.恒生帐户.Remove恒生帐户Row(恒生帐户Row1);
                }

            }







            public void Start()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (AASServer.DbDataSet.恒生帐户Row 恒生帐户Row1 in this)
                    {
                        if (!恒生帐户Row1.IsBusy)
                        {
                            恒生帐户Row1.Start();
                        }
                    }
                }
            }

            public void Stop()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (AASServer.DbDataSet.恒生帐户Row 恒生帐户Row1 in this)
                    {
                        if (恒生帐户Row1.IsBusy)
                        {
                            恒生帐户Row1.Stop();
                        }
                    }
                }
            }



            public void SendOrder(string 组合号, string 买卖类别, string 市场, string 证券代码, decimal 委托价格, decimal 委托数量, OrderCacheEntity orderCacheObj, out string Result, out string ErrInfo)
            {
                Result = string.Empty;
                ErrInfo = string.Empty;

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.恒生帐户Row 恒生帐户Row1 = this.FirstOrDefault(r => r.名称 == 组合号);


                    恒生帐户Row1.SendOrder(买卖类别, 市场, 证券代码, 委托价格, 委托数量, orderCacheObj, out Result, out ErrInfo);

                }
            }

            public void CancelOrder(string 组合号, int hth, out string Result, out string ErrInfo)
            {
                Result = string.Empty;
                ErrInfo = string.Empty;

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.恒生帐户Row 恒生帐户Row1 = this.FirstOrDefault(r => r.名称 == 组合号);
                    if (恒生帐户Row1 == null)
                    {
                        ErrInfo = string.Format("{0}不存在", 组合号);
                        return;
                    }


                    恒生帐户Row1.CancelOrder(hth, out Result, out ErrInfo);
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

        partial class 恒生帐户Row
        {
            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public object SendOrderObject = new object();

            public void EnableHsAccount(bool Enabled)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.启用 = Enabled;
                }
            }


            public void UpdateHsAccount(string IP, short 端口, string 基金编码, string 资产单元编号, string 组合编号, string 操作员用户名, string 操作员密码, string 登录IP, string MAC, string HDD, int 查询间隔时间)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.名称 = 名称;
                    this.IP = IP;
                    this.端口 = 端口;
                    this.基金编码 = 基金编码;
                    this.资产单元编号 = 资产单元编号;
                    this.组合编号 = 组合编号;
                    this.操作员用户名 = 操作员用户名;
                    this.操作员密码 = 操作员密码;
                    this.登录IP = 登录IP;
                    this.MAC = MAC;
                    this.HDD = HDD;
                    this.查询间隔时间 = 查询间隔时间;
                }
            }










            HsClient HsClient = new HsClient();

            string UserToken = string.Empty;






            public JyDataSet.成交DataTable 帐户成交DataTable = new JyDataSet.成交DataTable();
            public JyDataSet.委托DataTable 帐户委托DataTable = new JyDataSet.委托DataTable();



            BackgroundWorker backgroundWorker1 = new BackgroundWorker();




            public void Start()
            {
                this.backgroundWorker1.WorkerSupportsCancellation = true;
                this.backgroundWorker1.WorkerReportsProgress = true;
                this.backgroundWorker1.DoWork += backgroundWorker1_DoWork;
                this.backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
                this.backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
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




            void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
            {
                while (!this.backgroundWorker1.CancellationPending)
                {
                    if (this.Safe启用)
                    {
                        if (DateTime.Parse(Program.appConfig.GetValue("开始查询时间", "8:15")) <= DateTime.Now && DateTime.Now <= DateTime.Parse(Program.appConfig.GetValue("结束查询时间", "15:30")))
                        {
                            #region 交易时段
                            if (this.UserToken == string.Empty)
                            {
                                #region 登录
                                this.backgroundWorker1.ReportProgress(0, "登录中...");

                                string Msg = this.Logon();

                                this.backgroundWorker1.ReportProgress(0, Msg);
                                #endregion

                                Thread.Sleep(1000);
                            }
                            else
                            {
                                try
                                {
                                    #region 工作

                                    DateTime DateTime1 = DateTime.Now;

                                    this.QueryData();

                                    this.帐户委托DataTable.Deal();


                                    this.backgroundWorker1.ReportProgress(0, (DateTime.Now - DateTime1).TotalSeconds.ToString());

                                    #endregion

                                    Thread.Sleep(100);
                                }
                                catch (Exception ex)
                                {
                                    Program.logger.LogInfo("恒生帐户 {0} 线程异常:{1} {2}", this.名称, ex.Message, ex.StackTrace);

                                    Thread.Sleep(1000);
                                }
                            }

                            #endregion

                        }
                        else
                        {
                            #region 非交易时段
                            if (this.UserToken != string.Empty)
                            {
                                this.backgroundWorker1.ReportProgress(0, "注销中...");

                                this.Logoff();

                                this.backgroundWorker1.ReportProgress(0, "已注销");
                            }


                            this.backgroundWorker1.ReportProgress(0, "非交易时段");

                            Thread.Sleep(1000);

                            #endregion
                        }
                    }
                    else
                    {
                        #region 未启用
                        if (this.UserToken != string.Empty)
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

            void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                switch (e.ProgressPercentage)
                {
                    case 0:
                        Program.mainForm.BeginInvoke((Action)delegate()
                        {
                            UIDataSet.交易帐户Row 交易帐户Row1 = Program.UIdataSet.交易帐户.FirstOrDefault(r => r.组合号 == this.名称);
                            if (交易帐户Row1 == null)
                            {
                                交易帐户Row1 = Program.UIdataSet.交易帐户.New交易帐户Row();
                                交易帐户Row1.组合号 = this.名称;
                                交易帐户Row1.查询耗时 = e.UserState as string;
                                if (CommonUtils.ExistsGroup(this.名称))
                                {
                                    交易帐户Row1.IP = CommonUtils.GetGroupConfig(this.名称).ServerIP;
                                }
                                else {
                                    交易帐户Row1.IP = "本地登陆";
                                }
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

            void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Error == null)
                {
                    Program.mainForm.BeginInvoke((Action)delegate()
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
                    Program.mainForm.BeginInvoke((Action)delegate()
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
                    string ErrInfo;
                    this.HsClient.Open(this.IP, this.端口, out ErrInfo);
                    if (ErrInfo != string.Empty)
                    {
                        return "连接服务器失败:" + ErrInfo.ToString();
                    }
                    else
                    {
                        this.UserToken = this.HsClient.Logon(this.操作员用户名, this.操作员密码, this.登录IP, this.MAC, this.HDD, out ErrInfo);
                        if (this.UserToken == string.Empty)
                        {
                            return "登录失败:" + ErrInfo.ToString();
                        }
                        else
                        {
                            return "登录成功";
                        }
                    }
                }
            }

            public void Logoff()
            {
                this.HsClient.Logout(this.UserToken);

                this.UserToken = string.Empty;

                this.HsClient.Close();
            }


            public void QueryData()
            {
                DataTable 查到的委托DataTable;
                string 查到的委托ErrInfo;
                this.HsClient.QryData(this.UserToken, 32001, this.基金编码, this.资产单元编号, this.组合编号, out 查到的委托DataTable, out 查到的委托ErrInfo);
                Thread.Sleep(this.查询间隔时间 / 2);



                DataTable 查到的成交DataTable;
                string 查到的成交ErrInfo;
                this.HsClient.QryData(this.UserToken, 33001, this.基金编码, this.资产单元编号, this.组合编号, out 查到的成交DataTable, out 查到的成交ErrInfo);
                Thread.Sleep(this.查询间隔时间 / 2);









                if (查到的成交ErrInfo == string.Empty)
                {
                    this.帐户成交DataTable = this.Get规范成交(查到的成交DataTable);
                }
                else if (查到的成交ErrInfo == "无数据")
                {
                    this.帐户成交DataTable.Clear();
                }
                else
                {
                    Program.logger.LogInfo("恒生帐户 {0} 查询成交时出错:{1}", this.名称, 查到的成交ErrInfo);
                }



                if (查到的委托ErrInfo == string.Empty)
                {
                    this.帐户委托DataTable = this.Get规范委托(查到的委托DataTable);
                }
                else if (查到的委托ErrInfo == "无数据")
                {
                    this.帐户委托DataTable.Clear();
                }
                else
                {
                    Program.logger.LogInfo("恒生帐户 {0} 查询委托时出错:{1}", this.名称, 查到的委托ErrInfo);
                }





                Program.帐户成交DataTable[this.名称] = this.帐户成交DataTable.Copy() as JyDataSet.成交DataTable;
                Program.帐户委托DataTable[this.名称] = this.帐户委托DataTable.Copy() as JyDataSet.委托DataTable;


            }



            AASServer.JyDataSet.成交DataTable Get规范成交(DataTable 查到的成交DataTable)
            {
                AASServer.JyDataSet.成交DataTable 规范的成交DataTable = new JyDataSet.成交DataTable();

                foreach (DataRow DataRow0 in 查到的成交DataTable.Rows)
                {
                    AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.table恒生帐户.DbDataSet.已发委托.Get已发委托(DateTime.Today, this.名称, DataRow0["entrust_no"] as string);

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
                        成交Row1.成交编号 = 规范的成交DataTable.Count.ToString();

                        成交Row1.成交时间 = DataRow0["deal_time"] as string;
                        成交Row1.成交价格 = decimal.Parse(DataRow0["deal_price"] as string);
                        成交Row1.成交数量 = decimal.Parse(DataRow0["deal_amount"] as string);
                        成交Row1.成交金额 = decimal.Parse(DataRow0["deal_balance"] as string);


                        规范的成交DataTable.Add成交Row(成交Row1);
                    }
                }
                return 规范的成交DataTable;
            }


            AASServer.JyDataSet.委托DataTable Get规范委托(DataTable 查到的委托DataTable)
            {
                AASServer.JyDataSet.委托DataTable 规范的委托DataTable = new JyDataSet.委托DataTable();

                foreach (DataRow DataRow0 in 查到的委托DataTable.Rows)
                {
                    AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.table恒生帐户.DbDataSet.已发委托.Get已发委托(DateTime.Today, this.名称, DataRow0["entrust_no"] as string);
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


                        委托Row1.委托时间 = DataRow0["entrust_time"] as string;
                        委托Row1.成交价格 = decimal.Parse(DataRow0["deal_price"] as string);
                        委托Row1.成交数量 = decimal.Parse(DataRow0["deal_amount"] as string);
                        委托Row1.状态说明 = this.Get委托状态(DataRow0["entrust_state"] as string);
                        decimal 撤单数量 = decimal.Parse(DataRow0["withdraw_amount"] as string);
                        if (撤单数量 > 0)
                        {
                            委托Row1.撤单数量 = 撤单数量;
                        }
                        else
                        {
                            委托Row1.撤单数量 = 委托Row1.计算撤单数量();
                        }


                        规范的委托DataTable.Add委托Row(委托Row1);
                    }
                }
                return 规范的委托DataTable;
            }


            string Get委托状态(string 状态代码)
            {
                switch (状态代码)
                {
                    case "1":
                        return "未报";
                    case "2":
                        return "待报";
                    case "3":
                        return "正报";
                    case "4":
                        return "已报";
                    case "5":
                        return "废单";
                    case "6":
                        return "部成";
                    case "7":
                        return "已成";
                    case "8":
                        return "部撤";
                    case "9":
                        return "已撤";
                    case "a":
                        return "待撤";
                    case "A":
                        return "未撤";
                    case "B":
                        return "待撤";
                    case "C":
                        return "正撤";
                    case "D":
                        return "撤认";
                    case "E":
                        return "撤废";
                    case "F":
                        return "已撤";
                    default:
                        return "未知";
                }
            }





            public void SendOrder(string Category, string Market, string Zqdm, decimal Price, decimal Quantity, OrderCacheEntity orderCacheObj, out string Result, out string ErrInfo)
            {
                lock (this.SendOrderObject)
                {
                    Result = string.Empty;
                    ErrInfo = string.Empty;


                    if (this.UserToken == string.Empty)
                    {
                        ErrInfo = string.Format("{0}未登录到恒生交易服务器", this.名称);
                        return;
                    }




                    DataTable 下单前委托DataTable;
                    string 下单前ErrInfo;
                    while (true)
                    {
                        #region 下单前查一下委托
                        this.HsClient.QryData(this.UserToken, 32001, this.基金编码, this.资产单元编号, this.组合编号, out 下单前委托DataTable, out 下单前ErrInfo);
                        if (下单前ErrInfo == string.Empty)
                        {
                            break;
                        }
                        else
                        {
                            if (下单前ErrInfo == "无数据")
                            {
                                下单前委托DataTable = new DataTable();
                                break;
                            }
                            else
                            {
                                Thread.Sleep(100);
                            }
                        }
                        #endregion
                    }






                    DataTable DataTable1;
                    this.HsClient.SendOrder(this.UserToken, this.基金编码, this.资产单元编号, this.组合编号, Category, "0", Market, Zqdm, (double)Price, (double)Quantity, out DataTable1, out ErrInfo);
                    if (ErrInfo == string.Empty)
                    {
                        Result = DataTable1.Rows[0]["entrust_no"] as string;
                    }
                    else
                    {
                        if (ErrInfo.IndexOf("超时") != -1)
                        {
                            //Thread.Sleep(5000);//等5秒再查

                            DataTable 下单后委托DataTable;
                            string 下单后ErrInfo;
                            while (true)
                            {
                                #region 下单后查一下委托
                                this.HsClient.QryData(this.UserToken, 32001, this.基金编码, this.资产单元编号, this.组合编号, out 下单后委托DataTable, out 下单后ErrInfo);
                                if (下单后ErrInfo == string.Empty)
                                {
                                    break;
                                }
                                else
                                {
                                    if (下单后ErrInfo == "无数据")
                                    {
                                        下单后委托DataTable = new DataTable();
                                        break;
                                    }
                                    else
                                    {
                                        Thread.Sleep(100);
                                    }
                                }
                                #endregion
                            }


                            for (int i = 下单前委托DataTable.Rows.Count; i < 下单后委托DataTable.Rows.Count; i++)
                            {
                                DataRow DataRow0 = 下单后委托DataTable.Rows[i];
                                if (Category == DataRow0["entrust_direction"] as string &&
                                    "0" == DataRow0["price_type"] as string &&
                                    Market == DataRow0["market_no"] as string &&
                                    Zqdm == DataRow0["stock_code"] as string &&
                                    Price == decimal.Parse(DataRow0["entrust_price"] as string) &&
                                    Quantity == decimal.Parse(DataRow0["entrust_amount"] as string)
                                    )
                                {
                                    ErrInfo = string.Empty;
                                    Result = DataRow0["entrust_no"] as string;
                                    return;
                                }
                            }


                            Result = string.Empty;
                            return;
                        }
                        else
                        {
                            Result = string.Empty;
                        }
                    }
                }

            }

            public void CancelOrder(int hth, out string Result, out string ErrInfo)
            {

                Result = string.Empty;
                ErrInfo = string.Empty;


                if (this.UserToken == string.Empty)
                {
                    ErrInfo = string.Format("{0}未登录到恒生交易服务器", this.名称);
                    return;
                }


                DataTable DataTable1;
                this.HsClient.CancelOrder(this.UserToken, hth, out DataTable1, out ErrInfo);
                if (ErrInfo == string.Empty)
                {
                    Result = "撤单成功";
                }
                else
                {
                    Result = string.Empty;
                }
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

            public int GetClientID(string Name)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (var item in this)
                    {
                        if (item.名称 == Name)
                        {
                            return item.ClientID;
                        }
                    }
                    return -1;
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

            public void QueryPosition(string name, int dataType, StringBuilder result, StringBuilder error)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (券商帐户Row item in this)
                    {
                        if (item.名称 == name)
                        {
                            item.QueryPosition(dataType, result, error);
                            return;
                        }
                    }
                    error.Append("未找到对应券商账户!");
                }
            }

            public void EnableQSAccount(string Name, bool Enabled)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户Row1 = this.First(r => r.名称 == Name);
                    券商帐户Row1.EnableQSAccount(Enabled);
                }
            }


            public void AddQSAccount(bool Enabled, string Name, string QS, string Type, string 交易服务器, string Version, short YybID, string Account, string TradeAccount, string JyPassword, string TxPassword, string SHGDDM, string SZGDDM, int 查询间隔时间)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户Row1 = this.New券商帐户Row();
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
                    AASServer.DbDataSet.券商帐户Row 券商帐户Row1 = this.First(r => r.名称 == Name);
                    券商帐户Row1.UpdateQSAccount(交易服务器, Version, 交易密码, 通讯密码, 查询间隔时间);
                }

            }


            public void DeleteQSAccount(string Name)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户Row1 = this.First(r => r.名称 == Name);
                    券商帐户Row1.Stop();
                    Program.db.券商帐户.Remove券商帐户Row(券商帐户Row1);
                }

            }






            public void Start()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    foreach (AASServer.DbDataSet.券商帐户Row 券商帐户Row1 in this)
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
                    foreach (AASServer.DbDataSet.券商帐户Row 券商帐户Row1 in this)
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
                    AASServer.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == 组合号);


                    券商帐户1.SendOrder(买卖类别, 市场, 证券代码, 委托价格, 委托数量, orderCacheObj, out Result, out ErrInfo, out hasOrderNo);

                }
            }

            public void SendOrderLocal(string 组合号, int 买卖类别, string 股东代码, string 证券代码, decimal 委托价格, decimal 委托数量, string mac,  out string Result, out string ErrInfo)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == 组合号);
                    券商帐户1.SendOrderLocal(买卖类别, 股东代码, 证券代码, 委托价格, 委托数量, out Result, out ErrInfo);
                }
            }

            public GroupClient.GroupService.GroupQueryData QueryDataLocl(string 组合号)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == 组合号);
                    return 券商帐户1.LocalData;
                }
            }

            public void CancelOrder(string Zqdm, string 组合号, byte Market, string hth, out string Result, out string ErrInfo)
            {
                Result = string.Empty;
                ErrInfo = string.Empty;

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == 组合号);
                    if (券商帐户1 == null)
                    {
                        ErrInfo = string.Format("{0}不存在", 组合号);
                        return;
                    }


                    券商帐户1.CancelOrder(Zqdm, Market, hth, out Result, out ErrInfo);
                }
            }

            public void CancelOrderLocal(string 组合号, string hth, string ExchangeID, out string Result, out string ErrInfo)
            {
                Result = string.Empty;
                ErrInfo = string.Empty;

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == 组合号);
                    if (券商帐户1 == null)
                    {
                        ErrInfo = string.Format("{0}不存在", 组合号);
                        return;
                    }


                    券商帐户1.CancelOrderLocal(hth, ExchangeID, out Result, out ErrInfo);
                }
            }

            public void QueryPubStocks(string group, out string Result, out string ErrInfo)
            {
                Result = string.Empty;
                ErrInfo = string.Empty;

                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == group);
                    if (券商帐户1 == null)
                    {
                        ErrInfo = string.Format("{0}不存在", group);
                        return;
                    }

                    券商帐户1.QueryPubStocks(out Result, out ErrInfo);
                }
            }

            public string AccountRepay(string group, decimal amount)
            {
                StringBuilder result = new StringBuilder(1024);
                StringBuilder ErrMsg = new StringBuilder(1024);
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.券商帐户Row 券商帐户1 = this.FirstOrDefault(r => r.名称 == group);
                    if (券商帐户1 == null)
                    {
                        return string.Format("{0}不存在", group);
                    }
                    券商帐户1.Repay(Math.Round(amount, 3), result, ErrMsg);
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

            public ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();

            public static object LoginObject = new object();


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

            bool IsCATSAccount 
            {
                get
                {
                    return ConfigCache.CatsGroups != null && ConfigCache.CatsGroups.Contains(this.名称);
                }
            }

            bool IsImsAccount
            {
                get
                {
                    if (券商 != null && 券商.ToLower().Contains("ims"))
                    {
                        return true;
                    }
                    return false;
                }
            }


            public int ClientID = -1;


            public JyDataSet.成交DataTable 帐户成交DataTable = new JyDataSet.成交DataTable();
            public JyDataSet.委托DataTable 帐户委托DataTable = new JyDataSet.委托DataTable();



            BackgroundWorker backgroundWorker1 = new BackgroundWorker();



            object SendOrderObject = new object();
            ConcurrentQueue<string> OrderIDs = null;
            
            public void Start()
            {
                this.backgroundWorker1.WorkerSupportsCancellation = true;
                this.backgroundWorker1.WorkerReportsProgress = true;
                this.backgroundWorker1.DoWork += backgroundWorker1_DoWork;
                this.backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
                this.backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
                this.backgroundWorker1.RunWorkerAsync();

                var list = Program.db.已发委托.GetOrderIDList(DateTime.Today, this.名称);
                OrderIDs = new ConcurrentQueue<string>(list);
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

            //bool IsPubStockGroup = false;
            string PubStockResult = string.Empty;
            string PubStockErrInfo = string.Empty;
            void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
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

                                    //if (IsPubStockGroup)
                                    //{
                                    //    string strResltPub, strErrInfo;
                                    //    this.QueryPubStockInfo(out strResltPub, out strErrInfo);
                                    //    PubStockResult = strResltPub;
                                    //    PubStockErrInfo = strErrInfo;
                                    //}
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

            void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                switch (e.ProgressPercentage)
                {
                    case 0:
                        Program.mainForm.BeginInvoke((Action)delegate()
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
                            if (CommonUtils.ExistsGroup(this.名称))
                            {
                                交易帐户Row1.IP = CommonUtils.GetGroupConfig(this.名称).ServerIP;
                            }
                            else
                            {
                                交易帐户Row1.IP = "本地登陆";
                            }
                        });
                        break;
                    default:
                        break;
                }

            }

            void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                if (e.Error == null)
                {
                    Program.mainForm.BeginInvoke((Action)delegate()
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
                    Program.mainForm.BeginInvoke((Action)delegate()
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
                    if (this.IsCATSAccount)
                    {
                        if (!CommonUtils.ExistsGroup(this.名称))
                        {
                            return "未配置connect IP的 CATS账户！";
                        }
                        #region Cats Logon
                        try
                        {
                            if (CatsAdapter == null)
                            {
                                CatsAdapter = new CATSAdapter(this.名称);
                                CatsAdapter.OnOrdersChanged += new Action<List<CATSEntity.StandardOrderEntity>>(CacheNewestList);
                            }

                            if (CatsAdapter.Connected)
                            {
                                this.ClientID = 0;
                                return "登录成功";
                            }
                            else
                            {
                                return "登录中……";
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.logger.LogInfo(ex.Message);
                            return "登陆失败";
                        }
                        #endregion
                    }
                    else
                    {
                        if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday && DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
                        {
                            return "周末休息时间无法登录!";
                        }

                        if (CommonUtils.ExistsGroup(this.名称))
                        {
                            #region 组合号接口ip配置
                            var client = CommonUtils.GetGroupClient(this.名称);
                            if (client == null || client.State == CommunicationState.Faulted)
                            {
                                return "未配置组合号对应接口地址或接口地址异常！";
                            }
                            else
                            {
                                try
                                {
                                    this.ClientID = client.GetGroupClientID(this.名称);

                                    client.Close();
                                }
                                catch (System.ServiceModel.Security.SecurityNegotiationException)
                                {
                                    return "连接异常";
                                }
                                catch (Exception)
                                {
                                    //return "登录失败:" + ex.Message;
                                }
                                return ClientID > -1 ? "登录成功" : string.Format("登录失败, 配置ip为{0}, 请检查终端是否已启动", client.Endpoint.Address.Uri.Host);
                            }
                            #endregion
                        }
                        else
                        {
                            this.ClientID = TdxApi.Logon(JyServerInfo[1], short.Parse(JyServerInfo[2]), this.版本号, this.营业部代码, this.登录帐号, this.交易帐号, this.交易密码, this.通讯密码, ErrInfo);
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
                }
            }

            

            public void Logoff()
            {
                if (!CommonUtils.ExistsGroup(this.名称))
                {
                    TdxApi.Logoff(this.ClientID);
                }

                this.ClientID = -1;


            }

            
            double lastTimeCost = 0;
            string strWtTitle = null;
            string strCjTitle = null;
            public string QueryData()
            {
                if (IsCATSAccount)
                {
                    return CATSQueryData();
                }

                //判断是否需要查询，出现不能推送至客户端问题，需研究为什么。
                //if ((this.帐户委托DataTable.Count == OrderIDs.Count) && this.帐户委托DataTable.FirstOrDefault(_ => _.成交价格 > 0 && _.委托数量 > (_.成交数量 + _.撤单数量)) == null)
                //{
                //    Thread.Sleep(100);
                //    return "无需查询委托";
                //}
                DateTime dtStart = DateTime.Now;
                
                double timeCost = 0;
                DataTable 查到的成交Result = null;
                string 查到的成交ErrInfo = null;

                DataTable 查到的委托Result = null;
                string 查到的委托ErrInfo = null;
                if (CommonUtils.ExistsGroup(this.名称))
                {
                    QueryDataFromClient(ref timeCost, ref 查到的成交Result, ref 查到的成交ErrInfo, ref 查到的委托Result, ref 查到的委托ErrInfo);
                    double ms = (DateTime.Now - dtStart).TotalMilliseconds;
                    if (ms < 300)
                    {
                        Thread.Sleep(300 - (int)ms);
                    }
                }
                else
                {
                    QueryDataLocal(ref timeCost, ref 查到的成交Result, ref 查到的成交ErrInfo, ref 查到的委托Result, ref 查到的委托ErrInfo);
                }

                DateTime dtMid = DateTime.Now;
                //Program.logger.LogInfo("查询耗时:{0}", (dtMid - dtStart).TotalSeconds);

                if (isTradeResultChange || isOperateResultChange)
                {
                    if ((查到的成交ErrInfo ?? "").Length == 0)
                    {
                        if (查到的成交Result != null && 查到的成交Result.Rows.Count > 0)
                        {
                            this.帐户成交DataTable = this.Get规范成交(查到的成交Result);
                        }
                    }
                    else if (lastTimeCost != timeCost)
                    {
                        Program.logger.LogInfo("券商帐户 {0} 查询成交时出错:{1}", this.名称, 查到的成交ErrInfo);
                    }


                    if ((查到的委托ErrInfo ?? "").Length == 0)
                    {
                        if (查到的委托Result != null && 查到的委托Result.Rows.Count > 0)
                        {
                            this.帐户委托DataTable = this.Get规范委托(查到的委托Result);
                        }
                    }
                    else if (lastTimeCost != timeCost)
                    {
                        Program.logger.LogInfo("券商帐户 {0} 查询委托时出错:{1}", this.名称, 查到的委托ErrInfo);
                    }

                    lastTimeCost = timeCost;

                    Program.帐户成交DataTable[this.名称] = this.帐户成交DataTable.Copy() as JyDataSet.成交DataTable;
                    Program.帐户委托DataTable[this.名称] = this.帐户委托DataTable.Copy() as JyDataSet.委托DataTable;

                    if (ConfigCache.UseLogDetail == "1")
                    {
                        if (查到的委托Result != null && 查到的委托Result.Rows.Count > 0)
                        {
                            var dtNow = DateTime.Now;
                            foreach (var item in 帐户委托DataTable)
                            {
                                if (dictOrderSend.ContainsKey(item.委托编号) && dictOrderSend[item.委托编号] > DateTime.MinValue && item.成交数量 > 0)
                                {
                                    Program.logger.LogInfoDetail("下单委托编号{0}首次收到委托成交数量大于0信息，时间间隔{1}, 数据处理时间{2},组合号{3}", item.委托编号, (dtNow - dictOrderSend[item.委托编号]).TotalSeconds, (dtNow - dtMid).TotalSeconds, this.名称);
                                    dictOrderSend[item.委托编号] = DateTime.MinValue;
                                }
                                if (dictOrderCancel.ContainsKey(item.委托编号) && dictOrderCancel[item.委托编号] > DateTime.MinValue && item.撤单数量 > 0)
                                {
                                    Program.logger.LogInfoDetail("撤单委托编号{0}首次收到含撤单数量数据，时间间隔{1}，数据处理时间{2},组合号{3}", item.委托编号, (dtMid - dictOrderCancel[item.委托编号]).TotalSeconds, (dtNow - dtMid).TotalSeconds, this.名称);
                                    dictOrderCancel[item.委托编号] = DateTime.MinValue;
                                }
                            }
                        }
                    }
                }
                return timeCost.ToString();
            }

            public GroupClient.GroupService.GroupQueryData LocalData;

            bool isOperateResultChange = true;
            bool isTradeResultChange = true;
                 
            private void QueryDataLocal(ref double timeCost, ref DataTable 查到的成交Result, ref string 查到的成交ErrInfo, ref DataTable 查到的委托Result, ref string 查到的委托ErrInfo)
            {
                var dt = DateTime.Now;
                StringBuilder Result = new StringBuilder(1024 * 1024);
                StringBuilder ErrInfo = new StringBuilder(1024);
                if (LocalData == null)
                {
                    LocalData = new GroupClient.GroupService.GroupQueryData();
                }

                TdxApi.QueryData(this.ClientID, 3, Result, ErrInfo);
                if (LocalData.SearchTradeResult != Result.ToString())
                {
                    isTradeResultChange = true;
                    LocalData.SearchTradeResult = Result.ToString();
                    LocalData.SearchTradeErrInfo = ErrInfo.ToString();

                    查到的成交Result = Tool.ChangeDataStringToTable(LocalData.SearchTradeResult);
                    查到的成交ErrInfo = LocalData.SearchTradeErrInfo;
                }
                else
                {
                    isTradeResultChange = false;
                }

                Thread.Sleep(this.查询间隔时间 / 2);

                TdxApi.QueryData(this.ClientID, 2, Result, ErrInfo);
                if (LocalData.SearchOperatorResult != Result.ToString())
                {
                    isOperateResultChange = true;
                    LocalData.SearchOperatorResult = Result.ToString();
                    LocalData.SearchOperatorError = ErrInfo.ToString();

                    查到的委托Result = Tool.ChangeDataStringToTable(LocalData.SearchOperatorResult);
                    查到的委托ErrInfo = LocalData.SearchOperatorError;
                }
                else
                {
                    isOperateResultChange = false;
                }
                
                Thread.Sleep(this.查询间隔时间 / 2);
                
                LocalData.QueryTime = DateTime.Now - dt;
                timeCost = LocalData.QueryTime.TotalSeconds;
                
            }

            DateTime lastRefreshTime = DateTime.MinValue;
            private void QueryDataFromClient(ref double timeCost, ref DataTable 查到的成交Result, ref string 查到的成交ErrInfo, ref DataTable 查到的委托Result, ref string 查到的委托ErrInfo)
            {
                var client = CommonUtils.GetGroupClient(this.名称);
                //1.更新最新订单号
                if ((DateTime.Now - lastRefreshTime).TotalSeconds >= 30)
                {
                    var arrID = Program.db.已发委托.GetOrderIDList(DateTime.Today, this.名称);
                    if (arrID.Count > 0)
                    {
                        client.UpdateOrderIDList(CommonUtils.Mac, this.名称, arrID.ToArray());
                    }
                    lastRefreshTime = DateTime.Now;
                }
                
                if (this._isMultyThread == null)
                {
                    try
                    {
                        this._isMultyThread = client.IsGroupMultythread(this.名称);
                    }
                    catch
                    {
                        this._isMultyThread = false;
                    }
                }
                if (this._isMultyThread == true)
                {
                    #region 委托及成交更新 多线程查询及筛选数据组合号
                    try
                    {
                        //2.更新数据
                        bool needTiTle = string.IsNullOrEmpty(strWtTitle) || string.IsNullOrEmpty(strCjTitle);
                        var responseFilted = client.QueryDataFilted(CommonUtils.Mac, this.名称, needTiTle);
                        if (responseFilted != null)
                        {
                            if (needTiTle)
                            {
                                if (!string.IsNullOrEmpty(responseFilted.strTitleWT))
                                {
                                    strWtTitle = responseFilted.strTitleWT;
                                }
                                if (!string.IsNullOrEmpty(responseFilted.strTitleCJ))
                                {
                                    strCjTitle = responseFilted.strTitleCJ;
                                }
                            }
                            查到的委托Result = CommonUtils.GetDataTable(strWtTitle, responseFilted.lstWT);
                            查到的委托ErrInfo = string.IsNullOrWhiteSpace(responseFilted.ErrWT) ? string.Empty : responseFilted.ErrWT.Trim();

                            查到的成交Result = CommonUtils.GetDataTable(strCjTitle, responseFilted.lstCJ);
                            查到的成交ErrInfo = string.IsNullOrWhiteSpace(responseFilted.ErrCJ) ? string.Empty : responseFilted.ErrCJ.Trim();
                        }
                        else
                        {
                            查到的委托ErrInfo = "Error for QueryDataFilted, 委托 Result Null!";
                            查到的成交ErrInfo = "Error for QueryDataFilted, 成交 Result Null!";
                        }

                        timeCost = responseFilted.QueryTime.TotalSeconds;
                    }
                    catch (Exception ex)
                    {
                        查到的委托ErrInfo = this.名称 + "查询委托异常" + ex.Message;
                        查到的成交ErrInfo = this.名称 + "查询成交异常" + ex.Message;
                    }
                    #endregion
                }
                else
                {
                    //Program.logger.LogInfo("IsTestGroup {0} Value:{1}", this.名称, IsTestGroup);
                    #region 委托及成交更新 测试接口
                    ServiceReference.GroupServiceQueryFormatData response = null;
                    response = client.QueryDataAutoByMac(this.名称, CommonUtils.Mac);
                    if (response != null)
                    {
                        if (response.lstWT != null)
                        {
                            查到的委托Result = CommonUtils.ConvertListToDataTable(response.lstWT);
                        }
                        查到的委托ErrInfo = response.ErrWT;

                        if (response.lstCJ != null)
                        {
                            查到的成交Result = CommonUtils.ConvertListToDataTable(response.lstCJ);
                        }
                        查到的成交ErrInfo = response.ErrCJ;
                    }
                    else
                    {
                        查到的委托ErrInfo = "Error for QueryFormatData, 委托 Result Null!";
                        查到的成交ErrInfo = "Error for QueryFormatData, 成交 Result Null!";
                    }
                    timeCost = response.QueryTime.TotalSeconds;
                    #endregion
                }
            }

            #region CATS Interface
            CATSAdapter CatsAdapter = null;
            List<CATSEntity.StandardOrderEntity> CatsOrderList = new List<StandardOrderEntity>();
            private string CATSQueryData()
            {
                if (帐户委托DataTable.Count == 0)
                {
                    var lstOrderNo = Program.db.已发委托.Where(_ => _.组合号 == this.名称 && _.日期 == DateTime.Today).Select(_ => _.委托编号).ToList();
                    if (lstOrderNo.Count > 0)
                    {
                        CatsAdapter.GetRecordedOrder(this.交易帐号, lstOrderNo);
                    }
                }
                if (queueStandardOrder != null && queueStandardOrder.Count > 0)
                {
                    //如果缓存中存在有clientID无OrderID的项，将主动获取对应项进行更新
                    List<string> lstClientID = CommonUtils.OrderCacheQueue.Where(_ => _.GroupName == this.名称 && string.IsNullOrEmpty(_.OrderID) && !string.IsNullOrEmpty(_.ClientGUID)).Select(_ => _.ClientGUID).ToList();
                    if (lstClientID != null && lstClientID.Count() > 0)
                    {
                        this.CatsAdapter.GetOrdersByClientID(lstClientID);
                    }
                    List<CATSEntity.StandardOrderEntity> lstOrderChanged;
                    while (this.queueStandardOrder.TryDequeue(out lstOrderChanged))
                    {
                        this.RefreshCatsOrder(lstOrderChanged);
                    }
                    this.GetCatsDataTable(this.CatsOrderList, out this.帐户委托DataTable, out this.帐户成交DataTable);
                    Program.帐户成交DataTable[this.名称] = this.帐户成交DataTable.Copy() as JyDataSet.成交DataTable;
                    Program.帐户委托DataTable[this.名称] = this.帐户委托DataTable.Copy() as JyDataSet.委托DataTable;
                }
                
                return CatsAdapter.HeartBreakResult;
            }

            List<已发委托> dbWtList = null;
            private void GetCatsDataTable(List<StandardOrderEntity> CatsOrderList, out JyDataSet.委托DataTable dtWt, out JyDataSet.成交DataTable dtCj)
            {
                dtWt = new JyDataSet.委托DataTable();
                dtCj = new JyDataSet.成交DataTable();
                List<OrderCacheEntity> list = CommonUtils.OrderCacheQueue.Where(_ => _.GroupName == this.名称).ToList();
                if (dbWtList == null)
                {
                    using (AASDbContext aASDbContext = new AASDbContext())
                    {
                        this.dbWtList = (from _ in aASDbContext.已发委托 where _.日期 == System.DateTime.Today && _.组合号 == this.名称 select _).ToList<已发委托>();
                    }
                }
                foreach (var item in CatsOrderList)
                {
                    JyDataSet.委托Row 委托Row = dtWt.NewRow() as JyDataSet.委托Row;
                    OrderCacheEntity orderCacheEntity = list.FirstOrDefault(_ => _.ClientGUID == item.ClientID);
                    if (orderCacheEntity != null)
                    {
                        委托Row.市场代码 = orderCacheEntity.Market;
                        委托Row.买卖方向 = orderCacheEntity.Category;
                        委托Row.交易员 = orderCacheEntity.Trader;
                        委托Row.组合号 = orderCacheEntity.GroupName;
                        委托Row.证券名称 = orderCacheEntity.ZqName;
                    }
                    else
                    {
                        已发委托 已发委托 = this.dbWtList.FirstOrDefault(_ => _.委托编号 == item.OrderNo);
                        if (已发委托 == null)
                        {
                            Program.logger.LogInfo(" CATS Exception, DbDataSet.已发委托Row.GetCatsDataTable has no cache no db Exist item, values {0}", item.ToJson());
                            continue;
                        }
                        委托Row.市场代码 = 已发委托.市场代码;
                        委托Row.买卖方向 = 已发委托.买卖方向;
                        委托Row.交易员 = 已发委托.交易员;
                        委托Row.组合号 = 已发委托.组合号;
                        委托Row.证券名称 = 已发委托.证券名称;
                    }
                    CatsOrderRowFill(item, 委托Row);
                    dtWt.Add委托Row(委托Row);
                    CatsTradeFill(dtCj, 委托Row);
                }
            }

            private static void CatsOrderRowFill(StandardOrderEntity item, JyDataSet.委托Row newWt)
            {
                newWt.状态说明 = item.OrderStatus;
                newWt.证券代码 = item.StockCode.Trim();
                newWt.委托数量 = item.OrderQty;
                newWt.委托时间 = item.OrderTime.ToString("HH:mm:ss");
                newWt.委托价格 = System.Math.Round(item.OrderPrice, 5);
                newWt.委托编号 = item.OrderNo;
                newWt.撤单数量 = item.CancelQty;
                newWt.成交价格 = System.Math.Round(item.FilledPrice, 5);
                newWt.成交数量 = item.FilledQty;
            }
            private static void CatsTradeFill(JyDataSet.成交DataTable dtCj, JyDataSet.委托Row newWt)
            {
                if (newWt.成交数量 > 0m)
                {
                    JyDataSet.成交Row 成交Row = dtCj.NewRow() as JyDataSet.成交Row;
                    成交Row.组合号 = newWt.组合号;
                    成交Row.证券代码 = newWt.证券代码;
                    成交Row.证券名称 = newWt.证券名称;
                    成交Row.委托编号 = newWt.委托编号;
                    成交Row.买卖方向 = newWt.买卖方向;
                    成交Row.市场代码 = newWt.市场代码;
                    成交Row.成交编号 = dtCj.Rows.Count.ToString();
                    成交Row.成交时间 = newWt.委托时间;
                    成交Row.成交价格 = newWt.成交价格;
                    成交Row.成交数量 = newWt.成交数量;
                    成交Row.成交金额 = newWt.成交价格 * newWt.成交数量;
                    成交Row.交易员 = newWt.交易员;
                    dtCj.Add成交Row(成交Row);
                }
            }

            List<string> _lstOrderID = null;
            private void AddOrder(StandardOrderEntity item, OrderCacheEntity cacheItem)
            {
                if (this._lstOrderID == null)
                {
                    using (AASDbContext db = new AASDbContext())
                    {
                        this._lstOrderID = (from _ in db.已发委托 where _.日期 == System.DateTime.Today && _.组合号 == this.名称 select _.委托编号).ToList<string>();
                    }
                }
                if (!this._lstOrderID.Contains(item.OrderNo))
                {
                    Program.db.已发委托.Add(System.DateTime.Today, this.名称, item.OrderNo, cacheItem.Trader, "委托成功", cacheItem.Market, cacheItem.Zqdm.Trim(), cacheItem.ZqName, cacheItem.Category, 0m, 0m, cacheItem.Price, cacheItem.Quantity, 0m);
                    string 信息 = (cacheItem.Sender == cacheItem.Trader) ? "下单成功" : string.Format("风控员{0}下单成功", cacheItem.Trader);
                    Program.db.交易日志.Add(System.DateTime.Today, System.DateTime.Now.ToString("HH:mm:ss"), cacheItem.Trader, this.名称, cacheItem.Zqdm.Trim(), cacheItem.ZqName, cacheItem.OrderID, cacheItem.Category, cacheItem.Quantity, cacheItem.Price, 信息);
                    cacheItem.OrderID = item.OrderNo;
                    this._lstOrderID.Add(item.OrderNo);
                }
            }


            ConcurrentQueue<List<StandardOrderEntity>> queueStandardOrder;
            private void CacheNewestList(List<CATSEntity.StandardOrderEntity> obj)
            {
                if (queueStandardOrder ==null)
                {
                    queueStandardOrder = new ConcurrentQueue<List<CATSEntity.StandardOrderEntity>>();
                }
                queueStandardOrder.Enqueue(obj);
            }

            private void RefreshCatsOrder(List<CATSEntity.StandardOrderEntity> lstOrderChanged)
            {
                if (lstOrderChanged != null && lstOrderChanged.Count > 0)
                {
                    if (this.CatsOrderList.Exists((StandardOrderEntity _) => _.OrderTime < DateTime.Today))
                    {
                        this.CatsOrderList = CatsOrderList.Where(_=> _.OrderTime >= DateTime.Today).ToList();
                    }

                    lstOrderChanged = lstOrderChanged.Where(_=> _.Account.Trim() == this.交易帐号).ToList();

                    foreach (var item in lstOrderChanged)
                    {
                        try
                        {
                            StandardOrderEntity existItem = this.CatsOrderList.FirstOrDefault((StandardOrderEntity _) => _.OrderNo == item.OrderNo || _.ClientID == item.ClientID);
                            if (existItem == null)
                            {
                                this.CatsOrderList.Add(item);
                                var orderCacheEntity = CommonUtils.OrderCacheQueue.FirstOrDefault((OrderCacheEntity _) => _.GroupName == this.名称 && _.ClientGUID == item.ClientID);
                                if (orderCacheEntity != null)
                                {
                                    this.AddOrder(item, orderCacheEntity);
                                }
                            }
                            else
                            {
                                StandardOrderEntity standardOrderEntity2 = this.CatsOrderList.First((StandardOrderEntity _) => _.OrderNo == item.OrderNo);
                                standardOrderEntity2.CancelQty = item.CancelQty;
                                standardOrderEntity2.ErrMsg = item.ErrMsg;
                                standardOrderEntity2.FilledPrice = item.FilledPrice;
                                standardOrderEntity2.FilledQty = item.FilledQty;
                                standardOrderEntity2.OrderStatus = item.OrderStatus;
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.logger.LogInfoDetail("RefreshCatsOrder Exception, Message {1}", ex.Message);
                        }
                    }
                    using (List<StandardOrderEntity>.Enumerator enumerator = lstOrderChanged.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            StandardOrderEntity item = enumerator.Current;
                            
                        }
                    }
                }
            }
            #endregion

            JyDataSet.成交DataTable Get规范成交(DataTable 查到的成交DataTable)
            {
                if (IsImsAccount)
                {
                    JyDataSet.成交DataTable Ims成交DataTable = GetImsCjTable(查到的成交DataTable);
                    return Ims成交DataTable;
                }


                bool hasCancelSymble = 查到的成交DataTable.Columns.Contains("撤单标志");
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
                        if (hasCancelSymble && 查到的成交DataTable.Rows[i]["撤单标志"] as string == "1")
                        {
                            //查到的成交DataTable.Rows.RemoveAt(i);
                        }
                        else if (hasSuccessPrice && decimal.Parse(查到的成交DataTable.Rows[i]["成交价格"] as string) == 0)
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
                            AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.table券商帐户.DbDataSet.已发委托.Get已发委托(DateTime.Today, this.名称, 委托编号);
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
                                if ((this.券商 == "模拟测试" && this.类型 == "普通") ||
                                    (this.券商 == "招商证券" && this.类型 == "信用") ||
                                    (this.券商 == "招商证券" && this.类型 == "普通") ||
                                    (this.券商 == "银河证券" && this.类型 == "信用") ||
                                    (this.券商 == "广发证券" && this.类型 == "普通") ||
                                    (this.券商 == "广发证券" && this.类型 == "信用") ||
                                    (this.券商 == "光大证券" && this.类型 == "信用") ||
                                    (this.券商 == "光大证券" && this.类型 == "普通") ||
                                    (this.券商 == "国海证券" && this.类型 == "普通") ||
                                    (this.券商 == "东方证券" && this.类型 == "普通") ||
                                    (this.券商 == "东方证券" && this.类型 == "信用") ||
                                    (this.券商 == "民族证券" && this.类型 == "普通") ||
                                    (this.券商 == "中信证券" && this.类型 == "信用") ||
                                    (this.券商 == "中信证券" && this.类型 == "普通") ||
                                    (this.券商 == "兴业证券" && this.类型 == "普通") ||
                                    (this.券商 == "国信证券" && this.类型 == "普通") ||
                                    (this.券商 == "国信证券" && this.类型 == "信用") ||
                                    (this.券商 == "银河证券" && this.类型 == "普通") ||
                                    (this.券商 == "国泰君安" && this.类型 == "信用") ||
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
                                else if (this.券商 == "Ims")
                                {
                                    成交Row1.成交时间 = CommonUtils.GetImsDateTimeString(DataRow0["knockTime"] as string);
                                    成交Row1.成交价格 = decimal.Parse(DataRow0["knockPrice"] as string);
                                    成交Row1.成交数量 = decimal.Parse(DataRow0["knockQty"] as string);
                                    成交Row1.成交金额 = decimal.Parse(DataRow0["knockAmt"] as string);
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

            private JyDataSet.成交DataTable GetImsCjTable(DataTable 查到的成交DataTable)
            {
                JyDataSet.成交DataTable 规范的成交DataTable = new JyDataSet.成交DataTable();
                try
                {
                    for (int i = 查到的成交DataTable.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow DataRow0 = 查到的成交DataTable.Rows[i];
                        string 委托编号 = this.GetDataRow委托编号(DataRow0);
                        AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.table券商帐户.DbDataSet.已发委托.Get已发委托(DateTime.Today, this.名称, 委托编号);
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

                            成交Row1.成交时间 = CommonUtils.GetImsDateTimeString(DataRow0["knockTime"] as string);
                            成交Row1.成交价格 = decimal.Parse(DataRow0["knockPrice"] as string);
                            成交Row1.成交数量 = decimal.Parse(DataRow0["knockQty"] as string);
                            成交Row1.成交金额 = decimal.Parse(DataRow0["knockAmt"] as string);


                            规范的成交DataTable.Add成交Row(成交Row1);
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
                var needRepairList = CommonUtils.OrderCacheQueue.Where(_ => _.GroupName == this.名称 &&  _.IsTimeOutError == "1" && (DateTime.Now - _.SendTime).TotalSeconds < 30 &&  string.IsNullOrEmpty(_.OrderID)).ToList();

                foreach (DataRow DataRow0 in 查到的委托DataTable.Rows)
                {
                    string 委托编号 = this.GetDataRow委托编号(DataRow0);
                    if (规范的委托DataTable.Any(r => r.委托编号 == 委托编号))
                    {
                        ReCalculateTradeNumber(规范的委托DataTable, 委托编号);
                        ReCalculateTradePrice(规范的委托DataTable, 委托编号);
                        continue;
                    }



                    AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.table券商帐户.DbDataSet.已发委托.Get已发委托(DateTime.Today, this.名称, 委托编号);
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
                            &&  _.Quantity == decimal.Parse( DataRow0["委托数量"] + "") 
                            && _.Price == decimal.Parse(DataRow0["委托价格"] + "") 
                            && CommonUtils.IsTdxBuy(_.Category) == CommonUtils.IsTdxBuy(int.Parse(DataRow0["买卖标志"] + "")));
                        if (needAddOrd != null)
                        {
                            //加入委托表和交易日志表。
                            Program.db.已发委托.Add(DateTime.Today, this.名称, 委托编号, needAddOrd.Trader, "委托成功", needAddOrd.Market, needAddOrd.Zqdm, needAddOrd.ZqName,needAddOrd.Category, 0m, 0m, needAddOrd.Price, needAddOrd.Quantity, 0m);
                            string Msg =needAddOrd.IsRiskControl ? string.Format("风控员{0}下单成功", needAddOrd.Sender) : "下单成功";
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
                    case "Ims":
                        var errorInfo = DataRow0["errorMsg"] as string;
                        委托Row1.委托时间 = CommonUtils.GetImsDateTimeString(DataRow0["orderTime"] as string);
                        委托Row1.成交数量 = decimal.Parse(DataRow0["knockQty"] as string);
                        委托Row1.状态说明 = DataRow0["orderStatus"] as string;

                        if (string.IsNullOrEmpty(errorInfo))
                        {
                            委托Row1.成交价格 = decimal.Parse(DataRow0["averagePrice"] as string);
                        }
                        else
                        {
                            委托Row1.成交价格 = 0;
                            委托Row1.状态说明 += " ErrorInfo:" + errorInfo;
                            委托Row1.撤单数量 = 委托Row1.委托数量;
                        }
                        string cancelInfo = DataRow0["withdrawQty"] as string;
                        decimal cancelNum = 0;
                        decimal.TryParse(cancelInfo, out cancelNum);
                        委托Row1.撤单数量 = cancelNum;
                        break;
                    default:
                        throw new Exception(string.Format("不支持 {0}{1} 帐户", this.券商, this.类型));
                }
            }

            void Get成交Info(string 委托编号, out decimal 成交数量, out decimal 成交价格)
            {
                if (this.帐户成交DataTable.Any(r => r.委托编号 == 委托编号))
                {
                    // r.成交数量 > 0 && r.成交金额 == 0
                    var tradeRelated = this.帐户成交DataTable.Where(r => r.委托编号 == 委托编号);
                    decimal 成交金额 = 0;
                    成交数量 = 0;
                    成交价格 = 0;
                    foreach (var item in tradeRelated)
                    {
                        if (item.成交金额 == 0 && item.成交价格 > 0 && item.成交数量 > 0)
                            item.成交金额 = item.成交价格 * item.成交数量;
                        成交金额 += item.成交金额;
                        成交数量 += item.成交数量;
                    }
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
                Result = string.Empty;
                ErrInfo = string.Empty;

                if (this.ClientID == -1)
                {
                    ErrInfo = string.Format("{0}未登录到券商交易服务器", this.名称);
                    hasOrderNo = false;
                    return;
                }

                orderCacheObj.GroupName = this.名称;
                orderCacheObj.Market = Market;
                hasOrderNo = true;//只有CATS接口会用到这个参数，用于区别普通A股下单。因为此接口返回的是客户端自己生成的id，在一段事件后才会有接口生成的id。
                if (!Tool.IsSendOrderTimeFit())
                {
                    ErrInfo = string.Format("下单时限为9:00-15:00, 当前时间{0}超出下单时限", DateTime.Now);
                    return;
                }

                if (this.IsCATSAccount)
                {
                    SendCatsOrder(Category, Market, Zqdm, Price, Quantity, orderCacheObj, out Result, out ErrInfo, out hasOrderNo);
                    return;
                }
                if (IsImsAccount)
                {
                    SendImsOrder(Category, Market, Zqdm, Price, Quantity, out Result, out ErrInfo, out hasOrderNo);
                    return;
                }

                string 股东代码 = (Market == 0 ? this.深圳股东代码 : this.上海股东代码);
                if (股东代码 == string.Empty)
                {
                    ErrInfo = "未取到股东代码";
                    return;
                }

                if (!CommonUtils.ExistsGroup(this.名称))
                {
                    this.SendOrderLocal(Category, 股东代码, Zqdm, Price, Quantity, out Result, out ErrInfo);
                }
                else
                {
                    SendOrderClient(Category, 股东代码, Zqdm, Price, Quantity, ref Result, ref ErrInfo);
                }

                if (ErrInfo == string.Empty)
                {
                    Result = GetOrderID(Result);
                    OrderIDs.Enqueue(Result);
                    dictOrderSend[Result] = DateTime.Now;
                }
                else
                {
                    Result = string.Empty;
                    Program.logger.LogInfoDetail("下单错误 组合号{0}, 交易员{1}, 证券代码{2}, 错误信息 {3}", this.名称, orderCacheObj.Trader, Zqdm, ErrInfo);
                }
            }

            private void SendOrderClient(int Category, string Gddm, string Zqdm, decimal Price, decimal Quantity, ref string Result, ref string ErrInfo)
            {
                var client = CommonUtils.GetGroupClient(this.名称);
                var 下单response = client.SendOrder(this.名称, Category, 0, Gddm, Zqdm, (float)Price, (int)Quantity, CommonUtils.Mac);
                client.Close();
                ErrInfo = 下单response.Error;
                Result = 下单response.Result;
            }

            private string GetOrderID(string result)
            {
                if (result.Contains('\n'))
                {
                    DataTable DataTable1 = Tool.ChangeDataStringToTable(result);
                    if (this.券商 == "银河证券" && this.类型 == "信用")
                    {
                        return DataTable1.Rows[0]["合同编号"] as string;
                    }
                    else
                    {
                        return DataTable1.Rows[0]["委托编号"] as string;
                    }

                }
                else
                {
                    return result;
                }
            }

            public void SendOrderLocal(int Category, string Gddm, string Zqdm, decimal Price, decimal Quantity, out string Result, out string ErrInfo)
            {
                lock (this.SendOrderObject)
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

                    TdxApi.SendOrder(this.ClientID, Category, 0, Gddm, Zqdm, (float)Price, (int)Quantity, Result1, ErrInfo1);
                    ErrInfo = ErrInfo1.ToString();
                    if (ErrInfo == string.Empty)
                    {
                        Result = Result1.ToString();
                    }
                }
            }


            void SendCatsOrder(int Category, byte Market, string Zqdm, decimal Price, decimal Quantity, OrderCacheEntity orderCacheObj, out string Result, out string ErrInfo, out bool hasOrderNo)
            {
                var strSendingResult = CatsAdapter.SendOrder(this.交易帐号, Market, Zqdm, Price, Quantity, Category.ToString());
                if (Regex.IsMatch(strSendingResult, "^\\d+$"))
                {
                    //orderCacheObj.ClientGUID = strSendingResult;
                    //orderCacheObj.OrderID = orderCacheObj.ClientGUID;
                    Result = orderCacheObj.ClientGUID;
                    ErrInfo = string.Empty;
                    hasOrderNo = false;
                    //Result = string.Empty;
                    //ErrInfo = "已报，等待交易服务器响应";
                }
                else if (Regex.IsMatch(strSendingResult, "^[a-zA-Z0-9_]+$"))
                {
                    orderCacheObj.OrderID = strSendingResult;
                    Result = strSendingResult;
                    ErrInfo = string.Empty;
                    hasOrderNo = true;
                }
                else
                {
                    hasOrderNo = false;
                    Result = string.Empty;
                    ErrInfo = "下单失败：" + strSendingResult;
                }
            }

            void SendImsOrder(int Category, byte Market, string Zqdm, decimal Price, decimal Quantity, out string Result, out string ErrInfo, out bool hasOrderNo)
            {
                lock (this.SendOrderObject)
                {
                    Result = string.Empty;
                    ErrInfo = string.Empty;
                    hasOrderNo = true;

                    //我们数据库存的market数据为0:深市，1上海
                    var isSH = Market == 1 ? "0" : "1";
                    var bsFlag = Category % 2 == 0 ? "B" : "S";
                    var client = CommonUtils.GetGroupClient(this.名称);

                    var 下单response = client.SendIMSOrder(this.名称, bsFlag, isSH, Zqdm, (float)Price, (int)Quantity);
                    Result = 下单response.Result;
                    ErrInfo = 下单response.Error;
                }
                


            }

            public void CancelOrder(string Zqdm, byte Market, string hth, out string Result, out string ErrInfo)
            {
                if (this.IsCATSAccount)
                {
                    CancelCatsOrder(Market, hth, out Result, out ErrInfo);
                    return;
                }
                if (IsImsAccount)
                {
                    CancelImsOrder(Zqdm, Market, hth, out Result, out ErrInfo);
                    return;
                }

                if (this.ClientID == -1)
                {
                    Result = "";
                    ErrInfo = string.Format("{0}未登录到券商交易服务器", this.名称);
                    return;
                }

                string ExchangeID = null;
                if (this.券商 == "招商证券" && this.类型 == "普通")
                {
                    ExchangeID = (Market == 1 ? "1" : "2");
                }
                else
                {
                    ExchangeID = Market.ToString();
                }

                if (!CommonUtils.ExistsGroup(this.名称))
                {
                    this.CancelOrderLocal(hth, ExchangeID, out Result, out ErrInfo);
                    return;
                }

                StringBuilder Result1 = new StringBuilder(1024);
                StringBuilder ErrInfo1 = new StringBuilder(256);

                var client = CommonUtils.GetGroupClient(this.名称);
                var result = client.CancelOrder(this.名称, ExchangeID.ToString(), hth);
                Result1.Append(result.Result);
                ErrInfo1.Append(result.Error);
                client.Close();

                ErrInfo = ErrInfo1.ToString();
                if (ErrInfo == string.Empty)
                {
                    Result = "撤单成功";
                    dictOrderCancel[hth] = DateTime.Now;
                }
                else
                {
                    Result = string.Empty;
                }
            }

            public void CancelOrderLocal(string hth, string ExchangeID, out string Result, out string ErrInfo)
            {
                if (this.ClientID == -1)
                {
                    Result = "";
                    ErrInfo = string.Format("{0}未登录到券商交易服务器", this.名称);
                    return;
                }
                StringBuilder Result1 = new StringBuilder(1024);
                StringBuilder ErrInfo1 = new StringBuilder(256);
                TdxApi.CancelOrder(ClientID, ExchangeID.ToString(), hth, Result1, ErrInfo1);


                ErrInfo = ErrInfo1.ToString();
                if (ErrInfo == string.Empty)
                {
                    Result = "撤单成功";
                }
                else
                {
                    Result = string.Empty;
                }
            }

            public void CancelCatsOrder(byte Market, string hth, out string Result, out string ErrInfo)
            {
                if (CatsAdapter == null)
                {
                    ErrInfo = "未连接";
                    Result = string.Empty;
                }
                var result = CatsAdapter.CancelOrder(this.交易帐号, hth);
                if (Regex.IsMatch(result, "^\\d+$"))
                {
                    Result = "撤单成功";
                    ErrInfo = string.Empty;
                }
                else
                {
                    Result = string.Empty;
                    ErrInfo =  result;
                }
            }

            public void CancelImsOrder(string Zqdm, byte Market, string hth, out string Result, out string ErrInfo)
            {

                Result = string.Empty;
                ErrInfo = string.Empty;

                if (!CommonUtils.ExistsGroup(this.名称))
                {
                    ErrInfo = string.Format("{0}未配置发送端！", this.名称);
                    return;
                }

                if (this.ClientID == -1)
                {
                    ErrInfo = string.Format("{0}未登录到券商交易服务器", this.名称);
                    return;
                }

                StringBuilder Result1 = new StringBuilder(1024 * 1024);
                StringBuilder ErrInfo1 = new StringBuilder(256);

                var market = Market == 0 ? "1" : "0";

                var client = CommonUtils.GetGroupClient(this.名称);
                var result = client.CancelImsOrder(this.名称, market, Zqdm, hth);
                Result1.Append(result.Result);
                ErrInfo1.Append(result.Error);
                client.Close();

                ErrInfo = ErrInfo1.ToString();
                if (ErrInfo == string.Empty)
                {
                    Result = "撤单成功";
                    dictOrderCancel[hth] = DateTime.Now;
                }
                else
                {
                    Result = string.Empty;
                }
            }

            public void QueryPubStocks(out string Result, out string ErrInfo)
            {
                Result = PubStockResult;
                ErrInfo = PubStockErrInfo;
                return;
            }

            bool? _isMultyThread = null;

            public void Repay(decimal amount, StringBuilder Result, StringBuilder ErrInfo)
            {
                if (!CommonUtils.ExistsGroup(this.名称))
                {
                    if (this.ClientID > -1)
                    {
                        TdxApi.Repay(this.ClientID, amount.ToString(), Result, ErrInfo);
                    }
                    else
                    {
                        ErrInfo.AppendFormat("{0}: ClientID为-1", this.名称);
                    }
                }
                else
                {
                    var client = CommonUtils.GetGroupClient(this.名称);
                    var strRepayResult = client.AccountRepay(this.名称, amount);
                    Result.Append(strRepayResult);
                }
            }

            public void QueryPosition(int dataType, StringBuilder result, StringBuilder error)
            {
                if (CommonUtils.ExistsGroup(this.名称))
                {
                    var client = CommonUtils.GetGroupClient(this.名称);
                    var resultQuery = client.QueryPosition(this.名称);
                    result.Append(resultQuery.Result);
                    error.Append(resultQuery.Error);
                }
                else
                {
                    if (this.ClientID > -1)
                    {
                        TdxApi.QueryData(this.ClientID, 2, result, error);
                    }
                    else
                    {
                        error.Append("ClientID 为 -1 无法执行查询命令!");
                    }
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
                            Program.订单表Changed[e.Row.交易员] = true;
                            db.SaveChanges();
                            break;
                        case DataRowAction.Delete:
                            订单1 = db.订单.Find(e.Row.交易员, e.Row.组合号, e.Row.证券代码);
                            db.订单.Remove(订单1);
                            Program.订单表Changed[e.Row.交易员] = true;

                            db.SaveChanges();
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
                            Program.订单表Changed[e.Row.交易员] = true;
                            db.SaveChanges();
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
                    AASServer.DbDataSet.订单Row 订单1 = this.FirstOrDefault(r => r.交易员 == 交易员 && r.组合号 == 组合号 && r.证券代码 == 证券代码);
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
                    AASServer.DbDataSet.订单Row 订单1 = this.First(r => r.交易员 == 成交Row1.交易员 && r.组合号 == 成交Row1.组合号 && r.证券代码 == 成交Row1.证券代码);

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



            //public void Deal订单()
            //{
            //    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
            //    {
            //        for (int i = this.Count - 1; i >= 0; i--)
            //        {

            //            this[i].Deal();


            //            if (this[i].已开数量 == this[i].已平数量 && this[i].已开数量 != 0)
            //            {
            //                this.Remove订单Row(this[i]);
            //            }
            //        }
            //    }
            //}


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
                                AASServer.DbDataSet.已平仓订单Row 已平仓订单Row1 = this.New已平仓订单Row();
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
                    AASServer.DbDataSet.已平仓订单DataTable 已平仓订单DataTable1 = new DbDataSet.已平仓订单DataTable();
                    foreach (AASServer.DbDataSet.已平仓订单Row 已平仓订单Row1 in this.Where(r => r.交易员 == UserName))
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
                    AASServer.DbDataSet.已处理成交Row 已处理成交1 = this.New已处理成交Row();
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

            public void LoadToday()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    this.PrimaryKey = new DataColumn[] { this.Columns["日期"], this.Columns["组合号"], this.Columns["委托编号"] };


                    using (AASDbContext db = new AASDbContext())
                    {
                        var today = DateTime.Today;
                        foreach (已发委托 已发委托1 in db.已发委托.Where(r => r.日期 >= today))
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

                }
            }






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
                    arrID = this.Where(r => r.日期 == DateTime.Today && r.组合号 == 组合号).Select(_ => _.委托编号).ToList();
                }
                return arrID;
            }

            public JyDataSet.委托DataTable Get风控平仓委托DataTable()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    JyDataSet.委托DataTable 委托DataTable1 = new JyDataSet.委托DataTable();

                    foreach (AASServer.DbDataSet.已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.状态说明 == "风控平仓"))//已发委托 已发委托1 in db.已发委托.Where(r => r.交易员 == UserName && r.日期 == DateTime.Today  && r.证券代码 == 证券代码))
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
                    AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.FirstOrDefault(r => r.日期 == DateTime.Today && r.组合号 == 组合号 && r.委托编号 == 委托编号);
                    if (已发委托Row1 == null)
                    {
                        return null;
                    }

                    已发委托DataTable 已发委托DataTable1 = new 已发委托DataTable();
                    已发委托DataTable1.ImportRow(已发委托Row1);
                    return 已发委托DataTable1[0];
                }
            }
            public void Add(DateTime 日期, string 组合号, string 委托编号, string 交易员, string 状态说明, byte 市场代码, string 证券代码, string 证券名称, int 买卖方向, decimal 成交价格, decimal 成交数量, decimal 委托价格, decimal 委托数量, decimal 撤单数量)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.New已发委托Row();
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
                    AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.First(r => r.日期 == DateTime.Today && r.组合号 == 组合号 && r.委托编号 == 委托编号);
                    if (委托编号.Length == 36)
                        已发委托Row1.状态说明 = 状态说明;
                    else
                        已发委托Row1.状态说明 = "废单";
                }
            }

            public void Update(DateTime 日期, string 组合号, string 委托编号, decimal 成交价格, decimal 成交数量)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.First(r => r.日期 == DateTime.Today && r.组合号 == 组合号 && r.委托编号 == 委托编号);

                    已发委托Row1.成交价格 = 成交价格;
                    已发委托Row1.成交数量 = 成交数量;
                }
            }

            public void Update(DateTime 日期, string 组合号, string 委托编号, decimal 撤单数量)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.已发委托Row 已发委托Row1 = this.First(r => r.日期 == DateTime.Today && r.组合号 == 组合号 && r.委托编号 == 委托编号);

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
                    var limits = Program.db.额度分配.Where(_=> _.交易员 == user.用户名);
                    decimal 交易费用 = 0;
                    foreach (已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.交易员 == user.用户名))
                    {
                        var limit = limits.FirstOrDefault(_ => _.交易员 == user.用户名 && _.组合号 == 已发委托Row1.组合号 && _.证券代码 == 已发委托Row1.证券代码);
                        交易费用 += 已发委托Row1.Get交易费用(limit == null ? user.手续费率: limit.手续费率);
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

                    foreach (AASServer.DbDataSet.已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.交易员 == JyUserName && r.证券代码 == 证券代码))//已发委托 已发委托1 in db.已发委托.Where(r => r.交易员 == UserName && r.日期 == DateTime.Today  && r.证券代码 == 证券代码))
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

                    foreach (AASServer.DbDataSet.已发委托Row 已发委托Row1 in this.Where(r => r.日期 == DateTime.Today && r.交易员 == JyUserName))
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

                    if (this.组合号 == AyersMessageAdapter.GroupName)
                    {
                        var config = AyersConfig.GetFeeConfig();
                        decimal 佣金 = Math.Max(config.Commission * 成交金额, config.CommissionMin);
                        decimal 印花税 = Math.Ceiling(config.StampTax * 成交金额);
                        decimal 过户费 = config.TransferFee * 成交金额;
                        decimal levy = config.TransactionLevy * 成交金额; //交易征费
                        decimal trading_fee = config.TradingFee * 成交金额;// 交易费

                        return Math.Round(佣金 + 印花税 + 过户费 + levy + trading_fee, 2);
                    }
                    else
                    {
                        decimal 佣金 = Math.Max(5, Math.Round(成交金额 * 手续费率, 2, MidpointRounding.AwayFromZero));
                        decimal 印花税 = this.买卖方向 == 0 ? 0 : Math.Round(成交金额 * 0.001m, 2, MidpointRounding.AwayFromZero);
                        decimal 过户费 = this.市场代码 == 1 ? Math.Round(this.成交数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;
                        return 佣金 + 印花税 + 过户费;
                    }

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
                    AASServer.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();

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

            public AASServer.DbDataSet.额度分配DataTable QueryTradeLimit()
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
                    AASServer.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();

                    if (平台用户Row.分组 == (int)分组.ALL)
                    {
                        foreach (AASServer.DbDataSet.额度分配Row 额度分配Row1 in this)
                        {
                            额度分配DataTable1.ImportRow(额度分配Row1);

                        }
                    }
                    else
                    {
                        AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new 平台用户DataTable();
                        平台用户DataTable1.LoadToday();
                        foreach (AASServer.DbDataSet.额度分配Row 额度分配Row1 in this)
                        {
                            foreach (AASServer.DbDataSet.平台用户Row 交易员Row in 平台用户DataTable1.Where(r => r.用户名 == 额度分配Row1.交易员)) 
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

            public AASServer.DbDataSet.额度分配DataTable QueryTradeLimitBelongJy(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();

                    foreach (AASServer.DbDataSet.额度分配Row 额度分配Row1 in this.Where(r => r.交易员 == UserName))
                    {
                        额度分配DataTable1.ImportRow(额度分配Row1);

                    }

                    return 额度分配DataTable1;
                }

            }

            #region Limit Manager Functions
            public AASServer.DbDataSet.额度分配DataTable LimitManagerServiceQeury(string guid)
            {
                if (CommonUtils.LimitServiceID == guid)
                {
                    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                    {
                        AASServer.DbDataSet.额度分配DataTable 额度分配DataTable1 = new DbDataSet.额度分配DataTable();

                        foreach (AASServer.DbDataSet.额度分配Row 额度分配Row1 in this)
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
                int noTraderCount = 0;
                int updateCount = 0;
                int addCount = 0;

                msg = string.Empty;
                if (CommonUtils.LimitServiceID == guid)
                {
                    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                    {
                        foreach (var limitRow in dt)
                        {
                            try
                            {
                                var existRow = this.FirstOrDefault(_ => _.交易员 == limitRow.交易员 && _.证券代码 == limitRow.证券代码);
                                if (existRow != null)
                                {
                                    #region Update
                                    existRow.组合号 = limitRow.组合号;
                                    existRow.市场 = limitRow.市场;
                                    existRow.证券名称 = limitRow.证券名称;
                                    existRow.拼音缩写 = limitRow.拼音缩写;
                                    existRow.买模式 = limitRow.买模式;
                                    existRow.卖模式 = limitRow.卖模式;
                                    existRow.交易额度 = limitRow.交易额度;
                                    existRow.手续费率 = limitRow.手续费率;
                                    updateCount++;
                                    #endregion
                                }
                                else if (Program.db.平台用户.ExistsUserRole(limitRow.交易员, 角色.交易员))
                                {
                                    #region Add
                                    AASServer.DbDataSet.额度分配Row TradeLimit1 = this.New额度分配Row();
                                    TradeLimit1.交易员 = limitRow.交易员;
                                    TradeLimit1.组合号 = limitRow.组合号;
                                    TradeLimit1.证券代码 = limitRow.证券代码;
                                    TradeLimit1.市场 = limitRow.市场;
                                    TradeLimit1.证券名称 = limitRow.证券名称;
                                    TradeLimit1.拼音缩写 = limitRow.拼音缩写;
                                    TradeLimit1.买模式 = limitRow.买模式;
                                    TradeLimit1.卖模式 = limitRow.卖模式;
                                    TradeLimit1.交易额度 = limitRow.交易额度;
                                    TradeLimit1.手续费率 = limitRow.手续费率;
                                    this.Add额度分配Row(TradeLimit1);
                                    addCount++;
                                    #endregion
                                }
                                else
                                {
                                    noTraderCount++;
                                }
                            }
                            catch (Exception ex)
                            {
                                excetptionCount++;
                                Program.logger.LogInfoDetail("额度分配DataTable.LimitManagerserviceImport Exception {0}", ex.Message);
                            }
                        }
                    }
                    msg = string.Format("新增额度{0}条，修改额度{1}条，无匹配交易员数据{2}条，执行异常{3}条", addCount, updateCount, noTraderCount, excetptionCount);
                    if (excetptionCount > 0)
                    {
                        msg += "，异常数据多于0条，请联系管理员！";
                    }
                    return updateCount + addCount;
                }
                else
                {
                    msg = "LimitManagerservice 访问接口验证异常，请联系管理员！";
                }
                return 0;
            }
            #endregion

            public void AddTradeLimit(string 交易员, string 证券代码, string 组合号, byte 市场代码, string 证券名称, string 拼音缩写, 买模式 买模式1, 卖模式 卖模式1, decimal 交易额度, decimal 手续费率)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.额度分配Row TradeLimit1 = this.New额度分配Row();
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
                    AASServer.DbDataSet.额度分配Row TradeLimit1 = this.First(r => r.交易员 == 交易员 && r.证券代码 == 证券代码);

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
                    AASServer.DbDataSet.额度分配Row TradeLimit1 = this.First(r => r.交易员 == 交易员 && r.证券代码 == 证券代码);


                    this.Remove额度分配Row(TradeLimit1);
                }



            }

            public void DeleteAllTradeLimitByUser(string 交易员)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    for (int i = this.Rows.Count - 1; i >= 0; i--)
                    {
                        AASServer.DbDataSet.额度分配Row TradeLimit1 = this.Rows[i] as AASServer.DbDataSet.额度分配Row;
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
                    if (this.买模式 == (int)AASServer.买模式.现金买入)
                    {
                        return 0;
                    }
                    else if (this.买模式 == (int)AASServer.买模式.融资买入)
                    {
                        return 2;
                    }
                    else if (this.买模式 == (int)AASServer.买模式.担保品买入)
                    {
                        return 7;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (this.卖模式 == (int)AASServer.卖模式.现券卖出)
                    {
                        return 1;
                    }
                    else if (this.卖模式 == (int)AASServer.卖模式.融券卖出)
                    {
                        return 3;
                    }
                    else if (this.卖模式 == (int)AASServer.卖模式.担保品卖出)
                    {
                        return 8;
                    }
                    else
                    {
                        return 5;
                    }
                }
            }


            public string Get恒生帐户买卖类别(int 买卖方向)
            {
                if (买卖方向 == 0)
                {
                    if (this.买模式 == (int)AASServer.买模式.现金买入)
                    {
                        return "1";
                    }
                    else if (this.买模式 == (int)AASServer.买模式.融资买入)
                    {
                        return "75";
                    }
                    else if (this.买模式 == (int)AASServer.买模式.担保品买入)
                    {
                        return null;
                    }
                    else
                    {
                        return "68";
                    }
                }
                else
                {
                    if (this.卖模式 == (int)AASServer.卖模式.现券卖出)
                    {
                        return "2";
                    }
                    else if (this.卖模式 == (int)AASServer.卖模式.融券卖出)
                    {
                        return "67";
                    }
                    else if (this.卖模式 == (int)AASServer.卖模式.担保品卖出)
                    {
                        return null;
                    }
                    else
                    {
                        return "76";
                    }
                }
            }



            public string 恒生帐户市场类别
            {
                get
                {
                    if (this.市场 == 0)
                    {
                        return "2";
                    }
                    else
                    {
                        return "1";
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


            public AASServer.DbDataSet.风控分配DataTable Query风控分配BelongFK(string FKUserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.风控分配DataTable 风控分配DataTable1 = new AASServer.DbDataSet.风控分配DataTable();
                    foreach (AASServer.DbDataSet.风控分配Row 风控分配Row1 in this)
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
                    AASServer.DbDataSet.风控分配Row 风控分配Row1 = this.New风控分配Row();
                    风控分配Row1.风控员 = FkUserName;
                    风控分配Row1.交易员 = JyUserName;
                    this.Add风控分配Row(风控分配Row1);
                }
            }

            public void Delete(string JyUserName, string FkUserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.风控分配Row 风控分配Row1 = this.First(r => r.交易员 == JyUserName && r.风控员 == FkUserName);
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




            public AASServer.DbDataSet.平台用户DataTable QueryUserInRoles(int[] 角色Array,int 分组1)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                    foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in this)
                    {
                        if (角色Array.Contains(平台用户Row1.角色) && (分组1 == (int)分组.ALL || 平台用户Row1.分组 == 分组1))
                        {
                            平台用户DataTable1.ImportRow(平台用户Row1);
                        }

                    }
                    foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
                    {
                        平台用户Row1.密码 = string.Empty;
                    }
                    return 平台用户DataTable1;
                }
            }



            //public AASServer.DbDataSet.平台用户DataTable Query普通风控员和交易员()
            //{
            //    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
            //    {
            //        AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in this)
            //        {
            //            if (平台用户Row1.角色 == (int)角色.普通风控员 || 平台用户Row1.角色 == (int)角色.交易员)
            //            {
            //                平台用户DataTable1.ImportRow(平台用户Row1);
            //            }
            //        }
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
            //        {
            //            平台用户Row1.密码 = string.Empty;
            //        }
            //        return 平台用户DataTable1;
            //    }
            //}

            //public AASServer.DbDataSet.平台用户DataTable Query非超级管理员()
            //{
            //    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
            //    {
            //        AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in this)
            //        {
            //            if (平台用户Row1.角色 != (int)角色.超级管理员)
            //            {
            //                平台用户DataTable1.ImportRow(平台用户Row1);
            //            }
            //        }
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
            //        {
            //            平台用户Row1.密码 = string.Empty;
            //        }
            //        return 平台用户DataTable1;
            //    }
            //}


            //public AASServer.DbDataSet.平台用户DataTable QueryAllUser()
            //{
            //    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
            //    {
            //        AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in this)
            //        {
            //            平台用户DataTable1.ImportRow(平台用户Row1);

            //        }
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
            //        {
            //            平台用户Row1.密码 = string.Empty;
            //        }
            //        return 平台用户DataTable1;
            //    }
            //}

            //public AASServer.DbDataSet.平台用户DataTable Query普通风控员()
            //{
            //    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
            //    {
            //        AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in this)
            //        {
            //            if (平台用户Row1.角色 == (int)角色.普通风控员)
            //            {
            //                平台用户DataTable1.ImportRow(平台用户Row1);
            //            }

            //        }
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
            //        {
            //            平台用户Row1.密码 = string.Empty;
            //        }
            //        return 平台用户DataTable1;
            //    }
            //}

            //public AASServer.DbDataSet.平台用户DataTable Query交易员()
            //{
            //    using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
            //    {
            //        AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in this)
            //        {
            //            if (平台用户Row1.角色 == (int)角色.交易员)
            //            {
            //                平台用户DataTable1.ImportRow(平台用户Row1);
            //            }

            //        }
            //        foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
            //        {
            //            平台用户Row1.密码 = string.Empty;
            //        }
            //        return 平台用户DataTable1;
            //    }
            //}





            public AASServer.DbDataSet.平台用户Row Get平台用户(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.平台用户Row AASUser1 = this.First(r => r.用户名 == UserName);
                    AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                    平台用户DataTable1.ImportRow(AASUser1);
                    foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
                    {
                        平台用户Row1.密码 = string.Empty;
                    }
                    return 平台用户DataTable1[0];
                }
            }
            public AASServer.DbDataSet.平台用户DataTable QuerySingleUser(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.平台用户DataTable 平台用户DataTable1 = new DbDataSet.平台用户DataTable();
                    foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in this.Where(r => r.用户名 == UserName))
                    {
                        平台用户DataTable1.ImportRow(平台用户Row1);
                    }
                    foreach (AASServer.DbDataSet.平台用户Row 平台用户Row1 in 平台用户DataTable1)
                    {
                        平台用户Row1.密码 = string.Empty;
                    }
                    return 平台用户DataTable1;
                }
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
                    AASServer.DbDataSet.平台用户Row AASUser1 = this.First(r => r.用户名 == UserName);
                    AASUser1.密码 = Password;
                }
            }


            public void AddUser(string UserName, string Password, 角色 Role, decimal 仓位限制, decimal 亏损限制, decimal 手续费率, bool 允许删除碎股订单, 分组 Region)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.平台用户Row AASUser2 = this.New平台用户Row();
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
                    AASServer.DbDataSet.平台用户Row AASUser1 = this.First(r => r.用户名 == UserName);
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
                    AASServer.DbDataSet.平台用户Row AASUser1 = this.First(r => r.用户名 == UserName);
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

            public AASServer.DbDataSet.MAC地址分配DataTable QueryMACBelongUser(string UserName)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.MAC地址分配DataTable MAC地址分配DataTable1 = new DbDataSet.MAC地址分配DataTable();
                    foreach (AASServer.DbDataSet.MAC地址分配Row MAC地址分配Row1 in this)
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
                    AASServer.DbDataSet.MAC地址分配Row MAC地址分配1 = this.NewMAC地址分配Row();
                    MAC地址分配1.用户名 = UserName;
                    MAC地址分配1.MAC = MAC;
                    this.AddMAC地址分配Row(MAC地址分配1);
                }
            }



            public void DeleteMAC(string UserName, string MAC)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Write))
                {
                    AASServer.DbDataSet.MAC地址分配Row MAC地址分配1 = this.First(r => r.用户名 == UserName && r.MAC == MAC);

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
                    AASServer.DbDataSet.交易日志Row 交易日志Row1 = this.New交易日志Row();
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


            public AASServer.DbDataSet.交易日志DataTable QueryTodayJyLog(List<string> JyList)
            {
                using (ReadWriteLock ReadWriteLock1 = new ReadWriteLock(this.readerWriterLockSlim, ReadWriteMode.Read))
                {
                    AASServer.DbDataSet.交易日志DataTable 交易日志DataTable1 = new DbDataSet.交易日志DataTable();
                    foreach (AASServer.DbDataSet.交易日志Row 交易日志Row1 in this.Where(r => JyList.Contains(r.交易员) && r.日期 == DateTime.Today))
                    {
                        交易日志DataTable1.ImportRow(交易日志Row1);
                    }
                    return 交易日志DataTable1;
                }

            }



        }

    }
}

