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
        await _vm.Charger();
    }

    private void OnRepasRemainingThreshold(object? sender, EventArgs e)
    {
        if (_vm.ChargerPlusRepasCommand.CanExecute(null))
            _ = _vm.ChargerPlusRepasCommand.ExecuteAsync(null);
    }
}
