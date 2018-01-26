using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GroupClient
{

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void NotifyKnockInfoDelegate(string KnockInfo);

    public static class ImsApi
    {

        static bool IsOpened = false;
        //static bool IsConnected = false;
        static NotifyKnockInfoDelegate ac;

        public static void Init(Action<string> OnNotifyInfo)
        {
            var utaConfig = CommonUtils.GetConfig("UAT");
            if (string.IsNullOrEmpty(utaConfig) || !Regex.IsMatch(utaConfig, "[0-9A-Za-z.]+:[0-9]+"))
            {
                CommonUtils.Log("IMS配置异常，UTA未配置");
                return;
            }

            string[] ipPort = utaConfig.Split(':');
            string ip = ipPort[0];
            ushort port = Convert.ToUInt16(ipPort[1]);

            StringBuilder ErrInfo = new StringBuilder(256);

            
            //1.打开PB
            while (!IsOpened)
            {
                IsOpened = ImsPbClient_Open(ErrInfo);
                if (!IsOpened)
                {
                    CommonUtils.Log("Ims Open Fail, try login 1 second later");
                    Thread.Sleep(1000);
                }
            }


            //2.设置通知函数
            ac = new NotifyKnockInfoDelegate(OnNotifyInfo);
            GC.KeepAlive(ac);

            try
            {
                //3.连接服务器
                bool isConnected = ImsPbClient_Connect(ip, port, ErrInfo);
                if (!isConnected)
                {
                    CommonUtils.Log("ImsApi.ImsPbClient_Connect Error {0}", ErrInfo.ToString());
                    return;
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log(" IMS API Init Exception, {0}", ex.Message);
            }
        }

        public static void Close()
        {
            if (IsOpened)
            {
                try
                {
                    //断开连接
                    ImsPbClient_Disconnect();
                    //关闭PB
                    ImsPbClient_Close();
                }
                catch (Exception)
                {
                    
                    throw;
                }
               
            }
        }

        public static bool Login(string userName, string password, string version, out string errMsg)
        {
            errMsg = string.Empty;

            StringBuilder ErrInfo = new StringBuilder(1024);
            string pubIP = "61.177.128.170";// CommonUtils.GetPubIP();
            if (string.IsNullOrEmpty(pubIP))
            {
                CommonUtils.Log("ImsApi Get ip address in intranet fail!");
                return false;
            }

                string mac = CommonUtils.GetMacAddress();
                if (string.IsNullOrEmpty(mac))
                {
                    CommonUtils.Log("ImsApi Get local mac  fail!");
                    return false;
                }

                string hdID = CommonUtils.GetHdID();
                if (string.IsNullOrEmpty(hdID))
                {
                    CommonUtils.Log("ImsApi Get local hdID  fail!");
                    return false;
                }

                string cpuID = CommonUtils.GetCpuID();
                if (string.IsNullOrEmpty(cpuID))
                {
                    CommonUtils.Log("ImsApi Get local cpuID  fail!");
                    return false;
                }

            //4.登录 登录id及password如何保存
            bool isLogin = ImsPbClient_Login(userName, password, pubIP, mac, hdID, cpuID, version, ErrInfo);
            if (!isLogin)
            {
                errMsg = ErrInfo.ToString();
                CommonUtils.Log("ImsApi login  fail! Message {0}", errMsg);
                return false;
            }
            return isLogin;
        }


        /// <summary>
        /// 设置通知函数
        /// </summary>
        /// <param name="NotifyKnockInfoFunc"></param>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        public static extern void ImsPbClient_SetNotifyKnockInfoFunc(NotifyKnockInfoDelegate NotifyKnockInfoFunc);


        /// <summary>
        /// 打开PB, 整个应用程序只能成功执行一次,如果执行失败, 必须重复执行直至成功
        /// </summary>
        /// <param name="ErrInfo">返回的失败信息</param>
        /// <returns>成功 true    失败false</returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_Open(StringBuilder ErrInfo);


        /// <summary>
        /// 关闭PB
        /// </summary>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        public static extern void ImsPbClient_Close();



        /// <summary>
        /// 连接交易服务器
        /// </summary>
        /// <param name="IP">服务器地址</param>
        /// <param name="Port">服务器端口</param>
        /// <param name="ErrInfo">出错信息</param>
        /// <returns>成功 true    失败false</returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_Connect(string IP, ushort Port, StringBuilder ErrInfo);


        /// <summary>
        /// 断开交易服务器
        /// </summary>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        public static extern void ImsPbClient_Disconnect();




        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="User">用户名</param>
        /// <param name="Password">密码</param>
        /// <param name="IP">设置登录的IP</param>
        /// <param name="MAC">设置登录的MAC</param>
        /// <param name="LocalHD">硬盘号</param>
        /// <param name="CPU">CPU号</param>
        /// <param name="Version">券商IMS客户端当前版本号</param>
        /// <param name="ErrInfo">同上</param>
        /// <returns>同上</returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_Login(string User, string Password, string IP, string MAC, string LocalHD, string CPU, string Version, StringBuilder ErrInfo);


        /// <summary>
        /// 普通查询
        /// </summary>
        /// <param name="Category">查询类别,   0查询当日现货委托    1查询当日现货成交</param>
        /// <param name="RowCountPerPage">查询结果分页, 每页多少条数据</param>
        /// <param name="PageNum">返回分页后的第几页结果,  从1开始</param>
        /// <param name="Result">返回的查询结果 \n \t分隔的表格数据</param>
        /// <param name="ErrInfo">同上</param>
        /// <returns>同上</returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_CommonQuery(int Category, int RowCountPerPage, int PageNum, StringBuilder Result, StringBuilder ErrInfo);



        /// <summary>
        /// 查询委托
        /// </summary>
        /// <param name="lastupdatetime">每个委托带有lastupdatetime属性, 从此lastupdatetime的值开始查委托, 空字符串表示从最开始开始查</param>
        /// <param name="Result">同上</param>
        /// <param name="ErrInfo">同上</param>
        /// <returns></returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_QueryProductOrder(string lastupdatetime, StringBuilder Result, StringBuilder ErrInfo);

        /// <summary>
        /// 查询成交
        /// </summary>
        /// <param name="serialnum">每个成交带有serialnum属性, 从此serialnum的值开始查成交, 0表示从最开始开始查</param>
        /// <param name="Result">同上</param>
        /// <param name="ErrInfo">同上</param>
        /// <returns></returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_QueryProductKnock(int serialnum, StringBuilder Result, StringBuilder ErrInfo);




        /// <summary>
        /// 查询产品信息
        /// </summary>
        /// <param name="Category">查询类别,   0产品信息     1资产单元    2投资组合    3交易员</param>
        /// <param name="Result">同上</param>
        /// <param name="ErrInfo">同上</param>
        /// <returns>同上</returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_RequestProductList(int Category, StringBuilder Result, StringBuilder ErrInfo);




        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="产品信息">产品信息ID</param>
        /// <param name="资产单元">资产单元ID</param>
        /// <param name="投资组合">投资组合ID</param>
        /// <param name="交易市场">上海0    深圳1    港股H    沪港通0H      沪股通H0      深港通1H         深股通H1</param>
        /// <param name="证券代码"></param>
        /// <param name="买卖方向">买B     卖S</param>
        /// <param name="下单股数"></param>
        /// <param name="价格"></param>
        /// <param name="价格类型">限价单LIMIT     市价单ANY</param>
        /// <param name="市价类型"></param>
        /// 上海最优五档剩撤买 VB      上海最优五档剩转买 WB     上海最优五档剩撤卖 VS    上海最优五档剩转卖 WS
        /// 深圳即时成交买 2B         深圳最优五档买VB    深圳全额成交买WB     本方最优价格买XB   对手最优价格买YB
        /// 深圳即时成交卖 2S         深圳最优五档卖VS    深圳全额成交卖WS    本方最优价格卖XS   对手最优价格卖YS
        /// L 香港限价盘              S 香港特别限价盘        E 香港增强限价盘        A 香港竞价盘        I 香港竞价限价盘        ML 香港收盘限价盘        MC 香港收盘竞价盘
        /// <param name="备注"></param>
        /// <param name="Result">合同号</param>
        /// <param name="ErrInfo">同上</param>
        /// <returns>同上</returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_RequestNormalOrder(string 产品信息, string 资产单元, string 投资组合, string 交易市场, string 证券代码, string 买卖方向, int 下单股数, double 价格, string 价格类型, string 市价类型, string 备注, StringBuilder Result, StringBuilder ErrInfo);



        /// <summary>
        /// 撤单
        /// </summary>
        /// <param name="资产单元">资产单元ID</param>
        /// <param name="投资组合">投资组合ID</param>
        /// <param name="交易市场">上海0    深圳1</param>
        /// <param name="证券代码"></param>
        /// <param name="合同号">要撤的单子的合同号</param>
        /// <param name="序列号">保留,填空串</param>
        /// <param name="Result">撤的单子的合同号</param>
        /// <param name="ErrInfo">同上</param>
        /// <returns>同上</returns>
        [DllImport("ImsPbTrade.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool ImsPbClient_CancelNormalOrder(string 资产单元, string 投资组合, string 交易市场, string 证券代码, string 合同号, string 序列号, StringBuilder Result, StringBuilder ErrInfo);
    }
}
