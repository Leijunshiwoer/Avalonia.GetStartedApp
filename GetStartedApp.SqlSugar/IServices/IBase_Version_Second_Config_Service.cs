using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface IBase_Version_Second_Config_Service:IBaseService<Base_Version_Second_Config>
    {
        bool IsExist(string code, string name, int id);
        bool IsExistByParentId(int pId, string code, string name, int id);
        ICollection<Base_Version_Second_Config> GetVersionSeconds();
        ICollection<Base_Version_Second_Config> GetVersionSecondsByNotContainsRouteId(List<int> ids);
        ICollection<Base_Version_Second_Config> GetVersionSecondsByRouteId(int id);
        List<Base_Version_Primary_Config> GetGroupByPrimaryIdsByContainsSecondId(List<int> ids);
        int UpdateSecondRouteIdById(int secondId, int routeId);
        int UpdateSecondRouteIdByRouteId(int oldId, int newId);
    }
}
