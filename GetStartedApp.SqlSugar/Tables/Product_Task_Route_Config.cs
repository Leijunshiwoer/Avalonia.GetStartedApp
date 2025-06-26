using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 产品任务与工艺路线 多对多 关系表
    /// </summary>
    [SugarTable(tableName: "Product_Task_Route_Config")]
    public class Product_Task_Route_Config : AutoIncrementEntity
    {
        /// <summary>
        /// 产品任务ID
        /// </summary>
        public int TaskId { get; set; }
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public int RouteId { get; set; }
        /// <summary>
        /// 是否主流程
        /// </summary>
        [SugarColumn(Length = 1)]
        public string IsPrimary { get; set; } = "N";
    }
}
