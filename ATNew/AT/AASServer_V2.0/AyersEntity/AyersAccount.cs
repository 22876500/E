using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASServer.AyersEntity
{
    public class AyersAccount
    {

        public AyersAccount()
        { }

        public string Server_Ip { get; set; }

        public int Port_No_Encryption { get; set; }

        public int Port_Encryption { get; set; }

        public string Encryption_Key { get; set; }

        public string Message_Compression { get; set; }

        public string Api_Login_ID { get; set; }

        public string Api_Login_Psw { get; set; }

        public string Site_ID { get; set; }

        public string Station_ID { get; set; }

        public string Type { get; set; }

        public string Client_Acc_ID
        {
            get
            {
                return Client_ID_Using == Client_ID_First ? Client_ID_First : Client_ID_Second;
            }
        }

        public string Client_Acc_Psw
        {
            get
            {
                return Client_ID_Using == Client_ID_First ? Client_Psw_First : Client_Psw_Second;
            }
        }

        public string Client_ID_First { get; set; }

        public string Client_Psw_First { get; set; }

        public string Client_ID_Second { get; set; }

        public string Client_Psw_Second { get; set; }

        public string Client_ID_Using { get; set; }

        public int PortUsing { get; set; }
    }
}
