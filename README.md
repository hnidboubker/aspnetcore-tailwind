# ASP.NET Core Starter Templates with Tailwind CSS 4.3

A collection of modern **ASP.NET Core starter templates** preconfigured with **Tailwind CSS 4.3**, **PostCSS**, and **esbuild**.

These templates provide a clean foundation for building web applications with the latest .NET technologies while keeping the frontend workflow simple, maintainable, and easy to customize.

Whether you're starting a new project or exploring Tailwind CSS with ASP.NET Core, this repository helps you get up and running quickly.

---

## Features

- ASP.NET Core (.NET 8 / .NET 9)
- Tailwind CSS 4.3
- PostCSS integration
- JavaScript bundling with esbuild
- Minimal and clean project structure
- Ready-to-use npm build scripts
- Optimized frontend asset pipeline
- Step-by-step documentation

---

## Available Templates

| Template | Description |
|----------|-------------|
| Razor Pages | ASP.NET Core Razor Pages starter template |
| MVC | ASP.NET Core MVC starter template |
| Blazor | ASP.NET Core Blazor starter template |
| .NET MAUI | .NET MAUI starter project |

---

## Getting Started

### Clone the repository

```bash
git clone https://github.com/<username>/<repository>.git
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

### Bundle JavaScript

```bash
npm run build:js
```

### Run the application

```bash
dotnet run
```

---

## Project Structure

```
src/
│
├── RazorPages/
├── MVC/
├── Blazor/
└── Maui/
```

Frontend assets are generated inside:

```
wwwroot/
├── css/
└── js/
```

---

# Documentation

This repository includes detailed documentation explaining every step of the frontend setup.

## Step-by-Step Guides

- Project initialization
- Installing Node.js and npm
- Installing Tailwind CSS 4.3
- Configuring PostCSS
- Configuring `package.json`
- Configuring CSS
- Building frontend assets
- Development workflow
- Production deployment

Each guide explains **what is being configured, why it is required, and how it works**.

---

## Frontend Build Pipeline

```
npm install
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

## Development Workflow

During development:

```bash
npm run dev
```

The CSS pipeline watches for changes and automatically regenerates the output files.

For production:

```bash
npm run build
npm run build:js
```

The generated assets are placed in the `wwwroot` folder and served directly by ASP.NET Core.

---

## Requirements

- .NET SDK 8 or later
- Node.js 20 or later
- npm
- Visual Studio 2022
- Visual Studio Code (optional)

---

## Roadmap

- Razor Pages starter
- MVC starter
- Blazor starter
- .NET MAUI starter
- Additional UI examples
- Authentication templates
- Docker support
- GitHub Actions CI/CD

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

Thanks to the .NET and Tailwind CSS communities for their excellent tools and documentation.
