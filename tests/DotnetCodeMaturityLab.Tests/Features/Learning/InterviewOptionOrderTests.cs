using DotnetCodeMaturityLab.Features.Learning;
using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class InterviewOptionOrderTests
{
    [Theory]
    [InlineData("junior")]
    [InlineData("middle")]
    public void Create_RotatesCorrectAnswersAcrossVisualPositions(string level)
    {
        var questions = InterviewContentCatalog.Get(level, AppLanguage.English).Questions;

        var order = InterviewOptionOrder.Create(questions, new Random(2026));
        var correctPositions = questions
            .Select((question, questionIndex) =>
                Array.FindIndex(order[questionIndex], optionIndex => question.Options[optionIndex].IsCorrect))
            .ToArray();

        Assert.Equal(questions.Count, order.Length);
        Assert.All(order, optionOrder => Assert.Equal([0, 1, 2], optionOrder.Order()));
        Assert.All(correctPositions.Zip(correctPositions.Skip(1)), pair =>
            Assert.NotEqual(pair.First, pair.Second));
        Assert.Equal([0, 1, 2], correctPositions.Distinct().Order());
    }

    [Fact]
    public void Create_UsesAttemptSeedWithoutChangingCatalogOrder()
    {
        var questions = InterviewContentCatalog.Get("middle", AppLanguage.English).Questions;
        var originalFirstOptions = questions.Select(question => question.Options[0].Text).ToArray();

        var firstAttempt = InterviewOptionOrder.Create(questions, new Random(1));
        var secondAttempt = InterviewOptionOrder.Create(questions, new Random(2));

        Assert.False(firstAttempt.SelectMany(order => order).SequenceEqual(secondAttempt.SelectMany(order => order)));
        Assert.Equal(originalFirstOptions, questions.Select(question => question.Options[0].Text));
    }
}
