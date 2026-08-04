# Skill: Lot E — Tests (amorçage)

Ajouter la base minimale de tests automatisés au projet.

---

## TÂCHE E1 — Créer le projet de test xUnit

### Objectif
Créer le premier projet de test avec un test trivial, pour valider que le framework de test est correctement configuré.

### Étape 1 — Créer le projet

Créer `src/McsCore.Tests/McsCore.Tests.csproj` :

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
    <PackageReference Include="xunit" Version="2.9.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\McsCore.Core\McsCore.Core.csproj" />
  </ItemGroup>
</Project>
```

### Étape 2 — Ajouter le projet à la solution

```bash
dotnet sln mcs-project-mvc.sln add src/McsCore.Tests/McsCore.Tests.csproj
```

### Étape 3 — Créer le premier test

Créer `src/McsCore.Tests/Domain/EntityBasicsTests.cs` :

```csharp
using McsCore.Core.Entities;
using Xunit;

namespace McsCore.Tests.Domain;

public class EntityBasicsTests
{
    [Fact]
    public void Customer_HasCorrectDefaultProperties()
    {
        // Arrange & Act
        var customer = new Customer();

        // Assert
        Assert.NotNull(customer);
        Assert.False(customer.IsActive);
    }
}
```

> Note : vérifier que `Customer` a bien une propriété `IsActive` (ou `IsDeleted` ou autre flag de soft delete) avant d'écrire ce test. Adapter le test à la réalité de l'entité.

### Étape 4 — Exécuter les tests

```bash
dotnet test src/McsCore.Tests/
```

Attendu : 1 test passé, 0 échoué.

### Étape 5 — Exclure les tests du build CI/CD

Dans `.github/workflows/build.yml`, ajouter une étape de test après le build :

```yaml
- name: Test
  run: dotnet test mcs-project-mvc.sln --no-restore --verbosity normal
```

---

## VÉRIFICATION GLOBALE

- `dotnet build mcs-project-mvc.sln` passe sans erreur
- `dotnet test mcs-project-mvc.sln` retourne au moins 1 test réussi
- Le projet de test n'est pas inclus dans le Dockerfile (pas de déploiement des tests)

---

## RÈGLES PROJET À RESPECTER

- Pas de dépendance ajoutée sauf les packages xUnit (nécessaires)
- Test trivial pour valider le setup, pas de test métier complexe
- Le projet de test doit être indépendant et ne pas casser le build existant
- Mettre à jour AUDIT.md : "Lot E : projet xUnit créé, 1 test trivial"
- Langue : commentaire du test en anglais (convention xUnit standard)