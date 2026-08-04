# Skill: Lot A — HTTPS & CORS

Quick wins sécurité. Réactiver HTTPS + politique CORS explicite.

---

## TÂCHE A1 — Réactiver UseHttpsRedirection dans McsCore.Mvc

Fichier : src/McsCore.Mvc/Program.cs

Action :
1. Chercher la ligne `app.UseHttpsRedirection();` (elle existe, simplement commentée)
2. La décommenter
3. Vérifier qu'elle est bien placée après `app.UseStaticFiles()` et avant `app.UseRouting();`

Vérification : build OK, puis lancer l'app → navigation HTTP redirige vers HTTPS.

---

## TÂCHE A2 — Réactiver UseHttpsRedirection dans McsCore.Host

Fichier : src/McsCore.Host/Program.cs — même logique que A1.

---

## TÂCHE A3 — Créer politique CORS explicite dans McsCore.Host

Fichier : src/McsCore.Host/Program.cs

Ajout 1 — Service (après builder.Services.AddControllers()) :

```csharp
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("McsCorePolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

Ajout 2 — Middleware (avant app.UseAuthorization() ou app.MapControllers()) :

```csharp
app.UseCors("McsCorePolicy");
```

Ajout 3 — appsettings.json dans src/McsCore.Host/appsettings.json :

```json
"AllowedOrigins": [
  "https://localhost",
  "https://localhost:3000"
]
```

Vérification : build OK, origine listée autorisée, origine inconnue → 403 attendu.

---

RÈGLES PROJET : pas de suppression, changements minimaux, fichier touché unique, mettre à jour AUDIT.md après chaque tâche, langue français.