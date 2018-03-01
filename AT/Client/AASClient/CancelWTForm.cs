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
    public partial class CancelWTForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public TradeMainForm tradeMainForm;

        public CancelWTForm()
        {
            InitializeComponent();
        }

        private void CancelWTForm_Load(object sender, EventArgs e)
        {
           
            this.bindingSource1.DataSource = Program.jyDataSet;
            this.bindingSource1.DataMember = "委托";
            this.bindingSource1.Filter = "委托数量 > 成交数量+撤单数量";


            this.dataGridView委托.DataSource = this.bindingSource1;

            this.dataGridView委托.Columns["委托编号"].Visible = false;
            this.dataGridView委托.Columns["交易员"].Visible = false;
            this.dataGridView委托.Columns["市场代码"].Visible = false;
            this.dataGridView委托.Columns["组合号"].Visible = false;


            this.dataGridView委托.Columns["委托数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView委托.Columns["成交数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView委托.Columns["撤单数量"].DefaultCellStyle.Format = "f0";
        }

        private void dataGridView委托_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DateTime st = DateTime.Now;
            AASClient.AASServiceReference.JyDataSet.委托Row DataRow1 = (this.bindingSource1.Current as DataRowView).Row as AASClient.AASServiceReference.JyDataSet.委托Row;

            string Ret = null;
            if (DataRow1.证券代码.Length == 5)
            {
                Ret = Program.AASServiceClient.CancelAyersOrder(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.市场代码, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格);
            }
            else
            {
                Ret = Program.AASServiceClient.CancelOrder(Program.Current平台用户.用户名, DataRow1.组合号, DataRow1.证券代码, DataRow1.证券名称, DataRow1.市场代码, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格);
            }
            
            string[] Data = Ret.Split('|');
            var et = DateTime.Now;
            if (Data[1] != string.Empty)
            {
                Program.logger.LogJy(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格, "撤单失败: {0},耗时：{1}", Data[1], (et - st).TotalSeconds);
            }
            else
            {
                if (!this.tradeMainForm.dictOrderCancel.ContainsKey(DataRow1.证券代码))
                {
                    this.tradeMainForm.dictOrderCancel.Add(DataRow1.证券代码, new System.Collections.Concurrent.ConcurrentDictionary<string, string>());
                }
                this.tradeMainForm.dictOrderCancel[DataRow1.证券代码][DataRow1.委托编号] = et.ToString("HH:mm:ss fff");
                Program.logger.LogJy(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格, "撤单成功, 耗时{0}", (et - st).TotalSeconds);
            }
        }

        private void dataGridView委托_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int mmfx = (int)this.dataGridView委托["买卖方向", e.RowIndex].Value;

            if (mmfx == 0)
            {
                this.dataGridView委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
            else if (mmfx == 1)
            {
                this.dataGridView委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.dataGridView委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void dataGridView委托_KeyDown(object sender, KeyEventArgs e)
        {
            string KeyString = e.KeyCode.ToString();

            if (KeyString == Program.accountDataSet.参数.GetParaValue("撤单", "Escape"))
            {
                if (this.bindingSource1.Current != null)
                {
                    DateTime st = DateTime.Now;
                    AASClient.AASServiceReference.JyDataSet.委托Row DataRow1 = (this.bindingSource1.Current as DataRowView).Row as AASClient.AASServiceReference.JyDataSet.委托Row;

                    string Ret = null;
                    if (DataRow1.证券代码.Length == 5)
                    {
                        Ret = Program.AASServiceClient.CancelAyersOrder(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.市场代码, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格);
                    }
                    else
                    {
                        Ret = Program.AASServiceClient.CancelOrder(Program.Current平台用户.用户名, DataRow1.组合号, DataRow1.证券代码, DataRow1.证券名称, DataRow1.市场代码, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格);
                    }

                    //string Ret = Program.AASServiceClient.CancelOrder(Program.Current平台用户.用户名, DataRow1.组合号, DataRow1.证券代码, DataRow1.证券名称, DataRow1.市场代码, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格);
                    string[] Data = Ret.Split('|');
                    if (Data[1] != string.Empty)
                    {
                        Program.logger.LogJy(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格, "撤单失败: {0},耗时：{1}", Data[1], (DateTime.Now - st).TotalSeconds);
                    }
                    else
                    {
                        Program.logger.LogJy(Program.Current平台用户.用户名, DataRow1.证券代码, DataRow1.证券名称, DataRow1.委托编号, DataRow1.买卖方向, DataRow1.委托数量, DataRow1.委托价格, "撤单成功, 耗时：{0}", (DateTime.Now - st).TotalSeconds);
                    }
                }
            }
        }

        private void dataGridView委托_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
                    case 2:
                        e.Value = "融资买入";
                        break;
                    case 3:
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;

                }

            }
        }
    }
}
