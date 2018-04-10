using DataComparision.DataAdapter;
using DataComparision.Entity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataComparision.Controls
{
    /// <summary>
    /// HisSearch.xaml 的交互逻辑
    /// </summary>
    public partial class HisSearch : UserControl
    {
        private DataTable ExportData { get; set; }
        private List<券商> Groups = new List<券商>();

        public HisSearch()
        {
            InitializeComponent();
            this.Loaded += HisSearch_Loaded;
            
        }

        void HisSearch_Loaded(object sender, RoutedEventArgs e)
        {
            InitGroup();
        }

        private void InitGroup()
        {
            try
            {
                if (Groups.Count == 0)
                {
                    Groups.AddRange(CommonUtils.DictGroup.Values.OrderBy(_=> _.名称));
                }
                if (cmbGroup.ItemsSource == null)
                {
                    cmbGroup.ItemsSource = Groups;
                    cmbGroup.DisplayMemberPath = "名称";
                    cmbGroup.SelectedIndex = 0;
                }
                //if (Groups.Count == 0 && CommonUtils.DictGroup.ContainsKey("F11") && CommonUtils.DictGroup.ContainsKey("F12"))
                //{
                //    var GroupItem1 = CommonUtils.DictGroup["F11"];
                //    var GroupItem2 = CommonUtils.DictGroup["F12"];
                //    Groups.Add(GroupItem1);
                //    Groups.Add(GroupItem2);
                //    cmbGroup.ItemsSource = Groups;
                //    cmbGroup.DisplayMemberPath = "名称";
                //    cmbGroup.SelectedIndex = 0;
                //}
                //else
                //{
                //    CommonUtils.Log("初始化券商信息失败！接口中数据未包含F11或F12");    
                //}
            }
            catch (Exception ex)
            {
                CommonUtils.Log("初始化券商信息失败！", ex);
            }
            
        }

        #region Events
        private void Button_History_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateHistory())
            {
                loading.ShowLoading();
                var st = dpStart.SelectedDate.Value.Date;
                var et = dpEnd.SelectedDate.Value.Date;
                var group = cmbGroup.SelectedItem as 券商;
                if (group == null)
                {
                    MessageBox.Show("请选择券商！");
                    return;
                }

                this.Dispatcher.RunAsync(() =>
                {
                    DataTable dt = DataAdapter.DataHelper.QueryHisData(st, et, group, 3, true);
                    this.ExportData = dt;
                }, null, null, () =>
                {
                    dgDelivery.ItemsSource = this.ExportData == null ? null : this.ExportData.DefaultView;
                    loading.HideLoading();
                });
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
                    Dispatcher.ShowMsg("保存出错：" + ex.Message);
                }

            }, null, null,
            () =>
            {
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
