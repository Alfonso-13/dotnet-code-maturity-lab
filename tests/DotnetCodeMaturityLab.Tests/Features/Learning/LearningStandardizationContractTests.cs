using DotnetCodeMaturityLab.Features.Learning;
using DotnetCodeMaturityLab.Features.Localization;
using DotnetCodeMaturityLab.Features.Navigation;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class LearningStandardizationContractTests
{
    [Fact]
    public void LevelRegistry_HasUniqueLevelsModulesAndLocalizedContent()
    {
        Assert.Equal(3, LearningLevelCatalog.Levels.Count);
        Assert.Equal(
            LearningLevelCatalog.Levels.Count,
            LearningLevelCatalog.Levels.Select(level => level.Key).Distinct(StringComparer.OrdinalIgnoreCase).Count());

        foreach (var level in LearningLevelCatalog.Levels)
        {
            Assert.NotEmpty(level.Modules);
            Assert.False(string.IsNullOrWhiteSpace(level.LabCompletionSlug));
            Assert.DoesNotContain(level.LabCompletionSlug, level.Modules.SelectMany(module => module.Slugs));
            Assert.False(string.IsNullOrWhiteSpace(level.Stage.Get(AppLanguage.English)));
            Assert.False(string.IsNullOrWhiteSpace(level.Stage.Get(AppLanguage.Spanish)));
            Assert.Equal(
                level.Modules.Sum(module => module.Slugs.Length),
                level.Modules.SelectMany(module => module.Slugs).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        }
    }

    [Fact]
    public void AvailableLevels_ResolveEverySharedExperience()
    {
        var available = LearningLevelCatalog.Levels.Where(level => level.IsAvailable).ToArray();

        Assert.Equal(["junior", "middle"], available.Select(level => level.Key));
        foreach (var level in available)
        {
            Assert.True(LearningContentCatalog.TryGet(level.Key, AppLanguage.English, out var learning));
            Assert.True(InterviewContentCatalog.TryGet(level.Key, AppLanguage.English, out var interview));
            Assert.True(LaboratoryContentCatalog.TryGet(level.Key, out _));
            Assert.Equal(level.Modules.Count, learning.Modules.Count);
            Assert.NotEmpty(interview.Questions);
        }
    }

    [Fact]
    public void LegacyProgress_MigratesToReviewedAndNamespacedEntries()
    {
        var legacy = new LearningProgressState(
            ["fundamentos-csharp"],
            null,
            ["fundamentos-csharp", "crear-orden"],
            2);

        var migrated = LearningProgress.Migrate(legacy);

        Assert.Equal(LearningProgress.CurrentVersion, migrated.Version);
        Assert.Contains("junior:fundamentos-csharp", migrated.Viewed!);
        Assert.Contains("junior:fundamentos-csharp", migrated.Reviewed!);
        Assert.DoesNotContain("junior:fundamentos-csharp", migrated.Completed!);
        Assert.Contains("junior:crear-orden", migrated.Completed!);
    }

    [Fact]
    public void UpdatingOneLevel_PreservesOtherLevelProgress()
    {
        var original = new LearningProgressState(
            ["middle:solid"],
            ["middle:solid"],
            [],
            LearningProgress.CurrentVersion);

        var updated = LearningProgress.WithLevel(
            original,
            "junior",
            ["fundamentos-csharp"],
            ["fundamentos-csharp"],
            ["fundamentos-csharp"]);

        Assert.Contains("middle:solid", updated.Viewed!);
        Assert.Contains("junior:fundamentos-csharp", updated.Completed!);
    }

    [Fact]
    public void GenericModuleRoute_PreservesJuniorCanonicalUrlsAndSupportsFutureLevels()
    {
        Assert.Equal("learn/junior/csharp-fundamentals", AppRoutes.LearnModule("junior", "fundamentos-csharp"));
        Assert.Equal("learn/middle/applied-solid", AppRoutes.LearnModule("middle", "solid"));
        Assert.Equal("solid", AppRoutes.InternalModuleSlug("middle", "applied-solid"));
    }

    [Theory]
    [InlineData("junior", "poo", 4)]
    [InlineData("junior", "modelos-y-memoria", 4)]
    [InlineData("junior", "task-async-await", 4)]
    [InlineData("middle", "inyeccion-dependencias", 1)]
    [InlineData("middle", "strategy-patrones", 1)]
    public void TransferExamples_KeepClassicAndOrderApiContexts(
        string level,
        string slug,
        int perspectiveIndex)
    {
        foreach (var language in new[] { AppLanguage.English, AppLanguage.Spanish })
        {
            var lesson = level == "junior"
                ? JuniorLessonCatalog.Get(slug, language)
                : MiddleLessonCatalog.Get(slug, language);
            var perspective = lesson.Steps[perspectiveIndex];

            Assert.Equal(2, perspective.CodeVariants!.Length);
            Assert.Equal(1, perspective.DefaultCodeVariantIndex);
            Assert.Equal(
                language == AppLanguage.Spanish
                    ? ["Ejemplo clásico", "API de órdenes"]
                    : ["Classic example", "Order API"],
                perspective.CodeVariants.Select(variant => variant.Label));
            Assert.StartsWith("// ", perspective.CodeVariants[0].Left.Code, StringComparison.Ordinal);
            Assert.Same(perspective.Left, perspective.CodeVariants[1].Left);
            Assert.Same(perspective.Right, perspective.CodeVariants[1].Right);
        }
    }
}
