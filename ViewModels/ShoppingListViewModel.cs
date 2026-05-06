using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RecetteApp.Models;
using RecetteApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RecetteApp.ViewModels;

public partial class ShoppingListViewModel : ObservableObject
{
    private readonly ShoppingListStore _store;

    [ObservableProperty]
    public partial ObservableCollection<ShoppingItemRow> Articles { get; set; } = new();

    public ShoppingListViewModel(ShoppingListStore store)
    {
        _store = store;
    }

    public void Charger()
    {
        foreach (var row in Articles.ToList())
            row.PropertyChanged -= OnRowPropertyChanged;

        Articles.Clear();

        foreach (var dto in _store.Load())
        {
            var row = new ShoppingItemRow(dto.Id, dto.Text, dto.IsChecked);
            row.PropertyChanged += OnRowPropertyChanged;
            Articles.Add(row);
        }
    }

    private void OnRowPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ShoppingItemRow.IsChecked) or nameof(ShoppingItemRow.Text))
            Persister();
    }

    private void Persister()
    {
        var dtos = Articles.Select(r => new ShoppingItemDto
        {
            Id = r.Id,
            Text = r.Text,
            IsChecked = r.IsChecked
        });
        _store.Save(dtos);
    }

    [RelayCommand]
    private void EffacerCoches()
    {
        foreach (var row in Articles.Where(a => a.IsChecked).ToList())
        {
            row.PropertyChanged -= OnRowPropertyChanged;
            Articles.Remove(row);
        }

        Persister();
    }

    [RelayCommand]
    private void ToutEffacer()
    {
        foreach (var row in Articles.ToList())
            row.PropertyChanged -= OnRowPropertyChanged;

        Articles.Clear();
        Persister();
    }
}
