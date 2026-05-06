# 🍽️ RecetteApp — MAUI Project

## 📌 Description

RecetteApp est une application mobile développée avec **.NET MAUI** permettant de découvrir des recettes du monde entier.
L’application utilise l’API gratuite **TheMealDB**, qui ne nécessite ni compte ni clé API.

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

## 🚀 Jour 1 — Foundations
 🎯 Objectif
Mettre en place la base du projet et connecter l’application à l’API.

## 🔧 Travail réalisé

* Création du projet MAUI
* Mise en place de l’architecture (Models / Services / ViewModels / Views)
* Création du modèle de données pour les recettes
* Mise en place d’un service pour appeler l’API
* Configuration de l’injection de dépendances
* Création d’un ViewModel pour gérer les données
* Test de connexion à l’API

## 🌐 API utilisée

* **Nom** : TheMealDB
* **Type** : API REST
* **Accès** : Gratuit (sans clé API)

## 🧪 Résultat obtenu

Au lancement de l’application :

* L’API est appelée avec succès
* Les données sont récupérées
* Le nombre de recettes reçues est affiché

👉 Cela confirme que la connexion API fonctionne correctement.

## ⚠️ Difficultés rencontrées

* Problème de configuration de l’injection (`HttpClient`)
* Erreurs liées aux namespaces (`Views`)
* Problème de liaison entre XAML et code-behind
* Warnings liés aux propriétés non initialisées

👉 Tous les problèmes ont été corrigés avec succès.

## ✅ Statut

✔ Application fonctionnelle
✔ API connectée
✔ Données récupérées

🚀 Jour 2 — Interface & MVVM
🎯 Objectif

Afficher les recettes dans une interface utilisateur moderne avec MVVM.

🔧 Travail réalisé
Mise en place du MainViewModel avec ObservableObject
Utilisation de ObservableProperty pour la gestion d’état
Implémentation de RelayCommand pour le chargement des données
Gestion du chargement (spinner)
Gestion des erreurs avec fallback local
Affichage des recettes avec une CollectionView
Ajout des images, titres et catégories
Ajout du binding fort (x)
Mise en place d’un bandeau hors ligne en cas d’erreur API
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

## 👨‍💻 Auteur
Projet réalisé dans le cadre du cours MAUI.
