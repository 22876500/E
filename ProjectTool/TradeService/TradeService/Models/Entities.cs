using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradeService.Models
{
    public class Entities
    {
    }

    public class 券商
    {
        public string 名称 { get; set; }

        public bool 启用 { get; set; }

        public string 交易服务器 { get; set; }

        public string IP { get; set; }

        public short Port { get; set; }

        public string 版本号 { get; set; }

        public short 营业部代码 { get; set; }

        public string 登录帐号 { get; set; }

        public string 交易帐号 { get; set; }

        public string 交易密码 { get; set; }

        public string TradePsw
        {
            get { return Cryptor.MD5Decrypt(交易密码); }
        }

        public string 通讯密码 { get; set; }

        public string CommunicatePsw
        {
            get { return Cryptor.MD5Decrypt(通讯密码); }
        }

    }
}