namespace RecetteApp.Models;

/// <summary>Réponse TheMealDB pour categories.php.</summary>
public class MealCategoriesResponse
{
    public List<MealCategoryItem>? Categories { get; set; }
}

public class MealCategoryItem
{
    public string? IdCategory { get; set; }

    public string? StrCategory { get; set; }

    public string? StrCategoryDescription { get; set; }

    public string? StrCategoryThumb { get; set; }
}
