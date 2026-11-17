using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class JuniorLearningRoutesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private static readonly string[] Slugs =
    [
        "fundamentos-csharp",
        "metodos-y-clases",
        "modelos-y-memoria",
        "poo",
        "errores-y-validacion",
        "colecciones-y-genericos",
        "linq",
        "task-async-await",
        "pruebas-aaa",
        "rest-http",
        "persistencia-ef-core",
        "aspnet-core"
    ];

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
        var response = await client.GetAsync($"/aprender/junior/{slug}");
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
        var response = await client.GetAsync("/laboratorio/junior");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("code-compare-grid", html, StringComparison.Ordinal);
        Assert.Contains("request-journey", html, StringComparison.Ordinal);
        Assert.Contains("Execute request", html, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("/historia-dotnet", "A platform that changed to reach more places.", "From the original CLR to a unified platform")]
    [InlineData("/ecosistema-dotnet", "Choose the tool according to what you need to build.", "Which one should you use?")]
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
