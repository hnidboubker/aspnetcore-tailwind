# AGENTS.md

Concise operating guide for future agents. Only verified, repo-specific facts.

## What this repo is

A solution of **independent ASP.NET Core starter templates** integrating Tailwind CSS 4.3 + PostCSS + esbuild. Not a single app — each `Src/*` project runs standalone.

- Solution file: `aspnetcore-tailwind.slnx` (XML `.slnx`, folders: `/Blazor`, `/Core`, `/Maui`, `/Mvc`, `/RazorPage`)
- 5 projects: `TailwindIdentity.Core` (shared lib), `TailwindRazorPage.Web`, `TailwindMvc.Web`, `TailwindBlazor.Web` (Server + Client), `TailwindMaui.Web`
- Identity UI is **custom** (Tailwind, emerald-600 accent) — NOT the default Bootstrap-based UI

## Build & run

- Requires: .NET SDK 8+, Node.js 20+, npm, SQL Server LocalDB
- `npm install` triggers `postinstall` → `npm run build` (Tailwind compiles automatically)
- Per-project commands: `npm run dev` (watch), `npm run css:build` (minified prod)
- Each `.csproj` has MSBuild target `Tailwind` running `npm run css:build` **BeforeTargets="Build"** — every `dotnet build` invokes npm
- `dotnet ef` CLI is installed globally at v10.0.8
- Dev ports: Razor Pages 5063/7016, MVC 5253/7178, Blazor Server 5064/7243 (Blazor Client WASM has no dev port — runs in browser)

## EF Core migrations

- Migrations live in `Src/TailwindIdentity.Core/Migrations/`
- DbContext class is `DefaultContext` (not `ApplicationDbContext`)
- Generate: `dotnet ef migrations add <Name> -p Src/TailwindIdentity.Core -s Src/<WebProject> --framework net8.0`
- Apply: `dotnet ef database update -p Src/TailwindIdentity.Core -s Src/<WebProject>`
- All web projects reference `Microsoft.EntityFrameworkCore.Design` 8.0.11 so `dotnet ef` works from any of them

## Identity architecture

- **Shared library** `TailwindIdentity.Core`:
  - `Models/`: `ApplicationUser : IdentityUser<int>`, `ApplicationRole : IdentityRole<int>`, plus `ApplicationUserClaim`, `ApplicationUserRole`, `ApplicationUserLogin`, `ApplicationRoleClaim`, `ApplicationUserToken`. Keys are **`int`**, not GUIDs
  - `Data/DefaultContext.cs`: `IdentityDbContext<...>` with PascalCase table mappings (`User`, `Role`, `UserRole`, `UserClaim`, `UserToken`, `RoleClaim`)
  - `Services/MailKitEmailSender.cs`: implements `IEmailSender` from `Microsoft.AspNetCore.Identity.UI`
  - `IdentityServiceExtensions.AddTailwindIdentity(...)`: one-line DI setup called in each web project's `Program.cs`
- **Tailwind palette**: emerald-600 (`bg-emerald-600`, `ring-emerald-600`, `hover:bg-emerald-500`), light gray bg (`bg-gray-50`), cards (`rounded-xl bg-white shadow-sm ring-1 ring-gray-900/5`)

### Per-template wiring

| Project | Auth UI location | Notes |
|---|---|---|
| `TailwindRazorPage.Web` | `Pages/Account/` (`SignInPage`, `SignUpPage`, `PasswordForgot`, `ResetPassword`, `ConfirmEmail`, `SendConfirmation`, `Lockout`, `LoginWith2fa`) | Razor Pages. Migrated off `TailwindIdentity.Core` to the **Hasim** library family (`AppUser`/`AppRole` from `Hasim.Core`; `DefaultContext` in `Persistence/` extends Hasim's `AuditIdentityContext`). Login uses **UserNameOrEmail**. `Program.cs` uses Hasim Injectify module + `AddIdentity<AppUser, AppRole>()`. Main layout (`_Layout.cshtml`) has a collapsible sidebar (280px / 70px) for authenticated pages. Account and Legal pages use `_LayoutAccount.cshtml` (footer only, no header/sidebar). `appsettings.json` includes `Email` + `Identity` sections. Emails are sent via **MailKit** (`Services/MailKitEmailSender.cs`) and every attempt is persisted to the `EmailMessage` table (`Persistence/Models/EmailMessage.cs`). **Index page requires authentication** — redirects to SignInPage if not authenticated. Legal pages (`Pages/Legal/`: CGV, CGU, Confidentialite, RGPD) use `_LayoutAccount` and are linked in both layouts. |
| `TailwindMvc.Web` | `Controllers/AccountController.cs`, `Controllers/ManageController.cs`, `Views/Account/`, `Views/Manage/` | `[Authorize]` on `ManageController` |
| `TailwindBlazor.Web` | Server: `Components/Account/AccountEndpoints.cs` (minimal API); Client: `Components/Account/` (Login.razor, Register.razor, ForgotPassword.razor, Manage/Index.razor, Manage/ChangePassword.razor, LoginDisplay.razor, IdentityAuthenticationStateProvider.cs) | Uses `Microsoft.AspNetCore.Components.WebAssembly.Authentication` 8.0.11 in Client |
| `TailwindMaui.Web` | None | Out of scope for Identity |

## Razor Pages layouts & partials

**Two layouts:**
- `_Layout.cshtml` — main layout with collapsible sidebar (280px/70px), for authenticated pages (Index, Privacy, etc.)
- `_LayoutAccount.cshtml` — minimal layout without header/sidebar, only footer with legal links, for Account pages and Legal pages

**`<partial name="_LoginPartial" />` does NOT search Areas.** It resolves from standard shared folders only:
- `/Pages/_LoginPartial.cshtml`
- `/Pages/Shared/_LoginPartial.cshtml`
- `/Views/Shared/_LoginPartial.cshtml`

The partial lives in `TailwindRazorPage.Web/Pages/Shared/_LoginPartial.cshtml`. **Note:** The Razor Page template no longer uses an `Identity` Area — the `Areas/Identity/Pages/Account/` folder was removed and account pages now live under `Pages/Account/`. Since pages are no longer in an Area, `asp-area` is not required on their tag helpers.

**Legal pages** (`Pages/Legal/CGV.cshtml`, `CGU.cshtml`, `Confidentialite.cshtml`, `RGPD.cshtml`) use `_LayoutAccount` and are linked in both layout footers/sidebars.

## Blazor client gotchas

- Blazor WASM Client **cannot reference Server types**. The `IdentityAuthenticationStateProvider` lives in the **Client** project
- Client uses a plain `HttpClient` (registered scoped in Client `Program.cs`); Server does NOT use `IHttpClientFactory` here
- Components render with `@rendermode InteractiveWebAssembly`
- `Client/_Imports.razor` must include `Microsoft.AspNetCore.Components.Authorization` for `AuthenticationState` cascading parameter
- `Program.cs` in Client must register `AddAuthorizationCore()`, `AddCascadingAuthenticationState()`, and `AddSingleton<IdentityAuthenticationStateProvider>()`

## Tailwind CSS v4 specifics

- No `tailwind.config.js` — CSS-first config (`@import "tailwindcss";` in `wwwroot/css/app.css`)
- CSS pipeline: `app.css` → `style.css` (built, served). Layouts reference `~/css/style.css` with `asp-append-version="true"`
- `MAUI/wwwroot/index.html` references `css/style.css` (corrected from earlier `css/app.css`)
- MAUI `refreshIcons()` JS function is uncommented in `MAUI/wwwroot/index.html`

## appsettings.json — required keys per web project

`AddTailwindIdentity(...)` reads from `IConfiguration`. Each web project's `appsettings.json` must contain:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TailwindIdentity;Trusted_Connection=True;Encrypt=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
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

Currently **only `TailwindRazorPage.Web/appsettings.json` has these keys**. MVC and Blazor Server still need them added — otherwise `GetConnectionString("DefaultConnection")` will throw at startup.

## Legacy artifacts already removed (do not re-add)

- `wwwroot/lib/` (jQuery, Bootstrap, jquery-validation) in Razor Pages & MVC
- `libman.json` (Razor Pages)
- `_Layout.cshtml.css`, `site.js`, `site.css`, `_ValidationScriptsPartial.cshtml` (Razor Pages & MVC)
- `app.output.css` (Razor Pages build artifact)
- `wwwroot/style.css` empty file (Blazor)
- `<Folder Include="NewFolder\" />` reference in Razor Pages `.csproj`
- Solution folder typo `Balzor/` was corrected to `Blazor/`
- `.csproj.user` files (already ignored via `*.user` in `.gitignore`)
- Autoprefixer pinned to `^10.5.4` across all 4 `package.json` files

## No tests, no CI

- No test projects. Do not assume `dotnet test` works
- `.github/workflows/` is empty. No CI runs
- No Docker, no `.devcontainer`

## Git workflow

- Conventional Commits style observed in recent history (`fix:`, `feat:`, `docs:`)
- Branch: `master` is the default
- No protected branch rules visible — push directly if asked
