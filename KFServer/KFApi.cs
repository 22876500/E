using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AASServer
{
    //public class KFApi
    //{
    //    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    //    public delegate void rtn_order_callback_function(int orderID, KFRtnOrder order);

    //    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    //    public delegate void rtn_trade_callback_function(int orderID, KFRtnTrade trade);

    //    [DllImport("kfapi.dll", CharSet = CharSet.Unicode)]
    //    public extern static void connect(string client_name, string server_addr);

    //    [DllImport("kfapi.dll", CharSet = CharSet.Unicode)]
    //    public extern static void disconnect();

    //    [DllImport("kfapi.dll", CharSet = CharSet.Unicode)]
    //    public extern static int insert_market_order(string ticker, string exchange_id, double price, int volume, char direction, char offset);

    //    [DllImport("kfapi.dll", CharSet = CharSet.Unicode)]
    //    public extern static void cancel_order(int order_id);

    //    [DllImport("kfapi.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    //    public extern static void register_rtn_order_callback(rtn_order_callback_function rtn_order_callback);

    //    [DllImport("kfapi.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    //    public extern static void register_rtn_trade_callback(rtn_trade_callback_function rtn_trade_callback);
        

    //    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]  
    //    public struct KFRtnOrder
    //    {
    //        public StringBuilder ticker;
    //        public int order_id;
    //        public double price;
    //        public int volume;
    //        public int volume_remain;
    //        public int volume_traded;
    //        public char direction;//买卖方向
    //        public char offset; //开平标志
    //        public char status;//报单状态
    //    };

    //    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]  
    //    public struct KFRtnTrade
    //    {
    //        public StringBuilder ticker;
    //        public int order_id;
    //        public double price;
    //        public int volume;
    //        public char direction;//买卖方向  '0' 买 '1' 卖
    //        public char offset;//开平标志
    //    };

    //    ///////////////////////////////////
    //    // LfOffsetFlagType: 开平标志
    //    ///////////////////////////////////
    //    //开仓
    //    //#define LF_CHAR_Open            '0'
    //    ////平仓
    //    //#define LF_CHAR_Close           '1'
    //    ////强平
    //    //#define LF_CHAR_ForceClose      '2'
    //    ////平今
    //    //#define LF_CHAR_CloseToday      '3'
    //    ////平昨
    //    //#define LF_CHAR_CloseYesterday  '4'
    //    ////强减
    //    //#define LF_CHAR_ForceOff        '5'
    //    ////本地强平
    //    //#define LF_CHAR_LocalForceClose '6'
    //    ////不分开平
    //    //#define LF_CHAR_Non             'N'

    //    ///////////////////////////////////
    //    // LfOrderStatusType: 报单状态
    //    ///////////////////////////////////
    //    //全部成交（最终状态）
    //    //#define LF_CHAR_AllTraded       '0'
    //    ////部分成交还在队列中
    //    //#define LF_CHAR_PartTradedQueueing '1'
    //    ////部分成交不在队列中（部成部撤， 最终状态）
    //    //#define LF_CHAR_PartTradedNotQueueing '2'
    //    ////未成交还在队列中
    //    //#define LF_CHAR_NoTradeQueueing '3'
    //    ////未成交不在队列中（被拒绝，最终状态）
    //    //#define LF_CHAR_NoTradeNotQueueing '4'
    //    ////撤单
    //    //#define LF_CHAR_Canceled        '5'
    //    ////订单已报入交易所未应答
    //    //#define LF_CHAR_AcceptedNoReply '6'
    //    ////未知
    //    //#define LF_CHAR_Unknown         'a'
    //    ////尚未触发
    //    //#define LF_CHAR_NotTouched      'b'
    //    ////已触发
    //    //#define LF_CHAR_Touched         'c'
    //    ////废单错误（最终状态）
    //    //#define LF_CHAR_Error           'd'
    //    ////订单已写入
    //    //#define LF_CHAR_OrderInserted   'i'
    //    ////前置已接受
    //    //#define LF_CHAR_OrderAccepted   'j'
    //}
}
