using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YJDataClient.Common;

namespace YJDataClient
{
    public partial class OrderForm : Form
    {
        private static int iStartDate;
        private static int iEndDate;
        public OrderForm()
        {
            InitializeComponent();
            this.Load += OrderForm_Load;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.dateTimePicker1.Value = DateTime.Now;
            this.dateTimePicker2.Value = DateTime.Now;

            this.comboBox1.SelectedIndex = 0;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.lbInfo.Text = "正在查询...";
                string strWhere = string.Empty;
                string[] arrSDate = this.dateTimePicker1.Text.Split(new char[] { ' ' })[0].Split('/');
                if (arrSDate.Length != 3)
                {
                    MessageBox.Show("开始日期格式不正确！", "Error");
                    this.lbInfo.Text = "";
                    return;
                }
                DateTime startDate = Convert.ToDateTime(string.Format("{0}-{1}-{2}", arrSDate[0], arrSDate[1].PadLeft(2, '0'), arrSDate[2].PadLeft(2, '0')));
                iStartDate = int.Parse(string.Format("{0}{1}{2}", arrSDate[0], arrSDate[1].PadLeft(2, '0'), arrSDate[2].PadLeft(2, '0')));
                string[] arrEDate = this.dateTimePicker2.Text.Split(new char[] { ' ' })[0].Split('/');
                if (arrEDate.Length != 3)
                {
                    MessageBox.Show("结束日期格式不正确！", "Error");
                    this.lbInfo.Text = "";
                    return;
                }
                DateTime endDate = Convert.ToDateTime(string.Format("{0}-{1}-{2}", arrEDate[0], arrEDate[1].PadLeft(2, '0'), arrEDate[2].PadLeft(2, '0')));
                iEndDate = int.Parse(string.Format("{0}{1}{2}", arrEDate[0], arrEDate[1].PadLeft(2, '0'), arrEDate[2].PadLeft(2, '0')));
                if (startDate > endDate)
                {
                    MessageBox.Show("结束日期不能小于开始日期", "Error");
                    this.lbInfo.Text = "";
                    return;
                }
                if (this.textBox1.Text.Trim() != string.Empty)
                {
                    strWhere = string.Format("{0}='{1}'", this.comboBox1.SelectedItem.ToString(), this.textBox1.Text.Trim());
                }
                ServiceReference1.JyDataSet.已发委托DataTable dtWT = Program.DataServiceClient.QueryWTData(startDate, endDate, strWhere);

                this.dataGridView1.DataSource = dtWT;

                this.lbInfo.Text = string.Format("查询结果：{0} 条", this.dataGridView1.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                this.lbInfo.Text = "";
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.lbInfo.Text = "数据正在导出...";
                if (this.dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("没有可导出的数据", "提示");
                    return;
                }
                string fileName = string.Format("委托数据{0}-{1}", iStartDate, iEndDate);
                Excel.Export(dataGridView1, fileName);
                this.lbInfo.Text = "数据已导出";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                this.lbInfo.Text = "";
            }
        }
    }
}
