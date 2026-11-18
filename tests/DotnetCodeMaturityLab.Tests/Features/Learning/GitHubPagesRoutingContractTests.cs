namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class GitHubPagesRoutingContractTests
{
    [Fact]
    public void DeepLinkFallback_DoesNotRedeclareTheBrowserLocationGlobal()
    {
        var repositoryRoot = FindRepositoryRoot();
        var fallback = File.ReadAllText(Path.Combine(repositoryRoot, "GitHubPages", "wwwroot", "404.html"));

        Assert.Contains("const currentLocation = window.location;", fallback, StringComparison.Ordinal);
        Assert.DoesNotContain("const location = window.location;", fallback, StringComparison.Ordinal);
        Assert.Contains("currentLocation.replace(", fallback, StringComparison.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "DotnetCodeMaturityLab.csproj")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new DirectoryNotFoundException("Could not locate the repository root for the Pages routing contract test.");
    }
}
