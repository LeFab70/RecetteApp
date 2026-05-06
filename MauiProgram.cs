using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RecetteApp.Services;
using RecetteApp.ViewModels;
using RecetteApp.Views;

namespace RecetteApp
{
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
            builder.Services.AddHttpClient<MealService>(client =>
            {
                client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
