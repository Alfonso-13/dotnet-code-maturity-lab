using DotnetCodeMaturityLab.Features.Learning;
using DotnetCodeMaturityLab.Features.Localization;
using DotnetCodeMaturityLab.Features.Navigation;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class MiddleCurriculumTests
{
    [Fact]
    public void Curriculum_CoversEveryRegisteredModuleInOrder()
    {
        var level = LearningLevelCatalog.Get("middle");
        var registeredSlugs = level.Modules.SelectMany(module => module.Slugs).ToArray();
        var curriculumSlugs = MiddleCurriculum.Modules.Select(module => module.Slug).ToArray();

        Assert.True(level.IsAvailable);
        Assert.Equal(10, registeredSlugs.Length);
        Assert.Equal(registeredSlugs, curriculumSlugs);
        Assert.Equal(registeredSlugs, AppRoutes.ModuleSlugs("middle").Keys);
    }

    [Fact]
    public void EveryModule_HasCompleteBilingualLearningSpecification()
    {
        foreach (var module in MiddleCurriculum.Modules)
        {
            AssertLocalized(module.Objective);
            AssertLocalized(module.Decision);
            AssertLocalized(module.Hint);
            AssertLocalized(module.FinalExercise);

            Assert.True(module.Concepts.Count >= 3);
            Assert.All(module.Concepts, AssertLocalized);

            Assert.Equal(5, module.Perspectives.Count);
            Assert.Equal(
                ["understand", "compare", "design", "verify", "integrate"],
                module.Perspectives.Select(perspective => perspective.Key));
            Assert.All(module.Perspectives, perspective =>
            {
                AssertLocalized(perspective.Label);
                AssertLocalized(perspective.Outcome);
            });

            Assert.True(module.InterviewQuestions.Count >= 2);
            Assert.All(module.InterviewQuestions, AssertLocalized);
        }
    }

    [Fact]
    public void AiModule_ExtendsJuniorBoundaryWithoutGivingModelDomainAuthority()
    {
        var module = MiddleCurriculum.Get("ia-integracion");

        Assert.Contains("IAiOrderSuggestionService", module.FinalExercise.English, StringComparison.Ordinal);
        Assert.Contains("fallback", module.FinalExercise.English, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("reemplazable", module.Objective.Spanish, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ruta determinista", module.Decision.Spanish, StringComparison.OrdinalIgnoreCase);
    }

    private static void AssertLocalized(LocalizedText text)
    {
        Assert.False(string.IsNullOrWhiteSpace(text.Get(AppLanguage.English)));
        Assert.False(string.IsNullOrWhiteSpace(text.Get(AppLanguage.Spanish)));
        Assert.NotEqual(text.English, text.Spanish);
    }
}
