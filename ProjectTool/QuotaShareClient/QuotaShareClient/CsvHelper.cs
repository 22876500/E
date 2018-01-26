using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuotaShareClient
{
    public class CsvHelper
    {
        /// <summary>
        /// 将DataTable中数据写入到CSV文件中
        /// </summary>
        /// <param name="dt">提供保存数据的DataTable</param>
        /// <param name="fileName">CSV的文件路径</param>
        public static void SaveCSV(DataTable dt, string fileName)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Csv文件（*.csv）|*.csv";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "导出Csv文件到";
            saveFileDialog.FileName = fileName;

            System.Windows.Forms.DialogResult result = saveFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                //Program.logger.LogInfo("文件\"{0}\"取消保存", fileName);
                return;
            }
            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.UTF8);
            string data = "";
            try
            {
                DataView dataView = dt.DefaultView;
                dataView.Sort = string.Format("{0} asc,{1} asc", CommonUtils.heads[0], CommonUtils.heads[1]);
                dt = dataView.ToTable();
                //dt.DefaultView.Sort = string.Format("{0} asc,{1} asc", CommonUtils.heads[0], CommonUtils.heads[1]);
                //写出列名称
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    data += dt.Columns[i].ColumnName.ToString();
                    if (i < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
                //写出各行数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    data = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string str = dt.Rows[i][j].ToString();
                        str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                        if (str.Contains(',') || str.Contains('"')
                            || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                        {
                            str = string.Format("\"{0}\"", str);
                        }

                        data += str;
                        if (j < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }
                sw.Close();
                myStream.Close();
                //Program.logger.LogInfo("文件\"{0}\"保存成功", fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
        }

        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCSV(string filePath)
        {
            List<string> lstLines = new List<string>();
            Encoding encoding = CommonUtils.GetEncoding(filePath); //Encoding.ASCII;//
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            StreamReader sr = new StreamReader(fs, encoding);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                //strLine = Common.ConvertStringUTF8(strLine, encoding);
                //strLine = Common.ConvertStringUTF8(strLine);
                if (lstLines.Contains(strLine)) { continue; }
                lstLines.Add(strLine);
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    columnCount = tableHead.Length;

                    try
                    {
                        IsFirst = false;
                        int code = int.Parse(tableHead[1]);


                        if (columnCount != CommonUtils.heads.Length)
                        {
                            MessageBox.Show("导入的额度分配表格式不正确");
                            //Program.logger.LogInfo(string.Format("导入的已分配额度表格式不一致,应为“{0}”{1}列", string.Join(",", CommonUtils.heads), CommonUtils.heads.Length));
                            break;
                        }
                        else
                        {

                            for (int i = 0; i < CommonUtils.heads.Length; i++)
                            {
                                DataColumn dc = new DataColumn(CommonUtils.heads[i]);
                                dt.Columns.Add(dc);
                            }
                        }

                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                    catch (Exception)
                    {

                        if (columnCount != CommonUtils.heads.Length)
                        {
                            MessageBox.Show("导入的额度分配表格式不正确");
                            //Program.logger.LogInfo(string.Format("导入的已分配额度表格式不一致,应为“{0}”{1}列", string.Join(",", CommonUtils.heads), CommonUtils.heads.Length));
                            break;
                        }
                        else
                        {
                            for (int i = 0; i < CommonUtils.heads.Length; i++)
                            {
                                DataColumn dc = new DataColumn(CommonUtils.heads[i]);
                                dt.Columns.Add(dc);
                            }
                        }
                    }

                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (j == 1)//股票列表
                        {
                            dr[j] = aryLine[j].Trim().PadLeft(6, '0');
                        }
                        else
                        {
                            dr[j] = aryLine[j].Trim();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            if (aryLine != null && aryLine.Length > 0)
            {
                dt.DefaultView.Sort = CommonUtils.heads[1] + " " + "asc";
            }

            sr.Close();
            fs.Close();
            return dt;
        }
    }
}
