namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record LearningProgressState(
    string[]? Viewed,
    string[]? Reviewed,
    string[]? Completed,
    int? Version);

/// <summary>
/// Defines the browser progress contract shared by the server and GitHub Pages
/// hosts. Version 4 namespaces entries by level while retaining the existing
/// storage key so previously saved Junior progress can be migrated in place.
/// </summary>
public static class LearningProgress
{
    public const string StorageKey = "dotnet-code-maturity-lab-progress-v2";
    public const int CurrentVersion = 4;

    public static LearningProgressState Empty { get; } = new([], [], [], CurrentVersion);

    public static string Entry(string level, string slug) => $"{level}:{slug}";

    public static LearningProgressState Migrate(LearningProgressState? source)
    {
        if (source is null) return Empty;

        var viewed = (source.Viewed ?? []).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var reviewed = (source.Reviewed ?? []).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var completed = (source.Completed ?? []).ToHashSet(StringComparer.OrdinalIgnoreCase);

        if ((source.Version ?? 0) < 3)
        {
            var moduleSlugs = JuniorCurriculum.Topics.Take(13)
                .Select(topic => topic.Slug)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Older versions considered the fifth perspective a completion.
            // Preserve that evidence as reviewed and reserve completion for
            // the explicit confirmation introduced in version 3.
            foreach (var slug in completed.Where(moduleSlugs.Contains).ToArray())
            {
                completed.Remove(slug);
                reviewed.Add(slug);
            }
        }

        if ((source.Version ?? 0) < CurrentVersion)
        {
            viewed = QualifyLegacy(viewed);
            reviewed = QualifyLegacy(reviewed);
            completed = QualifyLegacy(completed);
        }

        return new(viewed.ToArray(), reviewed.ToArray(), completed.ToArray(), CurrentVersion);
    }

    public static HashSet<string> ForLevel(IEnumerable<string>? entries, string level)
    {
        var prefix = $"{level}:";
        return (entries ?? [])
            .Where(entry => entry.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(entry => entry[prefix.Length..])
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public static LearningProgressState WithLevel(
        LearningProgressState source,
        string level,
        IEnumerable<string> viewed,
        IEnumerable<string> reviewed,
        IEnumerable<string> completed)
    {
        var prefix = $"{level}:";
        var allViewed = ReplaceLevel(source.Viewed, prefix, viewed);
        var allReviewed = ReplaceLevel(source.Reviewed, prefix, reviewed);
        var allCompleted = ReplaceLevel(source.Completed, prefix, completed);
        return new(allViewed, allReviewed, allCompleted, CurrentVersion);
    }

    private static HashSet<string> QualifyLegacy(IEnumerable<string> entries) =>
        entries.Select(entry => entry.Contains(':', StringComparison.Ordinal) ? entry : Entry("junior", entry))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    private static string[] ReplaceLevel(IEnumerable<string>? source, string prefix, IEnumerable<string> current) =>
        (source ?? [])
            .Where(entry => !entry.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Concat(current.Select(slug => $"{prefix}{slug}"))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
}
