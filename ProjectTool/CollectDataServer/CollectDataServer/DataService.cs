using CollectDataServer.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CollectDataServer
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DataService
    {
        [OperationContract]
        public bool SendYJData(JyDataSet.业绩统计DataTable dtYJ, string serverMac)
        {
            try
            {
                if (dtYJ.Rows.Count == 0)
                {
                    Program.logger.LogInfo(string.Format("接收server-{0}的业绩统计DataTable.Rows=0", serverMac));
                    return false;
                }
                JyDataSet.业绩统计Row firstRow1 = dtYJ[0];
                //判断当前MAC是否已经更新数据
                string sqlMac = string.Format("select 更新日期 from dbo.日志表 where 服务器MAC='{0}' and 数据类型='业绩'", serverMac);
                DataTable dtMac = SQLHelper.ExecuteDt(sqlMac);
                if (dtMac.Rows.Count > 0)
                {
                    if (firstRow1.日期 == Convert.ToInt32(dtMac.Rows[0]["更新日期"]))
                    {
                        string updateType = ConfigMain.GetConfigValue("UpdateType");
                        if ("0".Equals(updateType))
                        {
                            //删除原有server Mac的数据
                            string sqlDel = string.Format("delete from dbo.业绩统计 where 服务器MAC='{0}' and 日期={1}", serverMac, firstRow1.日期);
                            SQLHelper.ExecuteSql(sqlDel);
                        }
                    }
                }
                decimal closePrice;
                Dictionary<int, decimal> dictPrice = new Dictionary<int, decimal>();
                StringBuilder sb = new StringBuilder();
                sb.Append("begin ");
                Program.logger.LogInfo(string.Format("业绩数据入库开始：{0},当前server-{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), serverMac));
                foreach (JyDataSet.业绩统计Row 业绩统计Row1 in dtYJ)
                {
                    //string sql = "insert into 业绩统计(交易员,组合号,证券代码,证券名称,毛利,交易费用,净利润,买入数量,买入金额,买入均价,卖出数量,卖出金额,卖出均价)values(@交易员,@组合号,@证券代码,@证券名称,@毛利,@交易费用,@净利润,@买入数量,@买入金额,@买入均价,@卖出数量,@卖出金额,@卖出均价)";
                    //SqlParameter[] sqlParm = new SqlParameter[] { new SqlParameter("@交易员", 业绩统计Row1.交易员),new SqlParameter("@组合号", 业绩统计Row1.组合号), new SqlParameter("@证券代码", 业绩统计Row1.证券代码) ,
                    //new SqlParameter("@证券名称", 业绩统计Row1.证券名称), new SqlParameter("@毛利", 业绩统计Row1.毛利),new SqlParameter("@交易费用", 业绩统计Row1.交易费用),
                    //new SqlParameter("@净利润", 业绩统计Row1.净利润),new SqlParameter("@买入数量", 业绩统计Row1.买入数量),new SqlParameter("@买入金额", 业绩统计Row1.买入金额),new SqlParameter("@买入均价", 业绩统计Row1.买入均价),new SqlParameter("@卖出数量", 业绩统计Row1.卖出数量),
                    //new SqlParameter("@卖出金额", 业绩统计Row1.卖出金额),new SqlParameter("@卖出均价", 业绩统计Row1.卖出均价)};

                    //SQLHelper.ExecuteSql(sql, sqlParm);
                    int iCode = Convert.ToInt32(业绩统计Row1.证券代码.Trim());
                    if (dictPrice.ContainsKey(iCode))
                    {
                        closePrice = dictPrice[iCode];
                    }
                    else
                    {
                        closePrice = CommonUtils.GetClosePrice(业绩统计Row1.证券代码.Trim());
                    }
                    string sql = string.Format("insert into dbo.业绩统计(交易员,组合号,证券代码,证券名称,毛利,交易费用,净利润,买入数量,买入金额,买入均价,卖出数量,卖出金额,卖出均价,日期,交易额度,收盘价,分组,服务器MAC)values('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},'{16}','{17}');",
                         业绩统计Row1.交易员,
                         业绩统计Row1.组合号,
                         业绩统计Row1.证券代码,
                         业绩统计Row1.证券名称,
                         业绩统计Row1.毛利,
                         业绩统计Row1.交易费用,
                         业绩统计Row1.净利润,
                         业绩统计Row1.买入数量,
                         业绩统计Row1.买入金额,
                         业绩统计Row1.买入均价,
                         业绩统计Row1.卖出数量,
                         业绩统计Row1.卖出金额,
                         业绩统计Row1.卖出均价,
                         业绩统计Row1.日期,
                         业绩统计Row1.交易额度,
                         closePrice,
                         业绩统计Row1.分组,
                         serverMac
                         );
                    sb.Append(sql);
                }
                sb.Append(" end;");
                SQLHelper.ExecuteSql(sb.ToString());

                //更新日志表
                string sqlLog = string.Empty;
                SqlParameter[] sqlParm;
                if (dtMac.Rows.Count > 0)
                {
                    sqlLog = "update dbo.日志表 set 更新日期=@更新日期,更新数量=@更新数量 where 服务器MAC=@服务器MAC and 数据类型='业绩'";
                    sqlParm = new SqlParameter[] { new SqlParameter("@更新日期", firstRow1.日期), new SqlParameter("@更新数量", dtYJ.Rows.Count), new SqlParameter("@服务器MAC", serverMac) };
                }
                else
                {
                    sqlLog = "insert into dbo.日志表(服务器MAC,更新数量,数据类型,更新日期)values(@服务器MAC,@更新数量,@数据类型,@更新日期)";
                    sqlParm = new SqlParameter[] { new SqlParameter("@更新日期", firstRow1.日期), new SqlParameter("@更新数量", dtYJ.Rows.Count), new SqlParameter("@数据类型", "业绩"), new SqlParameter("@服务器MAC", serverMac) };
                }

                SQLHelper.ExecuteSql(sqlLog, sqlParm);

                Program.logger.LogInfo(string.Format("业绩数据入库结束：{0},当前server-{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), serverMac));

            }
            catch (Exception ex)
            {

                Program.logger.LogInfo(string.Format("业绩数据入库异常：{0},当前server-{1}", ex.Message, serverMac));
                return false;
            }
            return true;
        }

        [OperationContract]
        public JyDataSet.业绩统计DataTable QueryYJData(int startDate,int endDate)
        {
            JyDataSet.业绩统计DataTable dtNew = new JyDataSet.业绩统计DataTable();
            string sql = string.Format("select 交易员,组合号,证券代码,证券名称,毛利,交易费用,净利润,买入数量,买入金额,买入均价,卖出数量,卖出金额,卖出均价,日期 from dbo.业绩统计 where 日期>={0} and 日期<={1}", startDate, endDate);
            DataTable resultDt = SQLHelper.ExecuteDt(sql);
            foreach (DataRow dr in resultDt.Rows)
            {
                JyDataSet.业绩统计Row newRow = dtNew.New业绩统计Row();
                dtNew.Add业绩统计Row(newRow);
                newRow.交易员 = dr["交易员"].ToString();
                newRow.组合号 = dr["组合号"].ToString();
                newRow.证券代码 = dr["证券代码"].ToString();
                newRow.证券名称 = dr["证券名称"].ToString();
                newRow.毛利 = (decimal)dr["毛利"];
                newRow.交易费用 = (decimal)dr["交易费用"];
                newRow.净利润 = (decimal)dr["净利润"];
                newRow.买入数量 = (decimal)dr["买入数量"];
                newRow.买入金额 = (decimal)dr["买入金额"];
                newRow.买入均价 = (decimal)dr["买入均价"];
                newRow.卖出数量 = (decimal)dr["卖出数量"];
                newRow.卖出金额 = (decimal)dr["卖出金额"];
                newRow.卖出均价 = (decimal)dr["卖出均价"];
            }
            return dtNew;
        }

        [OperationContract]
        public JyDataSet.交易员业绩统计DataTable QueryUserYjData(int startDate, int endDate)
        {
            Dictionary<string,decimal> dictSZ = new Dictionary<string,decimal>();
            int idx = 1;
            JyDataSet.交易员业绩统计DataTable dtUserYj = new JyDataSet.交易员业绩统计DataTable();

            try
            {
                string sql = string.Format("select 交易员,分组,sum(毛利)毛利,sum(买入数量+卖出数量)交易股数,sum(买入金额+卖出金额)交易额,sum(交易费用)手续费,sum(毛利-交易费用)利润,sum(净利润)NET, sum(买入数量*收盘价)使用市值,sum(交易额度*收盘价)总市值  from dbo.业绩统计 where 日期>={0} and 日期<={1}  group by 分组,交易员 order by 分组 ", startDate, endDate);
                DataTable resultDt = SQLHelper.ExecuteDt(sql);
                if (resultDt == null || resultDt.Rows.Count == 0) { return dtUserYj; }
                string sqlCap = string.Format("select 交易员,sum(交易额度*收盘价)总市值 from dbo.额度分配 where 日期>={0} and 日期<={1} group by 交易员", startDate, endDate);
                DataTable capDt = SQLHelper.ExecuteDt(sqlCap);
                foreach (DataRow dr in capDt.Rows)
                {
                    if (!dictSZ.ContainsKey(dr["交易员"].ToString()))
                    {
                        dictSZ.Add(dr["交易员"].ToString(), decimal.Round(Convert.ToDecimal(dr["总市值"]), 2));
                    }
                }
                foreach (DataRow dr in resultDt.Rows)
                {
                    JyDataSet.交易员业绩统计Row newRow = dtUserYj.New交易员业绩统计Row();
                    dtUserYj.Add交易员业绩统计Row(newRow);
                    newRow.日期 = startDate == endDate ? startDate.ToString() : string.Format("{0}-{1}", startDate, endDate);
                    newRow.序号 = idx;
                    newRow.账户 = dr["交易员"].ToString().Trim();
                    newRow.分组 = dr["分组"].ToString().Trim();
                    newRow.毛利润 = Convert.ToDecimal(dr["毛利"]);
                    newRow.交易股数 = Convert.ToInt32(dr["交易股数"]);
                    newRow.交易额 = Convert.ToDecimal(dr["交易额"]);
                    newRow.手续费 = Convert.ToDecimal(dr["手续费"]);
                    newRow.利润 = Convert.ToDecimal(dr["利润"]);
                    newRow.隔夜仓 = 0.00M;
                    newRow.其他 = 0.00M;
                    newRow.T1 = 0.00M;
                    newRow.NET = newRow.利润 + newRow.隔夜仓 + newRow.其他;
                    newRow.使用市值 = decimal.Round(Convert.ToDecimal(dr["使用市值"]), 2);
                    if (dictSZ.ContainsKey(newRow.账户))
                    {
                        newRow.总市值 = dictSZ[newRow.账户];
                    }
                    else
                    {
                        newRow.总市值 = 0;
                    }
                    newRow.效率 = newRow.交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(newRow.NET / newRow.交易额 * 100, 2, MidpointRounding.AwayFromZero));
                    newRow.使用率 = newRow.总市值 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(newRow.使用市值 / newRow.总市值 * 100, 2, MidpointRounding.AwayFromZero));
                    newRow.使用效率 = decimal.Round(Convert.ToDecimal(newRow.效率.Replace("%", "").Trim()) / 100 * Convert.ToDecimal(newRow.使用率.Replace("%", "").Trim()) / 100 * 10000, 2);
                    idx++;
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("QueryUserYjData：" + ex.Message);
            }
            
            return dtUserYj;
        }

        [OperationContract]
        public JyDataSet.分帐户业绩统计DataTable QueryGroupYjData(int startDate, int endDate,string region)
        {
            Dictionary<string, decimal> dictSZ = new Dictionary<string, decimal>();
            JyDataSet.分帐户业绩统计DataTable dtGroupYj = new JyDataSet.分帐户业绩统计DataTable();

            try
            {
                string sql = string.Empty;
                string sqlCap = string.Empty;
                if (region.Equals("ALL"))
                {
                    sql = string.Format("select 组合号,sum(毛利)毛利,sum(买入数量+卖出数量)交易股数,sum(买入金额+卖出金额)交易额,sum(交易费用)手续费,sum(毛利-交易费用)利润,sum(净利润)NET,sum(买入数量*收盘价)使用市值,sum(交易额度*收盘价)总市值  from dbo.业绩统计 where 日期>={0} and 日期<={1}  group by 组合号 ", startDate, endDate);
                    sqlCap = string.Format("select 组合号,sum(交易额度*收盘价)总市值 from dbo.额度分配 where 日期>={0} and 日期<={1} group by 组合号", startDate, endDate);
                }
                else
                {
                    sql = string.Format("select 组合号,sum(毛利)毛利,sum(买入数量+卖出数量)交易股数,sum(买入金额+卖出金额)交易额,sum(交易费用)手续费,sum(毛利-交易费用)利润,sum(净利润)NET,sum(买入数量*收盘价)使用市值,sum(交易额度*收盘价)总市值  from dbo.业绩统计 where 日期>={0} and 日期<={1} and 分组='{2}'  group by 组合号 ", startDate, endDate, region);
                    sqlCap = string.Format("select 组合号,sum(交易额度*收盘价)总市值 from dbo.额度分配 where 日期>={0} and 日期<={1} and 分组='{2}' group by 组合号", startDate, endDate,region);
                }
                
                DataTable resultDt = SQLHelper.ExecuteDt(sql);
                if (resultDt == null || resultDt.Rows.Count == 0) { return dtGroupYj; }

                DataTable capDt = SQLHelper.ExecuteDt(sqlCap);
                foreach (DataRow dr in capDt.Rows)
                {
                    if (!dictSZ.ContainsKey(dr["组合号"].ToString()))
                    {
                        dictSZ.Add(dr["组合号"].ToString(), decimal.Round(Convert.ToDecimal(dr["总市值"]), 2));
                    }
                }
                foreach (DataRow dr in resultDt.Rows)
                {
                    JyDataSet.分帐户业绩统计Row newRow = dtGroupYj.New分帐户业绩统计Row();
                    dtGroupYj.Add分帐户业绩统计Row(newRow);
                    newRow.日期 = startDate == endDate ? startDate.ToString() : string.Format("{0}-{1}", startDate, endDate);
                    newRow.账户 = dr["组合号"].ToString().Trim();
                    newRow.毛利润 = Convert.ToDecimal(dr["毛利"]);
                    newRow.交易股数 = Convert.ToInt32(dr["交易股数"]);
                    newRow.交易额 = Convert.ToDecimal(dr["交易额"]);
                    newRow.手续费 = Convert.ToDecimal(dr["手续费"]);
                    newRow.利润 = Convert.ToDecimal(dr["利润"]);
                    newRow.隔夜仓 = 0.00M;
                    newRow.其他 = 0.00M;
                    newRow.T1 = 0.00M;
                    newRow.NET = newRow.利润 + newRow.隔夜仓 + newRow.其他;
                    newRow.使用市值 = decimal.Round(Convert.ToDecimal(dr["使用市值"]), 2);
                    if (dictSZ.ContainsKey(newRow.账户))
                    {
                        newRow.总市值 = dictSZ[newRow.账户];
                    }
                    else
                    {
                        newRow.总市值 = 0;
                    }
                    newRow.效率 = newRow.交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(newRow.NET / newRow.交易额 * 100, 2, MidpointRounding.AwayFromZero));
                    newRow.使用率 = newRow.总市值 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(newRow.使用市值 / newRow.总市值 * 100, 2, MidpointRounding.AwayFromZero));
                    newRow.使用效率 = decimal.Round(Convert.ToDecimal(newRow.效率.Replace("%", "").Trim()) / 100 * Convert.ToDecimal(newRow.使用率.Replace("%", "").Trim()) / 100 * 10000, 2);

                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("QueryGroupYjData：" + ex.Message);
            }
            
            return dtGroupYj;
        }

        [OperationContract]
        public JyDataSet.分组业绩统计DataTable QueryUserRegionYjData(int startDate, int endDate)
        {
            Dictionary<string, decimal> dictSZ = new Dictionary<string, decimal>();
            JyDataSet.分组业绩统计DataTable dtUserRegionYj = new JyDataSet.分组业绩统计DataTable();
            try
            {
                string sql = string.Format("select 分组,sum(毛利)毛利,sum(买入数量+卖出数量)交易股数,sum(买入金额+卖出金额)交易额,sum(交易费用)手续费,sum(毛利-交易费用)利润,sum(净利润)NET,sum(买入数量*收盘价)使用市值,sum(交易额度*收盘价)总市值  from dbo.业绩统计 where 日期>={0} and 日期<={1}  group by 分组 ", startDate, endDate);
                DataTable resultDt = SQLHelper.ExecuteDt(sql);
                if (resultDt == null || resultDt.Rows.Count == 0) { return dtUserRegionYj; }
                string sqlCap = string.Format("select 分组,sum(交易额度*收盘价)总市值 from dbo.额度分配 where 日期>={0} and 日期<={1} group by 分组", startDate, endDate);
                DataTable capDt = SQLHelper.ExecuteDt(sqlCap);
                foreach (DataRow dr in capDt.Rows)
                {
                    if (!dictSZ.ContainsKey(dr["分组"].ToString()))
                    {
                        dictSZ.Add(dr["分组"].ToString(), decimal.Round(Convert.ToDecimal(dr["总市值"]), 2));
                    }
                }
                foreach (DataRow dr in resultDt.Rows)
                {
                    JyDataSet.分组业绩统计Row newRow = dtUserRegionYj.New分组业绩统计Row();
                    dtUserRegionYj.Add分组业绩统计Row(newRow);
                    newRow.日期 = startDate == endDate ? startDate.ToString() : string.Format("{0}-{1}", startDate, endDate);
                    newRow.分组 = dr["分组"].ToString().Trim();
                    newRow.毛利润 = Convert.ToDecimal(dr["毛利"]);
                    newRow.交易股数 = Convert.ToInt32(dr["交易股数"]);
                    newRow.交易额 = Convert.ToDecimal(dr["交易额"]);
                    newRow.手续费 = Convert.ToDecimal(dr["手续费"]);
                    newRow.利润 = Convert.ToDecimal(dr["利润"]);
                    newRow.隔夜仓 = 0.00M;
                    newRow.其他 = 0.00M;
                    newRow.T1 = 0.00M;
                    newRow.NET = newRow.利润 + newRow.隔夜仓 + newRow.其他;
                    newRow.使用市值 = decimal.Round(Convert.ToDecimal(dr["使用市值"]), 2);
                    if (dictSZ.ContainsKey(newRow.分组))
                    {
                        newRow.总市值 = dictSZ[newRow.分组];
                    }
                    else
                    {
                        newRow.总市值 = 0;
                    }
                    newRow.效率 = newRow.交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(newRow.NET / newRow.交易额 * 100, 2, MidpointRounding.AwayFromZero));
                    newRow.使用率 = newRow.总市值 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(newRow.使用市值 / newRow.总市值 * 100, 2, MidpointRounding.AwayFromZero));
                    newRow.使用效率 = decimal.Round(Convert.ToDecimal(newRow.效率.Replace("%", "").Trim()) / 100 * Convert.ToDecimal(newRow.使用率.Replace("%", "").Trim()) / 100 * 10000, 2);

                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("QueryUserRegionYjData：" + ex.Message);
            }
            return dtUserRegionYj;
        }

        [OperationContract]
        public bool AddUser(string userName, string pwd, out string error)
        {
            error = string.Empty;
            int result = 0;

            string querySql = string.Format("select * from dbo.用户表 where 用户名='{0}'", userName);
            DataTable dt = SQLHelper.ExecuteDt(querySql);

            if (dt.Rows.Count > 0)
            {
                string sql = "update dbo.用户表 set 密码=@密码 where 用户名=@用户名";
                SqlParameter[] sqlParm = new SqlParameter[] { new SqlParameter("@用户名", userName), new SqlParameter("@密码", CommonUtils.MD5Encrypt64(pwd)) };

                result = SQLHelper.ExecuteSql(sql, sqlParm);
                if (result != 1)
                {
                    error = "更新用户失败";
                    return false;
                }
            }
            else
            {
                int role = 0;
                string sql = "insert into dbo.用户表 (用户名,密码,角色)values(@用户名,@密码,@角色)";
                SqlParameter[] sqlParm = new SqlParameter[] { new SqlParameter("@用户名", userName), new SqlParameter("@密码", CommonUtils.MD5Encrypt64(pwd)), new SqlParameter("@角色", role) };

                result = SQLHelper.ExecuteSql(sql, sqlParm);
                if (result != 1)
                {
                    error = "添加用户失败";
                    return false;
                }
            }
            return true;
        }

        [OperationContract]
        public bool DeleteUser(string userName)
        {
            string sql = "delete from dbo.用户表 where 用户名=@用户名";
            SqlParameter[] sqlParm = new SqlParameter[] { new SqlParameter("@用户名", userName) };

            int result = SQLHelper.ExecuteSql(sql, sqlParm);
            if (result != 1) return false;
            return true;
        }
        [OperationContract]
        public bool LoginUser(string userName, string userPwd, out string error)
        {
            error = string.Empty;
            string sql = string.Format("select 密码 from dbo.用户表 where 用户名='{0}'", userName.Trim());
            DataTable dt = SQLHelper.ExecuteDt(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (CommonUtils.MD5Encrypt64(userPwd).Equals(dt.Rows[0]["密码"].ToString()))
                {
                    return true;
                }
                else
                {
                    error = "密码不正确";
                    return false;
                }
            }
            else
            {
                error = "用户不存在";
                return false;
            }
        }
        [OperationContract]
        public List<string> GetAllUserName()
        {
            List<string> lstUser = new List<string>();
            string querySql = string.Format("select 用户名 from dbo.用户表");
            DataTable dt = SQLHelper.ExecuteDt(querySql);
            foreach (DataRow dr in dt.Rows)
            {
                lstUser.Add(dr["用户名"].ToString());
            }
            return lstUser;
        }

        [OperationContract]
        public bool SendWTData(JyDataSet.已发委托DataTable dtWT, string serverMac)
        {
            try
            {
                if (dtWT.Rows.Count == 0)
                {
                    Program.logger.LogInfo(string.Format("接收server-{0}的已发委托DataTable.Rows=0", serverMac));
                    return false;
                }
                JyDataSet.已发委托Row firstRow1 = dtWT[0];
                //判断当前MAC是否已经更新数据
                string sqlMac = string.Format("select 更新日期 from dbo.日志表 where 服务器MAC='{0}' and 数据类型='委托'", serverMac);
                DataTable dtMac = SQLHelper.ExecuteDt(sqlMac);
                if (dtMac.Rows.Count > 0)
                {
                    int cDate = Convert.ToInt32(firstRow1.日期.ToString("yyyyMMdd"));
                    if (cDate == Convert.ToInt32(dtMac.Rows[0]["更新日期"].ToString()))
                    {
                        string updateType = ConfigMain.GetConfigValue("UpdateType");
                        if ("0".Equals(updateType))
                        {
                            //删除原有server Mac的数据
                            string sqlDel = string.Format("delete from dbo.已发委托 where 服务器MAC='{0}' and 日期='{1}'", serverMac, firstRow1.日期.ToString("yyyyMMdd"));
                            SQLHelper.ExecuteSql(sqlDel);
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("begin ");
                Program.logger.LogInfo(string.Format("委托数据入库开始：{0},当前server-{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), serverMac));
                foreach (JyDataSet.已发委托Row 已发委托Row1 in dtWT)
                {
                    string sql = string.Format("insert into dbo.已发委托(日期,组合号,委托编号,交易员,状态说明,市场代码,证券代码,证券名称,买卖方向,成交价格,成交数量,委托价格,委托数量,撤单数量,服务器MAC)values('{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}',{8},'{9}',{10},{11},{12},{13},'{14}');",
                         已发委托Row1.日期.ToString("yyyyMMdd"),
                         已发委托Row1.组合号,
                         已发委托Row1.委托编号,
                         已发委托Row1.交易员,
                         已发委托Row1.状态说明,
                         已发委托Row1.市场代码,
                         已发委托Row1.证券代码,
                         已发委托Row1.证券名称,
                         已发委托Row1.买卖方向,
                         已发委托Row1.成交价格,
                         已发委托Row1.成交数量,
                         已发委托Row1.委托价格,
                         已发委托Row1.委托数量,
                         已发委托Row1.撤单数量,
                         serverMac
                         );
                    sb.Append(sql);
                }
                sb.Append(" end;");
                SQLHelper.ExecuteSql(sb.ToString());

                //更新日志表
                string sqlLog = string.Empty;
                SqlParameter[] sqlParm;
                if (dtMac.Rows.Count > 0)
                {
                    sqlLog = "update dbo.日志表 set 更新日期=@更新日期,更新数量=@更新数量 where 服务器MAC=@服务器MAC and 数据类型='委托'";
                    sqlParm = new SqlParameter[] { new SqlParameter("@更新日期", Convert.ToInt32(firstRow1.日期.ToString("yyyyMMdd"))), new SqlParameter("@更新数量", dtWT.Rows.Count), new SqlParameter("@服务器MAC", serverMac) };
                }
                else
                {
                    sqlLog = "insert into dbo.日志表(服务器MAC,更新数量,数据类型,更新日期)values(@服务器MAC,@更新数量,@数据类型,@更新日期)";
                    sqlParm = new SqlParameter[] { new SqlParameter("@更新日期", Convert.ToInt32(firstRow1.日期.ToString("yyyyMMdd"))), new SqlParameter("@更新数量", dtWT.Rows.Count), new SqlParameter("@数据类型", "委托"), new SqlParameter("@服务器MAC", serverMac) };
                }

                SQLHelper.ExecuteSql(sqlLog, sqlParm);

                Program.logger.LogInfo(string.Format("委托数据入库结束：{0},当前server-{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), serverMac));

            }
            catch (Exception ex)
            {

                Program.logger.LogInfo(string.Format("委托数据入库异常：{0},当前server-{1}", ex.Message, serverMac));
                return false;
            }
            return true;
        }

        [OperationContract]
        public JyDataSet.已发委托DataTable QueryWTData(DateTime startDate, DateTime endDate, string strWhere)
        {
            StringBuilder sb = new StringBuilder();

            JyDataSet.已发委托DataTable dtWt = new JyDataSet.已发委托DataTable();
            string sql = string.Format("select 日期,组合号,委托编号,交易员,状态说明,市场代码,证券代码,证券名称,买卖方向,成交价格,成交数量,委托价格,委托数量,撤单数量  from dbo.已发委托 where 日期>='{0}' and 日期<='{1}'", startDate, endDate);
            sb.Append(sql);

            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append(string.Format(" and {0}", strWhere));
            }
            DataTable resultDt = SQLHelper.ExecuteDt(sb.ToString());
            foreach (DataRow dr in resultDt.Rows)
            {
                JyDataSet.已发委托Row newRow = dtWt.New已发委托Row();
                dtWt.Add已发委托Row(newRow);
                newRow.日期 = Convert.ToDateTime(dr["日期"]);
                newRow.组合号 = dr["组合号"].ToString();
                newRow.委托编号 = dr["委托编号"].ToString();
                newRow.交易员 = dr["交易员"].ToString();
                newRow.状态说明 = dr["状态说明"].ToString();
                newRow.市场代码 = Convert.ToByte(dr["市场代码"]);
                newRow.证券代码 = dr["证券代码"].ToString();
                newRow.证券名称 = dr["证券名称"].ToString();
                newRow.买卖方向 = Convert.ToInt32(dr["买卖方向"]);
                newRow.成交价格 = Convert.ToDecimal(dr["成交价格"]);
                newRow.成交数量 = Convert.ToDecimal(dr["成交数量"]);
                newRow.委托价格 = Convert.ToDecimal(dr["委托价格"]);
                newRow.委托数量 = Convert.ToDecimal(dr["委托数量"]);
                newRow.撤单数量 = Convert.ToDecimal(dr["撤单数量"]);

            }
            return dtWt;
        }

        [OperationContract]
        public bool SendQuotaData(JyDataSet.额度分配DataTable dtLimit, string serverMac)
        {
            try
            {
                if (dtLimit.Rows.Count == 0)
                {
                    Program.logger.LogInfo(string.Format("接收server-{0}的额度分配DataTable.Rows=0", serverMac));
                    return false;
                }
                //判断当前MAC是否已经更新数据
                string sqlMac = string.Format("select 更新日期 from dbo.日志表 where 服务器MAC='{0}' and 数据类型='额度'", serverMac);
                DataTable dtMac = SQLHelper.ExecuteDt(sqlMac);
                if (dtMac.Rows.Count > 0)
                {
                    int cDate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                    if (cDate == Convert.ToInt32(dtMac.Rows[0]["更新日期"].ToString()))
                    {
                        string updateType = ConfigMain.GetConfigValue("UpdateType");
                        if ("0".Equals(updateType))
                        {
                            //删除原有server Mac的数据
                            string sqlDel = string.Format("delete from dbo.额度分配 where 服务器MAC='{0}' and 日期={1}", serverMac, Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")));
                            SQLHelper.ExecuteSql(sqlDel);
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                Dictionary<int, decimal> dictPrice = new Dictionary<int, decimal>();
                //sb.Append("begin ");
                Program.logger.LogInfo(string.Format("额度分配数据入库开始：{0},当前server-{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), serverMac));
                foreach (JyDataSet.额度分配Row 额度分配Row1 in dtLimit)
                {
                    decimal closePrice;
                    int iCode = Convert.ToInt32(额度分配Row1.证券代码.Trim());
                    if (dictPrice.ContainsKey(iCode))
                    {
                        closePrice = dictPrice[iCode];
                    }
                    else
                    {
                        closePrice = CommonUtils.GetClosePrice(额度分配Row1.证券代码.Trim());
                    }
                    string sql = string.Format("insert into dbo.额度分配(交易员,证券代码,组合号,市场,证券名称,拼音缩写,买模式,卖模式,交易额度,手续费率,服务器MAC,收盘价,日期,分组)values('{0}','{1}','{2}',{3},'{4}','{5}',{6},{7},{8},{9},'{10}',{11},{12},'{13}');",
                         额度分配Row1.交易员,
                         额度分配Row1.证券代码,
                         额度分配Row1.组合号,
                         额度分配Row1.市场,
                         额度分配Row1.证券名称,
                         额度分配Row1.拼音缩写,
                         额度分配Row1.买模式,
                         额度分配Row1.卖模式,
                         额度分配Row1.交易额度,
                         额度分配Row1.手续费率,
                         serverMac,
                         closePrice,
                         Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")),
                         额度分配Row1.分组
                         );
                    //sb.Append(sql);
                    try
                    {
                        SQLHelper.ExecuteSql(sql);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                //sb.Append(" end;");
                //SQLHelper.ExecuteSql(sb.ToString());

                //更新日志表
                string sqlLog = string.Empty;
                SqlParameter[] sqlParm;
                if (dtMac.Rows.Count > 0)
                {
                    sqlLog = "update dbo.日志表 set 更新日期=@更新日期,更新数量=@更新数量 where 服务器MAC=@服务器MAC and 数据类型='委托'";
                    sqlParm = new SqlParameter[] { new SqlParameter("@更新日期", Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))), new SqlParameter("@更新数量", dtLimit.Rows.Count), new SqlParameter("@服务器MAC", serverMac) };
                }
                else
                {
                    sqlLog = "insert into dbo.日志表(服务器MAC,更新数量,数据类型,更新日期)values(@服务器MAC,@更新数量,@数据类型,@更新日期)";
                    sqlParm = new SqlParameter[] { new SqlParameter("@更新日期", Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"))), new SqlParameter("@更新数量", dtLimit.Rows.Count), new SqlParameter("@数据类型", "额度"), new SqlParameter("@服务器MAC", serverMac) };
                }

                SQLHelper.ExecuteSql(sqlLog, sqlParm);

                Program.logger.LogInfo(string.Format("额度分配数据入库结束：{0},当前server-{1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), serverMac));

            }
            catch (Exception ex)
            {
                Program.logger.LogInfo(string.Format("额度分配数据入库异常：{0},当前server-{1}", ex.Message, serverMac));
                return false;
            }
            return true;
        }
    }
}
