using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATSInterface
{
    public class OledbAdapter
    {
        private static object Sync = new object();
        private static string _conn = null;
        private static object InstructionSync = new object();
        static string ConnectionString
        {
            get
            {
                if (_conn == null)
                {
                    lock (Sync)
                    {
                        var path = Utils.GetConfig("CATS_PATH");
                        _conn = string.Format("Provider=vfpoledb;Data Source={0};Collating Sequence=machine;", path);
                    }
                    
                }
                return _conn;
            }
        }

        public static DataTable GetDataTable(string sql)
        {
            var dt = new DataTable();
            using (OleDbConnection conn = new OleDbConnection(ConnectionString))
            {
                OleDbDataAdapter adpt = new OleDbDataAdapter(sql, ConnectionString);
                adpt.Fill(dt);
            }
            return dt;
        }

        public static DataTable GetInstruction(string sql)
        {
            var dt = new DataTable();
            lock (InstructionSync)
            {
                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {
                    OleDbDataAdapter adpt = new OleDbDataAdapter(sql, ConnectionString);
                    adpt.Fill(dt);
                }
            }
            return dt;
        }
        //public static int SetDataTable(string sql)
        //{
        //    int count;
        //    using (OleDbConnection conn = new OleDbConnection(ConnectionString))
        //    {
        //        conn.Open();

        //        OleDbCommand cmd = new OleDbCommand(sql, conn);
        //        count = cmd.ExecuteNonQuery();
        //        conn.Close();
        //    }
        //    return count;
        //}

        public static int SetCancelInstruction(string client_id, string acct_type, string acct, string order_no)
        {
            int count = -1;
            lock (InstructionSync)
            {
                //string sql = string.Format("insert into {0} (inst_type, client_id, acct_type, acct, order_no) values('{1}', '{2}', '{3}', '{4}', '{5}')", table, "1", clientID, accType, account, orderNo);
                string sql = string.Format(@"insert into instructions (inst_type, client_id, acct_type, acct, symbol, tradeside, ord_qty, ord_price, ord_type, ord_no, ord_param) 
values('C', '{0}', '{1}', '{2}', '', '', 0, 0, '', '{3}', '')", client_id, acct_type, acct, order_no);
                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    try
                    {
                        count = cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        string s = ex.Message + ex.StackTrace;
                        Utils.logger.LogInfo(s);
                    }
                }
            }
            return count;
        }


        public static int SetAddInstruction(string client_id, string acct_type, string acct, string symbol,string tradeside,string ord_qty, string ord_price, string ord_type)
        {
            int count = -1;
            lock (InstructionSync)
            {
                string sql = string.Format(@"insert into instructions (inst_type, client_id, acct_type, acct, symbol, tradeside, ord_qty, ord_price, ord_type, ord_no, ord_param) 
values('O', '{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, '{7}', '', '')", client_id, acct_type, acct, symbol, tradeside, decimal.Parse(ord_qty), decimal.Parse(ord_price), ord_type);
                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand(sql, conn);
                    //cmd.Parameters.AddWithValue("@inst_type", "0");
                    //cmd.Parameters.AddWithValue("@client_id", client_id);
                    //cmd.Parameters.AddWithValue("@acct_type", acct_type);
                    //cmd.Parameters.AddWithValue("@acct", acct);
                    //cmd.Parameters.AddWithValue("@symbol", symbol);
                    //cmd.Parameters.AddWithValue("@tradeside", tradeside);
                    //cmd.Parameters.AddWithValue("@ord_qty", ord_qty);
                    //cmd.Parameters.AddWithValue("@ord_price", ord_price);
                    //cmd.Parameters.AddWithValue("@ord_type", ord_type);
                    try
                    {
                        
                        count = cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        
                        string s = ex.Message + ex.StackTrace;
                        Utils.logger.LogInfo(s);
                        return -1;
                    }
                }
            }
            return count;
        }
    }
}
