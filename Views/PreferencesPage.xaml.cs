using RecetteApp.ViewModels;

namespace RecetteApp.Views;

public partial class PreferencesPage : ContentPage
{
    private readonly ShoppingListViewModel _coursesVm;

    public PreferencesPage(PreferencesViewModel prefsVm, ShoppingListViewModel coursesVm)
    {
        InitializeComponent();
        BindingContext = prefsVm;
        _coursesVm = coursesVm;
        ShoppingRoot.BindingContext = _coursesVm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _coursesVm.Charger();
    }
}
