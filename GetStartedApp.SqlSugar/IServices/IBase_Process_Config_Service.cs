using GetStartedApp.SqlSugar.Tables;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface IBase_Process_Config_Service:IBaseService<Base_Process_Config>
    {
        ICollection<Base_Process_Config> GetProcesss();
        ICollection<Base_Process_Config> GetProcessPage(ref long totalNum, int pageIndex, int pageItems = 50);
        bool IsExist(string code, string name, int id);
    }
}