using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AASClient
{
    enum 提示类型 { 涨到, 跌到 }
    enum 提示等级 {黄, 红 }
    public partial class AddRemindForm : Form
    {
        Regex codRegex = new Regex("[a-zA-Z]+(BTC|ETH|USDT)", RegexOptions.IgnoreCase);
        bool NeedPrice = false;
        public AddRemindForm()
        {
            InitializeComponent();
        }

        private void AddRemindForm_Load(object sender, EventArgs e)
        {
            this.comboBox提示类型.DataSource = System.Enum.GetNames(typeof(提示类型));
            this.comboBox提示等级.DataSource = System.Enum.GetNames(typeof(提示等级));
            this.comboBox证券代码.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.comboBox证券代码.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            foreach (string 证券代码 in Program.证券名称.Keys)
            {
                this.comboBox证券代码.AutoCompleteCustomSource.Add(string.Format("{0} {1}", 证券代码, 证券代码));
            }
        }

        private void AddRemindForm_Activated(object sender, EventArgs e)
        {
            this.comboBox证券代码.Focus();
        }

        private void textBox证券代码_KeyPress(object sender, KeyPressEventArgs e)
        {
            //如果输入的不是数字键，也不是回车键、Backspace键，则取消该输入
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)13 && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void comboBox证券代码_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetMatches();
            }
        }
        
        private void textBox证券代码_TextChanged(object sender, EventArgs e)
        {
            if (codRegex.IsMatch(this.comboBox证券代码.Text))
            {
                Program.TempZqdm = this.comboBox证券代码.Text;
                this.NeedPrice = true;
            }
        }
        private void comboBox证券代码_Leave(object sender, EventArgs e)
        {
            SetMatches();
        }

        private void SetMatches()
        {
            if (codRegex.IsMatch(this.comboBox证券代码.Text))
            {
                Match Match1 = codRegex.Match(this.comboBox证券代码.Text);
                string Zqdm = Match1.Groups[0].Value;
                this.comboBox证券代码.Text = Zqdm;
                Program.TempZqdm = this.comboBox证券代码.Text;
                this.NeedPrice = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Program.HqDataTable.ContainsKey(this.comboBox证券代码.Text))
            {
                if (this.NeedPrice)
                {
                    DataTable DataTable1 = Program.HqDataTable[this.comboBox证券代码.Text];
                    DataRow DataRow1 = DataTable1.Rows[0];
                    this.numericUpDown提示价格.Value = decimal.Parse((DataRow1["现价"] as string));

                    this.NeedPrice = false;
                }
            }

            this.label证券名称.Text = this.comboBox证券代码.Text;

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            AccountDataSet.价格提示Row 价格提示Row1 = Program.accountDataSet.价格提示.New价格提示Row();
            价格提示Row1.证券代码 = this.comboBox证券代码.Text;
            价格提示Row1.证券名称 = this.comboBox证券代码.Text;
            价格提示Row1.提示类型 = this.comboBox提示类型.SelectedIndex;
            价格提示Row1.提示价格 = Math.Round(this.numericUpDown提示价格.Value, L2Api.Get精度(this.comboBox证券代码.Text), MidpointRounding.AwayFromZero);
            价格提示Row1.提示等级 = this.comboBox提示等级.SelectedIndex;
            价格提示Row1.启用 = this.checkBox启用.Checked;
            Program.accountDataSet.价格提示.Add价格提示Row(价格提示Row1);

        }

    }
}
