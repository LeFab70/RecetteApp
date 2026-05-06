using System.Text.RegularExpressions;

namespace RecetteApp.Helpers;

public static class RecetteTexteHelper
{
    /// <summary>Texte court sur une ligne pour les cartes (suite dans Détail).</summary>
    public static string ApercuInstructions(string? instructions, int maxChars = 110)
    {
        if (string.IsNullOrWhiteSpace(instructions))
            return string.Empty;

        var compact = Regex.Replace(instructions.Trim(), @"[\r\n]+", " ");
        compact = Regex.Replace(compact, @"\s+", " ");

        if (compact.Length <= maxChars)
            return compact;

        return compact[..maxChars].TrimEnd() + "…";
    }
}
