using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Tables
{
    [SugarTable(tableName:"Base_Route_ProcessStep_Config")]
    public class Base_Route_ProcessStep_Config : AutoIncrementEntity
    {
        public int RouteId { get; set; }
        public int ProcessStepId { get; set; }

    }
}
