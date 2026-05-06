using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecetteApp.Models;
using RecetteApp.Services;
using System.Collections.ObjectModel;

namespace RecetteApp.ViewModels;

public partial class MealPlannerViewModel : ObservableObject
{
    private readonly FavoritesDatabase _favorites;
    private readonly MealPlannerDatabase _planner;

    private bool _chargementEnCours;

    private static readonly string[] TitresJours =
    [
        "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"
    ];

    [ObservableProperty]
    public partial ObservableCollection<FavoriteMeal> FavorisPourPicker { get; set; } = new();

    [ObservableProperty]
    public partial ObservableCollection<JourPlanVm> Jours { get; set; } = new();

    [ObservableProperty]
    public partial bool EstEnChargement { get; set; }

    /// <summary>Entrée pour vider le jour dans le Picker.</summary>
    public FavoriteMeal OptionAucune { get; } = new()
    {
        IdMeal = string.Empty,
        StrMeal = "— Aucun —",
        StrCategory = string.Empty,
        StrArea = string.Empty,
        StrMealThumb = string.Empty
    };

    public MealPlannerViewModel(FavoritesDatabase favorites, MealPlannerDatabase planner)
    {
        _favorites = favorites;
        _planner = planner;

        for (var i = 0; i < 7; i++)
        {
            var jour = new JourPlanVm(_planner, i, TitresJours[i])
            {
                EstPretPourPersistance = () => !_chargementEnCours
            };
            Jours.Add(jour);
        }
    }

    [RelayCommand]
    public async Task Charger()
    {
        try
        {
            EstEnChargement = true;
            _chargementEnCours = true;

            var favs = await _favorites.GetAll();
            FavorisPourPicker.Clear();
            FavorisPourPicker.Add(OptionAucune);
            foreach (var f in favs.OrderBy(x => x.StrMeal))
                FavorisPourPicker.Add(f);

            var semaine = await _planner.GetWeekAsync();
            foreach (var jourVm in Jours)
            {
                var row = semaine.FirstOrDefault(x => x.DayIndex == jourVm.DayIndex);
                if (row is null || string.IsNullOrWhiteSpace(row.IdMeal))
                {
                    jourVm.RepasChoisi = OptionAucune;
                    continue;
                }

                var match = FavorisPourPicker.FirstOrDefault(f => f.IdMeal == row.IdMeal);
                jourVm.RepasChoisi = match ?? new FavoriteMeal
                {
                    IdMeal = row.IdMeal,
                    StrMeal = row.StrMeal,
                    StrCategory = string.Empty,
                    StrArea = string.Empty,
                    StrMealThumb = row.StrMealThumb
                };

                if (match is null && !string.IsNullOrWhiteSpace(row.IdMeal))
                    FavorisPourPicker.Add(jourVm.RepasChoisi);
            }
        }
        finally
        {
            _chargementEnCours = false;
            EstEnChargement = false;
        }
    }
}
