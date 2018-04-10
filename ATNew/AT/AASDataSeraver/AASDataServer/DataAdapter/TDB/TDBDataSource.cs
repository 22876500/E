using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.DataAdapter.TDB
{
    class TDBDataSource
    {
        public TDBDataSource(string strIP = " ", string strPort = " ", string strUser = " ", string strPassword = " ", int nTimeOutVal = 30, int nRetryCount = 1, int nRetryGap = 1)
        {
            m_strIP = strIP;
            m_strPort = strPort;
            m_strUser = strUser;
            m_strPassword = strPassword;
            m_nTimeOutVal = nTimeOutVal;
            m_nRetryCount = nRetryCount;
            m_nRetryGap = nRetryGap;

            m_hTdb = (IntPtr)0;
        }
        public TDBDataSource(string strIP = " ", string strPort = " ", string strUser = " ", string strPassword = " ",
            string strProxyIp = " ", string strProxyPort = " ", string strProxyUser = " ", string strProxyPwd = " ",
            LibTDBWrap.TDBProxyType nProxyType = LibTDBWrap.TDBProxyType.TDB_PROXY_HTTP11, int nTimeOutVal = 30, int nRetryCount = 1, int nRetryGap = 1)
        {
            m_strIP = strIP;
            m_strPort = strPort;
            m_strUser = strUser;
            m_strPassword = strPassword;
            m_strProxyIp = strProxyIp;
            m_strProxyPort = strProxyPort;
            m_strProxyUser = strProxyUser;
            m_strProxyPwd = strProxyPwd;
            m_nTimeOutVal = nTimeOutVal;
            m_nRetryCount = nRetryCount;
            m_nRetryGap = nRetryGap;
            m_nProxyType = (int)nProxyType;

            m_hTdb = (IntPtr)0;
        }


        ~TDBDataSource()
        {
            DisConnect();
        }

        //同步连接到数据源，返回值TDB_SUCCESS表示成功
        public TDBErrNo Connect(out TDBLoginResult tdbLoginResult)
        {
            //重复登录处理
            if ((UInt64)m_hTdb != 0)
            {
                tdbLoginResult = (TDBLoginResult)m_loginResult.Clone();
                Console.WriteLine("已经登录，登录信息:{0}", tdbLoginResult.m_strInfo);
                return TDBErrNo.TDB_SUCCESS;
            }

            LibTDBWrap.OPEN_SETTINGS openSettings = new LibTDBWrap.OPEN_SETTINGS();
            openSettings.szIP = LibWrapHelper.String2AnsiArr(m_strIP, 24);
            openSettings.szPort = LibWrapHelper.String2AnsiArr(m_strPort, 8);
            openSettings.szUser = LibWrapHelper.String2AnsiArr(m_strUser, 32);
            openSettings.szPassword = LibWrapHelper.String2AnsiArr(m_strPassword, 32);

            openSettings.nTimeOutVal = m_nTimeOutVal;
            openSettings.nRetryCount = m_nRetryCount;
            openSettings.nRetryGap = m_nRetryGap;

            IntPtr pUnmanagedOpenSettings = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LibTDBWrap.OPEN_SETTINGS)));
            Marshal.StructureToPtr(openSettings, pUnmanagedOpenSettings, false);

            IntPtr pUnmanagedLoginRes = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_ResLogin)));

            m_hTdb = LibTDBWrap.TDB_Open(pUnmanagedOpenSettings, pUnmanagedLoginRes);
            LibTDBWrap.TDBDefine_ResLogin loginRes = (LibTDBWrap.TDBDefine_ResLogin)Marshal.PtrToStructure(pUnmanagedLoginRes, typeof(LibTDBWrap.TDBDefine_ResLogin));

            Marshal.FreeHGlobal(pUnmanagedOpenSettings);
            Marshal.FreeHGlobal(pUnmanagedLoginRes);

            m_loginResult = new TDBLoginResult();
            m_loginResult.FromAPILoginResult(loginRes);

            tdbLoginResult = (TDBLoginResult)m_loginResult.Clone();

            if ((UInt64)m_hTdb != 0)
            {
                return TDBErrNo.TDB_SUCCESS;
            }
            else
            {
                return TDBErrNo.TDB_OPEN_FAILED;
            }
        }

        //代理连接，返回值TDB_SUCCESS表示成功
        public TDBErrNo ConnectProxy(out TDBLoginResult tdbLoginResult)
        {
            //重复登录处理
            if ((UInt64)m_hTdb != 0)
            {
                tdbLoginResult = (TDBLoginResult)m_loginResult.Clone();
                Console.WriteLine("已经登录，登录信息:{0}", tdbLoginResult.m_strInfo);
                return TDBErrNo.TDB_SUCCESS;
            }

            LibTDBWrap.OPEN_SETTINGS openSettings = new LibTDBWrap.OPEN_SETTINGS();
            openSettings.szIP = LibWrapHelper.String2AnsiArr(m_strIP, 24);
            openSettings.szPort = LibWrapHelper.String2AnsiArr(m_strPort, 8);
            openSettings.szUser = LibWrapHelper.String2AnsiArr(m_strUser, 32);
            openSettings.szPassword = LibWrapHelper.String2AnsiArr(m_strPassword, 32);

            LibTDBWrap.TDB_Proxy_SETTINGS proxySetting = new LibTDBWrap.TDB_Proxy_SETTINGS();
            proxySetting.szProxyHostIp = LibWrapHelper.String2AnsiArr(m_strProxyIp, 64);
            proxySetting.szProxyPort = LibWrapHelper.String2AnsiArr(m_strProxyPort, 8);
            proxySetting.szProxyUser = LibWrapHelper.String2AnsiArr(m_strProxyUser, 32);
            proxySetting.szProxyPwd = LibWrapHelper.String2AnsiArr(m_strProxyPwd, 32);
            proxySetting.nProxyType = m_nProxyType;

            openSettings.nTimeOutVal = m_nTimeOutVal;
            openSettings.nRetryCount = m_nRetryCount;
            openSettings.nRetryGap = m_nRetryGap;

            IntPtr pUnmanagedOpenSettings = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LibTDBWrap.OPEN_SETTINGS)));
            Marshal.StructureToPtr(openSettings, pUnmanagedOpenSettings, false);

            IntPtr pUnmanagedProxySettings = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LibTDBWrap.TDB_Proxy_SETTINGS)));
            Marshal.StructureToPtr(proxySetting, pUnmanagedProxySettings, false);

            IntPtr pUnmanagedLoginRes = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_ResLogin)));

            m_hTdb = LibTDBWrap.TDB_OpenProxy(pUnmanagedOpenSettings, pUnmanagedProxySettings, pUnmanagedLoginRes);
            LibTDBWrap.TDBDefine_ResLogin loginRes = (LibTDBWrap.TDBDefine_ResLogin)Marshal.PtrToStructure(pUnmanagedLoginRes, typeof(LibTDBWrap.TDBDefine_ResLogin));

            Marshal.FreeHGlobal(pUnmanagedOpenSettings);
            Marshal.FreeHGlobal(pUnmanagedProxySettings);
            Marshal.FreeHGlobal(pUnmanagedLoginRes);

            m_loginResult = new TDBLoginResult();
            m_loginResult.FromAPILoginResult(loginRes);

            tdbLoginResult = (TDBLoginResult)m_loginResult.Clone();

            if ((UInt64)m_hTdb != 0)
            {
                return TDBErrNo.TDB_SUCCESS;
            }
            else
            {
                return TDBErrNo.TDB_OPEN_FAILED;
            }
        }

        //断开到数据源的连接
        public void DisConnect()
        {
            if ((UInt64)m_hTdb != 0)
            {
                LibTDBWrap.TDB_Close(m_hTdb);
                m_hTdb = (IntPtr)0;
            }
        }

        //获取某个市场或者全部市场的代码表。strMarket取值: "SH"、"SZ"、"CF"、"SHF"、"CZC"、"DCE"，全部市场：""
        public TDBErrNo GetCodeTable(string strMarket, out TDBCode[] codeArr)
        {
            TDBErrNo nVerifyRet = SimpleVerifyReqInput(strMarket);
            codeArr = new TDBCode[0];
            if (nVerifyRet != TDBErrNo.TDB_SUCCESS)
            {
                return nVerifyRet;
            }
            int nArrLen = 128;
            byte[] btMarketArr = LibWrapHelper.String2AnsiArr(strMarket, nArrLen);
            IntPtr pUnmanagedMarket = Marshal.AllocHGlobal(btMarketArr.Length);
            Marshal.Copy(btMarketArr, 0, pUnmanagedMarket, nArrLen);
            IntPtr ppCodeTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            IntPtr pCount = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));

            int nRet = LibTDBWrap.TDB_GetCodeTable(m_hTdb, pUnmanagedMarket, ppCodeTable, pCount);
            IntPtr pCodeTable = (IntPtr)Marshal.PtrToStructure(ppCodeTable, typeof(IntPtr));
            int nCount = (Int32)Marshal.PtrToStructure(pCount, typeof(Int32));

            if (nRet == 0 && (UInt64)pCodeTable != 0 && nCount > 0)
            {
                codeArr = new TDBCode[nCount];
                int nElemLen = Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_Code));
                for (int i = 0; i < nCount; i++)
                {
                    LibTDBWrap.TDBDefine_Code apiCode = (LibTDBWrap.TDBDefine_Code)Marshal.PtrToStructure((IntPtr)((UInt64)pCodeTable + (UInt64)(nElemLen * i)), typeof(LibTDBWrap.TDBDefine_Code));
                    codeArr[i] = new TDBCode();
                    codeArr[i].FromAPICode(ref apiCode);
                }
            }
            else
            {
                //如果网络连接断掉，则关闭连接
                if (nRet == (int)TDBErrNo.TDB_NETWORK_ERROR)
                {
                    DisConnect();
                }
            }

            if ((UInt16)pCodeTable != 0)
            {
                LibTDBWrap.TDB_Free(pCodeTable);
            }

            Marshal.FreeHGlobal(pUnmanagedMarket);
            Marshal.FreeHGlobal(ppCodeTable);
            Marshal.FreeHGlobal(pCount);
            return (TDBErrNo)nRet;
        }

        //获取K线
        public TDBErrNo GetKLine(TDBReqKLine reqKLine, out TDBKLine[] tdbKLine)
        {
            TDBErrNo nVerifyRet = SimpleVerifyReqInput(reqKLine);
            tdbKLine = new TDBKLine[0];
            if (nVerifyRet != TDBErrNo.TDB_SUCCESS)
            {
                return nVerifyRet;
            }

            LibTDBWrap.TDBDefine_ReqKLine reqKLineInner = reqKLine.ToAPIReqKLine();
            IntPtr pUnmanagedAPIReqK = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_ReqKLine)));
            Marshal.StructureToPtr(reqKLineInner, pUnmanagedAPIReqK, false);

            IntPtr ppKLine = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            IntPtr pCount = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));
            int nRet = LibTDBWrap.TDB_GetKLine(m_hTdb, pUnmanagedAPIReqK, ppKLine, pCount);
            IntPtr PKLine = (IntPtr)Marshal.PtrToStructure(ppKLine, typeof(IntPtr));
            int nCount = (int)Marshal.PtrToStructure(pCount, typeof(Int32));
            if ((UInt64)PKLine != 0 && nRet == 0 && nCount > 0)
            {
                tdbKLine = new TDBKLine[nCount];
                int nElemLen = Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_KLine));
                for (int i = 0; i < nCount; i++)
                {
                    LibTDBWrap.TDBDefine_KLine apiKLine = (LibTDBWrap.TDBDefine_KLine)Marshal.PtrToStructure((IntPtr)((UInt64)PKLine + (UInt64)(nElemLen * i)), typeof(LibTDBWrap.TDBDefine_KLine));
                    tdbKLine[i] = new TDBKLine();
                    tdbKLine[i].FromAPIKLine(ref apiKLine);
                }

            }
            else
            {
                //如果网络连接断掉，则关闭连接
                if (nRet == (int)TDBErrNo.TDB_NETWORK_ERROR)
                {
                    DisConnect();
                }
            }

            if ((UInt64)PKLine != 0)
            {
                LibTDBWrap.TDB_Free(PKLine);
            }

            Marshal.FreeHGlobal(pUnmanagedAPIReqK);
            Marshal.FreeHGlobal(ppKLine);
            Marshal.FreeHGlobal(pCount);

            return (TDBErrNo)nRet;
        }

        //获取普通股票的行情数据(不带买卖盘)，本接口不支持期货，对于期货（CF市场和商品期货），需要调用GetFuture或GetFutureAB
        public TDBErrNo GetTick(TDBReq reqTick, out TDBTick[] tdbTick)
        {
            TDBErrNo nVerifyRet = SimpleVerifyReqInput(reqTick);
            tdbTick = new TDBTick[0];
            if (nVerifyRet != TDBErrNo.TDB_SUCCESS)
            {
                return nVerifyRet;
            }

            LibTDBWrap.TDBDefine_ReqTick reqAPITick = reqTick.ToAPIReqTick();

            IntPtr pUnmanagedReqAPITick = LibWrapHelper.CopyStructToGlobalMem(reqAPITick, typeof(LibTDBWrap.TDBDefine_ReqTick));
            IntPtr ppTick = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            IntPtr pCount = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));

            int nRet = LibTDBWrap.TDB_GetTick(m_hTdb, pUnmanagedReqAPITick, ppTick, pCount);

            IntPtr pTick = (IntPtr)Marshal.PtrToStructure(ppTick, typeof(IntPtr));
            int nCount = (Int32)Marshal.PtrToStructure(pCount, typeof(Int32));
            if ((UInt64)pTick != 0 && nCount > 0 && nRet == 0)
            {
                tdbTick = new TDBTick[nCount];
                int nElemLen = Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_Tick));
                for (int i = 0; i < nCount; i++)
                {
                    IntPtr pCurRecord = (IntPtr)((UInt64)pTick + (UInt64)(nElemLen * i));
                    LibTDBWrap.TDBDefine_Tick apiTick = (LibTDBWrap.TDBDefine_Tick)Marshal.PtrToStructure(pCurRecord, typeof(LibTDBWrap.TDBDefine_Tick));
                    tdbTick[i] = new TDBTick();
                    tdbTick[i].FromAPITick(ref apiTick);
                }
            }
            else
            {
                //如果网络连接断掉，则关闭连接
                if (nRet == (int)TDBErrNo.TDB_NETWORK_ERROR)
                {
                    DisConnect();
                }
            }

            if ((UInt64)pTick != 0)
            {
                LibTDBWrap.TDB_Free(pTick);
            }

            Marshal.FreeHGlobal(pUnmanagedReqAPITick);
            Marshal.FreeHGlobal(ppTick);
            Marshal.FreeHGlobal(pCount);

            return (TDBErrNo)nRet;
        }

        //获取逐笔成交
        public TDBErrNo GetTransaction(TDBReq reqTransaction, out TDBTransaction[] tdbTransaction)
        {
            TDBErrNo nVerifyRet = SimpleVerifyReqInput(reqTransaction);
            tdbTransaction = new TDBTransaction[0];
            if (nVerifyRet != TDBErrNo.TDB_SUCCESS)
            {
                return nVerifyRet;
            }

            LibTDBWrap.TDBDefine_ReqTransaction reqAPITransaction = reqTransaction.ToAPIReqTransaction();

            IntPtr pUnmanagedReqAPITransaction = LibWrapHelper.CopyStructToGlobalMem(reqAPITransaction, typeof(LibTDBWrap.TDBDefine_ReqTransaction));
            IntPtr ppTransaction = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            IntPtr pCount = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));

            int nRet = LibTDBWrap.TDB_GetTransaction(m_hTdb, pUnmanagedReqAPITransaction, ppTransaction, pCount);

            IntPtr pTransaction = (IntPtr)Marshal.PtrToStructure(ppTransaction, typeof(IntPtr));
            int nCount = (Int32)Marshal.PtrToStructure(pCount, typeof(Int32));
            if ((UInt64)pTransaction != 0 && nCount > 0 && nRet == 0)
            {
                tdbTransaction = new TDBTransaction[nCount];
                int nElemLen = Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_Transaction));
                for (int i = 0; i < nCount; i++)
                {
                    IntPtr pCurRecord = (IntPtr)((UInt64)pTransaction + (UInt64)(nElemLen * i));
                    LibTDBWrap.TDBDefine_Transaction apiFuture = (LibTDBWrap.TDBDefine_Transaction)Marshal.PtrToStructure(pCurRecord, typeof(LibTDBWrap.TDBDefine_Transaction));
                    tdbTransaction[i] = new TDBTransaction();
                    tdbTransaction[i].FromAPITransaction(ref apiFuture);
                }
            }
            else
            {
                //如果网络连接断掉，则关闭连接
                if (nRet == (int)TDBErrNo.TDB_NETWORK_ERROR)
                {
                    DisConnect();
                }
            }

            if ((UInt64)pTransaction != 0)
            {
                LibTDBWrap.TDB_Free(pTransaction);
            }

            Marshal.FreeHGlobal(pUnmanagedReqAPITransaction);
            Marshal.FreeHGlobal(ppTransaction);
            Marshal.FreeHGlobal(pCount);

            return (TDBErrNo)nRet;
        }

        //委托队列
        public TDBErrNo GetOrderQueue(TDBReq reqOrderQueue, out TDBOrderQueue[] tdbOrderQueue)
        {
            TDBErrNo nVerifyRet = SimpleVerifyReqInput(reqOrderQueue);
            tdbOrderQueue = new TDBOrderQueue[0];
            if (nVerifyRet != TDBErrNo.TDB_SUCCESS)
            {
                return nVerifyRet;
            }

            LibTDBWrap.TDBDefine_ReqTransaction reqAPIOrderQueue = reqOrderQueue.ToAPIReqTransaction();

            IntPtr pUnmanagedReqAPIOrderQueue = LibWrapHelper.CopyStructToGlobalMem(reqAPIOrderQueue, typeof(LibTDBWrap.TDBDefine_ReqTransaction));
            IntPtr ppOrderQueue = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            IntPtr pCount = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));

            int nRet = LibTDBWrap.TDB_GetOrderQueue(m_hTdb, pUnmanagedReqAPIOrderQueue, ppOrderQueue, pCount);

            IntPtr pOrderQueue = (IntPtr)Marshal.PtrToStructure(ppOrderQueue, typeof(IntPtr));
            int nCount = (Int32)Marshal.PtrToStructure(pCount, typeof(Int32));
            if ((UInt64)pOrderQueue != 0 && nCount > 0 && nRet == 0)
            {
                tdbOrderQueue = new TDBOrderQueue[nCount];
                int nElemLen = Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_OrderQueue));
                for (int i = 0; i < nCount; i++)
                {
                    IntPtr pCurRecord = (IntPtr)((UInt64)pOrderQueue + (UInt64)(nElemLen * i));
                    LibTDBWrap.TDBDefine_OrderQueue apiOrderQueue = (LibTDBWrap.TDBDefine_OrderQueue)Marshal.PtrToStructure(pCurRecord, typeof(LibTDBWrap.TDBDefine_OrderQueue));
                    tdbOrderQueue[i] = new TDBOrderQueue();
                    tdbOrderQueue[i].FromAPIOrderQueue(ref apiOrderQueue);
                }
            }
            else
            {
                //如果网络连接断掉，则关闭连接
                if (nRet == (int)TDBErrNo.TDB_NETWORK_ERROR)
                {
                    DisConnect();
                }
            }

            if ((UInt64)pOrderQueue != 0)
            {
                LibTDBWrap.TDB_Free(pOrderQueue);
            }

            Marshal.FreeHGlobal(pUnmanagedReqAPIOrderQueue);
            Marshal.FreeHGlobal(ppOrderQueue);
            Marshal.FreeHGlobal(pCount);

            return (TDBErrNo)nRet;
        }

        //逐笔委托
        public TDBErrNo GetOrder(TDBReq reqOrder, out TDBOrder[] tdbOrder)
        {
            TDBErrNo nVerifyRet = SimpleVerifyReqInput(reqOrder);
            tdbOrder = new TDBOrder[0];
            if (nVerifyRet != TDBErrNo.TDB_SUCCESS)
            {
                return nVerifyRet;
            }

            LibTDBWrap.TDBDefine_ReqTransaction reqAPIOrder = reqOrder.ToAPIReqTransaction();

            IntPtr pUnmanagedReqAPIOrder = LibWrapHelper.CopyStructToGlobalMem(reqAPIOrder, typeof(LibTDBWrap.TDBDefine_ReqTransaction));
            IntPtr ppOrder = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
            IntPtr pCount = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)));

            int nRet = LibTDBWrap.TDB_GetOrder(m_hTdb, pUnmanagedReqAPIOrder, ppOrder, pCount);

            IntPtr pOrder = (IntPtr)Marshal.PtrToStructure(ppOrder, typeof(IntPtr));
            int nCount = (Int32)Marshal.PtrToStructure(pCount, typeof(Int32));
            if ((UInt64)pOrder != 0 && nCount > 0 && nRet == 0)
            {
                tdbOrder = new TDBOrder[nCount];
                int nElemLen = Marshal.SizeOf(typeof(LibTDBWrap.TDBDefine_Order));
                for (int i = 0; i < nCount; i++)
                {
                    IntPtr pCurRecord = (IntPtr)((UInt64)pOrder + (UInt64)(nElemLen * i));
                    LibTDBWrap.TDBDefine_Order apiOrder = (LibTDBWrap.TDBDefine_Order)Marshal.PtrToStructure(pCurRecord, typeof(LibTDBWrap.TDBDefine_Order));
                    tdbOrder[i] = new TDBOrder();
                    tdbOrder[i].FromAPIOrder(ref apiOrder);
                }
            }
            else
            {
                //如果网络连接断掉，则关闭连接
                if (nRet == (int)TDBErrNo.TDB_NETWORK_ERROR)
                {
                    DisConnect();
                }
            }

            if ((UInt64)pOrder != 0)
            {
                LibTDBWrap.TDB_Free(pOrder);
            }

            Marshal.FreeHGlobal(pUnmanagedReqAPIOrder);
            Marshal.FreeHGlobal(ppOrder);
            Marshal.FreeHGlobal(pCount);

            return (TDBErrNo)nRet;
        }

        //如果查询的代码不存在，连接已经断掉、未连接，则返回null
        public TDBCode GetCodeInfo(string strWindCode, string strMarketKey)
        {
            TDBErrNo nVerifyRet = SimpleVerifyReqInput(strWindCode);
            if (nVerifyRet != TDBErrNo.TDB_SUCCESS)
            {
                return null;
            }
            int nMaxWindCodeLen = 64;
            int nMaxmarketLen = 48;
            IntPtr pszWindCode = Marshal.AllocHGlobal(nMaxWindCodeLen);
            byte[] btWindCode = LibWrapHelper.String2AnsiArr(strWindCode, nMaxWindCodeLen);
            btWindCode[btWindCode.Length - 1] = 0;
            Marshal.Copy(btWindCode, 0, pszWindCode, btWindCode.Length);

            IntPtr pszMarket = Marshal.AllocHGlobal(nMaxmarketLen);
            byte[] btMarket = LibWrapHelper.String2AnsiArr(strMarketKey, nMaxmarketLen);
            btMarket[btMarket.Length - 1] = 0;
            Marshal.Copy(btMarket, 0, pszMarket, btMarket.Length);

            IntPtr pCode = LibTDBWrap.TDB_GetCodeInfo(m_hTdb, pszWindCode, pszMarket);
            Marshal.FreeHGlobal(pszWindCode);
            Marshal.FreeHGlobal(pszMarket);
            if ((UInt64)pCode != 0)
            {
                LibTDBWrap.TDBDefine_Code apiCode = (LibTDBWrap.TDBDefine_Code)Marshal.PtrToStructure(pCode, typeof(LibTDBWrap.TDBDefine_Code));
                TDBCode tdbCode = new TDBCode();
                tdbCode.FromAPICode(ref apiCode);
                return tdbCode;
            }
            else
            {
                return null;
            }
        }

        //////////////////////////
        private TDBErrNo SimpleVerifyReqInput(object reqObj)
        {
            if (reqObj == null)
            {
                return TDBErrNo.TDB_INVALID_PARAMS;
            }

            if ((UInt64)m_hTdb == 0)
            {
                return TDBErrNo.TDB_NETWORK_ERROR;
            }

            return TDBErrNo.TDB_SUCCESS;
        }
        private string m_strIP;
        private string m_strPort;
        private string m_strUser;
        private string m_strPassword;
        private string m_strProxyIp;
        private string m_strProxyPort;
        private string m_strProxyUser;
        private string m_strProxyPwd;
        private int m_nProxyType;
        private int m_nTimeOutVal;
        private int m_nRetryCount;
        private int m_nRetryGap;

        private IntPtr m_hTdb;
        private TDBLoginResult m_loginResult;
    }
}
