using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using System.Windows.Threading;

namespace DataComparision.View
{
    /// <summary>
    /// Logon.xaml 的交互逻辑
    /// </summary>
    public partial class Logon : Window
    {

        static bool CanLogon = true;

        public static bool IsLogon { get; set; }

        public static int CanLoginCount {

            get 
            {
                var info = CommonUtils.GetConfig("info");
                if (!string.IsNullOrEmpty(info))
                {
                    info = Cryptor.MD5Decrypt(info);
                    var lastLogonInfo = info.Split('|');
                    var date = DateTime.Parse(lastLogonInfo[0]);
                    var count = int.Parse(lastLogonInfo[1]);
                    if (date == DateTime.Today)
                    {
                        return count;
                    }
                    else
                    {
                        return 10;
                    }
                }
                else
                {
                    CanLogon = false;
                    MessageBox.Show("配置文件出错，请联系管理员！");
                    return 0;
                }
            }
        }

        

        public Logon()
        {
            InitializeComponent();
            IsLogon = false;
            var userName = CommonUtils.GetConfig("user");
            var password = CommonUtils.GetConfig("password");
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                try
                {
                    this.txtName.Text = Cryptor.MD5Decrypt(userName);
                    this.psw.Password = Cryptor.MD5Decrypt(password);
                    this.ckbSavePsw.IsChecked = true;
                }
                catch { }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (CanLoginCount > 0)
            {
                var client = new ServiceReference.DataWebServiceSoapClient();
                client.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 3);

                var userName = this.txtName.Text;
                var psw = this.psw.Password;
                try
                {
                    var isLoginSuccess = client.CompareToolLogin(userName, psw);
                    if (isLoginSuccess)
                    {
                        CommonUtils.UserName = userName;

                        if (ckbSavePsw.IsChecked == true)
                        {
                            CommonUtils.SetConfig("user", Cryptor.MD5Encrypt(userName));
                            CommonUtils.SetConfig("password", Cryptor.MD5Encrypt(psw));
                        }
                        else
                        {
                            CommonUtils.SetConfig("user", string.Empty);
                            CommonUtils.SetConfig("password", string.Empty);
                        }

                        IsLogon = true;
                        this.Close();
                    }
                    else
                    {
                        CommonUtils.SetConfig("info", Cryptor.MD5Encrypt(DateTime.Today.ToString() + "|" + (CanLoginCount - 1)));
                        MessageBox.Show("用户名或密码不正确！当前可用登录次数为：" + CanLoginCount);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("登录异常，Message " + ex.Message);
                }
               
            }
            else
            {
                if (CanLogon)
                {
                    MessageBox.Show("可用登录次数已耗尽，请联系管理员！");
                }
            }

            
        }
    }
}
