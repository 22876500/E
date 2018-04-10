using DataComparision.Entity;
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

namespace DataComparision.View
{
    /// <summary>
    /// SoftwareEdit.xaml 的交互逻辑
    /// </summary>
    public partial class SoftwareEdit : Window
    {

        public Action OnEditComplete;

        软件委托 Entity { get; set; }

        bool IsCreateNew { get; set; }

        private SoftwareEdit()
        {
            InitializeComponent();
        }

        public SoftwareEdit(bool isAddNew, 软件委托 item)
        {
            InitializeComponent();
            //IsCreateNew = isAddNew;
            //Entity = item;
            InitPage(isAddNew, item);
        }

        private void InitPage(bool isNew, 软件委托 entity)
        {
            this.Entity = entity;
            this.IsCreateNew = isNew;

            this.txt成交日期.Text = entity.成交日期.ToString("yyyy/MM/dd");
            this.txt交易员.Text = entity.交易员 + "";
            this.txt组合号.Text = entity.组合号 + "";
            this.txt证券代码.Text = entity.证券代码 + "";
            this.txt证券名称.Text = entity.证券名称 + "";
            this.txt买卖标志.Text = entity.买卖标志 + "";

            if (isNew)
            {
                this.Title = "软件委托编辑";

                this.txt状态说明.Text = "手动添加";
            }
            else
            {
                this.Title = "软件委托编辑";
                this.txt委托时间.Text = entity.委托时间 + "";
                this.txt委托编号.Text = entity.委托编号 + "";
                this.txt委托价格.Text = entity.委托价格 + "";
                this.txt委托数量.Text = entity.委托数量 + "";
                this.txt成交价格.Text = entity.成交价格 + "";
                this.txt成交数量.Text = entity.成交数量 + "";
                this.txt撤单数量.Text = entity.撤单数量 + "";
                this.txt状态说明.Text = entity.状态说明 + "";
                this.txt券商名称.Text = entity.券商名称 + "";

                this.txt成交日期.IsReadOnly = true;
                this.txt组合号.IsReadOnly = true;
                this.txt证券代码.IsReadOnly = true;
                this.txt委托编号.IsReadOnly = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new DataComparisionDataset())
                {
                    if (IsCreateNew)
                    {
                        var item = SetEntityFromPage(Entity);
                        
                        db.软件委托ds.Add(item);
                        db.SaveChanges();
                    }
                    else
                    {
                        var list = db.软件委托ds.Where(_ => _.组合号 == Entity.组合号 && _.成交日期 == Entity.成交日期).ToList();
                        if (list.Count > 0)
                        {
                            var item = list.FirstOrDefault(_ => _.IsSame(Entity));
                            if (item != null)
                            {
                                SetEntityFromPage(item);
                                db.SaveChanges();
                            }
                            else
                                CommonUtils.ShowMsg("未找到数据对应项");
                        }
                        else
                            CommonUtils.ShowMsg("未找到数据对应项");
                    }
                }
                if (OnEditComplete != null)
                {
                    OnEditComplete.Invoke();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                CommonUtils.ShowMsg("保存失败！" + ex.Message);
            }

        }

        private 软件委托 SetEntityFromPage(软件委托 item)
        {
            item.交易员 = this.txt交易员.Text;
            item.组合号 = this.txt组合号.Text;
            item.成交日期 = DateTime.Parse(this.txt成交日期.Text);
            item.证券代码 = this.txt证券代码.Text;
            item.证券名称 = this.txt证券名称.Text;
            item.买卖标志 = this.txt买卖标志.Text;
            item.成交数量 = CommonUtils.GetDecimal(this.txt成交数量.Text);
            item.成交价格 = CommonUtils.GetDecimal(this.txt成交价格.Text);
            item.委托时间 = this.txt委托时间.Text;
            item.委托编号 = this.txt委托编号.Text;
            item.委托价格 =  CommonUtils.GetDecimal(this.txt委托价格.Text);
            item.委托数量 =  CommonUtils.GetDecimal(this.txt委托数量.Text);
            item.撤单数量 =  CommonUtils.GetDecimal(this.txt撤单数量.Text);
            item.状态说明 = this.txt状态说明.Text;
            item.券商名称 = this.txt券商名称.Text;
            return item;
        }
    }
}
