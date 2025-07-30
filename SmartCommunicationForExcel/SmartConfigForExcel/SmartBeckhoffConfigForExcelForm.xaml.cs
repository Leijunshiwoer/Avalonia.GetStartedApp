using SmartCommunicationForExcel.Implementation.Beckhoff;
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
using System.Windows.Shapes;

namespace SmartCommunicationForExcel.SmartConfigForExcel
{
    /// <summary>
    /// SmartBeckhoffConfigForExcelForm.xaml 的交互逻辑
    /// </summary>
    public partial class SmartBeckhoffConfigForExcelForm : Window
    {
        private IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance> _globalBeckhoffConfig;
        public SmartBeckhoffConfigForExcelForm()
        {
            InitializeComponent();
        }
        public void SetModel(IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance> globalConfig)
        {
            _globalBeckhoffConfig = globalConfig;
            int i = 0;
            foreach (BeckhoffEventInstance sei in _globalBeckhoffConfig.EventConfig)
            {
                comboBox1.Items.Add($"{++i},{sei.DisableEvent},{sei.EventClass},{sei.PC_LabelName},{sei.PLC_LabelName}--{sei.EventName}");
            }
        }
        private void ReFresh()
        {
            if (_globalBeckhoffConfig != null)
            {
                //CpuInfo
                lbCpuType.Text = _globalBeckhoffConfig.CpuInfo.CpuType.ToString();
                lbMark.Text = _globalBeckhoffConfig.CpuInfo.Mark;
                lbName.Text = _globalBeckhoffConfig.CpuInfo.Name;
                lbEapLabelName.Text = _globalBeckhoffConfig.CpuInfo.EapConfigLabel;
                lbEapConfigBeginAddress.Text = _globalBeckhoffConfig.CpuInfo.EapConfigBeginAddress + "";
                lbEapConfigBeginOffset.Text = _globalBeckhoffConfig.CpuInfo.EapConfigBeginOffset + "";
                lbEapEventBeginAddress.Text = _globalBeckhoffConfig.CpuInfo.EapEventBeginAddress + "";
                lbEapEventBeginOffset.Text = _globalBeckhoffConfig.CpuInfo.EapEventBeginOffset + "";
                lbPlcLabelName.Text = _globalBeckhoffConfig.CpuInfo.PlcConfigLabel;
                lbPlcConfigBeginAddress.Text = _globalBeckhoffConfig.CpuInfo.PlcConfigBeginAddress + "";
                lbPlcConfigBeginOffset.Text = _globalBeckhoffConfig.CpuInfo.PlcConfigBeginOffset + "";
                lbPlcEventBeginAddress.Text = _globalBeckhoffConfig.CpuInfo.PlcEventBeginAddress + "";
                lbPlcEventBeginOffset.Text = _globalBeckhoffConfig.CpuInfo.PlcEventBeginOffset + "";
                lbIP.Text = _globalBeckhoffConfig.CpuInfo.IP;
                lbPort.Text = _globalBeckhoffConfig.CpuInfo.Port + "";
                lbTargetNetId.Text = _globalBeckhoffConfig.CpuInfo.TargetNetId;
                lbSenderNetId.Text = _globalBeckhoffConfig.CpuInfo.SenderNetId;
                {
                    //EapConfig
                    lvEapConfig.Items.Clear();
                    for (int i = 0; i < _globalBeckhoffConfig.EapConfig.Count; i++)
                    {
                        MyConfig config = new MyConfig(
                            (i + 1) + "",
                            _globalBeckhoffConfig.EapConfig[i].DataValueStr,
                            _globalBeckhoffConfig.EapConfig[i].Length + "",
                            _globalBeckhoffConfig.EapConfig[i].MBAdr + "",
                            _globalBeckhoffConfig.EapConfig[i].MEAdr + "",
                            _globalBeckhoffConfig.EapConfig[i].DTType.ToString(),
                            _globalBeckhoffConfig.EapConfig[i].Mark,
                            _globalBeckhoffConfig.EapConfig[i].TagName,
                            _globalBeckhoffConfig.EapConfig[i].GlobalBeginAddress + "",
                            _globalBeckhoffConfig.PlcConfig[i].GlobalBeginOffset + "",
                            _globalBeckhoffConfig.EapConfig[i].GetMBAddressTag,
                            _globalBeckhoffConfig.EapConfig[i].GetMEAddressTag,
                            _globalBeckhoffConfig.EapConfig[i].DataFormat.ToString()
                            );

                        lvEapConfig.Items.Add(config);
                    }
                }

                {
                    //PLCConfig
                    lvPlcConfig.Items.Clear();
                    for (int i = 0; i < _globalBeckhoffConfig.PlcConfig.Count; i++)
                    {
                        MyConfig config = new MyConfig(
                           (i + 1) + "",
                           _globalBeckhoffConfig.PlcConfig[i].DataValueStr,
                           _globalBeckhoffConfig.PlcConfig[i].Length + "",
                           _globalBeckhoffConfig.PlcConfig[i].MBAdr + "",
                           _globalBeckhoffConfig.PlcConfig[i].MEAdr + "",
                           _globalBeckhoffConfig.PlcConfig[i].DTType.ToString(),
                           _globalBeckhoffConfig.PlcConfig[i].Mark,
                           _globalBeckhoffConfig.PlcConfig[i].TagName,
                           _globalBeckhoffConfig.PlcConfig[i].GlobalBeginAddress + "",
                           _globalBeckhoffConfig.PlcConfig[i].GlobalBeginOffset + "",
                           _globalBeckhoffConfig.PlcConfig[i].GetMBAddressTag,
                           _globalBeckhoffConfig.PlcConfig[i].GetMEAddressTag,
                           _globalBeckhoffConfig.PlcConfig[i].DataFormat.ToString()
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
                        if (splits.Length == 5)
                        {
                            {
                                int idx = Convert.ToInt32(splits[0]) - 1;
                                {
                                    lvEventConfigPC.Items.Clear();
                                    for (int i = 0; i < _globalBeckhoffConfig.EventConfig[idx].ListOutput.Count; i++)
                                    {
                                        MyConfig config = new MyConfig(
                                           (i + 1) + "",
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].DataValueStr,
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].Length + "",
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].MBAdr + "",
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].MEAdr + "",
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].DTType.ToString(),
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].Mark,
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].TagName,
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].GlobalBeginAddress + "",
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].GlobalBeginOffset + "",
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].GetMBAddressTag,
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].GetMEAddressTag,
                                           _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].DataFormat.ToString()
                                           );

                                        lvEventConfigPC.Items.Add(config);
                                    }
                                }
                            }
                            {
                                int idx = Convert.ToInt32(splits[0]) - 1;
                                {
                                    lvEventConfigPLC.Items.Clear();
                                    for (int i = 0; i < _globalBeckhoffConfig.EventConfig[idx].ListInput.Count; i++)
                                    {
                                        MyConfig config = new MyConfig(
                                          (i + 1) + "",
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].DataValueStr,
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].Length + "",
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].MBAdr + "",
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].MEAdr + "",
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].DTType.ToString(),
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].Mark,
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].TagName,
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].GlobalBeginAddress + "",
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].GlobalBeginOffset + "",
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].GetMBAddressTag,
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].GetMEAddressTag,
                                          _globalBeckhoffConfig.EventConfig[idx].ListInput[i].DataFormat.ToString()
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
