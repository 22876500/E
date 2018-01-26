using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroupClient
{
    /// <summary>
    /// AccountEdit.xaml 的交互逻辑
    /// </summary>
    public partial class AccountEdit : Window
    {
        public AccountEdit()
        {
            InitializeComponent();
            Loaded += AccountEdit_Loaded;
        }

        void AccountEdit_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtAccount.Text = CommonUtils.GetConfig("user");
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (Valid())
            {
                CommonUtils.SetConfig("user", txtAccount.Text);
                CommonUtils.SetConfig("password", Cryptor.MD5Encrypt(txtPassword.Password));
                this.Close();
            }
        }

        private bool Valid()
        {
            if (string.IsNullOrEmpty(txtAccount.Text))
            {
                MessageBox.Show("请填写管理员账户!", "管理员密码管理");
                return false;
            }
            else if (string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("请填写密码!", "管理员密码管理");
                return false;
            }
            else if(txtPassword.Password != txtPasswordAgain.Password)
            {
                MessageBox.Show("两次密码输入不一致!", "管理员密码管理");
                return false;
            }
            return true;
        }
    }
}
