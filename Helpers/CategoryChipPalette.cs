namespace RecetteApp.Helpers;

/// <summary>Couleurs pastel distinctes pour les puces catégories (style NewsApp).</summary>
public static class CategoryChipPalette
{
    private static readonly (string Fond, string Texte)[] Paires =
    [
        ("#DBEAFE", "#1D4ED8"),
        ("#DCFCE7", "#166534"),
        ("#FEE2E2", "#B91C1C"),
        ("#FCE7F3", "#BE185D"),
        ("#FFEDD5", "#C2410C"),
        ("#EDE9FE", "#6D28D9"),
        ("#CFFAFE", "#0E7490"),
        ("#FEF3C7", "#B45309"),
        ("#E0E7FF", "#4338CA"),
        ("#DCFCE7", "#047857"),
        ("#FFE4E6", "#BE123C"),
        ("#D1FAE5", "#065F46"),
        ("#FEF9C3", "#854D0E"),
        ("#E0F2FE", "#0369A1"),
        ("#F3E8FF", "#7E22CE"),
        ("#CCFBF1", "#0F766E")
    ];

    public static Color Fond(int index)
    {
        var i = ((index % Paires.Length) + Paires.Length) % Paires.Length;
        return Color.FromArgb(Paires[i].Fond);
    }

    public static Color Texte(int index)
    {
        var i = ((index % Paires.Length) + Paires.Length) % Paires.Length;
        return Color.FromArgb(Paires[i].Texte);
    }

    public static Color FondToutes =>
        Application.Current?.RequestedTheme == AppTheme.Dark
            ? Color.FromArgb("#374151")
            : Color.FromArgb("#E5E7EB");

    public static Color TexteToutes =>
        Application.Current?.RequestedTheme == AppTheme.Dark
            ? Color.FromArgb("#F9FAFB")
            : Color.FromArgb("#1F2937");
}
