using SmartCommunicationForExcel.Implementation.Beckhoff;
using SmartCommunicationForExcel.Interface;
using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using Avalonia.Data;

namespace SmartCommunicationForExcel.SmartConfigForExcel
{
    /// <summary>
    /// SmartBeckhoffConfigForExcelForm.xaml �Ľ����߼�
    /// </summary>
    public partial class SmartBeckhoffConfigForExcelForm : Window
    {
        private IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance> _globalBeckhoffConfig;

        // �ؼ�����
        //private TextBlock lbCpuType;
        //private TextBlock lbPlcType;
        //private TextBlock lbMark;
        //private TextBlock lbName;
        //private TextBlock lbEapLabelName;
        //private TextBlock lbEapConfigBeginAddress;
        //private TextBlock lbEapConfigBeginOffset;
        //private TextBlock lbEapEventBeginAddress;
        //private TextBlock lbEapEventBeginOffset;
        //private TextBlock lbPlcLabelName;
        //private TextBlock lbPlcConfigBeginAddress;
        //private TextBlock lbPlcConfigBeginOffset;
        //private TextBlock lbPlcEventBeginAddress;
        //private TextBlock lbPlcEventBeginOffset;
        //private TextBlock lbIP;
        //private TextBlock lbPort;
        //private TextBlock lbTargetNetId;
        //private TextBlock lbSenderNetId;
        //private ComboBox comboBox1;
        //private DataGrid lvEapConfig;
        //private DataGrid lvPlcConfig;
        //private DataGrid lvEventConfigPC;
        //private DataGrid lvEventConfigPLC;

        public SmartBeckhoffConfigForExcelForm()
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
            //lbEapLabelName = this.FindControl<TextBlock>("lbEapLabelName");
            //lbEapConfigBeginAddress = this.FindControl<TextBlock>("lbEapConfigBeginAddress");
            //lbEapConfigBeginOffset = this.FindControl<TextBlock>("lbEapConfigBeginOffset");
            //lbEapEventBeginAddress = this.FindControl<TextBlock>("lbEapEventBeginAddress");
            //lbEapEventBeginOffset = this.FindControl<TextBlock>("lbEapEventBeginOffset");
            //lbPlcLabelName = this.FindControl<TextBlock>("lbPlcLabelName");
            //lbPlcConfigBeginAddress = this.FindControl<TextBlock>("lbPlcConfigBeginAddress");
            //lbPlcConfigBeginOffset = this.FindControl<TextBlock>("lbPlcConfigBeginOffset");
            //lbPlcEventBeginAddress = this.FindControl<TextBlock>("lbPlcEventBeginAddress");
            //lbPlcEventBeginOffset = this.FindControl<TextBlock>("lbPlcEventBeginOffset");
            //lbIP = this.FindControl<TextBlock>("lbIP");
            //lbPort = this.FindControl<TextBlock>("lbPort");
            //lbTargetNetId = this.FindControl<TextBlock>("lbTargetNetId");
            //lbSenderNetId = this.FindControl<TextBlock>("lbSenderNetId");

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

        public void SetModel(IBeckhoffGlobalConfig<BeckhoffEventIO, BeckhoffCpuInfo, BeckhoffEventInstance> globalConfig)
        {
            _globalBeckhoffConfig = globalConfig;
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
            foreach (BeckhoffEventInstance sei in _globalBeckhoffConfig.EventConfig)
            {
                comboBox1.Items.Add($"{++i},{sei.DisableEvent},{sei.EventClass},{sei.PC_LabelName},{sei.PLC_LabelName}--{sei.EventName}");
            }
        }

        private void ReFresh()
        {
            if (_globalBeckhoffConfig == null) return;

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
            lbCpuType.Text = _globalBeckhoffConfig.CpuInfo.CpuType.ToString();
           // lbPlcType.Text = _globalBeckhoffConfig.CpuInfo.PlcType.ToString();
            lbMark.Text = _globalBeckhoffConfig.CpuInfo.Mark;
            lbName.Text = _globalBeckhoffConfig.CpuInfo.Name;
            lbEapLabelName.Text = _globalBeckhoffConfig.CpuInfo.EapConfigLabel;
            lbEapConfigBeginAddress.Text = _globalBeckhoffConfig.CpuInfo.EapConfigBeginAddress.ToString();
            lbEapConfigBeginOffset.Text = _globalBeckhoffConfig.CpuInfo.EapConfigBeginOffset.ToString();
            lbEapEventBeginAddress.Text = _globalBeckhoffConfig.CpuInfo.EapEventBeginAddress.ToString();
            lbEapEventBeginOffset.Text = _globalBeckhoffConfig.CpuInfo.EapEventBeginOffset.ToString();
            lbPlcLabelName.Text = _globalBeckhoffConfig.CpuInfo.PlcConfigLabel;
            lbPlcConfigBeginAddress.Text = _globalBeckhoffConfig.CpuInfo.PlcConfigBeginAddress.ToString();
            lbPlcConfigBeginOffset.Text = _globalBeckhoffConfig.CpuInfo.PlcConfigBeginOffset.ToString();
            lbPlcEventBeginAddress.Text = _globalBeckhoffConfig.CpuInfo.PlcEventBeginAddress.ToString();
            lbPlcEventBeginOffset.Text = _globalBeckhoffConfig.CpuInfo.PlcEventBeginOffset.ToString();
            lbIP.Text = _globalBeckhoffConfig.CpuInfo.IP;
            lbPort.Text = _globalBeckhoffConfig.CpuInfo.Port.ToString();
            lbTargetNetId.Text = _globalBeckhoffConfig.CpuInfo.TargetNetId;
            lbSenderNetId.Text = _globalBeckhoffConfig.CpuInfo.SenderNetId;
        }

        /// <summary>
        /// ����Eap�����б�
        /// </summary>
        private void UpdateEapConfigList()
        {
            var configList = new List<MyConfig>();
            for (int i = 0; i < _globalBeckhoffConfig.EapConfig.Count; i++)
            {
                var config = new MyConfig(
                    (i + 1).ToString(),
                    _globalBeckhoffConfig.EapConfig[i].DataValueStr,
                    _globalBeckhoffConfig.EapConfig[i].Length.ToString(),
                    _globalBeckhoffConfig.EapConfig[i].MBAdr.ToString(),
                    _globalBeckhoffConfig.EapConfig[i].MEAdr.ToString(),
                    _globalBeckhoffConfig.EapConfig[i].DTType.ToString(),
                    _globalBeckhoffConfig.EapConfig[i].Mark,
                    _globalBeckhoffConfig.EapConfig[i].TagName,
                    _globalBeckhoffConfig.EapConfig[i].GlobalBeginAddress.ToString(),
                    _globalBeckhoffConfig.PlcConfig[i].GlobalBeginOffset.ToString(),
                    _globalBeckhoffConfig.EapConfig[i].GetMBAddressTag,
                    _globalBeckhoffConfig.EapConfig[i].GetMEAddressTag,
                    _globalBeckhoffConfig.EapConfig[i].DataFormat.ToString()
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
            for (int i = 0; i < _globalBeckhoffConfig.PlcConfig.Count; i++)
            {
                var config = new MyConfig(
                    (i + 1).ToString(),
                    _globalBeckhoffConfig.PlcConfig[i].DataValueStr,
                    _globalBeckhoffConfig.PlcConfig[i].Length.ToString(),
                    _globalBeckhoffConfig.PlcConfig[i].MBAdr.ToString(),
                    _globalBeckhoffConfig.PlcConfig[i].MEAdr.ToString(),
                    _globalBeckhoffConfig.PlcConfig[i].DTType.ToString(),
                    _globalBeckhoffConfig.PlcConfig[i].Mark,
                    _globalBeckhoffConfig.PlcConfig[i].TagName,
                    _globalBeckhoffConfig.PlcConfig[i].GlobalBeginAddress.ToString(),
                    _globalBeckhoffConfig.PlcConfig[i].GlobalBeginOffset.ToString(),
                    _globalBeckhoffConfig.PlcConfig[i].GetMBAddressTag,
                    _globalBeckhoffConfig.PlcConfig[i].GetMEAddressTag,
                    _globalBeckhoffConfig.PlcConfig[i].DataFormat.ToString()
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
            // �滻ԭ�����жϺ��ַ����ָ����
            if (comboBox1.SelectedItem == null) return;

            string selectedText = comboBox1.SelectedItem.ToString();
            if (string.IsNullOrEmpty(selectedText)) return;

            string[] splits = selectedText.Trim().Split(',');
            if (splits.Length >= 1 && int.TryParse(splits[0], out int idx))
            {
                idx--; // ת��Ϊ0������

                // ����PC�¼��б�
                var pcEventList = new List<MyConfig>();
                for (int i = 0; i < _globalBeckhoffConfig.EventConfig[idx].ListOutput.Count; i++)
                {
                    var config = new MyConfig(
                        (i + 1).ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].DataValueStr,
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].Length.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].MBAdr.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].MEAdr.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].DTType.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].Mark,
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].TagName,
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].GlobalBeginAddress.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].GlobalBeginOffset.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].GetMBAddressTag,
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].GetMEAddressTag,
                        _globalBeckhoffConfig.EventConfig[idx].ListOutput[i].DataFormat.ToString()
                    );
                    pcEventList.Add(config);
                }
                lvEventConfigPC.ItemsSource = pcEventList;

                // ����PLC�¼��б�
                var plcEventList = new List<MyConfig>();
                for (int i = 0; i < _globalBeckhoffConfig.EventConfig[idx].ListInput.Count; i++)
                {
                    var config = new MyConfig(
                        (i + 1).ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].DataValueStr,
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].Length.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].MBAdr.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].MEAdr.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].DTType.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].Mark,
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].TagName,
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].GlobalBeginAddress.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].GlobalBeginOffset.ToString(),
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].GetMBAddressTag,
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].GetMEAddressTag,
                        _globalBeckhoffConfig.EventConfig[idx].ListInput[i].DataFormat.ToString()
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

    // ȷ��MyConfig��������������ƥ��
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

        public MyConfig(string no, string dateValue, string length, string mbAdr, string meAdr, string dtType,
                       string mark, string tagName, string globalBeginAddress, string globalBeginOffset,
                       string getMBAddressTag, string getMEAddressTag, string dataFormat)
        {
            No = no;
            DateValue = dateValue;
            Length = length;
            MBAdr = mbAdr;
            MEAdr = meAdr;
            DTType = dtType;
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
