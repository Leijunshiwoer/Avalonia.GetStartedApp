
using Avalonia;
using Avalonia.Controls;
using Irihi.Avalonia.Shared.Helpers;
using System;

namespace GetStartedApp.Utils
{
    public static class SizeObserver
    {
        public static readonly AttachedProperty<bool> IsObservingProperty =
            AvaloniaProperty.RegisterAttached<Control, Control, bool>("IsObserving", false);

        public static readonly AttachedProperty<Size> ObservedSizeProperty =
            AvaloniaProperty.RegisterAttached<Control, Control, Size>("ObservedSize");

        static SizeObserver()
        {
            IsObservingProperty.Changed.Subscribe(OnIsObservingChanged);
        }

        private static void OnIsObservingChanged(AvaloniaPropertyChangedEventArgs<bool> e)
        {
            if (e.Sender is Control control && e.NewValue.HasValue)
            {
                if (e.NewValue.Value)
                {
                    // 正确订阅 Bounds 变化
                    control.GetObservable(Visual.BoundsProperty).Subscribe(bounds =>
                    {
                        SetObservedSize(control, bounds.Size);
                    });
                }
            }
        }

        public static void SetIsObserving(Control element, bool value) =>
            element.SetValue(IsObservingProperty, value);

        public static bool GetIsObserving(Control element) =>
            element.GetValue(IsObservingProperty);

        public static void SetObservedSize(Control element, Size value) =>
            element.SetValue(ObservedSizeProperty, value);

        public static Size GetObservedSize(Control element) =>
            element.GetValue(ObservedSizeProperty);
    }
}

