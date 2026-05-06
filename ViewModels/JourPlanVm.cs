using CommunityToolkit.Mvvm.ComponentModel;
using RecetteApp.Models;
using RecetteApp.Services;

namespace RecetteApp.ViewModels;

public partial class JourPlanVm : ObservableObject
{
    private readonly MealPlannerDatabase _db;

    /// <summary>False pendant le chargement initial pour éviter d’effacer SQLite.</summary>
    public Func<bool> EstPretPourPersistance { get; set; } = () => true;

    public JourPlanVm(MealPlannerDatabase db, int dayIndex, string titre)
    {
        _db = db;
        DayIndex = dayIndex;
        Titre = titre;
    }

    public int DayIndex { get; }

    public string Titre { get; }

    /// <summary>Favori choisi pour ce jour (référence partagée avec la liste du parent).</summary>
    [ObservableProperty]
    public partial FavoriteMeal? RepasChoisi { get; set; }

    partial void OnRepasChoisiChanged(FavoriteMeal? value)
    {
        if (!EstPretPourPersistance())
            return;

        _ = PersisterRepasAsync(value);
    }

    private async Task PersisterRepasAsync(FavoriteMeal? value)
    {
        try
        {
            if (value is null || string.IsNullOrWhiteSpace(value.IdMeal))
                await _db.ClearDayAsync(DayIndex);
            else
                await _db.UpsertAsync(DayIndex, value.IdMeal, value.StrMeal, value.StrMealThumb);
        }
        catch
        {
            // Ignorer : pas bloquant pour l’UI
        }
    }
}
