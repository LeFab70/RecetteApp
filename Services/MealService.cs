using System;
using System.Collections.Generic;
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
        var response = await _httpClient.GetAsync("search.php?s=");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Erreur API : {response.StatusCode}");

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<MealResponse>(json, options);

        return result?.Meals ?? new List<Meal>();
    }
}