using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YjDataClient.Common;
using YJDataClient.Common;
using YJDataClient.ServiceReference1;

namespace YJDataClient
{
    public partial class MainForm : Form
    {
        private static int iStartDate;
        private static int iEndDate;

        private OrderForm frmOrder;
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.dateTimePicker1.Value = DateTime.Now;
            this.dateTimePicker2.Value = DateTime.Now;
            this.comboBox1.SelectedIndex = 0;
            this.comboBox1.Visible = false;

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.lbInfo.Text = "正在查询...";
                string[] arrSDate = this.dateTimePicker1.Text.Split(new char[] { ' ' })[0].Split('/');
                if (arrSDate.Length != 3)
                {
                    MessageBox.Show("开始日期格式不正确！", "Error");
                    this.lbInfo.Text = "";
                    return;
                }
                int startDate = int.Parse(string.Format("{0}{1}{2}", arrSDate[0], arrSDate[1].PadLeft(2, '0'), arrSDate[2].PadLeft(2, '0')));

                string[] arrEDate = this.dateTimePicker2.Text.Split(new char[] { ' ' })[0].Split('/');
                if (arrEDate.Length != 3)
                {
                    MessageBox.Show("结束日期格式不正确！", "Error");
                    this.lbInfo.Text = "";
                    return;
                }
                int endDate = int.Parse(string.Format("{0}{1}{2}", arrEDate[0], arrEDate[1].PadLeft(2, '0'), arrEDate[2].PadLeft(2, '0')));

                if (startDate > endDate)
                {
                    MessageBox.Show("结束日期不能小于开始日期", "Error");
                    this.lbInfo.Text = "";
                    return;
                }
                JyDataSet.业绩统计DataTable 业绩统计DataTable1 = Program.DataServiceClient.QueryYJData(startDate, endDate);
                this.dataGridView1.DataSource = new DataTable();
                //按交易员统计
                if (this.radioButton交易员.Checked)
                {
                    JyDataSet.交易员业绩统计DataTable userDt = Program.DataServiceClient.QueryUserYjData(startDate, endDate);
                    this.dataGridView1.DataSource = CommonUtils.SummaryUserData(userDt);
                    this.dataGridView1.Columns["分组"].Visible = false;
                    this.dataGridView1.Columns[8].DefaultCellStyle.BackColor = Color.Yellow;

                }
                else if (this.radioButton组合号.Checked)
                {
                    JyDataSet.分帐户业绩统计DataTable groupDt = Program.DataServiceClient.QueryGroupYjData(startDate, endDate, this.comboBox1.SelectedItem.ToString());
                    this.dataGridView1.DataSource = CommonUtils.SummaryGroupData(groupDt);
                    this.dataGridView1.Columns["分组"].Visible = false;
                    this.dataGridView1.Columns[6].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (this.radioButton分组.Checked)
                {
                    JyDataSet.分组业绩统计DataTable userRegionDt = Program.DataServiceClient.QueryUserRegionYjData(startDate, endDate);
                    this.dataGridView1.DataSource = CommonUtils.SummaryUserRegion(userRegionDt);
                }
                this.lbInfo.Text = string.Format("查询结果：{0} 条", this.dataGridView1.Rows.Count == 0 ? 0 : this.dataGridView1.Rows.Count - 1);
                iStartDate = startDate;
                iEndDate = endDate;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
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
                StringBuilder sb = new StringBuilder();
                if (this.radioButton交易员.Checked)
                {
                    sb.Append("交易员业绩统计_");
                }
                else if (this.radioButton组合号.Checked)
                {
                    sb.Append("分账户业绩统计_");
                }
                else if (this.radioButton分组.Checked)
                {
                    sb.Append("分组业绩统计_");
                }
                if (iStartDate == iEndDate)
                {
                    sb.Append(iStartDate);
                }
                else
                {
                    sb.Append(string.Format("{0}-{1}", iStartDate, iEndDate));
                }
                Excel.Export(dataGridView1, sb.ToString().Trim());

                this.lbInfo.Text = "数据已导出";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.ColumnIndex < 0)
            //{
            //    return;
            //}
            //if (this.dataGridView1.Columns[e.ColumnIndex].Equals("日期"))
            //{
            //    this.dataGridView1.Columns[e.ColumnIndex].FillWeight = 140;
            //}
            //else if (this.dataGridView1.Columns[e.ColumnIndex].Equals("分组"))
            //{
            //    this.dataGridView1.Columns[e.ColumnIndex].Visible = false;
            //}

            //if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Contains("合计"))
            //{
            //    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
            //}
            //if (this.dataGridView1.Columns[e.ColumnIndex].HeaderCell.Value.Equals("利润"))
            //{
            //    this.dataGridView1.Columns[e.ColumnIndex].DefaultCellStyle.BackColor = Color.Yellow;
            //}
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex > -1)
            //{
            //    if (e.RowIndex % 2 == 0)
            //        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            //    else
            //        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.AliceBlue;

            //}
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.RowIndex > -1)
            //{
            //    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PowderBlue;
            //}
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //foreach (DataGridViewColumn c in dataGridView1.Columns)
            //{
            //    switch (c.Name)
            //    {
            //        case "日期":
            //            c.FillWeight = 180;
            //            break;
            //        case "序号":
            //            c.FillWeight = 80;
            //            break;
            //        case "账户":
            //            c.FillWeight = 80;
            //            break;
            //        case "交易股数":
            //            c.FillWeight = 120;
            //            break;
            //        case "使用市值":
            //            c.FillWeight = 120;
            //            break;
            //        case "使用效率":
            //            c.FillWeight = 120;
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        private void 账户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserForm frmUser = new UserForm();
            frmUser.StartPosition = FormStartPosition.CenterScreen;
            frmUser.ShowDialog();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strHead = this.dataGridView1.Columns[e.ColumnIndex].Name;

            switch (strHead)
            {
                case "隔夜仓":
                case "其他":
                case "T1":
                    //当前行相关值修改
                    decimal 隔夜仓 = Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["隔夜仓"].Value);
                    decimal 其他 = Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["其他"].Value);
                    decimal T1 = Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["T1"].Value);
                    decimal NET = Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["NET"].Value);
                    decimal 交易额 = Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["交易额"].Value);
                    decimal 利润 = Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["利润"].Value);

                    this.dataGridView1.Rows[e.RowIndex].Cells["NET"].Value = 利润 + 隔夜仓 + 其他 + T1;
                    this.dataGridView1.Rows[e.RowIndex].Cells["效率"].Value = 交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["NET"].Value) / 交易额 * 100, 2, MidpointRounding.AwayFromZero));
                    this.dataGridView1.Rows[e.RowIndex].Cells["使用效率"].Value = decimal.Round(Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["效率"].Value.ToString().Replace("%", "").Trim()) / 100 * Convert.ToDecimal(this.dataGridView1.Rows[e.RowIndex].Cells["使用率"].Value.ToString().Replace("%", "").Trim()) / 100 * 10000, 2);
                    
                    //合计行相关值修改
                    int idx = this.dataGridView1.Rows.Count - 1;
                    decimal Total隔夜仓, Total其他, TotalT1, Total利润;
                    Total隔夜仓 = 0;
                    Total其他 = 0;
                    TotalT1 = 0;
                    Total利润 = 0;
                    decimal Total交易额 = Convert.ToDecimal(this.dataGridView1.Rows[idx].Cells["交易额"].Value);

                    for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
                    {
                        Total隔夜仓 += Convert.ToDecimal(this.dataGridView1.Rows[i].Cells["隔夜仓"].Value);
                        Total其他 += Convert.ToDecimal(this.dataGridView1.Rows[i].Cells["其他"].Value);
                        TotalT1 += Convert.ToDecimal(this.dataGridView1.Rows[i].Cells["T1"].Value);
                        Total利润 += Convert.ToDecimal(this.dataGridView1.Rows[i].Cells["利润"].Value);
                    }
                    this.dataGridView1.Rows[idx].Cells["隔夜仓"].Value = Total隔夜仓;
                    this.dataGridView1.Rows[idx].Cells["其他"].Value = Total其他;
                    this.dataGridView1.Rows[idx].Cells["T1"].Value = TotalT1;
                    this.dataGridView1.Rows[idx].Cells["NET"].Value = Total利润 + Total隔夜仓 + Total其他 + TotalT1;
                    this.dataGridView1.Rows[idx].Cells["效率"].Value = 交易额 == 0 ? "0.00%" : string.Format("{0}%", Math.Round(Convert.ToDecimal(this.dataGridView1.Rows[idx].Cells["NET"].Value) / 交易额 * 100, 2, MidpointRounding.AwayFromZero));
                    this.dataGridView1.Rows[idx].Cells["使用效率"].Value = decimal.Round(Convert.ToDecimal(this.dataGridView1.Rows[idx].Cells["效率"].Value.ToString().Replace("%", "").Trim()) / 100 * Convert.ToDecimal(this.dataGridView1.Rows[idx].Cells["使用率"].Value.ToString().Replace("%", "").Trim()) / 100 * 10000, 2);
                    break;
            }
        }

        private void 委托查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmOrder == null)
                frmOrder = new OrderForm();
            else
                frmOrder.Activate();
            frmOrder.Show();

        }

        private void radioButton组合号_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton组合号.Checked)
            {
                this.comboBox1.Visible = true;
            }
            else
            {
                this.comboBox1.Visible = false;
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (this.radioButton交易员.Checked)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString().Contains("合计"))
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
            else if (this.radioButton组合号.Checked)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Contains("合计"))
                {
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }


    }
}
