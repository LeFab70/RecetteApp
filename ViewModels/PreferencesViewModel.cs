using CommunityToolkit.Mvvm.ComponentModel;
using RecetteApp.Services;

namespace RecetteApp.ViewModels;

public partial class PreferencesViewModel : ObservableObject
{
    public List<ThemeOptionVm> ThemesDisponibles { get; } =
    [
        new ThemeOptionVm("System", "Système (suivre l’appareil)"),
        new ThemeOptionVm("Light", "Clair"),
        new ThemeOptionVm("Dark", "Sombre")
    ];

    [ObservableProperty]
    public partial ThemeOptionVm? ThemeChoisi { get; set; }

    [ObservableProperty]
    public partial string VersionApplication { get; set; } = string.Empty;

    public string DescriptionApplication => RecetteApp.Constants.AppDescription;

    public PreferencesViewModel()
    {
        VersionApplication = $"{AppInfo.VersionString} (build {AppInfo.BuildString})";
        var cle = ThemeHelper.GetSavedThemeLabel();
        ThemeChoisi = ThemesDisponibles.FirstOrDefault(t => t.Key == cle) ?? ThemesDisponibles[0];
    }

    partial void OnThemeChoisiChanged(ThemeOptionVm? value)
    {
        if (value is null)
            return;

        Preferences.Set(ThemeHelper.PreferenceKey, value.Key);
        ThemeHelper.ApplySavedTheme();
    }
}

public sealed record ThemeOptionVm(string Key, string Label);
