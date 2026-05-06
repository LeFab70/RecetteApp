using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using RecetteApp.Helpers;
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
        AppliquerPalette(Application.Current?.RequestedTheme == AppTheme.Dark);
    }

    [ObservableProperty]
    public partial Color CouleurFondCarte { get; set; }

    [ObservableProperty]
    public partial Color CouleurTexteTitre { get; set; }

    public void AppliquerPalette(bool themeSombre)
    {
        WeekdayPlannerColors.Pour(DayIndex, themeSombre, out var fond, out var titreCouleur);
        CouleurFondCarte = fond;
        CouleurTexteTitre = titreCouleur;
    }

    public int DayIndex { get; }

    public string Titre { get; }

    public bool AUnRepasPlanifie => RepasChoisi is not null && !RepasChoisi.EstAucune;

    /// <summary>Repas choisi ; doit être une entrée de la collection partagée du planificateur.</summary>
    [ObservableProperty]
    public partial RepasPickerOption? RepasChoisi { get; set; }

    partial void OnRepasChoisiChanged(RepasPickerOption? value)
    {
        OnPropertyChanged(nameof(AUnRepasPlanifie));
        if (!EstPretPourPersistance())
            return;

        _ = PersisterRepasAsync(value);
    }

    /// <summary>Synchronise SQLite avec l’état actuel (utilisé après chargement sans notifier pendant la phase batch).</summary>
    public Task PersisterEtatAsync() => PersisterRepasAsync(RepasChoisi);

    private async Task PersisterRepasAsync(RepasPickerOption? value)
    {
        try
        {
            if (value is null || value.EstAucune)
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
