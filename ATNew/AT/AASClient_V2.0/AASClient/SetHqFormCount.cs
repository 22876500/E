using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class SetHqFormCount : Form
    {
        public SetHqFormCount()
        {
            InitializeComponent();
        }

        private void SetHqFormCount_Load(object sender, EventArgs e)
        {
            this.numericUpDown1.Value = decimal.Parse(Program.accountDataSet.参数.GetParaValue("报价窗口数目", "4"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.accountDataSet.参数.SetParaValue("报价窗口数目", this.numericUpDown1.Value.ToString());

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

     
    }
}
