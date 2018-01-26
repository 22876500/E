using AASClient.AASServiceReference;
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
    public partial class ModifyTradeLimitForm : Form
    {
        AASClient.AASServiceReference.DbDataSet.额度分配Row 额度分配;
        public ModifyTradeLimitForm(AASClient.AASServiceReference.DbDataSet.额度分配Row TradeLimit1)
        {
            InitializeComponent();

            this.额度分配 = TradeLimit1;
        }

        private void ModifyTradeLimitForm_Load(object sender, EventArgs e)
        {
            this.comboBox交易员.DataSource = Program.AASServiceClient.QueryJY();
            this.comboBox交易员.DisplayMember = "用户名";

            AASClient.AASServiceReference.DbDataSet.券商帐户DataTable 券商帐户DataTable1 = Program.AASServiceClient.QueryQsAccount();

            foreach (AASClient.AASServiceReference.DbDataSet.券商帐户Row 券商帐户Row1 in 券商帐户DataTable1)
            {
                this.comboBox组合号.Items.Add(券商帐户Row1.名称);
            }
            if (this.comboBox组合号.Items.Count > 0)
            {
                this.comboBox组合号.SelectedIndex = 0;
            }
            


            this.comboBox买模式.DataSource = System.Enum.GetNames(typeof(买模式));
            this.comboBox卖模式.DataSource = System.Enum.GetNames(typeof(卖模式));

            this.comboBox交易员.Text = this.额度分配.交易员;
            this.comboBox组合号.Text = this.额度分配.组合号;
            this.textBox证券代码.Text = this.额度分配.证券代码;
            this.comboBox市场.SelectedIndex = this.额度分配.市场;
            this.textBox证券名称.Text = this.额度分配.证券名称;
            this.textBox拼音缩写.Text = this.额度分配.拼音缩写;
            this.comboBox买模式.SelectedIndex = this.额度分配.买模式;
            this.comboBox卖模式.SelectedIndex = this.额度分配.卖模式;
            this.numericUpDown交易额度.Value = this.额度分配.交易额度;
            this.numericUpDown手续费率.Value = this.额度分配.手续费率;
        }

        private void textBox拼音缩写_Leave(object sender, EventArgs e)
        {
            this.textBox拼音缩写.Text = this.textBox拼音缩写.Text.ToUpper();
        }

        private void button修改交易额度_Click(object sender, EventArgs e)
        {
            Program.AASServiceClient.UpdateTradeLimit(this.额度分配.交易员, this.额度分配.证券代码, this.comboBox组合号.Text, (byte)this.comboBox市场.SelectedIndex, this.textBox证券名称.Text, this.textBox拼音缩写.Text, (买模式)this.comboBox买模式.SelectedIndex, (卖模式)this.comboBox卖模式.SelectedIndex, this.numericUpDown交易额度.Value, this.numericUpDown手续费率.Value);


            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

       
    }
}
