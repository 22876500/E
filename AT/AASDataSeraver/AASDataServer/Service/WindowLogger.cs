using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using log4net;
using AASDataServer.Model;

namespace AASDataServer.Service
{
    public class WindowLogger : IWindowLogger
    {
        private static readonly ILog log = App.Logger;

        private int _capacity;
        private ObservableCollection<LogMessage> _logs;

        public ObservableCollection<LogMessage> Logs
        {
            get {
                return _logs;
            }
        }

        public int Capacity
        {
            get
            {
                return _capacity;
            }
            set
            {
                _capacity = value;
            }
        }

        public event Action<LogMessage> NewLogEvent;

        public WindowLogger()
        {
            _capacity = 1000;
            _logs = new ObservableCollection<LogMessage>();
        }

        public void Info(string msg)
        {
            Message(LogMessageType.INFO, "", msg, null);
        }

        public void Info(string sender, string msg)
        {
            Message(LogMessageType.INFO, sender, msg, null);
        }

        public void Warn(string msg)
        {
            Message(LogMessageType.WARN, "", msg, null);
        }

        public void Warn(string sender, string msg)
        {
            Message(LogMessageType.WARN, sender, msg, null);
        }

        public void Error(string msg)
        {
            Message(LogMessageType.ERROR, "", msg, null);
        }

        public void Error(string msg, Exception ex)
        {
            Message(LogMessageType.ERROR, "", msg, ex);
        }

        public void Error(string sender, string msg)
        {
            Message(LogMessageType.ERROR, sender, msg, null);
        }

        public void Error(string sender, string msg, Exception ex)
        {
            Message(LogMessageType.ERROR, sender, msg, ex);
        }

        private void Message(LogMessageType type, string sender, string msg, Exception ex)
        {
            LogMessage lm = new LogMessage();
            lm.Sender = sender;
            lm.Message = msg;
            lm.Type = type;
            lm.Time = DateTime.Now;
            switch (lm.Type)
            {
                case LogMessageType.INFO:
                    log.Info(sender + ":" + msg);
                    break;
                case LogMessageType.WARN:
                    log.Warn(sender + ":" + msg);
                    break;
                case LogMessageType.ERROR:
                    log.Error(sender + ":" + msg, ex);
                    break;
                default:
                    break;
            }

            if (_logs.Count > _capacity)
            {
                _logs.RemoveAt(_logs.Count - 1);
            }
            _logs.Insert(0, lm);

            if (NewLogEvent != null)
            {
                NewLogEvent(lm);
            }
        }






       
    }
}
