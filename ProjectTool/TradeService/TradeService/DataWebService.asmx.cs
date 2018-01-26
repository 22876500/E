using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;

namespace TradeService
{
    /// <summary>
    /// 交易信息查询服务
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DataWebService : System.Web.Services.WebService
    {
        static Dictionary<string, DateTime> dictLogon = new Dictionary<string, DateTime>();

        [WebMethod]
        public bool Login(string userName, string password)
        {
            try
            {
                bool isLogon = CommonUtils.GetConfig("user") == userName && CommonUtils.GetConfig("psw") == password;
                if (isLogon)
                {
                    if (dictLogon.ContainsKey(userName))
                    {
                        dictLogon[userName] = DateTime.Now;
                    }
                    else
                    {
                        dictLogon.Add(userName, DateTime.Now);
                        CommonUtils.Log(string.Format("用户'{0}'登录成功 ", Cryptor.MD5Decrypt(userName)));
                    }

                }
                return isLogon;

            }
            catch (Exception ex)
            {
                CommonUtils.Log(ex.Message);
                return false;
            }
            
        }

        [WebMethod]
        public bool CompareToolLogin(string userName, string password)
        {
            try
            {
                var configInfo = CommonUtils.GetConfig("compare_login");
                var upArr = Cryptor.MD5Decrypt(configInfo).Split('|');
                foreach (var item in upArr)
                {
                    var up = item.Split('-');
                    if (up[0] == userName)
                    {
                        bool isLogon = password == up[1];
                        if (isLogon)
                        {
                            if (dictLogon.ContainsKey(userName))
                            {
                                dictLogon[userName] = DateTime.Now;
                            }
                            else
                            {
                                dictLogon.Add(userName, DateTime.Now);
                                //CommonUtils.Log(string.Format("对比工具用户'{0}'登录成功 ", userName));
                            }
                        }
                        return isLogon;
                    }
                }
                return false;

                //bool isLogon = CommonUtils.GetConfig("compare_user") == userName && CommonUtils.GetConfig("compare_psw") == password;
                //if (isLogon)
                //{
                //    if (dictLogon.ContainsKey(userName))
                //    {
                //        dictLogon[userName] = DateTime.Now;
                //    }
                //    else
                //    {
                //        dictLogon.Add(userName, DateTime.Now);
                //        CommonUtils.Log(string.Format("用户'{0}'登录成功 ", Cryptor.MD5Decrypt(userName)));
                //    }

                //}
                //return isLogon;

            }
            catch (Exception ex)
            {
                CommonUtils.Log("CompareToolLogin Exception" + ex.Message);
                return false;
            }
        }

        [WebMethod]
        public void LogOut(string userName)
        {
            if (dictLogon.ContainsKey(userName))
            {
                dictLogon.Remove(userName);
            }
        }

        [WebMethod]
        public  string GetGroups(string userName)
        {
            if (dictLogon.ContainsKey(userName))
            {
                var dict = new Dictionary<string, string>();
                foreach (string item in WebConfigurationManager.AppSettings.AllKeys)
                {
                    if (Regex.IsMatch(item, "^[A-Z][0-9]{2}$"))
                    {
                        dict.Add(item, WebConfigurationManager.AppSettings[item].ToString());
                    }
                }
                return dict.ToJson();
            }
            return null;
        }

        [WebMethod]
        public string GetGroupNames(string userName)
        {
            if (dictLogon.ContainsKey(userName))
            {
                return string.Join(",", WebConfigurationManager.AppSettings.AllKeys.Where(_ => Regex.IsMatch(_, "^[A-Z][0-9]{2}$")));
            }
            else
            {
                return "请登录后查询！";
            }
        }

        [WebMethod]
        public bool SaveTradeData(string data, string dataType)
        {
            try
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory +  "\\TradeData\\" + DateTime.Today.ToString("yyyy-MM-dd");
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                System.IO.File.AppendAllText(dir + "\\" + dataType + ".txt", data);
                return true;
            }
            catch (Exception ex)
            {
                CommonUtils.Log("自动保存T+1接口数据功能异常:\r\n" + data, ex);
                return false;
            }
        }
    
        
    }
}
