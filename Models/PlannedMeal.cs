using SQLite;

namespace RecetteApp.Models;

/// <summary>Un jour de la semaine (0 = lundi … 6 = dimanche) et la recette assignée.</summary>
public class PlannedMeal
{
    [PrimaryKey]
    public int DayIndex { get; set; }

    public string IdMeal { get; set; } = string.Empty;

    public string StrMeal { get; set; } = string.Empty;

    public string StrMealThumb { get; set; } = string.Empty;
}
