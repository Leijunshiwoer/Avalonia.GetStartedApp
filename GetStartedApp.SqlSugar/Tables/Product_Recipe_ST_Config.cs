using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Recipe_ST_Config")]
    public class Product_Recipe_ST_Config : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "ST名称")]
        public string Name { get; set; }
        public int? RecipeConfigId { get; set; }

        [Navigate(NavigateType.OneToMany, nameof(Product_Recipe_ST_Parameter_Config.RecipeSTId))]
        public List<Product_Recipe_ST_Parameter_Config> Parameters { get; set; }
    }
}
