using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataComparision
{
    public static class StringUtils
    {
        #region Match Group Code
        static Regex regCode = new Regex("[A-Z][0-9]{2}");
        /// <summary>
        /// Get Group Name like A01~ Z99, a01~z99
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetGroupName(this string fileName)
        {
            if (regCode.IsMatch(fileName))
            {
                
                return regCode.Match(fileName).Value;
            }
            return string.Empty;
        } 
        #endregion

        #region Match Date
        static Regex regDate = new Regex("[0-9-\\/]{8,}");
        /// <summary>
        /// Default: DateTime.MinValue
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DateTime GetDate(this string fileName)
        {
            DateTime dt = DateTime.MinValue;
            var matche = regDate.Match(fileName);
            if (matche.Success)
            {
                if (!DateTime.TryParse(matche.Value, out dt))
                {
                    dt = new DateTime(int.Parse(matche.Value.Substring(0, 4)), int.Parse(matche.Value.Substring(4, 2)), int.Parse(matche.Value.Substring(6, 2)));
                }
            }
            return dt;
        } 
        #endregion

        #region Get Type
        public static ImportDataType GetImportType(this string fileName)
        {
            
            if (fileName.Contains("委托"))
            {
                return ImportDataType.软件委托;
            }
            else if (fileName.Contains("交割") || fileName.Contains("对账") || fileName.Contains("资金流水"))
            {
                return ImportDataType.交割单;
            }
            else
            {
                return ImportDataType.其他;
            }
        } 
        #endregion

        internal static string FixStockCode(this string code)
        {
            if (!string.IsNullOrWhiteSpace(code) && code.Length < 6)
            {
                for (int i = code.Length; i < 6; i++)
                {
                    code = "0" + code;
                }
            }
            return code;
        }

        public static string GetFileName(this string fullPathName)
        {
            return fullPathName.Substring(fullPathName.LastIndexOf('\\') + 1);
        }
    }

    public enum ImportDataType
    {
        交割单 = 0,
        软件委托 = 1,
        其他 = 2,
    }
}
