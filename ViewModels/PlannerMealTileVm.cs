namespace RecetteApp.ViewModels;

/// <summary>Vignette repas pour le planificateur (liste paginée).</summary>
public sealed class PlannerMealTileVm
{
    public PlannerMealTileVm(string idMeal, string strMeal, string strMealThumb)
    {
        IdMeal = idMeal;
        StrMeal = strMeal;
        StrMealThumb = strMealThumb;
    }

    public string IdMeal { get; }
    public string StrMeal { get; }
    public string StrMealThumb { get; }
}
