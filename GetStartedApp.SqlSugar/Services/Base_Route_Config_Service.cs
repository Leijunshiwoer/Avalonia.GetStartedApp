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
    public class Base_Route_Config_Service : BaseService<Base_Route_Config>, IBase_Route_Config_Service
    {
        private readonly ISqlSugarRepository<Base_Route_Config> _routeConfigRep;

        public Base_Route_Config_Service(
            ISqlSugarRepository<Base_Route_Config> routeConfigRep
            ) : base(routeConfigRep)
        {
            _routeConfigRep = routeConfigRep;
        }

        public ICollection<Base_Route_Config> GetRoutePage(ref long totalNum, int pageIndex, int pageItems = 50)
        {
            var total = 0;
            var pageProcess = _routeConfigRep.Context.Queryable<Base_Route_Config>()
               .Includes(x => x.VersionSecond)
               .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return pageProcess;
        }

        public ICollection<Base_Route_Config> GetRoutePageByTaskId(int taskId, ref long totalNum, int pageIndex, int pageItems = 50)
        {
            var total = 0;
            var pageProcess = _routeConfigRep.Context.Queryable<Base_Route_Config>()
               .Includes(x => x.VersionSecond)
               .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return pageProcess;
        }

        public bool IsExist(string code, string name, int id)
        {
            return _routeConfigRep.IsExists(x => (x.Code == code || x.Name == name) && x.Id != id);
        }

        public List<int> GetRouteIdList()
        {
            return _routeConfigRep.ToList().Select(x => x.Id).ToList();
        }

        public List<Base_Route_Config> GetPageAllNotContains(List<int> ids, ref long totalNum, int pageIndex, int pageItems = 50)
        {
            var total = 0;
            var page = _routeConfigRep.Context.Queryable<Base_Route_Config>()
                .Where(x => !ids.Contains(x.Id))
                .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return page;
        }

        public List<Base_Route_Config> GetPageAllContains(List<int> ids, ref long totalNum, int pageIndex, int pageItems = 50)
        {
            var total = 0;
            var page = _routeConfigRep.Context.Queryable<Base_Route_Config>()
                .Where(x => ids.Contains(x.Id))
                .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return page;
        }

        public int InsertOrUpdateReturnIdentity(Base_Route_Config route)
        {
            if (_routeConfigRep.IsExists(x => x.Id == route.Id))
            {
                //跟新
                if (_routeConfigRep.Context.Updateable(route).ExecuteCommand() > 0)
                    return route.Id;
                else
                    return 0;
            }
            else
            {
                //新增
                return _routeConfigRep.Context.Insertable(route).ExecuteReturnIdentity();
            }
        }

        public Base_Route_Config GetBySecondId(int secondId)
        {
            return _routeConfigRep.Context.Queryable<Base_Route_Config>()
                .Where(x => x.VersionSecondId == secondId)
                .Includes(
                x => x.Processs,
                p => p.Steps
                ).First();
        }

        public Base_Route_Config GetById(int id)
        {
            return _routeConfigRep.Context.Queryable<Base_Route_Config>()
                .Where(x => x.Id == id).First();
        }
    }
}
