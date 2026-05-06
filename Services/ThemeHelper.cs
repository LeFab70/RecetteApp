namespace RecetteApp.Services;

public static class ThemeHelper
{
    public const string PreferenceKey = "app_theme";

    /// <summary>À appeler depuis le constructeur <see cref="Application"/> : <c>ApplySavedTheme(this)</c>.</summary>
    public static void ApplySavedTheme(Application app)
    {
        var v = Preferences.Get(PreferenceKey, "System");
        app.UserAppTheme = v switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }

    public static void ApplySavedTheme()
    {
        if (Application.Current is null)
            return;

        ApplySavedTheme(Application.Current);
    }

    public static string GetSavedThemeLabel() => Preferences.Get(PreferenceKey, "System");
}
