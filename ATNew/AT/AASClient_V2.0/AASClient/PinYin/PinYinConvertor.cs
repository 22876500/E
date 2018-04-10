using Microsoft.International.Converters.PinYinConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASClient.PinYin
{
    class PinYinConvertor
    {
        /// <summary>   
        /// 汉字转化为拼音  
        /// </summary>   
        /// <param name="str">汉字</param>   
        /// <returns>全拼</returns>   
        public static string GetPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();
                    r += t.Substring(0, t.Length - 1);
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary>   
        /// 汉字转化为拼音首字母  
        /// </summary>   
        /// <param name="str">汉字</param>   
        /// <returns>首字母</returns>   
        public static string GetFirstPinyin(string str)
        {
            string r = string.Empty;
            foreach (char obj in str)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[chineseChar.PinyinCount - 1].ToString();
                    r += t[0];
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }
    }
}
