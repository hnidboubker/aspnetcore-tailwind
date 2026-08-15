# Skill: Lot C — Qualité Code Rapide

Corrections isolées, un fichier par tâche.

---

## TÂCHE C1 — Corriger la typo `ouput` → `output`

Fichier : `src/McsCore.Mvc/Controllers/InvoicesController.cs`

Action : chercher la variable `ouput` et la renommer en `output` dans tout le fichier.

Vérification : build OK.

---

## TÂCHE C2 — Corriger `OraganozationUnitId` → `OrganizationUnitId`

Fichier : `src/McsCore.Core/Interfaces/IMustHaveOrganization.cs` (ou nom équivalent)

Action : corriger la faute d'orthographe dans la propriété de l'interface.

Vérification : build de McsCore.Core puis de tous les projets qui le référencent.

Note : vérifier que ce champ n'est pas déjà mappé ailleurs sous le mauvais nom avant de renommer (sans cassure).

---

## TÂCHE C3 — Neutraliser le BCC personnel dans le code archivé

Fichier : `src/McsCore.Mvc/archive/InvoicesController-Copier.cs`

Action : remplacer l'adresse BCC personnelle par une constante `ARCHIVE_BCC_REMOVED` pour tracer l'intention sans laisser l'adresse.

Vérification : `grep -r "live.fr" src/` ne doit plus retourner de résultat.

---

## TÂCHE C4 — Documenter la configuration des secrets dans README.md

Fichier : `README.md` (racine)

Action : ajouter une section « Configuration » expliquant le mécanisme User Secrets / variables d'environnement, avec des marqueurs `[VARIABLE]` mais AUCUNE valeur réelle.

Vérification : README à jour, aucun secret écrit.

---

## TÂCHE C5 — Noter la convention d'archivage dans AUDIT.md

Fichier : `AUDIT.md`

Action : ajouter en fin de document la note : « Convention projet : code mort → dossier archive/, jamais suppression. »

Vérification : AUDIT.md contient la note.

---

RÈGLES PROJET : pas de suppression, pas de renommage de fichier/public API sans demande, mise à jour AUDIT.md, langue français.