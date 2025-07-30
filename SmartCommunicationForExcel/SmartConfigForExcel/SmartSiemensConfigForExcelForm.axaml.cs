using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Interface;
using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

namespace SmartCommunicationForExcel.SmartConfigForExcel
{
    /// <summary>
    /// 西门子配置窗口的交互逻辑
    /// </summary>
    public partial class SmartSiemensConfigForExcelForm : Window
    {
        private ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance> _globalSiemensConfig;

        // 控件引用
     

        public SmartSiemensConfigForExcelForm()
        {
            InitializeComponent();
            // 初始化控件引用
            InitializeControls();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// 初始化所有需要操作的控件引用
        /// </summary>
        private void InitializeControls()
        {
            // CPU信息相关控件
           
        }

        public void SetModel(ISiemensGlobalConfig<SiemensEventIO, SiemensCpuInfo, SiemensEventInstance> globalConfig)
        {
            _globalSiemensConfig = globalConfig;
            LoadEventInstances();
            ReFresh();
        }

        /// <summary>
        /// 加载事件实例到下拉框
        /// </summary>
        private void LoadEventInstances()
        {
            if (comboBox1 == null) return;

            comboBox1.Items.Clear();
            int i = 0;
            foreach (SiemensEventInstance sei in _globalSiemensConfig.EventConfig)
            {
                comboBox1.Items.Add($"{++i},{sei.DisableEvent},{sei.EventClass},{sei.EventName}");
            }
        }

        private void ReFresh()
        {
            if (_globalSiemensConfig == null) return;

            // 更新CPU信息
            UpdateCpuInfo();

            // 更新Eap配置列表
            UpdateEapConfigList();

            // 更新PLC配置列表
            UpdatePlcConfigList();

            // 更新事件配置列表
            UpdateEventConfigList();
        }

        /// <summary>
        /// 更新CPU信息显示
        /// </summary>
        private void UpdateCpuInfo()
        {
            lbCpuType.Text = _globalSiemensConfig.CpuInfo.CpuType.ToString();
            lbPlcType.Text = _globalSiemensConfig.CpuInfo.PlcType.ToString();
            lbMark.Text = _globalSiemensConfig.CpuInfo.Mark;
            lbName.Text = _globalSiemensConfig.CpuInfo.Name;
            lbEapConfigBeginAddress.Text = _globalSiemensConfig.CpuInfo.EapConfigBeginAddress.ToString();
            lbEapConfigBeginOffset.Text = _globalSiemensConfig.CpuInfo.EapConfigBeginOffset.ToString();
            lbEapEventBeginAddress.Text = _globalSiemensConfig.CpuInfo.EapEventBeginAddress.ToString();
            lbEapEventBeginOffset.Text = _globalSiemensConfig.CpuInfo.EapEventBeginOffset.ToString();
            lbPlcConfigBeginAddress.Text = _globalSiemensConfig.CpuInfo.PlcConfigBeginAddress.ToString();
            lbPlcConfigBeginOffset.Text = _globalSiemensConfig.CpuInfo.PlcConfigBeginOffset.ToString();
            lbPlcEventBeginAddress.Text = _globalSiemensConfig.CpuInfo.PlcEventBeginAddress.ToString();
            lbPlcEventBeginOffset.Text = _globalSiemensConfig.CpuInfo.PlcEventBeginOffset.ToString();
            lbIP.Text = _globalSiemensConfig.CpuInfo.IP;
            lbPort.Text = _globalSiemensConfig.CpuInfo.Port.ToString();
            lbRack.Text = _globalSiemensConfig.CpuInfo.Rack.ToString();
            lbSlot.Text = _globalSiemensConfig.CpuInfo.Slot.ToString();
        }

        /// <summary>
        /// 更新Eap配置列表
        /// </summary>
        private void UpdateEapConfigList()
        {
            var configList = new List<MyConfig>();
            for (int i = 0; i < _globalSiemensConfig.EapConfig.Count; i++)
            {
                var config = new MyConfig(
                    (i + 1).ToString(),
                    _globalSiemensConfig.EapConfig[i].DataValueStr,
                    _globalSiemensConfig.EapConfig[i].Length.ToString(),
                    _globalSiemensConfig.EapConfig[i].MBAdr.ToString(),
                    _globalSiemensConfig.EapConfig[i].MEAdr.ToString(),
                    _globalSiemensConfig.EapConfig[i].DTType.ToString(),
                    _globalSiemensConfig.EapConfig[i].Mark,
                    _globalSiemensConfig.EapConfig[i].TagName,
                    _globalSiemensConfig.EapConfig[i].GlobalBeginAddress.ToString(),
                    _globalSiemensConfig.PlcConfig[i].GlobalBeginOffset.ToString(),
                    _globalSiemensConfig.EapConfig[i].GetMBAddressTag,
                    _globalSiemensConfig.EapConfig[i].GetMEAddressTag,
                    _globalSiemensConfig.EapConfig[i].DataFormat.ToString()
                );
                configList.Add(config);
            }
            lvEapConfig.ItemsSource = configList;
        }

        /// <summary>
        /// 更新PLC配置列表
        /// </summary>
        private void UpdatePlcConfigList()
        {
            var configList = new List<MyConfig>();
            for (int i = 0; i < _globalSiemensConfig.PlcConfig.Count; i++)
            {
                var config = new MyConfig(
                    (i + 1).ToString(),
                    _globalSiemensConfig.PlcConfig[i].DataValueStr,
                    _globalSiemensConfig.PlcConfig[i].Length.ToString(),
                    _globalSiemensConfig.PlcConfig[i].MBAdr.ToString(),
                    _globalSiemensConfig.PlcConfig[i].MEAdr.ToString(),
                    _globalSiemensConfig.PlcConfig[i].DTType.ToString(),
                    _globalSiemensConfig.PlcConfig[i].Mark,
                    _globalSiemensConfig.PlcConfig[i].TagName,
                    _globalSiemensConfig.PlcConfig[i].GlobalBeginAddress.ToString(),
                    _globalSiemensConfig.PlcConfig[i].GlobalBeginOffset.ToString(),
                    _globalSiemensConfig.PlcConfig[i].GetMBAddressTag,
                    _globalSiemensConfig.PlcConfig[i].GetMEAddressTag,
                    _globalSiemensConfig.PlcConfig[i].DataFormat.ToString()
                );
                configList.Add(config);
            }
            lvPlcConfig.ItemsSource = configList;
        }

        /// <summary>
        /// 更新事件配置列表
        /// </summary>
        private void UpdateEventConfigList()
        {
            // 处理ComboBox选中项（Avalonia中使用SelectedItem）
            if (comboBox1.SelectedItem == null) return;

            string selectedText = comboBox1.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedText)) return;

            string[] splits = selectedText.Trim().Split(',');
            if (splits.Length == 4 && int.TryParse(splits[0], out int idx))
            {
                idx--; // 转换为0基索引

                // 更新PC事件列表
                var pcEventList = new List<MyConfig>();
                for (int i = 0; i < _globalSiemensConfig.EventConfig[idx].ListOutput.Count; i++)
                {
                    var config = new MyConfig(
                        (i + 1).ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].DataValueStr,
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].Length.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].MBAdr.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].MEAdr.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].DTType.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].Mark,
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].TagName,
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].GlobalBeginAddress.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].GlobalBeginOffset.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].GetMBAddressTag,
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].GetMEAddressTag,
                        _globalSiemensConfig.EventConfig[idx].ListOutput[i].DataFormat.ToString()
                    );
                    pcEventList.Add(config);
                }
                lvEventConfigPC.ItemsSource = pcEventList;

                // 更新PLC事件列表
                var plcEventList = new List<MyConfig>();
                for (int i = 0; i < _globalSiemensConfig.EventConfig[idx].ListInput.Count; i++)
                {
                    var config = new MyConfig(
                        (i + 1).ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].DataValueStr,
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].Length.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].MBAdr.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].MEAdr.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].DTType.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].Mark,
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].TagName,
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].GlobalBeginAddress.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].GlobalBeginOffset.ToString(),
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].GetMBAddressTag,
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].GetMEAddressTag,
                        _globalSiemensConfig.EventConfig[idx].ListInput[i].DataFormat.ToString()
                    );
                    plcEventList.Add(config);
                }
                lvEventConfigPLC.ItemsSource = plcEventList;
            }
        }

        // 刷新按钮点击事件
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReFresh();
        }
    }

    // 数据绑定使用的配置类（如果在其他文件中已定义可删除此类）
 
}
