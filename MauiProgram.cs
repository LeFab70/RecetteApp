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
                .UseMauiApp<App>();
            builder.Services.AddHttpClient<MealService>(client =>
            {
                client.BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            builder.Services.AddSingleton<FavoritesDatabase>();
            builder.Services.AddSingleton<MealPlannerDatabase>();
            builder.Services.AddSingleton<ShoppingListStore>();

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<FavoritesViewModel>();
            builder.Services.AddTransient<FavoritesPage>();
            builder.Services.AddTransient<DetailViewModel>();
            builder.Services.AddTransient<DetailPage>();
            builder.Services.AddTransient<ShoppingListViewModel>();
            builder.Services.AddTransient<MealPlannerViewModel>();
            builder.Services.AddTransient<MealPlannerPage>();
            builder.Services.AddTransient<PreferencesViewModel>();
            builder.Services.AddTransient<PreferencesPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
