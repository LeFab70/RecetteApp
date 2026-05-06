using RecetteApp.ViewModels;

namespace RecetteApp.Views;

public partial class ShoppingListPage : ContentPage
{
    private readonly ShoppingListViewModel _vm;

    public ShoppingListPage(ShoppingListViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Charger();
    }
}
