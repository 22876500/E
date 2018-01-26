using LimitManagement.AASServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimitManagement.Entities
{
    public class ServerLimitRow : INotifyPropertyChanged
    {
        private ServerLimitRow()
        { }

        public ServerLimitRow(AASServiceReference.DbDataSet.额度分配Row row, ServerInfo serverInfo)
        {
            OriginalRow = row;
            Server = serverInfo;
        }

        private AASServiceReference.DbDataSet.额度分配Row OriginalRow { get; set; }

        private ServerInfo Server { get; set; }

        public string Remark {
            get {
                return Server == null ? null : Server.Remark;
            }
        }

        public string Ip { get { return Server == null ? null : Server.Ip; } }

        public string 交易员
        {
            get
            {
                return OriginalRow == null ? null : OriginalRow.交易员;
            }
            //set
            //{
            //    OriginalRow.交易员 = value;
            //}
        }

        public string 证券代码
        {
            get
            {
                return OriginalRow == null ? null : OriginalRow.证券代码;
            }
            set
            {
                OriginalRow.证券代码 = value;
            }
        }

        public string 组合号
        {
            get
            {
                return OriginalRow == null ? null : OriginalRow.组合号;
            }
            set
            {
                OriginalRow.组合号 = value;
            }
        }

        public byte 市场
        {
            get
            {
                return OriginalRow == null ? (byte)0 : OriginalRow.市场;
            }
            set
            {
                OriginalRow.市场 = value;
            }
        }

        public string 证券名称
        {
            get
            {
                return OriginalRow == null ? null : OriginalRow.证券名称;
            }
            set
            {
                OriginalRow.证券名称 = value;
            }
        }

        public string 拼音缩写
        {
            get
            {
                return OriginalRow == null ? null : OriginalRow.拼音缩写;
            }
            set
            {
                OriginalRow.拼音缩写 = value;
            }
        }

        public int 买模式
        {
            get
            {
                return OriginalRow == null ? 0 : OriginalRow.买模式;
            }
            set
            {
                OriginalRow.买模式 = value;
            }
        }

        public int 卖模式
        {
            get
            {
                return OriginalRow == null ? 0 : OriginalRow.卖模式;
            }
            set
            {
                OriginalRow.卖模式 = value;
            }
        }

        public decimal 交易额度
        {
            get { return OriginalRow == null ? 0 : OriginalRow.交易额度; }
            set
            { 
                OriginalRow.交易额度 = value;
                NotifyPropertyChange("交易额度");
            }
        }

        public decimal 手续费率
        {
            get { return OriginalRow == null ? 0 : OriginalRow.手续费率; }
            set { OriginalRow.手续费率 = value; }
        }

        private bool _isSelected;
        public bool IsSelected
        { 
            get { return _isSelected; }
            set 
            {
                _isSelected = value;
                NotifyPropertyChange("IsSelected");
            }
        }
        public DbDataSet.额度分配Row Update(decimal limitQty, out string errMsg)
        {
            var table = new DbDataSet.额度分配DataTable();
            table.ImportRow(OriginalRow);
            var rowUpdate = table.FirstOrDefault();
            rowUpdate.交易额度 = limitQty;

            return Server.UpdateLimit(table, out errMsg);
        }

        public bool DeleteLimit(out string errMsg)
        {
            return Server.DeleteLimit(交易员, 证券代码, 组合号, out errMsg);
        }

        public void RefreshData(DbDataSet.额度分配Row updatedRow)
        {
            交易额度 = updatedRow.交易额度;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        
    }
}
