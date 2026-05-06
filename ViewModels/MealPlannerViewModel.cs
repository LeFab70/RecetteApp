using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using RecetteApp.Models;
using RecetteApp.Services;
using System.Collections.ObjectModel;

namespace RecetteApp.ViewModels;

public partial class MealPlannerViewModel : ObservableObject
{
    private readonly MealService _mealService;
    private readonly MealPlannerDatabase _planner;

    private bool _chargementListeInitial;
    private bool _chargementListePlusEnCours;
    private readonly Queue<(bool ParCategorie, string Token)> _fileChargement = new();
    private readonly HashSet<string> _idsDejaCharges = new(StringComparer.Ordinal);
    private bool _plusDeLotsDispo;

    private bool _chargementSemaineEnCours;

    private static readonly string[] TitresJours =
    [
        "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"
    ];

    [ObservableProperty]
    public partial ObservableCollection<JourPlanVm> Jours { get; set; } = new();

    /// <summary>Repas affichés en vignettes ; complété au scroll (<see cref="ChargerPlusRepasAsync"/>).</summary>
    [ObservableProperty]
    public partial ObservableCollection<PlannerMealTileVm> TuilesRepas { get; set; } = new();

    [ObservableProperty]
    public partial bool EstEnChargement { get; set; }

    /// <summary>Chargement d’un lot supplémentaire (pagination).</summary>
    [ObservableProperty]
    public partial bool EstChargementListePlus { get; set; }

    [ObservableProperty]
    public partial string? ErreurListeRepas { get; set; }

    public bool AErreurListe => !string.IsNullOrWhiteSpace(ErreurListeRepas);

    partial void OnErreurListeRepasChanged(string? value) => OnPropertyChanged(nameof(AErreurListe));

    [ObservableProperty]
    public partial bool PlusDeRepasDisponibles { get; set; }

    [ObservableProperty]
    public partial JourPlanVm? JourSelectionne { get; set; }

    public FavoriteMeal OptionAucune { get; } = new()
    {
        IdMeal = string.Empty,
        StrMeal = "— Aucun —",
        StrCategory = string.Empty,
        StrArea = string.Empty,
        StrMealThumb = string.Empty
    };

    public MealPlannerViewModel(MealService mealService, MealPlannerDatabase planner)
    {
        _mealService = mealService;
        _planner = planner;

        for (var i = 0; i < 7; i++)
        {
            var jour = new JourPlanVm(_planner, i, TitresJours[i])
            {
                EstPretPourPersistance = () => !_chargementSemaineEnCours
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
            _chargementSemaineEnCours = true;

            RafraichirCouleursJours();

            var semaine = await _planner.GetWeekAsync();
            foreach (var jourVm in Jours)
            {
                var row = semaine.FirstOrDefault(x => x.DayIndex == jourVm.DayIndex);
                if (row is null || string.IsNullOrWhiteSpace(row.IdMeal))
                {
                    jourVm.RepasChoisi = OptionAucune;
                    continue;
                }

                jourVm.RepasChoisi = new FavoriteMeal
                {
                    IdMeal = row.IdMeal,
                    StrMeal = row.StrMeal,
                    StrCategory = string.Empty,
                    StrArea = string.Empty,
                    StrMealThumb = row.StrMealThumb ?? string.Empty
                };
            }

            await ReinitialiserTuilesRepasAsync();

            if (JourSelectionne is null)
                JourSelectionne = Jours[0];
            else
            {
                var encoreLa = Jours.FirstOrDefault(j => j.DayIndex == JourSelectionne.DayIndex);
                JourSelectionne = encoreLa ?? Jours[0];
            }
        }
        catch (Exception ex)
        {
            ErreurListeRepas = $"Plan : {ex.Message}";
        }
        finally
        {
            _chargementSemaineEnCours = false;
            EstEnChargement = false;
        }
    }

    partial void OnJourSelectionneChanged(JourPlanVm? value)
    {
        foreach (var j in Jours)
            j.EstSelectionne = value is not null && ReferenceEquals(j, value);
    }

    private void RafraichirCouleursJours()
    {
        var sombre = Application.Current?.RequestedTheme == AppTheme.Dark;
        foreach (var j in Jours)
            j.AppliquerPalette(sombre);
    }

    /// <summary>Vide et recharge la grille TheMealDB (sans toucher au plan SQLite).</summary>
    private async Task ReinitialiserTuilesRepasAsync()
    {
        ErreurListeRepas = null;
        _plusDeLotsDispo = false;
        PlusDeRepasDisponibles = false;
        _idsDejaCharges.Clear();
        TuilesRepas.Clear();
        _fileChargement.Clear();

        try
        {
            var categories = await _mealService.GetCategoryNamesAsync();
            if (categories.Count > 0)
            {
                foreach (var c in categories)
                    _fileChargement.Enqueue((true, c));
            }
            else
            {
                const string alphabet = "abcdefghijklmnopqrstuvwxyz";
                foreach (var ch in alphabet)
                    _fileChargement.Enqueue((false, ch.ToString()));
            }
        }
        catch (Exception ex)
        {
            ErreurListeRepas = ex.Message;
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            foreach (var ch in alphabet)
                _fileChargement.Enqueue((false, ch.ToString()));
        }

        _chargementListeInitial = true;
        try
        {
            await ChargerPlusRepasAsync();
            await ChargerPlusRepasAsync();
        }
        finally
        {
            _chargementListeInitial = false;
        }
    }

    [RelayCommand]
    public async Task RafraichirListeRepasAsync()
    {
        try
        {
            EstEnChargement = true;
            await ReinitialiserTuilesRepasAsync();
        }
        catch (Exception ex)
        {
            ErreurListeRepas = ex.Message;
        }
        finally
        {
            EstEnChargement = false;
        }
    }

    /// <summary>Appelé quand l’utilisateur approche du bas de la grille des repas.</summary>
    [RelayCommand]
    public async Task ChargerPlusRepasAsync()
    {
        if (_plusDeLotsDispo || _chargementListePlusEnCours)
            return;

        _chargementListePlusEnCours = true;
        if (!_chargementListeInitial)
            EstChargementListePlus = true;

        try
        {
            while (_fileChargement.Count > 0)
            {
                var (parCat, token) = _fileChargement.Dequeue();
                List<Meal> lot;
                try
                {
                    lot = parCat
                        ? await _mealService.FilterMealsByCategoryAsync(token)
                        : await _mealService.SearchMealsAsync(token);
                }
                catch (Exception ex)
                {
                    ErreurListeRepas = ex.Message;
                    continue;
                }

                var nouveaux = new List<PlannerMealTileVm>();
                foreach (var m in lot)
                {
                    var id = m.IdMeal.Trim();
                    if (_idsDejaCharges.Contains(id))
                        continue;
                    _idsDejaCharges.Add(id);
                    nouveaux.Add(new PlannerMealTileVm(id, m.StrMeal ?? "", m.StrMealThumb ?? ""));
                }

                if (nouveaux.Count > 0)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var t in nouveaux)
                            TuilesRepas.Add(t);
                    });

                    PlusDeRepasDisponibles = false;
                    return;
                }
            }

            _plusDeLotsDispo = true;
            PlusDeRepasDisponibles = true;
        }
        finally
        {
            _chargementListePlusEnCours = false;
            EstChargementListePlus = false;
        }
    }

    [RelayCommand]
    public void SelectionnerJour(JourPlanVm? jour)
    {
        if (jour is null)
            return;

        JourSelectionne = jour;
    }

    [RelayCommand]
    public void EffacerJour(JourPlanVm? jour)
    {
        if (jour is null)
            return;

        jour.RepasChoisi = OptionAucune;
    }

    [RelayCommand]
    public void AssignerTuileAuJourSelectionne(PlannerMealTileVm? tuile)
    {
        if (tuile is null || JourSelectionne is null || string.IsNullOrWhiteSpace(tuile.IdMeal))
            return;

        JourSelectionne.RepasChoisi = new FavoriteMeal
        {
            IdMeal = tuile.IdMeal,
            StrMeal = tuile.StrMeal,
            StrCategory = string.Empty,
            StrArea = string.Empty,
            StrMealThumb = tuile.StrMealThumb
        };
    }
}
