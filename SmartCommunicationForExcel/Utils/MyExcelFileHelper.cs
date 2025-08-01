
using HslCommunication.Profinet.Siemens;
using Newtonsoft.Json;
using OfficeOpenXml;
using SmartCommunicationForExcel.Core;
using SmartCommunicationForExcel.Implementation.Beckhoff;
using SmartCommunicationForExcel.Implementation.Mitsubishi;
using SmartCommunicationForExcel.Implementation.Omron;
using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Interface;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCommunicationForExcel.Utils
{
    public class MyExcelFileHelper<T> : IConvertJsonToString
    {
        public string ConvertJsonString(string str)
        {
            try
            {
                //格式化json字符串
                JsonSerializer serializer = new JsonSerializer();
                TextReader tr = new StringReader(str);
                JsonTextReader jtr = new JsonTextReader(tr);
                object obj = serializer.Deserialize(jtr);
                if (obj != null)
                {
                    StringWriter textWriter = new StringWriter();
                    JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' '
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }

                return str;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public string ObjectToJson(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        string[] _sheetNames = new string[] {"CpuInfo", "EapConfig", "PlcConfig", "EventConfig" };

        int _cpuInfoMaxRow = 1;
        int _cpuInfoStartRow = 3;
        int _cpuInfoStartCol = 1;

        int _eapConfigMaxRow = 198;
        int _eapConfigStartRow = 3;
        int _eapConfigStartCol = 2;

        int _plcConfigMaxRow = 198;
        int _plcConfigStartRow = 3;
        int _plcConfigStartCol = 2;

        int _eventConfigMaxRow = 2000;
        int _eventConfigStartRow = 3;
        int _eventConfigStartCol = 1;
        int _eventConfigHeadStart = 1;
        int _eventConfigPcStart = 4;
        int _eventConfigPlcStart = 15;

        int _eventConfigPcStart_Beckhoff = 6;
        int _eventConfigPlcStart_Beckhoff = 17;


        public SiemensGlobalConfig ExcelToSiemensObject(string strFile)
        {
            SiemensGlobalConfig siemensGlobalConfig = new SiemensGlobalConfig();
            SiemensCpuInfo siemensCpuInfo = new SiemensCpuInfo();
            List<SiemensEventIO> EapConfig = new List<SiemensEventIO>();
            List<SiemensEventIO> PlcConfig = new List<SiemensEventIO>();
            List<SiemensEventInstance> EventConfig = new List<SiemensEventInstance>();

            siemensGlobalConfig.CpuInfo = siemensCpuInfo;
            siemensGlobalConfig.EapConfig = EapConfig;
            siemensGlobalConfig.PlcConfig = PlcConfig;
            siemensGlobalConfig.EventConfig = EventConfig;
            //判段文件是否存在 文件默认放在exe一起
            if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\" + strFile))
            {
                throw new Exception("文件不存在");
            }
            else
            {
                try
                {
                    ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\" + strFile)))
                    {
                        int vSheetCount = package.Workbook.Worksheets.Count; //获取总Sheet页
                        if (vSheetCount != 4)
                        {
                            throw new Exception("Excel文件Sheet页数量不对");
                        }
                        else
                        {
                            //查看sheet也名称是否正确
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                string err = string.Empty;
                                if (package.Workbook.Worksheets[i].Name != _sheetNames[i])
                                {
                                    err += (_sheetNames[i] + ",");
                                }
                                if (!string.IsNullOrEmpty(err))
                                {
                                    throw new Exception($"Excel文件{err}Sheet匹配失败");
                                }
                            }
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                int col = 0;
                                ExcelWorksheet sheet = package.Workbook.Worksheets[i];
                                if (i == 0)//CpuInfo
                                {
                                    string cpuType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.CpuType = (Model.CpuType)Enum.Parse(typeof(Model.CpuType), cpuType);
                                    string plcType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.PlcType = (SiemensPLCS)Enum.Parse(typeof(SiemensPLCS), plcType);
                                    string mark = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.Mark = mark;
                                    string name = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.Name = name;
                                    short eapConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.EapConfigBeginAddress = eapConfigBeginAddress;
                                    short eapConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.EapConfigBeginOffset = eapConfigBeginOffset;
                                    short eapEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.EapEventBeginAddress = eapEventBeginAddress;
                                    short eapEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.EapEventBeginOffset = eapEventBeginOffset;
                                    short plcConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.PlcConfigBeginAddress = plcConfigBeginAddress;
                                    short plcConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.PlcConfigBeginOffset = plcConfigBeginOffset;
                                    short plcEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.PlcEventBeginAddress = plcEventBeginAddress;
                                    short plcEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.PlcEventBeginOffset = plcEventBeginOffset;
                                    string ip = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.IP = ip;
                                    short port = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.Port = port;
                                    int rack = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.Rack = (byte)rack;
                                    int slot = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    siemensCpuInfo.Slot = (byte)slot;
                                    //string dll = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    //siemensCpuInfo.Dll = (DllType)Enum.Parse(typeof(DllType), dll);
                                }
                                if(i == 1)//EapConfig
                                {
                                    for (int row = 0; row < _eapConfigMaxRow; row++)
                                    {
                                        int colEap = 0;
                                        //检查行是否可用
                                        if(GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, 5].Value).Contains("DT"))
                                        {
                                            SiemensEventIO siemensEventIO = new SiemensEventIO();

                                            short length = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.Length = length;
                                            short mbAdr = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.MBAdr = mbAdr;
                                            short meAdr = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.MEAdr = meAdr;
                                            string dtTpye = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            //将数据类型汇总
                                            if (dtTpye.Contains("DTShort")) dtTpye = "DTShort";
                                            if (dtTpye.Contains("DTString")) dtTpye = "DTString";
                                            siemensEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtTpye);
                                            string mark = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.Mark = mark;
                                            string tagName = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.TagName = tagName;
                                            short globalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.GlobalBeginAddress = globalBeginAddress;
                                            short globalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.GlobalBeginOffset = globalBeginOffset;
                                            string getMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.GetMBAddressTag = getMBAddressTag;
                                            string getMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            siemensEventIO.GetMEAddressTag = getMEAddressTag;
                                            //string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            //siemensEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);

                                            EapConfig.Add(siemensEventIO);
                                        }
                                        else
                                        {
                                            //改行为空
                                        }
                                    }
                                }
                                if(i == 2)//PlcCofig
                                {
                                    for (int row = 0; row < _plcConfigMaxRow; row++)
                                    {
                                        int colPlc = 0;
                                        //检查行是否可用
                                        if (GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, 5].Value).Contains("DT"))
                                        {
                                            SiemensEventIO siemensEventIO = new SiemensEventIO();

                                            short length = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.Length = length;
                                            short mbAdr = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.MBAdr = mbAdr;
                                            short meAdr = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.MEAdr = meAdr;
                                            string dtType = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            //将数据类型汇总
                                            if (dtType.Contains("DTShort")) dtType = "DTShort";
                                            if (dtType.Contains("DTString")) dtType = "DTString";
                                            siemensEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                            string mark = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.Mark = mark;
                                            string tagName = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.TagName = tagName;
                                            short globalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.GlobalBeginAddress = globalBeginAddress;
                                            short globalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.GlobalBeginOffset = globalBeginOffset;
                                            string getMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.GetMBAddressTag = getMBAddressTag;
                                            string getMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            siemensEventIO.GetMEAddressTag = getMEAddressTag;
                                            //string dataFormat = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            //siemensEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);

                                            PlcConfig.Add(siemensEventIO);
                                        }
                                        else
                                        {
                                            //改行为空
                                        }
                                    }
                                }
                                if (i == 3)//EventConfi
                                {
                                    
                                    for (int row = 0; row < _eventConfigMaxRow; row++)
                                    {
                                        //查看事件头 所有事件都从事件头部所在的行开始 事件头字段[DisableEvent]不为空表示事件头有效
                                        string disableEvent = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart].Value);
                                        if(disableEvent.Contains("是") || disableEvent.Contains("否"))
                                        {
                                            int colHead = 0;
                                            //以下部分属于一个事件
                                            SiemensEventInstance siemensEventInstance = new SiemensEventInstance();
                                            //将实列头写入
                                            siemensEventInstance.DisableEvent = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value) == "是";
                                            siemensEventInstance.EventClass = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            siemensEventInstance.EventName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            EventConfig.Add(siemensEventInstance);
                                            
                                        }
                                        //获取事件列表中最后一个实列 siemensEventInstanceLast
                                        SiemensEventInstance siemensEventInstanceLast = EventConfig.LastOrDefault();
                                        //开始往实列中添加内容
                                        if(siemensEventInstanceLast != null)
                                        {
                                            //PC
                                            if (GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + 3].Value).Contains("DT"))
                                            {
                                                int colPc = 0;
                                                //PC可以开始
                                                SiemensEventIO siemensEventIO = new SiemensEventIO();
                                                siemensEventInstanceLast.ListOutput.Add(siemensEventIO);

                                                siemensEventIO.IsEapRead = false;
                                                siemensEventIO.Length = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                siemensEventIO.MBAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                siemensEventIO.MEAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                string dtType = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                //将数据类型汇总
                                                if (dtType.Contains("DTShort")) dtType = "DTShort";
                                                if (dtType.Contains("DTString")) dtType = "DTString";
                                                siemensEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                                siemensEventIO.Mark = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                siemensEventIO.TagName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                siemensEventIO.GlobalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                siemensEventIO.GlobalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                siemensEventIO.GetMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                siemensEventIO.GetMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                //string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                //siemensEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);
                                            }
                                            //PLC
                                            if(GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + 3].Value).Contains("DT"))
                                            {
                                                int colPlc = 0;
                                                //PC可以开始
                                                SiemensEventIO siemensEventIO = new SiemensEventIO();
                                                siemensEventInstanceLast.ListInput.Add(siemensEventIO);

                                                siemensEventIO.IsEapRead = true;
                                                siemensEventIO.Length = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                siemensEventIO.MBAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                siemensEventIO.MEAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                string dtType = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                //将数据类型汇总
                                                if (dtType.Contains("DTShort")) dtType = "DTShort";
                                                if (dtType.Contains("DTString")) dtType = "DTString";
                                                siemensEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                                siemensEventIO.Mark = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                siemensEventIO.TagName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                siemensEventIO.GlobalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                siemensEventIO.GlobalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                siemensEventIO.GetMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                siemensEventIO.GetMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                //string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                //siemensEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }catch(Exception ex)
                {
                    return null;
                }
            }
            return siemensGlobalConfig;
        }
        public OmronGlobalConfig ExcelToOmronObject(string strFile)
        {
            OmronGlobalConfig omronGlobalConfig = new OmronGlobalConfig();
            OmronCpuInfo omronCpuInfo = new OmronCpuInfo();
            List<OmronEventIO> EapConfig = new List<OmronEventIO>();
            List<OmronEventIO> PlcConfig = new List<OmronEventIO>();
            List<OmronEventInstance> EventConfig = new List<OmronEventInstance>();

            omronGlobalConfig.CpuInfo = omronCpuInfo;
            omronGlobalConfig.EapConfig = EapConfig;
            omronGlobalConfig.PlcConfig = PlcConfig;
            omronGlobalConfig.EventConfig = EventConfig;
            //判段文件是否存在 文件默认放在exe一起
            if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\" + strFile))
            {
                throw new Exception("文件不存在");
            }
            else
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\" + strFile)))
                    {
                        int vSheetCount = package.Workbook.Worksheets.Count; //获取总Sheet页
                        if (vSheetCount != 4)
                        {
                            throw new Exception("Excel文件Sheet页数量不对");
                        }
                        else
                        {
                            //查看sheet也名称是否正确
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                string err = string.Empty;
                                if (package.Workbook.Worksheets[i].Name != _sheetNames[i])
                                {
                                    err += (_sheetNames[i] + ",");
                                }
                                if (!string.IsNullOrEmpty(err))
                                {
                                    throw new Exception($"Excel文件{err}Sheet匹配失败");
                                }
                            }
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                int col = 0;
                                ExcelWorksheet sheet = package.Workbook.Worksheets[i];
                                if (i == 0)//CpuInfo
                                {
                                    string cpuType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    //omronCpuInfo.CpuType = (Model.CpuType)Enum.Parse(typeof(Model.CpuType), cpuType);
                                    //string plcType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    //omronCpuInfo.PlcType = (SiemensPLCS)Enum.Parse(typeof(SiemensPLCS), plcType);
                                    string mark = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.Mark = mark;
                                    string name = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.Name = name;
                                    short eapConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.EapConfigBeginAddress = eapConfigBeginAddress;
                                    short eapConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.EapConfigBeginOffset = eapConfigBeginOffset;
                                    short eapEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.EapEventBeginAddress = eapEventBeginAddress;
                                    short eapEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.EapEventBeginOffset = eapEventBeginOffset;
                                    short plcConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.PlcConfigBeginAddress = plcConfigBeginAddress;
                                    short plcConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.PlcConfigBeginOffset = plcConfigBeginOffset;
                                    short plcEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.PlcEventBeginAddress = plcEventBeginAddress;
                                    short plcEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.PlcEventBeginOffset = plcEventBeginOffset;
                                    string ip = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.IP = ip;
                                    short port = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.Port = port;
                                    short SA1 = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.SA1 = (byte)SA1;
                                    short DA1 = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.DA1 = (byte)DA1;
                                    short DA2 = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    omronCpuInfo.DA2 = (byte)DA2;
                                }
                                if (i == 1)//EapConfig
                                {
                                    for (int row = 0; row < _eapConfigMaxRow; row++)
                                    {
                                        int colEap = 0;
                                        //检查行是否可用
                                        if (GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, 5].Value).Contains("DT"))
                                        {
                                            OmronEventIO omronEventIO = new OmronEventIO();

                                            short length = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.Length = length;
                                            short mbAdr = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.MBAdr = mbAdr;
                                            short meAdr = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.MEAdr = meAdr;
                                            string dtType = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            //将数据类型汇总
                                            if (dtType.Contains("DTShort")) dtType = "DTShort";
                                            if (dtType.Contains("DTString")) dtType = "DTString";
                                            omronEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                            string mark = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.Mark = mark;
                                            string tagName = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.TagName = tagName;
                                            short globalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.GlobalBeginAddress = globalBeginAddress;
                                            short globalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.GlobalBeginOffset = globalBeginOffset;
                                            string getMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.GetMBAddressTag = getMBAddressTag;
                                            string getMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            omronEventIO.GetMEAddressTag = getMEAddressTag;
                                            string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            //omronEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);

                                            EapConfig.Add(omronEventIO);
                                        }
                                        else
                                        {
                                            //改行为空
                                        }
                                    }
                                }
                                if (i == 2)//PlcCofig
                                {
                                    for (int row = 0; row < _plcConfigMaxRow; row++)
                                    {
                                        int colPlc = 0;
                                        //检查行是否可用
                                        if (GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, 5].Value).Contains("DT"))
                                        {
                                            OmronEventIO omronEventIO = new OmronEventIO();

                                            short length = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.Length = length;
                                            short mbAdr = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.MBAdr = mbAdr;
                                            short meAdr = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.MEAdr = meAdr;
                                            string dtType = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            //将数据类型汇总
                                            if (dtType.Contains("DTShort")) dtType = "DTShort";
                                            if (dtType.Contains("DTString")) dtType = "DTString";
                                            omronEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                            string mark = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.Mark = mark;
                                            string tagName = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.TagName = tagName;
                                            short globalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.GlobalBeginAddress = globalBeginAddress;
                                            short globalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.GlobalBeginOffset = globalBeginOffset;
                                            string getMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.GetMBAddressTag = getMBAddressTag;
                                            string getMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            omronEventIO.GetMEAddressTag = getMEAddressTag;
                                            string dataFormat = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            //omronEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);

                                            PlcConfig.Add(omronEventIO);
                                        }
                                        else
                                        {
                                            //改行为空
                                        }
                                    }
                                }
                                if (i == 3)//EventConfi
                                {

                                    for (int row = 0; row < _eventConfigMaxRow; row++)
                                    {
                                        //查看事件头 所有事件都从事件头部所在的行开始 事件头字段[DisableEvent]不为空表示事件头有效
                                        string disableEvent = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart].Value);
                                        if (disableEvent.Contains("是") || disableEvent.Contains("否"))
                                        {
                                            int colHead = 0;
                                            //以下部分属于一个事件
                                            OmronEventInstance omronEventInstance = new OmronEventInstance();
                                            //将实列头写入
                                            omronEventInstance.DisableEvent = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value) == "是";
                                            omronEventInstance.EventClass = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            omronEventInstance.EventName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            EventConfig.Add(omronEventInstance);

                                        }
                                        //获取事件列表中最后一个实列 siemensEventInstanceLast
                                        OmronEventInstance omronEventInstanceLast = EventConfig.LastOrDefault();
                                        //开始往实列中添加内容
                                        if (omronEventInstanceLast != null)
                                        {
                                            //PC
                                            if (GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + 3].Value).Contains("DT"))
                                            {
                                                int colPc = 0;
                                                //PC可以开始
                                                OmronEventIO omronEventIO = new OmronEventIO();
                                                omronEventInstanceLast.ListOutput.Add(omronEventIO);

                                                omronEventIO.IsEapRead = false;
                                                omronEventIO.Length = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                omronEventIO.MBAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                omronEventIO.MEAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                string dtType = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                //将数据类型汇总
                                                if (dtType.Contains("DTShort")) dtType = "DTShort";
                                                if (dtType.Contains("DTString")) dtType = "DTString";
                                                omronEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                                omronEventIO.Mark = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                omronEventIO.TagName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                omronEventIO.GlobalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                omronEventIO.GlobalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                omronEventIO.GetMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                omronEventIO.GetMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                //omronEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);
                                            }
                                            //PLC
                                            if (GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + 3].Value).Contains("DT"))
                                            {
                                                int colPlc = 0;
                                                //PC可以开始
                                                OmronEventIO omronEvnetIO = new OmronEventIO();
                                                omronEventInstanceLast.ListInput.Add(omronEvnetIO);

                                                omronEvnetIO.IsEapRead = true;
                                                omronEvnetIO.Length = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                omronEvnetIO.MBAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                omronEvnetIO.MEAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                string dtType = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                //将数据类型汇总
                                                if (dtType.Contains("DTShort")) dtType = "DTShort";
                                                if (dtType.Contains("DTString")) dtType = "DTString";
                                                omronEvnetIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                                omronEvnetIO.Mark = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                omronEvnetIO.TagName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                omronEvnetIO.GlobalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                omronEvnetIO.GlobalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                omronEvnetIO.GetMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                omronEvnetIO.GetMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                //omronEvnetIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return omronGlobalConfig;
        }

        public MitsubishiGlobalConfig ExcelToMitsubishiObject(string strFile)
        {
            MitsubishiGlobalConfig mitsubishiGlobalConfig = new MitsubishiGlobalConfig();
            MitsubishiCpuInfo mitsubishiCpuInfo = new MitsubishiCpuInfo();
            List<MitsubishiEventIO> EapConfig = new List<MitsubishiEventIO>();
            List<MitsubishiEventIO> PlcConfig = new List<MitsubishiEventIO>();
            List<MitsubishiEventInstance> EventConfig = new List<MitsubishiEventInstance>();

            mitsubishiGlobalConfig.CpuInfo = mitsubishiCpuInfo;
            mitsubishiGlobalConfig.EapConfig = EapConfig;
            mitsubishiGlobalConfig.PlcConfig = PlcConfig;
            mitsubishiGlobalConfig.EventConfig = EventConfig;
            //判段文件是否存在 文件默认放在exe一起
            if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\" + strFile))
            {
                throw new Exception("文件不存在");
            }
            else
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\" + strFile)))
                    {
                        int vSheetCount = package.Workbook.Worksheets.Count; //获取总Sheet页
                        if (vSheetCount != 4)
                        {
                            throw new Exception("Excel文件Sheet页数量不对");
                        }
                        else
                        {
                            //查看sheet也名称是否正确
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                string err = string.Empty;
                                if (package.Workbook.Worksheets[i].Name != _sheetNames[i])
                                {
                                    err += (_sheetNames[i] + ",");
                                }
                                if (!string.IsNullOrEmpty(err))
                                {
                                    throw new Exception($"Excel文件{err}Sheet匹配失败");
                                }
                            }
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                int col = 0;
                                ExcelWorksheet sheet = package.Workbook.Worksheets[i];
                                if (i == 0)//CpuInfo
                                {
                                    string cpuType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.CpuType = (Model.CpuType)Enum.Parse(typeof(Model.CpuType), cpuType);
                                    string plcType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.PlcType = plcType;
                                    string mark = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.Mark = mark;
                                    string name = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.Name = name;
                                    short eapConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.EapConfigBeginAddress = eapConfigBeginAddress;
                                    short eapConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.EapConfigBeginOffset = eapConfigBeginOffset;
                                    short eapEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.EapEventBeginAddress = eapEventBeginAddress;
                                    short eapEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.EapEventBeginOffset = eapEventBeginOffset;
                                    short plcConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.PlcConfigBeginAddress = plcConfigBeginAddress;
                                    short plcConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.PlcConfigBeginOffset = plcConfigBeginOffset;
                                    short plcEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.PlcEventBeginAddress = plcEventBeginAddress;
                                    short plcEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.PlcEventBeginOffset = plcEventBeginOffset;
                                    string ip = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.IP = ip;
                                    short port1 = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.Port1 = port1;
                                    int port2 = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    mitsubishiCpuInfo.Port2 = (short)port2;
                                    //string dll = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    //siemensCpuInfo.Dll = (DllType)Enum.Parse(typeof(DllType), dll);
                                }
                                if (i == 1)//EapConfig
                                {
                                    for (int row = 0; row < _eapConfigMaxRow; row++)
                                    {
                                        int colEap = 0;
                                        //检查行是否可用
                                        if (GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, 5].Value).Contains("DT"))
                                        {
                                            MitsubishiEventIO mitsubishiEventIO = new MitsubishiEventIO();

                                            short length = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.Length = length;
                                            short mbAdr = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.MBAdr = mbAdr;
                                            short meAdr = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.MEAdr = meAdr;
                                            string dtTpye = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            //将数据类型汇总
                                            if (dtTpye.Contains("DTShort")) dtTpye = "DTShort";
                                            if (dtTpye.Contains("DTString")) dtTpye = "DTString";
                                            mitsubishiEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtTpye);
                                            string mark = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.Mark = mark;
                                            string tagName = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.TagName = tagName;
                                            short globalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.GlobalBeginAddress = globalBeginAddress;
                                            short globalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.GlobalBeginOffset = globalBeginOffset;
                                            string getMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.GetMBAddressTag = getMBAddressTag;
                                            string getMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.GetMEAddressTag = getMEAddressTag;
                                            string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            mitsubishiEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);

                                            EapConfig.Add(mitsubishiEventIO);
                                        }
                                        else
                                        {
                                            //改行为空
                                        }
                                    }
                                }
                                if (i == 2)//PlcCofig
                                {
                                    for (int row = 0; row < _plcConfigMaxRow; row++)
                                    {
                                        int colPlc = 0;
                                        //检查行是否可用
                                        if (GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, 5].Value).Contains("DT"))
                                        {
                                            MitsubishiEventIO mitsubishiEventIO = new MitsubishiEventIO();

                                            short length = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.Length = length;
                                            short mbAdr = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.MBAdr = mbAdr;
                                            short meAdr = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.MEAdr = meAdr;
                                            string dtType = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            //将数据类型汇总
                                            if (dtType.Contains("DTShort")) dtType = "DTShort";
                                            if (dtType.Contains("DTString")) dtType = "DTString";
                                            mitsubishiEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                            string mark = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.Mark = mark;
                                            string tagName = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.TagName = tagName;
                                            short globalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.GlobalBeginAddress = globalBeginAddress;
                                            short globalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.GlobalBeginOffset = globalBeginOffset;
                                            string getMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.GetMBAddressTag = getMBAddressTag;
                                            string getMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.GetMEAddressTag = getMEAddressTag;
                                            string dataFormat = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            mitsubishiEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);

                                            PlcConfig.Add(mitsubishiEventIO);
                                        }
                                        else
                                        {
                                            //改行为空
                                        }
                                    }
                                }
                                if (i == 3)//EventConfi
                                {

                                    for (int row = 0; row < _eventConfigMaxRow; row++)
                                    {
                                        //查看事件头 所有事件都从事件头部所在的行开始 事件头字段[DisableEvent]不为空表示事件头有效
                                        string disableEvent = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart].Value);
                                        if (disableEvent.Contains("是") || disableEvent.Contains("否"))
                                        {
                                            int colHead = 0;
                                            //以下部分属于一个事件
                                            MitsubishiEventInstance mitsubishiEventInstance = new MitsubishiEventInstance();
                                            //将实列头写入
                                            mitsubishiEventInstance.DisableEvent = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value) == "是";
                                            mitsubishiEventInstance.EventClass = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            mitsubishiEventInstance.EventName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            EventConfig.Add(mitsubishiEventInstance);

                                        }
                                        //获取事件列表中最后一个实列 siemensEventInstanceLast
                                        MitsubishiEventInstance mitsubishiEventInstanceLast = EventConfig.LastOrDefault();
                                        //开始往实列中添加内容
                                        if (mitsubishiEventInstanceLast != null)
                                        {
                                            //PC
                                            if (GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + 3].Value).Contains("DT"))
                                            {
                                                int colPc = 0;
                                                //PC可以开始
                                                MitsubishiEventIO mitsubishiEventIO = new MitsubishiEventIO();
                                                mitsubishiEventInstanceLast.ListOutput.Add(mitsubishiEventIO);

                                                mitsubishiEventIO.IsEapRead = false;
                                                mitsubishiEventIO.Length = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                mitsubishiEventIO.MBAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                mitsubishiEventIO.MEAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                string dtType = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                //将数据类型汇总
                                                if (dtType.Contains("DTShort")) dtType = "DTShort";
                                                if (dtType.Contains("DTString")) dtType = "DTString";
                                                mitsubishiEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                                mitsubishiEventIO.Mark = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                mitsubishiEventIO.TagName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                mitsubishiEventIO.GlobalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                mitsubishiEventIO.GlobalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                mitsubishiEventIO.GetMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                mitsubishiEventIO.GetMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart + colPc++].Value);
                                                mitsubishiEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);
                                            }
                                            //PLC
                                            if (GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + 3].Value).Contains("DT"))
                                            {
                                                int colPlc = 0;
                                                //PC可以开始
                                                MitsubishiEventIO mitsubishiEventIO = new MitsubishiEventIO();
                                                mitsubishiEventInstanceLast.ListInput.Add(mitsubishiEventIO);

                                                mitsubishiEventIO.IsEapRead = true;
                                                mitsubishiEventIO.Length = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                mitsubishiEventIO.MBAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                mitsubishiEventIO.MEAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                string dtType = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                //将数据类型汇总
                                                if (dtType.Contains("DTShort")) dtType = "DTShort";
                                                if (dtType.Contains("DTString")) dtType = "DTString";
                                                mitsubishiEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                                mitsubishiEventIO.Mark = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                mitsubishiEventIO.TagName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                mitsubishiEventIO.GlobalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                mitsubishiEventIO.GlobalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                mitsubishiEventIO.GetMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                mitsubishiEventIO.GetMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart + colPlc++].Value);
                                                mitsubishiEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return mitsubishiGlobalConfig;
        }

        public BeckhoffGlobalConfig ExcelToBeckhoffObject(string strFile)
        {
            BeckhoffGlobalConfig beckhoffGlobalConfig = new BeckhoffGlobalConfig();
            BeckhoffCpuInfo beckhoffCpuInfo = new BeckhoffCpuInfo();
            List<BeckhoffEventIO> EapConfig = new List<BeckhoffEventIO>();
            List<BeckhoffEventIO> PlcConfig = new List<BeckhoffEventIO>();
            List<BeckhoffEventInstance> EventConfig = new List<BeckhoffEventInstance>();

            beckhoffGlobalConfig.CpuInfo = beckhoffCpuInfo;
            beckhoffGlobalConfig.EapConfig = EapConfig;
            beckhoffGlobalConfig.PlcConfig = PlcConfig;
            beckhoffGlobalConfig.EventConfig = EventConfig;
            //判段文件是否存在 文件默认放在exe一起
            if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "\\" + strFile))
            {
                throw new Exception("文件不存在");
            }
            else
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(System.IO.Directory.GetCurrentDirectory() + "\\" + strFile)))
                    {
                        int vSheetCount = package.Workbook.Worksheets.Count; //获取总Sheet页
                        if (vSheetCount != 4)
                        {
                            throw new Exception("Excel文件Sheet页数量不对");
                        }
                        else
                        {
                            //查看sheet也名称是否正确
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                string err = string.Empty;
                                if (package.Workbook.Worksheets[i].Name != _sheetNames[i])
                                {
                                    err += (_sheetNames[i] + ",");
                                }
                                if (!string.IsNullOrEmpty(err))
                                {
                                    throw new Exception($"Excel文件{err}Sheet匹配失败");
                                }
                            }
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                int col = 0;
                                ExcelWorksheet sheet = package.Workbook.Worksheets[i];
                                if (i == 0)//CpuInfo
                                {
                                    string cpuType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.CpuType = (Model.CpuType)Enum.Parse(typeof(Model.CpuType), cpuType);
                                   // string plcType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    //beckhoffCpuInfo.PlcType = (SiemensPLCS)Enum.Parse(typeof(SiemensPLCS), plcType);
                                    string mark = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.Mark = mark;
                                    string name = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.Name = name;
                                    string eapLabel = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.EapConfigLabel = eapLabel;
                                    short eapConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.EapConfigBeginAddress = eapConfigBeginAddress;
                                    short eapConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.EapConfigBeginOffset = eapConfigBeginOffset;
                                    short eapEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.EapEventBeginAddress = eapEventBeginAddress;
                                    short eapEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.EapEventBeginOffset = eapEventBeginOffset;
                                    string plcLabel = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.PlcConfigLabel = plcLabel;
                                    short plcConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.PlcConfigBeginAddress = plcConfigBeginAddress;
                                    short plcConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.PlcConfigBeginOffset = plcConfigBeginOffset;
                                    short plcEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.PlcEventBeginAddress = plcEventBeginAddress;
                                    short plcEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.PlcEventBeginOffset = plcEventBeginOffset;
                                    string ip = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.IP = ip;
                                    int port = GetExcelCellToInt(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.Port = port;
                                    string TargetNetId = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.TargetNetId = TargetNetId;
                                    string SenderNetId = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);
                                    beckhoffCpuInfo.SenderNetId = SenderNetId;
                                }
                                if (i == 1)//EapConfig
                                {
                                    for (int row = 0; row < _eapConfigMaxRow; row++)
                                    {
                                        int colEap = 0;
                                        //检查行是否可用
                                        if (GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, 5].Value).Contains("DT"))
                                        {
                                            BeckhoffEventIO beckhoffEventIO = new BeckhoffEventIO();

                                            short length = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.Length = length;
                                            short mbAdr = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.MBAdr = mbAdr;
                                            short meAdr = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.MEAdr = meAdr;
                                            string dtTpye = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            //将数据类型汇总
                                            if (dtTpye.Contains("DTShort")) dtTpye = "DTShort";
                                            if (dtTpye.Contains("DTString")) dtTpye = "DTString";
                                            beckhoffEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtTpye);
                                            string mark = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.Mark = mark;
                                            string tagName = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.TagName = tagName;
                                            short globalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.GlobalBeginAddress = globalBeginAddress;
                                            short globalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.GlobalBeginOffset = globalBeginOffset;
                                            string getMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.GetMBAddressTag = getMBAddressTag;
                                            string getMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.GetMEAddressTag = getMEAddressTag;
                                            string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eapConfigStartRow, _eapConfigStartCol + colEap++].Value);
                                            beckhoffEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);

                                            EapConfig.Add(beckhoffEventIO);
                                        }
                                        else
                                        {
                                            //改行为空
                                        }
                                    }
                                }
                                if (i == 2)//PlcCofig
                                {
                                    for (int row = 0; row < _plcConfigMaxRow; row++)
                                    {
                                        int colPlc = 0;
                                        //检查行是否可用
                                        if (GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, 5].Value).Contains("DT"))
                                        {
                                            BeckhoffEventIO beckhoffEventIO = new BeckhoffEventIO();

                                            short length = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.Length = length;
                                            short mbAdr = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.MBAdr = mbAdr;
                                            short meAdr = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.MEAdr = meAdr;
                                            string dtType = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            //将数据类型汇总
                                            if (dtType.Contains("DTShort")) dtType = "DTShort";
                                            if (dtType.Contains("DTString")) dtType = "DTString";
                                            beckhoffEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                            string mark = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.Mark = mark;
                                            string tagName = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.TagName = tagName;
                                            short globalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.GlobalBeginAddress = globalBeginAddress;
                                            short globalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.GlobalBeginOffset = globalBeginOffset;
                                            string getMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.GetMBAddressTag = getMBAddressTag;
                                            string getMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.GetMEAddressTag = getMEAddressTag;
                                            string dataFormat = GetExcelCellToStr(sheet.Cells[row + _plcConfigStartRow, _plcConfigStartCol + colPlc++].Value);
                                            beckhoffEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);

                                            PlcConfig.Add(beckhoffEventIO);
                                        }
                                        else
                                        {
                                            //改行为空
                                        }
                                    }
                                }
                                if (i == 3)//EventConfi
                                {

                                    for (int row = 0; row < _eventConfigMaxRow; row++)
                                    {
                                        //查看事件头 所有事件都从事件头部所在的行开始 事件头字段[DisableEvent]不为空表示事件头有效
                                        string disableEvent = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart].Value);
                                        if (disableEvent.Contains("是") || disableEvent.Contains("否"))
                                        {
                                            int colHead = 0;
                                            //以下部分属于一个事件
                                            BeckhoffEventInstance beckhoffEventInstance = new BeckhoffEventInstance();
                                            //将实列头写入
                                            beckhoffEventInstance.DisableEvent = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value) == "是";
                                            beckhoffEventInstance.EventClass = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            beckhoffEventInstance.EventName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            beckhoffEventInstance.PC_LabelName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            beckhoffEventInstance.PLC_LabelName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigHeadStart + colHead++].Value);
                                            EventConfig.Add(beckhoffEventInstance);

                                        }
                                        //获取事件列表中最后一个实列 siemensEventInstanceLast
                                        BeckhoffEventInstance siemensEventInstanceLast = EventConfig.LastOrDefault();
                                        //开始往实列中添加内容
                                        if (siemensEventInstanceLast != null)
                                        {
                                            //PC
                                            if (GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + 3].Value).Contains("DT"))
                                            {
                                                int colPc = 0;
                                                //PC可以开始
                                                BeckhoffEventIO beckhoffEventIO = new BeckhoffEventIO();
                                                siemensEventInstanceLast.ListOutput.Add(beckhoffEventIO);

                                                beckhoffEventIO.IsEapRead = false;
                                                beckhoffEventIO.Length = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                beckhoffEventIO.MBAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                beckhoffEventIO.MEAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                string dtType = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                //将数据类型汇总
                                                if (dtType.Contains("DTShort")) dtType = "DTShort";
                                                if (dtType.Contains("DTString")) dtType = "DTString";
                                                beckhoffEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                                beckhoffEventIO.Mark = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                beckhoffEventIO.TagName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                beckhoffEventIO.GlobalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                beckhoffEventIO.GlobalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                beckhoffEventIO.GetMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                beckhoffEventIO.GetMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPcStart_Beckhoff + colPc++].Value);
                                                beckhoffEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);
                                            }
                                            //PLC
                                            if (GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + 3].Value).Contains("DT"))
                                            {
                                                int colPlc = 0;
                                                //PC可以开始
                                                BeckhoffEventIO beckhoffEventIO = new BeckhoffEventIO();
                                                siemensEventInstanceLast.ListInput.Add(beckhoffEventIO);

                                                beckhoffEventIO.IsEapRead = true;
                                                beckhoffEventIO.Length = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                beckhoffEventIO.MBAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                beckhoffEventIO.MEAdr = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                string dtType = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                //将数据类型汇总
                                                if (dtType.Contains("DTShort")) dtType = "DTShort";
                                                if (dtType.Contains("DTString")) dtType = "DTString";
                                                beckhoffEventIO.DTType = (CDataType)Enum.Parse(typeof(CDataType), dtType);
                                                beckhoffEventIO.Mark = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                beckhoffEventIO.TagName = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                beckhoffEventIO.GlobalBeginAddress = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                beckhoffEventIO.GlobalBeginOffset = GetExcelCellToShort(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                beckhoffEventIO.GetMBAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                beckhoffEventIO.GetMEAddressTag = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                string dataFormat = GetExcelCellToStr(sheet.Cells[row + _eventConfigStartRow, _eventConfigPlcStart_Beckhoff + colPlc++].Value);
                                                beckhoffEventIO.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), dataFormat);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return beckhoffGlobalConfig;
        }
        private string GetExcelCellToStr(object obj)
        {
            if (obj == null)
                return "";
            else
                return obj.ToString();
        }

        private short GetExcelCellToShort(object obj)
        {
            if (obj == null)
                return 0;
            else
                return Convert.ToInt16(obj);
        }

        private int GetExcelCellToInt(object obj)
        {
            if (obj == null)
                return 0;
            else
                return Convert.ToInt32(obj);
        }

        private char GetExcelCellToChar(object obj)
        {
            if (obj == null)
                return ' ';
            else
                return Convert.ToChar(obj);
        }
    }
}
