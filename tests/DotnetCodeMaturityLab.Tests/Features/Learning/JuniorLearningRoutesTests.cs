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
    }

    [Fact]
    public async Task JuniorLaboratory_RendersResponsiveComparisonAndRequestPlayground()
    {
        var response = await client.GetAsync("/laboratorio/junior");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("code-compare-grid", html, StringComparison.Ordinal);
        Assert.Contains("request-journey", html, StringComparison.Ordinal);
        Assert.Contains("Ejecutar solicitud", html, StringComparison.Ordinal);
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
