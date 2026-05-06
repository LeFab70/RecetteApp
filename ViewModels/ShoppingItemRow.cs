using CommunityToolkit.Mvvm.ComponentModel;

namespace RecetteApp.ViewModels;

public partial class ShoppingItemRow : ObservableObject
{
    public ShoppingItemRow(string id, string text, bool isChecked)
    {
        Id = id;
        Text = text;
        IsChecked = isChecked;
    }

    [ObservableProperty]
    public partial string Id { get; set; }

    [ObservableProperty]
    public partial string Text { get; set; }

    [ObservableProperty]
    public partial bool IsChecked { get; set; }
}
