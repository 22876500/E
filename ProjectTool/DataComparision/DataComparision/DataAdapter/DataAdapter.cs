using DataComparision.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataComparision.DataAdapter
{
    public class DataHelper
    {
        #region Property
        static Dictionary<string, string[]> _deliveryMap;
        /// <summary>
        /// 交割单列映射
        /// </summary>
        public static Dictionary<string, string[]> DeliveryMap
        {
            get
            {
                if (_deliveryMap == null)
                {
                    var formatInfo = CommonUtils.GetConfig("别名配置");
                    if (string.IsNullOrEmpty(formatInfo))
                    {
                        _deliveryMap = new Dictionary<string, string[]>();
                        _deliveryMap.Add("交割日期", new[] { "成交日期", "发生日期", "日期" });
                        _deliveryMap.Add("买卖标志", new[] { "操作", "业务类型", "业务名称", "摘要" });
                        _deliveryMap.Add("成交价格", new[] { "成交均价", });
                        _deliveryMap.Add("成交编号", new[] { "合同编号", "协议编号", "委托编号" });
                        _deliveryMap.Add("发生金额", new[] { "变动金额", "清算金额" });
                        _deliveryMap.Add("手续费", new[] { "佣金", });
                        CommonUtils.SetConfig("别名配置", _deliveryMap.ToJson());
                    }
                    else
                    {
                        _deliveryMap = formatInfo.FromJson<Dictionary<string, string[]>>();
                    }
                }
                return _deliveryMap;
            }
        }

        /// <summary>
        /// 特殊组合号，起止日期使用同一天时不能查出对应交割单
        /// </summary>
        static readonly string[] StrangeGroups = new[] { "B08", "D35" };

        static string[] _deliveryCol;
        /// <summary>
        /// 交割单列名
        /// </summary>
        public static string[] DeliveryCol
        {
            get
            {
                if (_deliveryCol == null)
                {
                    _deliveryCol = new[] { "组合号", "交割日期", "证券代码", "证券名称", "买卖标志", "成交数量", "成交价格", "成交金额", "成交编号", "发生金额", "手续费", "印花税", "过户费", "其他费", "备注" };
                }
                return _deliveryCol;
            }
        }


        //static bool? _isExists = null;
        ///// <summary>
        ///// 是否存在本机AAS数据库
        ///// </summary>
        //public static bool IsExistsAAS
        //{
        //    get
        //    {
        //        if (_isExists == null)
        //        {
        //            try
        //            {
        //                using (var conn = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=AAS;Integrated Security=True;"))
        //                {
        //                    conn.Open();
        //                    if (conn.State == ConnectionState.Open)
        //                    {
        //                        conn.Close();
        //                        _isExists = true;
        //                    }
        //                    else
        //                    {
        //                        _isExists = false;
        //                    }
        //                }
        //            }
        //            catch
        //            {
        //                _isExists = false;
        //            }
        //        }

        //        return _isExists == true;
        //    }
        //}
        #endregion

        public static bool ImportGroupFromAAS(bool isCover)
        {
            bool isImportSuccess = false;

            var dt = new DataTable();
            string cmd = "SELECT [名称] ,[启用] ,[交易服务器] ,[版本号] ,[营业部代码] ,[登录帐号] ,[交易帐号] ,[交易密码] ,[通讯密码] FROM [AAS].[dbo].[券商帐户]";
            List<string> groupNames = new List<string>();
            using (var adapter = new SqlDataAdapter(cmd, "Data Source=.\\SQLEXPRESS;Initial Catalog=AAS;Integrated Security=True;"))
            {
                adapter.Fill(dt);
                dt.Columns.Add("IP");
                dt.Columns.Add("Port");
                for (int i = dt.Rows.Count - 1; i > -1; i--)
                {
                    var item = dt.Rows[i];
                    var ipInfo = item["交易服务器"].ToString().Split(':');
                    item["IP"] = ipInfo[1];
                    item["Port"] = ipInfo[2];

                    if (isCover)
                        groupNames.Add(item["名称"].ToString());
                    else
                        dt.Rows.RemoveAt(i);
                }
            }

            using (var db = new DataComparisionDataset())
            {
                var old = db.券商ds.Where(_ => groupNames.Contains(_.名称)).ToList();
                db.券商ds.RemoveRange(old);
                db.SaveChanges();
            }


            isImportSuccess = WriteToDB(dt, "券商");
            return isImportSuccess;
        }

        public static bool WriteToDB(DataTable dt, string tableName)
        {
            if (dt == null || dt.Rows.Count == 0)
                return false;

            bool IsSavedToDB = true;
            string connectionString = CommonUtils.DBConnection;

            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString))
            {
                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.BatchSize = dt.Rows.Count;
                foreach (DataColumn item in dt.Columns)
                    sqlBulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);

                try
                {
                    sqlBulkCopy.WriteToServer(dt);
                    IsSavedToDB = true;
                }
                catch (Exception ex)
                {
                    IsSavedToDB = false;
                    CommonUtils.Log("SqlBulkCopy数据导入出错", ex);
                }
            }

            return IsSavedToDB;
        }

        public static void StandardDeliveryDataTable(DataTable dt, DateTime? importDate = null)
        {
            DateTime st = DateTime.Today;
            DateTime et = DateTime.MinValue;
            //1.修正列名
            StandardColumnName(dt);

            //2. 修正数据列
            for (int i = dt.Columns.Count - 1; i > -1; i--)
            {
                if (!DataHelper.DeliveryCol.Contains(dt.Columns[i].ColumnName))
                {
                    dt.Columns.RemoveAt(i);
                }
            }
            for (int i = DataHelper.DeliveryCol.Length - 1; i > -1; i--)
            {
                if (!dt.Columns.Contains(DataHelper.DeliveryCol[i]))
                {
                    dt.Columns.Add(DataHelper.DeliveryCol[i]);
                }
            }
            dt.Columns.Add("OrderID");
            dt.Columns.Add("SortSequence");
            

            //3. 修正数据类型，已对应当前数据库，将数字列进行转换
            List<string> colDec = new List<string>() { "成交数量", "成交价格", "成交金额", "发生金额", "手续费", "印花税", "过户费", "其他费" };
            List<string> groupList = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var groupName = dt.Rows[i]["组合号"].ToString();
                if (!groupList.Contains(groupName))
                {
                    groupList.Add(groupName);
                }

                var date = CommonUtils.GetDate(dt.Rows[i]["交割日期"]);
                if (date == DateTime.MinValue && importDate != null)
                {
                    date = importDate.Value;
                }
                if (st > date) st = date;
                if (et < date) et = date;

                dt.Rows[i]["OrderID"] = Guid.NewGuid().ToString();
                dt.Rows[i]["SortSequence"] = i;
                dt.Rows[i]["证券代码"] = dt.Rows[i]["证券代码"].ToString().FixStockCode();
                dt.Rows[i]["交割日期"] = date;

                foreach (var colName in colDec)
                {
                    dt.Rows[i][colName] = CommonUtils.GetDecimal(dt.Rows[i][colName]);
                }
            }

            //4.保存数据库
            using (var db = new DataComparisionDataset())
            {

                var oldData = db.交割单ds.Where(_ => _.交割日期 >= st && _.交割日期 < et && groupList.Contains(_.组合号));

                db.交割单ds.RemoveRange(oldData);
                db.SaveChanges();
            }
        }

        private static void StandardColumnName(DataTable dt)
        {
            var map = DataHelper.DeliveryMap;
            foreach (var kv in map)
            {
                if (!dt.Columns.Contains(kv.Key))
                {
                    foreach (var item in kv.Value)
                    {
                        if (dt.Columns.Contains(item))
                        {
                            dt.Columns[item].ColumnName = kv.Key;
                            break;
                        }
                    }
                }
            }
        }

        //public static DataTable JoinTable(DataTable dt1, DataTable dt2)
        //{
        //    foreach (DataRow row in dt1.Rows)
        //    {
        //        var newRow = dt2.NewRow();
        //        foreach (DataColumn col in dt2.Columns)
        //        {
        //            if (dt1.Columns.Contains(col.ColumnName))
        //            {
        //                newRow[col.ColumnName] = row[col.ColumnName];
        //            }
        //        }
        //        dt2.Rows.Add(newRow);
        //    }
        //    return dt1;
        //}

        private static StringBuilder queryHisResult = new StringBuilder(1024 * 1024);
        /// <summary>
        /// 通达信接口-历史数据查询(默认查询交割单)
        /// </summary>
        /// <param name="st">开始时间</param>
        /// <param name="et">结束时间</param>
        /// <param name="o">券商</param>
        /// <param name="historyDataType">表示查询信息的种类，0历史委托  1历史成交   2交割单</param>
        /// <returns></returns>
        public static DataTable QueryHisData(DateTime st, DateTime et, 券商 o, int historyDataType = 2, bool originalData = false)
        {
            DataTable dt = null;
            StringBuilder ErrInfo = new StringBuilder(256);
            //StringBuilder result = new StringBuilder(1024 * 1024);
            queryHisResult.Clear();
            if (o.营业部代码 == 8888)
            {
                if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday 
                 || DateTime.Today.DayOfWeek == DayOfWeek.Saturday
                 || DateTime.Now.Hour <= 9
                 || DateTime.Now.Hour >= 15)
                {
                    o.营业部代码 = 24;
                    o.IP = "124.74.242.150";
                    o.Port = 443;
                }
            }

            var ClientID = TdxApi.Logon(o.IP, o.Port, o.版本号, o.营业部代码, o.登录帐号, o.交易帐号, o.TradePsw, o.CommunicatePsw, ErrInfo);
            if (ErrInfo.Length > 0)
            {
                CommonUtils.Log(string.Format("通达信接口登录失败, 组合号：{0}, 起始日期：{1},结束日期：{2} ,错误信息：{3}", o.名称, st.ToString(), et.ToString(), ErrInfo.ToString()));
            }
            else
            {
                DateTime searchEndDate = et;
                if (StrangeGroups.Contains(o.名称) && st == et)
                {
                    searchEndDate = et.AddDays(1);
                }
                TdxApi.QueryHistoryData(ClientID, historyDataType, st.ToString("yyyyMMdd"), searchEndDate.ToString("yyyyMMdd"), queryHisResult, ErrInfo);
                if (ErrInfo.Length == 0)
                {
                    dt = CommonUtils.ChangeDataStringToTable(queryHisResult.ToString());
                    if (!originalData && historyDataType == 2)
                    {
                        FilteTableData(o, st, et, dt);
                        CheckDataIntegrity(st, et, o, dt, ErrInfo, queryHisResult, ClientID);
                        RepairData(o, dt);
                    }
                }
                else
                {
                    CommonUtils.Log("通达信接口查询失败，错误信息" + ErrInfo.ToString());
                }
            }
            TdxApi.Logoff(ClientID);
            return dt;
        }


        /// <summary>
        /// 过滤数据，如B08,如果只查一天的数据，会查不出来，必须查两天的，再根据日期过滤
        /// </summary>
        /// <param name="o"></param>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <param name="dt"></param>
        private static void FilteTableData(券商 o, DateTime st, DateTime et, DataTable dt)
        {
            if (StrangeGroups.Contains(o.名称) && st == et)
            {
                for (int i = dt.Rows.Count - 1; i > -1; i--)
                {
                    if (dt.Rows[i]["成交日期"].ToString() != st.ToString("yyyyMMdd")) dt.Rows.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 数据检查，查看是否需要根据历史成交进行补充。
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <param name="o"></param>
        /// <param name="dt"></param>
        /// <param name="ErrInfo"></param>
        /// <param name="result"></param>
        /// <param name="ClientID"></param>
        private static void CheckDataIntegrity(DateTime st, DateTime et, 券商 o, DataTable dt, StringBuilder ErrInfo, StringBuilder result, int ClientID)
        {
            #region 成交金额修正
            if (dt.Columns.Contains("成交金额"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["成交金额"] = Math.Abs(CommonUtils.GetDecimal(row["成交金额"]));
                }
            }
            else if (dt.Columns.Contains("成交数量") && dt.Columns.Contains("成交价格"))
            {
                dt.Columns.Add("成交金额");
                foreach (DataRow row in dt.Rows)
                {
                    row["成交金额"] = Math.Abs(CommonUtils.GetDecimal(row["成交价格"]) * CommonUtils.GetDecimal(row["成交数量"]));
                }
            } 
            #endregion

            #region 买卖标志修正
            if (dt.Columns.Contains("买卖标志") && dt.Rows.Count > 0 && Regex.IsMatch(dt.Rows[0]["买卖标志"] + "", "[012]"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    row["买卖标志"] = GetTradeSymble(row);
                }
            }
            else if (!dt.Columns.Contains("买卖标志") && !HasBuySaleColumn(dt) && dt.Columns.Contains("备注"))
            {
                dt.Columns.Add("买卖标志");
                foreach (DataRow row in dt.Rows)
                {
                    row["买卖标志"] = Regex.Match(row["备注"] + "", "买|卖").Value;
                }
            } 
            #endregion

            List<string> VIPColumns = new List<string>() { "证券代码", "证券名称", "成交价格", "成交数量", "成交金额" };
            var needAddColumn = VIPColumns.Where(_ => !dt.Columns.Contains(_)).ToList();

            if (needAddColumn.Count > 0)
            {
                TdxApi.QueryHistoryData(ClientID, 1, st.ToString("yyyyMMdd"), et.ToString("yyyyMMdd"), result, ErrInfo);
                if (ErrInfo.Length == 0)
                {
                    var dt1 = CommonUtils.ChangeDataStringToTable(result.ToString());
                    needAddColumn = needAddColumn.Where(_ => dt1.Columns.Contains(_)).ToList();
                    AddDataColumnFromHisTrade(dt, dt1, needAddColumn);
                }
            }
        }

        private static bool HasBuySaleColumn(DataTable dt)
        {
            bool hasBuySaleCol = false;
            foreach (var item in DeliveryMap["买卖标志"])
            {
                if (dt.Columns.Contains("买卖标志"))
                {
                    hasBuySaleCol = true;
                }
            }
            return hasBuySaleCol;
        }

        private static string GetTradeSymble(DataRow row)
        {
            switch (row["买卖标志"] + "")
            {
                case "0":
                    return "买";
                case "1":
                case "2":
                    return "卖";
                default:
                    return "";
            }
        }

        private static void AddDataColumnFromHisTrade(DataTable dt, DataTable dt1, List<string> needAddColumn)
        {
            foreach (var item in needAddColumn)
            {
                dt.Columns.Add(item);
            }

            int j = 0;
            if (dt.Rows.Count > 0 && dt1.Rows.Count > 0)
            {
                if (!needAddColumn.Contains("证券名称") && dt1.Columns.Contains("证券名称") && !needAddColumn.Contains("成交数量") && dt1.Columns.Contains("成交数量"))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["证券名称"].ToString() == dt1.Rows[j]["证券名称"].ToString() && dt.Rows[i]["成交数量"].ToString() == dt1.Rows[j]["成交数量"].ToString())
                        {
                            needAddColumn.ForEach(_ => dt.Rows[i][_] = dt1.Rows[j][_].ToString());
                            j++;
                        }
                    }
                }
                else if (dt.Columns.Contains("备注"))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (j < dt1.Rows.Count && dt.Rows[i]["备注"].ToString().Contains(dt1.Rows[j]["证券名称"].ToString()))
                        {
                            needAddColumn.ForEach(_ => dt.Rows[i][_] = dt1.Rows[j][_].ToString());
                            j++;
                        }
                    }
                }

            }

        }

        static Regex regFilter = new Regex("股东代码|资金账号|币种|产品帐号|帐号类别|股份发生数|句柄");

        private static void RepairData(券商 o, DataTable dt)
        {
            if (dt.Columns.Count == 0)
                return;

            dt.Columns.Add("组合号");
            foreach (DataRow item in dt.Rows)
                item["组合号"] = o.名称;

            for (int i = dt.Columns.Count - 1; i > -1; i--)
            {
                if (regFilter.IsMatch(dt.Columns[i].ColumnName)) dt.Columns.RemoveAt(i);
            }


        }

        

        /// <summary>
        /// 通达信接口-交易数据查询
        /// </summary>
        /// <param name="o">券商</param>
        /// <param name="tradeDataType">表示查询信息的种类，0资金  1股份   2当日委托  3当日成交     4可撤单   5股东代码  6融资余额   7融券余额  8可融证券</param>
        /// <returns></returns>
        public static DataTable QueryTradeData(券商 o, int tradeDataType)
        {
            DataTable dt = null;
            StringBuilder ErrInfo = new StringBuilder(256);
            StringBuilder result = new StringBuilder(1024 * 1024);
            if (o.营业部代码 == 8888 && (DateTime.Now.Hour >15 || DateTime.Now.Hour < 9 ))
            {
                o.营业部代码 = 24;
                o.IP = "124.74.242.150";
                o.Port = 443;
            }

            var ClientID = TdxApi.Logon(o.IP, o.Port, o.版本号, o.营业部代码, o.登录帐号, o.交易帐号, o.TradePsw, o.CommunicatePsw, ErrInfo);
            if (ErrInfo.Length > 0)
            {
                CommonUtils.ShowMsg(ErrInfo.ToString());
            }
            else
            {
                TdxApi.QueryData(ClientID, tradeDataType, result, ErrInfo);
                if (ErrInfo.Length == 0)
                {
                    dt = CommonUtils.ChangeDataStringToTable(result.ToString());
                }
                else
                {
                    CommonUtils.ShowMsg(ErrInfo.ToString());
                }
            }
            TdxApi.Logoff(ClientID);
            return dt;
        }
    }
}
