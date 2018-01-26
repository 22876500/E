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
    public partial class SubConfigForm : Form
    {
        public SubConfigForm()
        {
            InitializeComponent();

            this.Load += SubConfigForm_Load;
        }

        void SubConfigForm_Load(object sender, EventArgs e)
        {
            var names = Enum.GetNames(typeof(Model.SubType));
            foreach (var item in names)
            {
                comboBoxSubType.Items.Add(item);
            }

            Task task = new Task(() => {
                int subType = TDFData.DataCache.GetInstance().GetSubType();
                Action ac = new Action(() => { comboBoxSubType.SelectedIndex = (subType > -1 && subType < comboBoxSubType.Items.Count) ? subType : 0; });
                this.Invoke(ac);
            });

            task.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TDFData.DataCache.GetInstance().IsConnected)
            {
                bool result = TDFData.DataCache.GetInstance().SetSubType(comboBoxSubType.SelectedIndex);
                if (result)
                {
                    MessageBox.Show("修改成功！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("设置失败，请检查与行情软件连接!");
                }
            }
            else
            {
                MessageBox.Show("未连接，请检查与行情服务软件的连接是否正常！");
            }
        }
    }
}
