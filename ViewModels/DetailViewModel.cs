using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using RecetteApp.Models;
using RecetteApp.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace RecetteApp.ViewModels;

[QueryProperty(nameof(IdMeal), "IdMeal")]
public partial class DetailViewModel : ObservableObject
{
    private const string CacheFileName = "meals_cache.json";

    private readonly MealService _mealService;
    private readonly ShoppingListStore _courses;

    [ObservableProperty]
    public partial string IdMeal { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Meal? Recette { get; set; }

    [ObservableProperty]
    public partial bool EstEnChargement { get; set; }

    [ObservableProperty]
    public partial bool EstErreur { get; set; }

    [ObservableProperty]
    public partial string MessageErreur { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool AVideoYoutube { get; set; }

    [ObservableProperty]
    public partial bool AfficheContenu { get; set; }

    public ObservableCollection<string> Ingredients { get; } = new();

    public ObservableCollection<string> Etapes { get; } = new();

    public DetailViewModel(MealService mealService, ShoppingListStore courses)
    {
        _mealService = mealService;
        _courses = courses;
    }

    partial void OnIdMealChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
            _ = ChargerAsync();
    }

    private async Task ChargerAsync()
    {
        EstEnChargement = true;
        AfficheContenu = false;
        EstErreur = false;
        MessageErreur = string.Empty;
        Recette = null;
        AVideoYoutube = false;
        Ingredients.Clear();
        Etapes.Clear();

        try
        {
            Meal? m = null;
            try
            {
                m = await _mealService.GetMealByIdAsync(IdMeal);
            }
            catch
            {
                m = await ChargerDepuisSourcesLocalesAsync(IdMeal);
            }

            if (m is null)
            {
                EstErreur = true;
                MessageErreur = "Recette introuvable (hors ligne ou identifiant invalide).";
                return;
            }

            Recette = m;
            AfficheContenu = true;
            AVideoYoutube = !string.IsNullOrWhiteSpace(m.StrYoutube);

            foreach (var line in m.GetIngredientLines())
                Ingredients.Add(line);

            foreach (var step in DecouperEtapes(m.StrInstructions))
                Etapes.Add(step);
        }
        finally
        {
            EstEnChargement = false;
        }
    }

    private static IEnumerable<string> DecouperEtapes(string instructions)
    {
        if (string.IsNullOrWhiteSpace(instructions))
            yield break;

        var lignes = instructions.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        var i = 1;
        foreach (var raw in lignes)
        {
            var s = raw.Trim();
            if (s.Length == 0)
                continue;
            yield return $"{i}. {s}";
            i++;
        }
    }

    private static async Task<Meal?> ChargerDepuisSourcesLocalesAsync(string idMeal)
    {
        try
        {
            var cachePath = Path.Combine(FileSystem.AppDataDirectory, CacheFileName);
            if (File.Exists(cachePath))
            {
                var json = await File.ReadAllTextAsync(cachePath);
                var fromCache = JsonSerializer.Deserialize<MealResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var hit = fromCache?.Meals?.FirstOrDefault(x => x.IdMeal == idMeal);
                if (hit is not null)
                    return hit;
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
            var fromAsset = JsonSerializer.Deserialize<MealResponse>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return fromAsset?.Meals?.FirstOrDefault(x => x.IdMeal == idMeal);
        }
        catch
        {
            return null;
        }
    }

    [RelayCommand]
    private async Task Partager()
    {
        if (Recette is null)
            return;

        var text = $"{Recette.StrMeal}\n\n";
        if (!string.IsNullOrWhiteSpace(Recette.StrYoutube))
            text += $"{Recette.StrYoutube}\n\n";
        text += Recette.StrInstructions;

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Title = Recette.StrMeal,
            Text = text,
            Subject = Recette.StrMeal
        });
    }

    [RelayCommand]
    private async Task OuvrirYoutube()
    {
        if (Recette is null || string.IsNullOrWhiteSpace(Recette.StrYoutube))
            return;

        try
        {
            await Launcher.Default.OpenAsync(new Uri(Recette.StrYoutube));
        }
        catch
        {
            await Shell.Current.DisplayAlertAsync("Vidéo", "Impossible d’ouvrir le lien YouTube.", "OK");
        }
    }

    [RelayCommand]
    private async Task AjouterAListeCourses()
    {
        if (Recette is null)
            return;

        _courses.MergeIngredientsFromMeal(Recette);
        await Shell.Current.DisplayAlertAsync("Liste de courses", "Les ingrédients ont été ajoutés (sans doublons).", "OK");
    }
}
