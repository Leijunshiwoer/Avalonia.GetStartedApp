using Avalonia;
using Avalonia.Media;

namespace GetStartedApp.Browser
{
    public static class AvaloniaAppBuilderExtensions
    {
        private static string DefaultFontFamily => "avares://GetStartedApp/Assets/fonts/MiSans-Normal.ttf#MiSans";

        public static AppBuilder WithSourceHanSansCNFont(this AppBuilder builder) =>
            builder.With(new FontManagerOptions
            {
                DefaultFamilyName = DefaultFontFamily,
                FontFallbacks = [new FontFallback { FontFamily = new FontFamily(DefaultFontFamily) }]
            });
    }
}
