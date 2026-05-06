using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecetteApp.Models;
using RecetteApp.Services;
using System.Collections.ObjectModel;

namespace RecetteApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly MealService _mealService;

    public ObservableCollection<Meal> Meals { get; set; } = new();

    [ObservableProperty]
    public partial bool EstEnChargement { get; set; }

    [ObservableProperty]
    public partial bool EstFallback { get; set; }

    public MainViewModel(MealService mealService)
    {
        _mealService = mealService;
    }

    [RelayCommand]
    public async Task ChargerDonnees()
    {
        try
        {
            EstEnChargement = true;
            EstFallback = false;

            Meals.Clear();

            var resultats = await _mealService.GetMeals();

            foreach (var meal in resultats)
            {
                Meals.Add(meal);
            }
        }
        catch
        {
            EstFallback = true;

            Meals.Clear();

            Meals.Add(new Meal
            {
                StrMeal = "Recette indisponible",
                StrCategory = "Offline",
                StrMealThumb = ""
            });
        }
        finally
        {
            EstEnChargement = false;
        }
    }
}