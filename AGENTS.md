# AGENTS.md

Concise operating guide for future agents working on this repo. Only verified, repo-specific facts.

## What this repo is

A solution of **independent ASP.NET Core starter templates** that each integrate the same frontend stack (Tailwind CSS 4.3 + PostCSS + esbuild). It is **not** a single application — each `Src/*` project must run on its own.

- Solution file: `aspnetcore-tailwind.slnx` (XML `.slnx` format, folders: `/Blazor`, `/Core`, `/Maui`, `/Mvc`, `/RazorPage`)
- 5 projects: `TailwindIdentity.Core` (shared lib), `TailwindRazorPage.Web`, `TailwindMvc.Web`, `TailwindBlazor.Web` (Server + Client), `TailwindMaui.Web`

## Build & run

- **Requires**: .NET SDK 8+, Node.js 20+, npm, SQL Server LocalDB
- `npm install` runs the Tailwind CSS build automatically (`postinstall` → `npm run build`)
- `npm run dev` (per project) for watch mode; `npm run css:build` for minified prod CSS
- Each `.csproj` has an MSBuild target `Tailwind` running `npm run css:build` **BeforeTargets="Build"** — every `dotnet build` invokes npm. Ensure `node_modules` exists first.
- `dotnet ef` CLI is used at v10.0.8 globally. Migrations are generated against `TailwindIdentity.Core` from any web project as startup.

## EF Core migrations

- Migrations live in `Src/TailwindIdentity.Core/Migrations/`. The DbContext class is `DefaultContext` (not `ApplicationDbContext`).
- Generate: `dotnet ef migrations add <Name> -p Src/TailwindIdentity.Core -s Src/<WebProject> --framework net8.0`
- Apply: `dotnet ef database update -p Src/TailwindIdentity.Core -s Src/<WebProject>`
- All web projects reference `Microsoft.EntityFrameworkCore.Design` (8.0.11) so `dotnet ef` works from any of them.

## Identity architecture (Custom UI with Tailwind)

- **Shared library** `TailwindIdentity.Core`:
  - `Models/`: `ApplicationUser : IdentityUser<int>`, `ApplicationRole : IdentityRole<int>`, plus `ApplicationUserClaim`, `ApplicationUserRole`, `ApplicationUserLogin`, `ApplicationRoleClaim`, `ApplicationUserToken`. Keys are **`int`**, not GUIDs.
  - `Data/DefaultContext.cs`: `IdentityDbContext<...>` with explicit table mappings (PascalCase tables: `User`, `Role`, `UserRole`, `UserClaim`, `UserToken`, `RoleClaim`).
  - `Services/MailKitEmailSender.cs`: implements `IEmailSender` from `Microsoft.AspNetCore.Identity.UI`. Reads `Email:SmtpHost/Port/User/Password` and `Email:From`.
  - `IdentityServiceExtensions.AddTailwindIdentity(...)`: one-line DI setup — **must be called in each web project's `Program.cs`**.
- **Tailwind palette**: emerald-600 (`bg-emerald-600`, `ring-emerald-600`, `hover:bg-emerald-500`). Light background (`bg-gray-50`). Cards use `rounded-xl bg-white shadow-sm ring-1 ring-gray-900/5`.

### Per-template wiring

| Project | Auth UI location | Notes |
|---|---|---|
| `TailwindRazorPage.Web` | `Areas/Identity/Pages/Account/` + `Areas/Identity/Pages/Account/Manage/` | Razor Pages; uses `<partial name="_LoginPartial" />` from `Pages/Shared/_LoginPartial.cshtml` |
| `TailwindMvc.Web` | `Controllers/AccountController.cs`, `Controllers/ManageController.cs`, `Views/Account/`, `Views/Manage/` | `[Authorize]` on `ManageController` |
| `TailwindBlazor.Web` | Server: `Components/Account/AccountEndpoints.cs` (minimal API); Client: `Components/Account/` (Login.razor, Register.razor, ForgotPassword.razor, Manage/Index.razor, Manage/ChangePassword.razor, LoginDisplay.razor, IdentityAuthenticationStateProvider.cs) | Uses `Microsoft.AspNetCore.Components.WebAssembly.Authentication` 8.0.11 in Client |
| `TailwindMaui.Web` | None | Out of scope for Identity |

## Razor Pages partial path quirk

**`<partial name="_LoginPartial" />` does NOT search Areas.** Despite `area="Identity"` on tag helpers, the partial resolution searches the standard shared folders only:
- `/Pages/_LoginPartial.cshtml`
- `/Pages/Shared/_LoginPartial.cshtml`
- `/Views/Shared/_LoginPartial.cshtml`

If a partial must be reachable from a non-Area layout, place it in `Pages/Shared/` even when the content is Identity-related. Tag helpers inside the partial still need `asp-area="Identity"` for correct routing.

## Blazor client project — gotchas

- Blazor WASM Client **cannot reference Server types**. The `IdentityAuthenticationStateProvider` lives in the **Client** project, not Server.
- Client uses a plain `HttpClient` (registered scoped in Client `Program.cs`); Server does NOT use `IHttpClientFactory` here — the previous pattern was removed.
- Components render with `@rendermode InteractiveWebAssembly`.
- `Client/_Imports.razor` must include `Microsoft.AspNetCore.Components.Authorization` for `AuthenticationState` cascading parameter to resolve.
- `Program.cs` in Client must register `AddAuthorizationCore()`, `AddCascadingAuthenticationState()`, and `AddSingleton<IdentityAuthenticationStateProvider>()`.

## Tailwind CSS v4 specifics

- Tailwind v4 uses CSS-first config (`@import "tailwindcss";` in `wwwroot/css/app.css`). There is **no `tailwind.config.js`** anywhere in the repo.
- CSS pipeline: `app.css` → `style.css` (built, served). Layouts reference `~/css/style.css` with `asp-append-version="true"`.
- `MAUI/wwwroot/index.html` initially referenced the source `css/app.css` — it was corrected to `css/style.css`.
- MAUI's `refreshIcons()` JS function was previously commented out; it is now uncommented in `MAUI/wwwroot/index.html`.

## Legacy artifacts already removed (do not re-add)

These were cleaned up — do not reintroduce:
- `wwwroot/lib/` (jQuery, Bootstrap, jquery-validation) in Razor Pages & MVC
- `libman.json` (Razor Pages)
- `_Layout.cshtml.css`, `site.js`, `site.css`, `_ValidationScriptsPartial.cshtml` (Razor Pages & MVC)
- `app.output.css` (Razor Pages build artifact)
- `wwwroot/style.css` empty file (Blazor)
- `<Folder Include="NewFolder\" />` reference in Razor Pages `.csproj`
- Solution folder typo `Balzor/` was corrected to `Blazor/`
- `.csproj.user` files (already ignored via `*.user` in `.gitignore`)
- Autoprefixer version pinned to `^10.5.4` across all 4 `package.json` files

## appsettings.json connection string (per web project)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TailwindIdentity;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True"
  },
  "Email": {
    "From": "noreply@tailwind.local",
    "SmtpHost": "localhost",
    "SmtpPort": "587",
    "SmtpUser": "",
    "SmtpPassword": ""
  },
  "Identity": {
    "RequireConfirmedEmail": false
  }
}
```

## No tests, no CI

- No test projects in the solution. Do not assume `dotnet test` works.
- `.github/workflows/` is empty. No CI runs.
- No Docker, no `.devcontainer`.

## Git workflow

- Conventional Commits style observed in recent history (`fix:`, `feat:`, `docs:`).
- Branch: `master` is the default.
- No protected branch rules visible — push directly if asked.
