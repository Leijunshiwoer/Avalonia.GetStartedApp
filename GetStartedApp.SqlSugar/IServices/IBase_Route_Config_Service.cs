using GetStartedApp.SqlSugar.Tables;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface IBase_Route_Config_Service : IBaseService<Base_Route_Config>
    {
        ICollection<Base_Route_Config> GetRoutePage(ref long totalNum, int pageIndex, int pageItems = 50);

        ICollection<Base_Route_Config> GetRoutePageByTaskId(int taskId, ref long totalNum, int pageIndex, int pageItems = 50);

        bool IsExist(string code, string name, int id);

        List<int> GetRouteIdList();

        List<Base_Route_Config> GetPageAllNotContains(List<int> ids, ref long totalNum, int pageIndex, int pageItems = 50);

        List<Base_Route_Config> GetPageAllContains(List<int> ids, ref long totalNum, int pageIndex, int pageItems = 50);

        int InsertOrUpdateReturnIdentity(Base_Route_Config route);

        Base_Route_Config GetBySecondId(int secondId);

        Base_Route_Config GetById(int id);
    }
}