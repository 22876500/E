using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace TradeService
{
    public class TradeDataset : DbContext
    {
        public TradeDataset()
            : base(CommonUtils.DbConnectString)
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ManageDataset>());//如果模型不存在则创建。
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MarketListConfiguration());
            modelBuilder.Configurations.Add(new CompareDailyConfigration());

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<MarketDetailDaily> MarketDailyDS { get; set; }

        public DbSet<TraderDailyDetail> TraderDailyDS { get; set; }

        public DbSet<CompareDaily> CompareDailyDS { get; set; }

        //public DbSet<CompareTrader> CompareTraderDS { get; set; }

    }

    public class MarketListConfiguration : EntityTypeConfiguration<MarketDetailDaily>
    {
        public MarketListConfiguration()
        {
            Property(t => t.ECNRebate).HasPrecision(18, 4);
            Property(t => t.ExchangeFee).HasPrecision(18, 5);

            Property(t => t.NasdTradingActivityFee).HasPrecision(18, 6);
            Property(t => t.NetMoney).HasPrecision(18, 4);
            Property(t => t.NSCCPassThru).HasPrecision(18, 14);
            Property(t => t.SIPC).HasPrecision(18, 8);
            Property(t => t.FirstMoney).HasPrecision(18, 5);
            Property(t => t.TotalCommission_WithoutFeesAndCharges).HasPrecision(18, 4);
        }
    }

    public class CompareDailyConfigration : EntityTypeConfiguration<CompareDaily>
    {
        public CompareDailyConfigration()
        {
            Property(t => t.Gross).HasPrecision(18, 5);
            Property(t => t.Comm).HasPrecision(18, 4);
            Property(t => t.ECN).HasPrecision(18, 4);
            Property(t => t.Other).HasPrecision(18, 4);
        }
    }
}