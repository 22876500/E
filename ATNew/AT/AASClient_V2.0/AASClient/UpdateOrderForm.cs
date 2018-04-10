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
    public partial class UpdateOrderForm : Form
    {
        public UpdateOrderForm()
        {
            InitializeComponent();
        }

        public void Init(string orderID, string stockCode, decimal price, decimal qty)
        {
            this.textBoxOrderID.Text = orderID;
            this.textBoxStockCode.Text = stockCode;
            this.numericUpDownQty.Value = qty;
            this.numericUpDownPrice.Value = price;
        }

        private void UpdateOrderForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = Program.AASServiceClient.UpdateAyersOrder(Program.Current平台用户.用户名, textBoxOrderID.Text, numericUpDownPrice.Value, (int)numericUpDownQty.Value);
            MessageBox.Show(result);
        }
    }
}
