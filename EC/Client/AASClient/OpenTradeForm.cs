using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class OpenTradeForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        static Regex regexStockECoin = new Regex("^[a-zA-Z]{5,}$", RegexOptions.Compiled);
        static Regex regexStockChina = new Regex("^[0-9]{6}$", RegexOptions.Compiled);
        static Regex regexStockHongkong = new Regex("^[0-9]{5}$", RegexOptions.Compiled);
        

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


            this.dataGridView订单.Columns["已开数量"].DefaultCellStyle.Format = "f4";
            this.dataGridView订单.Columns["已平数量"].DefaultCellStyle.Format = "f4";
            foreach (var item in Program.serverDb.订单)
            {
                //TDFData.DataCache.GetInstance().AddSub(item.证券代码);
                MarketAdapter.BinanceAdapter.Instance.Subscribe(item.证券名称);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task.Run(()=> {
                foreach (AASClient.AASServiceReference.DbDataSet.订单Row 订单Row1 in Program.serverDb.订单)
                {
                    Refresh订单(订单Row1);
                }

                string mvTotal = Program.serverDb.订单.Get市值合计().ToString();
                string profitAndLoss = Program.serverDb.订单.Get浮动盈亏().ToString();
                decimal gross = Program.serverDb.已平仓订单.Get毛利();
                decimal commissionTotal  = 0;
                foreach (var wtRow in Program.jyDataSet.委托)
                {
                    if (wtRow.成交数量 > 0)
                    {
                        commissionTotal += wtRow.Get交易费用();
                    }
                }

                this.BeginInvoke(new Action(()=> {
                    this.label市值合计.Text = mvTotal;
                    this.label浮动盈亏.Text = profitAndLoss;
                    this.label实现盈亏.Text = (gross - commissionTotal ).ToString();
                }));
                
            });
        }

        private static void Refresh订单(AASServiceReference.DbDataSet.订单Row 订单Row1)
        {
            decimal XJ, ZS;
            if (regexStockChina.IsMatch(订单Row1.证券代码))
            {
                if (TDFData.DataSourceConfig.IsUseTDFData && TDFData.DataCache.GetInstance().MarketNewDict.ContainsKey(订单Row1.证券代码))
                {
                    var marketData = TDFData.DataCache.GetInstance().MarketNewDict[订单Row1.证券代码];
                    XJ = (decimal)marketData.Match / 10000;
                    ZS = (decimal)marketData.PreClose / 10000;

                    订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), 2, MidpointRounding.AwayFromZero);
                    订单Row1.刷新浮动盈亏();
                }
                else if (Program.HqDataTable.ContainsKey(订单Row1.证券代码))
                {
                    DataTable DataTable1 = Program.HqDataTable[订单Row1.证券代码];
                    DataRow DataRow1 = DataTable1.Rows[0];
                    XJ = decimal.Parse((DataRow1["现价"] as string));
                    ZS = decimal.Parse((DataRow1["昨收"] as string));
                    订单Row1.当前价位 = Math.Round((XJ == 0 ? ZS : XJ), L2Api.Get精度(订单Row1.证券代码), MidpointRounding.AwayFromZero);
                    订单Row1.刷新浮动盈亏();
                }
            }
            else if (regexStockECoin.IsMatch(订单Row1.证券代码))
            {
                if (MarketAdapter.BinanceAdapter.Instance.PriceDict.ContainsKey(订单Row1.证券代码))
                {
                    XJ = MarketAdapter.BinanceAdapter.Instance.PriceDict[订单Row1.证券代码];
                    订单Row1.当前价位 = XJ;
                    订单Row1.刷新浮动盈亏();
                }
                else
                {
                    MarketAdapter.BinanceAdapter.Instance.Subscribe(订单Row1.证券代码);
                }
            }
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
            var color = fdyk > 0 ? Color.Red : (fdyk < 0 ? Color.Blue : Color.Black);
            dataGridView订单.Rows[e.RowIndex].DefaultCellStyle.ForeColor = color;
        }

        private void hlRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.AASServiceClient.RefreshOrderTable(Program.Current平台用户.用户名);
        }

        private void dataGridView订单_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Program.logger.LogInfo(string.Format("OpenTradeForm DataError {0}", (e.Exception.InnerException ?? e.Exception).Message));
        }
    }
}
