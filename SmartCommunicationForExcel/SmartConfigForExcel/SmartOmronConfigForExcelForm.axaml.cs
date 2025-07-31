using SmartCommunicationForExcel.Implementation.Omron;
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
    /// ŷķ�����ô��ڵĽ����߼�
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
            foreach (OmronEventInstance sei in _globalOmronConfig.EventConfig)
            {
                comboBox1.Items.Add($"{++i},{sei.DisableEvent},{sei.EventClass},{sei.EventName}");
            }
        }

        private void ReFresh()
        {
            if (_globalOmronConfig == null) return;

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
            lbCpuType.Text = _globalOmronConfig.CpuInfo.CpuType.ToString();
            //lbPlcType.Text = _globalOmronConfig.CpuInfo.PlcType.ToString();
            lbMark.Text = _globalOmronConfig.CpuInfo.Mark;
            lbName.Text = _globalOmronConfig.CpuInfo.Name;
            lbEapConfigBeginAddress.Text = _globalOmronConfig.CpuInfo.EapConfigBeginAddress.ToString();
            lbEapConfigBeginOffset.Text = _globalOmronConfig.CpuInfo.EapConfigBeginOffset.ToString();
            lbEapEventBeginAddress.Text = _globalOmronConfig.CpuInfo.EapEventBeginAddress.ToString();
            lbEapEventBeginOffset.Text = _globalOmronConfig.CpuInfo.EapEventBeginOffset.ToString();
            lbPlcConfigBeginAddress.Text = _globalOmronConfig.CpuInfo.PlcConfigBeginAddress.ToString();
            lbPlcConfigBeginOffset.Text = _globalOmronConfig.CpuInfo.PlcConfigBeginOffset.ToString();
            lbPlcEventBeginAddress.Text = _globalOmronConfig.CpuInfo.PlcEventBeginAddress.ToString();
            lbPlcEventBeginOffset.Text = _globalOmronConfig.CpuInfo.PlcEventBeginOffset.ToString();
            lbIP.Text = _globalOmronConfig.CpuInfo.IP;
            lbPort.Text = _globalOmronConfig.CpuInfo.Port.ToString();
            lbSA1.Text = _globalOmronConfig.CpuInfo.SA1.ToString();
            lbDA1.Text = _globalOmronConfig.CpuInfo.DA1.ToString();
            lbDA2.Text = _globalOmronConfig.CpuInfo.DA2.ToString();
        }

        /// <summary>
        /// ����Eap�����б�
        /// </summary>
        private void UpdateEapConfigList()
        {
            var configList = new List<MyConfig>();
            for (int i = 0; i < _globalOmronConfig.EapConfig.Count; i++)
            {
                var config = new MyConfig(
                    (i + 1).ToString(),
                    _globalOmronConfig.EapConfig[i].DataValueStr,
                    _globalOmronConfig.EapConfig[i].Length.ToString(),
                    _globalOmronConfig.EapConfig[i].MBAdr.ToString(),
                    _globalOmronConfig.EapConfig[i].MEAdr.ToString(),
                    _globalOmronConfig.EapConfig[i].DTType.ToString(),
                    _globalOmronConfig.EapConfig[i].Mark,
                    _globalOmronConfig.EapConfig[i].TagName,
                    _globalOmronConfig.EapConfig[i].GlobalBeginAddress.ToString(),
                    _globalOmronConfig.EapConfig[i].GlobalBeginOffset.ToString(),
                    _globalOmronConfig.EapConfig[i].GetMBAddressTag,
                    _globalOmronConfig.EapConfig[i].GetMEAddressTag,
                    _globalOmronConfig.EapConfig[i].DataFormat.ToString()
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
            for (int i = 0; i < _globalOmronConfig.PlcConfig.Count; i++)
            {
                var config = new MyConfig(
                    (i + 1).ToString(),
                    _globalOmronConfig.PlcConfig[i].DataValueStr,
                    _globalOmronConfig.PlcConfig[i].Length.ToString(),
                    _globalOmronConfig.PlcConfig[i].MBAdr.ToString(),
                    _globalOmronConfig.PlcConfig[i].MEAdr.ToString(),
                    _globalOmronConfig.PlcConfig[i].DTType.ToString(),
                    _globalOmronConfig.PlcConfig[i].Mark,
                    _globalOmronConfig.PlcConfig[i].TagName,
                    _globalOmronConfig.PlcConfig[i].GlobalBeginAddress.ToString(),
                    _globalOmronConfig.PlcConfig[i].GlobalBeginOffset.ToString(),
                    _globalOmronConfig.PlcConfig[i].GetMBAddressTag,
                    _globalOmronConfig.PlcConfig[i].GetMEAddressTag,
                    _globalOmronConfig.PlcConfig[i].DataFormat.ToString()
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
                for (int i = 0; i < _globalOmronConfig.EventConfig[idx].ListOutput.Count; i++)
                {
                    var config = new MyConfig(
                        (i + 1).ToString(),
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].DataValueStr,
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].Length.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].MBAdr.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].MEAdr.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].DTType.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].Mark,
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].TagName,
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].GlobalBeginAddress.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].GlobalBeginOffset.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].GetMBAddressTag,
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].GetMEAddressTag,
                        _globalOmronConfig.EventConfig[idx].ListOutput[i].DataFormat.ToString()
                    );
                    pcEventList.Add(config);
                }
                lvEventConfigPC.ItemsSource = pcEventList;

                // ����PLC�¼��б�
                var plcEventList = new List<MyConfig>();
                for (int i = 0; i < _globalOmronConfig.EventConfig[idx].ListInput.Count; i++)
                {
                    var config = new MyConfig(
                        (i + 1).ToString(),
                        _globalOmronConfig.EventConfig[idx].ListInput[i].DataValueStr,
                        _globalOmronConfig.EventConfig[idx].ListInput[i].Length.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListInput[i].MBAdr.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListInput[i].MEAdr.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListInput[i].DTType.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListInput[i].Mark,
                        _globalOmronConfig.EventConfig[idx].ListInput[i].TagName,
                        _globalOmronConfig.EventConfig[idx].ListInput[i].GlobalBeginAddress.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListInput[i].GlobalBeginOffset.ToString(),
                        _globalOmronConfig.EventConfig[idx].ListInput[i].GetMBAddressTag,
                        _globalOmronConfig.EventConfig[idx].ListInput[i].GetMEAddressTag,
                        _globalOmronConfig.EventConfig[idx].ListInput[i].DataFormat.ToString()
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
