using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupClient
{
    public class Tool
    {
        public static DataTable ChangeDataStringToTable(string Data)
        {
            if (Data == string.Empty)
            {
                return null;
            }


            DataTable DataTable1 = new DataTable("Result");

            string[] Lines = Data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ColumnNames = Lines[0].Split(new char[] { '\t' });
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                if (DataTable1.Columns.IndexOf(ColumnNames[i]) == -1)
                {
                    DataTable1.Columns.Add(ColumnNames[i].Trim());
                }
                else
                {
                    DataTable1.Columns.Add(ColumnNames[i].Trim() + "_" + i.ToString());
                }
            }


            for (int i = 1; i < Lines.Length; i++)
            {
                string[] Cells = Lines[i].Split(new char[] { '\t' });
                DataTable1.Rows.Add(Cells);
            }

            return DataTable1;
        }
        /// 执行DataTable中的查询返回新的DataTable
        /// </summary>
        /// <param name="dt">源数据DataTable</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public static DataTable GetNewDataTable(DataTable dt, string condition)
        {
            DataTable newdt = new DataTable();
            newdt = dt.Clone();
            DataRow[] dr = dt.Select(condition);
            for (int i = 0; i < dr.Length; i++)
            {
                newdt.ImportRow((DataRow)dr[i]);
            }
            return newdt;
        }

        public static string[] QueryDataTableByWhere(DataTable dt, string condition)
        {
            string[] result = null;
            if (dt == null || dt.Rows.Count == 0) return result;
            DataRow[] dr = dt.Select(condition);
            if (dr.Length == 0) return result;
            result = new string[dr.Length + 1];
            result[0] = GetColumnsByDataTable(dt);
            for (int i = 0; i < dr.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    sb.AppendFormat("{0}\t", dr[i][k].ToString());
                }
                result[i + 1] = sb.ToString().TrimEnd('\t');
            }
            return result;
        }

        public static string GetColumnsByDataTable(DataTable dt)
        {
            StringBuilder sbColumns = new StringBuilder();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sbColumns.AppendFormat("{0}\t", dt.Columns[i].ColumnName);
            }

            return sbColumns.ToString().TrimEnd('\t');
        }

        const int StartSendOrderHour = 9;
        const int EndSendOrderHour = 15;
        public static bool IsSendOrderTimeFit()
        {
            if (DateTime.Now.Hour >= StartSendOrderHour && DateTime.Now.Hour < EndSendOrderHour)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
