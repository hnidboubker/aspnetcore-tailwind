# ASP.NET Core Starter Templates with Tailwind CSS 4.3

[![Build](https://github.com/hnidboubker/aspnetcore-tailwind/actions/workflows/build-mvc.yml/badge.svg)](https://github.com/hnidboubker/aspnetcore-tailwind/actions/workflows/build-mvc.yml)
[![Build](https://github.com/hnidboubker/aspnetcore-tailwind/actions/workflows/build-razorpage.yml/badge.svg)](https://github.com/hnidboubker/aspnetcore-tailwind/actions/workflows/build-razorpage.yml)
[![Build](https://github.com/hnidboubker/aspnetcore-tailwind/actions/workflows/build-blazor.yml/badge.svg)](https://github.com/hnidboubker/aspnetcore-tailwind/actions/workflows/build-blazor.yml)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)


A collection of modern **ASP.NET Core starter templates** preconfigured with **Tailwind CSS 4.3**, **PostCSS**, and **esbuild**.

These templates provide a clean foundation for building web applications with the latest .NET technologies while keeping the frontend workflow simple, maintainable, and easy to customize.

Whether you're starting a new project or exploring Tailwind CSS with ASP.NET Core, this repository helps you get up and running quickly.

---

## Features

- ASP.NET Core (.NET 8)
- Tailwind CSS 4.3
- PostCSS integration
- JavaScript bundling with esbuild
- Minimal and clean project structure
- Ready-to-use npm build scripts
- Optimized frontend asset pipeline
- **Custom Identity UI** with Tailwind CSS (Login, Register, Profile, Change Password)
- **MailKit** email sender for Identity
- **Entity Framework Core** with SQL Server LocalDB
- Shared Identity infrastructure (`TailwindIdentity.Core` + `TailwindIdentity.EntityFrameworkCore`)
- GitHub Actions CI/CD for all templates
- Step-by-step documentation

---

## Available Templates

| Template | Description |
| ---------- | ------------- |
| Razor Pages | ASP.NET Core Razor Pages + custom Identity UI |
| MVC | ASP.NET Core MVC + custom Identity UI |
| Blazor | ASP.NET Core Blazor Server + custom Identity UI |
| .NET MAUI | .NET MAUI Blazor Hybrid starter project |
| **TailwindIdentity.Core** | Shared Identity library (models, entities, services) |
| **TailwindIdentity.EntityFrameworkCore** | EF Core Identity implementation (DbContext, migrations) |

---

## Identity UI

Each web template includes a **custom Identity UI** built with Tailwind CSS, replacing the default Bootstrap-based pages.

### Pages Included

| Page | Route | Description |
| ------ | ------- | ------------- |
| Login | `/Account/SignInPage` (Razor Pages) / `/Account/Login` (MVC) | Email + password + remember me |
| Register | `/Account/SignUpPage` (Razor Pages) / `/Account/Register` (MVC) | First name, last name, email, password |
| Forgot Password | `/Account/PasswordForgot` (Razor Pages) / `/Account/ForgotPassword` (MVC) | Email input for reset link |
| Reset Password | `/Account/ResetPassword` | Reset password with token |
| Confirm Email | `/Account/ConfirmEmail` | Email confirmation handler |
| Send Confirmation | `/Account/SendConfirmation` | Resend confirmation email |
| 2FA Login | `/Account/LoginWith2fa` | Two-factor authentication |
| Lockout | `/Account/Lockout` | Account lockout notice |
| Profile | `/Manage/Index` (MVC) / Account pages | User profile management |
| Change Password | `/Manage/ChangePassword` (MVC) | Password update |

### Design

- **Two layouts (Razor Pages):**
  - `_Layout.cshtml` — Main layout with collapsible sidebar for authenticated pages (Index, Privacy)
  - `_LayoutAccount.cshtml` — Minimal layout with footer only (no header/sidebar) for Account and Legal pages
- **MVC Layout**: Standard `_Layout.cshtml` with `_LoginPartial.cshtml`
- Clean white cards on light gray background
- Emerald green accent color (`bg-emerald-600`)
- Rounded corners, subtle shadows
- Responsive design
- Form validation with error messages
- **Index page requires authentication** — redirects to login if not authenticated

### Legal Pages

- CGV (Conditions Générales de Vente)
- CGU (Conditions Générales d'Utilisation)
- Confidentialité (Privacy Policy)
- RGPD (GDPR Compliance)

All legal pages use `_LayoutAccount` layout (Razor Pages) or standard layout (MVC). Accessible from footer/sidebar links.

---

## Getting Started

### Prerequisites

- .NET SDK 8 or later
- Node.js 20 or later
- npm
- SQL Server LocalDB (included with Visual Studio)

### Clone the repository

```bash
git clone https://github.com/hnidboubker/aspnetcore-tailwind.git
```

### Install dependencies

```bash
npm install
```

### Start the development pipeline

```bash
npm run dev
```

### Build production assets

```bash
npm run build
```

### Run the application

```bash
dotnet run --project Src/TailwindRazorPage.Web
```

---

## Database Setup

The templates use **Entity Framework Core** with **SQL Server LocalDB**. Migrations are in `TailwindIdentity.EntityFrameworkCore`.

### Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TailwindIdentity;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True"
  }
}
```

### Create Database

```bash
dotnet ef database update -p Src/TailwindIdentity.EntityFrameworkCore -s Src/TailwindRazorPage.Web
```

### Email Configuration (MailKit)

Configure SMTP settings in `appsettings.json`:

```json
{
  "Email": {
    "From": "noreply@yourdomain.com",
    "SmtpHost": "smtp.yourdomain.com",
    "SmtpPort": "587",
    "SmtpUser": "your-user",
    "SmtpPassword": "your-password"
  }
}
```

---

## Project Structure

```text
Src/
├── TailwindIdentity.Core/               # Shared Identity library (entities, services)
│   ├── Entities/                        # ApplicationUser, ApplicationRole
│   ├── Data/                            # DefaultContext
│   ├── Persistence/                     # Entity configurations
│   ├── Services/                        # MailKitEmailSender
│   └── Migrations/
├── TailwindIdentity.EntityFrameworkCore/ # EF Core Identity implementation
│   ├── Extensions/                      # AddTailwindIdentity()
│   ├── Persistence/                     # DefaultContext
│   ├── Migrations/                      # EF Core migrations
│   ├── Seeds/                           # Data seeding
│   └── Services/
├── TailwindRazorPage.Web/               # Razor Pages template
│   ├── Pages/Account/                   # Custom Identity pages (SignInPage, SignUpPage, etc.)
│   ├── Pages/Legal/                     # Legal pages (CGV, CGU, Confidentialite, RGPD)
│   ├── Pages/Shared/                    # _Layout.cshtml, _LayoutAccount.cshtml
│   ├── Persistence/                     # DefaultContext, EmailMessage model
│   └── Services/                        # MailKitEmailSender, EmailOptions, IEmailService
├── TailwindMvc.Web/                     # MVC template
│   ├── Controllers/                     # AccountController, HomeController, LegalController, ManageController
│   ├── Models/                          # ErrorViewModel
│   ├── Views/
│   │   ├── Account/                     # Identity views (Login, Register, etc.)
│   │   ├── Home/                        # Index, Privacy
│   │   ├── Legal/                       # CGU, CGV, Confidentiality, RGPD
│   │   ├── Manage/                      # Profile, ChangePassword
│   │   └── Shared/                      # _Layout, _LoginPartial
│   ├── Program.cs
│   ├── wwwroot/css/                     # app.css (source), style.css (compiled)
│   └── package.json / postcss.config.js
├── TailwindBlazor.Web/                  # Blazor Server template
│   ├── TailwindBlazor.Web/              # Server project
│   │   ├── Components/
│   │   │   ├── Layout/
│   │   │   ├── Pages/
│   │   │   └── Account/                 # Identity components
│   │   ├── wwwroot/
│   │   └── Program.cs
│   └── TailwindBlazor.Web.Client/       # Client project (WASM)
└── TailwindMaui.Web/                    # MAUI Blazor Hybrid template
    ├── Components/
    │   ├── Pages/
    │   ├── Layout/
    │   └── Account/
    ├── Platforms/                       # Android, iOS, MacCatalyst, Windows
    ├── wwwroot/
    └── MauiProgram.cs
```

---

## Frontend Build Pipeline

Each web template includes its own `package.json` with Tailwind CSS 4.3, PostCSS, and esbuild.

### Razor Pages (`Src/TailwindRazorPage.Web/package.json`)

```bash
npm install           # Install dependencies + auto-build (postinstall)
npm run dev           # Development: postcss watch mode
npm run build         # Production: postcss compile
npm run css:build     # Minified production: Tailwind CLI
npm run build:js      # Bundle JavaScript with esbuild
```

### MVC (`Src/TailwindMvc.Web/package.json`)

```bash
npm install           # Install dependencies
npm run dev           # Development: postcss watch mode
npm run build         # Production: postcss compile
npm run css:build     # Minified production: Tailwind CLI
npm run build:js      # Bundle JavaScript with esbuild
```

### Blazor Server (`Src/TailwindBlazor.Web/TailwindBlazor.Web/package.json`)

```bash
npm install           # Install dependencies
npm run dev           # Development: postcss watch mode
npm run build         # Production build
```

### MAUI (`Src/TailwindMaui.Web/package.json`)

```bash
npm install           # Install dependencies
npm run dev           # Development: postcss watch mode
npm run build         # Production build
```

The `Tailwind` MSBuild target in each `.csproj` runs `npm run css:build` before each .NET build.

---

## Requirements

- .NET SDK 8 or later
- Node.js 20 or later
- npm
- SQL Server LocalDB
- Visual Studio 2022 (recommended)
- Visual Studio Code (optional)
- For MAUI: .NET MAUI workload (`dotnet workload install maui`)

---

## Roadmap

- [x] Razor Pages starter
- [x] MVC starter
- [x] Blazor Server starter
- [x] .NET MAUI Blazor Hybrid starter
- [x] Authentication templates (Identity UI)
- [x] GitHub Actions CI/CD (build-mvc, build-razorpage, build-blazor)
- [ ] Docker support
- [ ] Additional UI examples
- [ ] Role-based authorization

---

## Contributing

Contributions, suggestions, and bug reports are welcome.

If you would like to contribute:

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Open a Pull Request.

Please ensure your changes follow the existing coding style and include documentation where appropriate.

---

## License

This project is licensed under the **MIT License**.

See the [LICENSE](LICENSE) file for details.

---

## Acknowledgements

This project is built with:

- ASP.NET Core
- Tailwind CSS
- PostCSS
- esbuild
- MailKit
- Entity Framework Core

Thanks to the .NET and Tailwind CSS communities for their excellent tools and documentation.
