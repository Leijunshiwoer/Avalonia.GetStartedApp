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
    public class Base_Route_ProcessStep_Config_Service : IBase_Route_ProcessStep_Config_Service
    {
        private readonly ISqlSugarRepository<Base_Route_ProcessStep_Config> _routeStepConfigRep;

        public Base_Route_ProcessStep_Config_Service(
            ISqlSugarRepository<Base_Route_ProcessStep_Config> routeStepConfigRep
            )
        {
            _routeStepConfigRep = routeStepConfigRep;
        }
        public List<int> GetRouteProcessStepIdList(int routeId)
        {
            return _routeStepConfigRep.ToList(it => it.RouteId == routeId).Select(x => x.ProcessStepId).ToList();
        }
        public void UpdateRouteProcessStep(int routeId, List<Base_Route_ProcessStep_Config> rsList)
        {
            try
            {
                _routeStepConfigRep.Ado.BeginTran();
                //删除原工艺路线与工位关系
                _routeStepConfigRep.Delete(x => x.RouteId == routeId);
                //绑定新关系
                _routeStepConfigRep.Insert(rsList);

                _routeStepConfigRep.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _routeStepConfigRep.Ado.RollbackTran();
            }
        }
    }
}
