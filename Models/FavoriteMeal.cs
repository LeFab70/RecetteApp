using SQLite;

namespace RecetteApp.Models;

public class FavoriteMeal
{
    [PrimaryKey]
    public string IdMeal { get; set; } = string.Empty;

    public string StrMeal { get; set; } = string.Empty;

    public string StrCategory { get; set; } = string.Empty;

    public string StrArea { get; set; } = string.Empty;

    public string StrMealThumb { get; set; } = string.Empty;

    /// <summary>Extrait des instructions (carte favoris).</summary>
    public string StrSnippet { get; set; } = string.Empty;

    [Ignore]
    public string TexteApercuAffiche =>
        string.IsNullOrWhiteSpace(StrSnippet)
            ? "Voir la recette complète dans Détail…"
            : StrSnippet;
}

