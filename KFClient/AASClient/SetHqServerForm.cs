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
    public partial class SetHqServerForm : Form
    {
        public SetHqServerForm()
        {
            InitializeComponent();
        }

        private void SetHqServerForm_Load(object sender, EventArgs e)
        {
            this.comboBox行情服务器.DataSource = Program.HqServer;

            this.comboBox行情服务器.SelectedIndex = int.Parse(Program.accountDataSet.参数.GetParaValue("行情服务器","0"));
        }

        private void button保存_Click(object sender, EventArgs e)
        {
            Program.accountDataSet.参数.SetParaValue("行情服务器", this.comboBox行情服务器.SelectedIndex.ToString());

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
