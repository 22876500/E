using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    enum 提示类型 { 涨到, 跌到 }
    enum 提示等级 {黄, 红 }
    public partial class AddRemindForm : Form
    {
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
                this.comboBox证券代码.AutoCompleteCustomSource.Add(string.Format("{0} {1}", 证券代码, Program.证券名称[证券代码]));
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
                Regex Regex1 = new Regex("[0-9]{6}");
                if (Regex1.IsMatch(this.comboBox证券代码.Text))
                {
                    Match Match1 = Regex1.Match(this.comboBox证券代码.Text);
                    string Zqdm = Match1.Groups[0].Value;


                    this.comboBox证券代码.Text = Zqdm;


                    //this.numericUpDown提示价格.Value = 0;

                    Program.TempZqdm = this.comboBox证券代码.Text;

                    this.NeedPrice = true;
                }
            }
        }

        private void textBox证券代码_TextChanged(object sender, EventArgs e)
        {
            if (this.comboBox证券代码.Text.Length == 6
               && char.IsNumber(this.comboBox证券代码.Text[0])
               && char.IsNumber(this.comboBox证券代码.Text[1])
               && char.IsNumber(this.comboBox证券代码.Text[2])
               && char.IsNumber(this.comboBox证券代码.Text[3])
               && char.IsNumber(this.comboBox证券代码.Text[4])
               && char.IsNumber(this.comboBox证券代码.Text[5]))
            {
                //this.numericUpDown提示价格.Value = 0;

                Program.TempZqdm = this.comboBox证券代码.Text;

                this.NeedPrice = true;
            }
        }
        private void comboBox证券代码_Leave(object sender, EventArgs e)
        {
            Regex Regex1 = new Regex("[0-9]{6}");
            if (Regex1.IsMatch(this.comboBox证券代码.Text))
            {
                Match Match1 = Regex1.Match(this.comboBox证券代码.Text);
                string Zqdm = Match1.Groups[0].Value;


                this.comboBox证券代码.Text = Zqdm;


                //this.numericUpDown提示价格.Value = 0;

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

            this.label证券名称.Text = L2Api.Get名称(this.comboBox证券代码.Text);

        }




        private void button1_Click(object sender, EventArgs e)
        {
            
            if (this.comboBox证券代码.Text.Length == 6
               && char.IsNumber(this.comboBox证券代码.Text[0])
               && char.IsNumber(this.comboBox证券代码.Text[1])
               && char.IsNumber(this.comboBox证券代码.Text[2])
               && char.IsNumber(this.comboBox证券代码.Text[3])
               && char.IsNumber(this.comboBox证券代码.Text[4])
               && char.IsNumber(this.comboBox证券代码.Text[5]))
            {
                string 证券名称 = L2Api.Get名称(this.comboBox证券代码.Text);
                if (TDFData.DataSourceConfig.IsUseTDFData && TDFData.DataCache.GetInstance().MarketNewDict.ContainsKey(this.comboBox证券代码.Text))
                {
                    证券名称 = Manager.StockCodeManager.GetNameByCode(this.comboBox证券代码.Text);
                }
                
                if (证券名称 == string.Empty)
                {
                    
                    MessageBox.Show("未取到证券名称");
                    return;
                }

                AASClient.AccountDataSet.价格提示Row 价格提示Row1 = Program.accountDataSet.价格提示.New价格提示Row();
                价格提示Row1.证券代码 = this.comboBox证券代码.Text;
                价格提示Row1.证券名称 = 证券名称;
                价格提示Row1.提示类型 = this.comboBox提示类型.SelectedIndex;
                价格提示Row1.提示价格 = Math.Round(this.numericUpDown提示价格.Value, L2Api.Get精度(this.comboBox证券代码.Text), MidpointRounding.AwayFromZero);
                价格提示Row1.提示等级 = this.comboBox提示等级.SelectedIndex;
                价格提示Row1.启用 = this.checkBox启用.Checked;
                Program.accountDataSet.价格提示.Add价格提示Row(价格提示Row1);
            }
            else
            {
                MessageBox.Show("证券代码错误");
            }

        }


       
    }
}
