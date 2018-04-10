using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class LogForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public LogForm()
        {
            InitializeComponent();
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            this.dataGridView交易日志.AutoGenerateColumns = false;

            this.bindingSource1.DataSource = Program.serverDb;
            this.bindingSource1.DataMember = "交易日志";

            this.dataGridView交易日志.DataSource = this.bindingSource1;


           


            this.dataGridView交易日志.Columns["信息"].Width = 200;
            this.dataGridView交易日志.Columns["委托数量"].DefaultCellStyle.Format = "f0";

            this.dataGridView交易日志.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

  


        
        private void 查看文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Process.Start(string.Format("{0}\\交易log\\{1:yyyy-MM-dd}.txt", Program.Current平台用户.用户名, DateTime.Today));
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void 清除日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void dataGridView交易日志_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "买卖方向")
            {
                int int1 = (int)e.Value;

                switch (int1)
                {
                    case 0:
                        e.Value = "买入";
                        break;
                    case 1:
                        e.Value = "卖出";
                        break;
                    default:
                        break;

                }
               
            }
        }

    }





    
}
