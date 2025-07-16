using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
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

        public ConnSiemensViewModel()
        {
            UpdateImage();
        }

        private void UpdateImage()
        {
            Test();
            //  ImageFromBinding = GetRenderInfo();
        }


        private void Test()
        {
            // 示例数据
            List<RegularItemNode> regulars = new List<RegularItemNode>
            {
                new RegularItemNode(){Name="温度",Index=1,NodeType=200,RegularCode=3,TypeLength=1 },

            };
            string selectedRegular = "温度"; // 示例选中变量

            if (PanelWidth < 20) return;
            ImageFromBinding?.Dispose();
            ImageFromBinding = GetRenderInfo(regulars, selectedRegular);
        }


        #region 属性

        private const int EveryByteWidth = 16;
        private bool isShowText = true; // 示例值，根据实际需求调整

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
                renderContext.DrawLine(
                    penGray,
                    new Point(paint_x - 5, 0),
                    new Point(paint_x - 5, pixelSize.Height)
                );

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
                        12,
                        Brushes.Black
                        );
                        renderContext.DrawText(numText,
                            new Point(paint_x + j * every_byte_occupy - every_byte_occupy+23, paint_y + 17)
                        );

                        count++;
                        if (count >= max_byte) break;
                    }
                    paint_y += 50;
                }

                paint_y = 2;

                // 绘制变量区域
                for (int i = 0; i < regulars.Count; i++)
                {
                    var regular = regulars[i];
                    bool isSelected = !string.IsNullOrEmpty(selectedRegular) && selectedRegular == regular.Name;
                    int start = regular.GetStartedByteIndex();
                    int length = regular.GetLengthByte() - start;
                    int rowStart = GetNumberByUplimit(start, every_line_count);
                    int rowEnd = GetNumberByUplimit(start + length, every_line_count);

                    // 绘制辅助线
                    PaintLineAuxiliary(
                        renderContext,
                        paint_x,
                        paint_y,
                        every_line_count,
                        start,
                        length,
                        true,
                        isSelected,
                        regular.Name,
                        typeface,
                        regular
                    );

                    // 绘制数据块
                    int tmp = start;
                    for (int j = 0; j < length; j++)
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
                            12,
                            Brushes.Black
                        );
                        renderContext.DrawText(
                              numText,
                            new Point(posX + 4, posY)

                        );
                    }
                }
            }

            return renderBitmap;
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
            int byteLength,
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

            if (point1.Y == point2.Y)
            {
                // 同一行的情况
                if (isDown)
                {
                    // 选中效果
                    if (isSelect)
                    {
                        ctx.FillRectangle(
                            brushLightPink,
                            new Rect(point1.X - 9, point1.Y - 25, point2.X - point1.X + 18, 52)
                        );
                    }

                    // 绘制线段
                    point1 = point1.WithY(point1.Y + 12);
                    point2 = point2.WithY(point2.Y + 12);

                    ctx.DrawLine(penDimGray, point1, point1.WithY(point1.Y - 3));
                    ctx.DrawLine(penDimGray, point2, point2.WithY(point2.Y - 3));
                    ctx.DrawLine(penDimGray, point1, point2);

                    // 绘制文本
                    var text = new FormattedText(
                        regularNode.TypeLength == 1 ? info : $"{info} * {regularNode.TypeLength}",
                       CultureInfo.InvariantCulture,
                       FlowDirection.LeftToRight,
                        typeface,
                        12,
                        Brushes.Black
                    );
                    ctx.DrawText(text, new Point(point1.X, point1.Y));
                }
                else
                {
                    // 绘制线段
                    point1 = point1.WithY(point1.Y - 11);
                    point2 = point2.WithY(point2.Y - 11);

                    ctx.DrawLine(penChocolate, point1, point1.WithY(point1.Y + 3));
                    ctx.DrawLine(penChocolate, point2, point2.WithY(point2.Y + 3));
                    ctx.DrawLine(penChocolate, point1, point2);

                    // 绘制文本
                    if (isShowText)
                    {
                        var text = new FormattedText(
                            info,
                           CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            typeface,
                            12,
                            Brushes.Black
                        );
                        ctx.DrawText(text, new Point(point1.X +2, point1.Y - 14));
                    }
                }
            }
            else
            {
                // 跨行情况
                if (isDown)
                {
                    // 选中效果
                    if (isSelect)
                    {
                        ctx.FillRectangle(
                            brushLightPink,
                            new Rect(point1.X - 9, point1.Y - 25, point1_right.X, 52)
                        );
                        ctx.FillRectangle(
                            brushLightPink,
                            new Rect(point2_left.X - 10, point2.Y - 25, point2.X - point2_left.X + 19, 52)
                        );
                    }

                    // 绘制线段
                    point1 = point1.WithY(point1.Y + 12);
                    point2 = point2.WithY(point2.Y + 12);
                    point1_right = point1_right.WithY(point1.Y).WithX(point1_right.X + 10);
                    point2_left = point2_left.WithY(point2.Y).WithX(point2_left.X - 10);

                    ctx.DrawLine(penDimGray, point1, point1.WithY(point1.Y - 3));
                    ctx.DrawLine(penDimGray, point1, point1_right);
                    ctx.DrawLine(penDimGray, point2, point2.WithY(point2.Y - 3));
                    ctx.DrawLine(penDimGray, point2, point2_left);

                    // 绘制文本
                    var textStr = regularNode.TypeLength == 1 ? info : $"{info} * {regularNode.TypeLength}";
                    var text = new FormattedText(
                        textStr,
                       CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        typeface,
                        12,
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
                    // 绘制线段
                    point1 = point1.WithY(point1.Y - 11);
                    point2 = point2.WithY(point2.Y - 11);
                    point1_right = point1_right.WithY(point1.Y).WithX(point1_right.X + 10);
                    point2_left = point2_left.WithY(point2.Y).WithX(point2_left.X - 10);

                    ctx.DrawLine(penChocolate, point1, point1.WithY(point1.Y + 3));
                    ctx.DrawLine(penChocolate, point1, point1_right);
                    ctx.DrawLine(penChocolate, point2, point2.WithY(point2.Y + 3));
                    ctx.DrawLine(penChocolate, point2, point2_left);

                    // 绘制文本
                    if (isShowText)
                    {
                        var text = new FormattedText(
                            info,
                          CultureInfo.InvariantCulture,
                            FlowDirection.LeftToRight,
                            typeface,
                            12,
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