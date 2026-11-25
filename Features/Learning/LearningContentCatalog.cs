using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record LearningModuleContent(
    string Slug,
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
                    topic.Slug,
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

        if (level?.Equals("middle", StringComparison.OrdinalIgnoreCase) == true)
        {
            var definition = LearningLevelCatalog.Get("middle");
            var modules = MiddleCurriculum.Modules
                .Select(module => new LearningModuleContent(
                    module.Slug,
                    MiddleModuleMetadataCatalog.Get(module.Slug, language),
                    MiddleLessonCatalog.Get(module.Slug, language)))
                .ToArray();

            content = new(
                definition,
                new(
                    "Ten modules refactor the same order flow into a maintainable, authorized, and testable design.",
                    "Diez módulos refactorizan el mismo flujo de órdenes en un diseño mantenible, autorizado y comprobable."),
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
