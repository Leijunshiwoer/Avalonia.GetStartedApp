using SmartCommunicationForExcel.Implementation.Mitsubishi;
using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartCommunicationForExcel.SmartConfigForExcel
{
    /// <summary>
    /// Interaction logic for SmartMisubishiConfigForExcelForm.xaml
    /// </summary>
    
    public partial class SmartMitsubishiConfigForExcelForm : Window
    {
        private IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance> _globalMitsubishiConfig;
        public SmartMitsubishiConfigForExcelForm()
        {
            InitializeComponent();
        }
        public void SetModel(IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance> globalConfig)
        {
            _globalMitsubishiConfig = globalConfig;
            int i = 0;
            foreach (MitsubishiEventInstance sei in _globalMitsubishiConfig.EventConfig)
            {
                comboBox1.Items.Add($"{++i},{sei.DisableEvent},{sei.EventClass},{sei.EventName}");
            }
        }
        private void ReFresh()
        {
            if (_globalMitsubishiConfig != null)
            {
                //CpuInfo
                lbCpuType.Text = _globalMitsubishiConfig.CpuInfo.CpuType.ToString();
                lbPlcType.Text = _globalMitsubishiConfig.CpuInfo.PlcType.ToString();
                lbMark.Text = _globalMitsubishiConfig.CpuInfo.Mark;
                lbName.Text = _globalMitsubishiConfig.CpuInfo.Name;
                lbEapConfigBeginAddress.Text = _globalMitsubishiConfig.CpuInfo.EapConfigBeginAddress + "";
                lbEapConfigBeginOffset.Text = _globalMitsubishiConfig.CpuInfo.EapConfigBeginOffset + "";
                lbEapEventBeginAddress.Text = _globalMitsubishiConfig.CpuInfo.EapEventBeginAddress + "";
                lbEapEventBeginOffset.Text = _globalMitsubishiConfig.CpuInfo.EapEventBeginOffset + "";
                lbPlcConfigBeginAddress.Text = _globalMitsubishiConfig.CpuInfo.PlcConfigBeginAddress + "";
                lbPlcConfigBeginOffset.Text = _globalMitsubishiConfig.CpuInfo.PlcConfigBeginOffset + "";
                lbPlcEventBeginAddress.Text = _globalMitsubishiConfig.CpuInfo.PlcEventBeginAddress + "";
                lbPlcEventBeginOffset.Text = _globalMitsubishiConfig.CpuInfo.PlcEventBeginOffset + "";
                lbIP.Text = _globalMitsubishiConfig.CpuInfo.IP;
                lbPort1.Text = _globalMitsubishiConfig.CpuInfo.Port1 + "";
                lbPort2.Text = _globalMitsubishiConfig.CpuInfo.Port2 + "";

                {
                    //EapConfig
                    lvEapConfig.Items.Clear();
                    for (int i = 0; i < _globalMitsubishiConfig.EapConfig.Count; i++)
                    {
                        MyConfig config = new MyConfig(
                            (i + 1) + "",
                            _globalMitsubishiConfig.EapConfig[i].DataValueStr,
                            _globalMitsubishiConfig.EapConfig[i].Length + "",
                            _globalMitsubishiConfig.EapConfig[i].MBAdr + "",
                            _globalMitsubishiConfig.EapConfig[i].MEAdr + "",
                            _globalMitsubishiConfig.EapConfig[i].DTType.ToString(),
                            _globalMitsubishiConfig.EapConfig[i].Mark,
                            _globalMitsubishiConfig.EapConfig[i].TagName,
                            _globalMitsubishiConfig.EapConfig[i].GlobalBeginAddress + "",
                            _globalMitsubishiConfig.PlcConfig[i].GlobalBeginOffset + "",
                            _globalMitsubishiConfig.EapConfig[i].GetMBAddressTag,
                            _globalMitsubishiConfig.EapConfig[i].GetMEAddressTag,
                            _globalMitsubishiConfig.EapConfig[i].DataFormat.ToString()
                            );

                        lvEapConfig.Items.Add(config);
                    }
                }

                {
                    //PLCConfig
                    lvPlcConfig.Items.Clear();
                    for (int i = 0; i < _globalMitsubishiConfig.PlcConfig.Count; i++)
                    {
                        MyConfig config = new MyConfig(
                           (i + 1) + "",
                           _globalMitsubishiConfig.PlcConfig[i].DataValueStr,
                           _globalMitsubishiConfig.PlcConfig[i].Length + "",
                           _globalMitsubishiConfig.PlcConfig[i].MBAdr + "",
                           _globalMitsubishiConfig.PlcConfig[i].MEAdr + "",
                           _globalMitsubishiConfig.PlcConfig[i].DTType.ToString(),
                           _globalMitsubishiConfig.PlcConfig[i].Mark,
                           _globalMitsubishiConfig.PlcConfig[i].TagName,
                           _globalMitsubishiConfig.PlcConfig[i].GlobalBeginAddress + "",
                           _globalMitsubishiConfig.PlcConfig[i].GlobalBeginOffset + "",
                           _globalMitsubishiConfig.PlcConfig[i].GetMBAddressTag,
                           _globalMitsubishiConfig.PlcConfig[i].GetMEAddressTag,
                           _globalMitsubishiConfig.PlcConfig[i].DataFormat.ToString()
                           );

                        lvPlcConfig.Items.Add(config);
                    }
                }

                {
                    //EventConfig
                    if (!string.IsNullOrEmpty(comboBox1.Text))
                    {
                        //解析第一个事件 
                        string[] splits = comboBox1.Text.Trim().Split(',');
                        if (splits.Length == 4)
                        {
                            {
                                int idx = Convert.ToInt32(splits[0]) - 1;
                                {
                                    lvEventConfigPC.Items.Clear();
                                    for (int i = 0; i < _globalMitsubishiConfig.EventConfig[idx].ListOutput.Count; i++)
                                    {
                                        MyConfig config = new MyConfig(
                                           (i + 1) + "",
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].DataValueStr,
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].Length + "",
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].MBAdr + "",
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].MEAdr + "",
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].DTType.ToString(),
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].Mark,
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].TagName,
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].GlobalBeginAddress + "",
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].GlobalBeginOffset + "",
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].GetMBAddressTag,
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].GetMEAddressTag,
                                           _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].DataFormat.ToString()
                                           );
                                       
                                        lvEventConfigPC.Items.Add(config);
                                    }
                                }
                            }
                            {
                                int idx = Convert.ToInt32(splits[0]) - 1;
                                {
                                    lvEventConfigPLC.Items.Clear();
                                    for (int i = 0; i < _globalMitsubishiConfig.EventConfig[idx].ListInput.Count; i++)
                                    {
                                        MyConfig config = new MyConfig(
                                          (i + 1) + "",
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].DataValueStr,
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].Length + "",
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].MBAdr + "",
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].MEAdr + "",
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].DTType.ToString(),
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].Mark,
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].TagName,
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].GlobalBeginAddress + "",
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].GlobalBeginOffset + "",
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].GetMBAddressTag,
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].GetMEAddressTag,
                                          _globalMitsubishiConfig.EventConfig[idx].ListInput[i].DataFormat.ToString()
                                          );
                                        lvEventConfigPLC.Items.Add(config);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        //刷新
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReFresh();
        }
    }

    public class MyConfig
    {
        public string No { get; set; }
        public string DateValue { get; set; }
        public string Length { get; set; }
        public string MBAdr { get; set; }
        public string MEAdr { get; set; }
        public string DTType { get; set; }
        public string Mark { get; set; }
        public string TagName { get; set; }
        public string GlobalBeginAddress { get; set; }
        public string GlobalBeginOffset { get; set; }
        public string GetMBAddressTag { get; set; }
        public string GetMEAddressTag { get; set; }
        public string DataFormat { get; set; }

        public MyConfig(string no, string dateValue, string length, string mBAdr, string mEAdr, string dTType, string mark, string tagName, string globalBeginAddress, string globalBeginOffset, string getMBAddressTag, string getMEAddressTag, string dataFormat)
        {
            No = no;
            DateValue = dateValue;
            Length = length;
            MBAdr = mBAdr;
            MEAdr = mEAdr;
            DTType = dTType;
            Mark = mark;
            TagName = tagName;
            GlobalBeginAddress = globalBeginAddress;
            GlobalBeginOffset = globalBeginOffset;
            GetMBAddressTag = getMBAddressTag;
            GetMEAddressTag = getMEAddressTag;
            DataFormat = dataFormat;
        }
    }
}
