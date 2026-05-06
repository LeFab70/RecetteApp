namespace RecetteApp.Models;

public sealed class ShoppingItemDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string Text { get; set; } = string.Empty;

    public bool IsChecked { get; set; }
}
