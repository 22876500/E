using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace AASDataWService.Logger
{

    public class LogHelper
    {
        public const string Log4Config = "log.config";
        private ILog _log = null;
        private static LogHelper _logger = null;

        public LogHelper()
        {
            //string realPath = AppDomain.CurrentDomain.BaseDirectory;
            ////暂时写死
            //FileInfo configFile = new FileInfo(string.Format("{0}\\{1}", realPath, Log4Config));
            //log4net.Config.XmlConfigurator.ConfigureAndWatch(configFile);
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

            _log = LogManager.GetLogger("Logger");
        }
        public static LogHelper Instance
        {
            get
            {
                if (_logger == null)
                    _logger = new LogHelper();
                return _logger;
            }
        }

        public void Fatal(string message)
        {
            _log.Fatal(message);
        }

        public void Fatal(String message, System.Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void Warn(String message)
        {
            _log.Warn(message);
        }
        public void Warn(String message, System.Exception exception)
        {
            _log.Warn(message, exception);
        }

        public void Error(String message)
        {
            _log.Error(message);
        }

        public void Error(String message, System.Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Debug(String message)
        {
            _log.Debug(message);
        }

        public void Info(String message)
        {
            _log.Info(message);
        }

    }
}