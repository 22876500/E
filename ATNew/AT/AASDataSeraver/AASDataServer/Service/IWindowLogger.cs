using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AASDataServer.Model;

namespace AASDataServer.Service
{
    public interface IWindowLogger
    {
        int Capacity
        { get; set; }

        event Action<LogMessage> NewLogEvent;

        void Info(string msg);

        void Info(string sender, string msg);

        void Warn(string msg);

        void Warn(string sender, string msg);

        void Error(string msg);

        void Error(string sender, string msg);

        void Error(string msg, Exception ex);

        void Error(string sender, string msg, Exception ex);
    }
}
