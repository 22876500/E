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
    /// DeliveryEdit.xaml 的交互逻辑
    /// </summary>
    public partial class DeliveryEdit : Window
    {
        public  Action OnEditComplete;

        交割单 Entity { get; set; }
        bool IsCreateNew { get; set; }

        private DeliveryEdit()
        {
            InitializeComponent();
        }

        public DeliveryEdit(bool isNew ,交割单 entity)
        {
            InitializeComponent();

            InitPage(isNew, entity);
        }

        private void InitPage(bool isNew, 交割单 entity)
        {
            this.Entity = entity;
            this.IsCreateNew = isNew;

            this.txt组合号.Text = entity.组合号;
            this.txt交割日期.Text = entity.交割日期.ToShortDateString();
            this.txt证券代码.Text = entity.证券代码 + "";
            this.txt证券名称.Text = entity.证券名称 + "";
            this.txt买卖标志.Text = entity.买卖标志 + "";
            this.txt序号.Text = entity.SortSequence.ToString();

            if (isNew)
            {
                this.Title = "交割单新增";

                this.txt备注.Text = "手动添加";
            }
            else
            {
                this.Title = "交割单编辑";

                this.txt成交数量.Text = entity.成交数量 + "";
                this.txt成交价格.Text = entity.成交价格 + "";
                this.txt成交金额.Text = entity.成交金额 + "";
                this.txt发生金额.Text = entity.发生金额 + "";
                this.txt手续费.Text = entity.手续费 + "";
                this.txt印花税.Text = entity.印花税 + "";
                this.txt过户费.Text = entity.过户费 + "";
                this.txt其他费.Text = entity.其他费 + "";
                this.txt备注.Text = entity.备注;
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
                        item.OrderID = Guid.NewGuid().ToString();
                        db.交割单ds.Add(item);
                        db.SaveChanges();
                    }
                    else
                    {
                        var list = db.交割单ds.Where(_ => _.组合号 == Entity.组合号 && _.交割日期 == Entity.交割日期).ToList();
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
                CommonUtils.ShowMsg( "保存失败！" + ex.Message);
            }
            
        }

        private 交割单 SetEntityFromPage(交割单 item)
        {
            item.组合号 = this.txt组合号.Text;
            item.交割日期 = DateTime.Parse(this.txt交割日期.Text);
            item.证券代码 = this.txt证券代码.Text;
            item.证券名称 = this.txt证券名称.Text;
            item.买卖标志 = this.txt买卖标志.Text;
            item.成交数量 = CommonUtils.GetDecimal(this.txt成交数量.Text);
            item.成交价格 = CommonUtils.GetDecimal(this.txt成交价格.Text);
            if (string.IsNullOrEmpty(this.txt成交金额.Text) && item.成交价格 != 0 && item.成交数量 != 0)
            {
                item.发生金额 = item.成交数量 * item.成交价格;
            }
            else
            {
                item.成交金额 = CommonUtils.GetDecimal(this.txt成交金额.Text);
            }

            item.发生金额 = CommonUtils.GetDecimal(this.txt发生金额.Text);
            item.手续费 = CommonUtils.GetDecimal(this.txt手续费.Text);
            item.印花税 = CommonUtils.GetDecimal(this.txt印花税.Text);
            item.过户费 = CommonUtils.GetDecimal(this.txt过户费.Text);
            item.其他费 = CommonUtils.GetDecimal(this.txt其他费.Text);
            item.备注 = this.txt备注.Text;
            item.SortSequence = (int)CommonUtils.GetDecimal(this.txt序号.Text);
            return item;
        }


    }
}
