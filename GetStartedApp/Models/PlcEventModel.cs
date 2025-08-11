using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class PlcEventModel : BindableBase
    {
        private string _RandomCode;
        public string RandomCode
        {
            get { return _RandomCode; }
            set { SetProperty(ref _RandomCode, value); }
        }
        private int _FId;

        public int FId
        {
            get { return _FId; }
            set { SetProperty(ref _FId, value); }
        }

        public int PlcEventLogId { get; set; }

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

        private string _FStationName;
        public string FStationName
        {
            get { return _FStationName; }
            set { SetProperty(ref _FStationName, value); }
        }


        private string _FEvent;
        public string FEvent
        {
            get { return _FEvent; }
            set { SetProperty(ref _FEvent, value); }
        }

        private DateTime _FStartTime;
        public DateTime FStartTime
        {
            get { return _FStartTime; }
            set { SetProperty(ref _FStartTime, value); }
        }

        private double _FDoTime;
        public double FDoTime
        {
            get { return _FDoTime; }
            set { SetProperty(ref _FDoTime, value); }
        }

        private string _FResult;
        public string FResult
        {
            get { return _FResult; }
            set
            {
                SetProperty(ref _FResult, value);
                if (value == "1")
                {
                    //符合要求
                    FResultColor = "Green";
                }
                else if (value == "2")
                {
                    //符合要求
                    FResultColor = "Blue";
                }
                else if (value == "3")
                {
                    FResultColor = "Red";
                }
                else
                {
                    FResultColor = "Black";
                }
            }
        }

        private string _FResultColor;
        public string FResultColor
        {
            get { return _FResultColor; }
            set { SetProperty(ref _FResultColor, value); }
        }

        private string _FResultMark;
        public string FResultMark
        {
            get { return _FResultMark; }
            set { SetProperty(ref _FResultMark, value); }
        }
    }
}
