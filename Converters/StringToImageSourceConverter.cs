using System.Globalization;

namespace RecetteApp.Converters;

/// <summary>Évite UriImageSource vide / invalide (crash fréquent sur Android).</summary>
public sealed class StringToImageSourceConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var s = value as string;
        if (string.IsNullOrWhiteSpace(s))
            return null;

        s = s.Trim();
        if (!Uri.TryCreate(s, UriKind.Absolute, out var uri))
            return null;

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            return null;

        return ImageSource.FromUri(uri);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
