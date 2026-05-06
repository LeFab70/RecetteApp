using CommunityToolkit.Mvvm.ComponentModel;

namespace RecetteApp.ViewModels;

/// <summary>Puce catégorie sur l’accueil.</summary>
public partial class CategoryChipVm : ObservableObject
{
    public CategoryChipVm(string cle, string libelle, Color fond, Color couleurTexte, int paletteIndex)
    {
        Cle = cle;
        Libelle = libelle;
        Fond = fond;
        CouleurTexte = couleurTexte;
        PaletteIndex = paletteIndex;
    }

    /// <summary>Valeur pour filtrer (nom TheMealDB égal à StrCategory sur Meal); « Toutes » = pas de filtre.</summary>
    public string Cle { get; }

    public string Libelle { get; }

    public Color Fond { get; }

    public Color CouleurTexte { get; }

    public int PaletteIndex { get; }

    [ObservableProperty]
    public partial bool EstSelectionnee { get; set; }

    public double EpaisseurBordure => EstSelectionnee ? 2 : 0;

    public Color CouleurBordure =>
        EstSelectionnee ? Color.FromArgb(Application.Current?.RequestedTheme == AppTheme.Dark ? "#F9FAFB" : "#111827") : Colors.Transparent;

    partial void OnEstSelectionneeChanged(bool value)
    {
        OnPropertyChanged(nameof(EpaisseurBordure));
        OnPropertyChanged(nameof(CouleurBordure));
    }
}
