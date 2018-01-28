//using hundsun.t2sdk;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class HsClient
    {
        //private CT2Connection connection;

        //HsCT2CallbackInterface hsCallback = new HsCT2CallbackInterface();


        object SynObject = new object();

        public void Open(string IP, short Port, out string ErrInfo)
        {
            ErrInfo = string.Empty;
            //CT2Configinterface TradeConfig = new CT2Configinterface();
            //TradeConfig.SetString("t2sdk", "servers", string.Format("{0}:{1}", IP, Port));//117.78.5.224    183.6.169.83报单端口9003  设置字符串型属性值到对应的节点和变量中。返回0表示设置成功，其他表示失败。
            //TradeConfig.SetString("t2sdk", "license_file", "license.dat");
            //TradeConfig.SetString("t2sdk", "auto_reconnect", "1");
            //TradeConfig.SetString("t2sdk", "support_multi", "1");
            //this.connection = new CT2Connection(TradeConfig);
            //this.connection.Create2BizMsg(this.hsCallback);//返回0表示成功，否则表示失败，通过调用GetErrorMsg可以获取详细错误信息
            //int ErrCode = this.connection.Connect(5000);//返回0表示成功，否则表示失败，通过调用GetErrorMsg可以获取详细错误信息
            //if (ErrCode != 0)
            //{
            //    ErrInfo = this.connection.GetErrorMsg(ErrCode);
            //    this.connection.Close();
            //    return;
            //}
            return;
        }

        public void Close()
        {
            //this.connection.Close();
        }

        public string Logon(string UserName, string Password, string 登录IP, string MAC, string HDD, out string ErrInfo)
        {
            //CT2Packer Packer1 = new CT2Packer(2);
            //Packer1.BeginPack();
            //Packer1.AddField("operator_no", Convert.ToSByte('S'), 16, 4);//字段长度是指当前字段最大长度，在添加值的时候，超出最大长度，就会截断；字段精度只用于类型为F的时候。
            //Packer1.AddField("password", Convert.ToSByte('S'), 16, 4);//：I整数，F浮点数，C字符，S字符串，R任意二进制数据
            //Packer1.AddField("mac_address", Convert.ToSByte('S'), 32, 4);
            //Packer1.AddField("ip_address", Convert.ToSByte('S'), 32, 4);
            //Packer1.AddField("hd_volserial", Convert.ToSByte('S'), 10, 4);
            //Packer1.AddField("op_station", Convert.ToSByte('S'), 255, 4);
            //Packer1.AddField("authorization_id", Convert.ToSByte('S'), 64, 4);
            //Packer1.AddField("login_time", Convert.ToSByte('S'), 6, 4);
            //Packer1.AddField("verification_code", Convert.ToSByte('S'), 32, 4);


            ////要严格按照   IP:xx.xx.xx.xx,MAC:xx-xx-xx-xx-xx-xx,HDD:具体硬盘序列号，具体的ip是10.38.20.47，mac是68-05-CA-28-A1-B4，HDD是FD8394F4



            //Packer1.AddStr(UserName);
            //Packer1.AddStr(Password);
            //Packer1.AddStr(MAC);
            //Packer1.AddStr(登录IP);
            //Packer1.AddStr(HDD);
            //Packer1.AddStr("op_station");
            //Packer1.AddStr("");
            //Packer1.AddStr("");
            //Packer1.AddStr("");
            //Packer1.EndPack();



            //DataTable Result;
            //this.GetResult(10001, Packer1, out Result, out ErrInfo);//返回发送句柄（正数），否则表示失败，通过调用GetErrorMsg可以获取详细错误信息
            //if (ErrInfo != string.Empty)
            //{
            //    return string.Empty;
            //}
            //else
            //{
            //    return Result.Rows[0]["user_token"] as string;
            //}
            ErrInfo = string.Empty;
            return null;
        }

        public void Logout(string user_token)
        {
            //string ErrInfo;
            //CT2Packer Packer1 = new CT2Packer(2);
            //Packer1.BeginPack();
            //Packer1.AddField("user_token", Convert.ToSByte('S'), 512, 4);
            //Packer1.AddStr(user_token);
            //Packer1.EndPack();
            //DataTable Result;
            //this.GetResult(10002, Packer1, out Result, out ErrInfo);//返回发送句柄（正数），否则表示失败，通过调用GetErrorMsg可以获取详细错误信息
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="组合"></param>
        /// <param name="买卖方向">1买     2卖</param>
        /// <param name="价格类型">0限价   </param>
        /// <param name="市场">1上海  2深圳</param>
        /// <param name="证券代码"></param>
        /// <param name="价格"></param>
        /// <param name="数量"></param>
        /// <returns></returns>
        public void SendOrder(string user_token, string 帐户, string 资产单元, string 组合, string 买卖方向, string 价格类型, string 市场, string 证券代码, double 价格, double 数量, out DataTable Result, out string ErrInfo)
        {
            //CT2Packer Packer1 = new CT2Packer(2);

            //Packer1.BeginPack();
            //Packer1.AddField("user_token", Convert.ToSByte('S'), 512, 4);
            //if (帐户 != null)
            //{
            //    Packer1.AddField("account_code", Convert.ToSByte('S'), 32, 4);
            //}
            //if (资产单元 != null)
            //{
            //    Packer1.AddField("asset_no", Convert.ToSByte('S'), 16, 4);
            //}
            //if (组合 != null)
            //{
            //    Packer1.AddField("combi_no", Convert.ToSByte('S'), 8, 4);
            //}
            //Packer1.AddField("market_no", Convert.ToSByte('S'), 3, 4);
            //Packer1.AddField("stock_code", Convert.ToSByte('S'), 16, 4);
            //Packer1.AddField("entrust_direction", Convert.ToSByte('S'), 3, 4);
            //Packer1.AddField("price_type", Convert.ToSByte('S'), 1, 4);
            //Packer1.AddField("entrust_price", Convert.ToSByte('F'), 11, 4);
            //Packer1.AddField("entrust_amount", Convert.ToSByte('F'), 16, 2);

            //Packer1.AddStr(user_token);
            //if (帐户 != null)
            //{
            //    Packer1.AddStr(帐户);
            //}
            //if (资产单元 != null)
            //{
            //    Packer1.AddStr(资产单元);
            //}
            //if (组合 != null)
            //{
            //    Packer1.AddStr(组合);
            //}
            //Packer1.AddStr(市场);
            //Packer1.AddStr(证券代码);
            //Packer1.AddStr(买卖方向);
            //Packer1.AddStr(价格类型);//0 限价未做
            //Packer1.AddDouble(价格);
            //Packer1.AddDouble(数量);
            //Packer1.EndPack();
            //this.GetResult(91001, Packer1, out Result, out ErrInfo);//返回发送句柄（正数），否则表示失败，通过调用GetErrorMsg可以获取详细错误信息
            Result = new DataTable();
            ErrInfo = string.Empty;
        }

        public void CancelOrder(string user_token, int 委托编号, out DataTable Result, out string ErrInfo)
        {
            //CT2Packer packer = new CT2Packer(2);
            //packer.BeginPack();
            //packer.AddField("user_token", Convert.ToSByte('S'), 512, 4);
            //packer.AddField("entrust_no", Convert.ToSByte('I'), 8, 4);

            //packer.AddStr(user_token);
            //packer.AddInt(委托编号);
            //packer.EndPack();




            //this.GetResult(91101, packer, out Result, out ErrInfo);//返回发送句柄（正数），否则表示失败，通过调用GetErrorMsg可以获取详细错误信息

            //string success_flag = Result.Rows[0]["success_flag"] as string;
            //if (success_flag == "2")
            //{
            //    ErrInfo = Result.Rows[0]["fail_cause"] as string;
            //    return;
            //}
            Result = new DataTable();
            ErrInfo = string.Empty;
        }

        public void QryData(string user_token, int FuncID, string 帐户, string 资产单元, string 组合, out DataTable Result, out string ErrInfo)
        {
            //CT2Packer packer = new CT2Packer(2);
            //packer.BeginPack();
            //packer.AddField("user_token", Convert.ToSByte('S'), 512, 4);
            //if (帐户 != null)
            //{
            //    packer.AddField("account_code", Convert.ToSByte('S'), 32, 4);
            //}
            //if (资产单元 != null)
            //{
            //    packer.AddField("asset_no", Convert.ToSByte('S'), 16, 4);
            //}
            //if (组合 != null)
            //{
            //    packer.AddField("combi_no", Convert.ToSByte('S'), 8, 4);
            //}
            //packer.AddStr(user_token);
            //if (帐户 != null)
            //{
            //    packer.AddStr(帐户);
            //}
            //if (资产单元 != null)
            //{
            //    packer.AddStr(资产单元);
            //}
            //if (组合 != null)
            //{
            //    packer.AddStr(组合);
            //}
            //packer.EndPack();
            //this.GetResult(FuncID, packer, out Result, out ErrInfo);//返回发送句柄（正数），否则表示失败，通过调用GetErrorMsg可以获取详细错误信息
            Result = new DataTable();
            ErrInfo = string.Empty;
        }

        //void GetResult(int FuncID, CT2Packer Packer1, out DataTable Result, out string ErrInfo)
        //{
        //    lock (this.SynObject)
        //    {
        //        Result = null;
        //        ErrInfo = string.Empty;


        //        int hSendHandle = -1;
        //        using (CT2BizMessage reqMessage = new CT2BizMessage())
        //        {
        //            reqMessage.SetFunction(FuncID);//设置功能号
        //            reqMessage.SetPacketType(0);//设置消息类型为请求

        //            using (Packer1)
        //            {
        //                unsafe
        //                {
        //                    reqMessage.SetContent(Packer1.GetPackBuf(), Packer1.GetPackLen());
        //                }

        //                hSendHandle = connection.SendBizMsg(reqMessage, 0);//返回发送句柄（正数），否则表示失败，通过调用GetErrorMsg可以获取详细错误信息
        //                if (hSendHandle <= 0)
        //                {
        //                    ErrInfo = connection.GetErrorMsg(hSendHandle);
        //                    return;
        //                }
        //            }
        //        }
        //        CT2BizMessage AnsBizMessage = null;//外部所指向的消息对象的内存由SDK内部管理，外部切勿释放！

        //        int ErrCode = connection.RecvBizMsg(hSendHandle, out AnsBizMessage, 5000, 1);//返回0表示成功，否则表示失败，通过调用GetErrorMsg可以获取详细错误信息  	uiFlag 1表示当接收超时后，把hSend相关数据删除
        //        if (ErrCode != 0)
        //        {
        //            ErrInfo = connection.GetErrorMsg(ErrCode);
        //            return;
        //        }
        //        if (AnsBizMessage == null)
        //        {
        //            ErrInfo = "未收到应答消息";
        //            return;
        //        }
        //        ErrCode = AnsBizMessage.GetErrorNo();//获取错误码      一般用于客户端在收到应答之后，根据错误号来判断之前的请求是否正确处理                              
        //        if (ErrCode != 0)
        //        {
        //            ErrInfo = AnsBizMessage.GetErrorInfo();
        //            return;
        //        }
        //        //Console.WriteLine("ReturnCode={0}", AnsBizMessage.GetReturnCode());//获取返回码 一般用于客户端在收到应答之后，获取对应返回码，做不同的处理
        //        unsafe
        //        {
        //            int iLen = 0;
        //            void* buffer = AnsBizMessage.GetContent(&iLen);
        //            using (CT2UnPacker UnPacker1 = new CT2UnPacker(buffer, (uint)iLen))
        //            {
        //                //DataSet DataSet1 = new DataSet();
        //                //for (int i = 0; i < UnPacker1.GetDatasetCount();i++ )
        //                //{
        //                //    DataSet1.Tables.Add(this.GetDataTable(UnPacker1, i));
        //                //}
        //                UnPacker1.SetCurrentDatasetByIndex(0);//ErrorCode ErrorMsg MsgDetail  DataCount
        //                UnPacker1.First();
        //                ErrCode = UnPacker1.GetInt("ErrorCode");
        //                if (ErrCode != 0)//0 成功      <0失败   >0表示成功但有提示信息
        //                {
        //                    ErrInfo = UnPacker1.GetStr("ErrorMsg");
        //                    return;
        //                }

        //                int DataCount = UnPacker1.GetInt("DataCount");
        //                if (DataCount == 0)
        //                {
        //                    ErrInfo = "无数据";
        //                    return;
        //                }
        //                if (UnPacker1.GetDatasetCount() == 1)
        //                {
        //                    ErrInfo = "无数据";
        //                    return;
        //                }
        //                Result = this.GetDataTable(UnPacker1, 1);
        //                return;
        //            }
        //        }
        //    }
        //}


        //DataTable GetDataTable(CT2UnPacker UnPacker1, int index)
        //{
        //    UnPacker1.SetCurrentDatasetByIndex(index);
        //    UnPacker1.First();

        //    DataTable Result = new DataTable();
        //    for (int j = 0; j < UnPacker1.GetColCount(); j++)
        //    {
        //        Result.Columns.Add(UnPacker1.GetColName(j));
        //    }


        //    for (int k = 0; k < UnPacker1.GetRowCount(); k++)
        //    {

        //        DataRow DataRow1 = Result.NewRow();
        //        for (int t = 0; t < UnPacker1.GetColCount(); t++)
        //        {
        //            #region 添加每一行
        //            switch (UnPacker1.GetColType(t))
        //            {
        //                case (sbyte)'I':  //I 整数
        //                    DataRow1[t] = UnPacker1.GetIntByIndex(t).ToString();
        //                    break;
        //                case (sbyte)'C':  //C 
        //                    DataRow1[t] = ((char)UnPacker1.GetCharByIndex(t)).ToString();
        //                    break;
        //                case (sbyte)'S':   //S
        //                    DataRow1[t] = UnPacker1.GetStrByIndex(t);
        //                    break;
        //                case (sbyte)'F':  //F
        //                    DataRow1[t] = UnPacker1.GetDoubleByIndex(t).ToString();
        //                    break;
        //                case (sbyte)'R':  //R
        //                    DataRow1[t] = string.Empty;
        //                    break;
        //                default:
        //                    DataRow1[t] = string.Empty;
        //                    break;
        //            }
        //            #endregion
        //        }

        //        Result.Rows.Add(DataRow1);
        //        UnPacker1.Next();
        //    }

        //    return Result;

        //}
    }





    //public unsafe class HsCT2CallbackInterface : CT2CallbackInterface
    //{
    //    public override void OnClose(CT2Connection lpConnection)
    //    {
    //        Console.WriteLine("OnClose");
    //    }

    //    public override void OnConnect(CT2Connection lpConnection)
    //    {
    //        Console.WriteLine("OnConnect");
    //    }

    //    public override void OnReceivedBiz(CT2Connection lpConnection, int hSend, string lppStr, CT2UnPacker lppUnPacker, int nResult)
    //    {
    //        Console.WriteLine("OnReceivedBiz {0}", hSend);
    //    }

    //    public override void OnReceivedBizEx(CT2Connection lpConnection, int hSend, CT2RespondData lpRetData, string lppStr, CT2UnPacker lppUnPacker, int nResult)
    //    {
    //        Console.WriteLine("OnReceivedBizEx {0}", hSend);
    //    }

    //    public override void OnReceivedBizMsg(CT2Connection lpConnection, int hSend, CT2BizMessage AnsBizMessage)
    //    {
    //        Console.WriteLine("OnReceivedBizMsg {0}", hSend);

    //        //Program.logger.LogInfo("OnReceivedBizMsg {0}", AnsBizMessage.GetFunction());

    //        //string ErrInfo;
    //        //int ErrCode = AnsBizMessage.GetErrorNo();//获取错误码      一般用于客户端在收到应答之后，根据错误号来判断之前的请求是否正确处理                              
    //        //if (ErrCode != 0)
    //        //{
    //        //    ErrInfo = AnsBizMessage.GetErrorInfo();
    //        //    return;
    //        //}



    //        //Console.WriteLine("ReturnCode={0}", AnsBizMessage.GetReturnCode());//获取返回码 一般用于客户端在收到应答之后，获取对应返回码，做不同的处理






    //        //unsafe
    //        //{
    //        //    int iLen = 0;

    //        //    using (CT2UnPacker UnPacker1 = new CT2UnPacker(AnsBizMessage.GetContent(&iLen), (uint)iLen))
    //        //    {
    //        //        UnPacker1.SetCurrentDatasetByIndex(0);
    //        //        UnPacker1.First();

    //        //        ErrCode = UnPacker1.GetInt("ErrorCode");
    //        //        if (ErrCode != 0)
    //        //        {
    //        //            ErrInfo = UnPacker1.GetStr("ErrorMsg");
    //        //            return;
    //        //        }



    //        //        UnPacker1.SetCurrentDatasetByIndex(1);
    //        //        UnPacker1.First();

    //        //        DataTable Result = new DataTable();
    //        //        for (int j = 0; j < UnPacker1.GetColCount(); j++)
    //        //        {
    //        //            Result.Columns.Add(UnPacker1.GetColName(j));
    //        //        }



    //        //        for (int k = 0; k < UnPacker1.GetRowCount(); k++)
    //        //        {

    //        //            DataRow DataRow1 = Result.NewRow();
    //        //            for (int t = 0; t < UnPacker1.GetColCount(); t++)
    //        //            {
    //        //                #region 添加每一行
    //        //                switch (UnPacker1.GetColType(t))
    //        //                {
    //        //                    case (sbyte)'I':  //I 整数
    //        //                        DataRow1[t] = UnPacker1.GetIntByIndex(t).ToString();
    //        //                        break;
    //        //                    case (sbyte)'C':  //C 
    //        //                        DataRow1[t] = ((char)UnPacker1.GetCharByIndex(t)).ToString();
    //        //                        break;
    //        //                    case (sbyte)'S':   //S
    //        //                        DataRow1[t] = UnPacker1.GetStrByIndex(t);
    //        //                        break;
    //        //                    case (sbyte)'F':  //F
    //        //                        DataRow1[t] = UnPacker1.GetDoubleByIndex(t).ToString();
    //        //                        break;
    //        //                    case (sbyte)'R':  //R
    //        //                        DataRow1[t] = string.Empty;
    //        //                        break;
    //        //                    default:
    //        //                        DataRow1[t] = string.Empty;
    //        //                        break;
    //        //                }
    //        //                #endregion
    //        //            }

    //        //            Result.Rows.Add(DataRow1);
    //        //            UnPacker1.Next();
    //        //        }


    //        //        //Program.DisplayDataTable(Result);

    //        //        return;
    //        //    }
    //        //}

    //        //int iRetCode = lpMsg.GetErrorNo();//获取返回码
    //        //int iErrorCode = lpMsg.GetReturnCode();//获取错误码
    //        //int iFunction = lpMsg.GetFunction();
    //        //if (iRetCode != 0)
    //        //{
    //        //    Console.WriteLine("异步接收出错：" + lpMsg.GetErrorNo().ToString() + lpMsg.GetErrorInfo());
    //        //}
    //        //else
    //        //{



    //        //    if (iFunction == 620000)//1.0消息中心心跳
    //        //    {
    //        //        //lpMsg.ChangeReq2AnsMessage();
    //        //        //connection.SendBizMsg(lpMsg, 1);
    //        //        return;
    //        //    }
    //        //    else if (iFunction == 620003 || iFunction == 620025) //收到发布过来的行情
    //        //    {

    //        //        //Show("收到主推消息！");
    //        //        //int iKeyInfo = 0;
    //        //        //void* lpKeyInfo = lpMsg.GetKeyInfo(&iKeyInfo);
    //        //        //CT2UnPacker unPacker = new CT2UnPacker(lpKeyInfo, (uint)iKeyInfo);
    //        //        //Show(unPacker);
    //        //        //unPacker.Dispose();

    //        //    }
    //        //    else if (iFunction == 620001)
    //        //    {
    //        //        //Show("收到订阅应答！");
    //        //        return;
    //        //    }
    //        //    else if (iFunction == 620002)
    //        //    {
    //        //        //Show("收到取消订阅应答！");
    //        //        return;
    //        //    }



    //        //    CT2UnPacker unpacker = null;
    //        //    unsafe
    //        //    {
    //        //        int iLen = 0;
    //        //        void* lpdata = lpMsg.GetContent(&iLen);
    //        //        unpacker = new CT2UnPacker(lpdata, (uint)iLen);
    //        //    }
    //        //    if (iFunction == 10001)
    //        //    {
    //        //        int code = unpacker.GetInt("ErrCode");
    //        //        if (code == 0)
    //        //        {
    //        //            unpacker.SetCurrentDatasetByIndex(1);
    //        //            string session = unpacker.GetStr("user_token");
    //        //        }
    //        //    }


    //        //    //Show(unpacker);
    //        //}
    //    }

    //    public override void OnRegister(CT2Connection lpConnection)
    //    {
    //        Console.WriteLine("OnRegister");
    //    }

    //    public override void OnSafeConnect(CT2Connection lpConnection)
    //    {
    //        Console.WriteLine("OnSafeConnect");
    //    }

    //    public override void OnSent(CT2Connection lpConnection, int hSend, void* lpData, int nLength, int nQueuingData)
    //    {
    //        Console.WriteLine("OnSent {0}", hSend);
    //    }
    //}
}
