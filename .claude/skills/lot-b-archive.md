# Skill: Lot B — Archivage Code Mort

Conformément à `.rules/rules-project.md` : déplacer le code inerte vers `archive/`, jamais supprimer.

---

## TÂCHE B1 — Archiver InvoicesController - Copier.cs

Fichier source : `src/McsCore.Mvc/Controllers/InvoicesController - Copier.cs`
Destination : `src/McsCore.Mvc/archive/InvoicesController-Copier.cs`

Action :
1. Créer le dossier `src/McsCore.Mvc/archive/`
2. Déplacer le fichier (enlever l'espace du nom)
3. Vérifier que `InvoicesController.cs` (sans le - Copier) est toujours le contrôleur actif et que le build passe

---

## TÂCHE B2 — Archiver Invoices1Controller + vues

Fichiers source :
- `src/McsCore.Mvc/Controllers/Invoices1Controller.cs`
- `src/McsCore.Mvc/Views/Invoices1/` (tout le dossier)

Destination : `src/McsCore.Mvc/archive/Invoices1/`

Action :
1. Créer `src/McsCore.Mvc/archive/Invoices1/`
2. Déplacer le contrôleur et les vues dedans
3. Vérifier que les routes /Invoices retournent 404 (intentionnel), que InvoicesController reste actif

---

## TÂCHE B3 — Archiver Quotes1Controller + vues

Fichiers source :
- `src/McsCore.Mvc/Controllers/Quotes1Controller.cs`
- `src/McsCore.Mvc/Views/Quotes1/` (tout le dossier)

Destination : `src/McsCore.Mvc/archive/Quotes1/`

Action : identique à B2 pour la branche Quotes.

---

## TÂCHE B4 — Archiver senders email commentés

Fichiers source dans `src/Mcs.MailKit/Senders/` :
- `BillingEmailSender.cs`
- `MissionEmailSender.cs`

Destination : `src/Mcs.MailKit/archive/`

Action :
1. Créer `src/Mcs.MailKit/archive/`
2. Déplacer les deux fichiers commentés dedans
3. Vérifier que `FluentEmailSender.cs` et `DocumentEmailSender.cs` restent actifs et que le build passe

---

RÈGLES PROJET : archivage uniquement, pas de suppression, mise à jour AUDIT.md après chaque tâche, langue français.