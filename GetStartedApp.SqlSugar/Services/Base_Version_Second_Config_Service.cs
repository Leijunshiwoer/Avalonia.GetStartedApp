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
    public class Base_Version_Second_Config_Service : BaseService<Base_Version_Second_Config>, IBase_Version_Second_Config_Service
    {
        private readonly ISqlSugarRepository<Base_Version_Second_Config> _versionSecondConfigRep;
        private readonly ISqlSugarRepository<Base_Version_Primary_Config> _versionPrimaryConfigRep;

        public Base_Version_Second_Config_Service(
            ISqlSugarRepository<Base_Version_Second_Config> versionSecondConfigRep,
            ISqlSugarRepository<Base_Version_Primary_Config> versionPrimaryConfigRep
            ) : base(versionSecondConfigRep)
        {
            _versionSecondConfigRep = versionSecondConfigRep;
            _versionPrimaryConfigRep = versionPrimaryConfigRep;
        }

        public bool IsExist(string code, string name, int id)
        {
            return _versionSecondConfigRep.IsExists(x => (x.Code == code || x.Name == name) && x.Id != id);
        }

        public bool IsExistByParentId(int pId, string code, string name, int id)
        {
            return _versionSecondConfigRep.IsExists(x => x.Code == code && x.Id != id && x.VersionPrimaryId == pId);
        }

        public ICollection<Base_Version_Second_Config> GetVersionSeconds()
        {
            return _versionSecondConfigRep.Context.Queryable<Base_Version_Second_Config>()
                .Includes(x => x.VersionPrimary)
                .ToList();
        }

        public ICollection<Base_Version_Second_Config> GetVersionSecondsByNotContainsRouteId(List<int> ids)
        {
            return _versionSecondConfigRep.Context.Queryable<Base_Version_Second_Config>()
                .Where(x => !ids.Contains(x.RouteId))
                .Includes(x => x.VersionPrimary)
                .ToList();
        }
        public ICollection<Base_Version_Second_Config> GetVersionSecondsByRouteId(int id)
        {
            return _versionSecondConfigRep.Context.Queryable<Base_Version_Second_Config>()
                .Where(x => x.RouteId == id)
                .Includes(x => x.VersionPrimary)
                .ToList();
        }

        public List<Base_Version_Primary_Config> GetGroupByPrimaryIdsByContainsSecondId(List<int> ids)
        {
            var pIds = _versionSecondConfigRep
               .ToList(x => ids.Contains(x.Id))
               .OrderBy(x => x.VersionPrimaryId)
               .ToList()
               .Select(x => x.VersionPrimaryId)
               .ToList();
            return _versionPrimaryConfigRep.ToList(x => pIds.Contains(x.Id));
        }

        public int UpdateSecondRouteIdById(int secondId, int routeId)
        {
            return _versionSecondConfigRep.Context.Updateable<Base_Version_Second_Config>()
                .Where(x => x.Id == secondId)
                .SetColumns(x => x.RouteId == routeId)
                .ExecuteCommand();
        }

        public int UpdateSecondRouteIdByRouteId(int oldId, int newId)
        {
            return _versionSecondConfigRep.Context.Updateable<Base_Version_Second_Config>()
               .Where(x => x.RouteId == oldId)
               .SetColumns(x => x.RouteId == newId)
               .ExecuteCommand();
        }

    }
}
