using GetStartedApp.Interface;
using GetStartedApp.Models;
using SmartCommunicationForExcel;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.ViewModels.PLC
{
    public class EventSiemensViewModel: ViewModelBase
    {
        public EventSiemensViewModel(ISiemensEvent siemensEvent)
        {
            siemensEvent.Instance(this);
        }



        private ObservableCollection<PlcEventModel> _ObPlcEvnet;

        public ObservableCollection<PlcEventModel> ObPlcEvnet
        {
            get { return _ObPlcEvnet ?? (_ObPlcEvnet = new ObservableCollection<PlcEventModel>()); }
            set { SetProperty(ref _ObPlcEvnet, value); }
        }

        private List<PlcEventModel> m_plcEvent = new List<PlcEventModel>();

        public List<PlcEventModel> M_plcEvent
        {
            get { return m_plcEvent; }
            set { SetProperty(ref m_plcEvent, value); }
        }

        public PlcEventParamModel DoEvent(PlcEventParamModel param)
        {
            //开始处理  跟新到界面 按照PLC分类 每一类放在一个地方 每一类最多存放 20个
            //InsertObPlcEvent(param);

            //通过事件名称 判断事件类型 名称后面带 _IN _OUT 表示进出站 不带的表示其他用途
            //**************************************************************

            List<MyData> keyValuePairs = new List<MyData>();
            string st = param.Params.GetMyData("WorkStage").ValueData.ToString();
            PlcEventParamModel toPlcParam = new PlcEventParamModel
            {
                PlcAddr = param.PlcAddr,
                PlcName = param.PlcName,
                EventName = param.EventName,
                StartTime = param.StartTime,
                Params = keyValuePairs
            };


            string eventNameType = ExtractEventNameType(param.EventName);

            try
            {
                switch (eventNameType)
                {
                    case "PartReq":
                        toPlcParam.Params = PartReq(param.Params);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            toPlcParam.RandomCode = param.RandomCode;

            return toPlcParam;
        }


        #region 事件实现
        private List<MyData> PartReq(List<MyData> @params)
        {
            List<MyData> toPcDatas = new List<MyData>();
            toPcDatas.SetValue_ResultAndMessageW(1, "请求产品SN成功!");

            return toPcDatas;
        }
        #endregion


        private string ExtractEventNameType(string eventName)
        {
            string[] cs = eventName.Split('_');
            if (cs.Length > 1)
            {
                return cs[cs.Length - 1];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
