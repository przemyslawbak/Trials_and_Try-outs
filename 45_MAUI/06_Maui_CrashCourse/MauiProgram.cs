using Microsoft.Extensions.Logging;

namespace Sample_MAUI
{
    //https://www.youtube.com/watch?v=dlnNzjc7a60&list=PLfbOp004UaYVt1En4WW3pVuM-vm66OqZe&index=4
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
