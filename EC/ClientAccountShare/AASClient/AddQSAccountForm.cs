using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class AddQSAccountForm : Form
    {
        public AddQSAccountForm()
        {
            InitializeComponent();
        }

        private void AddQSAccountForm_Load(object sender, EventArgs e)
        {
            this.comboBox券商.SelectedIndex = 0;
            this.comboBox类型.SelectedIndex = 0;
        }


        private void comboBox券商_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox交易服务器.DataSource = Program.AASServiceClient.Get交易服务器(this.comboBox券商.Text);
           
        }

        private void comboBox类型_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }


        private void textBox登录帐号_TextChanged(object sender, EventArgs e)
        {
            this.textBox上海股东代码.Text = string.Empty;
            this.textBox深圳股东代码.Text = string.Empty;
        }

        private void textBox交易帐号_TextChanged(object sender, EventArgs e)
        {
        }


        private void button测试帐号_Click(object sender, EventArgs e)
        {
            this.textBox上海股东代码.Text = string.Empty;
            this.textBox深圳股东代码.Text = string.Empty;



            string[] JyServerInfo = this.comboBox交易服务器.Text.Split(new char[] { ':' });



            string Ret = GetTestResult(JyServerInfo);
            string[] Data = Ret.Split('|');
            if (Data[1] != string.Empty)
            {
                MessageBox.Show(Data[1], "测试失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            DataTable GddmDataTable = Tool.ChangeDataStringToTable(Data[0]);

            List<string> GddmList = new List<string>();
            foreach (DataRow GddmDataRow in GddmDataTable.Rows)
            {
                GddmList.Add(GddmDataRow["股东代码"] as string);
            }

            if (this.comboBox券商.Text == "模拟测试")
            {
                this.textBox上海股东代码.Text = GddmList[0];
                this.textBox深圳股东代码.Text = GddmList[1];
            }
            else
            {

                if (this.comboBox类型.Text == "普通")
                {
                    if (GddmList.Any(r => r.StartsWith("A")))
                    {
                        this.textBox上海股东代码.Text = GddmList.First(r => r.StartsWith("A"));
                    }
                    else if (GddmList.Any(r => r.StartsWith("B")))
                    {
                        this.textBox上海股东代码.Text = GddmList.First(r => r.StartsWith("B"));
                    }
                    else
                    {
                        MessageBox.Show("未获取到上海股东代码");
                    }

                    this.textBox深圳股东代码.Text = GddmList.FirstOrDefault(r => Regex.IsMatch(r, "^0[0128]")) ?? string.Empty;
                    if(string.IsNullOrEmpty(this.textBox深圳股东代码.Text))
                    {
                        MessageBox.Show("未获取到深圳股东代码");
                    }
                }
                else
                {
                    if (GddmList.Any(r => r.StartsWith("E")))
                    {
                        this.textBox上海股东代码.Text = GddmList.First(r => r.StartsWith("E"));
                    }
                    else
                    {
                        MessageBox.Show("未获取到上海股东代码");
                    }


                    if (GddmList.Any(r => r.StartsWith("06")))
                    {
                        this.textBox深圳股东代码.Text = GddmList.First(r => r.StartsWith("06"));
                    }
                    else
                    {
                        MessageBox.Show("未获取到深圳股东代码");
                    }
                }
            }



            this.dataGridView股东列表.DataSource = GddmDataTable;


            if (GddmDataTable.Columns.Contains("资金帐号"))
            {
                string 资金帐号 = GddmDataTable.Rows[0]["资金帐号"] as string;
                if (资金帐号 != this.textBox交易帐号.Text)
                {
                    MessageBox.Show(string.Format("所填交易帐号 {0} 与表中资金帐号 {1} 不一致", this.textBox交易帐号.Text, 资金帐号));
                    return;
                }
            }
        }

        private string GetTestResult(string[] JyServerInfo)
        {
            try
            {
                string Ret = Program.AASServiceClient.Test(JyServerInfo[1], short.Parse(JyServerInfo[2]), this.textBox版本号.Text, (short)this.numericUpDown营业部代码.Value, this.textBox登录帐号.Text, this.textBox交易帐号.Text, this.textBox交易密码.Text, this.textBox通讯密码.Text);
                return Ret;
            }
            catch (Exception ex)
            {
                return string.Format("|测试帐号信息时发生异常，请重新测试\n错误信息：{0}", ex.Message);
            }
        }

        private void button添加帐号_Click(object sender, EventArgs e)
        {
            if (this.textBox上海股东代码.Text == string.Empty && this.textBox深圳股东代码.Text == string.Empty)
            {
                MessageBox.Show("请先测试帐号是否可用,用来获取股东代码");
                return;
            }


            Program.AASServiceClient.AddQSAccount(this.checkBox启用.Checked, this.textBox帐户名称.Text, this.comboBox券商.Text, this.comboBox类型.Text, this.comboBox交易服务器.Text, this.textBox版本号.Text, (short)this.numericUpDown营业部代码.Value, this.textBox登录帐号.Text, this.textBox交易帐号.Text, this.textBox交易密码.Text, this.textBox通讯密码.Text, this.textBox上海股东代码.Text, this.textBox深圳股东代码.Text, (int)this.numericUpDown查询时间间隔.Value);



            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void AddQSAccountForm_Activated(object sender, EventArgs e)
        {
            this.textBox帐户名称.Focus();
        }
    }
}
