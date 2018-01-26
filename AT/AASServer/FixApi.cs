using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AASServer
{
    public static class FixApi
    {
        ///// <summary>
        ///// 第三方接口库初始化。只有调用一次就可以，不能多次调用。
        ///// </summary>
        ///// <returns>返回True表示初始化成功；False表示失败。</returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_Initialize();

        ///// <summary>
        ///// 第三方接口库卸载。只有调用一次就可以，不能多次调用。
        ///// </summary>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_Uninitialize();

        ///// <summary>
        ///// 设置第三方应用信息
        ///// </summary>
        ///// <param name="pszAppName">应用名称</param>
        ///// <param name="pszVer">应用版本</param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetAppInfo(string pszAppName, string pszVer);

        ///// <summary>
        ///// 设置每个业务请求包缺省的头信息
        ///// </summary>
        ///// <param name="pszUser">系统要求的柜员代码(八位的字符串)</param>
        ///// <param name="pszWTFS">委托方式</param>
        ///// <param name="pszFBDM">发生营业部的代码信息(四位的字符串)</param>
        ///// <param name="pszDestFBDM">目标营业部的代码信息(四位的字符串)</param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetDefaultInfo(string pszUser, string pszWTFS, string pszFBDM, string pszDestFBDM);

        ///// <summary>
        ///// 设置缺省的区域代码
        ///// </summary>
        ///// <param name="pszQYBM"></param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetDefaultQYBM(string pszQYBM);

        ///// <summary>
        ///// 连接到顶点中间件服务器 
        ///// </summary>
        ///// <param name="pszAddr">为连接的服务器地址; 格式为: "ip 地址@端口 /tcp" </param>
        ///// <param name="pszUser">通信用户名称; (默认为空串) </param>
        ///// <param name="pszPwd">通信用户的密码; (默认为空串) </param>
        ///// <param name="pszFileCert">客户端证书文件 (由券商向开发商提供） (默认为空串) </param>
        ///// <param name="pszCertPwd">客户端证书密码(由券商向开发商提供） (默认为空串) </param>
        ///// <param name="pszFileCA">信任机构证书文件(由券商向开发商提供） (默认为空串)</param>
        ///// <param name="pszProcotol">启用 SSL/TLS 或顶点证书安全协议;  可以根据客户要求(默认为空串) "SSLv3" 表示启用 SSLv3 协议建立连接 "TLSv1" 表示启用 TLSv1 协议建立连接 "APEX" 表示顶点证书安全协议 </param>
        ///// <param name="bVerify">SSL/TLS 方式下是否校验服务器端证书.true 表示校验对方证书。 </param>
        ///// <param name="nTimeOut">连接超时时间，单位秒 </param>
        ///// <returns>系统返回类型为 HANDLE_CONN 的连接句柄。 如果连接失败则返回 0; 成功不为 0</returns>
        //[DllImport("FixApi.dll")]
        //public static extern int Fix_ConnectEx(string pszAddr, string pszUser, string pszPwd, string pszFileCert, string pszCertPwd, string pszFileCA, string pszProcotol, bool bVerify, int nTimeOut);

        ///// <summary>
        ///// 与顶点中间件连接关闭
        ///// </summary>
        ///// <param name="conn"></param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_Close(int conn);

        ///// <summary>
        ///// 申请会话句柄 
        ///// </summary>
        ///// <param name="conn">类型为 HANDLE_CONN 的连接句柄。该句柄由 Fix_Connect 生成的</param>
        ///// <returns>返回值类型为 HANDLE_SESSION 的会话对象；如果对象值为空表示对象 分配失败。否则表示成功</returns>
        //[DllImport("FixApi.dll")]
        //public static extern int Fix_AllocateSession(int conn);

        ///// <summary>
        ///// 释放会话句柄 
        ///// </summary>
        ///// <param name="sess">类型为 HANDLE_SESSION 的会话对象。 </param>
        ///// <returns>返回值为 True 表示成功; False 表示失败</returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_ReleaseSession(int sess);

        /////// <summary>
        /////// 设置会话超时 
        /////// </summary>
        /////// <param name="conn">类型为 HANDLE_CONN 的连接句柄。该句柄由 Fix_Connect 生成的。 </param>
        /////// <returns>业务应答超时时间；单位为秒，系统默认为 30 秒。 </returns>
        ////[DllImport("FixApi.dll")]
        ////public static extern bool Fix_SetTimeOut(int conn, short timeout);

        ///// <summary>
        /////  设置会话委托方式 
        ///// </summary>
        ///// <param name="sess"> 类型为 HANDLE_SESSION 的会话句柄。 </param>
        ///// <param name="val">用于表示客户的接入方式；比如电话委托，磁卡委托，互联网委托等。 </param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetWTFS(int sess, string val);

        ///// <summary>
        ///// 设置业务的来源营业部 
        ///// </summary>
        ///// <param name="sess">类型为 HANDLE_SESSION 的会话句柄</param>
        ///// <param name="val">字符串类型。用于表示客户业务发生的营业代码,必须是 四位的营业部代码; </param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetFBDM(int sess, string val);

        ///// <summary>
        ///// 设置业务的目标营业部 
        ///// </summary>
        ///// <param name="sess">类型为 HANDLE_SESSION 的会话句柄。 </param>
        ///// <param name="val">字符串类型。用于表示客户业务发生的营业代码,必须是 四位的营业部代码;</param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetDestFBDM(int sess, string val);

        ///// <summary>
        ///// 设置会话的业务发生的节点信息。 (必须在 Fix_CreateHead 或者 Fix_CreateReq 函数之前调用)  
        ///// </summary>
        ///// <param name="sess"></param>
        ///// <param name="node">字符串类型。一般是客户网卡信息或 IP 地址。 </param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetNode(int sess, string node);

        ///// <summary>
        ///// 设置会话的业务发生的柜员代码信息.(必须在Fix_CreateHead函数之前调用)
        ///// </summary>
        ///// <param name="sess"> 类型为HANDLE_SESSION的会话句柄</param>
        ///// <param name="val">字符串类型。对一些柜台特殊业务，需要用到柜员信息进行认证。</param>
        ///// <returns>返回值为True表示成功; FALSE表示失败。</returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetGYDM(int sess, string node);

        ///// <summary>
        ///// 设置会话的业务功能号.具体的功能号的定义请参照【第三方接入业务接口文档】。
        ///// </summary>
        ///// <param name="sess">类型为HANDLE_SESSION的会话句柄。</param>
        ///// <param name="nFunc">整型。即为系统提供的业务功能号。</param>
        ///// <returns>返回值为True表示成功; FALSE表示失败。</returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_CreateHead(int sess, int nFunc);
        ///// <summary>
        /////  设置该会话要发送给中间件的业务的请求域数据 
        ///// </summary>
        ///// <param name="sess">类型为 HANDLE_SESSION 的会话句柄。 </param>
        ///// <param name="id">请求域的 tag 值；具体的定义值请参考【第三方接入业务 接口文档】。 </param>
        ///// <param name="val">字符串类型;对应于 id 的业务数据。 </param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetString(int sess, int id, string val);

        ///// <summary>
        ///// 设置该会话要发送给中间件的业务的请求域数据  
        ///// </summary>
        ///// <param name="sess">类型为 HANDLE_SESSION 的会话句柄。 </param>
        ///// <param name="id">请求域的 tag 值；具体的定义值请参考【第三方接入业 务接口文档】。</param>
        ///// <param name="val">浮点类型;对应于 id 的业务数据。 </param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetLong(int sess, int id, int val);

        ///// <summary>
        /////  设置该会话要发送给中间件的业务的请求域数据 
        ///// </summary>
        ///// <param name="sess">类型为 HANDLE_SESSION 的会话句柄。 </param>
        ///// <param name="id"></param>
        ///// <param name="val"></param>
        ///// <returns></returns>
        //[DllImport("FixApi.dll")]
        //public static extern bool Fix_SetItem(int sess, int id, double val);

    }
}
