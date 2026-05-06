using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using RecetteApp.Models;
using RecetteApp.Services;

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

    /// <summary>Liste partagée pour tous les Pickers (complétée par <see cref="ChargerPlusRepasAsync"/>).</summary>
    [ObservableProperty]
    public partial ObservableCollection<RepasPickerOption> OptionsRepas { get; set; } = new();

    public RepasPickerOption OptionAucune { get; } = new(string.Empty, "— Aucun —", string.Empty);

    [ObservableProperty]
    public partial bool EstEnChargement { get; set; }

    [ObservableProperty]
    public partial bool EstChargementListePlus { get; set; }

    [ObservableProperty]
    public partial string? ErreurListeRepas { get; set; }

    public bool AErreurListe => !string.IsNullOrWhiteSpace(ErreurListeRepas);

    partial void OnErreurListeRepasChanged(string? value) => OnPropertyChanged(nameof(AErreurListe));

    [ObservableProperty]
    public partial bool PlusDeRepasDisponibles { get; set; }

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

            foreach (var j in Jours)
                j.RepasChoisi = OptionAucune;

            await ReinitialiserOptionsRepasAsync();

            foreach (var jourVm in Jours)
            {
                var row = semaine.FirstOrDefault(x => x.DayIndex == jourVm.DayIndex);
                if (row is null || string.IsNullOrWhiteSpace(row.IdMeal))
                {
                    jourVm.RepasChoisi = OptionAucune;
                    continue;
                }

                var opt = OptionsRepas.FirstOrDefault(o => o.IdMeal == row.IdMeal);
                if (opt is null)
                {
                    opt = new RepasPickerOption(row.IdMeal, row.StrMeal ?? "", row.StrMealThumb ?? "");
                    OptionsRepas.Add(opt);
                }

                jourVm.RepasChoisi = opt;
            }

            _chargementSemaineEnCours = false;

            foreach (var j in Jours)
                await j.PersisterEtatAsync();
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

    private void RafraichirCouleursJours()
    {
        var sombre = Application.Current?.RequestedTheme == AppTheme.Dark;
        foreach (var j in Jours)
            j.AppliquerPalette(sombre);
    }

    private async Task ReinitialiserOptionsRepasAsync()
    {
        ErreurListeRepas = null;
        _plusDeLotsDispo = false;
        PlusDeRepasDisponibles = false;
        _idsDejaCharges.Clear();
        OptionsRepas.Clear();
        OptionsRepas.Add(OptionAucune);
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
            _chargementSemaineEnCours = true;

            var snapshots = Jours.Select(j =>
            {
                var r = j.RepasChoisi;
                return (
                    j.DayIndex,
                    Id: r?.IdMeal ?? string.Empty,
                    Nom: r?.StrMeal ?? string.Empty,
                    Thumb: r?.StrMealThumb ?? string.Empty);
            }).ToList();

            foreach (var j in Jours)
                j.RepasChoisi = OptionAucune;

            await ReinitialiserOptionsRepasAsync();

            foreach (var s in snapshots)
            {
                var jourVm = Jours.First(j => j.DayIndex == s.DayIndex);
                if (string.IsNullOrWhiteSpace(s.Id))
                {
                    jourVm.RepasChoisi = OptionAucune;
                    continue;
                }

                var opt = OptionsRepas.FirstOrDefault(o => o.IdMeal == s.Id);
                if (opt is null)
                {
                    opt = new RepasPickerOption(s.Id, s.Nom, s.Thumb);
                    OptionsRepas.Add(opt);
                }

                jourVm.RepasChoisi = opt;
            }

            _chargementSemaineEnCours = false;
            foreach (var j in Jours)
                await j.PersisterEtatAsync();
        }
        catch (Exception ex)
        {
            ErreurListeRepas = ex.Message;
        }
        finally
        {
            _chargementSemaineEnCours = false;
            EstEnChargement = false;
        }
    }

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

                var nouveaux = new List<RepasPickerOption>();
                foreach (var m in lot)
                {
                    var id = m.IdMeal.Trim();
                    if (_idsDejaCharges.Contains(id))
                        continue;
                    _idsDejaCharges.Add(id);
                    nouveaux.Add(new RepasPickerOption(id, m.StrMeal ?? "", m.StrMealThumb ?? ""));
                }

                if (nouveaux.Count > 0)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var t in nouveaux)
                            OptionsRepas.Add(t);
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
    public void EffacerJour(JourPlanVm? jour)
    {
        if (jour is null)
            return;

        jour.RepasChoisi = OptionAucune;
    }
}
