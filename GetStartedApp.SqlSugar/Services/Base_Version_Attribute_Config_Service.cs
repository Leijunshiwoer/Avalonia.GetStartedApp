using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Repositories;
using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Services
{
    public class Base_Version_Attribute_Config_Service : BaseService<Base_Version_Attribute_Config>, IBase_Version_Attribute_Config_Service
    {
        private readonly ISqlSugarRepository<Base_Version_Attribute_Config> _attributeRep;

        public Base_Version_Attribute_Config_Service(
            ISqlSugarRepository<Base_Version_Attribute_Config> attributeRep
            ) : base(attributeRep)
        {
            _attributeRep = attributeRep;
        }

        public List<Base_Version_Attribute_Config> GetPageAttributeBySecondIds(List<int> secondIds, ref int totalNum, int pageIndex, int pageItems = 50)
        {
            int total = 0;
            var page = _attributeRep.Context.Queryable<Base_Version_Attribute_Config>()
                .Where(x => secondIds.Contains(x.SecondId))
                .Includes(x => x.VersionSecond)
                .Includes(x => x.Step)
                .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            var result = page.Single(x => x.Code == "EWMLX");
            if (result.Value == "类型Ⅰ")
            {
                page.RemoveAll(x => x.Code.Contains("EWM-2") || x.Code.Contains("EWM-3"));
            }
            else if (result.Value == "类型Ⅱ")
            {
                page.RemoveAll(x => x.Code.Contains("EWM-1") || x.Code.Contains("EWM-3"));
            }
            else if (result.Value == "类型Ⅲ")
            {
                page.RemoveAll(x => x.Code.Contains("EWM-2") || x.Code.Contains("EWM-1"));
            }
            else if (result.Value == "类型Ⅳ")
            {
                page.RemoveAll(x => x.Code.Contains("EWM-1") || x.Code.Contains("EWM-3") || x.Code.Contains("EWM-2-GSDM")
                    );
            }
            else if (result.Value == "类型Ⅴ")
            {
                page.RemoveAll(x => x.Code.Contains("EWM-1") || x.Code.Contains("EWM-3"));
            }
            page.RemoveAll(x => x.Code == "YWM-CPPH");
            return page;
        }

        public List<Base_Version_Attribute_Config> GetAttributeBySecondId(int secondId)
        {
            var list = _attributeRep.Context.Queryable<Base_Version_Attribute_Config>()
                .Where(x => x.SecondId == secondId).ToList();
            return list;
        }

        public bool IsExist(string code, string name, int id)
        {
            return _attributeRep.Context.Queryable<Base_Version_Attribute_Config>().Any(x => (x.Code == code && x.Name == name) && x.Id != id);
        }

        public List<Base_Version_Attribute_Config> GetAttributeByRecipeNo(int recipeNo)
        {
            return _attributeRep.Context.Queryable<Base_Version_Attribute_Config>().Where(x => x.Code == "PFSY").Where(x => x.Value == recipeNo.ToString()).ToList();
        }

        public int AddAttributes(List<Base_Version_Attribute_Config> list)
        {
            return _attributeRep.Context.Insertable(list).ExecuteCommand();
        }
    }
}
