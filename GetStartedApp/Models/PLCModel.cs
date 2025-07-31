using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class PLCModel : BindableBase
    {
        private string _FFileName;
        public string FFileName//文件名称 
        {
            get { return _FFileName; }
            set { SetProperty(ref _FFileName, value); }
        }

        private string _FIsConn = "未连接";
        public string FIsConn
        {
            get { return _FIsConn; }
            set
            {
                SetProperty(ref _FIsConn, value);
                if (value == "未连接")
                {
                    FColor = "Black";
                }
                else if (value == "已连接")
                {
                    FColor = "Lime";
                }
                else if (value == "连接失败")
                {
                    FColor = "Red";
                }
                else if (value == "后台连接中")
                {
                    FColor = "Orange";
                }
            }
        }

        private string _FColor = "Black";//默认黑色
        public string FColor
        {
            get { return _FColor; }
            set { SetProperty(ref _FColor, value); }
        }

        private string _FCpuType;
        public string FCpuType
        {
            get { return _FCpuType; }
            set { SetProperty(ref _FCpuType, value); }
        }

        private string _FPLCType;
        public string FPLCType
        {
            get { return _FPLCType; }
            set { SetProperty(ref _FPLCType, value); }
        }

        private string _FName;
        public string FName
        {
            get { return _FName; }
            set { SetProperty(ref _FName, value); }
        }

        private string _FAddr;
        public string FAddr
        {
            get { return _FAddr; }
            set { SetProperty(ref _FAddr, value); }
        }

        private string _FMark;
        public string FMark
        {
            get { return _FMark; }
            set { SetProperty(ref _FMark, value); }
        }
    }
}
