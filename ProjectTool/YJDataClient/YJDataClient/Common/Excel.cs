using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YJDataClient.Common
{
   public class Excel
    {
       public static void Export(DataGridView dataGridView1,string fileName)
       {
           SaveFileDialog saveFileDialog = new SaveFileDialog();
           saveFileDialog.Filter = "Excel 97-2003工作簿（*.xls）|*.xls";
           saveFileDialog.FilterIndex = 0;
           saveFileDialog.RestoreDirectory = true;
           saveFileDialog.CreatePrompt = true;
           saveFileDialog.Title = "导出Excel文件到";
           saveFileDialog.FileName = fileName;

           saveFileDialog.ShowDialog();

           Stream myStream;
           myStream = saveFileDialog.OpenFile();
           StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding("gb2312"));
           string str = "";
           try
           {
               //写标题     
               for (int i = 0; i < dataGridView1.ColumnCount; i++)
               {
                   if (dataGridView1.Columns[i].HeaderText.Equals("分组") && (fileName.Contains("交易员") || fileName.Contains("分帐户"))) { continue; }
                   if (i > 0)
                   {
                       str += "\t";
                   }
                   str += dataGridView1.Columns[i].HeaderText;
               }

               sw.WriteLine(str);
               //写内容   
               for (int j = 0; j < dataGridView1.Rows.Count; j++)
               {
                   string tempStr = "";
                   for (int k = 0; k < dataGridView1.Columns.Count; k++)
                   {
                       if (dataGridView1.Columns[k].HeaderText.Equals("分组") && (fileName.Contains("交易员") || fileName.Contains("分帐户"))) { continue; }
                       if (k > 0)
                       {
                           tempStr += "\t";
                       }
                       tempStr += dataGridView1.Rows[j].Cells[k].Value.ToString();
                   }
                   sw.WriteLine(tempStr);
               }
               sw.Close();
               myStream.Close();
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
    }
}
