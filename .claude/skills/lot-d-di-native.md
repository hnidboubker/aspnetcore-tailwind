# Skill: Lot D — DI Ninjector vers natif (pilote)

Expérimentation contrôlée : migrer UN seul service vers le DI natif pour valider le pattern avant extension.

---

## TÂCHE D1 — Enregistrer un service via le DI natif (pilote)

### Objectif
Valider que le DI natif .NET fonctionne en parallèle du système Ninjector existant sur UN seul service, sans casser l'application.

### Service candidat : `IEmailSender` (ou équivalent)

**Étape 1 — Identifier le service actuel**
Dans `McsCore.Infrastructure/Modules/InfrastructureModule.cs` (ou similaire), repérer l'enregistrement du service email :
```csharp
// Pattern actuel (Ninjector) :
services.AddScoped<IEmailSender, EmailSender>();
```

**Étape 2 — Ajouter l'enregistrement natif dans Program.cs**
Dans `src/McsCore.Mvc/Program.cs`, **après** les appels `ModuleLoader.RegisterModulesFromAssembly(...)` :
```csharp
// Enregistrement natif en complément (ne remplace pas Ninjector pour ce service)
builder.Services.AddScoped<IEmailSender, EmailSender>();
```

**Étape 3 — Vérifier la priorité**
Le DI natif .NET applique la dernière registration. Si le même service est enregistré deux fois (Ninjector + natif), la dernière registration gagne. Ici la ligne `builder.Services.AddScoped(...)` est après ModuleLoader, donc elle écrase la registration Ninjector pour ce service uniquement.

**Étape 4 — Build et test manuel**
- `dotnet build --project src/McsCore.Mvc/McsCore.Mvc.csproj`
- Lancer l'application et tester un chemin qui utilise le service email (envoi de facture, notification, etc.)
- Vérifier que le service est bien résolu et fonctionne

### Risques et vérifications
| Risque | Vérification |
|--------|-------------|
| Conflit de registration | Vérifier que `AddScoped` est bien après `ModuleLoader` dans Program.cs |
| Service non résolu | `dotnet run` + test chemin email, vérifier qu'aucune exception n'est levée |
| Incompatibilité avec Ninjector | Vérifier que les autres services toujours gérés par Ninjector fonctionnent |

### Convention projet respectée
- Pas de suppression de code Ninjector existant
- Ajout minimal (2-3 lignes)
- Pattern existant respecté (AddScoped comme le reste du projet)
- Si ça fonctionne, le pattern peut être étendu à d'autres services dans de futurs lots

---

## RÈGLES PROJET À RESPECTER

- Changement minimal : 1 seul service déplacé vers le natif
- Pas de modification du code existant dans Infrastructure/
- Vérifier que l'application démarre et que le service fonctionne avant de valider
- Mettre à jour AUDIT.md : "Lot D : pilote DI natif sur [service] — succès/échec"
- Langue : français dans les commentaires