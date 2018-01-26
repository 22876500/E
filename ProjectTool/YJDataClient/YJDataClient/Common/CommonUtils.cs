using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YJDataClient.Common
{
    public class CommonUtils
    {
        public static ServiceReference1.JyDataSet.交易员业绩统计DataTable SummaryUserData(ServiceReference1.JyDataSet.交易员业绩统计DataTable 交易员业绩统计DataTable1)
        {
            if (交易员业绩统计DataTable1.Rows.Count == 0) { return 交易员业绩统计DataTable1; }

            int idx = 0;
            ServiceReference1.JyDataSet.交易员业绩统计Row last交易员业绩统计Row = null;
            ServiceReference1.JyDataSet.交易员业绩统计DataTable new交易员业绩统计DataTable = new ServiceReference1.JyDataSet.交易员业绩统计DataTable();
            Dictionary<string, ServiceReference1.JyDataSet.交易员业绩统计Row> dictRegion = new Dictionary<string, ServiceReference1.JyDataSet.交易员业绩统计Row>();
            foreach (ServiceReference1.JyDataSet.交易员业绩统计Row 交易员业绩统计Row1 in 交易员业绩统计DataTable1)
            {
                ServiceReference1.JyDataSet.交易员业绩统计Row new交易员业绩统计Row = new交易员业绩统计DataTable.New交易员业绩统计Row();
                new交易员业绩统计Row.账户 = 交易员业绩统计Row1.账户;
                new交易员业绩统计Row.毛利润 = 交易员业绩统计Row1.毛利润;
                new交易员业绩统计Row.交易股数 = 交易员业绩统计Row1.交易股数;
                new交易员业绩统计Row.交易额 = 交易员业绩统计Row1.交易额;
                new交易员业绩统计Row.手续费 = 交易员业绩统计Row1.手续费;
                new交易员业绩统计Row.利润 = 交易员业绩统计Row1.利润;
                new交易员业绩统计Row.隔夜仓 = 交易员业绩统计Row1.隔夜仓;
                new交易员业绩统计Row.其他 = 交易员业绩统计Row1.其他;
                new交易员业绩统计Row.T1 = 交易员业绩统计Row1.T1;
                new交易员业绩统计Row.NET = 交易员业绩统计Row1.NET;
                new交易员业绩统计Row.使用市值 = 交易员业绩统计Row1.使用市值;
                new交易员业绩统计Row.总市值 = 交易员业绩统计Row1.总市值;
                new交易员业绩统计Row.效率 = 交易员业绩统计Row1.效率;
                new交易员业绩统计Row.使用率 = 交易员业绩统计Row1.使用率;
                new交易员业绩统计Row.使用效率 = 交易员业绩统计Row1.使用效率;
                new交易员业绩统计Row.分组 = 交易员业绩统计Row1.分组;
                new交易员业绩统计Row.日期 = 交易员业绩统计Row1.日期;
                new交易员业绩统计Row.序号 = 交易员业绩统计Row1.序号;

                if (dictRegion.ContainsKey(交易员业绩统计Row1.分组))
                {
                    ServiceReference1.JyDataSet.交易员业绩统计Row regionRow = dictRegion[交易员业绩统计Row1.分组];
                    //regionRow.账户 = 交易员业绩统计Row1.分组;
                    regionRow.毛利润 += 交易员业绩统计Row1.毛利润;
                    regionRow.交易股数 += 交易员业绩统计Row1.交易股数;
                    regionRow.交易额 += 交易员业绩统计Row1.交易额;
                    regionRow.手续费 += 交易员业绩统计Row1.手续费;
                    regionRow.利润 += 交易员业绩统计Row1.利润;
                    regionRow.隔夜仓 += 交易员业绩统计Row1.隔夜仓;
                    regionRow.其他 += 交易员业绩统计Row1.其他;
                    regionRow.T1 += 交易员业绩统计Row1.T1;
                    regionRow.NET += 交易员业绩统计Row1.利润 + 交易员业绩统计Row1.隔夜仓 + 交易员业绩统计Row1.其他;
                    regionRow.使用市值 += 交易员业绩统计Row1.使用市值;
                    regionRow.总市值 += 交易员业绩统计Row1.总市值;
                    regionRow.效率 = regionRow.交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(regionRow.NET / regionRow.交易额 * 100, 2, MidpointRounding.AwayFromZero));
                    regionRow.使用率 = regionRow.总市值 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(regionRow.使用市值 / regionRow.总市值 * 100, 2, MidpointRounding.AwayFromZero));
                    regionRow.使用效率 = decimal.Round(Convert.ToDecimal(regionRow.效率.Replace("%", "").Trim()) / 100 * Convert.ToDecimal(regionRow.使用率.Replace("%", "").Trim()) / 100 * 10000, 2);
                    regionRow.分组 = 交易员业绩统计Row1.分组;
                    //regionRow.日期 = 交易员业绩统计Row1.日期;
                    dictRegion[交易员业绩统计Row1.分组] = regionRow;
                }
                else
                {
                    //dictRegion.Add(交易员业绩统计Row1.分组, new交易员业绩统计Row);
                    ServiceReference1.JyDataSet.交易员业绩统计Row first交易员业绩统计Row = new交易员业绩统计DataTable.New交易员业绩统计Row();
                    first交易员业绩统计Row.账户 = string.Format("{0}合计", 交易员业绩统计Row1.分组);
                    first交易员业绩统计Row.毛利润 = 交易员业绩统计Row1.毛利润;
                    first交易员业绩统计Row.交易股数 = 交易员业绩统计Row1.交易股数;
                    first交易员业绩统计Row.交易额 = 交易员业绩统计Row1.交易额;
                    first交易员业绩统计Row.手续费 = 交易员业绩统计Row1.手续费;
                    first交易员业绩统计Row.利润 = 交易员业绩统计Row1.利润;
                    first交易员业绩统计Row.隔夜仓 = 交易员业绩统计Row1.隔夜仓;
                    first交易员业绩统计Row.其他 = 交易员业绩统计Row1.其他;
                    first交易员业绩统计Row.T1 = 交易员业绩统计Row1.T1;
                    first交易员业绩统计Row.NET = 交易员业绩统计Row1.NET;
                    first交易员业绩统计Row.使用市值 = 交易员业绩统计Row1.使用市值;
                    first交易员业绩统计Row.总市值 = 交易员业绩统计Row1.总市值;
                    first交易员业绩统计Row.效率 = 交易员业绩统计Row1.效率;
                    first交易员业绩统计Row.使用率 = 交易员业绩统计Row1.使用率;
                    first交易员业绩统计Row.使用效率 = 交易员业绩统计Row1.使用效率;
                    first交易员业绩统计Row.分组 = 交易员业绩统计Row1.分组;
                    //first交易员业绩统计Row.日期 = 交易员业绩统计Row1.日期;
                    dictRegion.Add(交易员业绩统计Row1.分组, first交易员业绩统计Row);

                    if (last交易员业绩统计Row != null && last交易员业绩统计Row.分组 != 交易员业绩统计Row1.分组)
                    {
                        new交易员业绩统计DataTable.Add交易员业绩统计Row(dictRegion[last交易员业绩统计Row.分组]);
                    }
                    last交易员业绩统计Row = 交易员业绩统计Row1;
                }
                new交易员业绩统计DataTable.Add交易员业绩统计Row(new交易员业绩统计Row);
                if (idx == 交易员业绩统计DataTable1.Rows.Count - 1)
                {
                    new交易员业绩统计DataTable.Add交易员业绩统计Row(dictRegion[last交易员业绩统计Row.分组]);
                }
                idx++;

            }

            ServiceReference1.JyDataSet.交易员业绩统计Row summaryRow = new交易员业绩统计DataTable.New交易员业绩统计Row();
            summaryRow.账户 = "合计";
            summaryRow.毛利润 = 0;
            summaryRow.交易股数 = 0;
            summaryRow.交易额 = 0;
            summaryRow.手续费 = 0;
            summaryRow.利润 = 0;
            summaryRow.隔夜仓 = 0;
            summaryRow.其他 = 0;
            summaryRow.T1 = 0;
            summaryRow.NET = 0;
            summaryRow.使用市值 = 0;
            summaryRow.总市值 = 0;

            foreach (var key in dictRegion.Keys)
            {
                ServiceReference1.JyDataSet.交易员业绩统计Row region交易员业绩统计Row = new交易员业绩统计DataTable.New交易员业绩统计Row();
                ServiceReference1.JyDataSet.交易员业绩统计Row totalRow = dictRegion[key];
                region交易员业绩统计Row.账户 = string.Format("{0}合计", totalRow.分组);
                region交易员业绩统计Row.毛利润 = totalRow.毛利润;
                region交易员业绩统计Row.交易股数 = totalRow.交易股数;
                region交易员业绩统计Row.交易额 = totalRow.交易额;
                region交易员业绩统计Row.手续费 = totalRow.手续费;
                region交易员业绩统计Row.利润 = totalRow.利润;
                region交易员业绩统计Row.隔夜仓 = totalRow.隔夜仓;
                region交易员业绩统计Row.其他 = totalRow.其他;
                region交易员业绩统计Row.T1 = totalRow.T1;
                region交易员业绩统计Row.NET = totalRow.NET;
                region交易员业绩统计Row.使用市值 = totalRow.使用市值;
                region交易员业绩统计Row.总市值 = totalRow.总市值;
                region交易员业绩统计Row.效率 = totalRow.效率;
                region交易员业绩统计Row.使用率 = totalRow.使用率;
                region交易员业绩统计Row.使用效率 = totalRow.使用效率;
                region交易员业绩统计Row.分组 = totalRow.分组;
                new交易员业绩统计DataTable.Add交易员业绩统计Row(region交易员业绩统计Row);

                summaryRow.毛利润 += totalRow.毛利润;
                summaryRow.交易股数 += totalRow.交易股数;
                summaryRow.交易额 += totalRow.交易额;
                summaryRow.手续费 += totalRow.手续费;
                summaryRow.利润 += totalRow.利润;
                summaryRow.隔夜仓 += totalRow.隔夜仓;
                summaryRow.其他 += totalRow.其他;
                summaryRow.T1 += totalRow.T1;
                summaryRow.NET += totalRow.NET;
                summaryRow.使用市值 += totalRow.使用市值;
                summaryRow.总市值 += totalRow.总市值;

            }
            
            summaryRow.效率 = summaryRow.交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(summaryRow.NET / summaryRow.交易额 * 100, 2, MidpointRounding.AwayFromZero));
            summaryRow.使用率 = summaryRow.总市值 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(summaryRow.使用市值 / summaryRow.总市值 * 100, 2, MidpointRounding.AwayFromZero));
            summaryRow.使用效率 = decimal.Round(Convert.ToDecimal(summaryRow.效率.Replace("%", "").Trim()) / 100 * Convert.ToDecimal(summaryRow.使用率.Replace("%", "").Trim()) / 100 * 10000, 2);
            summaryRow.分组 = "所有";
            new交易员业绩统计DataTable.Add交易员业绩统计Row(summaryRow);

            return new交易员业绩统计DataTable;
        }

        public static ServiceReference1.JyDataSet.分帐户业绩统计DataTable SummaryGroupData(ServiceReference1.JyDataSet.分帐户业绩统计DataTable 分帐户业绩统计DataTable1)
        {
            if (分帐户业绩统计DataTable1.Rows.Count == 0) { return 分帐户业绩统计DataTable1; }
            ServiceReference1.JyDataSet.分帐户业绩统计Row summaryRow = 分帐户业绩统计DataTable1.New分帐户业绩统计Row();

            summaryRow.账户 = "合计";
            summaryRow.毛利润 = 分帐户业绩统计DataTable1.Sum(r => r.毛利润);
            summaryRow.交易股数 = 分帐户业绩统计DataTable1.Sum(r => r.交易股数);
            summaryRow.交易额 = 分帐户业绩统计DataTable1.Sum(r => r.交易额);
            summaryRow.手续费 = 分帐户业绩统计DataTable1.Sum(r => r.手续费);
            summaryRow.利润 = 分帐户业绩统计DataTable1.Sum(r => r.利润);
            summaryRow.隔夜仓 = 分帐户业绩统计DataTable1.Sum(r => r.隔夜仓);
            summaryRow.其他 = 分帐户业绩统计DataTable1.Sum(r => r.其他);
            summaryRow.T1 = 分帐户业绩统计DataTable1.Sum(r => r.T1);
            summaryRow.NET = summaryRow.利润 + summaryRow.隔夜仓 + summaryRow.其他;
            summaryRow.使用市值 = 分帐户业绩统计DataTable1.Sum(r => r.使用市值);
            summaryRow.总市值 = 分帐户业绩统计DataTable1.Sum(r => r.总市值);
            summaryRow.效率 = summaryRow.交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(summaryRow.NET / summaryRow.交易额 * 100, 2, MidpointRounding.AwayFromZero));
            summaryRow.使用率 = summaryRow.总市值 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(summaryRow.使用市值 / summaryRow.总市值 * 100, 2, MidpointRounding.AwayFromZero));
            summaryRow.使用效率 = decimal.Round(Convert.ToDecimal(summaryRow.效率.Replace("%", "").Trim()) / 100 * Convert.ToDecimal(summaryRow.使用率.Replace("%", "").Trim()) / 100 * 10000, 2);
            分帐户业绩统计DataTable1.Add分帐户业绩统计Row(summaryRow);

            return 分帐户业绩统计DataTable1;
        }

        public static ServiceReference1.JyDataSet.分组业绩统计DataTable SummaryUserRegion(ServiceReference1.JyDataSet.分组业绩统计DataTable 分组业绩统计DataTable1)
        {
            if (分组业绩统计DataTable1.Rows.Count == 0) { return 分组业绩统计DataTable1; }
            ServiceReference1.JyDataSet.分组业绩统计Row summaryRow = 分组业绩统计DataTable1.New分组业绩统计Row();

            summaryRow.分组 = "合计";
            summaryRow.毛利润 = 分组业绩统计DataTable1.Sum(r => r.毛利润);
            summaryRow.交易股数 = 分组业绩统计DataTable1.Sum(r => r.交易股数);
            summaryRow.交易额 = 分组业绩统计DataTable1.Sum(r => r.交易额);
            summaryRow.手续费 = 分组业绩统计DataTable1.Sum(r => r.手续费);
            summaryRow.利润 = 分组业绩统计DataTable1.Sum(r => r.利润);
            summaryRow.隔夜仓 = 分组业绩统计DataTable1.Sum(r => r.隔夜仓);
            summaryRow.其他 = 分组业绩统计DataTable1.Sum(r => r.其他);
            summaryRow.T1 = 分组业绩统计DataTable1.Sum(r => r.T1);
            summaryRow.NET = summaryRow.利润 + summaryRow.隔夜仓 + summaryRow.其他;
            summaryRow.使用市值 = 分组业绩统计DataTable1.Sum(r => r.使用市值);
            summaryRow.总市值 = 分组业绩统计DataTable1.Sum(r => r.总市值);
            summaryRow.效率 = summaryRow.交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(summaryRow.NET / summaryRow.交易额 * 100, 2, MidpointRounding.AwayFromZero));
            summaryRow.使用率 = summaryRow.总市值 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(summaryRow.使用市值 / summaryRow.总市值 * 100, 2, MidpointRounding.AwayFromZero));
            summaryRow.使用效率 = decimal.Round(Convert.ToDecimal(summaryRow.效率.Replace("%", "").Trim()) / 100 * Convert.ToDecimal(summaryRow.使用率.Replace("%", "").Trim()) / 100 * 10000, 2);
            分组业绩统计DataTable1.Add分组业绩统计Row(summaryRow);

            return 分组业绩统计DataTable1;
        }

        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        //默认加密密钥
        public static string encryptKey = "sxfrdoac";
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
    }
}
