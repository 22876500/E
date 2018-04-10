using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AASServer.AyersEntity
{
    public class LoginMessage
    {
        public LoginMessage()
        { }

        public LoginMessage(string xDoc)
        {
            Init(xDoc);
        }

        public void Init(string xDoc)
        {
            XDocument doc = XDocument.Parse(xDoc);
            Status = doc.Root.Element("status").Value;
            Information = doc.Root.Element("information").Value;
            Alert_Change_Pwd = doc.Root.Element("alert_change_pwd").Value;
            Force_Change_Pwd = doc.Root.Element("force_change_pwd").Value;
            Pwd_Expiry_Date = DateTime.Parse(doc.Root.Element("pwd_expiry_date").Value);
            Last_Login_Time = DateTime.Parse(doc.Root.Element("last_login_time").Value);
            Recovery_Order_Count = doc.Root.Element("recovery_order_count").Value;
        }

        public string Status { get; set; }

        public string Information { get; set; }

        public string Alert_Change_Pwd { get; set; }

        public string Force_Change_Pwd { get; set; }

        public DateTime Pwd_Expiry_Date { get; set; }

        public DateTime Last_Login_Time { get; set; }

        public string Recovery_Order_Count { get; set; }
    }
}
