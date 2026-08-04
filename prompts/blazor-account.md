# Documentation de Migration Blazor — Dossier Account

## Table des matières

1. [Vue d'ensemble](#vue-densemble)
2. [Architecture Blazor](#architecture-blazor)
3. [Répertoire source (Razor Pages)](#répertoire-source-razor-pages)
4. [Composants Blazor existants](#composants-blazor-existants)
5. [Analyse par page](#analyse-par-page)
6. [Gaps et travaux restants](#gaps-et-travaux-restants)
7. [Plan d'action](#plan-daction)

---

## Vue d'ensemble

Ce document documente la migration du dossier **Account** d'un projet Razor Pages vers Blazor. L'implémentation de référence se trouve sur la branche **`net-blazor`**.

### Projet source
- **Chemin** : `Src/TailwindRazorPage.Web/Areas/Identity/Pages/Account/`
- **Technologie** : ASP.NET Core Razor Pages + Identity
- **Pages** : 7 pages (Login, Register, ForgotPassword, ConfirmEmail, Logout, Manage/Index, Manage/ChangePassword)

### Projet cible
- **Chemin** : `Src/TailwindBlazor.Web/`
- **Technologie** : Blazor Web App + Interactive WebAssembly
- **Architecture** : Server (endpoints API) + Client (composants UI)

---

## Architecture Blazor

### Structure des projets

```
TailwindBlazor.Web/                    # Projet Server
├── Components/
│   ├── App.razor                      # Shell HTML racine
│   ├── Routes.razor                   # Routeur avec AuthorizeRouteView
│   ├── Layout/
│   │   └── MainLayout.razor           # Layout principal avec LoginDisplay
│   ├── Pages/
│   │   ├── Home.razor
│   │   └── Error.razor
│   └── Account/
│       └── AccountEndpoints.cs        # Minimal API endpoints
├── Program.cs                         # Configuration Blazor + Identity
└── appsettings.json                   # Configuration (⚠️ incomplet)

TailwindBlazor.Web.Client/            # Projet Client (WASM)
├── Components/
│   └── Account/
│       ├── Login.razor
│       ├── Register.razor
│       ├── ForgotPassword.razor       # ⚠️ Stub uniquement
│       ├── LoginDisplay.razor
│       ├── IdentityAuthenticationStateProvider.cs
│       └── Manage/
│           ├── Index.razor
│           └── ChangePassword.razor
└── Program.cs                         # Configuration Client
```

### Flux d'authentification

1. **Login** : Component Client → HttpClient → `POST /account/login` → Set-Cookie → AuthenticationState
2. **Register** : Component Client → HttpClient → `POST /account/register` → Email confirmation
3. **Logout** : LoginDisplay → HttpClient → `POST /account/logout` → Clear-Cookie
4. **Profile** : Component Client → HttpClient → `GET/POST /account/profile`

### Conventions

- **Rendu** : `@rendermode InteractiveWebAssembly`
- **Design** : Tailwind CSS avec palette emerald-600
- **Validation** : DataAnnotations + ValidationMessage
- **State** : `IdentityAuthenticationStateProvider` (singleton)
- **Communication** : HttpClient vers endpoints Minimal API côté serveur

---

## Répertoire source (Razor Pages)

### Emplacement
`Src/TailwindRazorPage.Web/Areas/Identity/Pages/Account/`

### Liste des pages

| Page | Fichier | Description |
|------|---------|-------------|
| Login | `Login.cshtml[.cs]` | Connexion email + mot de passe avec "Remember Me" |
| Register | `Register.cshtml[.cs]` | Inscription avec FirstName, LastName, Email, Password |
| ForgotPassword | `ForgotPassword.cshtml[.cs]` | Demande de réinitialisation mot de passe |
| ConfirmEmail | `ConfirmEmail.cshtml[.cs]` | Confirmation d'email via token |
| Logout | `Logout.cshtml[.cs]` | Déconnexion POST-only |
| Manage/Index | `Manage/Index.cshtml[.cs]` | Profil utilisateur (FirstName, LastName, Email) |
| Manage/ChangePassword | `Manage/ChangePassword.cshtml[.cs]` | Changement de mot de passe |

### Services et dépendances

- `UserManager<IdentityUser>` — Gestion des utilisateurs
- `SignInManager<IdentityUser>` — Gestion de l'authentification
- `IEmailSender` — Envoi d'emails
- `DefaultContext` — Base de données (IdentityDbContext)

---

## Composants Blazor existants

### ✅ Login.razor
- **Route** : `/login`
- **Statut** : Complet
- **Fonctionnalités** :
  - Formulaire email + password
  - Checkbox "Remember Me"
  - Gestion des erreurs (account locked, invalid credentials)
  - Redirect vers LoginWith2fa si 2FA activé
- **Appels API** : `POST /account/login`

### ✅ Register.razor
- **Route** : `/register`
- **Statut** : Complet
- **Fonctionnalités** :
  - Formulaire FirstName, LastName, Email, Password, ConfirmPassword
  - Validation côté client
  - Génération de token de confirmation email
- **Appels API** : `POST /account/register`

### ✅ ForgotPassword.razor
- **Route** : `/forgot-password`
- **Statut** : Complet (endpoint `POST /account/forgot-password` ajouté)
- **Fonctionnalités** :
  - Appel API via HttpClient vers `account/forgot-password`
  - Gestion de l'état "envoyé" (`_sent`)
  - Anti-énumération de comptes (réponse identique que l'email existe ou non)
- **Endpoint** : `POST /account/forgot-password` (génère token + envoie email via `IEmailSender`)

### ✅ LoginDisplay.razor
- **Route** : Aucun (composant inclus dans le layout)
- **Statut** : Complet
- **Fonctionnalités** :
  - Utilisateur anonyme : Liens Login/Register
  - Utilisateur connecté : Nom + bouton Logout
- **Appels API** : `POST /account/logout`

### ✅ Manage/Index.razor
- **Route** : `/manage`
- **Statut** : Complet
- **Fonctionnalités** :
  - Affichage du profil (FirstName, LastName, Email)
  - Édition inline
  - Appel `RefreshSignInAsync` après mise à jour
- **Appels API** : `GET /account/user`, `POST /account/profile`

### ✅ Manage/ChangePassword.razor
- **Route** : `/manage/changepassword`
- **Statut** : Complet
- **Fonctionnalités** :
  - Formulaire OldPassword, NewPassword, ConfirmPassword
  - Validation minimum 8 caractères
- **Appels API** : `POST /account/changepassword`

---

## Analyse par page

### 1. Login

#### Razor Pages
- **Chemin** : `Areas/Identity/Pages/Account/Login.cshtml`
- **Modèle** : `LoginModel` avec `[BindProperty] LoginInputModel`
- **Services** : `SignInManager`, `UserManager`
- **Flux** :
  1. Validation du formulaire
  2. `SignInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure)`
  3. Gestion du redirect (2FA, lockout, returnUrl)
  4. `RedirectToPage("/Account/LoginWith2fa")` si 2FA

#### Blazor
- **Composant** : `Login.razor`
- **Rendu** : Interactive WebAssembly
- **Flux** :
  1. Validation du formulaire
  2. Appel `POST /account/login` via HttpClient
  3. `IdentityAuthenticationStateProvider` met à jour l'état
  4. Navigation vers returnUrl ou home
- **Différences** :
  - Pas de redirect vers LoginWith2fa (2FA pas implémenté)
  - Utilise Minimal API au lieu de Razor Pages handler

---

### 2. Register

#### Razor Pages
- **Chemin** : `Areas/Identity/Pages/Account/Register.cshtml`
- **Modèle** : `RegisterModel` avec `[BindProperty] InputModel`
- **Services** : `UserManager`, `IEmailSender`
- **Flux** :
  1. Validation du formulaire
  2. `UserManager.CreateAsync(user, password)`
  3. Génération de token de confirmation
  4. `IEmailSender.SendEmailAsync` pour confirmation

#### Blazor
- **Composant** : `Register.razor`
- **Rendu** : Interactive WebAssembly
- **Flux** :
  1. Validation du formulaire
  2. Appel `POST /account/register` via HttpClient
  3. Le serveur gère la création et l'envoi d'email
  4. Redirect vers page de confirmation
- **Différences** :
  - L'envoi d'email est géré côté serveur
  - Pas de confirmation email automatique après inscription

---

### 3. ForgotPassword

#### Razor Pages
- **Chemin** : `Areas/Identity/Pages/Account/ForgotPassword.cshtml`
- **Modèle** : `ForgotPasswordModel` avec `[BindProperty] InputModel`
- **Services** : `UserManager`, `IEmailSender`
- **Flux** :
  1. Validation de l'email
  2. `UserManager.FindByEmailAsync(email)`
  3. `UserManager.GeneratePasswordResetTokenAsync(user)`
  4. `IEmailSender.SendEmailAsync` avec lien de reset
- **⚠️ Note** : Contient un code de debug `ResetPasswordAsync(user, code, "Temp@123")`

#### Blazor
- **Composant** : `ForgotPassword.razor`
- **Statut** : ⚠️ **STUB** — `HandleSubmit` ne fait rien
- **Travail restant** :
  1. Créer endpoint `POST /account/forgot-password`
  2. Implémenter la logique de génération de token
  3. Implémenter l'envoi d'email
  4. Ajouter la page ForgotPasswordConfirmation

---

### 4. ConfirmEmail

#### Razor Pages
- **Chemin** : `Areas/Identity/Pages/Account/ConfirmEmail.cshtml`
- **Modèle** : `ConfirmEmailModel`
- **Services** : `UserManager`
- **Flux** :
  1. Récupération des paramètres `userId` et `code` depuis l'URL
  2. `UserManager.ConfirmEmailAsync(user, code)`
  3. Affichage du résultat

#### Blazor
- **Composant** : `ConfirmEmail.razor` ✅ Créé
- **Route** : `/confirm-email?userId=...&code=...`
- **Rendu** : Interactive WebAssembly
- **Flux** :
  1. Lecture des paramètres via `[SupplyParameterFromQuery]`
  2. Appel `GET /account/confirm-email` via HttpClient
  3. Affichage du succès ou de l'erreur
- **Endpoint** : `GET /account/confirm-email` (valide le token via `UserManager.ConfirmEmailAsync`)

---

### 5. Logout

#### Razor Pages
- **Chemin** : `Areas/Identity/Pages/Account/Logout.cshtml`
- **Modèle** : `LogoutModel`
- **Services** : `SignInManager`
- **Flux** :
  1. POST-only
  2. `SignInManager.SignOutAsync()`
  3. Redirect vers home ou returnUrl

#### Blazor
- **Composant** : `LoginDisplay.razor` (pas de page dédiée)
- **Flux** :
  1. Clic sur bouton Logout
  2. Appel `POST /account/logout` via HttpClient
  3. `IdentityAuthenticationStateProvider` clear l'état
  4. Navigation vers home
- **Différences** :
  - Pas de page dédiée (c'est un bouton dans le layout)
  - Logique similaire

---

### 6. Manage/Index (Profile)

#### Razor Pages
- **Chemin** : `Areas/Identity/Pages/Account/Manage/Index.cshtml`
- **Modèle** : `IndexModel` avec `[BindProperty] InputModel`
- **Services** : `UserManager`, `SignInManager`
- **Flux** :
  1. Chargement du profil via `UserManager.GetUserAsync`
  2. Affichage du formulaire
  3. `UserManager.UpdateAsync(user)` + `SignInManager.RefreshSignInAsync`

#### Blazor
- **Composant** : `Manage/Index.razor`
- **Rendu** : Interactive WebAssembly
- **Flux** :
  1. Chargement via `GET /account/user`
  2. Édition inline
  3. Sauvegarde via `POST /account/profile`
- **Différences** :
  - Utilise HttpClient au lieu de UserManager direct
  - Même logique métier

---

### 7. Manage/ChangePassword

#### Razor Pages
- **Chemin** : `Areas/Identity/Pages/Account/Manage/ChangePassword.cshtml`
- **Modèle** : `ChangePasswordModel` avec `[BindProperty] InputModel`
- **Services** : `UserManager`, `SignInManager`
- **Flux** :
  1. Validation du formulaire
  2. `UserManager.ChangePasswordAsync(user, oldPassword, newPassword)`
  3. `SignInManager.RefreshSignInAsync`

#### Blazor
- **Composant** : `Manage/ChangePassword.razor`
- **Rendu** : Interactive WebAssembly
- **Flux** :
  1. Validation du formulaire
  2. Appel `POST /account/changepassword` via HttpClient
  3. Le serveur gère la logique
- **Différences** :
  - Utilise HttpClient au lieu de UserManager direct
  - Même logique métier

---

## Gaps et travaux restants

### 🔴 Bloquants

| Gap | Impact | Priorité |
|-----|--------|----------|
| ~~`appsettings.json` incomplet~~ | ✅ Corrigé | — |
| ~~ForgotPassword est un stub~~ | ✅ Endpoint + composant complétés | — |
| ~~ConfirmEmail manquant~~ | ✅ Composant + endpoint créés | — |
| `prompts/blazor-account.md` | ✅ Créé | — |

### 🟡 Améliorations

| Gap | Impact | Priorité |
|-----|--------|----------|
| Tests unitaires à étendre (Login, ForgotPassword, ConfirmEmail en cours) | Qualité code | Moyenne |
| ResetPassword / ResetPasswordConfirmation non implémentés | Flow complet | Moyenne |
| 2FA non implémenté | Sécurité | Basse |

### ✅ Complet

| Composant | Statut |
|-----------|--------|
| Login | ✅ Fonctionnel |
| Register | ✅ Fonctionnel |
| Logout | ✅ Fonctionnel |
| ForgotPassword | ✅ Fonctionnel (endpoint ajouté) |
| ConfirmEmail | ✅ Créé (endpoint ajouté) |
| Manage/Index | ✅ Fonctionnel |
| Manage/ChangePassword | ✅ Fonctionnel |

---

## Plan d'action

### Tâche 1 : Infrastructure & Configuration (2-3 semaines)
- [ ] Corriger `appsettings.json` avec ConnectionStrings, Email, Identity
- [ ] Vérifier que le projet build et se lance
- [ ] Mettre en place l'infrastructure de tests (xUnit + bUnit)
- [ ] Écrire les premiers tests de base
- **Livrable** : Projet fonctionnel + tests de base

### Tâche 2 : Documentation de migration (1-2 semaines)
- [ ] Finaliser ce document `blazor-account.md`
- [ ] Ajouter des exemples de code pour chaque page
- [ ] Documenter les patterns de test
- [ ] Créer un guide de contribution
- **Livrable** : Documentation complète et exploitable

### Tâche 3 : Compléter ForgotPassword (2-3 semaines)
- [ ] Compléter `ForgotPassword.razor` avec appel API
- [ ] Créer endpoint `POST /account/forgot-password`
- [ ] Implémenter la génération de token
- [ ] Implémenter l'envoi d'email
- [ ] Créer page `ForgotPasswordConfirmation`
- [ ] Tests unitaires et d'intégration
- **Livrable** : ForgotPassword fonctionnel avec tests

### Tâche 4 : Créer ConfirmEmail (2-3 semaines)
- [ ] Créer composant `ConfirmEmail.razor`
- [ ] Créer endpoint `GET /account/confirm-email`
- [ ] Implémenter la validation du token
- [ ] Gérer les erreurs (token invalide, expiré)
- [ ] Tests unitaires et d'intégration
- **Livrable** : ConfirmEmail fonctionnel avec tests

### Tâche 5 : Review & Polish global (2-3 semaines)
- [ ] Revoir Login, Register, Profile, ChangePassword
- [ ] Ajouter les tests manquants
- [ ] Vérifier la cohérence du design Tailwind
- [ ] Mettre à jour la documentation
- [ ] Revue de code finale
- **Livrable** : Toutes les pages testées et documentées

---

## Annexe

### Fichiers clés à modifier

| Fichier | Action |
|---------|--------|
| `TailwindBlazor.Web/appsettings.json` | Modifier |
| `TailwindBlazor.Web/Components/Account/AccountEndpoints.cs` | Modifier |
| `TailwindBlazor.Web.Client/Components/Account/ForgotPassword.razor` | Modifier |
| `TailwindBlazor.Web.Client/Components/Account/ConfirmEmail.razor` | Créer |
| `prompts/blazor-account.md` | Modifier (ce fichier) |

### Dépendances NuGet

- `Microsoft.AspNetCore.Components.Web` — Blazor
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` — Identity
- `Microsoft.EntityFrameworkCore.SqlServer` — EF Core
- `bUnit` — Tests Blazor (à ajouter)
- `xUnit` — Tests unitaires (à ajouter)
