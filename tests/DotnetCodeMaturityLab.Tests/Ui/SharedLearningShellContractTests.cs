namespace DotnetCodeMaturityLab.Tests.Ui;

public sealed class SharedLearningShellContractTests
{
    private static readonly string RepositoryRoot = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "../../../../.."));

    [Fact]
    public void CoursePlayerUsesNeutralStructuralSelectors()
    {
        var component = File.ReadAllText(Path.Combine(RepositoryRoot, "Components", "Pages", "Learning.razor"));
        var css = File.ReadAllText(Path.Combine(RepositoryRoot, "wwwroot", "app.css"));

        Assert.Contains("learning-module-shell learning-course-player", component);
        Assert.Contains(".learning-module-shell", css);
        Assert.Contains(".learning-course-player", css);
        Assert.DoesNotContain(".junior-module-one", css);
        Assert.DoesNotContain(".junior-course-player", css);
    }

    [Fact]
    public void ActiveModuleStaysVisibleInsideTheScrollableCurriculum()
    {
        var component = File.ReadAllText(Path.Combine(RepositoryRoot, "Components", "Pages", "Learning.razor"));
        var script = File.ReadAllText(Path.Combine(RepositoryRoot, "wwwroot", "lab-progress.js"));

        Assert.Contains("TopicElementId(index)", component);
        Assert.Contains("learningCourse.ensureModuleVisible", component);
        Assert.Contains("ensureModuleVisible(elementId)", script);
        Assert.Contains("curriculum.scrollTo", script);
    }

    [Fact]
    public void ReviewConfirmationAnimatesPersistsAndAdvancesInOneAction()
    {
        var component = File.ReadAllText(Path.Combine(RepositoryRoot, "Components", "Pages", "Learning.razor"));
        var css = File.ReadAllText(Path.Combine(RepositoryRoot, "wwwroot", "app.css"));

        Assert.Contains("ConfirmReviewAndAdvance", component);
        Assert.Contains("await PersistProgress()", component);
        Assert.Contains("SelectTopic(ActiveTopicIndex + 1)", component);
        Assert.Contains("Navigation.NavigateTo(AppRoutes.Roadmap)", component);
        Assert.Contains("completion-check", component);
        Assert.Contains("Returning to maturity roadmap", component);
        Assert.Contains("level-completion-confetti", component);
        Assert.Contains(".confirm-review", css);
        Assert.Contains("completion-check-draw", css);
        Assert.Contains("level-confetti-fall", css);
        Assert.Contains("confirm-review:disabled { opacity: 1; cursor: default; }", css);
    }
}
