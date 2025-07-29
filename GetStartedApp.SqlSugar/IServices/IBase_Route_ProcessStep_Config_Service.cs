using GetStartedApp.SqlSugar.Tables;

namespace GetStartedApp.SqlSugar.IServices
{
    /// <summary>
    /// 工艺路线与工位之间多对多关系
    /// </summary>
    public interface IBase_Route_ProcessStep_Config_Service
    {
        /// <summary>
        /// 根据工艺路线ID 获取满足的工位Id列表
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        List<int> GetRouteProcessStepIdList(int routeId);
        void UpdateRouteProcessStep(int routeId, List<Base_Route_ProcessStep_Config> rsList);
    }
}