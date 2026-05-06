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
}

