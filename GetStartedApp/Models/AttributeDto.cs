using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class AttributeDto : BaseDto
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
        private Em_Attribute_Type _AttributeType;
        public Em_Attribute_Type AttributeType
        {
            get { return _AttributeType; }
            set { SetProperty(ref _AttributeType, value); }
        }
        private Em_Value_Type _ValueType;
        public Em_Value_Type ValueType
        {
            get { return _ValueType; }
            set { SetProperty(ref _ValueType, value); }
        }
        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { SetProperty(ref _Value, value); }
        }
        private string _Target;
        public string Target
        {
            get { return _Target; }
            set { SetProperty(ref _Target, value); }
        }
        private int _SecondId;
        public int SecondId
        {
            get { return _SecondId; }
            set { SetProperty(ref _SecondId, value); }
        }
        private VersionSecondDto _VersionSecond;
        public VersionSecondDto VersionSecond
        {
            get { return _VersionSecond; }
            set { SetProperty(ref _VersionSecond, value); }
        }
        private int _StepId;
        public int StepId
        {
            get { return _StepId; }
            set { SetProperty(ref _StepId, value); }
        }
        private ProcessStepDto _Step;
        public ProcessStepDto Step
        {
            get { return _Step; }
            set { SetProperty(ref _Step, value); }
        }
    }
}
