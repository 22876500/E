using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.Model.Setting
{
    public class HHDataAdapterSetting
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> InitCodes { get; set; }
        public bool IsHistory { get; set; }
        public DateTime PlayTime { get; set; }

        public HHDataAdapterSetting()
        {
            Ip = "114.80.154.34";
            Port = 6221;
            Username = "TD1038491003";
            Password = "37442745";
            InitCodes = new List<string>();
            IsHistory = false;
            //PlayTime = new DateTime(2016,7,14,10,0,0);
        }
    }
}
