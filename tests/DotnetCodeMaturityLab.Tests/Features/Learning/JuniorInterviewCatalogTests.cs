using DotnetCodeMaturityLab.Features.Learning;
using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class JuniorInterviewCatalogTests
{
    [Theory]
    [InlineData(AppLanguage.English)]
    [InlineData(AppLanguage.Spanish)]
    public void Catalog_CoversEachJuniorModuleWithOneCorrectAnswer(AppLanguage language)
    {
        var questions = JuniorInterviewCatalog.Get(language);

        Assert.Equal(14, questions.Count);
        Assert.Equal(13, questions.Select(question => question.ModuleSlug).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        Assert.Equal(2, questions.Count(question => question.ModuleSlug == "ia-llm"));

        foreach (var question in questions)
        {
            Assert.False(string.IsNullOrWhiteSpace(question.ModuleTitle));
            Assert.False(string.IsNullOrWhiteSpace(question.Prompt));
            Assert.Equal(3, question.Options.Length);
            Assert.Single(question.Options, option => option.IsCorrect);
            Assert.All(question.Options, option =>
            {
                Assert.False(string.IsNullOrWhiteSpace(option.Text));
                Assert.False(string.IsNullOrWhiteSpace(option.Feedback));
            });
            Assert.False(string.IsNullOrWhiteSpace(question.DefenseQuestion));
            Assert.False(string.IsNullOrWhiteSpace(question.ExpectedAnswer));
        }
    }
}
