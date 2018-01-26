using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class TradeLimitForm : Form
    {
        public TradeLimitForm()
        {
            InitializeComponent();
        }

        private void TradeLimitForm_Load(object sender, EventArgs e)
        {
            this.dataGridView平台用户.AutoGenerateColumns = false;


            this.bindingSource平台用户.DataSource = Program.serverDb.平台用户;
            this.dataGridView平台用户.DataSource = this.bindingSource平台用户;

            this.bindingSource额度分配.DataSource = Program.serverDb.额度分配;
            this.dataGridView额度分配.DataSource = this.bindingSource额度分配;



            this.dataGridView额度分配.Columns["交易员"].Visible = false;
            this.dataGridView额度分配.Columns["拼音缩写"].Visible = false;
            this.dataGridView额度分配.Columns["组合号"].Visible = false;
            this.dataGridView额度分配.Columns["市场"].Visible = false;
            this.dataGridView额度分配.Columns["买模式"].Visible = false;
            this.dataGridView额度分配.Columns["卖模式"].Visible = false;
            this.dataGridView额度分配.Columns["手续费率"].Visible = false;

            this.dataGridView额度分配.Columns.Insert(2, new DataGridViewTextBoxColumn() { HeaderText = "可卖\n股数", Name = "可卖股数", Width = 60 });
            //this.dataGridView额度分配.Columns.Add("可卖股数", "可卖股数");
            this.dataGridView额度分配.Columns.Add("已卖股数", "已卖\n股数");
            this.dataGridView额度分配.Columns["证券代码"].HeaderText = "证券\n代码";
            this.dataGridView额度分配.Columns["证券名称"].HeaderText = "证券\n名称";
            this.dataGridView额度分配.Columns["交易额度"].HeaderText = "交易\n额度";

            this.dataGridView额度分配.Columns["交易额度"].DefaultCellStyle.Format = "f0";
            this.dataGridView额度分配.Columns["已卖股数"].DefaultCellStyle.Format = "f0";
            this.dataGridView额度分配.Columns["可卖股数"].DefaultCellStyle.Format = "f0";
            this.dataGridView额度分配.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            //this.dataGridView额度分配.Columns["可卖股数"]
            this.timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow DataGridViewRow1 in this.dataGridView额度分配.Rows)
            {
                string Zqdm = DataGridViewRow1.Cells["证券代码"].Value as string;
                decimal 交易额度 = (decimal)DataGridViewRow1.Cells["交易额度"].Value;

                decimal 已买股数 = 0;
                decimal 已卖股数 = 0;
                Program.jyDataSet.委托.Get已买卖股数(Zqdm, out 已买股数, out 已卖股数);
                

                decimal 可用股数 = 交易额度 - 已卖股数;


                DataGridViewRow1.Cells["已卖股数"].Value = 已卖股数;
                DataGridViewRow1.Cells["可卖股数"].Value = 可用股数;
            }
        }
    }
}
