namespace RecetteApp.Models;

/// <summary>Ingrédients TheMealDB (strIngredient1…20 + strMeasure1…20).</summary>
public partial class Meal
{
    public string? StrIngredient1 { get; set; }
    public string? StrIngredient2 { get; set; }
    public string? StrIngredient3 { get; set; }
    public string? StrIngredient4 { get; set; }
    public string? StrIngredient5 { get; set; }
    public string? StrIngredient6 { get; set; }
    public string? StrIngredient7 { get; set; }
    public string? StrIngredient8 { get; set; }
    public string? StrIngredient9 { get; set; }
    public string? StrIngredient10 { get; set; }
    public string? StrIngredient11 { get; set; }
    public string? StrIngredient12 { get; set; }
    public string? StrIngredient13 { get; set; }
    public string? StrIngredient14 { get; set; }
    public string? StrIngredient15 { get; set; }
    public string? StrIngredient16 { get; set; }
    public string? StrIngredient17 { get; set; }
    public string? StrIngredient18 { get; set; }
    public string? StrIngredient19 { get; set; }
    public string? StrIngredient20 { get; set; }

    public string? StrMeasure1 { get; set; }
    public string? StrMeasure2 { get; set; }
    public string? StrMeasure3 { get; set; }
    public string? StrMeasure4 { get; set; }
    public string? StrMeasure5 { get; set; }
    public string? StrMeasure6 { get; set; }
    public string? StrMeasure7 { get; set; }
    public string? StrMeasure8 { get; set; }
    public string? StrMeasure9 { get; set; }
    public string? StrMeasure10 { get; set; }
    public string? StrMeasure11 { get; set; }
    public string? StrMeasure12 { get; set; }
    public string? StrMeasure13 { get; set; }
    public string? StrMeasure14 { get; set; }
    public string? StrMeasure15 { get; set; }
    public string? StrMeasure16 { get; set; }
    public string? StrMeasure17 { get; set; }
    public string? StrMeasure18 { get; set; }
    public string? StrMeasure19 { get; set; }
    public string? StrMeasure20 { get; set; }

    /// <summary>Lignes « mesure + ingrédient » pour affichage et liste de courses.</summary>
    public IReadOnlyList<string> GetIngredientLines()
    {
        var list = new List<string>();

        void Add(string? ingredient, string? measure)
        {
            if (string.IsNullOrWhiteSpace(ingredient))
                return;
            var m = string.IsNullOrWhiteSpace(measure) ? string.Empty : measure.Trim() + " ";
            list.Add($"{m}{ingredient.Trim()}".Trim());
        }

        Add(StrIngredient1, StrMeasure1);
        Add(StrIngredient2, StrMeasure2);
        Add(StrIngredient3, StrMeasure3);
        Add(StrIngredient4, StrMeasure4);
        Add(StrIngredient5, StrMeasure5);
        Add(StrIngredient6, StrMeasure6);
        Add(StrIngredient7, StrMeasure7);
        Add(StrIngredient8, StrMeasure8);
        Add(StrIngredient9, StrMeasure9);
        Add(StrIngredient10, StrMeasure10);
        Add(StrIngredient11, StrMeasure11);
        Add(StrIngredient12, StrMeasure12);
        Add(StrIngredient13, StrMeasure13);
        Add(StrIngredient14, StrMeasure14);
        Add(StrIngredient15, StrMeasure15);
        Add(StrIngredient16, StrMeasure16);
        Add(StrIngredient17, StrMeasure17);
        Add(StrIngredient18, StrMeasure18);
        Add(StrIngredient19, StrMeasure19);
        Add(StrIngredient20, StrMeasure20);

        return list;
    }
}
