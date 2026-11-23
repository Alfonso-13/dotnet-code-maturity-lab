namespace DotnetCodeMaturityLab.Features.Navigation;

/// <summary>
/// Canonical, host-relative application routes. These values intentionally omit
/// a leading slash so Blazor keeps the configured base path (for example the
/// GitHub Pages repository path) when it generates a URL.
/// </summary>
public static class AppRoutes
{
    public const string Home = "";
    public const string Roadmap = "roadmap";
    public const string Labs = "labs";
    public const string DotNetHistory = "dotnet-history";
    public const string DotNetEcosystem = "dotnet-ecosystem";
    public const string Interview = "interview";

    // Curriculum keys remain stable internal identifiers. Only this boundary translates
    // them into English-first public URLs, which avoids duplicating or rewriting content.
    private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> CanonicalModuleSlugsByLevel =
        new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["junior"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["fundamentos-csharp"] = "csharp-fundamentals",
                ["metodos-y-clases"] = "methods-and-classes",
                ["modelos-y-memoria"] = "models-and-memory",
                ["poo"] = "object-oriented-programming",
                ["errores-y-validacion"] = "errors-and-validation",
                ["colecciones-y-genericos"] = "collections-generics-complexity",
                ["linq"] = "linq",
                ["task-async-await"] = "task-async-await",
                ["pruebas-aaa"] = "testing-and-aaa",
                ["rest-http"] = "rest-dtos-http",
                ["persistencia-ef-core"] = "sql-and-ef-core",
                ["aspnet-core"] = "aspnet-core",
                ["ia-llm"] = "ai-and-llms"
            },
            ["middle"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["solid"] = "applied-solid",
                ["inyeccion-dependencias"] = "dependency-injection",
                ["strategy-patrones"] = "strategy-and-patterns",
                ["refactorizacion"] = "safe-refactoring",
                ["ef-core"] = "ef-core-persistence",
                ["problem-details"] = "problem-details",
                ["pruebas-integracion"] = "integration-testing",
                ["limites-arquitectura"] = "architecture-boundaries"
            },
            ["senior"] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["idempotencia"] = "idempotency",
                ["concurrencia"] = "concurrency-and-consistency",
                ["resiliencia"] = "resilience",
                ["mensajeria-outbox"] = "messaging-and-outbox",
                ["observabilidad"] = "observability",
                ["seguridad-api"] = "api-security",
                ["decisiones-arquitectura"] = "architecture-decisions",
                ["azure-escalabilidad"] = "azure-and-scalability"
            }
        };

    // The reverse map lets canonical routes resolve the existing curriculum without
    // leaking route names into lesson catalogs or progress-storage keys.
    private static readonly IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> InternalModuleSlugsByLevel =
        CanonicalModuleSlugsByLevel.ToDictionary(
            level => level.Key,
            level => (IReadOnlyDictionary<string, string>)level.Value.ToDictionary(
                pair => pair.Value,
                pair => pair.Key,
                StringComparer.OrdinalIgnoreCase),
            StringComparer.OrdinalIgnoreCase);

    public static string Learn(string level) => $"learn/{level}";

    public static string LearnModule(string level, string internalSlug) =>
        $"learn/{level}/{CanonicalModuleSlug(level, internalSlug)}";

    public static string LearnJunior(string internalSlug) =>
        LearnModule("junior", internalSlug);

    public static string Lab(string level) => $"labs/{level}";

    public static string InterviewFor(string level) => $"interview/{level}";

    public static string ComingSoon(string section) => $"coming-soon/{section}";

    public static string CanonicalModuleSlug(string internalSlug) =>
        CanonicalModuleSlug("junior", internalSlug);

    public static string CanonicalModuleSlug(string level, string internalSlug) =>
        CanonicalModuleSlugsByLevel.TryGetValue(level, out var slugs)
        && slugs.TryGetValue(internalSlug, out var canonicalSlug)
            ? canonicalSlug
            : internalSlug;

    public static string InternalModuleSlug(string routeSlug) =>
        InternalModuleSlug("junior", routeSlug);

    public static string InternalModuleSlug(string level, string routeSlug) =>
        InternalModuleSlugsByLevel.TryGetValue(level, out var slugs)
        && slugs.TryGetValue(routeSlug, out var internalSlug)
            ? internalSlug
            : routeSlug;

    public static IReadOnlyDictionary<string, string> ModuleSlugs(string level) =>
        CanonicalModuleSlugsByLevel.TryGetValue(level, out var slugs)
            ? slugs
            : new Dictionary<string, string>();

    public static IReadOnlyDictionary<string, string> JuniorModuleSlugs => ModuleSlugs("junior");
}
