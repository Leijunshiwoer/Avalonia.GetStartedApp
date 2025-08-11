using GetStartedApp.Interface;
using GetStartedApp.Models;
using SmartCommunicationForExcel;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Threading; // Avalonia的Dispatcher命名空间

namespace GetStartedApp.ViewModels.PLC
{
    public class EventSiemensViewModel : ViewModelBase
    {
        // 直接初始化集合，确保始终使用同一个实例
        private readonly ObservableCollection<PlcEventModel> _obPlcEvnet = new ObservableCollection<PlcEventModel>();

        public ObservableCollection<PlcEventModel> ObPlcEvnet
        {
            get { return _obPlcEvnet; }
        }

        private List<PlcEventModel> m_plcEvent = new List<PlcEventModel>();

        public List<PlcEventModel> M_plcEvent
        {
            get { return m_plcEvent; }
            set { SetProperty(ref m_plcEvent, value); }
        }

        public EventSiemensViewModel(ISiemensEvent siemensEvent)
        {
            siemensEvent.Instance(this);
        }

        public PlcEventParamModel DoEvent(PlcEventParamModel param)
        {
            // 核心业务逻辑处理
            List<MyData> keyValuePairs = new List<MyData>();
            string st = param.Params.GetMyData("WorkStage").ValueData.ToString();
            PlcEventParamModel toPlcParam = new PlcEventParamModel
            {
                PlcAddr = param.PlcAddr,
                PlcName = param.PlcName,
                EventName = param.EventName,
                StartTime = param.StartTime,
                EventClass = st,
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

            // 添加事件记录，确保在UI线程执行
            AddEventRecord(toPlcParam);

            return toPlcParam;
        }


        #region 事件实现
        private List<MyData> PartReq(List<MyData> @params)
        {
            List<MyData> toPcDatas = new List<MyData>();
            toPcDatas.SetValue_ResultAndMessageW(1, "成功");
            return toPcDatas;
        }
        #endregion

        #region 私有方法
        private string ExtractEventNameType(string eventName)
        {
            string[] cs = eventName.Split('_');
            return cs.Length > 1 ? cs[cs.Length - 1] : string.Empty;
        }

        // 适配Avalonia的UI线程更新逻辑
        private void AddEventRecord(PlcEventParamModel param)
        {
            // 检查当前是否在UI线程
            if (Dispatcher.UIThread.CheckAccess())
            {
                // 在UI线程，直接更新
                UpdateCollection(param);
            }
            else
            {
                // 不在UI线程，调度到UI线程执行
                Dispatcher.UIThread.Post(() => UpdateCollection(param));
                // 如果需要等待执行完成，可以使用InvokeAsync:
                //await Dispatcher.UIThread.InvokeAsync(() => UpdateCollection(param));
            }
        }

        // 实际执行集合更新的方法
        private void UpdateCollection(PlcEventParamModel param)
        {
            var newRecord = new PlcEventModel
            {
                PlcEventLogId = param.PlcEventLogId,
                RandomCode = param.RandomCode,
                FName = param.PlcName,
                FStationName = param.EventClass,
                FAddr = param.PlcAddr,
                FEvent = param.EventName,
                FStartTime = param.StartTime,
                FDoTime = (DateTime.Now - param.StartTime).TotalMilliseconds,
                FResult = param.Params.FirstOrDefault(it => it.Key == "ResultCode")?.ValueData.ToString(),
                FResultMark = param.Params.FirstOrDefault(it => it.Key == "Message")?.ValueData.ToString(),
            };

            if (ObPlcEvnet.Count >= 100)
            {
                ObPlcEvnet.RemoveAt(ObPlcEvnet.Count - 1);
            }

            ObPlcEvnet.Insert(0, newRecord);
        }
        #endregion
    }
}
