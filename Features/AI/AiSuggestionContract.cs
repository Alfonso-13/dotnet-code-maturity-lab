namespace DotnetCodeMaturityLab.Features.AI;

/// <summary>
/// Provider-independent boundary shared by the server adapter and capstone.
/// An accepted suggestion is still advisory; review is always required.
/// </summary>
public static class AiSuggestionContract
{
    public static bool IsReviewable(AiOrderSuggestion suggestion) =>
        suggestion.RequiresReview
        && IsAllowedCategory(suggestion.Category)
        && !string.IsNullOrWhiteSpace(suggestion.Summary)
        && suggestion.Summary.Length <= 120;

    public static bool IsAllowedCategory(string? category) =>
        category is "delivery" or "product" or "other";
}
