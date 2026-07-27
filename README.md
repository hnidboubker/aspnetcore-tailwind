# ASP.NET Core Starter Templates with Tailwind CSS 4.3

A collection of modern **ASP.NET Core starter templates** preconfigured with **Tailwind CSS 4.3**, **PostCSS**, and **esbuild**.

These templates provide a clean foundation for building web applications with the latest .NET technologies while keeping the frontend workflow simple, maintainable, and easy to customize.

Whether you're starting a new project or exploring Tailwind CSS with ASP.NET Core, this repository helps you get up and running quickly.

---

## Features

- ASP.NET Core (.NET 8 / .NET 10)
- Tailwind CSS 4.3
- PostCSS integration
- JavaScript bundling with esbuild
- Minimal and clean project structure
- Ready-to-use npm build scripts
- Optimized frontend asset pipeline
- **Custom Identity UI** with Tailwind CSS (Login, Register, Profile, Change Password)
- **MailKit** email sender for Identity
- **Entity Framework Core** with SQL Server LocalDB
- Step-by-step documentation

---

## Available Templates

| Template | Description |
| ---------- | ------------- |
| Razor Pages | ASP.NET Core Razor Pages + custom Identity UI |
| MVC | ASP.NET Core MVC + custom Identity UI |
| Blazor | ASP.NET Core Blazor WebAssembly + custom Identity UI |
| .NET MAUI | .NET MAUI Blazor Hybrid starter project |
| **Identity.Core** | Shared Identity library (models, DbContext, MailKit) |

---

## Identity UI

Each web template includes a **custom Identity UI** built with Tailwind CSS, replacing the default Bootstrap-based pages.

### Pages Included

| Page | Route | Description |
| ------ | ------- | ------------- |
| Login | `/Identity/Account/Login` | Email + password + remember me |
| Register | `/Identity/Account/Register` | First name, last name, email, password |
| Forgot Password | `/Identity/Account/ForgotPassword` | Email input for reset link |
| Profile | `/Identity/Account/Manage` | View/edit first name, last name |
| Change Password | `/Identity/Account/Manage/ChangePassword` | Old/new/confirm password |

### Design

- Clean white cards on light gray background
- Emerald green accent color (`bg-emerald-600`)
- Rounded corners, subtle shadows
- Responsive design
- Form validation with error messages

---

## Getting Started

### Prerequisites

- .NET SDK 8 or later
- Node.js 20 or later
- npm
- SQL Server LocalDB (included with Visual Studio)

### Clone the repository

```bash
git clone https://github.com/<username>/<repository>.cd
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
dotnet run
```

---

## Database Setup

The templates use **Entity Framework Core** with **SQL Server LocalDB**.

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
dotnet ef database update -p Src/TailwindIdentity.Core -s Src/TailwindRazorPage.Web
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

```Src/
├── TailwindIdentity.Core/          # Shared Identity library
│   ├── Models/                     # ApplicationUser, ApplicationRole
│   ├── Data/                       # ApplicationDbContext
│   ├── Services/                   # MailKitEmailSender
│   └── Migrations/                 # EF Core migrations
├── TailwindRazorPage.Web/          # Razor Pages template
│   └── Areas/Identity/Pages/       # Custom Identity pages
├── TailwindMvc.Web/                # MVC template
│   ├── Controllers/                # AccountController, ManageController
│   └── Views/Account/              # Identity views
├── TailwindBlazor.Web/             # Blazor WebAssembly template
│   ├── Server/Components/Account/  # API endpoints
│   └── Client/Components/Account/  # Identity components
└── TailwindMaui.Web/               # MAUI template
```

---

## Frontend Build Pipeline

```npm install
        │
        ▼
Install dependencies
        │
        ▼
postinstall
        │
        ▼
Generate CSS
        │
        ├───────────────┐
        │               │
        ▼               ▼
npm run dev      npm run build
        │               │
        ▼               ▼
Development     Production
        │
        ▼
wwwroot/
```

---

## Requirements

- .NET SDK 8 or later
- Node.js 20 or later
- npm
- SQL Server LocalDB
- Visual Studio 2022 (recommended)
- Visual Studio Code (optional)

---

## Roadmap

- [x] Razor Pages starter
- [x] MVC starter
- [x] Blazor starter
- [x] .NET MAUI starter
- [x] Authentication templates (Identity UI)
- [ ] Docker support
- [ ] GitHub Actions CI/CD
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
