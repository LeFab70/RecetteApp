# 🍽️ RecetteApp — MAUI Project

## 📌 Description

RecetteApp est une application mobile développée avec **.NET MAUI** permettant de découvrir des recettes du monde entier.
L’application utilise l’API gratuite **TheMealDB**, qui ne nécessite ni compte ni clé API.

## 👨‍💻 Programmeurs

- **Kayleb**
- **Fabrice**
- **Perez**

## 🎯 Objectifs du projet

* Consulter des recettes par catégorie
* Voir les détails d’une recette
* Sauvegarder des favoris
* Créer une liste de courses
* Planifier les repas

## 🧱 Architecture

Le projet suit une structure organisée :

* **Models** : données (recettes)
* **Services** : communication avec l’API
* **ViewModels** : logique de l’application
* **Views** : interface utilisateur

## 🚀 Lancement (3 étapes)

1. Ouvrir le projet dans Visual Studio / Rider avec le workload **.NET MAUI** installé
2. Sélectionner **Android Emulator** comme cible
3. Démarrer l’application (Run)

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
- Ajout du **SwipeView** pour supprimer un favori par glissement
- Ajout d’une action “Favori” (swipe) depuis la liste principale

🧪 Résultat obtenu

- On peut ajouter une recette aux favoris depuis la page Recettes
- Les favoris restent après fermeture/réouverture de l’application
- Suppression par glissement fonctionnelle dans la page Favoris

### Jour 4 — Liste de courses (Preferences) (à faire)

🎯 Objectif

Rendre une liste de courses persistante via Preferences.

### Jour 5 — Finition & présentation (à faire)

🎯 Objectif

Finaliser le design, vérifier tous les scénarios (réseau présent/absent) et préparer la démo.

## 🧭 Navigation (Shell)

- **Recettes**: liste principale (recherche + refresh + fallback hors ligne)
- **Favoris**: liste SQLite + suppression par SwipeView

## 💾 SQLite — Favoris

- Les favoris sont stockés localement dans une base SQLite (`favorites.db3`) et rechargés au démarrage.
- La page Favoris utilise un **SwipeView** pour retirer un item.

## 🧩 Fichiers touchés / refactorés

- `RecetteApp.csproj` (ajout packages SQLite)
- `MauiProgram.cs` (DI: `FavoritesDatabase`, `FavoritesPage`, `FavoritesViewModel`)
- `AppShell.xaml` (TabBar navigation)
- `Models/FavoriteMeal.cs` (modèle SQLite)
- `Services/FavoritesDatabase.cs` (CRUD favoris)
- `ViewModels/MainViewModel.cs` (commande Ajouter aux favoris + cache/fallback)
- `Views/MainPage.xaml` (Swipe “Favori” + recherche/refresh/erreurs)
- `ViewModels/FavoritesViewModel.cs` (chargement + suppression)
- `Views/FavoritesPage.xaml` / `Views/FavoritesPage.xaml.cs` (UI + OnAppearing)
- `Resources/Raw/fallback_meals.json` (données hors ligne)

## 🤖 Utilisation d’IA

Cette application a été développée avec l’assistance d’un outil de copilot.
