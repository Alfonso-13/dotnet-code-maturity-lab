using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public static class JuniorInterviewCatalog
{
    public static IReadOnlyList<InterviewQuestion> Get(AppLanguage language)
    {
        var topics = JuniorCurriculum.Topics.Take(13);
        var moduleQuestions = language == AppLanguage.Spanish
            ? topics.Select(FromSpanishTopic)
            : topics.Select(FromEnglishTopic);

        return moduleQuestions.Append(AdditionalAiQuestion(language)).ToArray();
    }

    private static InterviewQuestion FromSpanishTopic(LabTopic topic) => new(
        topic.Slug,
        JuniorModuleMetadataCatalog.Get(topic, AppLanguage.Spanish).ShortTitle,
        topic.ChallengeQuestion,
        topic.ChallengeOptions
            .Select(option => new InterviewOption(option.Text, option.IsCorrect, option.Feedback))
            .ToArray(),
        topic.InterviewQuestion,
        topic.InterviewAnswer);

    private static InterviewQuestion FromEnglishTopic(LabTopic topic)
    {
        var content = English[topic.Slug];
        return new InterviewQuestion(
            topic.Slug,
            JuniorModuleMetadataCatalog.Get(topic, AppLanguage.English).ShortTitle,
            content.Prompt,
            content.Options,
            content.DefenseQuestion,
            content.ExpectedAnswer);
    }

    private static readonly IReadOnlyDictionary<string, EnglishInterviewContent> English =
        new Dictionary<string, EnglishInterviewContent>(StringComparer.OrdinalIgnoreCase)
        {
            ["fundamentos-csharp"] = Question(
                "If quantity is 0, what should happen before calculating the order?",
                [
                    Wrong("Continue and save the order", "Saving first would allow an invalid state."),
                    Correct("Enter validation and stop the flow", "A guard clause prevents invalid data from continuing."),
                    Wrong("Automatically convert 0 to 1", "The server should not silently change the client's intent.")
                ],
                "What is the difference between if, switch, and foreach?",
                "if evaluates conditions, switch selects among cases, and foreach iterates through elements. Choose the construct that best communicates the intent."),

            ["metodos-y-clases"] = Question(
                "Why does Total use a private setter?",
                [
                    Correct("So only Order controls how it changes", "The value remains readable while the state transition stays encapsulated."),
                    Wrong("Because private makes the application faster", "Visibility communicates design and state protection, not an automatic optimization."),
                    Wrong("To prevent tests from reading Total", "The getter remains public, so tests can observe the result.")
                ],
                "What is the difference between a constructor, a method, and a property?",
                "A constructor creates an instance, a method performs behavior, and a property exposes a value through an access contract."),

            ["modelos-y-memoria"] = Question(
                "If secondOrder = firstOrder and the instance changes, what does firstOrder observe?",
                [
                    Wrong("Nothing, because a class is copied completely", "Assigning a class variable copies the reference, not the whole object."),
                    Correct("The same change, because both variables reference the instance", "Both variables point to the same object."),
                    Wrong("A compilation error", "Assigning references of the same type is valid.")
                ],
                "What is the difference between class, struct, and interface?",
                "A class represents a reference instance with identity and behavior, a struct represents a copied value, and an interface declares a contract that either can implement."),

            ["poo"] = Question(
                "You need email and SMS without changing OrderService. Which option best represents the variation?",
                [
                    Wrong("Add an if for every provider inside the service", "The service would grow with every new variation."),
                    Correct("Depend on INotifier and inject the implementation", "The service depends on a capability and uses polymorphism."),
                    Wrong("Make every method static", "static does not model alternatives or reduce coupling.")
                ],
                "What are the pillars of OOP, and where does composition fit?",
                "Encapsulation, abstraction, inheritance, and polymorphism are the usual pillars. Composition relates objects and is often preferable for combining independent behavior."),

            ["errores-y-validacion"] = Question(
                "Quantity is 0. Which response best represents the case?",
                [
                    Correct("A correctable validation error", "This is an expected invalid value defined by the contract."),
                    Wrong("An unhandled exception", "The case is expected and should be communicated structurally."),
                    Wrong("201 Created with a warning", "A resource that violates the rules must not be created.")
                ],
                "When would you catch an exception?",
                "Catch it when you can recover, translate the error at a boundary, or add useful context before rethrowing it."),

            ["colecciones-y-genericos"] = Question(
                "You repeatedly need to find an order by Id. Which collection best expresses the intent?",
                [
                    Correct("Dictionary<Guid, Order>", "The Id acts as the lookup key."),
                    Wrong("LinkedList<Order>", "You would still traverse nodes to find the Id."),
                    Wrong("HashSet<decimal>", "It neither represents orders nor uses the Id as a key.")
                ],
                "Why is List usually preferred over LinkedList?",
                "List offers indexed access, efficient iteration, and good memory locality. LinkedList is useful when you already retain nodes and make frequent local insertions or removals."),

            ["linq"] = Question(
                "Which operation executes and stores the query result immediately?",
                [
                    Wrong("Where", "Where normally preserves deferred execution."),
                    Wrong("Select", "Select normally preserves deferred execution."),
                    Correct("ToList", "ToList enumerates and materializes the result.")
                ],
                "What does deferred execution mean in LINQ?",
                "The query is described first and runs when it is enumerated. Materialization captures the result at that moment."),

            ["task-async-await"] = Question(
                "Does creating a Task always create a new thread?",
                [
                    Wrong("Yes, Task and Thread are the same", "Task represents an operation; it is not synonymous with Thread."),
                    Correct("No, an I/O wait may not occupy a thread", "The operation can remain pending until the external result arrives."),
                    Wrong("Only when Task returns a value", "Task and Task<T> differ by result, not by thread creation.")
                ],
                "What is the difference between Task<T> and ValueTask<T>?",
                "Task<T> is the general, composable choice. ValueTask<T> can avoid an allocation when results are often synchronous, but adds constraints and should be justified by measurement."),

            ["pruebas-aaa"] = Question(
                "In which AAA phase do you create the CreateOrderRequest?",
                [
                    Correct("Arrange", "Arrange prepares the scenario and its inputs."),
                    Wrong("Act", "Act executes the behavior under test."),
                    Wrong("Assert", "Assert verifies the observed result.")
                ],
                "What is the difference between a unit test and an integration test?",
                "A unit test isolates a small behavior and runs quickly. An integration test verifies that real components collaborate correctly across a boundary."),

            ["rest-http"] = Question(
                "The order was created and is available at /api/orders/42. Which response fits?",
                [
                    Wrong("200 without headers", "200 can return a result but does not communicate creation as precisely."),
                    Correct("201 Created with Location", "It communicates creation and identifies the new resource location."),
                    Wrong("204 No Content", "204 does not communicate the created resource or its location.")
                ],
                "Which HTTP methods and status codes would you use for an order lifecycle?",
                "Use POST with 201 to create, GET with 200 or 404 to read, PUT or PATCH with 200/204 to update, and DELETE with 204 or 404 to remove."),

            ["persistencia-ef-core"] = Question(
                "You only need to display an order. Which option avoids unnecessary tracking?",
                [
                    Correct("Use AsNoTracking", "It marks the query as read-only and avoids retaining entity changes."),
                    Wrong("Call SaveChanges after reading", "A read does not need to save changes."),
                    Wrong("Create one global DbContext", "DbContext should be short-lived and not globally shared.")
                ],
                "What is the difference between a primary key and a foreign key?",
                "A primary key uniquely identifies a row. A foreign key references another table's key and expresses the relationship between them."),

            ["aspnet-core"] = Question(
                "What is the difference between authentication and authorization?",
                [
                    Correct("Authentication identifies; authorization evaluates permissions", "Identity is established before the application decides what it may do."),
                    Wrong("They are two names for DTO validation", "Input validation is a different application boundary."),
                    Wrong("Authorization always happens before authentication", "Permission evaluation normally requires an established identity.")
                ],
                "What is the difference between Transient, Scoped, and Singleton?",
                "Transient creates an instance per resolution, Scoped shares one inside a scope such as an HTTP request, and Singleton retains one for the application lifetime."),

            ["ia-llm"] = Question(
                "What should you do before using a response generated by an LLM?",
                [
                    Correct("Validate its structure and rules before applying it", "Model output is external input and may be incorrect or unexpected."),
                    Wrong("Trust it because the provider already trained the model", "Training does not guarantee accuracy or compliance with your rules."),
                    Wrong("Store the provider key in the browser", "A client-side key is exposed and can be used by third parties.")
                ],
                "What is the difference between a business rule and an LLM suggestion?",
                "A business rule must be deterministic and verifiable. An LLM suggestion is probabilistic, can vary, and must be validated before it influences the system.")
        };

    private static InterviewQuestion AdditionalAiQuestion(AppLanguage language) =>
        language == AppLanguage.Spanish
            ? new InterviewQuestion(
                "ia-llm",
                "IA y LLM",
                "¿Dónde debe realizarse la llamada a un proveedor de IA que requiere una clave secreta?",
                [
                    new("En un servicio del servidor", true, "Correcto: el servidor protege la credencial y controla datos, límites y autorización."),
                    new("Directamente desde JavaScript en el navegador", false, "El navegador expondría la clave a cualquier visitante."),
                    new("Dentro del prompt", false, "Un prompt no es un almacén seguro de secretos.")
                ],
                "¿Qué controles mínimos aplicarías a una integración con un LLM?",
                "Proteger credenciales en el servidor, minimizar datos, definir formato, validar la salida, controlar errores y tiempos de espera, registrar evidencia sin secretos y ofrecer un fallback seguro.")
            : new InterviewQuestion(
                "ia-llm",
                "AI and LLMs",
                "Where should a call to an AI provider that requires a secret key be made?",
                [
                    new("In a server-side service", true, "The server protects the credential and controls data, limits, and authorization."),
                    new("Directly from browser JavaScript", false, "The browser would expose the key to every visitor."),
                    new("Inside the prompt", false, "A prompt is not a secure secret store.")
                ],
                "What minimum controls would you apply to an LLM integration?",
                "Protect credentials on the server, minimize data, define an output format, validate responses, handle errors and timeouts, log evidence without secrets, and provide a safe fallback.");

    private static EnglishInterviewContent Question(
        string prompt,
        InterviewOption[] options,
        string defenseQuestion,
        string expectedAnswer) => new(prompt, options, defenseQuestion, expectedAnswer);

    private static InterviewOption Correct(string text, string feedback) => new(text, true, feedback);
    private static InterviewOption Wrong(string text, string feedback) => new(text, false, feedback);

    private sealed record EnglishInterviewContent(
        string Prompt,
        InterviewOption[] Options,
        string DefenseQuestion,
        string ExpectedAnswer);
}
