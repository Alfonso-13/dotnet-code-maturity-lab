using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.AI;

/// <summary>
/// Explicit, offline fault scenario for the Middle learning laboratory. It does
/// not call the configured provider or claim that a real outage occurred.
/// </summary>
public static class AiProviderFailureScenario
{
    public static AiOrderSuggestion TimeoutFallback(AppLanguage language) => new(
        "other",
        language == AppLanguage.Spanish
            ? "No se aplicó una sugerencia automática."
            : "No automated suggestion was applied.",
        language == AppLanguage.Spanish
            ? "Tiempo de espera simulado: conserva la orden y solicita revisión humana."
            : "Simulated timeout: preserve the order and request human review.",
        true);
}
