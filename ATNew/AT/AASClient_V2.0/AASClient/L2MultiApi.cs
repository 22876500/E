using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AASClient
{
    public class L2MultiApi
    {
        #region Members
        
        static object sync = new object();

        static L2MultiApi _instance;
        
        #endregion

        #region Properties
        public static L2MultiApi Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new L2MultiApi();
                        }
                    }
                }
                return _instance;
            }
        }

        public static bool IsOpened { get; private set; }

        public int ConnectionID { get; private set; } 
        #endregion

        private L2MultiApi()
        {
            ConnectionID = int.MinValue;
        }


        public bool Open()
        {
            try
            {
                StringBuilder sb = new StringBuilder(1024 * 16);
                bool isOpenSucces = OpenTdx(sb);
                if (!isOpenSucces) Program.logger.LogRunning("Tdx L2MultiApi Open 失败! ErrorInfo:{0}", sb.ToString());
                IsOpened = isOpenSucces;
                return isOpenSucces;
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("Tdx L2MultiApi Open 失败! Message:{0}, StackTrace:{1}", ex.Message, ex.StackTrace);
                return false;
            }
        }

        public void Close()
        {
            try
            {
                CloseTdx();
            }
            catch (Exception)
            {
                
            }
            
        }   

        public bool Connect(string IP, int Port, StringBuilder Result, StringBuilder ErrInfo)
        {
            try
            {
                if (!IsOpened)
                {
                    Open();
                }
                int id = TdxExHq_Multi_Connect(IP, Port, Result, ErrInfo);
                if (ErrInfo.ToString() == string.Empty)
                {
                    ConnectionID = id;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Disconnect()
        {
            if (ConnectionID != int.MinValue)
            {
                TdxExHq_Multi_Disconnect(ConnectionID);
                ConnectionID = int.MinValue;
            }
        }

        public bool GetMarkets(StringBuilder Result, StringBuilder ErrInfo)
        {
            if (ConnectionID != int.MinValue)
            {
                return TdxExHq_Multi_GetMarkets(ConnectionID, Result, ErrInfo);
            }
            else
            {
                ErrInfo.Append("未连接到服务器!");
                return false;
            }
        }


        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I4)]
        static extern int TdxExHq_Multi_Connect(string IP, int Port, StringBuilder Result, StringBuilder ErrInfo);//连接券商行情服务器


        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        static extern void TdxExHq_Multi_Disconnect(int ConnectionID);//断开服务器


        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool TdxExHq_Multi_GetMarkets(int ConnectionID, StringBuilder Result, StringBuilder ErrInfo);//获取所有市场代码


        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool TdxExHq_Multi_GetInstrumentInfo(int ConnectionID, int Start, short Count, StringBuilder Result, StringBuilder ErrInfo);//获取所有品种代码

        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool OpenTdx(StringBuilder ErrInfo);

        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        static extern void CloseTdx();
        // bool  OpenTdx(char* ErrInfo);//打开通达信
        // void  CloseTdx();//关闭通达信

    }
}
