using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranInfoManager.Entity;

namespace TranInfoManager
{
    public class ManageDataset : DbContext
    {
        public ManageDataset()
            : base(CommonUtils.DbConnectString)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ManageDataset>());//如果模型不存在则创建。
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MarketListConfiguration());
            modelBuilder.Configurations.Add(new CompareDailyConfigration());

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<MarketDetailDaily> MarketDailyDS { get;set; }

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

    public class DataHandler
    {
        public static int DeleteTraderData( DateTime date)
        {
            int count = 0;
            string sql = string.Format("delete [dbo].[TraderDailyDetails] where [Date] = @date");
            using (var conn = new SqlConnection(CommonUtils.DbConnectString))
            {
                conn.Open();
                var cmd = new SqlCommand() { CommandText = sql, Connection = conn };
                cmd.Parameters.AddWithValue("@date", date);
                count = cmd.ExecuteNonQuery();
            }
            return count;
        }

        public static int DeleteMarketDataInfo(DateTime date)
        {
            int count = 0;
            string sql = string.Format("delete [dbo].[MarketDetailDailies] where [TradeDate] = @date");
            using (var conn = new SqlConnection(CommonUtils.DbConnectString))
            {
                conn.Open();
                var cmd = new SqlCommand() { CommandText = sql, Connection = conn };
                var para = cmd.Parameters.AddWithValue("@date", date);
                count = cmd.ExecuteNonQuery();
            }
            return count;
        }
    }
}
