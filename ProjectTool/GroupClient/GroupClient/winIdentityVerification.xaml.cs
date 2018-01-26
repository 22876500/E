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
    /// winIdentityVerification.xaml 的交互逻辑
    /// </summary>
    public partial class winIdentityVerification : Window
    {
        public winIdentityVerification()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Valid())
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("用户名或密码不正确！");
            }
        }

        private bool Valid()
        {
            var userName = CommonUtils.GetConfig("user");//admin
            var psw = CommonUtils.GetConfig("password");//12345

            if (string.IsNullOrEmpty(userName))
            {
                userName = "admin";
                CommonUtils.SetConfig("user", "admin");
            }
            if (string.IsNullOrEmpty(psw))
            {
                psw = Cryptor.MD5Decrypt("57DDBC28E53C7DEA");
                CommonUtils.SetConfig("password", "57DDBC28E53C7DEA");
            }
            if (txtUserName.Text == userName && Cryptor.MD5Encrypt(pb.Password) == psw)
            {
                return true;
            }
            return false;
        }
    }
}
