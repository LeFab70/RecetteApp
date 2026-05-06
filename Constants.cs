using System;
using System.Collections.Generic;
using System.Text;

namespace RecetteApp
{
    public static class Constants
    {
        public const string ApiBaseUrl = "https://www.themealdb.com/api/json/v1/1/";

        /// <summary>Clé Preferences pour la liste de courses (JSON).</summary>
        public const string ShoppingListPreferenceKey = "shopping_list_items_v1";

        public const string AppDescription =
            "RecetteApp — découverte de recettes (TheMealDB), favoris SQLite, liste de courses et planificateur.";
    }
}
