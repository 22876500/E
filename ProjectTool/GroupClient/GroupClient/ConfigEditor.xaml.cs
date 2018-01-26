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
using System.Windows.Shapes;

namespace GroupClient
{
    /// <summary>
    /// ConfigEdit.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigEditor : Window
    {
        Regex regTime = new Regex("(0?[0-9]|1[0-9]|2[0-4]):[0-9]{2}");

        public ConfigEditor()
        {
            InitializeComponent();
            var st = CommonUtils.GetConfig("开始查询时间", "8:15");
            var et = CommonUtils.GetConfig("结束查询时间", "15:30");
            this.txtStartTime.Text = st;
            this.txtEndTime.Text = et;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (regTime.IsMatch(txtStartTime.Text) && regTime.IsMatch(txtEndTime.Text))
            {
                SetTime();
                
            }
        }

        private void SetTime()
        {
            DateTime st, et;
            try
            {
                st = DateTime.Parse(txtStartTime.Text);
                et = DateTime.Parse(txtEndTime.Text);

                CommonUtils.SetConfig("开始查询时间", st.ToShortTimeString());
                CommonUtils.SetConfig("结束查询时间", et.ToShortTimeString());
                //MessageBox.Show("设置完毕");
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("输入的时间格式不正确");
            }
        }

    }
}
