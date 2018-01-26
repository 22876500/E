using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// AddImsGroup.xaml 的交互逻辑
    /// </summary>
    public partial class AddImsGroup : Window
    {
        public Action<券商> OnAddComplete;

        券商 GroupEntity;


        public AddImsGroup(券商 groupItem)
        {
            InitializeComponent();
            Init(groupItem);
        }

        private void Init(券商 group)
        {
            GroupEntity = group;
            this.Title = "组合号编辑";
            this.ckIsEnable.IsChecked = group.启用;
            this.ckIsEncrypt.IsChecked = true;
            //this.txtIP.Text = group.IP;
            //this.txtPort.Text = group.Port.ToString();
            this.txt版本号.Text = group.版本号;
            
            this.txt帐号.Text = group.交易帐号;
            this.txt密码.Password = group.交易密码;
            this.txt名称.Text = group.名称;
            this.txt查询时间间隔.Text = group.查询间隔时间.ToString();
            this.txt产品信息.Text = group.产品信息;
            this.txt资产单元.Text = group.资产单元;
            this.txt投资组合.Text = group.投资组合;
            this.btnSave.IsEnabled = true;
        }

        public AddImsGroup()
        {
            InitializeComponent();
        }


        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                var entity = GetPageEntity();

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
        }

        private 券商 GetPageEntity()
        {
            if (this.GroupEntity == null)
                GroupEntity = new 券商() { IsIMSAccount = true };

            var o = GroupEntity;

            o.名称 = this.txt名称.Text.Trim();
            o.启用 = this.ckIsEnable.IsChecked == true;
            o.版本号 = this.txt版本号.Text.Trim();
            //o.交易服务器 = this.txt交易服务器.Text.Trim();
            //var serverInfo = Regex.Match(o.交易服务器, "[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}").Value;
            //o.IP = serverInfo;

            o.交易帐号 = this.txt帐号.Text.Trim();
            if (ckIsEncrypt.IsChecked == true)
            {
                o.交易密码 = this.txt密码.Password;
                o.通讯密码 = Cryptor.MD5Encrypt(string.Empty); ;
            }
            else
            {
                o.交易密码 = Cryptor.MD5Encrypt(this.txt密码.Password);
                o.通讯密码 = Cryptor.MD5Encrypt(string.Empty);

            }

            o.产品信息 = this.txt产品信息.Text.Trim();
            o.资产单元 = this.txt资产单元.Text.Trim();
            o.投资组合 = this.txt投资组合.Text.Trim();

            int time = 0;
            if (int.TryParse(this.txt查询时间间隔.Text.Trim(), out time))
            {
                o.查询间隔时间 = time;
            }

            //o.Multithreading = ckIsMultiThread.IsChecked == true;
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

            //if (!Regex.IsMatch(this.txt交易服务器.Text, "[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}"))
            //{
            //    MessageBox.Show("交易服务器不符合规范，格式应为ip地址或网址带端口号，如 192.168.1.1", this.Title);
            //    return false;
            //}

            if (string.IsNullOrEmpty(this.txt版本号.Text))
            {
                MessageBox.Show("版本号不能为空!", this.Title);
                return false;
            }
          
            if (string.IsNullOrEmpty(this.txt帐号.Text))
            {
                MessageBox.Show("帐号不能为空!", this.Title);
                return false;
            }
            if (string.IsNullOrEmpty(this.txt密码.Password))
            {
                MessageBox.Show("密码不能为空!", this.Title);
                return false;
            }
            else if (this.ckIsEncrypt.IsChecked == true)
            {
                try
                {
                    Cryptor.MD5Decrypt(txt密码.Password);
                }
                catch (Exception)
                {
                    MessageBox.Show("密码解密失败，请确认输入的是加密后的密码！", this.Title);
                    return false;
                }
            }

            return true;
        }

        private void Button_SearchAccountInfo_Click(object sender, RoutedEventArgs e)
        {
            var entity = this.GetPageEntity();
            ImsApi.Init(ImsNotify);
            string errMsg = string.Empty;
            
            bool isLogin = ImsApi.Login(entity.交易帐号, Cryptor.MD5Decrypt(entity.交易密码), entity.版本号, out errMsg);
            if (!string.IsNullOrEmpty(errMsg) || isLogin)
            {
                MessageBox.Show(errMsg);
                return;
            }

            StringBuilder Result = new StringBuilder(1024);
            StringBuilder ErrInfo = new StringBuilder(1024);

            if (string.IsNullOrEmpty(txt产品信息.Text))
            {
                ImsApi.ImsPbClient_RequestProductList(0, Result, ErrInfo);
                lblErrInfo产品信息.Text = ErrInfo.ToString();
                var dt = CommonUtils.ChangeDataStringToTable(Result.ToString());
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("productid"))
                {
                    this.txt产品信息.Text = dt.Rows[0]["productid"] + "";
                }
            }

            if (string.IsNullOrEmpty(txt资产单元.Text))
            {
                ImsApi.ImsPbClient_RequestProductList(1, Result, ErrInfo);
                //this.txt资产单元.Text = Result.ToString();
                this.lblErrInfo资产单元.Text = ErrInfo.ToString();
                var dt = CommonUtils.ChangeDataStringToTable(Result.ToString());
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("productunitid"))
                {
                    this.txt资产单元.Text = dt.Rows[0]["productunitid"] + "";
                }
            }

            if (string.IsNullOrEmpty(txt投资组合.Text))
            {
                ImsApi.ImsPbClient_RequestProductList(2, Result, ErrInfo);
                //this.txt投资组合.Text = Result.ToString();
                this.lblErrInfo投资组合.Text = ErrInfo.ToString();
                var dt = CommonUtils.ChangeDataStringToTable(Result.ToString());
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("portfolioid"))
                {
                    this.txt投资组合.Text = dt.Rows[0]["portfolioid"] + "";
                }
            }

            btnSave.IsEnabled = true;
        }

        private void ImsNotify(string obj)
        {
            CommonUtils.Log(obj);
        }
    }
}
