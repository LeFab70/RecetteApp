using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecetteApp.Models;
using RecetteApp.Services;
using RecetteApp.Views;
using System.Collections.ObjectModel;

namespace RecetteApp.ViewModels;

public partial class FavoritesViewModel : ObservableObject
{
    private readonly FavoritesDatabase _db;

    [ObservableProperty]
    public partial ObservableCollection<FavoriteMeal> Favoris { get; set; } = new();

    [ObservableProperty]
    public partial bool EstEnChargement { get; set; }

    public FavoritesViewModel(FavoritesDatabase db)
    {
        _db = db;
    }

    [RelayCommand]
    public async Task Charger()
    {
        try
        {
            EstEnChargement = true;
            Favoris.Clear();

            var items = await _db.GetAll();
            foreach (var item in items)
                Favoris.Add(item);
        }
        finally
        {
            EstEnChargement = false;
        }
    }

    [RelayCommand]
    private async Task OuvrirDetail(FavoriteMeal? fav)
    {
        if (fav is null || string.IsNullOrWhiteSpace(fav.IdMeal))
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?IdMeal={Uri.EscapeDataString(fav.IdMeal)}");
    }

    [RelayCommand]
    private async Task Supprimer(FavoriteMeal fav)
    {
        if (fav is null || string.IsNullOrWhiteSpace(fav.IdMeal))
            return;

        await _db.Delete(fav.IdMeal);
        Favoris.Remove(fav);
    }

    /// <summary>Cœur plein : retire des favoris (comme supprimer, sans swipe).</summary>
    [RelayCommand]
    private async Task RetirerFavoriCoeur(FavoriteMeal? fav)
    {
        if (fav is null || string.IsNullOrWhiteSpace(fav.IdMeal))
            return;

        await _db.Delete(fav.IdMeal);
        Favoris.Remove(fav);
    }
}

