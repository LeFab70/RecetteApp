using RecetteApp.Models;
using SQLite;

namespace RecetteApp.Services;

public class MealPlannerDatabase
{
    private SQLiteAsyncConnection? _db;

    private async Task Init()
    {
        if (_db is not null)
            return;

        var path = Path.Combine(FileSystem.AppDataDirectory, "meal_planner.db3");
        _db = new SQLiteAsyncConnection(path);
        await _db.CreateTableAsync<PlannedMeal>();
    }

    public async Task<List<PlannedMeal>> GetWeekAsync()
    {
        await Init();
        var rows = await _db!.Table<PlannedMeal>().ToListAsync();
        var dict = rows.ToDictionary(x => x.DayIndex);
        var result = new List<PlannedMeal>();
        for (var i = 0; i < 7; i++)
        {
            if (dict.TryGetValue(i, out var p))
                result.Add(p);
            else
                result.Add(new PlannedMeal { DayIndex = i });
        }

        return result;
    }

    public async Task UpsertAsync(int dayIndex, string idMeal, string strMeal, string strMealThumb)
    {
        await Init();
        var row = new PlannedMeal
        {
            DayIndex = dayIndex,
            IdMeal = idMeal ?? string.Empty,
            StrMeal = strMeal ?? string.Empty,
            StrMealThumb = strMealThumb ?? string.Empty
        };
        await _db!.InsertOrReplaceAsync(row);
    }

    public async Task ClearDayAsync(int dayIndex)
    {
        await Init();
        await _db!.DeleteAsync<PlannedMeal>(dayIndex);
    }
}
