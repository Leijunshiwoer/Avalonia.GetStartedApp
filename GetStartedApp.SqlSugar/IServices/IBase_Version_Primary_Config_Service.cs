using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface IBase_Version_Primary_Config_Service:IBaseService<Base_Version_Primary_Config>
    {
        List<Base_Version_Primary_Config> GetVersionPrimayPageList(ref long totalNum, int pageIndex, int pageItems = 50);
        List<Base_Version_Primary_Config> GetVersionPrimayTree();
        bool IsExist(string code, string name, int id);
    }
}
