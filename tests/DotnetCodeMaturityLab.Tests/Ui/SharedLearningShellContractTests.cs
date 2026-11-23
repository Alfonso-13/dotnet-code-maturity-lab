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
}
