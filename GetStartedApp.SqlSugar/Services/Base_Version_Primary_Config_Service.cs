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
    public class Base_Version_Primary_Config_Service : BaseService<Base_Version_Primary_Config>,
        IBase_Version_Primary_Config_Service
    {
        private readonly ISqlSugarRepository<Base_Version_Primary_Config> _repository;
        public Base_Version_Primary_Config_Service(
            ISqlSugarRepository<Base_Version_Primary_Config> repository) : base(repository)
        {
            _repository = repository;
        }

        public List<Base_Version_Primary_Config> GetVersionPrimayPageList(ref long totalNum, int pageIndex, int pageItems = 50)
        {
            var total = 0;
            var page = _repository.Context.Queryable<Base_Version_Primary_Config>()
                .Includes(x => x.VersionSeconds)
                .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return page;
        }

        public List<Base_Version_Primary_Config> GetVersionPrimayTree()
        {
            return _repository.Context.Queryable<Base_Version_Primary_Config>()
               .Includes(x => x.VersionSeconds)
               .ToList();
        }

        public bool IsExist(string code, string name, int id)
        {
            return _repository.Context.Queryable<Base_Version_Primary_Config>().Any(x => (x.Code == code || x.Name == name) && x.Id != id);

        }
    }
}
