namespace DotnetCodeMaturityLab.Features.Configuration;

public sealed class SiteOptions
{
    public static SiteOptions Default { get; } = new();

    public string ProductName { get; } = ".NET Code Maturity Lab";
    public string OwnerName { get; } = "Red & Black Codeworkers";
    public string GitHubUrl { get; } = "https://github.com/Alfonso-13/dotnet-code-maturity-lab";
    public string PortfolioUrl { get; } = "https://github.com/Alfonso-13";
}
