using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitManagement
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

        void Init()//2运行日志
        {
            string DirectoryName = Utils.CurrentPath + "\\log";
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

        public void LogRunning(string format, params object[] args)
        {
            this.LogInfo(string.Format(format, args));
        }

        public void LogErr(string info, Exception e)
        {
            this.ILog.Error(info, e);
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
