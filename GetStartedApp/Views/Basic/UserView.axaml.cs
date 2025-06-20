using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GetStartedApp;

public partial class UserView : UserControl
{
    public UserView()
    {
        InitializeComponent();
        // Prism will auto-wire ViewModel via ViewModelLocator, do not set DataContext manually
    }
}