using RecetteApp.Services;
using RecetteApp.ViewModels;

namespace RecetteApp.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _vm;

    public MainPage()
    {
        InitializeComponent();

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://www.themealdb.com/api/json/v1/1/"),
            Timeout = TimeSpan.FromSeconds(10)
        };

        var service = new MealService(httpClient);
        _vm = new MainViewModel(service);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _vm.LoadMeals();

        resultLabel.Text = $"✔ {_vm.Meals.Count} items reçus";
    }
}