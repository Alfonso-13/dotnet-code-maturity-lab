namespace DotnetCodeMaturityLab.Features.Learning;

/// <summary>
/// Code sample rendered by the shared learning experience. The owning level
/// catalog supplies the content while the UI controls its presentation.
/// </summary>
public sealed record LessonCodePane(string FileName, string Label, string Code, string CssClass = "");

/// <summary>
/// One of the five perspectives used by every learning module.
/// </summary>
public sealed record LessonPerspective(
    string Name,
    string Label,
    string Caption,
    LessonCodePane Left,
    LessonCodePane? Right,
    string HintTitle,
    string HintBody,
    string HintExample);

public sealed record ModuleLesson(string Slug, LessonPerspective[] Steps);

public sealed record InterviewOption(string Text, bool IsCorrect, string Feedback);

public sealed record InterviewQuestion(
    string ModuleSlug,
    string ModuleTitle,
    string Prompt,
    InterviewOption[] Options,
    string DefenseQuestion,
    string ExpectedAnswer);

public sealed record ModuleMetadata(string ShortTitle, string Title, string Block);
