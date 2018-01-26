using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CollectDataServer.Common
{
    public class CommonUtils
    {
        public static decimal GetClosePrice(string strCode)
        {
            decimal closePrice = 0;
            try
            {
                string formatCode = FormatCode(strCode);
                if (string.IsNullOrEmpty(formatCode)) return closePrice;
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                Byte[] pageData = MyWebClient.DownloadData(string.Format("http://hq.sinajs.cn/list={0}", formatCode)); //从指定网站下载数据

                string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

                //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
     
                    //var hq_str_sh601003="柳钢股份,7.140,7.150,7.080,7.350,6.920,7.090,7.100,35804402,255146542.000,61200,7.090,27700,7.080,35300,7.070,189700,7.060,307900,7.050,12300,7.100,46200,7.110,28600,7.120,176300,7.130,9000,7.140,2017-10-11,15:00:00,00";

                    //Regex reg = new Regex(@"var\s*hq_str_(sh|sz)\d{6}\s*=""(\S+)\,\S+\,(\S+)*");
                    //Match match = reg.Match(item);

                if (!pageHtml.Contains("hq_str_") || pageHtml.Contains("hq_str_sys_auth")) { return 0; }
                closePrice = Convert.ToDecimal(pageHtml.Split(',')[3].Trim());

            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("证券代码{0},获取收盘价格异常,{1}", strCode, ex.Message);
                return closePrice;
            }

            return closePrice;
        }

        public static string FormatCode(string code)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(code)) return result;

            if (code.StartsWith("6"))
            {
                result = "sh" + code;
            }
            else if (code.StartsWith("00") || code.StartsWith("300"))
            {
                result = "sz" + code;
            }
            return result;
        }

        /// <summary>
        /// 64位MD5加密
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
    }
}
