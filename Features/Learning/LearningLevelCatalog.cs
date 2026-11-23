using DotnetCodeMaturityLab.Features.Localization;
using DotnetCodeMaturityLab.Features.Navigation;

namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record LearningModuleDefinition(
    LocalizedText Title,
    string Icon,
    string[] Slugs);

public sealed record LearningLevelDefinition(
    string Key,
    string Label,
    LocalizedText Stage,
    LocalizedText Verb,
    LocalizedText Goal,
    LocalizedText LabTitle,
    LocalizedText LabDescription,
    bool IsAvailable,
    IReadOnlyList<LearningModuleDefinition> Modules)
{
    public string LabRoute => IsAvailable ? AppRoutes.Lab(Key) : AppRoutes.ComingSoon($"{Key}-lab");
}

/// <summary>
/// Single source of truth for the public maturity levels and their module map.
/// Level-specific catalogs still own lesson, interview, and laboratory content.
/// </summary>
public static class LearningLevelCatalog
{
    public static IReadOnlyList<LearningLevelDefinition> Levels { get; } =
    [
        new(
            "junior",
            "Junior",
            Text("Fundamentals", "Fundamentos"),
            Text("Build", "Construir"),
            Text("Build a correct API", "Construye una API correcta"),
            Text("Create an order correctly", "Crear una orden correctamente"),
            Text(
                "Integrate language, domain, persistence, tests, and the HTTP contract.",
                "Integra lenguaje, dominio, persistencia, pruebas y contrato HTTP."),
            true,
            [
                Module("C# fundamentals", "Fundamentos de C#", "</>", "fundamentos-csharp"),
                Module("Methods and classes", "Métodos y clases", "ƒ", "metodos-y-clases"),
                Module("Models and memory", "Modelos y memoria", "◫", "modelos-y-memoria"),
                Module("Object-oriented programming", "Programación orientada a objetos", "◎", "poo"),
                Module("Errors and validation", "Errores y validación", "!", "errores-y-validacion"),
                Module("Collections and complexity", "Colecciones y complejidad", "▤", "colecciones-y-genericos"),
                Module("Lambdas and LINQ", "Lambdas y LINQ", "λ", "linq"),
                Module("Task and async", "Task y async", "⇢", "task-async-await"),
                Module("Tests and AAA", "Pruebas y AAA", "△", "pruebas-aaa"),
                Module("REST and HTTP", "REST y HTTP", "↗", "rest-http"),
                Module("SQL and EF Core", "SQL y EF Core", "▱", "persistencia-ef-core"),
                Module("ASP.NET Core", "ASP.NET Core", "⌘", "aspnet-core"),
                Module("AI and LLMs", "IA y LLM", "✦", "ia-llm")
            ]),
        new(
            "middle",
            "Middle",
            Text("Design", "Diseño"),
            Text("Design", "Diseñar"),
            Text("Make the API maintainable and testable", "Haz que la API sea mantenible y comprobable"),
            Text("Refactor the order flow", "Refactorizar el flujo de órdenes"),
            Text(
                "Apply design, persistence, and integration tests to the same case.",
                "Aplica diseño, persistencia y pruebas de integración sobre el mismo caso."),
            false,
            [
                Module("Applied SOLID", "SOLID aplicado", "◇", "solid"),
                Module("Dependency injection", "Inyección de dependencias", "⌘", "inyeccion-dependencias"),
                Module("Strategy and patterns", "Strategy y patrones", "⇄", "strategy-patrones"),
                Module("Safe refactoring", "Refactorización segura", "↻", "refactorizacion"),
                Module("Persistence with EF Core", "Persistencia con EF Core", "▱", "ef-core"),
                Module("Errors and ProblemDetails", "Errores y ProblemDetails", "!", "problem-details"),
                Module("Integration tests", "Pruebas de integración", "⛓", "pruebas-integracion"),
                Module("Architecture boundaries", "Límites de arquitectura", "▦", "limites-arquitectura")
                // TODO(AI-Middle): Add the bilingual AI integration module with the Middle learning path.
            ]),
        new(
            "senior",
            "Senior",
            Text("Production", "Producción"),
            Text("Operate", "Operar"),
            Text("Prepare the API for real conditions", "Prepara la API para condiciones reales"),
            Text("Operate under pressure", "Operar bajo presión"),
            Text(
                "Withstand duplicates, concurrency, external failures, and Azure growth.",
                "Resiste duplicados, concurrencia, fallos externos y crecimiento en Azure."),
            false,
            [
                Module("Idempotency", "Idempotencia", "↺", "idempotencia"),
                Module("Concurrency and consistency", "Concurrencia y consistencia", "≋", "concurrencia"),
                Module("Resilience", "Resiliencia", "⚡", "resiliencia"),
                Module("Messaging and Outbox", "Mensajería y Outbox", "⇢", "mensajeria-outbox"),
                Module("Observability", "Observabilidad", "◉", "observabilidad"),
                Module("API security", "Seguridad de API", "◆", "seguridad-api"),
                Module("Architecture decisions", "Decisiones de arquitectura", "⌁", "decisiones-arquitectura"),
                Module("Azure and scalability", "Azure y escalabilidad", "☁", "azure-escalabilidad")
                // TODO(AI-Senior): Add the bilingual AI integration module with the Senior learning path.
            ])
    ];

    public static LearningLevelDefinition Get(string level) =>
        Levels.First(item => item.Key.Equals(level, StringComparison.OrdinalIgnoreCase));

    public static bool TryGet(string? level, out LearningLevelDefinition definition)
    {
        definition = Levels.FirstOrDefault(item => item.Key.Equals(level, StringComparison.OrdinalIgnoreCase))!;
        return definition is not null;
    }

    public static int TotalModuleCount => Levels.Sum(level => level.Modules.Count);

    private static LocalizedText Text(string english, string spanish) => new(english, spanish);

    private static LearningModuleDefinition Module(string english, string spanish, string icon, params string[] slugs) =>
        new(Text(english, spanish), icon, slugs);
}
