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

namespace AASServer
{
    public class Logger
    {
        ILog ILog;


        public void Init(ListView listView1)
        {
            string DirectoryName = "log";
            Directory.CreateDirectory(DirectoryName);


            ListViewAppender ListViewAppender1 = new ListViewAppender(listView1);
            ListViewAppender1.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(ListViewAppender1);

            log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
            appender.File =  DirectoryName+"\\";
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

        public void Init()
        {
            string DirectoryName = "log";
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

        public void LogInfo(string format, params object[] args)
        {
            this.LogInfo(string.Format(format, args));
        }
        public void LogInfo(string Msg)
        {
            if (this.ILog == null)
            {
                return;
            }

            this.ILog.Info(Msg.Replace("\r", string.Empty).Replace("\n", string.Empty));
        }

        public void LogInfoDetail(string format, params object[] args)
        {
            if (ConfigCache.UseLogDetail == "1")
            {
                this.LogInfo(string.Format(format, args));
            }
            
        }
    }

    /// <summary>
    /// Description of RichTextBoxAppender.
    /// </summary>
    public class ListViewAppender : AppenderSkeleton
    {
        #region Private Instance Fields

        private ListView _listView = null;
        private LevelMapping levelMapping = new LevelMapping();
        private int maxTextLength = 100000;

        #endregion Private Instance Fields

        private delegate void UpdateControlDelegate(LoggingEvent loggingEvent);

        #region Constructor

        public ListViewAppender(ListView myListView)
            : base()
        {
            _listView = myListView;
        }

        #endregion Constructor

        private void UpdateControl(LoggingEvent loggingEvent)
        {
            if (loggingEvent.Level != Level.Info)
            {
                return;
            }

            if (_listView == null)
                return;

            if (_listView.IsDisposed)
                return;


            // There may be performance issues if the buffer gets too long
            // So periodically clear the buffer
            if (_listView.Items.Count > maxTextLength)
            {
                _listView.Items.Clear();
            }

            ListViewItem lvi = new ListViewItem();

            //switch (loggingEvent.Level.ToString())
            //{
            //    case "INFO":
            //        lvi.ForeColor = System.Drawing.Color.Blue;
            //        break;
            //    case "WARN":
            //        lvi.ForeColor = System.Drawing.Color.Orange;
            //        break;
            //    case "ERROR":
            //        lvi.ForeColor = System.Drawing.Color.Red;
            //        break;
            //    case "FATAL":
            //        lvi.ForeColor = System.Drawing.Color.DarkOrange;
            //        break;
            //    case "DEBUG":
            //        lvi.ForeColor = System.Drawing.Color.DarkGreen;
            //        break;
            //    default:
            //        lvi.ForeColor = System.Drawing.Color.Black;
            //        break;
            //}

            //if (this._listView.Items.Count % 2 == 0)
            //{
            //    lvi.BackColor = System.Drawing.Color.Silver;
            //}
            //else
            //{
            //    lvi.BackColor = System.Drawing.Color.LightBlue;
            //}


            lvi.Text = loggingEvent.TimeStamp.ToString("HH:mm:ss");
            // lvi.SubItems.Add(loggingEvent.UserName);
            //lvi.SubItems.Add(loggingEvent.Level.DisplayName);
            //lvi.SubItems.Add(loggingEvent.LoggerName);
            // lvi.SubItems.Add(string.Format("{0}.{1}.{2}", loggingEvent.LocationInformation.ClassName, loggingEvent.LocationInformation.MethodName, loggingEvent.LocationInformation.LineNumber));
            lvi.SubItems.Add(loggingEvent.RenderedMessage);
            _listView.Items.Add(lvi);
            lvi.EnsureVisible();
            //_listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        protected override void Append(LoggingEvent LoggingEvent)
        {
            if (_listView.InvokeRequired)
            {
                _listView.Invoke(
                    new UpdateControlDelegate(UpdateControl),
                    new object[] { LoggingEvent });
            }
            else
            {
                UpdateControl(LoggingEvent);
            }
        }

        public void AddMapping(LevelMappingEntry mapping)
        {
            levelMapping.Add(mapping);
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            levelMapping.ActivateOptions();
        }

        protected override bool RequiresLayout
        {
            get
            {
                return false;
            }
        }
    }

   
}
