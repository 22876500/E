using AASClient.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class PubStocksForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        static string[] DisplayColumn = new string[] { "证券代码", "证券名称", "可卖数量" };

        public Action<PublicStock> OnStockClick;

        public static DataTable PubStockDataSouce;

        public static List<PublicStock> PublicStockList { get; set; }

        public PubStocksForm()
        {
            InitializeComponent();
        }

        private void PubStocks_Load(object sender, EventArgs e)
        {
            try
            {
                var result = Program.AASServiceClient.QueryPubStock("");
                if (result.StartsWith("0|"))
                {
                    var resultDataStr = result.Substring(2);
                    if (!string.IsNullOrEmpty(resultDataStr))
                    {
                        var dt = Tool.ChangeDataStringToTable(resultDataStr);
                        DeleteTableData(dt);
                        
                        PublicStockList = new List<PublicStock>();
                        foreach (DataRow row in dt.Rows)
                        {
                            PublicStockList.Add(new PublicStock() {
                                StockCode = row["证券代码"].ToString(),
                                StockName = row["证券名称"].ToString(),
                                CanSaleCount = int.Parse(row["可卖数量"].ToString()),
                                Market = (byte)((row["交易所名称"].ToString()).IndexOf("上海") > 0 ? 1 : 0),
                            });
                        }
                        
                        //var code = row.Cells["证券代码"].Value + "";
                        //var name = row.Cells["证券名称"].Value + "";
                        //var market = (row.Cells["交易所名称"].Value + "").IndexOf("上海") > 0 ? (byte)1 : (byte)0;

                        this.bindingSource公共券池.DataSource = PublicStockList;
                        dataGridView公共券池.DataSource = this.bindingSource公共券池;
                        foreach (DataGridViewColumn column in this.dataGridView公共券池.Columns)
                        {
                            column.Visible = DisplayColumn.Contains(column.HeaderText);
                        }
                        dataGridView公共券池.Columns["帐号类别"].Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show(result.Substring(2));
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("公共券池查询异常!\r\n  Message:{0}\r\n  StackTrace:{1}", ex.Message, ex.StackTrace);
            }
        }

        private static void DeleteTableData(DataTable dt)
        {
            if (dt.Columns.Contains("帐号类别"))
            {
                dt.Columns.Remove(dt.Columns["帐号类别"]);
            }
            if (dt.Columns.Contains("可卖数量"))
            {
                decimal canBuyCount;
                for (int i = dt.Rows.Count - 1; i > -1; i--)
                {
                    if (!decimal.TryParse(dt.Rows[i]["可卖数量"] + "", out canBuyCount) || canBuyCount <= 0)
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }
            }
        }

    
        private void dataGridView公共券池_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //双击时显示到lv2窗口
            if (dataGridView公共券池.SelectedCells.Count > 0)
            {
                try
                {
                    var row = dataGridView公共券池.SelectedCells[0].OwningRow;
                    
                    var pubStockItem = row.DataBoundItem as PublicStock;
                    

                    if (OnStockClick != null && pubStockItem.CanSaleCount > 0)
                    {
                        OnStockClick.Invoke(pubStockItem);
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("{0} {1}", ex.Message, ex.StackTrace);
                }
                

                //B06 证券代码	证券名称	融券数量上限	交易所代码	交易所名称	保留信息
                //A01 帐号类别，交易所名称，证券代码，证券名称，融券保证金比例，可卖数量，备注，保留信息
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Program.AASServiceClient.State == System.ServiceModel.CommunicationState.Opened)
                {
                    var result = Program.AASServiceClient.QueryPubStock("");
                    if (result.StartsWith("0|"))
                    {
                        var resultDataStr = result.Substring(2);
                        if (!string.IsNullOrEmpty(resultDataStr))
                        {
                            var dt = Tool.ChangeDataStringToTable(resultDataStr);
                            //DeleteTableData(dt);
                            List<string> codes = new List<string>();
                            foreach (DataRow row in dt.Rows)
                            {
                                var code = row["证券代码"] + "";
                                codes.Add(code);

                                var entity = PublicStockList.FirstOrDefault(_=>_.StockCode == code);
                                if (entity == null)
                                {
                                    //帐号类别，交易所名称，证券代码，证券名称，融券保证金比例，可卖数量，备注，保留信息
                                    PublicStockList.Add(new PublicStock()
                                    {
                                        StockCode = code,
                                        StockName = row["证券代码"].ToString(),
                                        CanSaleCount = int.Parse(row["可卖数量"].ToString()),
                                        Market = (byte)(row["可卖数量"].ToString().IndexOf("") > 0 ? 1 : 0)
                                    });
                                }
                                else
                                {
                                    entity.CanSaleCount = int.Parse(row["可卖数量"].ToString());
                                }
                            }
                            if (codes.Count < PublicStockList.Count)
                            {
                                foreach (var item in PublicStockList)
                                {
                                    if (!codes.Contains(item.StockCode))
                                    {
                                        item.CanSaleCount = 0;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        this.timer1.Stop();
                        //Program.logger.LogRunning("公共券池查询错误,{0}", result.Substring(2));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("公共券池查询异常,{0}", ex.Message);
            }
        }

        private void textBoxStockCode_TextChanged(object sender, EventArgs e)
        {
            if (Regex.IsMatch(textBoxStockCode.Text, "^[0-9]{6}$"))
            {
                var item = PublicStockList.FirstOrDefault(_ => _.StockCode == textBoxStockCode.Text);
                if (item != null)
                {
                    foreach (DataGridViewRow row in dataGridView公共券池.Rows)
                    {
                        if (row.Cells["StockCode"].Value.ToString() == item.StockCode)
                        {
                            //row.Selected = true;
                            dataGridView公共券池.CurrentCell = row.Cells[0]; 
                            break;
                        }
                    }
                }
            }
        }
    }
}
