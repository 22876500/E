using AASServer.AyersEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AASServer
{

    public class AyersConfig
    {
        public static int MsgNum { get; set; }

        //public static XDocument GetBasicMessage()
        //{
        //    XDocument xDoc = new XDocument(
        //                         new XElement("message",
        //                             new XAttribute("type", "login"),
        //                             new XAttribute("msgnum", AyersMessageUtils.MsgNum++)
        //                         )
        //                     );
        //    return xDoc;
        //}

        private const string feeConfigPath = "/config/fee_config.xml";
        public static AyersFeeConfig GetFeeConfig()
        {
            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath +  "/config/"))
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "/config/");

            XDocument doc = GetFeeDoc();

            AyersFeeConfig config = new AyersFeeConfig()
            {
                Commission = decimal.Parse(doc.Root.Element("commission").Value),
                CommissionMin = decimal.Parse(doc.Root.Element("commission_min").Value),
                StampTax = decimal.Parse(doc.Root.Element("stamp_tax").Value),
                TransactionLevy = decimal.Parse(doc.Root.Element("transaction_levy").Value),
                TradingFee = decimal.Parse(doc.Root.Element("trading_fee").Value),
                TransferFee = decimal.Parse(doc.Root.Element("transfer_fee").Value),
            };

            return config;
        }

        public static bool SetFeeConfig(decimal Commission, decimal CommissionMinj, decimal StampTax, decimal TransactionLevy, decimal TradingFee,decimal TransferFee, out string errMsg)
        {
            errMsg = string.Empty;
            bool success = false;
            try
            {
                var doc = new XDocument(new XElement("fee_config",
                                         new XElement("commission", "0.0025"),
                                         new XElement("commission_min", "150"),
                                         new XElement("stamp_tax", "0.001"),
                                         new XElement("transaction_levy", "0.00003"),
                                         new XElement("trading_fee", "0.00005"),
                                         new XElement("transfer_fee", "0")));
                doc.Save(feeConfigPath);
                success = true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                success = false;
                Program.logger.LogInfoDetail("AyersConfig, Set Fee Config Failed! Message:{0}, StackTrace:{1}", ex.Message, ex.StackTrace);
            }
            return success;
        }

        public static XDocument GetFeeDoc()
        {
            XDocument doc = null;
            if (File.Exists(System.Windows.Forms.Application.StartupPath + feeConfigPath))
            {
                try
                {
                    var str = File.ReadAllText(System.Windows.Forms.Application.StartupPath + feeConfigPath);
                    doc = XDocument.Parse(str);
                }
                catch (Exception) { }
            }
            if (doc == null)
            {
                doc = new XDocument(new XElement("fee_config",
                                         new XElement("commission", "0.0025"),
                                         new XElement("commission_min", "100"),
                                         new XElement("stamp_tax", "0.001"),
                                         new XElement("transaction_levy", "0.000027"),
                                         new XElement("trading_fee", "0.00005"),
                                         new XElement("transfer_fee", "0")));
                doc.Save(System.Windows.Forms.Application.StartupPath + feeConfigPath);
            }
            return doc;
        }
    }



}
