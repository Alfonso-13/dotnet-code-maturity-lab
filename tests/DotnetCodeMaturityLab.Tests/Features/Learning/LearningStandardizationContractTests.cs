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
            Assert.False(string.IsNullOrWhiteSpace(level.Stage.Get(AppLanguage.English)));
            Assert.False(string.IsNullOrWhiteSpace(level.Stage.Get(AppLanguage.Spanish)));
            Assert.Equal(
                level.Modules.Sum(module => module.Slugs.Length),
                level.Modules.SelectMany(module => module.Slugs).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        }
    }

    [Fact]
    public void AvailableLevel_ResolvesEverySharedExperience()
    {
        var junior = LearningLevelCatalog.Get("junior");

        Assert.True(junior.IsAvailable);
        Assert.True(LearningContentCatalog.TryGet(junior.Key, AppLanguage.English, out var learning));
        Assert.True(InterviewContentCatalog.TryGet(junior.Key, AppLanguage.English, out var interview));
        Assert.True(LaboratoryContentCatalog.TryGet(junior.Key, out _));
        Assert.Equal(junior.Modules.Count, learning.Modules.Count);
        Assert.NotEmpty(interview.Questions);
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
}
