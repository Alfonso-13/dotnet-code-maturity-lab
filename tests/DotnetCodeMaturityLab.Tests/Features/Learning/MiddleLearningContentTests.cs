using DotnetCodeMaturityLab.Features.Learning;
using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class MiddleLearningContentTests
{
    [Fact]
    public void ImplementedBlocks_HaveBilingualModulesWithFivePerspectives()
    {
        Assert.Equal(
            MiddleCurriculum.Modules.Select(module => module.Slug),
            MiddleLessonCatalog.ImplementedSlugs);

        foreach (var slug in MiddleLessonCatalog.ImplementedSlugs)
        {
            var english = MiddleLessonCatalog.Get(slug, AppLanguage.English);
            var spanish = MiddleLessonCatalog.Get(slug, AppLanguage.Spanish);

            Assert.Equal(slug, english.Slug);
            Assert.Equal(5, english.Steps.Length);
            Assert.Equal(english.Steps.Length, spanish.Steps.Length);

            Assert.All(english.Steps, AssertCompletePerspective);
            Assert.All(spanish.Steps, AssertCompletePerspective);

            var englishMetadata = MiddleModuleMetadataCatalog.Get(slug, AppLanguage.English);
            var spanishMetadata = MiddleModuleMetadataCatalog.Get(slug, AppLanguage.Spanish);
            Assert.Equal("MIDDLE DESIGN", englishMetadata.Block);
            Assert.Equal("DISEÑO MIDDLE", spanishMetadata.Block);
        }

        Assert.Equal(
            50,
            MiddleLessonCatalog.ImplementedSlugs.Sum(slug =>
                MiddleLessonCatalog.Get(slug, AppLanguage.English).Steps.Length));
        Assert.Equal(
            50,
            MiddleLessonCatalog.ImplementedSlugs.Sum(slug =>
                MiddleLessonCatalog.Get(slug, AppLanguage.Spanish).Steps.Length));
    }

    [Fact]
    public void InterviewCatalog_CoversOnlyImplementedLessonsAndHasOneCorrectOption()
    {
        foreach (var language in new[] { AppLanguage.English, AppLanguage.Spanish })
        {
            var questions = MiddleInterviewCatalog.Get(language);

            Assert.Equal(14, questions.Count);
            Assert.Equal(
                MiddleLessonCatalog.ImplementedSlugs,
                questions.Take(MiddleLessonCatalog.ImplementedSlugs.Count).Select(question => question.ModuleSlug));
            Assert.All(
                MiddleLessonCatalog.ImplementedSlugs,
                slug => Assert.Contains(questions, question => question.ModuleSlug == slug));
            Assert.All(questions, question =>
            {
                Assert.Equal(3, question.Options.Length);
                Assert.Single(question.Options, option => option.IsCorrect);
                Assert.All(question.Options, option =>
                {
                    Assert.False(string.IsNullOrWhiteSpace(option.Text));
                    Assert.False(string.IsNullOrWhiteSpace(option.Feedback));
                });
                Assert.False(string.IsNullOrWhiteSpace(question.DefenseQuestion));
                Assert.False(string.IsNullOrWhiteSpace(question.ExpectedAnswer));
            });
        }
    }

    [Fact]
    public void CompleteMiddleContent_IsRegisteredAsPublicLevelContent()
    {
        Assert.True(LearningLevelCatalog.Get("middle").IsAvailable);
        Assert.True(LearningContentCatalog.TryGet("middle", AppLanguage.English, out var content));
        Assert.Equal(10, content.Modules.Count);
        Assert.True(InterviewContentCatalog.TryGet("middle", AppLanguage.English, out var interview));
        Assert.Equal(14, interview.Questions.Count);
        Assert.True(LaboratoryContentCatalog.TryGet("middle", out var laboratory));
        Assert.Equal(typeof(Components.Orders.MiddleLaboratory), laboratory.ComponentType);
    }

    [Fact]
    public void SolidModule_MapsOnePerspectiveToEveryPrinciple()
    {
        string[] expectedLetters = ["S", "O", "L", "I", "D"];
        string[] englishNames =
        [
            "S · Single Responsibility",
            "O · Open/Closed",
            "L · Liskov Substitution",
            "I · Interface Segregation",
            "D · Dependency Inversion"
        ];
        string[] spanishNames =
        [
            "S · Responsabilidad única",
            "O · Abierto/cerrado",
            "L · Sustitución de Liskov",
            "I · Segregación de interfaces",
            "D · Inversión de dependencias"
        ];

        foreach (var language in new[] { AppLanguage.English, AppLanguage.Spanish })
        {
            var lesson = MiddleLessonCatalog.Get("solid", language);

            Assert.Equal(expectedLetters.Length, lesson.Steps.Length);
            Assert.Equal(
                language == AppLanguage.Spanish ? spanishNames : englishNames,
                lesson.Steps.Select(step => step.Name));
            Assert.Equal(
                expectedLetters,
                lesson.Steps.Select(step => step.Name.Split('·')[0].Trim()));
            Assert.All(lesson.Steps, step => Assert.NotNull(step.Right));
            Assert.All(lesson.Steps, step => Assert.Equal("bad-practice", step.Left.CssClass));
            Assert.All(lesson.Steps, step => Assert.Equal("good-practice", step.Right!.CssClass));
            Assert.All(lesson.Steps, step =>
            {
                Assert.Equal(2, step.CodeVariants!.Length);
                Assert.Equal(1, step.DefaultCodeVariantIndex);
                Assert.Equal(
                    language == AppLanguage.Spanish ? ["Ejemplo clásico", "API de órdenes"] : ["Classic example", "Order API"],
                    step.CodeVariants.Select(variant => variant.Label));
                Assert.StartsWith("// ", step.CodeVariants[0].Left.Code, StringComparison.Ordinal);
                Assert.StartsWith("// ", step.CodeVariants[0].Right!.Code, StringComparison.Ordinal);
                Assert.Same(step.Left, step.CodeVariants[1].Left);
                Assert.Same(step.Right, step.CodeVariants[1].Right);
            });
        }

        var englishLesson = MiddleLessonCatalog.Get("solid", AppLanguage.English);
        Assert.Contains("Employee", englishLesson.Steps[0].CodeVariants![0].Left.Code, StringComparison.Ordinal);
        Assert.Contains("AreaCalculator", englishLesson.Steps[1].CodeVariants![0].Left.Code, StringComparison.Ordinal);
        Assert.Contains("duck", englishLesson.Steps[2].CodeVariants![0].Left.Code, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("BasicPrinter", englishLesson.Steps[3].CodeVariants![0].Left.Code, StringComparison.Ordinal);
        Assert.Contains("LightSwitch", englishLesson.Steps[4].CodeVariants![0].Left.Code, StringComparison.Ordinal);
    }

    [Fact]
    public void JwtModule_SeparatesTokenTrustFromPermissionDecisions()
    {
        foreach (var language in new[] { AppLanguage.English, AppLanguage.Spanish })
        {
            var lesson = MiddleLessonCatalog.Get("seguridad-jwt", language);
            var content = lesson.Steps
                .SelectMany(step => new[] { step.Left.Code, step.Right?.Code ?? string.Empty })
                .ToArray();

            Assert.Contains(content, code => code.Contains("TokenValidationParameters", StringComparison.Ordinal));
            Assert.Contains(content, code => code.Contains("ValidateIssuerSigningKey = true", StringComparison.Ordinal));
            Assert.Contains(content, code => code.Contains("RequireClaim(\"permission\", \"orders.write\")", StringComparison.Ordinal));
            Assert.Contains(content, code => code.Contains("HttpStatusCode.Unauthorized", StringComparison.Ordinal));
            Assert.Contains(content, code => code.Contains("HttpStatusCode.Forbidden", StringComparison.Ordinal));
            Assert.Contains(content, code => code.Contains("HttpStatusCode.Created", StringComparison.Ordinal));
        }
    }

    [Fact]
    public void EfCoreModule_ConnectsQueryShapeToIndexAndPlanEvidence()
    {
        var lesson = MiddleLessonCatalog.Get("ef-core", AppLanguage.English);
        var content = lesson.Steps
            .SelectMany(step => new[] { step.Left.Code, step.Right?.Code ?? string.Empty })
            .ToArray();

        Assert.Contains(content, code => code.Contains("HasIndex", StringComparison.Ordinal));
        Assert.Contains(content, code => code.Contains("ToQueryString", StringComparison.Ordinal));
        Assert.Contains(content, code => code.Contains("EXPLAIN", StringComparison.Ordinal));
        Assert.Contains(content, code => code.Contains("migration.CreateIndex", StringComparison.Ordinal));
    }

    private static void AssertCompletePerspective(LessonPerspective perspective)
    {
        Assert.False(string.IsNullOrWhiteSpace(perspective.Name));
        Assert.False(string.IsNullOrWhiteSpace(perspective.Label));
        Assert.False(string.IsNullOrWhiteSpace(perspective.Caption));
        Assert.False(string.IsNullOrWhiteSpace(perspective.HintTitle));
        Assert.False(string.IsNullOrWhiteSpace(perspective.HintBody));
        Assert.False(string.IsNullOrWhiteSpace(perspective.HintExample));
        AssertCodePane(perspective.Left);
        if (perspective.Right is not null) AssertCodePane(perspective.Right);
        foreach (var variant in perspective.CodeVariants ?? [])
        {
            Assert.False(string.IsNullOrWhiteSpace(variant.Label));
            AssertCodePane(variant.Left);
            if (variant.Right is not null) AssertCodePane(variant.Right);
        }
    }

    private static void AssertCodePane(LessonCodePane pane)
    {
        Assert.EndsWith(".cs", pane.FileName, StringComparison.OrdinalIgnoreCase);
        Assert.False(string.IsNullOrWhiteSpace(pane.Label));
        Assert.False(string.IsNullOrWhiteSpace(pane.Code));
        Assert.DoesNotContain('\t', pane.Code);
        Assert.Equal(pane.Code.Trim('\r', '\n'), pane.Code);
    }
}
