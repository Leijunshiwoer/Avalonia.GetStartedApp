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
    public class Base_Process_Step_Config_Service : BaseService<Base_Process_Step_Config>, IBase_Process_Step_Config_Service
    {
        private readonly ISqlSugarRepository<Base_Process_Step_Config> _stepConfigRep;
        private readonly IBase_Route_ProcessStep_Config_Service _route_ProcessStep_Config_Service;

        public Base_Process_Step_Config_Service(
            ISqlSugarRepository<Base_Process_Step_Config> stepConfigRep,
            IBase_Route_ProcessStep_Config_Service route_ProcessStep_Config_Service
            ) : base(stepConfigRep)
        {
            _stepConfigRep = stepConfigRep;
            _route_ProcessStep_Config_Service = route_ProcessStep_Config_Service;
        }
        public int DeletedProcessStepById(int id)
        {
            return _stepConfigRep.Context.Updateable<Base_Process_Step_Config>(id)
                .SetColumns(x => x.IsDeleted == "Y")
                .ExecuteCommand();
        }

        public ICollection<Base_Process_Step_Config> GetProcessStepPage(ref long totalNum, int pageIndex, int pageItems = 50)
        {
            var total = 0;
            var page = _stepConfigRep.Context.Queryable<Base_Process_Step_Config>()
                .OrderBy(x => x.ProcessId)
                .OrderBy(x => x.Index)
                .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return page;
        }

        public ICollection<Base_Process_Step_Config> GetProcessStepsByInPLC()
        {
            return _stepConfigRep.ToList(x => x.IsInPLC == 1);
            // .OrderBy(x => x.ProcessId)
            // .OrderBy(x => x.Index)
            // .ToList();
        }

        public ICollection<Base_Process_Step_Config> GetProcessStepsById(int proId)
        {
            return _stepConfigRep.ToList()
                .Where(x => x.ProcessId == proId)
                .OrderBy(x => x.Index)
                .ToList();
        }

        public bool IsExist(string code, string name, int id)
        {
            return _stepConfigRep.Context.Queryable<Base_Process_Step_Config>().Any(x => (x.Code == code || x.Name == name) && x.Id != id);
        }

        public List<Base_Process_Step_Config> GetByRouteId(int routeId)
        {
            var ids = _route_ProcessStep_Config_Service.GetRouteProcessStepIdList(routeId);
            return _stepConfigRep.ToList(x => ids.Contains(x.Id));
        }
    }
}
