# 🍽️ RecetteApp — MAUI Project

## 📌 Description

RecetteApp est une application mobile développée avec **.NET MAUI** permettant de découvrir des recettes du monde entier.
L’application utilise l’API gratuite **TheMealDB**, qui ne nécessite ni compte ni clé API.

Sur l’**accueil**, une rangée de **puces catégories** (couleurs distinctes) permet de **filtrer** la liste en complément de la recherche. Un **bandeau hors ligne**, un **cache fichier** et un **JSON de secours** (`Resources/Raw/fallback_meals.json`) permettent de continuer à parcourir des repas sans réseau.

## 👨‍💻 Programmeurs

- **Kayleb**
- **Fabrice**
- **Perez**

## 🎯 Objectifs du projet

* Consulter des recettes et les **filtrer par catégorie** (puces colorées sur l’accueil)
* Voir les **détails** d’une recette (ingrédients, étapes, lien vidéo, partage, ajout liste de courses)
* Sauvegarder des favoris
* Créer une liste de courses
* Planifier les repas

## 🧱 Architecture

Le projet suit une structure organisée :

* **Models** : données (recettes, catégories API, favoris, planificateur, courses)
* **Services** : communication avec l’API, SQLite, Preferences (thème, liste de courses)
* **Helpers** : palette des puces catégories, extractions de texte pour les cartes
* **ViewModels** : logique de l’application
* **Views** : interface utilisateur

## 🚀 Lancement

**IDE** : ouvrir le projet dans Visual Studio ou Rider, sélectionner **Android Emulator** (ou un appareil USB), puis **Run**.

**CLI** (émulateur ou appareil déjà visible pour `adb`) :

```bash
dotnet build RecetteApp.csproj -c Debug -f net10.0-android -t:Run
```

## 📅 Journal de travail (5 jours)

### Jour 1 — Fondations (modèle, API, injection)
 🎯 Objectif
Mettre en place la base du projet et connecter l’application à l’API.

🔧 Travail réalisé

* Création du projet MAUI
* Mise en place de l’architecture (Models / Services / ViewModels / Views)
* Création du modèle de données pour les recettes
* Mise en place d’un service pour appeler l’API
* Configuration de l’injection de dépendances
* Création d’un ViewModel pour gérer les données
* Test de connexion à l’API

🌐 API utilisée

* **Nom** : TheMealDB
* **Type** : API REST
* **Accès** : Gratuit (sans clé API)

🧪 Résultat obtenu

Au lancement de l’application :

* L’API est appelée avec succès
* Les données sont récupérées
* Le nombre de recettes reçues est affiché

👉 Cela confirme que la connexion API fonctionne correctement.

⚠️ Difficultés rencontrées

* Problème de configuration de l’injection (`HttpClient`)
* Erreurs liées aux namespaces (`Views`)
* Problème de liaison entre XAML et code-behind
* Warnings liés aux propriétés non initialisées

👉 Tous les problèmes ont été corrigés avec succès.

✅ Statut

✔ Application fonctionnelle
✔ API connectée
✔ Données récupérées

### Jour 2 — Interface & MVVM (page principale conforme)

🎯 Objectif

Afficher les recettes dans une interface utilisateur moderne avec MVVM.

🔧 Travail réalisé

Mise en place du MainViewModel avec ObservableObject
Utilisation de ObservableProperty pour la gestion d’état
Implémentation de RelayCommand pour le chargement des données
Gestion du chargement (spinner)
Gestion des erreurs avec fallback local + cache hors ligne
Affichage des recettes avec une CollectionView
Ajout des images, titres et catégories
Ajout du binding fort (x)
Mise en place du bandeau **"📴 Hors ligne"**, message d’erreur et bouton **Réessayer**
Ajout de la recherche (filtre local)
Ajout du pull-to-refresh (RefreshView)
Configuration de l’injection (AddTransient)
🧪 Résultat obtenu

Au lancement de l’application :

🔄 Un indicateur de chargement s’affiche
📡 Les données sont récupérées depuis l’API
🍽️ Les recettes s’affichent sous forme de liste avec images
❌ En cas d’erreur → affichage d’un message hors ligne

👉 L’application est maintenant interactive et dynamique

⚠️ Difficultés rencontrées

Configuration de l’injection (HttpClient)
Gestion des namespaces (Views)
Liaison XAML / ViewModel
Migration vers ObservableObject et RelayCommand
Warning AOT avec [ObservableProperty]

👉 Tous les problèmes ont été corrigés.

### Jour 3 — Navigation + page Favoris (SQLite)

🎯 Objectif

Ajouter la navigation Shell et la page **Mes Favoris** avec persistance.

🔧 Travail réalisé

- Ajout d’une navigation par onglets (Shell TabBar)
- Création de la page **Mes Favoris**
- Ajout de **SQLite** pour sauvegarder les recettes favorites
- Ajout du **SwipeView** pour supprimer un favori par glissement (page Favoris uniquement)
- Sur la liste **Recettes**, ajout / retrait favoris via le **bouton cœur** sur chaque ligne

🧪 Résultat obtenu

- On peut ajouter une recette aux favoris depuis la page Recettes
- Les favoris restent après fermeture/réouverture de l’application
- Suppression par glissement ou cœur fonctionnelle dans la page Favoris

### Jour 4 — Détails, planificateur, préférences & liste de courses

🎯 Objectif

Aller au-delà de la liste : fiche recette complète, plan de repas persisté, préférences (thème) et liste de courses.

🔧 Travail réalisé

- Page **Détail** (`DetailPage` / `DetailViewModel`) : navigation depuis une ligne de la liste ; ingrédients, instructions, YouTube, partage, ajout à la liste de courses, favori.
- Onglet **Plan** : SQLite (`meal_planner.db3`, `MealPlannerDatabase`, `PlannedMeal`) pour associer des repas à des jours.
- Onglet **Preferences** : choix de thème (clair / sombre / système) via `ThemeHelper` et **Preferences** ; liste de courses gérée par `ShoppingListStore` (persistance Preferences).
- Service **MealService** : recherche, détail par id, liste des **catégories** (`categories.php`) pour les puces ; gestion d’erreurs réseau / parsing.

🧪 Résultat obtenu

- Navigation fluide liste → détail ; plan et courses survivent aux fermetures d’app ; thème cohérent au redémarrage.

### Jour 5 — Finition & présentation

🎯 Objectif

Finaliser l’expérience utilisateur (filtres, cohérence UI) et valider les scénarios réseau / hors ligne.

🔧 Travail réalisé

- **Filtre par catégorie** sur l’accueil : `CategoryChipVm`, `CategoryChipPalette`, chargement des noms depuis l’API (repli sur catégories déduites des repas si besoin).
- Affichage type cartes sur la liste (`MealListRowVm`), rafraîchissement et messages d’erreur avec **Réessayer**.
- Vérifications manuelles : API disponible, mode dégradé hors ligne, favoris et plan.

## 🧭 Navigation (Shell)

- **Recettes** : liste (recherche, pull-to-refresh, puces **catégories** colorées, cœur favori, bandeau hors ligne).
- **Favoris** : SQLite ; suppression par **SwipeView** ou bouton **cœur** sur la carte.
- **Plan** : planificateur de repas (SQLite).
- **Preferences** : thème + liste de courses + informations utiles sur l’app.
- **Détail** : ouverture depuis un tap sur une recette (pas un onglet Shell).

## 📡 API TheMealDB (extrait)

| Usage | Endpoint (v1) |
|--------|----------------|
| Recherche | `search.php?s=` |
| Détail | `lookup.php?i=` |
| Catégories | `categories.php` |

Le filtre catégorie côté app s’appuie sur le champ catégorie renvoyé avec chaque repas (`strCategory`), aligné sur les libellés TheMealDB lorsque l’API les fournit.

## 💾 Persistance locale

| Donnée | Mécanisme |
|--------|-----------|
| Favoris | SQLite `favorites.db3` (`FavoritesDatabase`) |
| Plan repas | SQLite `meal_planner.db3` (`MealPlannerDatabase`) |
| Liste de courses / thème | `Preferences` (`ShoppingListStore`, `ThemeHelper`) |
| Cache repas (réseau OK) | Fichier `meals_cache.json` dans le stockage app |

## 🧩 Fichiers touchés / refactorés (principaux)

- `RecetteApp.csproj`, `MauiProgram.cs`, `AppShell.xaml`, `App.xaml.cs`
- `Models/` : `Meal.cs`, `MealCategory.cs`, `FavoriteMeal.cs`, `PlannedMeal.cs`, `ShoppingItemDto.cs`, réponses API
- `Services/` : `MealService.cs`, `FavoritesDatabase.cs`, `MealPlannerDatabase.cs`, `ShoppingListStore.cs`, `ThemeHelper.cs`
- `Helpers/` : `CategoryChipPalette.cs`, `RecetteTexteHelper.cs`
- `ViewModels/` : `MainViewModel.cs`, `CategoryChipVm.cs`, `MealListRowVm.cs`, `DetailViewModel.cs`, `FavoritesViewModel.cs`, `MealPlannerViewModel.cs`, `JourPlanVm.cs`, `PreferencesViewModel.cs`, `ShoppingListViewModel.cs`
- `Views/` : `MainPage`, `DetailPage`, `FavoritesPage`, `MealPlannerPage`, `PreferencesPage`
- `Resources/Raw/fallback_meals.json`

## 🤖 Utilisation d’IA

Cette application a été développée avec l’assistance d’un outil de copilot.
