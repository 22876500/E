using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASClient.Model
{
    public enum SubType
    {
        None = 0,
        /// <summary>
        /// 沪深300
        /// </summary>
        HS300 = 1,
        /// <summary>
        /// 中正500
        /// </summary>
        ZZ500 = 2,
        /// <summary>
        /// 自选股
        /// </summary>
        Portfolio = 3,
        /// <summary>
        /// 所有(包含初始订阅列表，中正500，沪深300及自选股)
        /// </summary>
        All = 4,
    }

    public enum CalculateType
    {
        /// <summary>
        /// 加法运算
        /// </summary>
        Add = 0,
        /// <summary>
        /// 减法运算
        /// </summary>
        Sub = 1,
        /// <summary>
        /// 乘法运算
        /// </summary>
        Mul = 2,
        /// <summary>
        /// 除法运算
        /// </summary>
        Div = 3,
    }

    public enum CompareType
    { 
        /// <summary>
        /// 大于
        /// </summary>
        More = 0,
        /// <summary>
        /// 大于等于
        /// </summary>
        MoreOrEqual = 1,
        /// <summary>
        /// 小于
        /// </summary>
        Less = 2,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessOrEqual = 3,
    }

    public enum WarningLevel
    {
        Yellow = 0,
        Red = 1,
    }
}
