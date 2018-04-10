using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASServer
{
    public partial class GroupConfigForm : Form
    {
        public GroupConfigForm()
        {
            InitializeComponent();
            this.Load += GroupConfigForm_Load;
        }

        void GroupConfigForm_Load(object sender, EventArgs e)
        {
            var groups = Program.db.券商帐户.Select(_=>_.名称).ToList();
            groups.Insert(0, "未设置");
            this.comboBoxGroups.DataSource = groups;
            this.comboBoxGroups.DisplayMember = "名称";

            var name = CommonUtils.GetConfig("LocalGroup");
            var item = groups.FirstOrDefault(_=>_ == name);
            if (item != null)
            {
                comboBoxGroups.SelectedItem = item;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string localGroupName = comboBoxGroups.SelectedItem.ToString();
                if (localGroupName == "未设置")
                {
                    localGroupName = "";
                }
                CommonUtils.SetConfig("LocalGroup", localGroupName);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("设置时发生异常："+ ex.Message);
            }
        }
    }
}
