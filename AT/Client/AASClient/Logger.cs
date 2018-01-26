using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public class Logger
    {
        ILog ILog;


        public void Init()//2运行日志
        {
            string DirectoryName = Program.Current平台用户.用户名 + "\\log";
            Directory.CreateDirectory(DirectoryName);

            log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
            appender.File = DirectoryName + "\\";
            appender.AppendToFile = true;
            appender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Date;
            appender.DatePattern = "yyyy-MM-dd\".txt\"";
            appender.Layout = new log4net.Layout.PatternLayout("%date{HH:mm:ss}   %message%newline");
            appender.StaticLogFileName = false;
            appender.ImmediateFlush = true;
            appender.LockingModel = new log4net.Appender.FileAppender.MinimalLock();
            appender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(appender);

            this.ILog = LogManager.GetLogger("Logger");
        }


        public void LogJy(string UserName, string Zqdm, string Zqmc, string Wtbh, int Jylb, decimal Wtsl, decimal Wtjg, string format, params object[] args)
        {
            string Msg = string.Format(format, args);




            Program.mainForm.Invoke((Action)delegate()
            {
                AASClient.AASServiceReference.DbDataSet.交易日志Row 交易日志Row1 = Program.serverDb.交易日志.New交易日志Row();
                交易日志Row1.日期 = DateTime.Today;
                交易日志Row1.时间 = DateTime.Now.ToString("HH:mm:ss");
                交易日志Row1.交易员 = UserName;
                交易日志Row1.组合号 = string.Empty;
                交易日志Row1.证券代码 = Zqdm;
                交易日志Row1.证券名称 = Zqmc;
                交易日志Row1.委托编号 = Wtbh;
                交易日志Row1.买卖方向 = Jylb;
                交易日志Row1.委托数量 = Wtsl;
                交易日志Row1.委托价格 = Wtjg;
                交易日志Row1.信息 = Msg;
                Program.serverDb.交易日志.Rows.InsertAt(交易日志Row1, 0);
            });




            string string1 = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", UserName, Zqdm, Zqmc, Wtbh, Jylb, Wtsl, Wtjg, Msg);

            this.LogWarn(string1);
        }

      




        public void LogRunning(string format, params object[] args)
        {
            if (Program.mainForm == null)
                return;

            if (Program.mainForm.IsHandleCreated)
            {
                Program.mainForm.Invoke((Action)delegate()
                {
                    try
                    {
                        AASClient.UIDataSet.运行日志Row 运行日志Row1 = Program.uiDataSet.运行日志.New运行日志Row();
                        运行日志Row1.时间 = DateTime.Now;
                        运行日志Row1.信息 = string.Format(format, args);
                        Program.uiDataSet.运行日志.Rows.InsertAt(运行日志Row1, 0);
                    }
                    catch (Exception)
                    {
                        //运行日志记录出错！
                    }
                    
                });

                this.LogInfo(string.Format(format, args));
            }
        }




        public void LogInfo(string Msg)
        {
            if (this.ILog == null)
            {
                return;
            }

            string LogMsg = Msg.Replace("\r", string.Empty).Replace("\n", string.Empty);


           


            this.ILog.Info(LogMsg);
        }

        void LogWarn(string Msg)
        {
            if (this.ILog == null)
            {
                return;
            }

            string LogMsg = Msg.Replace("\r", string.Empty).Replace("\n", string.Empty);


            this.ILog.Warn(LogMsg);
        }


        
    }

   
}
