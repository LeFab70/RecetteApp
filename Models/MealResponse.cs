using System;
using System.Collections.Generic;
using System.Text;


namespace RecetteApp.Models;
public class MealResponse
{
    public List<Meal> Meals { get; set; } = new();
}
