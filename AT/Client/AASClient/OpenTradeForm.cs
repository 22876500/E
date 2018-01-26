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
    public partial class OpenTradeForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public OpenTradeForm()
        {
            InitializeComponent();
        }

        private void OpenTradeForm_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = Program.serverDb;
            this.bindingSource1.DataMember = "订单";
            this.dataGridView订单.DataSource = this.bindingSource1;


            this.dataGridView订单.Columns["交易员"].Visible = false;
            this.dataGridView订单.Columns["组合号"].Visible = false;
            this.dataGridView订单.Columns["市场代码"].Visible = false;
            this.dataGridView订单.Columns["已开金额"].Visible = false;
            this.dataGridView订单.Columns["平仓时间"].Visible = false;
            this.dataGridView订单.Columns["平仓类别"].Visible = false;
            this.dataGridView订单.Columns["已平数量"].Visible = false;
            this.dataGridView订单.Columns["已平金额"].Visible = false;
            this.dataGridView订单.Columns["平仓价位"].Visible = false;

            this.dataGridView订单.Columns["开仓时间"].DefaultCellStyle.Format = "HH:mm:ss";


            this.dataGridView订单.Columns["已开数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView订单.Columns["已平数量"].DefaultCellStyle.Format = "f0";
            foreach (var item in Program.serverDb.订单)
            {
                TDFData.DataCache.GetInstance().AddSub(item.证券代码);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (AASClient.AASServiceReference.DbDataSet.订单Row 订单Row1 in Program.serverDb.订单)
            {
                decimal XJ, ZS;
                if (TDFData.DataSourceConfig.IsUseTDFData && TDFData.DataCache.GetInstance().MarketNewDict.ContainsKey(订单Row1.证券代码))
                {
                    var marketData = TDFData.DataCache.GetInstance().MarketNewDict[订单Row1.证券代码];
                    XJ = (decimal)marketData.Match / 10000;
                    ZS = (decimal)marketData.PreClose / 10000;
                    
                    //订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), L2Api.Get精度(订单Row1.证券代码), MidpointRounding.AwayFromZero);
                    订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), 2, MidpointRounding.AwayFromZero);
                    订单Row1.刷新浮动盈亏();
                    //Program.logger.LogRunning("根据TDF行情数据更新，证券代码{0}, 现价{1}, 昨收{2}, 计算后当前价位{3}", 订单Row1.证券代码, XJ, ZS, 订单Row1.当前价位);
                }
                else if (Program.HqDataTable.ContainsKey(订单Row1.证券代码))
                {
                    DataTable DataTable1 = Program.HqDataTable[订单Row1.证券代码];
                    DataRow DataRow1 = DataTable1.Rows[0];
                    XJ = decimal.Parse((DataRow1["现价"] as string));
                    ZS = decimal.Parse((DataRow1["昨收"] as string));
                    订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), L2Api.Get精度(订单Row1.证券代码), MidpointRounding.AwayFromZero);
                    订单Row1.刷新浮动盈亏();
                    //Program.logger.LogRunning("根据Tdx行情数据更新，证券代码{0}, 现价{1}, 昨收{2}, 计算后当前价位{3}", 订单Row1.证券代码, XJ, ZS, 订单Row1.当前价位);
                }
                else
                {
                    Program.logger.LogRunning("证券代码{0}, 无行情数据，将不更新当前价位及浮动盈亏数据", 订单Row1.证券代码);
                }
            }




            this.label市值合计.Text = Program.serverDb.订单.Get市值合计().ToString();
            this.label浮动盈亏.Text = Program.serverDb.订单.Get浮动盈亏().ToString();

            decimal 当日委托交易费用 = 0;
            foreach (AASClient.AASServiceReference.JyDataSet.委托Row 委托Row1 in Program.jyDataSet.委托.Where(r => r.成交数量 > 0))
            {
                当日委托交易费用 += 委托Row1.Get交易费用();
            }

            //decimal 当日委托交易费用 = Program.jyDataSet.委托.Get交易费用();
            decimal 毛利 = Program.serverDb.已平仓订单.Get毛利();
            this.label实现盈亏.Text = (毛利 - 当日委托交易费用).ToString();


        }

        private void dataGridView订单_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            if (this.dataGridView订单.Columns[e.ColumnIndex].Name == "开仓类别" || this.dataGridView订单.Columns[e.ColumnIndex].Name == "平仓类别")
            {
                int int1 = (int)e.Value;
                switch (int1)
                {
                    case 0:
                    case 2:
                    case 69:
                        e.Value = "多";
                        break;
                    case 1:
                    case 3:
                    case 70:
                        e.Value = "空";
                        break;
                    default:
                        e.Value = "X";
                        break;
                }
            }


           
        }

        private void dataGridView订单_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            decimal fdyk = (decimal)this.dataGridView订单["浮动盈亏", e.RowIndex].Value;

            if (fdyk > 0)
            {
                this.dataGridView订单.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
            else if (fdyk < 0)
            {
                this.dataGridView订单.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.dataGridView订单.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }
    }
}
