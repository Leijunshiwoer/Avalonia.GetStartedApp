using Avalonia.Controls;
using Avalonia;
using Irihi.Avalonia.Shared.Helpers;

namespace GetStartedApp.Utils;
public static class ScrollViewerExtensions
{
    public static readonly AttachedProperty<bool> AutoScrollToEndProperty =
        AvaloniaProperty.RegisterAttached<ScrollViewer, bool>("AutoScrollToEnd", typeof(ScrollViewerExtensions));

    static ScrollViewerExtensions()
    {
        AutoScrollToEndProperty.Changed.Subscribe(OnAutoScrollToEndChanged);
    }

    private static void OnAutoScrollToEndChanged(AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.Sender is ScrollViewer sv && e.NewValue.Value)
        {
            sv.ScrollToEnd();
        }
    }

    public static void SetAutoScrollToEnd(AvaloniaObject element, bool value) =>
        element.SetValue(AutoScrollToEndProperty, value);

    public static bool GetAutoScrollToEnd(AvaloniaObject element) =>
        element.GetValue(AutoScrollToEndProperty);
}