using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Recipe_ST_Parameter_Config")]
    public class Product_Recipe_ST_Parameter_Config : AutoIncrementEntity
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public int? RecipeSTId { get; set; }
    }
}