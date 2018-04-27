using DataComparision.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace DataComparision
{
    public class DataComparisionDataset : DbContext
    {
        public DataComparisionDataset()
            : base(CommonUtils.DBConnection)
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataComparisionDataset>());
            Database.SetInitializer(new CreateDatabaseIfNotExists<DataComparisionDataset>());//如果模型不存在则创建。
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //注册配置
            modelBuilder.Configurations.Add(new DeliveryListConfiguration());
            modelBuilder.Configurations.Add(new SoftwareDelegateConfiguration());
            modelBuilder.Configurations.Add(new AmountConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<交割单> 交割单ds { get; set; }

        public DbSet<软件委托> 软件委托ds { get; set; }

        public DbSet<合计表> 合计表ds { get; set; }

        public DbSet<券商> 券商ds { get; set; }
    }

    public class DeliveryListConfiguration : EntityTypeConfiguration<交割单>
    {
        public DeliveryListConfiguration()
        {
            Property(t => t.成交价格).HasPrecision(18, 4);
            Property(t => t.成交金额).HasPrecision(18, 4);

            Property(t => t.发生金额).HasPrecision(18, 4);
            Property(t => t.手续费).HasPrecision(18, 4);
            Property(t => t.印花税).HasPrecision(18, 4);
            Property(t => t.过户费).HasPrecision(18, 4);
            Property(t => t.其他费).HasPrecision(18, 4);
        }
    }

    public class SoftwareDelegateConfiguration : EntityTypeConfiguration<软件委托>
    {
        public SoftwareDelegateConfiguration()
        {
            Property(t => t.成交价格).HasPrecision(18, 4);
            Property(t => t.委托价格).HasPrecision(18, 4);
        }
    }

    public class AmountConfiguration : EntityTypeConfiguration<合计表>
    {
        public AmountConfiguration()
        {
            Property(t => t.成交金额).HasPrecision(18, 4);
            Property(t => t.发生金额).HasPrecision(18, 4);
        }
    }


}
