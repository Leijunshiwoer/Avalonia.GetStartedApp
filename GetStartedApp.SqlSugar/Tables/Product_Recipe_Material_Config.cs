using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Recipe_Material_Config")]
    public class Product_Recipe_Material_Config : AutoIncrementEntity
    {
        public string Name { get; set; }
        [SugarColumn(IsNullable = true)]
        public string Value { get; set; }
        public int? RecipeConfigId { get; set; }
    }
}
