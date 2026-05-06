using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using RecetteApp.Models;

namespace RecetteApp.Services;

public class MealService
{
    private readonly HttpClient _httpClient;

    public MealService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Meal>> GetMeals()
    {
        try
        {
            var response = await _httpClient.GetAsync("search.php?s=");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var msg =
                    $"TheMealDB HTTP {(int)response.StatusCode} {response.ReasonPhrase}{TruncateBody(body)}";
                Debug.WriteLine(msg);
                throw new Exception(msg);
            }

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<MealResponse>(json, options);
            return result?.Meals ?? new List<Meal>();
        }
        catch (TaskCanceledException ex)
        {
            // HttpClient timeout (souvent TaskCanceledException)
            var msg = "Délai dépassé (timeout) lors de l’appel à TheMealDB.";
            Debug.WriteLine($"{msg}\n{ex}");
            throw new Exception(msg, ex);
        }
        catch (HttpRequestException ex)
        {
            var msg = ex.Message ?? "Erreur réseau lors de l’appel à TheMealDB.";

            // Message plus clair pour DNS / pas d’accès Internet sur émulateur
            if (msg.Contains("hostname", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("servname", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Name or service not known", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("No such host", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("nodename", StringComparison.OrdinalIgnoreCase))
            {
                msg = "Pas d'accès Internet/DNS sur l'émulateur (impossible de résoudre themealdb.com).";
            }

            Debug.WriteLine($"TheMealDB HttpRequestException: {ex}");
            throw new Exception(msg, ex);
        }
        catch (JsonException ex)
        {
            var msg = "Réponse TheMealDB invalide (JSON non lisible).";
            Debug.WriteLine($"{msg}\n{ex}");
            throw new Exception(msg, ex);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"TheMealDB exception: {ex}");
            throw;
        }
    }

    /// <summary>Récupère une recette complète (ingrédients, instructions…) via TheMealDB.</summary>
    public async Task<Meal?> GetMealByIdAsync(string idMeal)
    {
        if (string.IsNullOrWhiteSpace(idMeal))
            return null;

        try
        {
            var response = await _httpClient.GetAsync($"lookup.php?i={Uri.EscapeDataString(idMeal.Trim())}");

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var msg =
                    $"TheMealDB HTTP {(int)response.StatusCode} {response.ReasonPhrase}{TruncateBody(body)}";
                Debug.WriteLine(msg);
                throw new Exception(msg);
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<MealResponse>(json, options);
            return result?.Meals?.FirstOrDefault();
        }
        catch (TaskCanceledException ex)
        {
            var msg = "Délai dépassé (timeout) lors de l’appel à TheMealDB.";
            Debug.WriteLine($"{msg}\n{ex}");
            throw new Exception(msg, ex);
        }
        catch (HttpRequestException ex)
        {
            var msg = ex.Message ?? "Erreur réseau lors de l’appel à TheMealDB.";
            if (msg.Contains("hostname", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("servname", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Name or service not known", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("No such host", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("nodename", StringComparison.OrdinalIgnoreCase))
            {
                msg = "Pas d'accès Internet/DNS sur l'émulateur (impossible de résoudre themealdb.com).";
            }

            Debug.WriteLine($"TheMealDB HttpRequestException: {ex}");
            throw new Exception(msg, ex);
        }
        catch (JsonException ex)
        {
            var msg = "Réponse TheMealDB invalide (JSON non lisible).";
            Debug.WriteLine($"{msg}\n{ex}");
            throw new Exception(msg, ex);
        }
    }

    private static string TruncateBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return "";

        var t = body.Replace("\r", " ").Replace("\n", " ").Trim();
        if (t.Length > 140)
            t = t[..140] + "...";

        return $" — {t}";
    }
}