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
    public partial class SetShortcutForm : Form
    {
        public SetShortcutForm()
        {
            InitializeComponent();
        }

        private void SetShortcutForm_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = Program.accountDataSet.快捷键;
            this.dataGridView1.DataSource = this.bindingSource1;
            //this.textBox默认股数和金额.Text = Program.accountDataSet.参数.GetParaValue("默认股数和金额", "Oemtilde");
            //this.textBox买券还券.Text = Program.accountDataSet.参数.GetParaValue("买券还券", "F2");
            //this.textBox现金买入.Text = Program.accountDataSet.参数.GetParaValue("现金买入", "F1");
            //this.textBox融资买入.Text = Program.accountDataSet.参数.GetParaValue("融资买入", "F5");
            //this.textBox现券卖出.Text = Program.accountDataSet.参数.GetParaValue("现券卖出", "F4");
            //this.textBox融券卖出.Text = Program.accountDataSet.参数.GetParaValue("融券卖出", "F3");
            //this.textBox增加委托价格.Text = Program.accountDataSet.参数.GetParaValue("增加委托价格", "Up");
            //this.textBox减少委托价格.Text = Program.accountDataSet.参数.GetParaValue("减少委托价格", "Down");
            //this.textBox撤单.Text = Program.accountDataSet.参数.GetParaValue("撤单", "Escape");
        }

        //private void textBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    (sender as TextBox).Text = e.KeyCode.ToString();
        //}

        //private void textBox_KeyUp(object sender, KeyEventArgs e)
        //{
        //}

        //private void textBox默认股数和金额_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    e.Handled = true;//表示取消输入
        //}

        //private void button确定_Click(object sender, EventArgs e)
        //{
        //    string[] keys = { this.textBox默认股数和金额.Text, 
        //                     this.textBox买券还券.Text,
        //                    this.textBox现金买入.Text,
        //                    this.textBox融资买入.Text,
        //                    this.textBox现券卖出.Text,
        //                    this.textBox融券卖出.Text,
        //                     this.textBox增加委托价格.Text,
        //                    this.textBox减少委托价格.Text,
        //                     this.textBox撤单.Text};

        //    Dictionary<string, bool> KeyDictionary = new Dictionary<string, bool>();
        //    foreach (string string1 in keys)
        //    {
        //        if (KeyDictionary.ContainsKey(string1))
        //        {
        //            MessageBox.Show(string.Format("不允许两个操作设置相同的快捷键 {0}", string1));
        //            return;
        //        }
        //        else
        //        {
        //            KeyDictionary[string1] = true;
        //        }
        //    }


        //    Program.accountDataSet.参数.SetParaValue("默认股数和金额", this.textBox默认股数和金额.Text);
        //    Program.accountDataSet.参数.SetParaValue("买券还券", this.textBox买券还券.Text);
        //    Program.accountDataSet.参数.SetParaValue("现金买入", this.textBox现金买入.Text);
        //    Program.accountDataSet.参数.SetParaValue("融资买入", this.textBox融资买入.Text);
        //    Program.accountDataSet.参数.SetParaValue("现券卖出", this.textBox现券卖出.Text);
        //    Program.accountDataSet.参数.SetParaValue("融券卖出", this.textBox融券卖出.Text);
        //    Program.accountDataSet.参数.SetParaValue("增加委托价格", this.textBox增加委托价格.Text);
        //    Program.accountDataSet.参数.SetParaValue("减少委托价格", this.textBox减少委托价格.Text);
        //    Program.accountDataSet.参数.SetParaValue("撤单", this.textBox撤单.Text);

        //    this.DialogResult = System.Windows.Forms.DialogResult.OK;
        //}

        private void 添加快捷键ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddShortcutForm AddShortcutForm1 = new AddShortcutForm();
            AddShortcutForm1.ShowDialog();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "方向")
            {
                int int1 = (int)e.Value;

                交易方向 交易方向1 = (交易方向)int1;
                e.Value = 交易方向1.ToString();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "价格模式")
            {
                int int1 = (int)e.Value;

                价格模式 价格模式1 = (价格模式)int1;
                e.Value = 价格模式1.ToString();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "价差模式")
            {
                int int1 = (int)e.Value;

                价差模式 价差模式1 = (价差模式)int1;
                e.Value = 价差模式1.ToString();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "股数模式")
            {
                int int1 = (int)e.Value;

                股数模式 股数模式1 = (股数模式)int1;
                e.Value = 股数模式1.ToString();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AASClient.AccountDataSet.快捷键Row 快捷键Row1 = (this.bindingSource1.Current as DataRowView).Row as AASClient.AccountDataSet.快捷键Row;
            ModifyShortcutForm ModifyShortcutForm1 = new ModifyShortcutForm(快捷键Row1);
            ModifyShortcutForm1.ShowDialog();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AASClient.AccountDataSet.快捷键Row 快捷键Row1 = (this.bindingSource1.Current as DataRowView).Row as AASClient.AccountDataSet.快捷键Row;
            ModifyShortcutForm ModifyShortcutForm1 = new ModifyShortcutForm(快捷键Row1);
            ModifyShortcutForm1.ShowDialog();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AASClient.AccountDataSet.快捷键Row 快捷键Row1 = (this.bindingSource1.Current as DataRowView).Row as AASClient.AccountDataSet.快捷键Row;

            Program.accountDataSet.快捷键.Remove快捷键Row(快捷键Row1);
        }

      

       

        
    }
}
