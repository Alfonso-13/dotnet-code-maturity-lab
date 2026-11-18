using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record JuniorModuleMetadata(string ShortTitle, string Title, string Block);

/// <summary>
/// Keeps route-independent module labels in one place so the curriculum and
/// the lesson catalog always switch language together.
/// </summary>
public static class JuniorModuleMetadataCatalog
{
    private const string EnglishBlock = "C# FUNDAMENTALS";

    private static readonly IReadOnlyDictionary<string, JuniorModuleMetadata> English =
        new Dictionary<string, JuniorModuleMetadata>(StringComparer.OrdinalIgnoreCase)
        {
            ["fundamentos-csharp"] = new("C# fundamentals", "C# fundamentals: data, decisions, and repetition", EnglishBlock),
            ["metodos-y-clases"] = new("Methods and classes", "Methods, constructors, and properties", EnglishBlock),
            ["modelos-y-memoria"] = new("Models and memory", "Value, reference, class, struct, record, and enum", EnglishBlock),
            ["poo"] = new("OOP", "Object-oriented programming", EnglishBlock),
            ["errores-y-validacion"] = new("Errors and validation", "Validation, results, and exceptions", EnglishBlock),
            ["colecciones-y-genericos"] = new("Collections", "Collections, generics, and complexity", EnglishBlock),
            ["linq"] = new("LINQ", "LINQ and lambda expressions", EnglishBlock),
            ["task-async-await"] = new("Task and async", "Task, Task<T>, async, and await", EnglishBlock),
            ["pruebas-aaa"] = new("Testing and AAA", "Unit tests, integration tests, and AAA", EnglishBlock),
            ["rest-http"] = new("REST and HTTP", "REST, DTOs, routes, and HTTP status codes", EnglishBlock),
            ["persistencia-ef-core"] = new("SQL and EF Core", "Relational persistence with SQL and Entity Framework Core", EnglishBlock),
            ["aspnet-core"] = new("ASP.NET Core", "ASP.NET Core application fundamentals", EnglishBlock),
            ["ia-llm"] = new("AI and LLMs", "Generative AI and LLM fundamentals", "APPLIED AI")
        };

    public static JuniorModuleMetadata Get(LabTopic topic, AppLanguage language)
    {
        if (language == AppLanguage.English && English.TryGetValue(topic.Slug, out var metadata))
        {
            return metadata;
        }

        return new JuniorModuleMetadata(topic.ShortTitle, topic.Title, topic.Block);
    }
}
