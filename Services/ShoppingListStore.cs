using System.Text.Json;
using RecetteApp.Models;
using static RecetteApp.Constants;

namespace RecetteApp.Services;

/// <summary>Liste de courses persistée dans Preferences (JSON).</summary>
public class ShoppingListStore
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public List<ShoppingItemDto> Load()
    {
        try
        {
            var json = Preferences.Get(ShoppingListPreferenceKey, string.Empty);
            if (string.IsNullOrWhiteSpace(json))
                return new List<ShoppingItemDto>();

            var list = JsonSerializer.Deserialize<List<ShoppingItemDto>>(json, JsonOptions);
            return list ?? new List<ShoppingItemDto>();
        }
        catch
        {
            return new List<ShoppingItemDto>();
        }
    }

    public void Save(IEnumerable<ShoppingItemDto> items)
    {
        var list = items.ToList();
        var json = JsonSerializer.Serialize(list, JsonOptions);
        Preferences.Set(ShoppingListPreferenceKey, json);
    }

    /// <summary>Ajoute les ingrédients d'une recette sans doublon (texte identique ignoré).</summary>
    public void MergeIngredientsFromMeal(Meal meal)
    {
        var existing = Load();
        var texts = new HashSet<string>(existing.Select(x => Normalize(x.Text)), StringComparer.OrdinalIgnoreCase);

        foreach (var line in meal.GetIngredientLines())
        {
            var n = Normalize(line);
            if (string.IsNullOrEmpty(n) || texts.Contains(n))
                continue;
            texts.Add(n);
            existing.Add(new ShoppingItemDto { Text = line.Trim(), IsChecked = false });
        }

        Save(existing);
    }

    private static string Normalize(string? s) => (s ?? string.Empty).Trim().ToLowerInvariant();
}
