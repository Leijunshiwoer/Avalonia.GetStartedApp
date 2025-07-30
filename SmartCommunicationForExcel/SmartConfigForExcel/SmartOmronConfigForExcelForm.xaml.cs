using SmartCommunicationForExcel.Implementation.Omron;
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
    /// Interaction logic for SmartOmronConfigForExcelForm.xaml
    /// </summary>
    public partial class SmartOmronConfigForExcelForm : Window
    {
        private IOmronGlobalConfig<OmronEventIO, OmronCpuInfo, OmronEventInstance> _globalOmronConfig;
        public SmartOmronConfigForExcelForm()
        {
            InitializeComponent();
        }
        public void SetModel(IOmronGlobalConfig<OmronEventIO, OmronCpuInfo, OmronEventInstance> globalConfig)
        {
            _globalOmronConfig = globalConfig;
            int i = 0;
            foreach (OmronEventInstance sei in _globalOmronConfig.EventConfig)
            {
                comboBox1.Items.Add($"{++i},{sei.DisableEvent},{sei.EventClass},{sei.EventName}");
            }
        }
        private void ReFresh()
        {
            if (_globalOmronConfig != null)
            {
                //CpuInfo
                lbCpuType.Text = _globalOmronConfig.CpuInfo.CpuType.ToString();
                lbMark.Text = _globalOmronConfig.CpuInfo.Mark;
                lbName.Text = _globalOmronConfig.CpuInfo.Name;
                lbEapConfigBeginAddress.Text = _globalOmronConfig.CpuInfo.EapConfigBeginAddress + "";
                lbEapConfigBeginOffset.Text = _globalOmronConfig.CpuInfo.EapConfigBeginOffset + "";
                lbEapEventBeginAddress.Text = _globalOmronConfig.CpuInfo.EapEventBeginAddress + "";
                lbEapEventBeginOffset.Text = _globalOmronConfig.CpuInfo.EapEventBeginOffset + "";
                lbPlcConfigBeginAddress.Text = _globalOmronConfig.CpuInfo.PlcConfigBeginAddress + "";
                lbPlcConfigBeginOffset.Text = _globalOmronConfig.CpuInfo.PlcConfigBeginOffset + "";
                lbPlcEventBeginAddress.Text = _globalOmronConfig.CpuInfo.PlcEventBeginAddress + "";
                lbPlcEventBeginOffset.Text = _globalOmronConfig.CpuInfo.PlcEventBeginOffset + "";
                lbIP.Text = _globalOmronConfig.CpuInfo.IP;
                lbPort.Text = _globalOmronConfig.CpuInfo.Port + "";
                lbSA1.Text = _globalOmronConfig.CpuInfo.SA1 + "";
                lbDA1.Text = _globalOmronConfig.CpuInfo.DA1 + "";
                lbDA2.Text = _globalOmronConfig.CpuInfo.DA2 + "";

                {
                    //EapConfig
                    lvEapConfig.Items.Clear();
                    for (int i = 0; i < _globalOmronConfig.EapConfig.Count; i++)
                    {
                        MyConfig config = new MyConfig(
                            (i + 1) + "",
                            _globalOmronConfig.EapConfig[i].DataValueStr,
                            _globalOmronConfig.EapConfig[i].Length + "",
                            _globalOmronConfig.EapConfig[i].MBAdr + "",
                            _globalOmronConfig.EapConfig[i].MEAdr + "",
                            _globalOmronConfig.EapConfig[i].DTType.ToString(),
                            _globalOmronConfig.EapConfig[i].Mark,
                            _globalOmronConfig.EapConfig[i].TagName,
                            _globalOmronConfig.EapConfig[i].GlobalBeginAddress + "",
                            _globalOmronConfig.PlcConfig[i].GlobalBeginOffset + "",
                            _globalOmronConfig.EapConfig[i].GetMBAddressTag,
                            _globalOmronConfig.EapConfig[i].GetMEAddressTag,
                            _globalOmronConfig.EapConfig[i].DataFormat.ToString()
                            );

                        lvEapConfig.Items.Add(config);
                    }
                }

                {
                    //PLCConfig
                    lvPlcConfig.Items.Clear();
                    for (int i = 0; i < _globalOmronConfig.PlcConfig.Count; i++)
                    {
                        MyConfig config = new MyConfig(
                           (i + 1) + "",
                           _globalOmronConfig.PlcConfig[i].DataValueStr,
                           _globalOmronConfig.PlcConfig[i].Length + "",
                           _globalOmronConfig.PlcConfig[i].MBAdr + "",
                           _globalOmronConfig.PlcConfig[i].MEAdr + "",
                           _globalOmronConfig.PlcConfig[i].DTType.ToString(),
                           _globalOmronConfig.PlcConfig[i].Mark,
                           _globalOmronConfig.PlcConfig[i].TagName,
                           _globalOmronConfig.PlcConfig[i].GlobalBeginAddress + "",
                           _globalOmronConfig.PlcConfig[i].GlobalBeginOffset + "",
                           _globalOmronConfig.PlcConfig[i].GetMBAddressTag,
                           _globalOmronConfig.PlcConfig[i].GetMEAddressTag,
                           _globalOmronConfig.PlcConfig[i].DataFormat.ToString()
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
                                    for (int i = 0; i < _globalOmronConfig.EventConfig[idx].ListOutput.Count; i++)
                                    {
                                        MyConfig config = new MyConfig(
                                           (i + 1) + "",
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].DataValueStr,
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].Length + "",
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].MBAdr + "",
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].MEAdr + "",
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].DTType.ToString(),
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].Mark,
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].TagName,
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].GlobalBeginAddress + "",
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].GlobalBeginOffset + "",
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].GetMBAddressTag,
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].GetMEAddressTag,
                                           _globalOmronConfig.EventConfig[idx].ListOutput[i].DataFormat.ToString()
                                           );

                                        lvEventConfigPC.Items.Add(config);
                                    }
                                }
                            }
                            {
                                int idx = Convert.ToInt32(splits[0]) - 1;
                                {
                                    lvEventConfigPLC.Items.Clear();
                                    for (int i = 0; i < _globalOmronConfig.EventConfig[idx].ListInput.Count; i++)
                                    {
                                        MyConfig config = new MyConfig(
                                          (i + 1) + "",
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].DataValueStr,
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].Length + "",
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].MBAdr + "",
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].MEAdr + "",
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].DTType.ToString(),
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].Mark,
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].TagName,
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].GlobalBeginAddress + "",
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].GlobalBeginOffset + "",
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].GetMBAddressTag,
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].GetMEAddressTag,
                                          _globalOmronConfig.EventConfig[idx].ListInput[i].DataFormat.ToString()
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