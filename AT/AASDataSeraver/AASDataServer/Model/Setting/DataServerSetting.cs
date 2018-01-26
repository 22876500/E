using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.Model.Setting
{
    public class DataServerSetting
    {

        /// <summary>
        /// TCP广播地址
        /// </summary>
        public string PubAddress { get; set; }

        /// <summary>
        /// TCB广播端口
        /// </summary>
        public int PubPort { get; set; }

        /// <summary>
        /// 是否提供数据转发
        /// </summary>
        public bool IsTransport { get; set; }

        /// <summary>
        /// 订阅数据是否压缩
        /// </summary>
        public bool IsCompress { get; set; }

        /// <summary>
        /// 订阅地址
        /// </summary>
        public string SubAddress { get; set; }

        /// <summary>
        /// 订阅端口号
        /// </summary>
        public int SubPort { get; set; }

        /// <summary>
        /// 开盘时间
        /// </summary>
        public DateTime OpenTime { get; set; }

        /// <summary>
        /// 收盘时间
        /// </summary>
        public DateTime CloseTime { get; set; }

        /// <summary>
        /// 是否重启
        /// </summary>
        public bool IsReboot { get; set; }

        /// <summary>
        /// 重启时间
        /// </summary>
        public DateTime RebootTime { get; set; }

        public List<string> InitCodes { get; set; }

        public string InitCodesString 
        {
            get {
                StringBuilder s = new StringBuilder();
                foreach (string code in InitCodes)
                {
                    s.Append(code);
                    s.Append(";");
                }
                return s.ToString().TrimEnd(';');
            }
            set {
                string s = value;
                string[] codes = s.Split(';');
                InitCodes = new List<string>();
                if (codes.Length > 0)
                {
                    foreach (string code in codes)
                    {
                        if (code.Length == 6 && InitCodes.Contains(code)==false)
                        {
                            InitCodes.Add(code);
                        }
                    }
                }
            }
        }

        public DataServerSetting()
        {
            PubAddress = "192.168.1.51";
            PubPort = 38002;
            IsTransport = false;
            IsCompress = false;
            SubAddress = "192.168.1.51";
            SubPort = 38000;
            InitCodes = new List<string>();
            OpenTime = new DateTime(2015, 4, 26, 9, 30, 0);
            CloseTime = new DateTime(2015, 4, 26, 15, 0, 0);
            IsReboot = false;
            RebootTime = new DateTime(2015, 4, 26, 9, 0, 0);
        }
    }
}
