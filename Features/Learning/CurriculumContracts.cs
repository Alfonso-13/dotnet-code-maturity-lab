using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

/// <summary>
/// Planning contract for one of the five perspectives that every module must
/// later express through the shared lesson player.
/// </summary>
public sealed record CurriculumPerspectiveDefinition(
    string Key,
    LocalizedText Label,
    LocalizedText Outcome);

/// <summary>
/// Level-independent curriculum specification. It keeps learning intent
/// separate from Razor presentation and from the executable lesson examples.
/// </summary>
public sealed record CurriculumModulePlan(
    string Slug,
    LocalizedText Objective,
    IReadOnlyList<LocalizedText> Concepts,
    LocalizedText Decision,
    IReadOnlyList<CurriculumPerspectiveDefinition> Perspectives,
    LocalizedText Hint,
    LocalizedText FinalExercise,
    IReadOnlyList<LocalizedText> InterviewQuestions);
