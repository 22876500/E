using DataComparision.Entity;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataComparision.View
{
    /// <summary>
    /// GroupEdit.xaml 的交互逻辑
    /// </summary>
    public partial class GroupEdit : Window
    {
        券商 Entity { get; set; }

        public GroupEdit()
        {
            InitializeComponent();
        }

        public GroupEdit(券商 o)
        {
            InitializeComponent();
            Init(o);
        }

        public void Init(券商 o)
        {
            Entity = o;
            this.txt名称.Text = o.名称;
            this.txt版本号.Text = o.版本号;
            this.txt交易服务器.Text = o.交易服务器;
            this.txtIP.Text = o.IP;
            this.txtPort.Text = o.Port + "";
            this.txt营业部代码.Text = o.营业部代码 + "";
            this.txt登录帐号.Text = o.登录帐号;
            this.txt交易帐号.Text = o.交易帐号;
            this.txt交易密码.Text = o.交易密码;
            this.txt通讯密码.Text = o.通讯密码;
            
            this.ckIsEnable.IsChecked = o.启用;
            
            

        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                try
                {
                    券商 o = GetPageEntity();
                    StringBuilder ErrInfo = new StringBuilder(256);
                    //int clientID = TdxApi.Logon(o.IP, o.Port, o.版本号, o.营业部代码, o.登录帐号, o.交易帐号, o.TradePsw, o.CommunicatePsw, ErrInfo);
                    //if (clientID > -1)
                    //{
                       
                    //    TdxApi.Logoff(clientID);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("登录验证失败, 错误信息：" + ErrInfo.ToString());
                    //}
                    using (var db = new DataComparisionDataset())
                    {
                        var oldEntity = db.券商ds.FirstOrDefault(_ => _.名称 == o.名称);
                        if (oldEntity != null)
                        {
                            db.券商ds.Remove(oldEntity);
                        }


                        db.券商ds.Add(o);
                        db.SaveChanges();
                        MessageBox.Show("保存完毕", "组合号编辑");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    CommonUtils.ShowMsg(ex.Message);
                }
            }
        }

        private bool Validate()
        {
            if (!Regex.IsMatch(this.txt名称.Text, "^[A-Z][0-9]{2,}$")) 
            {
                MessageBox.Show("名称不符合规范，格式应为大写字母后带两个以上的数字！");
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
                    MessageBox.Show("交易服务器中包含的ip信息与ip地址栏中填写的不一致！");
                    return false;
                }

                if (string.IsNullOrEmpty(txtPort.Text))
                {
                    this.txtPort.Text = ipInfo[1];
                }
                else if (txtPort.Text != ipInfo[1])
                {
                    MessageBox.Show("交易服务器中包含的端口信息与Port地址栏中填写的不一致！");
                    return false;
                }
            }

            //if (!CommonUtils.IsIP(this.txtIP.Text))
            //{
            //    MessageBox.Show("ip不符合规范，应在0~255.0~255.0~255.0~255范围内，请在交易服务器中包含或单独填写");
            //    return false;
            //}
            if (!Regex.IsMatch(this.txtPort.Text, "^[1-9][0-9]*$"))
            {
                MessageBox.Show("端口号不符合规范，应为数字,如80，请在交易服务器中包含或单独填写");
            }


            if (string.IsNullOrEmpty(this.txt版本号.Text))
            {
                MessageBox.Show("版本号不能为空!");
                return false;
            }
            if (string.IsNullOrEmpty(this.txt营业部代码.Text)) 
            {
                MessageBox.Show("营业部代码不能为空!");
                return false; 
            }
            if (string.IsNullOrEmpty(this.txt登录帐号.Text))
            {
                MessageBox.Show("登录帐号不能为空!");
                return false;
            }
            if (string.IsNullOrEmpty(this.txt交易帐号.Text))
            {
                MessageBox.Show("交易帐号不能为空!");
                return false; 
            }
            if (string.IsNullOrEmpty(this.txt交易密码.Text))
            {
                MessageBox.Show("交易密码不能为空!");
                return false;
            }
            
            return true;
        }

        private 券商 GetPageEntity()
        {
            var o = new Entity.券商();
            o.IP = this.txtIP.Text;
            o.Port = short.Parse(this.txtPort.Text);
            o.版本号 = this.txt版本号.Text;
            o.登录帐号 = this.txt登录帐号.Text;
            o.交易服务器 = this.txt交易服务器.Text;
            o.交易密码 = this.txt交易密码.Text.Trim();
            o.交易帐号 = this.txt交易帐号.Text;
            o.名称 = this.txt名称.Text;
            o.启用 = this.ckIsEnable.IsChecked == true;
            o.通讯密码 = this.txt通讯密码.Text.Trim();

            if (ckbIsEncrypcy.IsChecked != true)
            {
                o.交易密码 = Cryptor.MD5Encrypt(this.txt交易密码.Text.Trim());
                o.通讯密码 = Cryptor.MD5Encrypt(this.txt通讯密码.Text.Trim());
            }
            if (o.通讯密码 == "")
            {
                o.通讯密码 = Cryptor.MD5Encrypt(o.通讯密码);
            }
            o.营业部代码 = short.Parse(this.txt营业部代码.Text);
            return o;
        }
    }
}
