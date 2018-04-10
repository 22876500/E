using DataComparision.DataAdapter;
using DataComparision.Entity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataComparision.Controls
{
    /// <summary>
    /// OutputExcel.xaml 的交互逻辑
    /// </summary>
    public partial class OutputExcel : UserControl
    {
       

        private DataTable ExportData { get; set; }

        public OutputExcel()
        {
            InitializeComponent();
            
            this.Loaded += OutputExcel_Loaded;
            this.dpStart.SelectedDate = DateTime.Today.AddDays(-1);
            this.dpEnd.SelectedDate = DateTime.Today.AddDays(-1);
        }

        void OutputExcel_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        public void Init()
        {
            try
            {
                using (var db = new DataComparisionDataset())
                {
                    var groups = CommonUtils.DictGroup.Values.Where(_=>_.启用 == true).ToList();
                    if (groups != null && groups.Count > 0)
                    {
                        cmbGroup.ItemsSource = groups;
                        cmbGroup.DisplayMemberPath = "名称";
                    }
                }
            }
            catch{ }
        }

        #region Events
        private void Button_History_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateHistory())
            {
                loading.ShowLoading();
                var st = dpStart.SelectedDate.Value.Date;
                var et = dpEnd.SelectedDate.Value.Date;
                int historyDataType = int.Parse((cbHisType.SelectedItem as ComboBoxItem).DataContext.ToString());
                var o = cmbGroup.SelectedItem as 券商;

                this.Dispatcher.RunAsync(() =>
                {
                    DataTable dt = DataAdapter.DataHelper.QueryHisData(st, et, o, historyDataType);
                    this.ExportData = dt;
                }, null, null, () => {
                    dgDelivery.ItemsSource = this.ExportData == null ? null : this.ExportData.DefaultView;
                    loading.HideLoading();
                });
            }
        }

        private void Button_Today_Click(object sender, RoutedEventArgs e)
        {
            int tradeDataType = int.Parse((cbTodayType.SelectedItem as ComboBoxItem).DataContext.ToString());
            var o = cmbGroup.SelectedItem as 券商;
            loading.ShowLoading();
            this.Dispatcher.RunAsync(() =>
            {
                DataTable dt = DataAdapter.DataHelper.QueryTradeData(o, tradeDataType);
                if (dt != null)
                {
                    this.ExportData = dt;
                }
            }, null, null, () => 
            {
                dgDelivery.ItemsSource = this.ExportData == null ? null : this.ExportData.DefaultView;
                loading.HideLoading();
            });
            

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (spHistory == null || spToday == null)
                return;

            var rd = sender as RadioButton;
            if (rd.Content.ToString().Contains("历史"))
            {
                this.spToday.Visibility = Visibility.Collapsed;
                this.spHistory.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.spToday.Visibility = Visibility.Visible;
                this.spHistory.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Button_Export_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel();
        }
        #endregion

        private bool ValidateHistory()
        {
            if (!dpStart.SelectedDate.HasValue)
            {
                dpStart.Focus();
                MessageBox.Show("请选择开始日期！");
                return false;
            }
            else if (!dpEnd.SelectedDate.HasValue)
            {
                dpEnd.Focus();
                MessageBox.Show("请选择截止日期！");
                return false;
            }
            else if (cmbGroup.SelectedIndex < 0)
            {
                MessageBox.Show("请选择组合号！");
                return false;
            }
            return true;
        }

        private void ExportToExcel()
        {
            var dt = this.ExportData;
            if (dt != null)
            {
                SaveFileDialog s = new SaveFileDialog() { Filter = "Excel 97~2003|*.xls|Excel 2007/2010|*.xlsx" };
                if (s.ShowDialog() == true && !string.IsNullOrEmpty(s.FileName))
                {
                    using (var stream = File.OpenWrite(s.FileName))
                    {
                        var workbook = Utils.ExcelUtils.RenderToExcel(dt);
                        workbook.Write(stream);
                    }
                }
            }
            else
            {
                CommonUtils.ShowMsg("无可导出内容！");
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (cbHisType.SelectedIndex != 0)
                return;

            var o = cmbGroup.SelectedItem as 券商;
            var dt = ExportData;
            var sd = dpStart.SelectedDate.Value;
            var ed = dpEnd.SelectedDate.Value;

            bool saveResult = false;
            this.loading.ShowLoading();
            Dispatcher.RunAsync(() => 
            {
                try
                {
                    DataHelper.StandardDeliveryDataTable(dt);
                    using (var db = new DataComparisionDataset())
                    {
                        var oldData = db.交割单ds.Where(_ => _.交割日期 >= sd && _.交割日期 <= ed);
                        if (oldData.Count() > 0)
                        {
                            db.交割单ds.RemoveRange(oldData);
                            db.SaveChanges();
                        }
                    }
                    saveResult = DataHelper.WriteToDB(dt, "交割单");
                }
                catch (Exception ex)
                {
                    Dispatcher.ShowMsg("保存出错：" +ex.Message);
                }
                
            }, null, null, 
            () => {
                this.dgDelivery.ItemsSource = null;
                this.loading.HideLoading();
                if (saveResult)
                {
                    CommonUtils.ShowMsg("保存完毕!");
                }
                else
                {
                    CommonUtils.ShowMsg("保存失败!");
                }
                
            });
            
            
        }
    }
}
