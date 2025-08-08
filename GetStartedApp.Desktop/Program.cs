using Avalonia;
using GetStartedApp;
using GetStartedApp.Utils;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.MaterialDesign;
using System;
using Velopack;


namespace Avalonia.GetStartedApp.Desktop;

internal sealed class Program
{
    public static MemoryLogger Log { get; private set; } = new();

    [STAThread]
    public static void Main(string[] args)
    {
        // Initialize Velopack
        try
        {
            // It's important to Run() the VelopackApp as early as possible in app startup.
            VelopackApp.Build()
                .OnFirstRun((v) => { /* Your first run code here */ })
                .SetLogger(Log)
                .Run();

            // Now it's time to run Avalonia
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

        }
        catch (Exception ex)
        {
            string message = "Unhandled exception: " + ex.ToString();
            Console.WriteLine(message);
            throw;
        }
    }
    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current.Register<MaterialDesignIconProvider>();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }


}
