using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using GetStartedApp.Models;
using GetStartedApp.Utils.Node;
using NetTaste;
using OfficeOpenXml;
using Prism.Commands;
using SmartCommunicationForExcel.EventHandle.Siemens;
using SmartCommunicationForExcel.Executer;
using SmartCommunicationForExcel.Implementation.Siemens;
using SmartCommunicationForExcel.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Ursa.Controls;

namespace GetStartedApp.ViewModels.PLC
{
    public class ConnSiemensViewModel : ViewModelBase, ISiemensEventExecuter
    {

        public ConnSiemensViewModel()

        {
            InitPLCs();
        }

        #region 属性
        private const int EveryByteWidth = 16;
        private bool isShowText = true;
        private const string Path = @"/Assets/PLCs/";

        private int _panelWidth = 800;
        public int PanelWidth
        {
            get => _panelWidth;
            private set => SetProperty(ref _panelWidth, value);
        }

        private Bitmap? _imageFromBinding;
        public Bitmap? ImageFromBinding
        {
            get => _imageFromBinding;
            set => SetProperty(ref _imageFromBinding, value);
        }

        private string _PLCConnStatus;
        public string PLCConnStatus
        {
            get { return _PLCConnStatus; }
            set { SetProperty(ref _PLCConnStatus, value); }
        }

        private ObservableCollection<PLCModel> _ObPLC;

        public ObservableCollection<PLCModel> ObPLC
        {
            get { return _ObPLC ?? (_ObPLC = new ObservableCollection<PLCModel>()); }
            set { SetProperty(ref _ObPLC, value); }
        }
        #endregion

        private PLCModel _SelectedPLC;
        public PLCModel SelectedPLC
        {
            get { return _SelectedPLC; }
            set { SetProperty(ref _SelectedPLC, value); }
        }

        #region 事件
        private DelegateCommand _ConnAllPlcCmd;
        public DelegateCommand ConnAllPlcCmd =>
            _ConnAllPlcCmd ?? (_ConnAllPlcCmd = new DelegateCommand(ExecuteConnAllPlcCmd));

        void ExecuteConnAllPlcCmd()
        {

        }

        private DelegateCommand _PLCConfigCmd;
        public DelegateCommand PLCConfigCmd =>
            _PLCConfigCmd ?? (_PLCConfigCmd = new DelegateCommand(ExecutePLCConfigCmd));

        void ExecutePLCConfigCmd()
        {

        }

        private DelegateCommand<object> _ConnCmd;
        public DelegateCommand<object> ConnCmd =>
            _ConnCmd ?? (_ConnCmd = new DelegateCommand<object>(ExecuteConnCmd));

        void ExecuteConnCmd(object parameter)
        {
            var model = parameter as PLCModel;
            SelectedPLC = model;
            if (model != null)
            {
                if (model.FIsConn != "已连接")
                {
                    PLCModel pLCModel = ObPLC.Where(it => it.FFileName == model.FFileName).FirstOrDefault();

                    ResultState resultState = _sc.StartSiemensWorkInstance(model.FAddr, Path + model.FFileName);
                    pLCModel.FIsConn = resultState.IsSuccess ? "已连接" : "连接失败";
                    if (!resultState.IsSuccess) MessageBox.ShowAsync(resultState.Message);
                }
                else
                {

                }

            }

        }


        private DelegateCommand<object> _UnCnnCmd;
        public DelegateCommand<object> UnCnnCmd =>
            _UnCnnCmd ?? (_UnCnnCmd = new DelegateCommand<object>(ExecuteUnCnnCmd));

        void ExecuteUnCnnCmd(object parameter)
        {
            var model = parameter as PLCModel;
            SelectedPLC = model;
            if (model != null)
            {
                if (model.FIsConn == "已连接" || model.FIsConn == "后台连接中")
                {
                    PLCModel pLCModel = ObPLC.Where(it => it.FFileName == model.FFileName).FirstOrDefault();
                    _sc.StopSiemensWorkInstance(model.FAddr);
                    pLCModel.FIsConn = "未连接";
                }
                else
                {

                }

            }
        }

        private DelegateCommand<object> _MonitoringCmd;
        public DelegateCommand<object> MonitoringCmd =>
            _MonitoringCmd ?? (_MonitoringCmd = new DelegateCommand<object>(ExecuteMonitoringCmd));

        void ExecuteMonitoringCmd(object parameter)
        {
            var model = parameter as PLCModel;
            SelectedPLC = model;
            if (model != null)
            {
                if (model.FIsConn == "已连接" || model.FIsConn == "后台连接中")
                {
                    ResultState resultState = _sc.ShowSimensConfig(model.FAddr);
                    if (!resultState.IsSuccess)
                    {

                    }
                }
                else
                {

                }
            }
        }

        #endregion


        #region 方法
        public object HandleEvent(object sei)
        {
            throw new NotImplementedException();
        }

        public void SubscribeCommonInfo(string strInstanceName, bool bSuccess, List<SiemensEventIO> listInput, List<SiemensEventIO> listOutput, string strError = "")
        {

        }

        public void Err(string strInstanceName, byte[] data, string strError = "")
        {

        }


        private void InitPLCs()
        {
            var pLCModels = GetPLCConfigInfoByFolder(Directory.GetCurrentDirectory() + Path);
            if (pLCModels != null)
            {
                if (pLCModels.Length > 0)
                {
                    ObPLC = new ObservableCollection<PLCModel>(pLCModels);
                }
            }

            _sc = new SmartContainer();
            //注册
            _sc.RegisterInstance<ISiemensEventExecuter>(ConstName.SiemensRegisterName, this);
            //开现程连接plc
            if (m_isAutoConnPLC)
            {
                for (int i = 0; i < ObPLC.Count; i++)
                {
                    int n = i;
                    new Thread(() =>
                    {
                        do
                        {
                            if (ObPLC[n].FIsConn != "已连接")
                                ObPLC[n].FIsConn = _sc.StartSiemensWorkInstance(ObPLC[n].FAddr, Path + ObPLC[n].FFileName).IsSuccess ? "已连接" : "连接失败";
                        } while (ObPLC[n].FIsConn != "已连接" && !m_mre.WaitOne(m_interval));
                    })
                    { IsBackground = true }.Start();
                    Thread.Sleep(100);
                }
            }
        }


        private ManualResetEvent m_mre = new ManualResetEvent(false);
        private bool m_isAutoConnPLC = false;
        private int m_interval = 2000;//s

        private SmartContainer _sc = null;

        private string[] _sheetNames = new string[] { "CpuInfo", "EapConfig", "PlcConfig", "EventConfig" };

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

        private char GetExcelCellToChar(object obj)
        {
            if (obj == null)
                return ' ';
            else
                return Convert.ToChar(obj);
        }

        private int _cpuInfoMaxRow = 1;
        private int _cpuInfoStartRow = 3;
        private int _cpuInfoStartCol = 1;

        //读取指定PLC配置文件夹下所有配置文件信息
        /// <summary>
        ///
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns></returns>
        private PLCModel[] GetPLCConfigInfoByFolder(string path)
        {
            List<PLCModel> listPLCModel = new List<PLCModel>();
            if (!Directory.Exists(path))//判断文件夹是否存在
            {
                throw new Exception("文件不存在");
            }
            else
            {
                //查看文件夹下面满足条件的文件
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();

                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.ToLower().Contains("_siemens.xlsx"))//表示是plc配置文件
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        try
                        {
                            using (ExcelPackage package = new ExcelPackage(new FileInfo(files[i].FullName)))
                            {
                                //查看sheet也名称是否正确
                                for (int j = 0; j < package.Workbook.Worksheets.Count; j++)
                                {
                                    string err = string.Empty;
                                    if (package.Workbook.Worksheets[j].Name != _sheetNames[j])
                                    {
                                        err += (_sheetNames[j] + ",");
                                    }
                                    if (!string.IsNullOrEmpty(err))
                                    {
                                        throw new Exception($"Excel文件{err}Sheet匹配失败");
                                    }
                                }
                                int col = 0;
                                ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                                string cpuType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                string plcType = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                string mark = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                string name = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short eapConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short eapConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short eapEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short eapEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short plcConfigBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short plcConfigBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short plcEventBeginAddress = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short plcEventBeginOffset = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                string ip = GetExcelCellToStr(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                short port = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                int rack = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                int slot = GetExcelCellToShort(sheet.Cells[_cpuInfoStartRow, _cpuInfoStartCol + col++].Value);

                                PLCModel model = new PLCModel()
                                {
                                    FFileName = files[i].Name,
                                    FCpuType = cpuType,
                                    FPLCType = plcType,
                                    FName = name,
                                    FIsConn = "未连接",
                                    FColor = "Black",
                                    FAddr = ip,
                                    FMark = mark
                                };
                                listPLCModel.Add(model);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return listPLCModel.ToArray();
        }
        #endregion


        #region 绘图

        // 字体缩放相关配置
        private const int BaseFontSize = 12; // 基础字体大小
        private const int MaxLengthForBaseFont = 20; // 基础字体对应的最大长度
        private const int MinFontSize = 8; // 最小字体大小（避免过小无法阅读）

        private RenderTargetBitmap GetRenderInfo(List<RegularItemNode> regulars, string selectedRegular)
        {
            regulars.Sort();
            int max_byte = regulars.Count == 0 ? 0 : regulars.Max(m => m.GetLengthByte());
            int every_byte_occupy = EveryByteWidth + 4;
            int every_line_count = (PanelWidth - 19 - 90) / every_byte_occupy;
            if (every_line_count < 10) every_line_count = 10;
            int line_count = GetNumberByUplimit(max_byte, every_line_count);

            var pixelSize = new PixelSize(PanelWidth - 19, line_count * 50 + 5);
            var dpi = new Vector(96, 96);
            var renderBitmap = new RenderTargetBitmap(pixelSize, dpi);

            using (var renderContext = renderBitmap.CreateDrawingContext())
            {
                var penGray = new Pen(Brushes.Gray, 1);
                var penDimGray = new Pen(Brushes.DimGray, 1);
                var penChocolate = new Pen(Brushes.Chocolate, 1);
                var typeface = new Typeface("Microsoft YaHei");

                // 绘制背景
                renderContext.FillRectangle(Brushes.White, new Rect(0, 0, pixelSize.Width, pixelSize.Height));

                int paint_x = 85;
                int paint_y = 2;
                int count = 0;

                // 绘制左侧竖线
                renderContext.DrawLine(penGray, new Point(paint_x - 5, 0), new Point(paint_x - 5, pixelSize.Height));
                double byteNumFontSize = GetDynamicFontSize(every_line_count);
                // 绘制行号和字节格子
                for (int i = 0; i < line_count; i++)
                {
                    var text = new FormattedText(
                        $"[{count:D3} - {(count + Math.Min(max_byte - count - 1, every_line_count - 1)):D3}]",
                        CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        typeface,
                        12,
                        Brushes.Black
                    );
                    renderContext.DrawText(text, new Point(2, paint_y + EveryByteWidth));

                    for (int j = 0; j < every_line_count; j++)
                    {
                        var rect = new Rect(
                            paint_x + j * (EveryByteWidth + 4),
                            paint_y + 17,
                            EveryByteWidth,
                            EveryByteWidth
                        );
                        renderContext.DrawRectangle(penGray, rect);

                        var numText = new FormattedText(
                            count.ToString(),
                            CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            typeface,
                            byteNumFontSize,
                            Brushes.Black
                        );
                        DrawCenteredText(renderContext, numText, rect);

                        count++;
                        if (count >= max_byte) break;
                    }
                    paint_y += 50;
                }

                paint_y = 2;

                // 绘制变量区域
                foreach (var regular in regulars)
                {
                    bool isSelected = !string.IsNullOrEmpty(selectedRegular) && selectedRegular == regular.Name;
                    int start = regular.GetStartedByteIndex();
                    int byteLength = regular.GetLengthByte() - start; // 变量总长度（字节数）
                    int rowStart = GetNumberByUplimit(start, every_line_count);
                    int rowEnd = GetNumberByUplimit(start + byteLength, every_line_count);

                    // 绘制辅助线（包含动态字体）
                    PaintLineAuxiliary(
                        renderContext,
                        paint_x,
                        paint_y,
                        every_line_count,
                        start,
                        byteLength,
                        true,
                        isSelected,
                        regular.Name,
                        typeface,
                        regular
                    );

                    // 绘制数据块
                    for (int j = 0; j < byteLength; j++)
                    {
                        int posX = paint_x + (start + j) % every_line_count * every_byte_occupy;
                        int posY = paint_y + 17 + (start + j) / every_line_count * 50;

                        var rect = new Rect(posX, posY, 16, 16);
                        renderContext.FillRectangle(regular.BackColor, rect);
                        renderContext.DrawRectangle(penDimGray, rect);

                        var numText = new FormattedText(
                            (start + j).ToString(),
                            CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            typeface,
                            GetDynamicFontSize(byteLength),
                            Brushes.Black
                        );
                        DrawCenteredText(renderContext, numText, rect);
                    }
                }
            }

            return renderBitmap;
        }

        // 居中绘制文本
        private void DrawCenteredText(DrawingContext context, FormattedText text, Rect rect)
        {
            double x = rect.X + (rect.Width - text.Width) / 2;
            double y = rect.Y + (rect.Height - text.Height) / 2;
            context.DrawText(text, new Point(x, y));
        }

        // 计算动态字体大小（根据长度自动缩小）
        private double GetDynamicFontSize(int byteLength)
        {
            if (byteLength <= MaxLengthForBaseFont)
                return BaseFontSize; // 长度较小时用基础字体
            // 长度超过阈值时，按比例缩小（最小不小于MinFontSize）
            double scale = (double)MaxLengthForBaseFont / byteLength;
            return Math.Max(MinFontSize, BaseFontSize * scale);
        }
        private int GetNumberByUplimit(int value, int count)
        {
            if (value == 0) return 1;
            return value % count == 0 ? value / count : value / count + 1;
        }

        private Point CalculatePointWithByteIndex(int paint_x, int paint_y, int every_line_count, int index)
        {
            return new Point(
                paint_x + index % every_line_count * 20 + 8,
                paint_y + 17 + index / every_line_count * 50 + 8
            );
        }

        private void PaintLineAuxiliary(
            DrawingContext ctx,
            int paint_x,
            int paint_y,
            int every_line_count,
            int index,
            int byteLength, // 变量总长度（字节数）
            bool isDown,
            bool isSelect,
            string info,
            Typeface typeface,
            RegularItemNode regularNode)
        {
            Point point1 = CalculatePointWithByteIndex(paint_x, paint_y, every_line_count, index);
            Point point2 = CalculatePointWithByteIndex(paint_x, paint_y, every_line_count, index + byteLength - 1);
            Point point1_right = CalculatePointWithByteIndex(paint_x, paint_y, every_line_count, every_line_count - 1);
            Point point2_left = CalculatePointWithByteIndex(paint_x, paint_y, every_line_count, 0);

            var penDimGray = new Pen(Brushes.DimGray, 1);
            var penChocolate = new Pen(Brushes.Chocolate, 1);
            var brushLightPink = new SolidColorBrush(Colors.LightPink);

            // ********** 解决：跨行选中背景不完整 **********
            if (isSelect)
            {
                // 计算变量占据的所有行（起始行到结束行）
                int startIndex = index;
                int endIndex = index + byteLength - 1;
                int startRow = startIndex / every_line_count; // 起始行号
                int endRow = endIndex / every_line_count;     // 结束行号

                // 遍历所有行，填充粉色背景
                for (int row = startRow; row <= endRow; row++)
                {
                    // 计算当前行的起始和结束索引
                    int rowStartIndex = row * every_line_count;
                    int rowEndIndex = (row + 1) * every_line_count - 1;

                    // 当前行在变量范围内的实际起始和结束索引
                    int currentStart = Math.Max(startIndex, rowStartIndex);
                    int currentEnd = Math.Min(endIndex, rowEndIndex);

                    // 计算当前行背景的X坐标和宽度
                    double x = paint_x + (currentStart % every_line_count) * 20 + 8 - 9; // 左偏移（与原逻辑保持一致）
                    double width = (currentEnd - currentStart + 1) * 20 + 10; // 宽度=字节数*每个字节宽度+边距
                    double y = paint_y + 17 + row * 50 - 25; // Y坐标（与原逻辑保持一致）

                    // 填充当前行的粉色背景
                    ctx.FillRectangle(brushLightPink, new Rect(x, y, width, 52));
                }
            }

            // ********** 解决：字体随长度变长自动缩小 **********
            // 计算动态字体大小
            double fontSize = 12;

            if (point1.Y == point2.Y)
            {
                // 同一行
                if (isDown)
                {
                    point1 = point1.WithY(point1.Y + 12);
                    point2 = point2.WithY(point2.Y + 12);

                    ctx.DrawLine(penDimGray, point1, point1.WithY(point1.Y - 3));
                    ctx.DrawLine(penDimGray, point2, point2.WithY(point2.Y - 3));
                    ctx.DrawLine(penDimGray, point1, point2);

                    // 绘制变量文本（动态字体）
                    var text = new FormattedText(
                        regularNode.TypeLength == 1 ? info : $"{info} * {regularNode.TypeLength}",
                        CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        typeface,
                        fontSize, // 使用动态字体大小
                        Brushes.Black
                    );
                    double centerX = (point1.X + point2.X) / 2 - text.Width / 2;
                    ctx.DrawText(text, new Point(centerX, point1.Y));
                }
                else
                {
                    point1 = point1.WithY(point1.Y - 11);
                    point2 = point2.WithY(point2.Y - 11);

                    ctx.DrawLine(penChocolate, point1, point1.WithY(point1.Y + 3));
                    ctx.DrawLine(penChocolate, point2, point2.WithY(point2.Y + 3));
                    ctx.DrawLine(penChocolate, point1, point2);

                    if (isShowText)
                    {
                        var text = new FormattedText(
                            info,
                            CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            typeface,
                            fontSize, // 动态字体
                            Brushes.Black
                        );
                        double centerX = (point1.X + point2.X) / 2 - text.Width / 2;
                        ctx.DrawText(text, new Point(centerX, point1.Y - 14));
                    }
                }
            }
            else
            {
                // 跨行
                if (isDown)
                {
                    point1 = point1.WithY(point1.Y + 12);
                    point2 = point2.WithY(point2.Y + 12);
                    point1_right = point1_right.WithY(point1.Y).WithX(point1_right.X + 10);
                    point2_left = point2_left.WithY(point2.Y).WithX(point2_left.X - 10);

                    ctx.DrawLine(penDimGray, point1, point1.WithY(point1.Y - 3));
                    ctx.DrawLine(penDimGray, point1, point1_right);
                    ctx.DrawLine(penDimGray, point2, point2.WithY(point2.Y - 3));
                    ctx.DrawLine(penDimGray, point2, point2_left);

                    // 绘制变量文本（动态字体）
                    var textStr = regularNode.TypeLength == 1 ? info : $"{info} * {regularNode.TypeLength}";
                    var text = new FormattedText(
                        textStr,
                        CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        typeface,
                        fontSize, // 动态字体
                        Brushes.Black
                    );

                    if ((point1_right.X - point1.X) > (point2.X - point2_left.X))
                    {
                        ctx.DrawText(text, new Point(point1.X - 40, point1.Y));
                    }
                    else
                    {
                        ctx.DrawText(text, new Point(point2_left.X - 40, point2.Y));
                    }
                }
                else
                {
                    point1 = point1.WithY(point1.Y - 11);
                    point2 = point2.WithY(point2.Y - 11);
                    point1_right = point1_right.WithY(point1.Y).WithX(point1_right.X + 10);
                    point2_left = point2_left.WithY(point2.Y).WithX(point2_left.X - 10);

                    ctx.DrawLine(penChocolate, point1, point1.WithY(point1.Y + 3));
                    ctx.DrawLine(penChocolate, point1, point1_right);
                    ctx.DrawLine(penChocolate, point2, point2.WithY(point2.Y + 3));
                    ctx.DrawLine(penChocolate, point2, point2_left);

                    if (isShowText)
                    {
                        var text = new FormattedText(
                            info,
                            CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            typeface,
                            fontSize, // 动态字体
                            Brushes.Black
                        );

                        if ((point1_right.X - point1.X) > (point2.X - point2_left.X))
                        {
                            ctx.DrawText(text, new Point(point1.X - 40, point1.Y - 14));
                        }
                        else
                        {
                            ctx.DrawText(text, new Point(point2_left.X - 40, point2.Y - 14));
                        }
                    }
                }
            }
        }
        #endregion
    }
}