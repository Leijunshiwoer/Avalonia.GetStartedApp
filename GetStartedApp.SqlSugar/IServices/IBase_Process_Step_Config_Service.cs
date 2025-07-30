using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface IBase_Process_Step_Config_Service: IBaseService<Base_Process_Step_Config>
    {

        int DeletedProcessStepById(int id);
        ICollection<Base_Process_Step_Config> GetProcessStepPage(ref long totalNum, int pageIndex, int pageItems = 50);
        ICollection<Base_Process_Step_Config> GetProcessStepsByInPLC();
        ICollection<Base_Process_Step_Config> GetProcessStepsById(int proId);
        bool IsExist(string code, string name, int id);
    }
}
