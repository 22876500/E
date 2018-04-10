using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.Model
{
    public enum LogMessageType
    {
        INFO,
        WARN,
        ERROR
    }

    public class LogMessage : AbstractModel
    {
        private string _sender;
        private string _message;
        private LogMessageType _type;
        private DateTime _time;

        public string Sender 
        {
            get {
                return _sender;
            }
            set {
                _sender = value;
            }
        }

        public string Message 
        {
            get {
                return _message;
            }
            set {
                _message = value;
            }
        }
        public LogMessageType Type 
        {
            get {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public DateTime Time 
        {
            get {
                return _time;
            }
            set
            {
                _time = value;
            }
        }

        public LogMessage()
        {
            _sender = "";
            _message = "";
            _type = LogMessageType.INFO;
            _time = DateTime.Now;
        }
    }
}
