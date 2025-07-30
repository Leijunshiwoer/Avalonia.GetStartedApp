using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartCommunicationForExcel.SmartConfigForExcel
{
    /// <summary>
    /// Interaction logic for SmartSiemensConfigForExcelForm.xaml
    /// </summary>
    public partial class SmartSiemensConfigForExcelForm : Window
    {
        private ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance> _globalSiemensConfig;
        public SmartSiemensConfigForExcelForm()
        {
            InitializeComponent();
        }
        public void SetModel(ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance> globalConfig)
        {
            _globalSiemensConfig = globalConfig;
            int i = 0;
            foreach (SiemensEventInstance sei in _globalSiemensConfig.EventConfig)
            {
                comboBox1.Items.Add($"{++i},{sei.DisableEvent},{sei.EventClass},{sei.EventName}");
            }
        }
        private void ReFresh()
        {
            if (_globalSiemensConfig != null)
            {
                //CpuInfo
                lbCpuType.Text = _globalSiemensConfig.CpuInfo.CpuType.ToString();
                lbPlcType.Text = _globalSiemensConfig.CpuInfo.PlcType.ToString();
                lbMark.Text = _globalSiemensConfig.CpuInfo.Mark;
                lbName.Text = _globalSiemensConfig.CpuInfo.Name;
                lbEapConfigBeginAddress.Text = _globalSiemensConfig.CpuInfo.EapConfigBeginAddress + "";
                lbEapConfigBeginOffset.Text = _globalSiemensConfig.CpuInfo.EapConfigBeginOffset + "";
                lbEapEventBeginAddress.Text = _globalSiemensConfig.CpuInfo.EapEventBeginAddress + "";
                lbEapEventBeginOffset.Text = _globalSiemensConfig.CpuInfo.EapEventBeginOffset + "";
                lbPlcConfigBeginAddress.Text = _globalSiemensConfig.CpuInfo.PlcConfigBeginAddress + "";
                lbPlcConfigBeginOffset.Text = _globalSiemensConfig.CpuInfo.PlcConfigBeginOffset + "";
                lbPlcEventBeginAddress.Text = _globalSiemensConfig.CpuInfo.PlcEventBeginAddress + "";
                lbPlcEventBeginOffset.Text = _globalSiemensConfig.CpuInfo.PlcEventBeginOffset + "";
                lbIP.Text = _globalSiemensConfig.CpuInfo.IP;
                lbPort.Text = _globalSiemensConfig.CpuInfo.Port + "";
                lbRack.Text = _globalSiemensConfig.CpuInfo.Rack + "";
                lbSlot.Text = _globalSiemensConfig.CpuInfo.Slot + "";

                {
                    //EapConfig
                    lvEapConfig.Items.Clear();
                    for (int i = 0; i < _globalSiemensConfig.EapConfig.Count; i++)
                    {
                        MyConfig config = new MyConfig(
                            (i + 1) + "",
                            _globalSiemensConfig.EapConfig[i].DataValueStr,
                            _globalSiemensConfig.EapConfig[i].Length + "",
                            _globalSiemensConfig.EapConfig[i].MBAdr + "",
                            _globalSiemensConfig.EapConfig[i].MEAdr + "",
                            _globalSiemensConfig.EapConfig[i].DTType.ToString(),
                            _globalSiemensConfig.EapConfig[i].Mark,
                            _globalSiemensConfig.EapConfig[i].TagName,
                            _globalSiemensConfig.EapConfig[i].GlobalBeginAddress + "",
                            _globalSiemensConfig.PlcConfig[i].GlobalBeginOffset + "",
                            _globalSiemensConfig.EapConfig[i].GetMBAddressTag,
                            _globalSiemensConfig.EapConfig[i].GetMEAddressTag,
                            _globalSiemensConfig.EapConfig[i].DataFormat.ToString()
                            );

                        lvEapConfig.Items.Add(config);
                    }
                }

                {
                    //PLCConfig
                    lvPlcConfig.Items.Clear();
                    for (int i = 0; i < _globalSiemensConfig.PlcConfig.Count; i++)
                    {
                        MyConfig config = new MyConfig(
                           (i + 1) + "",
                           _globalSiemensConfig.PlcConfig[i].DataValueStr,
                           _globalSiemensConfig.PlcConfig[i].Length + "",
                           _globalSiemensConfig.PlcConfig[i].MBAdr + "",
                           _globalSiemensConfig.PlcConfig[i].MEAdr + "",
                           _globalSiemensConfig.PlcConfig[i].DTType.ToString(),
                           _globalSiemensConfig.PlcConfig[i].Mark,
                           _globalSiemensConfig.PlcConfig[i].TagName,
                           _globalSiemensConfig.PlcConfig[i].GlobalBeginAddress + "",
                           _globalSiemensConfig.PlcConfig[i].GlobalBeginOffset + "",
                           _globalSiemensConfig.PlcConfig[i].GetMBAddressTag,
                           _globalSiemensConfig.PlcConfig[i].GetMEAddressTag,
                           _globalSiemensConfig.PlcConfig[i].DataFormat.ToString()
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
                                    for (int i = 0; i < _globalSiemensConfig.EventConfig[idx].ListOutput.Count; i++)
                                    {
                                        MyConfig config = new MyConfig(
                                           (i + 1) + "",
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].DataValueStr,
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].Length + "",
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].MBAdr + "",
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].MEAdr + "",
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].DTType.ToString(),
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].Mark,
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].TagName,
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].GlobalBeginAddress + "",
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].GlobalBeginOffset + "",
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].GetMBAddressTag,
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].GetMEAddressTag,
                                           _globalSiemensConfig.EventConfig[idx].ListOutput[i].DataFormat.ToString()
                                           );

                                        lvEventConfigPC.Items.Add(config);
                                    }
                                }
                            }
                            {
                                int idx = Convert.ToInt32(splits[0]) - 1;
                                {
                                    lvEventConfigPLC.Items.Clear();
                                    for (int i = 0; i < _globalSiemensConfig.EventConfig[idx].ListInput.Count; i++)
                                    {
                                        MyConfig config = new MyConfig(
                                          (i + 1) + "",
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].DataValueStr,
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].Length + "",
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].MBAdr + "",
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].MEAdr + "",
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].DTType.ToString(),
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].Mark,
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].TagName,
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].GlobalBeginAddress + "",
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].GlobalBeginOffset + "",
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].GetMBAddressTag,
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].GetMEAddressTag,
                                          _globalSiemensConfig.EventConfig[idx].ListInput[i].DataFormat.ToString()
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
}
