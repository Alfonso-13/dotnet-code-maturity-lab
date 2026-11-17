# .NET Code Maturity Lab

A bilingual, visual, and interactive learning laboratory that shows how a .NET solution grows from language fundamentals into maintainable application code.

The first published stage focuses on the **Junior path**: twelve connected modules, sixty code-first perspectives, and one order-processing laboratory. English is the default language and Spanish can be selected from the interface; the choice and learning progress stay in browser `localStorage`.

> **Live demo:** pending the first GitHub Pages deployment from `main`.

## Junior stage

The same order-processing domain connects all twelve modules:

1. C# fundamentals: data, decisions, and repetition.
2. Methods, constructors, and properties.
3. Value, reference, class, struct, record, enum, and interface.
4. Object-oriented programming and its pillars.
5. Validation, results, and exceptions.
6. Collections, generics, and complexity.
7. LINQ and lambda expressions.
8. `Task`, `Task<T>`, `ValueTask`, `async`, and `await`.
9. Unit tests, HTTP integration tests, and AAA.
10. REST, DTOs, routes, methods, and common HTTP status codes.
11. Relational persistence with SQL and Entity Framework Core.
12. ASP.NET Core composition, middleware, configuration, and dependency injection.

Every module contains five perspectives with copyable, consistently formatted C# examples, an optional side-by-side comparison, contextual hints, and explicit reviewed/completed progress.

## What can be explored

- A maturity roadmap for Junior, Middle, and Senior engineering concerns.
- A visual .NET history timeline from .NET Framework through modern .NET.
- A practical ecosystem guide covering ASP.NET Core, Blazor, Razor Pages, MVC, .NET MAUI, WPF, EF Core, SignalR, and gRPC.
- A functional Junior laboratory for creating orders and observing validation, persistence, and HTTP responses.
- English and Spanish content using one shared component and rendering model.

Middle and Senior content is intentionally marked **Coming soon**. It is not presented as completed work.

## Two execution profiles

The repository preserves the real application and the static portfolio demo without duplicating the learning UI.

| Profile | Project | Persistence | Purpose |
| --- | --- | --- | --- |
| ASP.NET Core | `DotnetCodeMaturityLab.csproj` | EF Core + SQLite | Real endpoints, HTTP integration tests, and local persistence. |
| GitHub Pages | `GitHubPages/DotnetCodeMaturityLab.GitHubPages.csproj` | Browser-session memory | Static WebAssembly portfolio demo with a visible simulation notice. |

GitHub Pages cannot execute ASP.NET Core endpoints or SQLite. The static laboratory therefore runs the same validation and domain flow through `InMemoryOrderStore`; it does not claim to be a deployed server API.

## Run locally

Requirements:

- .NET 10 SDK.

Run the complete ASP.NET Core application:

```powershell
./start-local.ps1
```

or:

```powershell
dotnet run --project DotnetCodeMaturityLab.csproj
```

The server profile creates `App_Data/orders.db`. Local database files, build output, and deployment artifacts are ignored by Git.

Run the static WebAssembly profile:

```powershell
dotnet run --project GitHubPages/DotnetCodeMaturityLab.GitHubPages.csproj
```

## Verification

Run the automated suite:

```powershell
dotnet test tests/DotnetCodeMaturityLab.Tests/DotnetCodeMaturityLab.Tests.csproj -c Release
```

The tests cover the twelve lesson routes, sixty bilingual perspectives, copyable code formatting, explicit reviewed/completed progress, order rules, SQLite persistence, and HTTP integration behavior.

Build and validate the static artifact:

```powershell
./build-github-pages.ps1
```

The script publishes into a staging directory, validates the required Pages files, sets the repository base path, and only then replaces `artifacts/github-pages`.

## GitHub Pages deployment

The workflow at `.github/workflows/deploy-pages.yml` runs only from `main` or by manual dispatch from `main`. It executes the test suite, builds the static artifact, uploads it through the official GitHub Pages actions, and deploys it to the `github-pages` environment.

The repository must use **Settings → Pages → Source: GitHub Actions** before the first deployment.

## Repository structure

```text
Components/             Shared Blazor layout, pages, lessons, and laboratory UI
Features/Learning/      Junior curriculum, bilingual lesson catalog, and metadata
Features/Orders/        Domain, application service, stores, EF Core, and endpoints
Features/Localization/  Language state and shared localization behavior
GitHubPages/            Static WebAssembly host
tests/                  Unit, content-contract, persistence, and HTTP tests
```

## Español

.NET Code Maturity Lab es un laboratorio visual bilingüe para estudiar cómo una solución .NET pasa de los fundamentos de C# a una aplicación Junior completa. Esta primera etapa incluye doce módulos, sesenta perspectivas, historia y ecosistema .NET, ejemplos copiables y un laboratorio de órdenes.

La aplicación ASP.NET Core usa EF Core y SQLite con endpoints reales. La versión de GitHub Pages es una demostración WebAssembly estática que usa memoria de la sesión del navegador y lo comunica explícitamente; no simula tener un servidor desplegado.

El contenido Middle y Senior permanece marcado como **Próximamente** hasta que esté implementado y validado.

## License

This project is available under the [MIT License](LICENSE).
