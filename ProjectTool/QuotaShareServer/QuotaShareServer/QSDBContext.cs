using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaShareServer
{
    public enum BuyMode { 现金买入 = 0, 融资买入 = 1, 买券还券 = 2, 担保品买入 = 3 }

    public enum SaleMode { 现券卖出 = 0, 融券卖出 = 1, 卖券还款 = 2, 担保品卖出 = 3 }

    class QSDBContext : DbContext
    {
        public QSDBContext()
            : base("Data Source=.\\SQLEXPRESS;Initial Catalog=QuotaShareDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFrameworkMUE")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<QSDBContext>());//模型发生变化则drop后生成新的模型
            //Database.SetInitializer(new CreateDatabaseIfNotExists<QSDBContext>());//如果模型不存在则创建。
        }

        public DbSet<StockLimit> StockLimit { get; set; }
    }

    public class StockLimit
    {
        /// <summary>
        /// 组合号
        /// </summary>
        [Key, Column(Order = 0)]
        public string Account { get; set; }

        /// <summary>
        /// 证券代码
        /// </summary>
        [Key, Column(Order = 1), MinLength(5), MaxLength(6)]
        public string StockID { get; set; }

        /// <summary>
        /// 证券名称
        /// </summary>
        [MinLength(1), MaxLength(10)]
        public string StockName { get; set; }

        /// <summary>
        /// 市场（0 深市 1 沪市）
        /// </summary>
        public byte Market { get; set; }

        /// <summary>
        /// 简写(拼音首字母)
        /// </summary>
        [MinLength(1), MaxLength(10)]
        public string StockShortName { get; set; }

        /// <summary>
        /// 买模式
        /// </summary>
        public BuyMode BuyType { get; set; }

        /// <summary>
        /// 卖模式
        /// </summary>
        public SaleMode SaleType { get; set; }

        /// <summary>
        /// 总额度
        /// </summary>
        public decimal QtyCanUse { get; set; }

        /// <summary>
        /// 手续费率
        /// </summary>
        public decimal CommissionCharge { get; set; }
    }

    public class AccountConfig
    {
        public string Account { get; set; }

        /// <summary>
        /// 对应发送端的IP
        /// </summary>
        public string IP { get; set; }

        public int Port { get; set; }


    }

}
