using RecetteApp.ViewModels;

namespace RecetteApp.Views;

public partial class PreferencesPage : ContentPage
{
    public PreferencesPage(PreferencesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
