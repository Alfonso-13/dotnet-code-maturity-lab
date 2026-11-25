using DotnetCodeMaturityLab.Features.Learning;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class MiddleLaboratoryContractTests
{
    [Fact]
    public void Laboratory_IsRegisteredForAvailableMiddleLevel()
    {
        Assert.True(LaboratoryContentCatalog.TryGet("middle", out var laboratory));
        Assert.Equal(typeof(Components.Orders.MiddleLaboratory), laboratory.ComponentType);
        Assert.True(LearningLevelCatalog.Get("middle").IsAvailable);
    }

    [Fact]
    public void Laboratory_RequiresArchitectureIntegrationAndAiEvidence()
    {
        var root = FindRepositoryRoot();
        var source = File.ReadAllText(Path.Combine(root, "Components", "Orders", "MiddleLaboratory.razor"));

        Assert.Contains("HasArchitectureEvidence && HasIntegrationEvidence && HasAiEvidence", source, StringComparison.Ordinal);
        Assert.Contains("LearningLevelCatalog.Get(\"middle\").LabCompletionSlug", source, StringComparison.Ordinal);
        Assert.Contains("LearningProgress.WithLevel(progress, \"middle\"", source, StringComparison.Ordinal);
        Assert.Contains("EvidenceService.VerifyAsync()", source, StringComparison.Ordinal);
        Assert.Contains("AiSuggestionService.SuggestAsync(", source, StringComparison.Ordinal);
        Assert.Contains("HostingProfile.IsStaticDemo", source, StringComparison.Ordinal);
        Assert.Contains("<MiddleLabCodeReview />", source, StringComparison.Ordinal);
        Assert.Contains("<MiddleLabDecisionPanel", source, StringComparison.Ordinal);
        Assert.Contains("<MiddleLabHttpEvidencePanel", source, StringComparison.Ordinal);
        Assert.Contains("<MiddleLabAiEvidencePanel", source, StringComparison.Ordinal);
    }

    [Fact]
    public void PublicLaboratoryRoute_RequiresAvailableLevel()
    {
        var root = FindRepositoryRoot();
        var source = File.ReadAllText(Path.Combine(root, "Components", "Pages", "Laboratory.razor"));

        Assert.Contains("levelDefinition.IsAvailable", source, StringComparison.Ordinal);
        Assert.Contains("LaboratoryContentCatalog.TryGet", source, StringComparison.Ordinal);
    }

    [Fact]
    public void MiddleLabCompletion_RemainsSeparateFromModulesAndJuniorProgress()
    {
        var original = new LearningProgressState(
            ["junior:fundamentos-csharp"],
            ["junior:fundamentos-csharp"],
            ["junior:crear-orden", "middle:solid"],
            LearningProgress.CurrentVersion);

        var completed = LearningProgress.ForLevel(original.Completed, "middle");
        completed.Add(LearningLevelCatalog.Get("middle").LabCompletionSlug);
        var updated = LearningProgress.WithLevel(
            original,
            "middle",
            LearningProgress.ForLevel(original.Viewed, "middle"),
            LearningProgress.ForLevel(original.Reviewed, "middle"),
            completed);

        Assert.Contains("junior:crear-orden", updated.Completed!);
        Assert.Contains("middle:solid", updated.Completed!);
        Assert.Contains("middle:refactor-order-flow", updated.Completed!);
        Assert.DoesNotContain("junior:refactor-order-flow", updated.Completed!);
    }

    [Fact]
    public void ResponsiveStyles_StackMiddleEvidenceAndSharedCodeComparisons()
    {
        var root = FindRepositoryRoot();
        var styles = File.ReadAllText(Path.Combine(root, "wwwroot", "app.css"));

        Assert.Contains(".middle-decision-grid { grid-template-columns: 1fr; }", styles, StringComparison.Ordinal);
        Assert.Contains(".lesson-dual-code, .final-code-review { grid-template-columns: 1fr; height: auto; }", styles, StringComparison.Ordinal);
        Assert.Contains(".middle-decision-grid > article", styles, StringComparison.Ordinal);
        Assert.Contains("min-width: 0", styles, StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "DotnetCodeMaturityLab.csproj")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("Repository root was not found.");
    }
}
