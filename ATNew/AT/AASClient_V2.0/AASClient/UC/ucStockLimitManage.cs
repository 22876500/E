using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AASClient.AASServiceReference;
using System.IO;

namespace AASClient.UC
{
    public partial class ucStockLimitManage : System.Windows.Forms.UserControl
    {
        public ucStockLimitManage()
        {
            InitializeComponent();
            this.Load += UcStockLimitManage_Load;
        }

        private void UcStockLimitManage_Load(object sender, EventArgs e)
        {
            
            this.dataGridView交易额度.DataSource = this.bindingSource交易额度;
            this.dataGridView交易额度.Columns["Column交易额度"].DefaultCellStyle.Format = "f0";
            Task.Run(()=> 
            {
                try
                {
                    var dt = Program.AASServiceClient.QueryTradeLimit();
                    string[] accList = Program.AASServiceClient.QueryQSNameList();
                    this.Invoke(new Action(() =>
                    {
                        comboBox组合号.Items.Clear();
                        comboBox组合号.Items.Add("全部");
                        foreach (var item in accList)
                        {
                            comboBox组合号.Items.Add(item);
                        }
                        
                        labelLoading.Visible = false;
                        this.bindingSource交易额度.DataSource = dt;
                    }));
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("风控额度分配管理界面初始化异常:" + ex.Message);
                }
            });
        }

        private void comboBox组合号_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridViewAutoFilter.DataGridViewAutoFilterColumnHeaderCell.ValueListForFilter.Clear();
            string filterInfo = bindingSource交易额度.Filter;
            if (comboBox组合号.Text == "全部")
            {
                bindingSource交易额度.Filter = null;
            }
            else
            {
                bindingSource交易额度.Filter = string.Format("组合号='{0}'", comboBox组合号.Text);
            }
        }

        private void textBox证券代码_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox证券名称_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox交易员_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonFilte_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilter.DataGridViewAutoFilterColumnHeaderCell.ValueListForFilter.Clear();
            bindingSource交易额度.Filter = null;

            StringBuilder sbFilter = new StringBuilder(64);
            string acc = comboBox组合号.Text;
            string stockID = textBox证券代码.Text.Trim();
            string stockName = textBox证券名称.Text.Trim();
            string trader = textBox交易员.Text.Trim();

            if (acc.Length > 0 && acc != "全部")
            {
                sbFilter.Append(string.Format("组合号='{0}'", comboBox组合号.Text));
            }

            AddFilteItem(sbFilter, "证券代码", stockID);
            AddFilteItem(sbFilter, "证券名称", stockName);
            AddFilteItem(sbFilter, "交易员", trader);
            bindingSource交易额度.Filter = sbFilter.ToString();
        }

        private static void AddFilteItem(StringBuilder sbFilter, string columnName, string columnValue)
        {
            if (columnValue.Length > 0)
            {
                if (sbFilter.Length > 0)
                    sbFilter.Append(" and ");

                sbFilter.Append(string.Format("{0} like '%{1}%'", columnName, columnValue));
            }
        }
        #region 交易额度管理

        private void dataGridView交易额度_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Name.Contains("市场"))
            {
                byte byte1 = (byte)e.Value;
                e.Value = byte1 == 0 ? "深圳" : "上海";
            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].Name.Contains("买模式"))
            {
                AASServiceReference.买模式 买模式1 = (AASServiceReference.买模式)e.Value;
                e.Value = 买模式1.ToString();
            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].Name.Contains("卖模式"))
            {
                AASServiceReference.卖模式 卖模式1 = (AASServiceReference.卖模式)e.Value;
                e.Value = 卖模式1.ToString();
            }
        }

        private void dataGridView交易额度_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            AASServiceReference.DbDataSet.额度分配Row DataRow1 = (this.bindingSource交易额度.Current as DataRowView).Row as AASServiceReference.DbDataSet.额度分配Row;
            ModifyTradeLimitForm ModifyTradeLimitForm1 = new ModifyTradeLimitForm(DataRow1);
            DialogResult DialogResult1 = ModifyTradeLimitForm1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
        }

        private void 添加交易额度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTradeLimitForm AddTradeLimitForm1 = new AddTradeLimitForm();
            DialogResult DialogResult1 = AddTradeLimitForm1.ShowDialog();
            if (DialogResult1 != DialogResult.OK)
            {
                return;
            }
            this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
        }

        private void 修改交易额度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var DataRow1 = (this.bindingSource交易额度.Current as DataRowView).Row as AASServiceReference.DbDataSet.额度分配Row;
            ModifyTradeLimitForm ModifyTradeLimitForm1 = new ModifyTradeLimitForm(DataRow1);
            DialogResult DialogResult1 = ModifyTradeLimitForm1.ShowDialog();
            if (DialogResult1 != DialogResult.OK)
            {
                return;
            }
            this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
        }

        private void 删除交易额度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelLoading.Text = "删除中……";
            labelLoading.Visible = true;
            AASServiceReference.DbDataSet.额度分配DataTable dt = new AASServiceReference.DbDataSet.额度分配DataTable();

            foreach (DataGridViewRow row in this.dataGridView交易额度.SelectedRows)
            {
                var o = (row.DataBoundItem as DataRowView).Row as AASServiceReference.DbDataSet.额度分配Row;
                if (o != null)
                {
                    dt.Add额度分配Row(o.交易员, o.证券代码, o.组合号, o.市场, o.证券名称, o.拼音缩写, o.买模式, o.卖模式, o.交易额度, o.手续费率);
                }
            }

            Task.Run(()=> 
            {
                foreach (var row in dt)
                {
                    try
                    {
                        string loadingInfo = string.Format("正在删除项：组合号{0},证券代码{1},交易员{2}", row.组合号, row.证券代码, row.交易员);
                        this.BeginInvoke(new Action(() => { labelLoading.Text = loadingInfo; }));
                        Program.AASServiceClient.DeleteTradeLimit(row.交易员, row.证券代码, row.组合号);
                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke(new Action(() => { labelLoading.Text = string.Format("删除额度分配项时发生异常，组合号{0},证券代码{1},交易员{2}\r\n异常信息:{3}", row.组合号, row.证券代码, row.交易员, ex.Message); }));
                    }
                    
                }
                this.BeginInvoke(new Action(()=> 
                {
                    this.bindingSource交易额度.DataSource = Program.AASServiceClient.QueryTradeLimit();
                    labelLoading.Text = "加载中……";
                    labelLoading.Visible = false;
                }));
                
            });   
        }

        private void 全部显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewAutoFilter.DataGridViewAutoFilterColumnHeaderCell.ValueListForFilter.Clear();
            this.bindingSource交易额度.Filter = null;
        }

        private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult DialogResult1 = this.openFileDialog1.ShowDialog();
            if (DialogResult1 != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }


            string[] FileContent = File.ReadAllLines(this.openFileDialog1.FileName, Encoding.Default);

            StringBuilder sbErr = new StringBuilder(128);
            Task.Run(() =>
            {
                AASClient.AASServiceReference.DbDataSet.平台用户DataTable 平台用户DataTable1 = Program.AASServiceClient.QueryUser();
                for (int i = 1; i < FileContent.Length; i++)
                {
                    string[] Data = FileContent[i].Split(',');
                    try
                    {
                        string stockID = UpdateStockCode(Data[1].Trim());
                        买模式 买模式1 = (买模式)Enum.Parse(typeof(买模式), Data[6], false);
                        卖模式 卖模式1 = (卖模式)Enum.Parse(typeof(卖模式), Data[7], false);
                        byte 市场 = byte.Parse(Data[3].Replace("深圳", "0").Replace("上海", "1"));
                        decimal 交易额度 = decimal.Parse(Data[8]);
                        decimal 手续费率 = decimal.Parse(Data[9]);

                        var userInfo = 平台用户DataTable1.FirstOrDefault(r => r.用户名.Equals(Data[0].Trim()));
                        if (userInfo != null)
                        {
                            Program.AASServiceClient.AddTradeLimit(Data[0], Data[1], Data[2], 市场, Data[4], Data[5], 买模式1, 卖模式1, 交易额度, 手续费率);
                        }
                        else
                        {
                            this.Invoke(new Action(() => { labelLoading.Text = string.Format("导入第{0}条记录[{1} {2}]失败,未找到该用户", i, Data[0], Data[1]); }));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => { MessageBox.Show(string.Format("导入第{0}条记录[{1} {2}]异常,{3}", i, Data[0], Data[1], ex.Message)); }));
                        break;
                    }
                }
                var dt = Program.AASServiceClient.QueryTradeLimit();
                this.Invoke(new Action(() =>
                {
                    this.bindingSource交易额度.DataSource = dt;
                }));
            });
        }

        private string UpdateStockCode(string v)
        {
            if (v.Length < 6)
            {
                for (int i = v.Length; i < 6; i++)
                {
                    v = "0" + v;
                }
            }
            return v;
        }
        #endregion
    }
}
