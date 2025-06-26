using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Product_Recipe_Config")]
    public class Product_Recipe_Config : AutoIncrementEntity
    {
        [SugarColumn(ColumnDescription = "配方编号")]
        public int RecipeNo { get; set; }
        [SugarColumn(ColumnDescription = "配方名称")]
        public string Name { get; set; }
        public int SecondId { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Product_Recipe_Material_Config.RecipeConfigId))]
        public List<Product_Recipe_Material_Config> Materials { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Product_Recipe_ST_Config.RecipeConfigId))]
        public List<Product_Recipe_ST_Config> STs { get; set; }
    }
}
