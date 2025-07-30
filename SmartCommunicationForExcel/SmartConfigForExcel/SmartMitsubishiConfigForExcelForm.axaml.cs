using SmartCommunicationForExcel.Implementation.Mitsubishi;
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
    /// �������ô��ڵĽ����߼�
    /// </summary>
    public partial class SmartMitsubishiConfigForExcelForm : Window
    {
        private IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance> _globalMitsubishiConfig;

        //// �ؼ�����
        //private TextBlock lbCpuType;
        //private TextBlock lbPlcType;
        //private TextBlock lbMark;
        //private TextBlock lbName;
        //private TextBlock lbEapConfigBeginAddress;
        //private TextBlock lbEapConfigBeginOffset;
        //private TextBlock lbEapEventBeginAddress;
        //private TextBlock lbEapEventBeginOffset;
        //private TextBlock lbPlcConfigBeginAddress;
        //private TextBlock lbPlcConfigBeginOffset;
        //private TextBlock lbPlcEventBeginAddress;
        //private TextBlock lbPlcEventBeginOffset;
        //private TextBlock lbIP;
        //private TextBlock lbPort1;
        //private TextBlock lbPort2;
        //private ComboBox comboBox1;
        //private DataGrid lvEapConfig;
        //private DataGrid lvPlcConfig;
        //private DataGrid lvEventConfigPC;
        //private DataGrid lvEventConfigPLC;

        public SmartMitsubishiConfigForExcelForm()
        {
            InitializeComponent();
            // ��ʼ���ؼ�����
            InitializeControls();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// ��ʼ��������Ҫ�����Ŀؼ�����
        /// </summary>
        private void InitializeControls()
        {
            //// CPU��Ϣ��ؿؼ�
            //lbCpuType = this.FindControl<TextBlock>("lbCpuType");
            //lbPlcType = this.FindControl<TextBlock>("lbPlcType");
            //lbMark = this.FindControl<TextBlock>("lbMark");
            //lbName = this.FindControl<TextBlock>("lbName");
            //lbEapConfigBeginAddress = this.FindControl<TextBlock>("lbEapConfigBeginAddress");
            //lbEapConfigBeginOffset = this.FindControl<TextBlock>("lbEapConfigBeginOffset");
            //lbEapEventBeginAddress = this.FindControl<TextBlock>("lbEapEventBeginAddress");
            //lbEapEventBeginOffset = this.FindControl<TextBlock>("lbEapEventBeginOffset");
            //lbPlcConfigBeginAddress = this.FindControl<TextBlock>("lbPlcConfigBeginAddress");
            //lbPlcConfigBeginOffset = this.FindControl<TextBlock>("lbPlcConfigBeginOffset");
            //lbPlcEventBeginAddress = this.FindControl<TextBlock>("lbPlcEventBeginAddress");
            //lbPlcEventBeginOffset = this.FindControl<TextBlock>("lbPlcEventBeginOffset");
            //lbIP = this.FindControl<TextBlock>("lbIP");
            //lbPort1 = this.FindControl<TextBlock>("lbPort1");
            //lbPort2 = this.FindControl<TextBlock>("lbPort2");

            //// ���ݱ��ؼ�
            //lvEapConfig = this.FindControl<DataGrid>("lvEapConfig");
            //lvPlcConfig = this.FindControl<DataGrid>("lvPlcConfig");
            //lvEventConfigPC = this.FindControl<DataGrid>("lvEventConfigPC");
            //lvEventConfigPLC = this.FindControl<DataGrid>("lvEventConfigPLC");

            //// ������ؼ�
            //comboBox1 = this.FindControl<ComboBox>("comboBox1");

            //// ��ʼ��DataGrid��ItemsSource
            //lvEapConfig.ItemsSource = new List<MyConfig>();
            //lvPlcConfig.ItemsSource = new List<MyConfig>();
            //lvEventConfigPC.ItemsSource = new List<MyConfig>();
            //lvEventConfigPLC.ItemsSource = new List<MyConfig>();
        }

        public void SetModel(IMitsubishiGlobalConfig<MitsubishiEventIO, MitsubishiCpuInfo, MitsubishiEventInstance> globalConfig)
        {
            _globalMitsubishiConfig = globalConfig;
            LoadEventInstances();
            ReFresh();
        }

        /// <summary>
        /// �����¼�ʵ����������
        /// </summary>
        private void LoadEventInstances()
        {
            if (comboBox1 == null) return;

            comboBox1.Items.Clear();
            int i = 0;
            foreach (MitsubishiEventInstance sei in _globalMitsubishiConfig.EventConfig)
            {
                comboBox1.Items.Add($"{++i},{sei.DisableEvent},{sei.EventClass},{sei.EventName}");
            }
        }

        private void ReFresh()
        {
            if (_globalMitsubishiConfig == null) return;

            // ����CPU��Ϣ
            UpdateCpuInfo();

            // ����Eap�����б�
            UpdateEapConfigList();

            // ����PLC�����б�
            UpdatePlcConfigList();

            // �����¼������б�
            UpdateEventConfigList();
        }

        /// <summary>
        /// ����CPU��Ϣ��ʾ
        /// </summary>
        private void UpdateCpuInfo()
        {
            lbCpuType.Text = _globalMitsubishiConfig.CpuInfo.CpuType.ToString();
            lbPlcType.Text = _globalMitsubishiConfig.CpuInfo.PlcType.ToString();
            lbMark.Text = _globalMitsubishiConfig.CpuInfo.Mark;
            lbName.Text = _globalMitsubishiConfig.CpuInfo.Name;
            lbEapConfigBeginAddress.Text = _globalMitsubishiConfig.CpuInfo.EapConfigBeginAddress.ToString();
            lbEapConfigBeginOffset.Text = _globalMitsubishiConfig.CpuInfo.EapConfigBeginOffset.ToString();
            lbEapEventBeginAddress.Text = _globalMitsubishiConfig.CpuInfo.EapEventBeginAddress.ToString();
            lbEapEventBeginOffset.Text = _globalMitsubishiConfig.CpuInfo.EapEventBeginOffset.ToString();
            lbPlcConfigBeginAddress.Text = _globalMitsubishiConfig.CpuInfo.PlcConfigBeginAddress.ToString();
            lbPlcConfigBeginOffset.Text = _globalMitsubishiConfig.CpuInfo.PlcConfigBeginOffset.ToString();
            lbPlcEventBeginAddress.Text = _globalMitsubishiConfig.CpuInfo.PlcEventBeginAddress.ToString();
            lbPlcEventBeginOffset.Text = _globalMitsubishiConfig.CpuInfo.PlcEventBeginOffset.ToString();
            lbIP.Text = _globalMitsubishiConfig.CpuInfo.IP;
            lbPort1.Text = _globalMitsubishiConfig.CpuInfo.Port1.ToString();
            lbPort2.Text = _globalMitsubishiConfig.CpuInfo.Port2.ToString();
        }

        /// <summary>
        /// ����Eap�����б�
        /// </summary>
        private void UpdateEapConfigList()
        {
            var configList = new List<MyConfig>();
            for (int i = 0; i < _globalMitsubishiConfig.EapConfig.Count; i++)
            {
                var config = new MyConfig(
                    (i + 1).ToString(),
                    _globalMitsubishiConfig.EapConfig[i].DataValueStr,
                    _globalMitsubishiConfig.EapConfig[i].Length.ToString(),
                    _globalMitsubishiConfig.EapConfig[i].MBAdr.ToString(),
                    _globalMitsubishiConfig.EapConfig[i].MEAdr.ToString(),
                    _globalMitsubishiConfig.EapConfig[i].DTType.ToString(),
                    _globalMitsubishiConfig.EapConfig[i].Mark,
                    _globalMitsubishiConfig.EapConfig[i].TagName,
                    _globalMitsubishiConfig.EapConfig[i].GlobalBeginAddress.ToString(),
                    _globalMitsubishiConfig.PlcConfig[i].GlobalBeginOffset.ToString(),
                    _globalMitsubishiConfig.EapConfig[i].GetMBAddressTag,
                    _globalMitsubishiConfig.EapConfig[i].GetMEAddressTag,
                    _globalMitsubishiConfig.EapConfig[i].DataFormat.ToString()
                );
                configList.Add(config);
            }
            lvEapConfig.ItemsSource = configList;
        }

        /// <summary>
        /// ����PLC�����б�
        /// </summary>
        private void UpdatePlcConfigList()
        {
            var configList = new List<MyConfig>();
            for (int i = 0; i < _globalMitsubishiConfig.PlcConfig.Count; i++)
            {
                var config = new MyConfig(
                    (i + 1).ToString(),
                    _globalMitsubishiConfig.PlcConfig[i].DataValueStr,
                    _globalMitsubishiConfig.PlcConfig[i].Length.ToString(),
                    _globalMitsubishiConfig.PlcConfig[i].MBAdr.ToString(),
                    _globalMitsubishiConfig.PlcConfig[i].MEAdr.ToString(),
                    _globalMitsubishiConfig.PlcConfig[i].DTType.ToString(),
                    _globalMitsubishiConfig.PlcConfig[i].Mark,
                    _globalMitsubishiConfig.PlcConfig[i].TagName,
                    _globalMitsubishiConfig.PlcConfig[i].GlobalBeginAddress.ToString(),
                    _globalMitsubishiConfig.PlcConfig[i].GlobalBeginOffset.ToString(),
                    _globalMitsubishiConfig.PlcConfig[i].GetMBAddressTag,
                    _globalMitsubishiConfig.PlcConfig[i].GetMEAddressTag,
                    _globalMitsubishiConfig.PlcConfig[i].DataFormat.ToString()
                );
                configList.Add(config);
            }
            lvPlcConfig.ItemsSource = configList;
        }

        /// <summary>
        /// �����¼������б�
        /// </summary>
        private void UpdateEventConfigList()
        {
            // ����ComboBoxѡ���Avalonia��ʹ��SelectedItem��
            if (comboBox1.SelectedItem == null) return;

            string selectedText = comboBox1.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedText)) return;

            string[] splits = selectedText.Trim().Split(',');
            if (splits.Length == 4 && int.TryParse(splits[0], out int idx))
            {
                idx--; // ת��Ϊ0������

                // ����PC�¼��б�
                var pcEventList = new List<MyConfig>();
                for (int i = 0; i < _globalMitsubishiConfig.EventConfig[idx].ListOutput.Count; i++)
                {
                    var config = new MyConfig(
                        (i + 1).ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].DataValueStr,
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].Length.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].MBAdr.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].MEAdr.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].DTType.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].Mark,
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].TagName,
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].GlobalBeginAddress.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].GlobalBeginOffset.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].GetMBAddressTag,
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].GetMEAddressTag,
                        _globalMitsubishiConfig.EventConfig[idx].ListOutput[i].DataFormat.ToString()
                    );
                    pcEventList.Add(config);
                }
                lvEventConfigPC.ItemsSource = pcEventList;

                // ����PLC�¼��б�
                var plcEventList = new List<MyConfig>();
                for (int i = 0; i < _globalMitsubishiConfig.EventConfig[idx].ListInput.Count; i++)
                {
                    var config = new MyConfig(
                        (i + 1).ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].DataValueStr,
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].Length.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].MBAdr.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].MEAdr.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].DTType.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].Mark,
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].TagName,
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].GlobalBeginAddress.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].GlobalBeginOffset.ToString(),
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].GetMBAddressTag,
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].GetMEAddressTag,
                        _globalMitsubishiConfig.EventConfig[idx].ListInput[i].DataFormat.ToString()
                    );
                    plcEventList.Add(config);
                }
                lvEventConfigPLC.ItemsSource = plcEventList;
            }
        }

        // ˢ�°�ť����¼�
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReFresh();
        }
    }

   
}
