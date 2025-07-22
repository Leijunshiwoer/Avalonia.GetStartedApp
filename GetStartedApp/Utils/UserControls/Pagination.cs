using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GetStartedApp.Utils.UserControls
{
    public class Pagination : ContentControl
    {
        private int _currentPage = 1;
        private Button _prevPageButton = null!;
        private Button _nextPageButton = null!;
        private Button goToPageButton = null!;
        private TextBox _pageNumberTextBox = null!;
        private TextBlock _pageInfoTextBlock = null!;
        private ComboBox _itemsPerPageComboBox = null!;

        // 定义可绑定的 ItemsSource 属性
        public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
            AvaloniaProperty.Register<Pagination, IEnumerable>(nameof(ItemsSource), defaultBindingMode: BindingMode.TwoWay);


        public IEnumerable ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // 定义可绑定的 ItemsPerPage 属性
        public static readonly StyledProperty<int> ItemsPerPageProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(ItemsPerPage), 10, defaultBindingMode: BindingMode.TwoWay);

        public int ItemsPerPage
        {
            get => GetValue(ItemsPerPageProperty);
            set => SetValue(ItemsPerPageProperty, value);
        }

        // 定义可绑定的 TotalItems 属性
        public static readonly StyledProperty<int> TotalItemsProperty =
        AvaloniaProperty.Register<Pagination, int>(nameof(TotalItems), defaultValue: -1);

        public int TotalItems
        {
            get => GetValue(TotalItemsProperty);
            set => SetValue(TotalItemsProperty, value);
        }

        // 定义可绑定的 IsBackendPaging 属性
        public static readonly StyledProperty<bool> IsBackendPagingProperty =
        AvaloniaProperty.Register<Pagination, bool>(nameof(IsBackendPaging), false, defaultBindingMode: BindingMode.TwoWay);


        public bool IsBackendPaging
        {
            get => GetValue(IsBackendPagingProperty);
            set => SetValue(IsBackendPagingProperty, value);
        }

        public static readonly StyledProperty<ICommand> PageChangedCommandProperty =
    AvaloniaProperty.Register<Pagination, ICommand>(nameof(PageChangedCommand));

        public ICommand PageChangedCommand
        {
            get => GetValue(PageChangedCommandProperty);
            set => SetValue(PageChangedCommandProperty, value);
        }

        // 定义事件，通知外部逻辑当前页数和每页条数,后台分页模式下返回当前页号和每页条数
        public event Action<Pagination, int, int, IEnumerable> PageChanged = null!;

        public Pagination()
        {
            InitializeControls();
        }
        /// <summary>
        /// 重写 OnApplyTemplate 方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e); // 传递 TemplateAppliedEventArgs 参数

            // Replace the following line:  
            var index = _itemsPerPageComboBox.ItemsSource?
                .Cast<int>() // 将 IEnumerable 转换为 IEnumerable<int>
                .Select((value, idx) => new { Value = value, Index = idx }) // 将值和索引绑定
                .FirstOrDefault(x => x.Value == ItemsPerPage)?.Index; // 查找目标值并返回索引
            _itemsPerPageComboBox.SelectedIndex = index == null ? 0 : (int)index;
            // 确保控件初始化完成后再添加监听
            if (!IsBackendPaging)
            {
                // 伪分页，监听数据源属性变化
                ItemsSourceProperty.Changed.AddClassHandler<Pagination>((x, e) => x.UpdatePagination());
            }
            else
            {
                // 后台分页，监听总数属性变化，更新分页信息
                TotalItemsProperty.Changed.AddClassHandler<Pagination>((x, e) => x.UpdatePaginationInfo());
            }

            // 监听每页条数属性变化
            ItemsPerPageProperty.Changed.AddClassHandler<Pagination>((x, e) => x.UpdatePagination());


            _prevPageButton.Click += (s, e) => GoToPage(_currentPage - 1);
            _nextPageButton.Click += (s, e) => GoToPage(_currentPage + 1);


            goToPageButton.Click += (s, e) =>
            {
                if (int.TryParse(_pageNumberTextBox.Text, out var pageNumber))
                {
                    var totalPages = CalculateTotalPages();
                    if (pageNumber < 1 || pageNumber > totalPages)
                    {
                        _pageNumberTextBox.Text = string.Empty; // 清空输入框
                        return;
                    }
                    GoToPage(pageNumber);
                }
            };

            _itemsPerPageComboBox.SelectionChanged += (s, e) =>
            {
                if (_itemsPerPageComboBox.SelectedItem is int selectedItemsPerPage)
                {
                    ItemsPerPage = selectedItemsPerPage;
                    _currentPage = 1; // 重置到第一页
                    UpdatePagination();
                }
            };

            // 初始化分页
            UpdatePagination();
        }

        // 初始化控件
        private void InitializeControls()
        {
            // 创建分页控件
            var paginationPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 10, 0, 0)
            };

            _itemsPerPageComboBox = new ComboBox
            {
                Width = 100,
                Margin = new Thickness(0, 0, 10, 0),
                ItemsSource = new[] { 5,10, 20, 50, 100, 500, 1000 }
            };

            // 上一页按钮
            _prevPageButton = new Button
            {
                Content = "上一页",
                Padding = new Thickness(5, 0),
                FontSize = 14,
                Height = 34,
                Margin = new Thickness(0, 0, 5, 0)
            };

            // 下一页按钮
            _nextPageButton = new Button
            {
                Content = "下一页",
                Padding = new Thickness(5, 0),
                FontSize = 14,
                Height = 34,
                Margin = new Thickness(5, 0, 0, 0)
            };

            // 页号输入框
            _pageNumberTextBox = new TextBox
            {
                Width = 100,
                Margin = new Thickness(5, 0, 5, 0),
                FontSize = 14,
                Height = 34,
                VerticalAlignment = VerticalAlignment.Center
            };

            // 页号跳转按钮
            goToPageButton = new Button
            {
                Content = "跳转",
                Padding = new Thickness(5, 0),
                FontSize = 14,
                Height = 34,
                Margin = new Thickness(5, 0, 0, 0)
            };


            // 当前页/总页数显示
            _pageInfoTextBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 10, 0)
            };

            paginationPanel.Children.Add(_itemsPerPageComboBox);
            paginationPanel.Children.Add(_prevPageButton);
            paginationPanel.Children.Add(_pageInfoTextBlock);
            paginationPanel.Children.Add(_pageNumberTextBox);
            paginationPanel.Children.Add(goToPageButton);
            paginationPanel.Children.Add(_nextPageButton);

            // 将分页控件设置为 Content
            Content = paginationPanel;
        }
        //后台分页，由于只提供数据总数，所以只触发更新,不触发事件
        private void UpdatePaginationInfo()
        {
            if (TotalItems < 0) return;
            var totalPages = CalculateTotalPages();
            _pageInfoTextBlock.Text = $"第 {_currentPage} 页 / 共 {totalPages} 页（共 {TotalItems} 条）";
            _prevPageButton.IsEnabled = _currentPage > 1;
            _nextPageButton.IsEnabled = _currentPage < totalPages;
        }
        // 更新分页并触发Change
        private void UpdatePagination()
        {
            if (ItemsSource == null) return;
            if (!IsBackendPaging && ItemsSource.Cast<object>().Count() == 0) return;
            var totalPages = CalculateTotalPages();
            var totalItems = TotalItems == -1 ? ItemsSource.Cast<object>().Count() : TotalItems;
            _pageInfoTextBlock.Text = $"第 {_currentPage} 页 / 共 {totalPages} 页（共 {totalItems} 条）";
            _prevPageButton.IsEnabled = _currentPage > 1;
            _nextPageButton.IsEnabled = _currentPage < totalPages;

            if (!IsBackendPaging)
            {
                // 伪分页模式：返回当前页的数据集
                var pagedData = ItemsSource.Cast<object>()
                .Skip((_currentPage - 1) * ItemsPerPage)
                .Take(ItemsPerPage);
                PageChanged?.Invoke(this, _currentPage, ItemsPerPage, pagedData);
            }
            else
            {
                // 后台分页模式：返回当前页号和每页条数
                PageChanged?.Invoke(this, _currentPage, ItemsPerPage, Enumerable.Empty<object>());
            }

            // 触发MVVM命令
            PageChangedCommand?.Execute(new PageChangedEventArgs
            {
                CurrentPage = _currentPage,
                ItemsPerPage = ItemsPerPage
            });
        }

        // 计算总页数
        private int CalculateTotalPages()
        {
            var totalItems = TotalItems == -1 ? ItemsSource.Cast<object>().Count() : TotalItems;
            return (int)Math.Ceiling((double)totalItems / ItemsPerPage);
        }

        // 跳转到指定页
        private void GoToPage(int page)
        {
            _currentPage = page;
            UpdatePagination();
        }

    }

}
