using System.Net;
using DotnetCodeMaturityLab.Features.Navigation;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class JuniorLearningRoutesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly string[] Slugs = AppRoutes.JuniorModuleSlugs.Values.ToArray();

    private readonly HttpClient client;

    public JuniorLearningRoutesTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    public static TheoryData<string> LessonSlugs => new(Slugs);

    [Theory]
    [MemberData(nameof(LessonSlugs))]
    public async Task Lesson_RendersFivePerspectivesAndPrimaryControls(string slug)
    {
        var response = await client.GetAsync($"/{AppRoutes.Learn("junior")}/{slug}");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(5, Count(html, "role=\"tab\""));
        Assert.Contains("class=\"hint-lamp\"", html, StringComparison.Ordinal);
        Assert.Contains("class=\"code-copy-icon\"", html, StringComparison.Ordinal);
        Assert.Contains("class=\"course-navigation\"", html, StringComparison.Ordinal);
        Assert.Contains("class=\"course-curriculum", html, StringComparison.Ordinal);
        Assert.Contains("C# FUNDAMENTALS", html, StringComparison.Ordinal);
        Assert.Contains("Understand", html, StringComparison.Ordinal);
        Assert.DoesNotContain(">Entiende<", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task JuniorLaboratory_RendersResponsiveComparisonAndRequestPlayground()
    {
        var response = await client.GetAsync($"/{AppRoutes.Lab("junior")}");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("code-compare-grid", html, StringComparison.Ordinal);
        Assert.Contains("request-journey", html, StringComparison.Ordinal);
        Assert.Contains("Execute request", html, StringComparison.Ordinal);
        Assert.Contains("Cross-cutting AI case", html, StringComparison.Ordinal);
        Assert.Contains("Simulated LLM", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task LaboratoryCatalog_RendersWithoutReusingThePreviousLevel()
    {
        var response = await client.GetAsync($"/{AppRoutes.Labs}");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Three laboratories. One API that matures.", html, StringComparison.Ordinal);
        Assert.Contains("Create an order", html, StringComparison.Ordinal);
        Assert.Contains("Refactor the flow", html, StringComparison.Ordinal);
        Assert.Contains("Operate under pressure", html, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("/dotnet-history", "A platform that changed to reach more places.", "From the original CLR to a unified platform")]
    [InlineData("/dotnet-ecosystem", "Choose the tool according to what you need to build.", "Which one should you use?")]
    public async Task DotNetReferencePages_DefaultToCompleteEnglishContent(
        string route,
        string heading,
        string section)
    {
        var response = await client.GetAsync(route);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(heading, html, StringComparison.Ordinal);
        Assert.Contains(section, html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task JuniorInterview_RendersOneQuestionPerModule()
    {
        var response = await client.GetAsync($"/{AppRoutes.InterviewFor("junior")}");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Sixteen questions connect the learning path", html, StringComparison.Ordinal);
        Assert.Contains("Start interview", html, StringComparison.Ordinal);
        Assert.Contains("two AI questions", html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task InterviewCatalog_OffersTheThreeLevelTestsFromOneEntryPoint()
    {
        var response = await client.GetAsync($"/{AppRoutes.Interview}");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Prepare for interview", html, StringComparison.Ordinal);
        Assert.Contains("Open Junior test", html, StringComparison.Ordinal);
        Assert.Contains("Middle", html, StringComparison.Ordinal);
        Assert.Contains("Senior", html, StringComparison.Ordinal);
        Assert.DoesNotContain("Junior interview", html, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("/ruta", "One API. Three engineering levels.")]
    [InlineData("/laboratorio", "Three laboratories. One API that matures.")]
    [InlineData("/laboratorio/junior", "Create an order that protects its contract.")]
    [InlineData("/entrevista", "Practice the decisions expected at each .NET level.")]
    [InlineData("/entrevista/junior", "Sixteen questions connect the learning path")]
    [InlineData("/historia-dotnet", "A platform that changed to reach more places.")]
    [InlineData("/ecosistema-dotnet", "Choose the tool according to what you need to build.")]
    [InlineData("/aprender/junior/ia-llm", "Generative AI and LLM fundamentals")]
    public async Task LegacySpanishRoutes_RenderWithoutClientSideCanonicalRedirects(
        string route,
        string expectedContent)
    {
        var response = await client.GetAsync(route);
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(expectedContent, html, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Roadmap_RendersAfterTheFinalJuniorModule()
    {
        var finalModule = await client.GetStringAsync($"/{AppRoutes.LearnJunior("ia-llm")}");
        var roadmap = await client.GetStringAsync($"/{AppRoutes.Roadmap}");

        Assert.Contains("Generative AI and LLM fundamentals", finalModule, StringComparison.Ordinal);
        Assert.Contains("One API. Three engineering levels.", roadmap, StringComparison.Ordinal);
        Assert.Contains("Junior", roadmap, StringComparison.Ordinal);
        Assert.Contains("Middle", roadmap, StringComparison.Ordinal);
        Assert.Contains("Senior", roadmap, StringComparison.Ordinal);
    }

    private static int Count(string value, string fragment)
    {
        var count = 0;
        var index = 0;

        while ((index = value.IndexOf(fragment, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += fragment.Length;
        }

        return count;
    }
}
