using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.AI;

/// <summary>
/// Deterministic learning provider used when no external AI provider is configured.
/// It never sends data outside the process and keeps the validation/fallback path
/// repeatable in tests and in the GitHub Pages demo.
/// </summary>
public sealed class SimulatedAiOrderSuggestionService : IAiOrderSuggestionService
{
    public Task<AiOrderSuggestion> SuggestAsync(
        string customerNote,
        AppLanguage language,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var note = customerNote.Trim();

        if (string.IsNullOrWhiteSpace(note))
        {
            return Task.FromResult(Fallback(language,
                "No note was provided.",
                "No se proporcionó una nota."));
        }

        // This is a deterministic teaching guard, not a prompt-injection classifier.
        // It demonstrates that user text cannot override the application contract.
        if (note.Contains("ignore", StringComparison.OrdinalIgnoreCase)
            || note.Contains("ignora", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(Fallback(language,
                "The note contained an instruction that must not override the system contract.",
                "La nota contenía una instrucción que no debe reemplazar el contrato del sistema."));
        }

        // Fixed rules keep the simulated provider repeatable across tests and Pages.
        // A real provider would still have to return one of these allowed categories.
        var category = note.Contains("deliver", StringComparison.OrdinalIgnoreCase)
            || note.Contains("entrega", StringComparison.OrdinalIgnoreCase)
                ? "delivery"
                : note.Contains("broken", StringComparison.OrdinalIgnoreCase)
                  || note.Contains("monitor", StringComparison.OrdinalIgnoreCase)
                  || note.Contains("producto", StringComparison.OrdinalIgnoreCase)
                    ? "product"
                    : "other";
        var summary = note.Length <= 120 ? note : $"{note[..117]}…";
        var reason = Localize(language,
            "The schema and allowed category passed validation. A person must still approve its use.",
            "El esquema y la categoría permitida pasaron la validación. Una persona todavía debe aprobar su uso.");

        return Task.FromResult(new AiOrderSuggestion(category, summary, reason, false));
    }

    private static AiOrderSuggestion Fallback(AppLanguage language, string englishReason, string spanishReason) => new(
        "other",
        Localize(language, "No automated suggestion was applied.", "No se aplicó una sugerencia automática."),
        Localize(language, englishReason, spanishReason),
        true);

    private static string Localize(AppLanguage language, string english, string spanish) =>
        language == AppLanguage.Spanish ? spanish : english;
}
