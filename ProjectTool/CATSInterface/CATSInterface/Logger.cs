using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATSInterface
{
    public class Logger
    {
        ILog ILog;

        public Logger()
        {
            try
            {
                Init();
            }
            catch (Exception)
            {

            }

        }

        public void Init()//2运行日志
        {
            string DirectoryName = AppDomain.CurrentDomain.BaseDirectory + "\\log";
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


        public void LogInfo(string Msg)
        {
            if (this.ILog == null)
            {
                return;
            }

            string LogMsg = Msg.Replace("\r", string.Empty).Replace("\n", string.Empty);
            this.ILog.Info(LogMsg);
        }

        public void LogInfo(string Msg, params object[] paras)
        {
            if (this.ILog == null)
            {
                return;
            }

            string LogMsg = string.Format(Msg, paras).Replace("\r", string.Empty).Replace("\n", string.Empty);
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
