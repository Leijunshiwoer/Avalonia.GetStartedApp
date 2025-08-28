using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.Models
{
    public class MqttReciveModel:BindableBase
    {
        private string _FTopic;
        public string FTopic
        {
            get { return _FTopic; }
            set { SetProperty(ref _FTopic, value); }
        }

        private string _FMessage;
        public string FMessage
        {
            get { return _FMessage; }
            set { SetProperty(ref _FMessage, value); }
        }

        private DateTime _FRecivedTime;
        public DateTime FRecivedTime
        {
            get { return _FRecivedTime; }
            set { SetProperty(ref _FRecivedTime, value); }
        }
    }
}
