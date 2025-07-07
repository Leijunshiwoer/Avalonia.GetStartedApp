using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface IBase_Version_Attribute_Config_Service : IBaseService<Base_Version_Attribute_Config>
    {
        List<Base_Version_Attribute_Config> GetPageAttributeBySecondIds(List<int> secondIds, ref int totalNum, int pageIndex, int pageItems = 50);
        List<Base_Version_Attribute_Config> GetAttributeBySecondId(int secondId);
        bool IsExist(string code, string name, int id);
        List<Base_Version_Attribute_Config> GetAttributeByRecipeNo(int recipeNo);

        int AddAttributes(List<Base_Version_Attribute_Config> list);
    }
}
