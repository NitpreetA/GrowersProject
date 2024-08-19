using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Sharpnado.Tabs;
using SkiaSharp.Views.Maui.Controls.Hosting;

/*
 * Team: Growers.IO
 * Semester: Winter 2024
 * Date: May 14, 2024
 * Course: Application Development III
 * Description: MAUI program builder
 */

namespace Growers.io
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseSharpnadoTabs(loggerEnable: false)
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiMaps();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
