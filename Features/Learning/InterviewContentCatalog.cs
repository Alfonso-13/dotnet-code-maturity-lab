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
                    "Fourteen questions connect the learning path with the decisions a .NET interviewer may ask you to explain, including safe LLM integration.",
                    "Catorce preguntas conectan la ruta de aprendizaje con las decisiones que un entrevistador .NET puede pedirte explicar, incluida una integración segura con LLM."),
                new("All Junior modules · two AI questions", "Todos los módulos Junior · dos preguntas de IA"),
                new("Strong Junior foundation", "Base Junior sólida"),
                JuniorInterviewCatalog.Get(language));
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
