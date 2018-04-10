using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.DataAdapter.TDB
{
    public class LibTDBWrap
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct OPEN_SETTINGS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] szIP;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] szPort;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] szUser;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] szPassword;

            public Int32 nTimeOutVal;
            public Int32 nRetryCount;
            public Int32 nRetryGap;
        }

        public enum TDBProxyType
        {
            TDB_PROXY_SOCK4,
            TDB_PROXY_SOCK4A,
            TDB_PROXY_SOCK5,
            TDB_PROXY_HTTP11,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDB_Proxy_SETTINGS
        {
            public Int32 nProxyType;    //enum TDBProxyType

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szProxyHostIp;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] szProxyPort;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] szProxyUser;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] szProxyPwd;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_ResLogin
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] szInfo;

            public Int32 nMarkets;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256 * 24)]
            public byte[] szMarket;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public Int32[] nDynDate;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_Code
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chWindCode;        //万得代码(AG1312.SHF)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode;            //交易所代码(ag1312)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] chMarket;           //市场代码(SHF-1-0)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCNName;          //证券中文名称

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chENName;          //证券英文名称

            public Int32 nType;                 //证券类型
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_ReqKLine
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode;//证券万得代码(AG1312.SHF)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] chMarketKey;//市场设置（SHF-1-0）

            public Int32 nCQFlag; //除权标志：0:不复权，1:向前复权，2:向后复权
            public Int32 nCQDate; //复权日期(<=0:全程复权) 格式：YYMMDD，例如20130101表示2013年1月1日
            public Int32 nQJFlag; //全价标志(债券)(0:净价 1:全价)

            public Int32 nCycType;//数据周期：秒线、分钟、日线、周线、月线、季线、半年线、年线
            public Int32 nCycDef; //周期数量：仅当nCycType取值：秒、分钟、日线、周线、月线时，这个字段有效。

            public Int32 nAutoComplete;   //自动补齐：仅1秒钟线、1分钟线支持这个标志，（1：补齐；0：不补齐）
            public Int32 nBeginDate;             //开始日期(交易日，<0:从上市日期开始； 0:从今天开始)
            public Int32 nEndDate;               //结束日期(交易日，<=0:跟nBeginDate一样) ,秒线最多为一周，分钟线最多为一个月，每月按30天计算
            public Int32 nBeginTime;             //开始时间，<=0表示从开始，格式：（HHMMSSmmm）例如94500000 表示 9点45分00秒000毫秒
            public Int32 nEndTime;               //结束时间，<=0表示到结束，格式：（HHMMSSmmm）例如94500000 表示 9点45分00秒000毫秒
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_KLine
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chWindCode;            //万得代码(AG1312.SHF)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode;                //交易所代码(ag1312)

            public Int32 nDate;                   //日期（自然日）格式：YYMMDD，例如20130101表示2013年1月1日，0表示今天
            public Int32 nTime;                   //时间（HHMMSSmmm）例如94500000 表示 9点45分00秒000毫秒
            public Int32 nOpen;                   //开盘((a double number + 0.00005) *10000)
            public Int32 nHigh;                   //最高((a double number + 0.00005) *10000)
            public Int32 nLow;                    //最低((a double number + 0.00005) *10000)
            public Int32 nClose;                  //收盘((a double number + 0.00005) *10000)

            public Int64 iVolume;             //成交量
            public Int64 iTurover;            //成交额(元)

            public Int32 nMatchItems;         //成交笔数
            public Int32 nInterest;           //持仓量(期货)、IOPV(基金)、利息(债券)
        }

        public enum REFILLFLAG
        {
            REFILL_NONE = 0,            //不复权
            REFILL_BACKWARD = 1,      //全程向前复权（从现在向过去）
            REFILL_FORWARD = 2,       //全程向后复权（从过去向现在）
        }
        public enum CYCTYPE
        {
            CYC_SECOND = 0,
            CYC_MINUTE,
            CYC_DAY,
            CYC_WEEK,
            CYC_MONTH,
            CYC_SEASON,
            CYC_HAFLYEAR,
            CYC_YEAR,
            CYC_TICKBAR,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_ReqTick
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode; //证券万得代码(AG1312.SHF)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] chMarketKey;//市场设置（SHF-1-0）
            public Int32 nDate;    //日期（交易日）,为0则从今天，格式：YYMMDD，例如20130101表示2013年1月1日
            public Int32 nBeginTime;    //开始时间：若<=0则从头，格式：（HHMMSSmmm）例如94500000 表示 9点45分00秒000毫秒
            public Int32 nEndTime;      //结束时间：若<=0则至最后

            public Int32 nAutoComplete; //自动补齐，仅期货，暂不支持
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_Tick
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chWindCode;                //万得代码(AG1312.SHF)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode;                    //交易所代码(ag1312)
            public Int32 nDate;                          //日期（自然日）
            public Int32 nTime;                          //时间（HHMMSSmmm）例如94500000 表示 9点45分00秒000毫秒
            public Int32 nPrice;                         //成交价((a double number + 0.00005) *10000)
            public Int64 iVolume;                    //成交量
            public Int64 iTurover;                //成交额(元)
            public Int32 nMatchItems;                    //成交笔数
            public Int32 nInterest;                      //IOPV(基金)、利息(债券)
            public byte chTradeFlag;                   //成交标志
            public byte chBSFlag;                      //BS标志
            public Int64 iAccVolume;                 //当日累计成交量
            public Int64 iAccTurover;             //当日成交额(元)
            public Int32 nHigh;                          //最高((a double number + 0.00005) *10000)
            public Int32 nLow;                           //最低((a double number + 0.00005) *10000)
            public Int32 nOpen;                       //开盘((a double number + 0.00005) *10000)
            public Int32 nPreClose;                   //前收盘((a double number + 0.00005) *10000)

            //期货字段
            public Int32 nSettle;                        //结算价((a double number + 0.00005) *10000)
            public Int32 nPosition;                       //持仓量
            public Int32 nCurDelta;                      //虚实度
            public Int32 nPreSettle;                    //昨结算((a double number + 0.00005) *10000)
            public Int32 nPrePosition;                  //昨持仓

            //买卖盘字段
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public Int32[] nAskPrice;               //叫卖价((a double number + 0.00005) *10000)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public UInt32[] nAskVolume;            //叫卖量

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public Int32[] nBidPrice;               //叫买价((a double number + 0.00005) *10000)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public UInt32[] nBidVolume;            //叫买量

            public Int32 nAskAvPrice;                 //加权平均叫卖价(上海L2)((a double number + 0.00005) *10000)
            public Int32 nBidAvPrice;                 //加权平均叫买价(上海L2)((a double number + 0.00005) *10000)
            public Int64 iTotalAskVolume;           //叫卖总量(上海L2)
            public Int64 iTotalBidVolume;           //叫买总量(上海L2)


            //指数字段
            public Int32 nIndex;                  //不加权指数
            public Int32 nStocks;                 //品种总数
            public Int32 nUps;                    //上涨品种数
            public Int32 nDowns;                  //下跌品种数
            public Int32 nHoldLines;              //持平品种数

            public Int32 nResv1;
            public Int32 nResv2;
            public Int32 nResv3;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_ReqTransaction
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode;               //证券万得代码(AG1312.SHF)

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public byte[] chMarketKey;//市场设置（SHF-1-0）
            public Int32 nDate;    //日期（交易日）,为0则从今天，格式：YYMMDD，例如20130101表示2013年1月1日
            public Int32 nBeginTime;    //开始时间：若<=0则从头，格式：（HHMMSSmmm）例如94500000 表示 9点45分00秒000毫秒
            public Int32 nEndTime;      //结束时间：若<=0则至最后
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_Order
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chWindCode;        //万得代码(AG1312.SHF)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode;            //交易所代码(ag1312)
            public Int32 nDate;                 //日期（自然日）格式YYMMDD
            public Int32 nTime;                 //委托时间（精确到毫秒HHMMSSmmm）
            public Int32 nIndex;                //委托编号，从1开始，递增1
            public Int32 nOrder;                //交易所委托号
            public byte chOrderKind;           //委托类别
            public byte chFunctionCode;        //委托代码, B, S, C
            public Int32 nOrderPrice;           //委托价格((a double number + 0.00005) *10000)
            public Int32 nOrderVolume;          //委托数量
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_Transaction
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chWindCode;     //万得代码(AG1312.SHF)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode;         //交易所代码(ag1312)
            public Int32 nDate;              //日期（自然日）格式:YYMMDD
            public Int32 nTime;              //成交时间(精确到毫秒HHMMSSmmm)
            public Int32 nIndex;             //成交编号(从1开始，递增1)
            public byte chFunctionCode;     //成交代码: 'C', 0
            public byte chOrderKind;        //委托类别
            public byte chBSFlag;           //BS标志
            public Int32 nTradePrice;        //成交价格((a double number + 0.00005) *10000)
            public Int32 nTradeVolume;       //成交数量
            public Int32 nAskOrder;          //叫卖序号
            public Int32 nBidOrder;          //叫买序号
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TDBDefine_OrderQueue
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chWindCode;         //万得代码(AG1312.SHF)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chCode;             //交易所代码(ag1312)
            public Int32 nDate;                  //日期（自然日）格式YYMMDD
            public Int32 nTime;                  //订单时间(精确到毫秒HHMMSSmmm)
            public Int32 nSide;                  //买卖方向('B':Bid 'A':Ask)
            public Int32 nPrice;                 //成交价格((a double number + 0.00005) *10000)
            public Int32 nOrderItems;            //订单数量
            public Int32 nABItems;               //明细个数
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public Int32[] nABVolume;          //订单明细
        }

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_Open",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern IntPtr TDB_Open(IntPtr pSetting, IntPtr pLoginRes);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_OpenProxy",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern IntPtr TDB_OpenProxy(IntPtr pSetting, IntPtr pProxySetting, IntPtr pLoginRes);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_Close",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern Int32 TDB_Close(IntPtr hTdb);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_GetCodeTable",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern Int32 TDB_GetCodeTable(IntPtr hTdb, IntPtr szMarket, IntPtr ppCodeTable, IntPtr pCount);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_Free",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern void TDB_Free(IntPtr pArr);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_GetKLine",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern Int32 TDB_GetKLine(IntPtr hTdb, IntPtr pReq, IntPtr pData, IntPtr pCount);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_GetTick",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern Int32 TDB_GetTick(IntPtr hTdb, IntPtr pReq, IntPtr pData, IntPtr pCount);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_GetTransaction",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern Int32 TDB_GetTransaction(IntPtr hTdb, IntPtr pReq, IntPtr pData, IntPtr pCount);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_GetOrderQueue",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern Int32 TDB_GetOrderQueue(IntPtr hTdb, IntPtr pReq, IntPtr pData, IntPtr pCount);

        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_GetOrder",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern Int32 TDB_GetOrder(IntPtr hTdb, IntPtr pReq, IntPtr pData, IntPtr pCount);


        [DllImport(
             "TDBAPI.dll",
             EntryPoint = "TDB_GetCodeInfo",
             CallingConvention = CallingConvention.Cdecl,
             ExactSpelling = true
             )]
        public static extern IntPtr TDB_GetCodeInfo(IntPtr hTdb, IntPtr pWindCode, IntPtr pMarket);

    }
}
