namespace RecetteApp.Services;

public static class ThemeHelper
{
    public const string PreferenceKey = "app_theme";

    public static void ApplySavedTheme()
    {
        var v = Preferences.Get(PreferenceKey, "System");
        if (Application.Current is null)
            return;

        Application.Current.UserAppTheme = v switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }

    public static string GetSavedThemeLabel() => Preferences.Get(PreferenceKey, "System");
}
