using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using GetStartedApp.Utils.Node;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GetStartedApp.ViewModels.PLC
{
    public class ConnSiemensViewModel : ViewModelBase
    {
        // 字体缩放相关配置
        private const int BaseFontSize = 12; // 基础字体大小
        private const int MaxLengthForBaseFont = 20; // 基础字体对应的最大长度
        private const int MinFontSize = 8; // 最小字体大小（避免过小无法阅读）

        public ConnSiemensViewModel()
        {
            UpdateImage();
        }

        private void UpdateImage()
        {
            Test();
        }

        private void Test()
        {
            List<RegularItemNode> regulars = new List<RegularItemNode>
            {
                new RegularItemNode(){Name="温度",Index=100,NodeType=200,RegularCode=3,TypeLength=120 },
            };
            string selectedRegular = "温度";

            if (PanelWidth < 20) return;
            ImageFromBinding?.Dispose();
            ImageFromBinding = GetRenderInfo(regulars, selectedRegular);
        }

        #region 属性
        private const int EveryByteWidth = 16;
        private bool isShowText = true;

        private Size _panelSize;
        public Size PanelSize
        {
            get => _panelSize;
            set
            {
                if (SetProperty(ref _panelSize, value))
                {
                    PanelWidth = (int)value.Width;
                    UpdateImage();
                }
            }
        }

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
        #endregion

        #region 绘图
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
            double fontSize =12;

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

        #region 事件
        private DelegateCommand _RefreshCmd;
        public DelegateCommand RefreshCmd =>
            _RefreshCmd ?? (_RefreshCmd = new DelegateCommand(ExecuteRefreshCmd));

        void ExecuteRefreshCmd()
        {
            Test();
        }
        #endregion
    }
}