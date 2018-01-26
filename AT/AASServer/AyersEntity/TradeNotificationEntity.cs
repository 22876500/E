using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AASServer.AyersEntity
{
    public class TradeNotificationEntity
    {
        public TradeNotificationEntity()
        { }

        public TradeNotificationEntity(string msg)
        {
            Init(msg);
        }

        private void Init(string msg)
        {
            var doc = XDocument.Parse(msg);
            Trade_ID = doc.Element("trade_id").Value;
            Price = decimal.Parse(doc.Element("price").Value);
            Qty = decimal.Parse(doc.Element("qty").Value);
            Exch_Trade_Ref = doc.Element("exch_trade_ref").Value;
            Charge = doc.Element("charge").Value;
            Reference = doc.Element("reference").Value;
            Reference1 = doc.Element("reference1").Value;
            UpdateTime = DateTime.Parse(doc.Element("updated_time").Value);
            CreateTime = DateTime.Parse(doc.Element("create_time").Value);
            OutStand_Qty = decimal.Parse(doc.Element("outstand_qty").Value);
            Exec_Qty = decimal.Parse(doc.Element("exec_qty").Value);
            Exec_Price = decimal.Parse(doc.Element("exec_price").Value);
            Order_No = doc.Element("order_no").Value;
            Order_Status = doc.Element("order_status").Value;
            Order_Sub_Status = doc.Element("order_sub_status").Value;
            Cr_Action_Code = doc.Element("cr_action_code").Value;
            Cr_Amend_Status = doc.Element("cr_amend_status").Value;
            Last_Order_Action_Code = doc.Element("last_order_action_code").Value;
            Reject_Reason = doc.Element("reject_reason").Value;
            Bs_Flag = doc.Element("bs_flag").Value;
            Client_Acc_Code = doc.Element("client_acc_code").Value;
            Exchange_Code = doc.Element("exchange_code").Value;
            Product_Code = doc.Element("product_code").Value;
            Order_Type = doc.Element("order_type").Value;
            Input_Channel = doc.Element("input_channel").Value;
        }

        /// <summary>
        /// 交易ID
        /// </summary>
        public string Trade_ID { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        public string Exch_Trade_Ref { get; set; }

        public string Charge { get; set; }

        public string Reference { get; set; }

        public string Reference1 { get; set; }

        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public decimal OutStand_Qty { get; set; }

        public decimal Exec_Qty { get; set; }

        public decimal Exec_Price { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string Order_No { get; set; }

        public string Order_Status { get; set; }

        public string Order_Sub_Status { get; set; }

        public string Cr_Action_Code { get; set; }

        public string Cr_Amend_Status { get; set; }

        public string Last_Order_Action_Code { get; set; }

        public string Reject_Reason { get; set; }

        /// <summary>
        /// 买卖标志
        /// </summary>
        public string Bs_Flag { get; set; }

        public string Client_Acc_Code { get; set; }

        public string Exchange_Code { get; set; }

        public string Product_Code { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string Order_Type { get; set; }

        public string Input_Channel { get; set; }

        //<message type=”trade_notification”>
        //  <trades>
        //      <trade>
        //          <trade_id>20050829:31800389:6.45</trade_id>
        //          <price>6.45</price>
        //          <qty>10000</qty>
        //          <create_time>2005-08-29 15:51:12</create_time>
        //          <exch_trade_ref>20605</exch_trade_ref>
        //          <charge>0</charge>
        //      </trade>
        //  </trades>
        //  <reference>BEA:2407</reference>
        //  <reference1></reference1>
        //  <updated_time>2005-08-29 15:51:14</updated_time> 
        //  <create_time>2005-08-29 10:51:04</create_time>
        //  <qty>10000</qty>
        //  <outstand_qty>0</outstand_qty>
        //  <exec_qty>10000</exec_qty>
        //  <exec_price>6.45</exec_price>
        //  <price>6.45</price>
        //  <order_no>1234</order_no>
        //  <order_status>FEX</order_status>
        //  <order_sub_status></order_sub_status>
        //  <cr_action_code></cr_action_code>
        //  <cr_amend_status></cr_amend_status>
        //  <last_order_action_code></ last_order_action_code>
        //  <reject_reason></reject_reason>
        //  <bs_flag>B</bs_flag>
        //  <client_acc_code>CC1</client_acc_code>
        //  <exchange_code>HKEX</exchange_code>
        //  <product_code>00001</product_code>
        //  <order_type>L<order_type>
        //  <input_channel>TS</input_channel>
        //</message> 
    }
}
