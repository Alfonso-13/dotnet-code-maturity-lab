namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson AiLlmSpanish() => Module("ia-llm",
        Step("Entiende", "LLM", "Una herramienta probabilística dentro de una aplicación determinista",
            Pane("AiConcepts.cs", "Modelo de lenguaje", """
                // Un LLM genera una continuación probable a partir
                // de instrucciones y contexto.
                string prompt = "Resume la nota del cliente";

                // La misma entrada puede producir variantes.
                AiSuggestion suggestion = await assistant.SuggestAsync(
                    customerNote,
                    cancellationToken);
                """), null,
            "Generar no significa conocer la verdad", "Un LLM trabaja con patrones del lenguaje. Puede ayudar a resumir, clasificar o redactar, pero puede inventar datos y variar entre ejecuciones.", "entrada + instrucciones → salida probable"),
        Step("Compara", "Límite seguro", "Proteger credenciales y mantener la llamada en el servidor",
            Bad("BrowserAiCall.cs", "Clave expuesta", """
                // ❌ El navegador entrega esta clave a cualquier visitante.
                string apiKey = "provider-secret";

                await httpClient.PostAsync(
                    "https://provider.example/chat",
                    request);
                """),
            Good("OrderAiAssistant.cs", "Servicio del servidor", """
                public sealed class OrderAiAssistant(
                    IAiProvider provider,
                    IOptions<AiOptions> options)
                {
                    public Task<AiSuggestion> SuggestAsync(
                        string note,
                        CancellationToken token) =>
                        provider.GenerateAsync(note, token);
                }
                """),
            "La clave nunca pertenece al cliente", "El servidor obtiene secretos desde configuración segura, controla qué datos salen y puede aplicar límites, logs y autorización.", "browser → API propia → proveedor IA"),
        Step("Contexto", "Prompt y datos", "Enviar solo el contexto necesario",
            Pane("SuggestionPrompt.cs", "Instrucciones", """
                string prompt =
                    "Clasifica la nota como delivery, billing, " +
                    "product u other. Devuelve JSON y no inventes " +
                    $"datos de la orden. Nota: {Sanitize(customerNote)}";
                """),
            Pane("DataBoundary.cs", "Minimización", """
                // No envíes secretos, credenciales ni datos personales
                // si la tarea no los necesita.
                string safeContext = customerNote[..Math.Min(
                    customerNote.Length,
                    500)];
                """),
            "El contexto también es una frontera de datos", "Una instrucción clara reduce ambigüedad, pero no garantiza la respuesta. Minimiza datos y trata el texto del usuario como contenido no confiable.", "instrucciones + contexto mínimo + formato esperado"),
        Step("Valida", "Salida estructurada", "Comprobar la respuesta antes de usarla",
            Bad("AiResult.Before.cs", "Confianza automática", """
                // ❌ La salida cambia el dominio sin comprobación.
                order.Category = modelResponse.Category;
                order.ApproveDiscount(modelResponse.Discount);
                """),
            Good("AiResult.After.cs", "Validación y fallback", """
                if (!AllowedCategories.Contains(result.Category)
                    || result.Summary.Length > 160)
                {
                    return AiSuggestion.Fallback;
                }

                return result with { RequiresReview = true };
                """),
            "La salida del modelo es entrada externa", "Valida esquema, longitud, valores permitidos y permisos. Si falla, usa un fallback seguro; no escondas una decisión crítica dentro del prompt.", "parsear → validar → aceptar o fallback"),
        Step("Integra", "Caso Junior", "Asistir una orden sin entregar el control del dominio",
            Pane("OrderSuggestionService.cs", "Orquestación", """
                public async Task<AiSuggestion> ReviewNoteAsync(
                    string customerNote,
                    CancellationToken token)
                {
                    AiSuggestion suggestion =
                        await assistant.SuggestAsync(customerNote, token);

                    return validator.IsValid(suggestion)
                        ? suggestion
                        : AiSuggestion.Fallback;
                }
                """),
            Pane("OrderSuggestionTests.cs", "Prueba con fake", """
                [Fact]
                public async Task InvalidCategory_UsesFallback()
                {
                    var provider = new FakeAiProvider(
                        new("unknown", "text", false));

                    AiSuggestion result = await service.ReviewNoteAsync(
                        "Deliver after 5 PM",
                        default);

                    Assert.True(result.RequiresReview);
                }
                """),
            "Integra mediante un contrato reemplazable", "Las pruebas usan un fake determinista. El laboratorio usa el proveedor configurado en local y una simulación identificada en GitHub Pages.", "LLM sugiere → código valida → persona revisa"));

    private static ModuleLesson AiLlmEnglish() => Module("ia-llm",
        Step("Understand", "LLM", "A probabilistic tool inside a deterministic application",
            Pane("AiConcepts.cs", "Language model", """
                // An LLM generates a probable continuation from
                // instructions and context.
                string prompt = "Summarize the customer note";

                // The same input may produce different variants.
                AiSuggestion suggestion = await assistant.SuggestAsync(
                    customerNote,
                    cancellationToken);
                """), null,
            "Generating text is not the same as knowing truth", "An LLM works with language patterns. It can summarize, classify, or draft, but it can invent details and vary between runs.", "input + instructions → probable output"),
        Step("Compare", "Safe boundary", "Protect credentials and keep the provider call on the server",
            Bad("BrowserAiCall.cs", "Exposed key", """
                // ❌ Every visitor can extract this browser secret.
                string apiKey = "provider-secret";

                await httpClient.PostAsync(
                    "https://provider.example/chat",
                    request);
                """),
            Good("OrderAiAssistant.cs", "Server service", """
                public sealed class OrderAiAssistant(
                    IAiProvider provider,
                    IOptions<AiOptions> options)
                {
                    public Task<AiSuggestion> SuggestAsync(
                        string note,
                        CancellationToken token) =>
                        provider.GenerateAsync(note, token);
                }
                """),
            "The API key never belongs in the client", "The server reads secrets from secure configuration, controls which data leaves the system, and can apply limits, logs, and authorization.", "browser → your API → AI provider"),
        Step("Context", "Prompt and data", "Send only the context needed for the task",
            Pane("SuggestionPrompt.cs", "Instructions", """
                string prompt =
                    "Classify the note as delivery, billing, " +
                    "product, or other. Return JSON and do not " +
                    $"invent order data. Note: {Sanitize(customerNote)}";
                """),
            Pane("DataBoundary.cs", "Data minimization", """
                // Do not send secrets, credentials, or personal data
                // when the task does not require them.
                string safeContext = customerNote[..Math.Min(
                    customerNote.Length,
                    500)];
                """),
            "Context is also a data boundary", "Clear instructions reduce ambiguity but do not guarantee the answer. Minimize data and treat user text as untrusted content.", "instructions + minimum context + expected format"),
        Step("Validate", "Structured output", "Check the response before using it",
            Bad("AiResult.Before.cs", "Automatic trust", """
                // ❌ Unchecked output changes domain state.
                order.Category = modelResponse.Category;
                order.ApproveDiscount(modelResponse.Discount);
                """),
            Good("AiResult.After.cs", "Validation and fallback", """
                if (!AllowedCategories.Contains(result.Category)
                    || result.Summary.Length > 160)
                {
                    return AiSuggestion.Fallback;
                }

                return result with { RequiresReview = true };
                """),
            "Model output is external input", "Validate schema, length, allowlisted values, and permissions. Use a safe fallback when validation fails; never hide a critical rule in a prompt.", "parse → validate → accept or fallback"),
        Step("Integrate", "Junior case", "Assist an order without surrendering domain control",
            Pane("OrderSuggestionService.cs", "Orchestration", """
                public async Task<AiSuggestion> ReviewNoteAsync(
                    string customerNote,
                    CancellationToken token)
                {
                    AiSuggestion suggestion =
                        await assistant.SuggestAsync(customerNote, token);

                    return validator.IsValid(suggestion)
                        ? suggestion
                        : AiSuggestion.Fallback;
                }
                """),
            Pane("OrderSuggestionTests.cs", "Test with a fake", """
                [Fact]
                public async Task InvalidCategory_UsesFallback()
                {
                    var provider = new FakeAiProvider(
                        new("unknown", "text", false));

                    AiSuggestion result = await service.ReviewNoteAsync(
                        "Deliver after 5 PM",
                        default);

                    Assert.True(result.RequiresReview);
                }
                """),
            "Integrate through a replaceable contract", "Tests use a deterministic fake. The laboratory uses the configured provider locally and a labeled simulation on GitHub Pages.", "LLM suggests → code validates → human reviews"));
}
