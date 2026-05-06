using RecetteApp.Helpers;
using RecetteApp.Models;
using SQLite;

namespace RecetteApp.Services;

public class FavoritesDatabase
{
    private SQLiteAsyncConnection? _db;

    private async Task Init()
    {
        if (_db is not null)
            return;

        var path = Path.Combine(FileSystem.AppDataDirectory, "favorites.db3");
        _db = new SQLiteAsyncConnection(path);
        await _db.CreateTableAsync<FavoriteMeal>();
        try
        {
            await _db.ExecuteAsync("ALTER TABLE FavoriteMeal ADD COLUMN StrSnippet TEXT DEFAULT ''");
        }
        catch
        {
            // Colonne déjà présente
        }
    }

    public async Task<List<FavoriteMeal>> GetAll()
    {
        await Init();
        return await _db!.Table<FavoriteMeal>().OrderBy(x => x.StrMeal).ToListAsync();
    }

    public async Task AddOrReplace(Meal meal)
    {
        await Init();

        if (string.IsNullOrWhiteSpace(meal.IdMeal))
            return;

        var fav = new FavoriteMeal
        {
            IdMeal = meal.IdMeal,
            StrMeal = meal.StrMeal ?? string.Empty,
            StrCategory = meal.StrCategory ?? string.Empty,
            StrArea = meal.StrArea ?? string.Empty,
            StrMealThumb = meal.StrMealThumb ?? string.Empty,
            StrSnippet = RecetteTexteHelper.ApercuInstructions(meal.StrInstructions, 160)
        };

        await _db!.InsertOrReplaceAsync(fav);
    }

    public async Task<bool> EstFavoriAsync(string idMeal)
    {
        if (string.IsNullOrWhiteSpace(idMeal))
            return false;

        await Init();
        var count = await _db!.Table<FavoriteMeal>().Where(x => x.IdMeal == idMeal).CountAsync();
        return count > 0;
    }

    public async Task Delete(string idMeal)
    {
        await Init();
        await _db!.DeleteAsync<FavoriteMeal>(idMeal);
    }
}

