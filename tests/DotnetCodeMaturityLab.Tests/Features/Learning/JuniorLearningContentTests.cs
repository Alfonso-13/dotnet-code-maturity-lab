using System.Reflection;
using DotnetCodeMaturityLab.Features.Learning;
using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class JuniorLearningContentTests
{
    [Fact]
    public void Curriculum_ContainsThirteenModulesWithFivePerspectives()
    {
        var moduleTopics = JuniorCurriculum.Topics.Take(13).ToArray();

        Assert.Equal(13, moduleTopics.Length);

        foreach (var topic in moduleTopics)
        {
            var module = JuniorLessonCatalog.Get(topic.Slug);
            Assert.Equal(topic.Slug, module.Slug);
            Assert.Equal(5, module.Steps.Length);
            Assert.All(module.Steps, step => Assert.False(string.IsNullOrWhiteSpace(step.Name)));
        }
    }

    [Fact]
    public void CatalogSteps_ContainLegibleHintsAndCopyableCode()
    {
        foreach (var language in new[] { AppLanguage.English, AppLanguage.Spanish })
        {
            foreach (var topic in JuniorCurriculum.Topics.Take(13))
            {
                var module = JuniorLessonCatalog.Get(topic.Slug, language);

                foreach (var step in module.Steps)
                {
                    Assert.False(string.IsNullOrWhiteSpace(step.Name));
                    Assert.False(string.IsNullOrWhiteSpace(step.Label));
                    Assert.False(string.IsNullOrWhiteSpace(step.Caption));
                    Assert.False(string.IsNullOrWhiteSpace(step.HintTitle));
                    Assert.False(string.IsNullOrWhiteSpace(step.HintBody));
                    Assert.False(string.IsNullOrWhiteSpace(step.HintExample));

                    AssertCodePane(step.Left);
                    if (step.Right is not null) AssertCodePane(step.Right);
                }
            }
        }
    }

    [Fact]
    public void EnglishAndSpanishCatalogs_HaveTheSameSixtyFivePerspectiveStructure()
    {
        var topics = JuniorCurriculum.Topics.Take(13).ToArray();
        var perspectiveCount = 0;

        foreach (var topic in topics)
        {
            var spanish = JuniorLessonCatalog.Get(topic.Slug, AppLanguage.Spanish);
            var english = JuniorLessonCatalog.Get(topic.Slug, AppLanguage.English);

            Assert.Equal(topic.Slug, english.Slug);
            Assert.Equal(spanish.Steps.Length, english.Steps.Length);
            Assert.Equal(5, english.Steps.Length);

            for (var index = 0; index < spanish.Steps.Length; index++)
            {
                Assert.Equal(spanish.Steps[index].Right is null, english.Steps[index].Right is null);
            }

            var metadata = JuniorModuleMetadataCatalog.Get(topic, AppLanguage.English);
            Assert.False(string.IsNullOrWhiteSpace(metadata.ShortTitle));
            Assert.False(string.IsNullOrWhiteSpace(metadata.Title));
            Assert.Equal(topic.Slug == "ia-llm" ? "APPLIED AI" : "C# FUNDAMENTALS", metadata.Block);
            perspectiveCount += english.Steps.Length;
        }

        Assert.Equal(65, perspectiveCount);
    }

    [Fact]
    public void LastPerspective_ReviewsBeforeExplicitCompletion()
    {
        var component = new Components.Pages.Learning();
        SetProperty(component, "Level", "junior");
        SetProperty(component, "TopicSlug", "fundamentos-csharp");
        Invoke(component, "OnParametersSet");

        for (var index = 0; index < 4; index++) Invoke(component, "GoNextStep");

        Assert.Equal(4, GetProperty<int>(component, "ActivePerspective"));
        Assert.Contains("fundamentos-csharp", GetField<HashSet<string>>(component, "Reviewed"));
        Assert.DoesNotContain("fundamentos-csharp", GetField<HashSet<string>>(component, "Completed"));

        Invoke(component, "GoNextStep");

        Assert.Equal(4, GetProperty<int>(component, "ActivePerspective"));
        Assert.Contains("fundamentos-csharp", GetField<HashSet<string>>(component, "Completed"));

        Invoke(component, "GoPreviousStep");
        Assert.Equal(3, GetProperty<int>(component, "ActivePerspective"));
    }

    [Fact]
    public void ClosingModules_MatchTheExecutableJuniorLaboratory()
    {
        var persistence = JuniorLessonCatalog.Get("persistencia-ef-core");
        var aspNet = JuniorLessonCatalog.Get("aspnet-core");
        var closingCode = persistence.Steps.Concat(aspNet.Steps)
            .SelectMany(step => new[] { step.Left, step.Right })
            .Where(pane => pane is not null)
            .Select(pane => pane!.Code)
            .ToArray();

        Assert.Contains(closingCode, code => code.Contains("SqliteOrderStore", StringComparison.Ordinal));
        Assert.Contains(closingCode, code => code.Contains("UseSqlite", StringComparison.Ordinal));
        Assert.DoesNotContain(closingCode, code => code.Contains("EfOrderStore", StringComparison.Ordinal));
        Assert.DoesNotContain(closingCode, code => code.Contains("UseSqlServer", StringComparison.Ordinal));
    }

    private static void AssertCodePane(JuniorCodePane pane)
    {
        Assert.True(
            pane.FileName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
            || pane.FileName.EndsWith(".http", StringComparison.OrdinalIgnoreCase),
            $"{pane.FileName} debe ser un ejemplo C# o una solicitud HTTP copiable.");
        Assert.False(string.IsNullOrWhiteSpace(pane.Label));
        AssertCopyableCode(pane.Code);
    }

    private static void AssertCopyableCode(string code)
    {
        Assert.False(string.IsNullOrWhiteSpace(code));
        Assert.DoesNotContain('\t', code);
        Assert.Equal(code.Trim('\r', '\n'), code);

        var lines = code.Replace("\r\n", "\n", StringComparison.Ordinal).Split('\n');
        Assert.All(lines, line => Assert.Equal(line.TrimEnd(), line));
        Assert.False(string.IsNullOrWhiteSpace(lines.First()));
        Assert.Equal(lines.First().TrimStart(), lines.First());
    }

    private static void Invoke(object instance, string methodName)
    {
        var method = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(method);
        method.Invoke(instance, null);
    }

    private static void SetField(object instance, string fieldName, object value)
    {
        var property = instance.GetType().GetProperty(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(property);
        property.SetValue(instance, value);
    }

    private static void SetProperty(object instance, string propertyName, object? value)
    {
        var property = instance.GetType().GetProperty(
            propertyName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(property);
        property.SetValue(instance, value);
    }

    private static T GetProperty<T>(object instance, string propertyName)
    {
        var property = instance.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(property);
        return Assert.IsType<T>(property.GetValue(instance));
    }

    private static T GetField<T>(object instance, string propertyName) => GetProperty<T>(instance, propertyName);
}
