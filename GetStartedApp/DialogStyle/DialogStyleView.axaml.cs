using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Prism.Dialogs;

namespace GetStartedApp;

public partial class DialogStyleView : Window,IDialogWindow
{
    public DialogStyleView()
    {
        InitializeComponent();
    }

    public IDialogResult Result { get;set; }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}