namespace DotnetCodeMaturityLab.Features.Learning;

/// <summary>
/// Creates the visual option order for an interview attempt without changing
/// the readable, review-friendly order stored in the content catalogs.
/// </summary>
public static class InterviewOptionOrder
{
    public static int[][] Create(IReadOnlyList<InterviewQuestion> questions, Random random)
    {
        ArgumentNullException.ThrowIfNull(questions);
        ArgumentNullException.ThrowIfNull(random);

        var result = new int[questions.Count][];
        var startingCorrectPosition = questions.Count == 0
            ? 0
            : random.Next(questions[0].Options.Length);

        for (var questionIndex = 0; questionIndex < questions.Count; questionIndex++)
        {
            var options = questions[questionIndex].Options;
            if (options.Length == 0)
            {
                throw new InvalidOperationException("Interview questions must define at least one option.");
            }

            var order = Enumerable.Range(0, options.Length).ToArray();
            random.Shuffle(order);

            var correctOptionIndex = Array.FindIndex(options, option => option.IsCorrect);
            if (correctOptionIndex < 0)
            {
                throw new InvalidOperationException("Interview questions must define a correct option.");
            }

            // Rotate the correct answer through A/B/C so the learner cannot pass
            // by repeating one letter. Wrong answers remain shuffled per attempt.
            var targetPosition = (startingCorrectPosition + questionIndex) % options.Length;
            var currentPosition = Array.IndexOf(order, correctOptionIndex);
            (order[currentPosition], order[targetPosition]) =
                (order[targetPosition], order[currentPosition]);

            result[questionIndex] = order;
        }

        return result;
    }
}
