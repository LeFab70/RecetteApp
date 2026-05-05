using System;
using System.Collections.Generic;
using System.Text;

using RecetteApp.Models;
using RecetteApp.Services;

namespace RecetteApp.ViewModels;

public class MainViewModel
{
    private readonly MealService _mealService;

    public List<Meal> Meals { get; set; } = new();

    public MainViewModel(MealService mealService)
    {
        _mealService = mealService;
    }

    public async Task LoadMeals()
    {
        Meals = await _mealService.GetMeals();
    }
}