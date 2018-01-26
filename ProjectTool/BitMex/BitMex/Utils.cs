using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMex
{
    public static class Utils
    {
        #region Json
        public static string ToJson(this object o)
        {
            try
            {
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception ex)
            {
                App.Log.LogInfoFormat("Json序列化错误 {0}", ex.Message);
                return string.Empty;
            }
        }

        public static T FromJson<T>(this string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception ex)
            {
                App.Log.LogInfoFormat("Json解析错误，解析对象：{0}, 错误信息:{1}", s, ex.ToString());
                return default(T);
            }
        }

        public static List<T> ListFromJson<T>(this string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(s);
            }
            catch (Exception ex)
            {
                App.Log.LogInfoFormat("Json解析错误，解析对象：{0}, 错误信息:{1}", s, ex.ToString());
                return new List<T>();
            }
        }
        #endregion
    }
}
