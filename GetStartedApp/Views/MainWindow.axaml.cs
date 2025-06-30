using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using GetStartedApp.ViewModels;
using System.Diagnostics;
using Ursa.Controls;

namespace GetStartedApp.Views
{
    public partial class MainWindow : Window
    {
        //private MainWindowViewModel? _viewModel;
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            //base.OnAttachedToVisualTree(e);
            //if (DataContext is not MainWindowViewModel vm) return;
            //_viewModel = vm;
            //_viewModel.ToastManager = new WindowToastManager(TopLevel.GetTopLevel(this)) { MaxItems = 3 };
        }

    }
}
