# Migration Razor Pages Account vers Blazor

Je veux que tu migres mon dossier **Account** actuel en **Blazor** en conservant exactement le même design et la même expérience utilisateur.

## Objectif

Recréer dans Blazor une copie **visuelle et fonctionnelle 1:1** de mon dossier Razor Pages `Account`.

Le résultat doit avoir :
- le même design
- le même layout
- les mêmes couleurs
- les mêmes composants visuels
- les mêmes formulaires
- les mêmes validations
- les mêmes textes
- les mêmes comportements utilisateur

---

# Structure actuelle Razor Pages

Mon dossier contient des pages comme :

```
Account
├── SignIn
├── SignUp
├── ForgotPassword
├── ResetPassword
├── ConfirmEmail
└── autres pages existantes
```

Analyse tous les fichiers fournis :
- `.cshtml`
- `.cshtml.cs`
- fichiers CSS
- images
- icônes
- scripts JavaScript éventuels

---

# Nouvelle structure Blazor demandée

Créer l'équivalent :

```
Pages
└── Account
    ├── AccountLayout.razor
    ├── SignIn.razor
    ├── SignUp.razor
    ├── ForgotPassword.razor
    ├── ResetPassword.razor
    ├── ConfirmEmail.razor
    └── autres composants nécessaires
```

---

# Conversion Razor Pages → Blazor

Convertir :

| Razor Pages | Blazor |
|---|---|
| `.cshtml` | `.razor` |
| `PageModel` | `@code` |
| `asp-for` | `@bind-Value` |
| `asp-validation-for` | `ValidationMessage` |
| `<form method="post">` | `EditForm` |
| Actions POST | Méthodes C# Blazor |
| ViewData | Paramètres ou services |
| TempData | State management adapté |

---

# Design

Le design doit rester strictement identique.

Conserver :

- HTML existant quand possible
- classes CSS existantes
- animations
- responsive design
- Bootstrap/Tailwind si utilisé
- icônes
- images
- espacements
- tailles de police
- boutons
- cartes
- backgrounds

Ne pas simplifier l'interface.

Ne pas créer un nouveau design.

Je veux une reproduction exacte du rendu Razor Pages actuel.

---

# Layout Account

Créer un layout partagé :

```
AccountLayout.razor
```

Toutes les pages :

```
SignIn
SignUp
ForgotPassword
ResetPassword
```

doivent utiliser le même environnement graphique.

Exemple :

```razor
@layout AccountLayout
```

---

# Authentification

Si le projet utilise ASP.NET Identity, conserver :

- UserManager
- SignInManager
- IdentityUser
- services existants
- logique de validation
- gestion des erreurs
- messages utilisateur

Adapter uniquement la couche interface pour Blazor.

---

# Résultat attendu

Je veux obtenir :

✅ Les mêmes pages  
✅ Le même design pixel par pixel  
✅ Les mêmes champs  
✅ Les mêmes validations  
✅ Les mêmes messages  
✅ Les mêmes liens de navigation  
✅ Le même comportement utilisateur  
✅ Une architecture Blazor propre et maintenable  

---

# Avant de générer le code

Commence par analyser les fichiers fournis et donne :

1. La liste des pages à convertir.
2. La liste des composants Blazor nécessaires.
3. La liste des fichiers CSS/images à réutiliser.
4. Le plan de migration.

Ensuite génère le code complet.