namespace DotnetCodeMaturityLab.Features.AI;

public sealed record AiOrderSuggestion(
    string Category,
    string Summary,
    string Reason,
    bool IsFallback,
    bool RequiresReview = true);
