# Graph Report - .  (2026-07-27)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 672 nodes · 726 edges · 85 communities (64 shown, 21 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `a349d4c9`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Community 0
- Community 1
- Community 2
- Community 3
- Community 4
- Community 5
- Community 6
- Community 7
- Community 8
- Community 9
- Community 10
- Community 11
- Community 12
- Community 13
- Community 14
- Community 15
- Community 16
- Community 17
- Community 18
- Community 19
- Community 20
- Community 21
- Community 22
- Community 23
- Community 24
- Community 25
- Community 26
- Community 27
- Community 28
- Community 29
- Community 30
- Community 31
- Community 32
- Community 33
- Community 34
- Community 35
- Community 36
- Community 37
- Community 38
- Community 39
- Community 40
- Community 41
- Community 42
- Community 43
- Community 44
- Community 45
- Community 46
- Community 47
- Community 48
- Community 49
- Community 50
- Community 51
- Community 52
- Community 53
- Community 54
- Community 55
- Community 56
- Community 57
- Community 58
- Community 59
- Community 60
- Community 61
- Community 62
- Community 63

## God Nodes (most connected - your core abstractions)
1. `TailwindIdentity.Core.Models` - 20 edges
2. `AccountController` - 12 edges
3. `TailwindIdentity.Core` - 11 edges
4. `IdentityAuthenticationStateProvider` - 10 edges
5. `DefaultContext` - 10 edges
6. `TailwindMaui.Web` - 10 edges
7. `TailwindMaui.Web` - 10 edges
8. `ChangePasswordModel` - 9 edges
9. `RegisterModel` - 9 edges
10. `LoginModel` - 8 edges

## Surprising Connections (you probably didn't know these)
- `TailwindIdentity.Core` --references--> `Microsoft.NET.Sdk.Razor`  [EXTRACTED]
  Src/TailwindIdentity.Core/TailwindIdentity.Core.csproj → Src/TailwindMaui.Web/TailwindMaui.Web.csproj
- `TailwindBlazor.Web.Client` --references--> `net8.0`  [EXTRACTED]
  Src/TailwindBlazor.Web/TailwindBlazor.Web.Client/TailwindBlazor.Web.Client.csproj → Src/TailwindMvc.Web/TailwindMvc.Web.csproj
- `TailwindBlazor.Web` --references--> `net8.0`  [EXTRACTED]
  Src/TailwindBlazor.Web/TailwindBlazor.Web/TailwindBlazor.Web.csproj → Src/TailwindMvc.Web/TailwindMvc.Web.csproj
- `TailwindBlazor.Web` --references--> `Microsoft.NET.Sdk.Web`  [EXTRACTED]
  Src/TailwindBlazor.Web/TailwindBlazor.Web/TailwindBlazor.Web.csproj → Src/TailwindMvc.Web/TailwindMvc.Web.csproj
- `TailwindIdentity.Core` --references--> `net8.0`  [EXTRACTED]
  Src/TailwindIdentity.Core/TailwindIdentity.Core.csproj → Src/TailwindMvc.Web/TailwindMvc.Web.csproj

## Import Cycles
- None detected.

## Communities (85 total, 21 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (30): TailwindIdentity.Core.Services, TailwindIdentity.Core.Areas.Identity.Pages.Account.Manage, TailwindIdentity.Core.Models, TailwindIdentity.Core.Data, TailwindIdentity.Core, IdentityDbContext, IdentityRole, IdentityRoleClaim (+22 more)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (47): devDependencies, autoprefixer, esbuild, lucide, postcss, postcss-cli, tailwindcss, @tailwindcss/cli (+39 more)

### Community 2 - "Community 2"
Cohesion: 0.09
Nodes (29): net10.0-android, net10.0-ios, net10.0-maccatalyst, net10.0-windows10.0.19041.0, $(TargetFrameworks), MailKit (4.8.0), Microsoft.AspNetCore.Components.WebAssembly (8.0.29), Microsoft.AspNetCore.Components.WebAssembly.Authentication (8.0.11) (+21 more)

### Community 3 - "Community 3"
Cohesion: 0.07
Nodes (28): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, inspectUri, launchBrowser, applicationUrl (+20 more)

### Community 4 - "Community 4"
Cohesion: 0.10
Nodes (19): Controller, TailwindMvc.Web.Models, TailwindMvc.Web.Controllers, ResponseCache, IActionResult, ILogger, HomeController, HttpGet (+11 more)

### Community 5 - "Community 5"
Cohesion: 0.08
Nodes (25): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+17 more)

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (25): ASPNETCORE_ENVIRONMENT, applicationUrl, commandName, dotnetRunMessages, environmentVariables, launchBrowser, applicationUrl, commandName (+17 more)

### Community 7 - "Community 7"
Cohesion: 0.09
Nodes (18): TailwindBlazor.Web.Components.Account, IEndpointRouteBuilder, AccountEndpoints, ChangePasswordRequest, LoginRequest, ProfileRequest, RegisterRequest, Microsoft.AspNetCore.Components.Forms (+10 more)

### Community 8 - "Community 8"
Cohesion: 0.13
Nodes (15): AuthenticationStateProvider, Errors, HttpClient, Task, IdentityAuthenticationStateProvider, LoginResult, RegisterResult, UserInfo (+7 more)

### Community 9 - "Community 9"
Cohesion: 0.22
Nodes (12): HttpGet, HttpPost, IActionResult, ILogger, SignInManager, Task, UserManager, ValidateAntiForgeryToken (+4 more)

### Community 10 - "Community 10"
Cohesion: 0.13
Nodes (9): TailwindIdentity.Core.Migrations, Migration, MigrationBuilder, ModelSnapshot, ModelBuilder, InitialIdentity, InitialIdentity, ModelBuilder (+1 more)

### Community 11 - "Community 11"
Cohesion: 0.13
Nodes (14): BuildFiles, CustomRegexes, HTML, JavaScript, Razor, Override, Values, Override (+6 more)

### Community 12 - "Community 12"
Cohesion: 0.13
Nodes (14): BuildFiles, CustomRegexes, HTML, JavaScript, Razor, Override, Values, Override (+6 more)

### Community 13 - "Community 13"
Cohesion: 0.15
Nodes (11): TailwindBlazor.Web.Client.Components.Account, Microsoft.AspNetCore.Components.Authorization, Microsoft.AspNetCore.Components.Forms, Microsoft.AspNetCore.Components.Routing, Microsoft.AspNetCore.Components.Web, Microsoft.AspNetCore.Components.Web.Virtualization, Microsoft.JSInterop, static (+3 more)

### Community 14 - "Community 14"
Cohesion: 0.14
Nodes (13): author, description, keywords, license, main, name, scripts, build (+5 more)

### Community 15 - "Community 15"
Cohesion: 0.14
Nodes (13): author, description, keywords, license, main, name, scripts, build (+5 more)

### Community 16 - "Community 16"
Cohesion: 0.14
Nodes (13): author, description, keywords, license, main, name, scripts, build (+5 more)

### Community 17 - "Community 17"
Cohesion: 0.14
Nodes (13): author, description, keywords, license, main, name, scripts, build (+5 more)

### Community 18 - "Community 18"
Cohesion: 0.18
Nodes (7): MauiUIApplicationDelegate, MauiApp, MauiProgram, MauiApp, AppDelegate, MauiApp, AppDelegate

### Community 19 - "Community 19"
Cohesion: 0.18
Nodes (5): TailwindMaui.Web, MauiAppCompatActivity, MainActivity, Program, Program

### Community 20 - "Community 20"
Cohesion: 0.24
Nodes (8): IActionResult, ILogger, InputModel, SignInManager, Task, UserManager, ChangePasswordModel, InputModel

### Community 21 - "Community 21"
Cohesion: 0.20
Nodes (8): IActionResult, ILogger, InputModel, SignInManager, Task, UserManager, InputModel, RegisterModel

### Community 22 - "Community 22"
Cohesion: 0.22
Nodes (5): TailwindRazorPage.Web.Pages, ILogger, IndexModel, ILogger, PrivacyModel

### Community 23 - "Community 23"
Cohesion: 0.22
Nodes (8): route:/manage, OnInitializedAsync, HttpClient, NavigationManager, PageTitle, System.Net.Http.Json, SaveProfile, UserInfo

### Community 24 - "Community 24"
Cohesion: 0.22
Nodes (8): Microsoft.AspNetCore.Components.Forms, Microsoft.AspNetCore.Components.Routing, Microsoft.AspNetCore.Components.Web, Microsoft.AspNetCore.Components.Web.Virtualization, Microsoft.JSInterop, System.Net.Http.Json, TailwindMaui.Web, TailwindMaui.Web.Components

### Community 25 - "Community 25"
Cohesion: 0.22
Nodes (7): IActionResult, ILogger, InputModel, SignInManager, Task, InputModel, LoginModel

### Community 26 - "Community 26"
Cohesion: 0.28
Nodes (7): IActionResult, InputModel, SignInManager, Task, UserManager, IndexModel, InputModel

### Community 27 - "Community 27"
Cohesion: 0.25
Nodes (7): ChangePasswordResult, route:/manage/changepassword, HandleChangePassword, HttpClient, NavigationManager, PageTitle, System.Net.Http.Json

### Community 28 - "Community 28"
Cohesion: 0.25
Nodes (7): route:/login, HandleLogin, EditForm, IdentityAuthenticationStateProvider, NavigationManager, PageTitle, TailwindBlazor.Web.Client.Components.Account

### Community 29 - "Community 29"
Cohesion: 0.25
Nodes (7): route:/register, HandleRegister, EditForm, IdentityAuthenticationStateProvider, NavigationManager, PageTitle, TailwindBlazor.Web.Client.Components.Account

### Community 30 - "Community 30"
Cohesion: 0.25
Nodes (5): OnInitialized, PageTitle, ILogger, ErrorModel, System.Diagnostics

### Community 31 - "Community 31"
Cohesion: 0.25
Nodes (6): IActionResult, InputModel, Task, UserManager, ForgotPasswordModel, InputModel

### Community 32 - "Community 32"
Cohesion: 0.29
Nodes (4): TailwindMaui.Web.WinUI, MauiWinUIApplication, MauiApp, App

### Community 33 - "Community 33"
Cohesion: 0.29
Nodes (4): IActivationState, Application, App, Window

### Community 34 - "Community 34"
Cohesion: 0.33
Nodes (5): PageModel, IActionResult, Task, UserManager, ConfirmEmailModel

### Community 35 - "Community 35"
Cohesion: 0.33
Nodes (4): blazorWebView, ContentPage, MainPage, BlazorWebView

### Community 36 - "Community 36"
Cohesion: 0.33
Nodes (5): ApplicationUser, Microsoft.AspNetCore.Identity, SignInManager<ApplicationUser>, TailwindIdentity.Core.Models, UserManager<ApplicationUser>

### Community 37 - "Community 37"
Cohesion: 0.33
Nodes (4): IActionResult, SignInManager, Task, LogoutModel

### Community 38 - "Community 38"
Cohesion: 0.40
Nodes (4): AuthorizeRouteView, FocusOnNavigate, Found, Router

### Community 39 - "Community 39"
Cohesion: 0.40
Nodes (3): MauiApplication, MauiApp, MainApplication

### Community 40 - "Community 40"
Cohesion: 0.40
Nodes (4): route:/forgot-password, HandleSubmit, EditForm, PageTitle

### Community 41 - "Community 41"
Cohesion: 0.40
Nodes (4): RouteView, FocusOnNavigate, Found, Router

### Community 43 - "Community 43"
Cohesion: 0.40
Nodes (4): ApplicationUser, SignInManager<ApplicationUser>, TailwindIdentity.Core.Models, UserManager<ApplicationUser>

## Knowledge Gaps
- **261 isolated node(s):** `route:/forgot-password`, `PageTitle`, `EditForm`, `HandleSubmit`, `UserInfo` (+256 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **21 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `TailwindIdentity.Core.Models` connect `Community 0` to `Community 4`, `Community 37`, `Community 7`, `Community 9`, `Community 44`, `Community 31`?**
  _High betweenness centrality (0.068) - this node is a cross-community bridge._
- **Why does `TailwindIdentity.Core.Data` connect `Community 0` to `Community 10`?**
  _High betweenness centrality (0.014) - this node is a cross-community bridge._
- **Why does `AccountController` connect `Community 9` to `Community 4`?**
  _High betweenness centrality (0.012) - this node is a cross-community bridge._
- **What connects `route:/forgot-password`, `PageTitle`, `EditForm` to the rest of the system?**
  _261 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.05272895467160037 - nodes in this community are weakly interconnected._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05920444033302498 - nodes in this community are weakly interconnected._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.09195402298850575 - nodes in this community are weakly interconnected._