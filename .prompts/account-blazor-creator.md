# Prompt — Inspection et migration Blazor du répertoire `Account`

## Objectif

Inspecte le projet **Razor Pages** en te concentrant exclusivement sur le répertoire **`Account`**, puis mets à jour la documentation de migration Blazor en t'appuyant sur l'implémentation de référence disponible sur la branche **`net-blazor`**.

## Tâches

1. Analyse l'intégralité du répertoire `Account` du projet Razor Pages.
2. Ouvre le fichier `prompts/blazor-account.md`.
3. Après l'inspection, **mets à jour ce même fichier** avec les résultats de ton analyse. N'en crée pas un nouveau.
4. Reproduis **fidèlement** l'architecture, les conventions, les composants et les bonnes pratiques déjà utilisés sur la branche **`net-blazor`**.
5. Pour chaque page ou fonctionnalité du répertoire `Account`, documente précisément :
   - la stratégie de migration vers Blazor ;
   - les composants Blazor à créer ou à adapter ;
   - les services, modèles et dépendances à utiliser ;
   - les flux d'authentification et d'autorisation ;
   - les différences éventuelles avec l'implémentation Razor Pages ;
   - les points d'attention ou travaux restants.

## Contraintes

- Traiter **uniquement** le répertoire `Account`.
- Se concentrer exclusivement sur la **migration Blazor**.
- **Ne pas créer de nouveau fichier** : mettre à jour uniquement `prompts/blazor-account.md`.
- Respecter strictement l'architecture et les conventions de la branche `net-blazor`.
- Ne pas modifier ni documenter les autres parties du projet.
- Le fichier `prompts/blazor-account.md` doit constituer une spécification de migration claire, détaillée et exploitable, reflétant fidèlement l'implémentation Blazor de référence.
```