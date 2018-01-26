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
    public partial class ShareLimitListForm : Form
    {
        AASServiceReference.ShareLimitGroupItem[] ShareLimitGroups; 

        public ShareLimitListForm()
        {
            InitializeComponent();

            InitPageData();
        }

        private void InitPageData()
        {
            Init();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (comboBoxGroup.SelectedIndex > -1 && ShareLimitGroups != null)
            {
                var group = ShareLimitGroups[comboBoxGroup.SelectedIndex];
                BindingShareLimitData(group);
            }
        }

        private void Init()
        {
            dataGridViewStock.AutoGenerateColumns = false;
            //dataGridViewTrader.AutoGenerateColumns = false;

            var groups = Program.AASServiceClient.ShareGroupQuery();
            if (groups != null && groups.Length > 0)
            {
                comboBoxGroup.Items.Clear();

                groups.ToList().ForEach(_ => comboBoxGroup.Items.Add(_.GroupName));
                comboBoxGroup.SelectedIndex = 0;
                BindingShareLimitData(groups.First());
            }
            ShareLimitGroups = groups;
        }

        private void BindingShareLimitData(AASServiceReference.ShareLimitGroupItem shareLimitGroupItem)
        {
            

            this.bindingSourceStock.DataSource = shareLimitGroupItem.GroupStockList;
            dataGridViewStock.DataSource = bindingSourceStock;
            

            //this.bindingSourceTrader.DataSource = shareLimitGroupItem.GroupTraderList;
            //dataGridViewTrader.DataSource = bindingSourceTrader;
            
        }
    }
}
