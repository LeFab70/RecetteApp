namespace RecetteApp.ViewModels;

/// <summary>Entrée affichée dans les Pickers du planificateur.</summary>
public sealed class RepasPickerOption
{
    public RepasPickerOption(string idMeal, string strMeal, string strMealThumb)
    {
        IdMeal = idMeal ?? string.Empty;
        StrMeal = strMeal ?? string.Empty;
        StrMealThumb = strMealThumb ?? string.Empty;
    }

    public string IdMeal { get; }
    public string StrMeal { get; }
    public string StrMealThumb { get; }

    public bool EstAucune => string.IsNullOrWhiteSpace(IdMeal);
}
