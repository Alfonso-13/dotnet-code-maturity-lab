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
    private static readonly IReadOnlyDictionary<string, string> CanonicalModuleSlugs =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
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
        };

    // The reverse map lets canonical routes resolve the existing curriculum without
    // leaking route names into lesson catalogs or progress-storage keys.
    private static readonly IReadOnlyDictionary<string, string> InternalModuleSlugs =
        CanonicalModuleSlugs.ToDictionary(pair => pair.Value, pair => pair.Key, StringComparer.OrdinalIgnoreCase);

    public static string Learn(string level) => $"learn/{level}";

    public static string LearnJunior(string internalSlug) =>
        $"learn/junior/{CanonicalModuleSlug(internalSlug)}";

    public static string Lab(string level) => $"labs/{level}";

    public static string InterviewFor(string level) => $"interview/{level}";

    public static string ComingSoon(string section) => $"coming-soon/{section}";

    public static string CanonicalModuleSlug(string internalSlug) =>
        CanonicalModuleSlugs.TryGetValue(internalSlug, out var canonicalSlug)
            ? canonicalSlug
            : internalSlug;

    public static string InternalModuleSlug(string routeSlug) =>
        InternalModuleSlugs.TryGetValue(routeSlug, out var internalSlug)
            ? internalSlug
            : routeSlug;

    public static IReadOnlyDictionary<string, string> JuniorModuleSlugs => CanonicalModuleSlugs;
}
