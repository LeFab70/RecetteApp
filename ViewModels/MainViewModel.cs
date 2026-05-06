using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecetteApp.Models;
using RecetteApp.Services;
using RecetteApp.Views;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace RecetteApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly MealService _mealService;
    private readonly FavoritesDatabase _favoritesDb;

    private const string CacheFileName = "meals_cache.json";
    private List<Meal> _toutesLesMeals = new();
    private HashSet<string> _idsFavoris = new(StringComparer.Ordinal);

    [ObservableProperty]
    public partial ObservableCollection<MealListRowVm> Meals { get; set; } = new();

    [ObservableProperty]
    public partial bool EstEnChargement { get; set; }

    [ObservableProperty]
    public partial bool EstFallback { get; set; }

    [ObservableProperty]
    public partial bool EstErreurVisible { get; set; }

    [ObservableProperty]
    public partial string MessageErreur { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string TexteRecherche { get; set; } = string.Empty;

    public MainViewModel(MealService mealService, FavoritesDatabase favoritesDb)
    {
        _mealService = mealService;
        _favoritesDb = favoritesDb;
    }

    [RelayCommand]
    public async Task ChargerDonnees()
    {
        try
        {
            EstEnChargement = true;
            EstFallback = false;
            EstErreurVisible = false;
            MessageErreur = string.Empty;

            Meals.Clear();
            _toutesLesMeals.Clear();

            var resultats = await _mealService.GetMeals();

            _toutesLesMeals = resultats ?? new List<Meal>();

            SauvegarderCache(_toutesLesMeals);
            await RafraichirIdsFavorisAsync();
            RafraichirListeAffichee();
        }
        catch (Exception ex)
        {
            EstFallback = true;

            var cached = await ChargerDepuisCacheOuFallbackLocal();
            _toutesLesMeals = cached;
            await RafraichirIdsFavorisAsync();
            RafraichirListeAffichee();

            EstErreurVisible = true;
            MessageErreur = $"Réseau indisponible. Données hors ligne affichées.\n{ex.Message}";
        }
        finally
        {
            EstEnChargement = false;
        }
    }

    [RelayCommand]
    private async Task Reessayer()
    {
        await ChargerDonnees();
    }

    [RelayCommand]
    private async Task OuvrirDetail(MealListRowVm? ligne)
    {
        var meal = ligne?.Recette;
        if (meal is null || string.IsNullOrWhiteSpace(meal.IdMeal))
            return;

        await Shell.Current.GoToAsync($"{nameof(DetailPage)}?IdMeal={Uri.EscapeDataString(meal.IdMeal)}");
    }

    /// <summary>Cœur : ajouter / retirer des favoris (SQLite). Pas de swipe sur cette liste.</summary>
    [RelayCommand]
    private async Task BasculerFavori(MealListRowVm? ligne)
    {
        if (ligne is null || string.IsNullOrWhiteSpace(ligne.Recette.IdMeal))
            return;

        if (ligne.EstFavori)
        {
            await _favoritesDb.Delete(ligne.Recette.IdMeal);
            ligne.EstFavori = false;
            _idsFavoris.Remove(ligne.Recette.IdMeal);
        }
        else
        {
            await _favoritesDb.AddOrReplace(ligne.Recette);
            ligne.EstFavori = true;
            _idsFavoris.Add(ligne.Recette.IdMeal);
        }
    }

    partial void OnTexteRechercheChanged(string value)
    {
        RafraichirListeAffichee();
    }

    private async Task RafraichirIdsFavorisAsync()
    {
        var favs = await _favoritesDb.GetAll();
        _idsFavoris = favs.Select(f => f.IdMeal).Where(id => !string.IsNullOrWhiteSpace(id))
            .ToHashSet(StringComparer.Ordinal);
    }

    private void RafraichirListeAffichee()
    {
        Meals.Clear();

        var q = (TexteRecherche ?? string.Empty).Trim();
        var source = _toutesLesMeals ?? new List<Meal>();

        IEnumerable<Meal> result = source;
        if (!string.IsNullOrWhiteSpace(q))
        {
            result = source.Where(m =>
                (m.StrMeal ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (m.StrCategory ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (m.StrArea ?? string.Empty).Contains(q, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var meal in result)
        {
            var estFav = !string.IsNullOrWhiteSpace(meal.IdMeal) && _idsFavoris.Contains(meal.IdMeal);
            Meals.Add(new MealListRowVm(meal, estFav));
        }
    }

    private void SauvegarderCache(List<Meal> meals)
    {
        try
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, CacheFileName);
            var json = JsonSerializer.Serialize(new MealResponse { Meals = meals }, new JsonSerializerOptions { WriteIndented = false });
            File.WriteAllText(path, json);
        }
        catch
        {
            // Best-effort
        }
    }

    private async Task<List<Meal>> ChargerDepuisCacheOuFallbackLocal()
    {
        try
        {
            var cachePath = Path.Combine(FileSystem.AppDataDirectory, CacheFileName);
            if (File.Exists(cachePath))
            {
                var json = await File.ReadAllTextAsync(cachePath);
                var fromCache = JsonSerializer.Deserialize<MealResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (fromCache?.Meals?.Count > 0)
                    return fromCache.Meals;
            }
        }
        catch
        {
            // ignore
        }

        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("fallback_meals.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var fromAsset = JsonSerializer.Deserialize<MealResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return fromAsset?.Meals ?? new List<Meal>();
        }
        catch
        {
            return new List<Meal>
            {
                new Meal { StrMeal = "Recette indisponible", StrCategory = "📴 Hors ligne" }
            };
        }
    }
}
