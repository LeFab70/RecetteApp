using Microsoft.Maui.Graphics;

namespace RecetteApp.Helpers;

/// <summary>Couleurs distinctes par jour pour le planificateur (clair / sombre).</summary>
public static class WeekdayPlannerColors
{
    public static void Pour(int dayIndex, bool dark, out Color fondCarte, out Color texteTitre)
    {
        if (dark)
        {
            fondCarte = dayIndex switch
            {
                0 => Color.FromArgb("#1E3A5F"),
                1 => Color.FromArgb("#3D2E5C"),
                2 => Color.FromArgb("#1B4D3E"),
                3 => Color.FromArgb("#5C4A1E"),
                4 => Color.FromArgb("#4A2C2A"),
                5 => Color.FromArgb("#213D56"),
                _ => Color.FromArgb("#3D2E40")
            };
            texteTitre = Color.FromArgb("#FFF9C4");
        }
        else
        {
            fondCarte = dayIndex switch
            {
                0 => Color.FromArgb("#E3F2FD"),
                1 => Color.FromArgb("#F3E5F5"),
                2 => Color.FromArgb("#E8F5E9"),
                3 => Color.FromArgb("#FFF8E1"),
                4 => Color.FromArgb("#FBE9E7"),
                5 => Color.FromArgb("#E0F7FA"),
                _ => Color.FromArgb("#FCE4EC")
            };
            texteTitre = Color.FromArgb("#263238");
        }
    }
}
