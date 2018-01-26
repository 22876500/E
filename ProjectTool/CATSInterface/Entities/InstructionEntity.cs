using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class InstructionEntity
    {
        /// <summary>
        /// 指令类型，区分是下单还是撤单委托	是	O:下单委托(大写字母O) C:撤单委托
        /// </summary>
        public string InstType { get; set; }

        /// <summary>
        /// 外部ID 否 下单时使用 下单委托带入int，会输出到订单流水表关联的记录里
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 账户类型 是 参见数据字典章节
        /// </summary>
        public string AcctType { get; set; }

        /// <summary>
        /// 交易账户
        /// </summary>
        public string Acct { get; set; }

        /// <summary>
        /// 订单编码 撤单时必须
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 标的代码	下单时必须	带交易所，如600030.SH
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易方向	下单时必须
        /// </summary>
        public string TradeSide { get; set; }

        /// <summary>
        /// 委托数量	下单时必须	股票单位为股
        /// </summary>
        public int OrdQty { get; set; }

        /// <summary>
        /// 委托价格	下单时必须	市价单时建议填0
        /// </summary>
        public decimal OrdPrice { get; set; }

        /// <summary>
        /// 委托类型	下单时必须	参数数据字典章节
        /// </summary>
        public string OrdType { get; set; }

        /// <summary>
        /// 委托参数	否	预留字段
        /// </summary>
        public string OrdParam { get; set; }

    }
}
