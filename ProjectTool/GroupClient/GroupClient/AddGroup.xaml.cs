using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// AddGroup.xaml 的交互逻辑
    /// </summary>
    public partial class AddGroup : Window
    {
        券商 GroupEntity { get; set; }

        public Action<券商> OnAddComplete { get; set; }

        public AddGroup()
        {
            InitializeComponent();
            if (File.Exists(CommonUtils.CurrentPath + "交易服务器.txt"))
            {
                var arr = File.ReadAllLines(CommonUtils.CurrentPath + "交易服务器.txt", Encoding.Default);
                cmbFiles.ItemsSource = arr;
            }
        }

        public void Init(券商 group)
        {
            GroupEntity = group;
            this.Title = "组合号编辑";
            this.ckIsEnable.IsChecked = group.启用;
            this.ckIsEncrypt.IsChecked = true;

            this.txtIP.Text = group.IP;
            this.txtPort.Text = group.Port.ToString();
            this.txt版本号.Text = group.版本号;
            this.txt登录帐号.Text = group.登录帐号;
            this.txt交易服务器.Text = group.交易服务器;
            this.txt交易密码.Password = group.交易密码;
            this.txt交易帐号.Text = group.交易帐号;
            this.txt名称.Text = group.名称;
            this.txt通讯密码.Password = group.通讯密码;
            this.txt营业部代码.Text = group.营业部代码.ToString();
            this.txt查询时间间隔.Text = group.查询间隔时间.ToString();
            this.ckIsMultiThread.IsChecked = group.Multithreading;


        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                var entity = GetPageEntity();

                bool isLogon = Test(entity.IP, entity.Port, entity.版本号, entity.营业部代码, entity.登录帐号, entity.交易帐号, Cryptor.MD5Decrypt(entity.交易密码), Cryptor.MD5Decrypt(entity.通讯密码));
                if (isLogon)
                {
                    if (Adapter.UpdateGroup(entity))
                    {
                        MessageBox.Show("保存成功！");
                        if (OnAddComplete != null)
                        {
                            OnAddComplete.Invoke(entity);
                        }
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("保存失败！");
                    }
                }
                //else
                //{
                //    MessageBox.Show("帐号或密码不正确！");
                //}
                
            }
        }


        private 券商 GetPageEntity()
        {
            if (this.GroupEntity == null)
                GroupEntity = new 券商();

            var o = GroupEntity;
            o.IP = this.txtIP.Text;
            o.Port = short.Parse(this.txtPort.Text.Trim());
            o.版本号 = this.txt版本号.Text.Trim();
            o.登录帐号 = this.txt登录帐号.Text.Trim();
            o.交易服务器 = this.txt交易服务器.Text.Trim();
            o.交易帐号 = this.txt交易帐号.Text.Trim();
            o.名称 = this.txt名称.Text.Trim();
            o.启用 = this.ckIsEnable.IsChecked == true;
            if (ckIsEncrypt.IsChecked == true)
            {
                o.交易密码 = this.txt交易密码.Password;
                o.通讯密码 = this.txt通讯密码.Password;
            }
            else
            {
                o.交易密码 = Cryptor.MD5Encrypt(this.txt交易密码.Password);
                o.通讯密码 = Cryptor.MD5Encrypt(this.txt通讯密码.Password);
                
            }

            int time = 0;
            if (int.TryParse(this.txt查询时间间隔.Text.Trim(), out time))
            {
                o.查询间隔时间 = time;
            }

            o.营业部代码 = short.Parse(this.txt营业部代码.Text);
            o.Multithreading = ckIsMultiThread.IsChecked == true;
            return o;
        }

        private bool Validate()
        {
            if (GroupEntity == null && !string.IsNullOrEmpty(CommonUtils.GetConfig(this.txt名称.Text)))
            {
                MessageBox.Show("已存在该组合号配置信息！", this.Title);
                return false;
            }
            if (!Regex.IsMatch(this.txt名称.Text, "^[A-Z][0-9]{2,}$"))
            {
                MessageBox.Show("名称不符合规范，格式应为大写字母后带两个以上的数字！", this.Title);
                return false;
            }

            if (Regex.IsMatch(this.txt交易服务器.Text, "[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}:[0-9]+"))
            {
                var ipInfo = Regex.Match(this.txt交易服务器.Text, "[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}:[0-9]+").Value.Split(':');
                if (string.IsNullOrEmpty(txtIP.Text))
                {
                    this.txtIP.Text = ipInfo[0];
                }
                else if (txtIP.Text != ipInfo[0])
                {
                    MessageBox.Show("交易服务器中包含的ip信息与ip地址栏中填写的不一致！", this.Title);
                    return false;
                }

                if (string.IsNullOrEmpty(txtPort.Text))
                {
                    this.txtPort.Text = ipInfo[1];
                }
                else if (txtPort.Text != ipInfo[1])
                {
                    MessageBox.Show("交易服务器中包含的端口信息与Port地址栏中填写的不一致！", this.Title);
                    return false;
                }
            }

            if (!CommonUtils.IsIP(this.txtIP.Text))
            {
                MessageBox.Show("ip不符合规范，应在0~255.0~255.0~255.0~255范围内，请在交易服务器中包含或单独填写", this.Title);
                return false;
            }
            if (!Regex.IsMatch(this.txtPort.Text, "^[1-9][0-9]+$"))
            {
                MessageBox.Show("端口号不符合规范，应为数字,如80，请在交易服务器中包含或单独填写", this.Title);
            }


            if (string.IsNullOrEmpty(this.txt版本号.Text))
            {
                MessageBox.Show("版本号不能为空!", this.Title);
                return false;
            }
            if (string.IsNullOrEmpty(this.txt营业部代码.Text))
            {
                MessageBox.Show("营业部代码不能为空!", this.Title);
                return false;
            }
            if (string.IsNullOrEmpty(this.txt登录帐号.Text))
            {
                MessageBox.Show("登录帐号不能为空!", this.Title);
                return false;
            }
            if (string.IsNullOrEmpty(this.txt交易帐号.Text))
            {
                MessageBox.Show("交易帐号不能为空!", this.Title);
                return false;
            }
            if (string.IsNullOrEmpty(this.txt交易密码.Password))
            {
                MessageBox.Show("交易密码不能为空!", this.Title);
                return false;
            }
            else if (this.ckIsEncrypt.IsChecked == true)
            {
                try
                {
                    var psw = Cryptor.MD5Decrypt(txt交易密码.Password);
                    var psw1 = Cryptor.MD5Decrypt(txt通讯密码.Password);
                }
                catch (Exception)
                {
                    MessageBox.Show("密码解密失败，请确认输入的是加密后的密码！", this.Title);
                    return false;
                }
            }

            return true;
        }

        public bool Test(string IP, short Port, string Version, short YybID, string AccountNo, string TradeAccount, string JyPassword, string TxPassword)
        {
            try
            {
                StringBuilder Result = new StringBuilder(1024 * 1024);
                StringBuilder ErrInfo = new StringBuilder(256);

                int ClientID = TdxApi.Logon(IP, Port, Version, YybID, AccountNo, TradeAccount, JyPassword, TxPassword, ErrInfo);

                if (ClientID == -1)
                {
                    MessageBox.Show("登录测试失败：" + ErrInfo.ToString());
                    return false;
                }


                TdxApi.QueryData(ClientID, 5, Result, ErrInfo);
                TdxApi.Logoff(ClientID);


                return true;
            }
            catch (Exception ex)
            {
                CommonUtils.Log("组合号添加异常：" + ex.Message);
                return false;
            }

        }

        private void ckIsShowPsw_Click(object sender, RoutedEventArgs e)
        {
            //this.txt交易密码;
        }

        private void Button_Txt_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = true, Multiselect = true, Filter = "Text File|*.txt", InitialDirectory = CommonUtils.CurrentPath };

            if (dialog.ShowDialog() == true)
            {
                var arr = File.ReadAllLines(dialog.FileName, Encoding.Default);
                cmbFiles.ItemsSource = arr;
            }
        }

        private void cmbFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFiles.Items.Count > 0)
            {
                this.txt交易服务器.Text = cmbFiles.SelectedItem.ToString();
                var arr = this.txt交易服务器.Text.Split(':');
                if (arr.Length >= 3)
                {
                    txtIP.Text = arr[1];
                    txtPort.Text = arr[2];
                }
            }
        }
    }
}
