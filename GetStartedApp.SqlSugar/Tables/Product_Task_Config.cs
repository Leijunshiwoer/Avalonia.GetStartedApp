using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
    {
    [SugarTable(tableName: "Product_Task_Config")]
    public class Product_Task_Config : AutoIncrementEntity
        {
        [SugarColumn(ColumnDescription = "工单代码")]
        public string Code { get; set; }

        [SugarColumn(ColumnDescription = "工单名称")]
        public string Name { get; set; }

        [SugarColumn(ColumnDescription = "批次号")]
        public string Batch { get; set; }

        [SugarColumn(ColumnDescription = "批次名称")]
        public string BatchName { get; set; }

        [SugarColumn(ColumnDescription = "一维码起始序列号")]
        public int StartSerialNumber1 { get; set; }

        [SugarColumn(ColumnDescription = "二维码起始序列号")]
        public int StartSerialNumber2 { get; set; }

        [SugarColumn(ColumnDescription = "盒起始序列号")]
        public int StartSerialNumber3 { get; set; }

        [SugarColumn(ColumnDescription = "工单状态")]
        public Em_Task_Status Status { get; set; }

        [SugarColumn(ColumnDescription = "计划数量")]
        public int PlanQty { get; set; }

        [SugarColumn(ColumnDescription = "底座投料数")]
        public int DZPutIntoQty { get; set; }

        [SugarColumn(ColumnDescription = "底座NG数")]
        public int DZNgQty { get; set; }

        [SugarColumn(ColumnDescription = "管壳投料数")]
        public int GKPutIntoQty { get; set; }

        [SugarColumn(ColumnDescription = "管壳NG数")]
        public int GKNgQty { get; set; }

        [SugarColumn(ColumnDescription = "合并成品数")]
        public int MergeQty { get; set; }

        [SugarColumn(ColumnDescription = "成品OK数量")]
        public int OkQty { get; set; }

        [SugarColumn(ColumnDescription = "成品NG数量")]
        public int NgQty { get; set; }

        [SugarColumn(ColumnDescription = "OP10模式")]
        public ProductMode OP10Mode { get; set; }

        [SugarColumn(ColumnDescription = "OP20模式")]
        public ProductMode OP20Mode { get; set; }

        [SugarColumn(ColumnDescription = "OP20模式")]
        public ProductMode OP30Mode { get; set; }

        [Navigate(typeof(Product_Task_Route_Config), nameof(Product_Task_Route_Config.TaskId), nameof(Product_Task_Route_Config.RouteId))]
        public List<Base_Route_Config> Routes { get; set; }
        }

    public enum Em_Task_Status
        {
        [Display(Name = "待产状态")]
        Await = 1,

        [Display(Name = "生产状态")]
        Run,

        [Display(Name = "清料状态")]
        Clear,

        [Display(Name = "暂停状态")]
        Stop,

        [Display(Name = "完工状态")]
        Finished
        }

    public enum ProductMode
        {
        正常工单 = 1,
        单机返修 = 2,
        联机返修 = 3
        }
    }