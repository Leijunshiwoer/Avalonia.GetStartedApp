using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class ProcessStepDto : BaseDto
    {
        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { SetProperty(ref _Code, value); }
        }
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        private string _UnitCode;
        public string UnitCode
        {
            get { return _UnitCode; }
            set { SetProperty(ref _UnitCode, value); }
        }
        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { SetProperty(ref _Index, value); }
        }

        private Em_Step_Type _StepType;
        public Em_Step_Type StepType
        {
            get { return _StepType; }
            set { SetProperty(ref _StepType, value); }
        }
        private int _ProcessId;
        public int ProcessId
        {
            get { return _ProcessId; }
            set { SetProperty(ref _ProcessId, value); }
        }
    }
}
