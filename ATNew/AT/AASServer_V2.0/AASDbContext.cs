using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASServer
{
    public enum 数据表 { 成交, 委托, 平台用户, 券商帐户, 额度分配, 已发委托, 已处理成交, 订单, 已平仓订单, 风控分配, 交易日志, MAC地址分配 };


    public enum 买模式 { 现金买入 = 0, 融资买入 = 1, 买券还券 = 2, 担保品买入 = 3 }

    public enum 卖模式 { 现券卖出 = 0, 融券卖出 = 1, 卖券还款 = 2, 担保品卖出 = 3 }

    public class AASUserConfiguration : EntityTypeConfiguration<平台用户>
    {
        public AASUserConfiguration()
        {
            //配置手续费率的精度为18，小数位数为5
            Property(t => t.手续费率).HasPrecision(18, 5);
        }
    }

    public class AASTraderLimitConfiguration : EntityTypeConfiguration<额度分配>
    {
        public AASTraderLimitConfiguration()
        {
            Property(t => t.手续费率).HasPrecision(18, 6);
        }

    }

    public class AASOrderConfig : EntityTypeConfiguration<已发委托>
    {
        public AASOrderConfig()
        {
            Property(t => t.成交价格).HasPrecision(18, 4);
        }
    }

    public class AASAccountConfig : EntityTypeConfiguration<券商帐户>
    {
        public AASAccountConfig()
        {
            Property(t => t.手续费率).HasPrecision(18, 6);
        }
    }

    class AASDbContext : DbContext
    {
        public AASDbContext()
        {
            string serverName = ConfigCache.DbServerName;
            string port = ConfigCache.ConnectPort;
            string dbName = ConfigCache.DBName;

            this.Database.Connection.ConnectionString = string.Format("Data Source=.\\{0};Initial Catalog={1};Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFrameworkMUE", serverName, dbName);
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AASDbContext>());//模型发生变化则drop后生成新的模型
            Database.SetInitializer(new CreateDatabaseIfNotExists<AASDbContext>());//如果模型不存在则创建。
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           //注册配置
            modelBuilder.Configurations.Add(new AASUserConfiguration());
            modelBuilder.Configurations.Add(new AASTraderLimitConfiguration());
            modelBuilder.Configurations.Add(new AASOrderConfig());
            modelBuilder.Configurations.Add(new AASAccountConfig());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<平台用户> 平台用户 { get; set; }

        public DbSet<券商帐户> 券商帐户 { get; set; }

        public DbSet<额度分配> 额度分配 { get; set; }

        public DbSet<已发委托> 已发委托 { get; set; }


        public DbSet<已处理成交> 已处理成交 { get; set; }

        public DbSet<订单> 订单 { get; set; }

        public DbSet<已平仓订单> 已平仓订单 { get; set; }


        public DbSet<风控分配> 风控分配 { get; set; }


        public DbSet<交易日志> 交易日志 { get; set; }

        public DbSet<MAC地址分配> MAC地址分配 { get; set; }

        //public DbSet<顶点账户> 顶点账户 { get; set; }

        public DbSet<恒生帐户> 恒生帐户 { get; set; }

        public DbSet<可用仓位> 可用仓位 { get; set; }

        public DbSet<组合号交易员关联> 组合号交易员关联 { get; set; }
    }

    public class MAC地址分配
    {
        [Key, Column(Order = 0)]
        public string 用户名 { get; set; }

        [Key, Column(Order = 1), MinLength(12), MaxLength(12)]
        public string MAC { get; set; }
    }



    public class 恒生帐户
    {
        [Key, MinLength(1)]
        public string 名称 { get; set; }

     
        public bool 启用 { get; set; }


        [MinLength(7)]
        public string IP { get; set; }


        public short 端口 { get; set; }



        [MinLength(1)]
        public string 基金编码 { get; set; }

         [MinLength(1)]
        public string 资产单元编号 { get; set; }

        [MinLength(1)]
        public string 组合编号 { get; set; }

        [MinLength(1)]
        public string 操作员用户名 { get; set; }

        [MinLength(1)]
        public string 操作员密码 { get; set; }


        [MinLength(7)]
        public string 登录IP { get; set; }


        [MinLength(12)]
        public string MAC { get; set; }


        [MinLength(1)]
        public string HDD { get; set; }


        public int 查询间隔时间 { get; set; }
    }




    public class 平台用户
    {
        [Key,  MinLength(1)]
        public string 用户名 { get; set; }

        [MinLength(1)]
        public string 密码 { get; set; }

        public 角色 角色 { get; set; }


        public decimal 仓位限制 { get; set; }

        public decimal 亏损限制 { get; set; }

        public decimal 手续费率 { get; set; }


        public bool 允许删除碎股订单 { get; set; }

        public 分组 分组 { get; set; }
    }


    public class 券商帐户
    {
        [Key, MinLength(1)]
        public string 名称 { get; set; }


     

        public bool 启用 { get; set; }

        public string 券商 { get; set; }

        public string 类型 { get; set; }


        public string 交易服务器 { get; set; }

        [MinLength(1)]
        public string 版本号 { get; set; }

        public short 营业部代码 { get; set; }

        [MinLength(1)]
        public string 登录帐号 { get; set; }

        [MinLength(1)]
        public string 交易帐号 { get; set; }

        [MinLength(1)]
        public string 交易密码 { get; set; }

        public string 通讯密码 { get; set; }

        public string 上海股东代码 { get; set; }


        public string 深圳股东代码 { get; set; }


        public int 查询间隔时间 { get; set; }

        public decimal 手续费率 { get; set; }

        public 买模式 买入方式 { get; set; }

        public 卖模式 卖出方式 { get; set; }
    }


    public class 额度分配
    {
        [Key, Column(Order = 0)]
        public string 交易员 { get; set; }

        [Key, Column(Order = 1), MinLength(5), MaxLength(6)]
        public string 证券代码 { get; set; }

        [Key, Column(Order = 2)]
        public string 组合号 { get; set; }

        public byte 市场 { get; set; }



        [MinLength(1), MaxLength(10)]
        public string 证券名称 { get; set; }


        [MinLength(1), MaxLength(10)]
        public string 拼音缩写 { get; set; }

        public 买模式 买模式 { get; set; }

        public 卖模式 卖模式 { get; set; }

        public decimal 交易额度 { get; set; }

        public decimal 手续费率 { get; set; }
    }





    public class 风控分配
    {

         [Key, Column(Order = 0)]
        public string 交易员 { get; set; }

         [Key, Column(Order = 1)]
        public string 风控员 { get; set; }
    }


   

    public class 已发委托
    {
        [Key, Column(Order = 0)]

        public DateTime 日期 { get; set; }

        [Key, Column(Order = 1)]

        public string 组合号 { get; set; }

        [Key, Column(Order = 2)]

        public string 委托编号 { get; set; }



      



        public string 交易员 { get; set; }

        public string 状态说明 { get; set; }//委托被券商服务器接受后暂时为成功状态，但是有可能被交易所打回成为废单
        public byte 市场代码 { get; set; }
        public string 证券代码 { get; set; }
        public string 证券名称 { get; set; }
        public int 买卖方向 { get; set; }
        public decimal 成交价格 { get; set; }
        public decimal 成交数量 { get; set; }

        public decimal 委托价格 { get; set; }
        public decimal 委托数量 { get; set; }
        public decimal 撤单数量 { get; set; }
    }

    public class 已处理成交
    {
        [Key, Column(Order = 0)]

        public DateTime 日期 { get; set; }



        [Key, Column(Order = 1)]
        public string 组合号 { get; set; }

       

        [Key, Column(Order = 2)]

        public string 委托编号 { get; set; }

        [Key, Column(Order = 3)]

        public string 成交编号 { get; set; }
    }


   

    public class 订单
    {
         [Key, Column(Order = 0)]
        public string 交易员 { get; set; }


        [Key, Column(Order = 1)]
        public string 组合号 { get; set; }



         [Key, Column(Order = 2)]
        public string 证券代码 { get; set; }




        public string 证券名称 { get; set; }

        public DateTime 开仓时间 { get; set; }

       

        public int 开仓类别 { get; set; }

        public decimal 已开数量 { get; set; }

        public decimal 已开金额 { get; set; }

        public decimal 开仓价位 { get; set; }

        public decimal 当前价位 { get; set; }

        public decimal 浮动盈亏 { get; set; }


        public DateTime 平仓时间 { get; set; }

       

        public int 平仓类别 { get; set; }

        public decimal 已平数量 { get; set; }

        public decimal 已平金额 { get; set; }

        public decimal 平仓价位 { get; set; }


        public byte 市场代码 { get; set; }


       

    }

    public class 已平仓订单
    {
        public int ID { get; set; }
        public string 交易员 { get; set; }
        public string 组合号 { get; set; }

        public string 证券代码 { get; set; }

        public string 证券名称 { get; set; }

        public DateTime 开仓时间 { get; set; }

     

        public int 开仓类别 { get; set; }

        public decimal 已开数量 { get; set; }

        public decimal 已开金额 { get; set; }

        public decimal 开仓价位 { get; set; }

     


        public DateTime 平仓时间 { get; set; }
        public DateTime 平仓日期
        {
            get
            {
                return this.平仓时间.Date;
            }
        }


        public int 平仓类别 { get; set; }

        public decimal 已平数量 { get; set; }

        public decimal 已平金额 { get; set; }

        public decimal 平仓价位 { get; set; }



        public decimal 毛利 { get; set; }

        public decimal 佣金 { get; set; }


        public decimal 印花税 { get; set; }

        public decimal 过户费 { get; set; }

        public decimal 净利润 { get; set; }


        public decimal 交易费用 { get; set; }

    }



    public class 交易日志
    {
        public int ID { get; set; }
        public DateTime 日期 { get; set; }

        public string 时间 { get; set; }

        public string 交易员 { get; set; }

        public string 组合号 { get; set; }

        public string 证券代码 { get; set; }

        public string 证券名称 { get; set; }

        public string 委托编号 { get; set; }

        public int 买卖方向 { get; set; }

        public decimal 委托数量 { get; set; }

        public decimal 委托价格 { get; set; }

        public string 信息 { get; set; }
    }

    public class 可用仓位
    {
        [Key, Column(Order = 0)]
        public string 组合号 { get; set; }

        [Key, Column(Order = 1)]
        public string 证券代码 { get; set; }

        public string 证券名称 { get; set; }

        public decimal 总仓位 { get; set; }

        //public decimal 剩余数量 { get; set; }


    }


    public class 组合号交易员关联
    {
        [Key, Column(Order = 0)]
        public string 组合号 { get; set; }

        [Key, Column(Order = 1)]
        public string 交易员 { get; set; }
    }
    //public class 顶点账户
    //{

    //}
}
