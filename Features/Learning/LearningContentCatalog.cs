using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record LearningModuleContent(
    LabTopic Topic,
    ModuleMetadata Metadata,
    ModuleLesson Lesson);

public sealed record LearningLevelContent(
    LearningLevelDefinition Definition,
    LocalizedText Description,
    IReadOnlyList<LearningModuleContent> Modules);

/// <summary>
/// Resolves level-specific content behind the shared course player. Adding a
/// level means registering its provider here, not duplicating the UI.
/// </summary>
public static class LearningContentCatalog
{
    public static bool TryGet(string? level, AppLanguage language, out LearningLevelContent content)
    {
        if (level?.Equals("junior", StringComparison.OrdinalIgnoreCase) == true)
        {
            var definition = LearningLevelCatalog.Get("junior");
            var modules = JuniorCurriculum.Topics.Take(13)
                .Select(topic => new LearningModuleContent(
                    topic,
                    JuniorModuleMetadataCatalog.Get(topic, language),
                    JuniorLessonCatalog.Get(topic.Slug, language)))
                .ToArray();

            content = new(
                definition,
                new(
                    "Thirteen modules connected through the same visual experience.",
                    "Trece módulos conectados con la misma experiencia visual."),
                modules);
            return true;
        }

        content = null!;
        return false;
    }

    public static LearningLevelContent Get(string level, AppLanguage language) =>
        TryGet(level, language, out var content)
            ? content
            : throw new KeyNotFoundException($"No available learning content is registered for level '{level}'.");
}
