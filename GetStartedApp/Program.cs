using Avalonia;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.MaterialDesign;
using System;
using Velopack;

namespace GetStartedApp
{
    internal sealed class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Initialize Velopack
           // VelopackApp.Build().Run();


            BuildAvaloniaApp()
          .StartWithClassicDesktopLifetime(args);
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
}
