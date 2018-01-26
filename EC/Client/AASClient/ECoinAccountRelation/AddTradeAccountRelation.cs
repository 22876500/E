using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.ECoinAccountRelation
{
    public partial class AddTradeAccountRelation : Form
    {
        public AddTradeAccountRelation()
        {
            InitializeComponent();
            this.Load += Loaded;
        }

        private void Loaded(object sender, EventArgs e)
        {
            var user = Program.AASServiceClient.QueryJY();
            foreach (var item in user)
            {
                comboBox1.Items.Add(item.用户名);
            }

            var account = Program.AASServiceClient.QueryECoinQsAccount();
            foreach (var item in account)
            {
                comboBox2.Items.Add(item.名称);
            }
        }

        private void button保存_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1 && comboBox2.SelectedIndex > -1)
            {
                var str = Program.AASServiceClient.AddAccountRelation(comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString());
                if (str.StartsWith("0|"))
                {
                    MessageBox.Show(str.Substring(2));
                }
                else
                {
                    this.Close();
                }
            }
            
        }
    }
}
