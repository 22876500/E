using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class OrdersEntity
    {
        /// <summary>
        /// 下单时带入的id
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        /// 订单编码
        /// </summary>
        public string ord_no { get; set; }

        /// <summary>
        /// 订单状态  参见数据字典章节
        /// </summary>
        public string ord_status { get; set; }

        /// <summary>
        /// 账户类型  参见数据字典章节
        /// </summary>
        public string acct_type { get; set; }

        /// <summary>
        /// 交易账户
        /// </summary>
        public string acct { get; set; }

        /// <summary>
        /// CATS账户  如果该交易账户有多个CATS账户绑定，可用于识别
        /// </summary>
        public string cats_acct { get; set; }

        /// <summary>
        /// 标的代码
        /// </summary>
        public string symbol { get; set; }

        /// <summary>
        /// 交易方向  参见数据字典章节
        /// </summary>
        public string tradeside { get; set; }

        /// <summary>
        /// 委托数量
        /// </summary>
        public string ord_qty { get; set; }

        /// <summary>
        /// 委托价格
        /// </summary>
        public string ord_px { get; set; }

        /// <summary>
        /// 委托类型  参见数据字典章节
        /// </summary>
        public string ord_type { get; set; }

        /// <summary>
        /// 委托参数  预留字段，如委托时填写会回填至此
        /// </summary>
        public string ord_param { get; set; }

        /// <summary>
        /// 下单来源类型  本面板下单固定为CLIENT_SCAN_ORDER
        /// </summary>
        public string corr_type { get; set; }

        /// <summary>
        /// 下单来源ID
        /// </summary>
        public string corr_id { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public string filled_qty { get; set; }

        /// <summary>
        /// 成交均价
        /// </summary>
        public string avg_px { get; set; }

        /// <summary>
        /// 撤单数量
        /// </summary>
        public string cxl_qty { get; set; }

        /// <summary>
        /// 委托时间  格式yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string ord_time { get; set; }

        /// <summary>
        /// 错误信息  如委托、撤单请求失败时有值
        /// </summary>
        public string err_msg { get; set; }
    }
}
