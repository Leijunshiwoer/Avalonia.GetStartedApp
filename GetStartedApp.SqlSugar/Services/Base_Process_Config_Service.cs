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
    public class Base_Process_Config_Service : BaseService<Base_Process_Config>, IBase_Process_Config_Service
    {
        private readonly ISqlSugarRepository<Base_Process_Config> _processConfigRep;

        public Base_Process_Config_Service(
            ISqlSugarRepository<Base_Process_Config> processConfigRep
            ) : base(processConfigRep)
        {
            _processConfigRep = processConfigRep;
        }

        public ICollection<Base_Process_Config> GetProcessPage(ref long totalNum, int pageIndex, int pageItems = 50)
        {
            var total = 0;
            var page = _processConfigRep.Context.Queryable<Base_Process_Config>()
                .OrderBy(x => x.Index)
                .OrderBy(x => x.RouteId)
                .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return page;
        }

        public ICollection<Base_Process_Config> GetProcesss()
        {
            return _processConfigRep.ToList()
                .OrderBy(x => x.Index)
                .OrderBy(x => x.RouteId)
                .ToList();
        }


        public bool IsExist(string code, string name, int id)
        {
            return _processConfigRep.Context.Queryable<Base_Process_Config>().Any(x => (x.Code == code || x.Name == name) && x.Id != id);
        }
    }
}
