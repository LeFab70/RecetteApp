using RecetteApp.ViewModels;

namespace RecetteApp.Views;

public partial class MealPlannerPage : ContentPage
{
    private readonly MealPlannerViewModel _vm;

    public MealPlannerPage(MealPlannerViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _vm.Charger();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Planificateur", $"Erreur au chargement : {ex.Message}", "OK");
        }
    }
}
