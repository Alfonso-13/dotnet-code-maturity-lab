using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.AI;

public interface IAiOrderSuggestionService
{
    Task<AiOrderSuggestion> SuggestAsync(
        string customerNote,
        AppLanguage language,
        CancellationToken cancellationToken = default);
}
