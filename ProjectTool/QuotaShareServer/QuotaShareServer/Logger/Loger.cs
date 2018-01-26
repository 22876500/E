using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaShareServer.Logger
{
    public class Loger
    {
        public Loger(string logs)
        {
            this.Times = DateTime.Now.ToString("HH:mm:ss");
            this.Logs = logs;
            Logger.LogHelper.Instance.Info(this.Logs);
        }
        public string Logs { get; set; }

        public string Times { get; set; }
    }
}
