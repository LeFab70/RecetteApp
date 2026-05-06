using CommunityToolkit.Mvvm.ComponentModel;
using RecetteApp.Models;

namespace RecetteApp.ViewModels;

/// <summary>Ligne recette sur la page principale : favori via cœur (pas de swipe).</summary>
public partial class MealListRowVm : ObservableObject
{
    public Meal Recette { get; }

    [ObservableProperty]
    public partial bool EstFavori { get; set; }

    public MealListRowVm(Meal recette, bool estFavori)
    {
        Recette = recette;
        EstFavori = estFavori;
    }

    public bool PeutFavoriser => !string.IsNullOrWhiteSpace(Recette.IdMeal);

    public string SymboleCoeur => EstFavori ? "♥" : "♡";

    partial void OnEstFavoriChanged(bool value)
    {
        OnPropertyChanged(nameof(SymboleCoeur));
        OnPropertyChanged(nameof(CouleurCoeur));
    }

    public Color CouleurCoeur => EstFavori
        ? Color.FromArgb("#C62828")
        : Application.Current?.RequestedTheme == AppTheme.Dark
            ? Color.FromArgb("#B0B0B0")
            : Color.FromArgb("#757575");
}
