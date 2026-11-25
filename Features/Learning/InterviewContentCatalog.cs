using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record InterviewLevelContent(
    LearningLevelDefinition Definition,
    LocalizedText HeroTitle,
    LocalizedText HeroDescription,
    LocalizedText FormatDescription,
    LocalizedText StrongResult,
    IReadOnlyList<InterviewQuestion> Questions);

/// <summary>
/// Keeps interview questions and level-specific copy outside the shared
/// simulator. Middle and Senior providers can be added without a new page.
/// </summary>
public static class InterviewContentCatalog
{
    public static bool TryGet(string? level, AppLanguage language, out InterviewLevelContent content)
    {
        if (level?.Equals("junior", StringComparison.OrdinalIgnoreCase) == true)
        {
            content = new(
                LearningLevelCatalog.Get("junior"),
                new("Defend the Junior fundamentals, not only the syntax.", "Defiende los fundamentos Junior, no solo la sintaxis."),
                new(
                    "Sixteen questions connect the learning path with the decisions a .NET interviewer may ask you to explain, including runtime behavior and safe LLM integration.",
                    "Dieciséis preguntas conectan la ruta de aprendizaje con las decisiones que un entrevistador .NET puede pedirte explicar, incluido el comportamiento del runtime y una integración segura con LLM."),
                new("All Junior modules · two runtime questions · two AI questions", "Todos los módulos Junior · dos preguntas de runtime · dos de IA"),
                new("Strong Junior foundation", "Base Junior sólida"),
                JuniorInterviewCatalog.Get(language));
            return true;
        }

        if (level?.Equals("middle", StringComparison.OrdinalIgnoreCase) == true)
        {
            content = new(
                LearningLevelCatalog.Get("middle"),
                new(
                    "Defend design decisions through change, collaboration, and evidence.",
                    "Defiende decisiones de diseño mediante cambio, colaboración y evidencia."),
                new(
                    "Fourteen scenarios connect SOLID, dependency injection, persistence, JWT authorization, HTTP integration, architecture boundaries, and maintainable AI.",
                    "Catorce escenarios conectan SOLID, inyección de dependencias, persistencia, autorización JWT, integración HTTP, límites de arquitectura e IA mantenible."),
                new(
                    "All Middle modules · four cross-module scenarios",
                    "Todos los módulos Middle · cuatro escenarios transversales"),
                new("Strong Middle design reasoning", "Razonamiento de diseño Middle sólido"),
                MiddleInterviewCatalog.Get(language));
            return true;
        }

        content = null!;
        return false;
    }

    public static InterviewLevelContent Get(string level, AppLanguage language) =>
        TryGet(level, language, out var content)
            ? content
            : throw new KeyNotFoundException($"No available interview is registered for level '{level}'.");
}
