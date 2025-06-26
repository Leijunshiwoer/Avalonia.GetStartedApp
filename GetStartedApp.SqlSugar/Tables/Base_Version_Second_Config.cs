using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName: "Base_Version_Second_Config")]
    public class Base_Version_Second_Config : AutoIncrementEntity
    {
        [SugarColumn(Length = 50)]
        public string Code { get; set; }
        [SugarColumn(Length = 200)]
        public string Name { get; set; }
        public int VersionPrimaryId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(VersionPrimaryId))]
        public Base_Version_Primary_Config VersionPrimary { get; set; }
        public int RouteId { get; set; }
        [Navigate(NavigateType.OneToOne, nameof(RouteId))]
        public Base_Route_Config Route { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Base_Version_Attribute_Config.SecondId))]
        public List<Base_Version_Attribute_Config> Attributes { get; set; }
        [Navigate(NavigateType.OneToMany, nameof(Product_Recipe_Config.SecondId))]
        public List<Product_Recipe_Config> Recipes { get; set; }
    }
}
