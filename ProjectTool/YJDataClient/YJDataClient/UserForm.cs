using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YJDataClient
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
            this.Load += UserForm_Load;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            LoadAllUser();
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(tabControl1.SelectedIndex)
            {
                case 0:
                    LoadAllUser();
                    break;
                case 1:
                    AddUserInfo();
                    break;
            }
        }

        private void LoadAllUser()
        {
            this.listView1.Items.Clear();

            ImageList image = new ImageList();
            image.ImageSize = new Size(1, 30);
            this.listView1.SmallImageList = image;
            this.listView1.Columns[0].Width = this.listView1.ClientRectangle.Width;

            string[] lstUser = Program.DataServiceClient.GetAllUserName();
            if (lstUser == null)
            {
                MessageBox.Show("数据加载失败", "Error");
                return;
            }
            foreach (var str in lstUser)
            {
                ListViewItem item = new ListViewItem();
                item = this.listView1.Items.Add(str);
            }
        }

        private void AddUserInfo()
        {

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string error;
            if (Program.DataServiceClient.AddUser(this.txtUser.Text.Trim(), this.txtPwd.Text.Trim(), out error) == false)
            {
                MessageBox.Show(error, "Error");
            }
            else
            {
                MessageBox.Show("添加成功", "提示");
            }


        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //双击删除数据
            DialogResult dr = MessageBox.Show("删除当前用户？", "删除数据", MessageBoxButtons.OKCancel);

            if (dr == DialogResult.OK)
            {
                string userName = this.listView1.SelectedItems[0].SubItems[0].Text;

                if (Program.DataServiceClient.DeleteUser(userName) == false)
                {
                    MessageBox.Show("数据删除失败", "Error");
                    return;
                }
                LoadAllUser();
            }
        }
    }
}
