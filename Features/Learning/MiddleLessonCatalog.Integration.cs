using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class MiddleLessonCatalog
{
    private static ModuleLesson IntegrationTesting(AppLanguage language) => Module("pruebas-integracion",
        Step(language, "Understand", "Entiende", "Verify the collaboration the client actually uses", "Comprueba la colaboración que realmente usa el cliente",
            Pane("CreateOrderApiTests.cs", T(language, "HTTP integration", "Integración HTTP"), """
                public sealed class CreateOrderApiTests(
                    OrderApiFactory factory) : IClassFixture<OrderApiFactory>
                {
                    private readonly HttpClient client = factory.CreateClient();

                    [Fact]
                    public async Task PostValidOrder_ReturnsCreated()
                    {
                        HttpResponseMessage response = await client.PostAsJsonAsync(
                            "/api/orders",
                            ValidRequest);

                        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                    }
                }
                """), null,
            "An HTTP integration test crosses the real application boundary", "Una prueba de integración HTTP cruza el límite real de la aplicación",
            "It verifies routing, serialization, DI, middleware, application behavior, and response mapping together.", "Comprueba en conjunto rutas, serialización, DI, middleware, aplicación y mapeo de respuesta.",
            "request → host → response", "solicitud → host → respuesta"),
        Step(language, "Compare", "Compara", "Calling a service versus exercising the API", "Llamar un servicio frente a ejercer la API",
            Bad("OrderApiTests.Before.cs", T(language, "Not an HTTP test", "No es una prueba HTTP"), """
                [Fact]
                public async Task Create_ReturnsOrder()
                {
                    Order result = await service.CreateAsync(ValidRequest, default);
                    Assert.NotNull(result);
                }
                """),
            Good("OrderApiTests.After.cs", T(language, "Observable contract", "Contrato observable"), """
                [Fact]
                public async Task PostValidOrder_ReturnsContract()
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        ValidRequest);

                    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                    Assert.NotNull(response.Headers.Location);
                    Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
                }
                """),
            "Test at the boundary where the risk exists", "Prueba en el límite donde existe el riesgo",
            "A service test cannot prove route matching, JSON options, middleware order, or HTTP headers.", "Una prueba de servicio no demuestra rutas, opciones JSON, orden de middleware ni encabezados HTTP.",
            "collaboration risk → integration test", "riesgo de colaboración → prueba de integración"),
        Step(language, "Design", "Diseña", "Isolate state and replace only external boundaries", "Aísla estado y sustituye solo límites externos",
            Pane("OrderApiFactory.cs", T(language, "Deterministic host", "Host determinista"), """
                public sealed class OrderApiFactory : WebApplicationFactory<Program>
                {
                    protected override void ConfigureWebHost(IWebHostBuilder builder)
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            services.RemoveAll<DbContextOptions<OrderDbContext>>();
                            services.AddDbContext<OrderDbContext>(options =>
                                options.UseSqlite(CreateIsolatedConnection()));
                            services.AddSingleton<INotifier, RecordingNotifier>();
                        });
                    }
                }
                """), null,
            "Control nondeterminism without replacing the system under test", "Controla el no determinismo sin sustituir el sistema bajo prueba",
            "Keep the real endpoint and database behavior; fake only external notification or AI providers.", "Conserva endpoint y base reales; simula solo proveedores externos de notificación o IA.",
            "real core + controlled edge", "núcleo real + borde controlado"),
        Step(language, "Verify", "Comprueba", "Assert response and persisted evidence", "Comprueba respuesta y evidencia persistida",
            Pane("CreateOrderApiTests.cs", T(language, "End-to-end evidence", "Evidencia de extremo a extremo"), """
                [Fact]
                public async Task PostValidOrder_PersistsCalculatedOrder()
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        ValidRequest);
                    CreatedOrder body = (await response.Content
                        .ReadFromJsonAsync<CreatedOrder>())!;

                    await using OrderDbContext db = factory.CreateDbContext();
                    Order saved = await db.Orders.SingleAsync(order => order.Id == body.Id);

                    Assert.Equal(6999.80m, saved.Subtotal);
                }
                """), null,
            "A useful assertion follows the business evidence", "Una aserción útil sigue la evidencia del negocio",
            "Verify the response contract and the durable state that the use case promises, not every internal call.", "Comprueba el contrato de respuesta y el estado durable prometido, no cada llamada interna.",
            "HTTP evidence + database evidence", "evidencia HTTP + evidencia de base"),
        Step(language, "Integrate", "Integra", "Cover critical success and failure journeys", "Cubre recorridos críticos de éxito y fallo",
            Pane("CreateOrderApiContractTests.cs", T(language, "Critical matrix", "Matriz crítica"), """
                [Theory]
                [InlineData("", 2, 3499.90, HttpStatusCode.BadRequest)]
                [InlineData("MONITOR-24", 0, 3499.90, HttpStatusCode.BadRequest)]
                [InlineData("MONITOR-24", 2, -1, HttpStatusCode.BadRequest)]
                public async Task InvalidRequests_ReturnValidationProblem(
                    string productCode,
                    int quantity,
                    decimal unitPrice,
                    HttpStatusCode expected)
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        new(productCode, quantity, unitPrice));

                    Assert.Equal(expected, response.StatusCode);
                }
                """),
            Pane("TestIsolation.cs", T(language, "Fresh state", "Estado limpio"), """
                public async Task InitializeAsync()
                {
                    await using OrderDbContext db = factory.CreateDbContext();
                    await db.Database.EnsureDeletedAsync();
                    await db.Database.EnsureCreatedAsync();
                }
                """),
            "Deterministic tests own their state and cleanup", "Las pruebas deterministas poseen su estado y limpieza",
            "Each scenario starts from a known database and communicates failures through the public contract.", "Cada escenario comienza con una base conocida y comunica fallos mediante el contrato público.",
            "known state → request → observable evidence", "estado conocido → solicitud → evidencia observable"));

    private static ModuleLesson ArchitectureBoundaries(AppLanguage language) => Module("limites-arquitectura",
        Step(language, "Understand", "Entiende", "Separate policy from delivery and storage details", "Separa políticas de detalles de entrega y almacenamiento",
            Pane("DependencyDirection.cs", T(language, "Dependency rule", "Regla de dependencias"), """
                // Presentation → Application → Domain
                // Infrastructure → Application → Domain
                // Domain does not reference HTTP, EF Core, or providers.
                """),
            Pane("CreateOrderUseCase.cs", T(language, "Application policy", "Política de aplicación"), """
                public interface ICreateOrderUseCase
                {
                    public Task<CreateOrderResult> ExecuteAsync(
                        CreateOrderCommand command,
                        CancellationToken token);
                }
                """),
            "Dependencies point toward stable policy", "Las dependencias apuntan hacia políticas estables",
            "HTTP and EF Core implement outer details; the use case coordinates domain decisions through ports.", "HTTP y EF Core implementan detalles externos; el caso coordina decisiones mediante puertos.",
            "details → ports → policy", "detalles → puertos → política"),
        Step(language, "Compare", "Compara", "Folder ceremony versus enforceable boundaries", "Ceremonia de carpetas frente a límites verificables",
            Bad("OrdersController.cs", T(language, "Leaking details", "Detalles filtrados"), """
                public async Task<IResult> Create(CreateOrderRequest request)
                {
                    db.Orders.Add(MapEntity(request));
                    await db.SaveChangesAsync();
                    return Results.Created("/api/orders", request);
                }
                """),
            Good("CreateOrderEndpoint.cs", T(language, "Thin adapter", "Adaptador delgado"), """
                public static async Task<IResult> Create(
                    CreateOrderRequest request,
                    ICreateOrderUseCase useCase,
                    CancellationToken token)
                {
                    CreateOrderResult result = await useCase.ExecuteAsync(
                        request.ToCommand(),
                        token);
                    return result.ToHttpResult();
                }
                """),
            "A boundary changes dependency ownership, not only file location", "Un límite cambia la propiedad de dependencias, no solo la ubicación del archivo",
            "Moving the same coupled code into folders does not protect application policy from frameworks.", "Mover el mismo código acoplado entre carpetas no protege la política de los frameworks.",
            "dependency direction > folder count", "dirección de dependencias > cantidad de carpetas"),
        Step(language, "Design", "Diseña", "Choose ports around volatile capabilities", "Elige puertos alrededor de capacidades volátiles",
            Pane("ApplicationPorts.cs", T(language, "Owned ports", "Puertos propios"), """
                public interface IOrderStore
                {
                    Task SaveAsync(Order order, CancellationToken token);
                }

                public interface IOrderSuggestionService
                {
                    Task<OrderSuggestion> SuggestAsync(
                        OrderNote note,
                        CancellationToken token);
                }
                """),
            Pane("InfrastructureAdapters.cs", T(language, "Replaceable adapters", "Adaptadores reemplazables"), """
                public sealed class EfOrderStore(OrderDbContext db) : IOrderStore
                {
                    public async Task SaveAsync(Order order, CancellationToken token)
                    {
                        db.Orders.Add(order);
                        await db.SaveChangesAsync(token);
                    }
                }

                public sealed class ConfiguredAiSuggestionService(
                    IAiSuggestionClient client) : IOrderSuggestionService
                {
                    public Task<OrderSuggestion> SuggestAsync(
                        OrderNote note,
                        CancellationToken token) =>
                        client.SuggestAsync(note.LimitedText, token);
                }
                """),
            "The application owns the contracts it needs", "La aplicación posee los contratos que necesita",
            "Adapters conform to those ports, allowing persistence and AI providers to change without rewriting policy.", "Los adaptadores cumplen esos puertos y permiten cambiar persistencia o IA sin reescribir políticas.",
            "application-owned port ← adapter", "puerto de aplicación ← adaptador"),
        Step(language, "Verify", "Comprueba", "Enforce dependency direction automatically", "Protege automáticamente la dirección de dependencias",
            Pane("ArchitectureTests.cs", T(language, "Reference contract", "Contrato de referencias"), """
                [Fact]
                public void Domain_HasNoInfrastructureDependencies()
                {
                    string[] forbidden = ["EntityFrameworkCore", "AspNetCore", "OpenAI"];

                    Assert.DoesNotContain(
                        domainAssembly.GetReferencedAssemblies(),
                        reference => forbidden.Any(reference.Name!.Contains));
                }
                """), null,
            "Architecture rules need executable evidence", "Las reglas de arquitectura necesitan evidencia ejecutable",
            "A diagram becomes trustworthy when project references and tests prevent accidental inward dependencies.", "Un diagrama se vuelve confiable cuando referencias y pruebas impiden dependencias accidentales hacia adentro.",
            "document + enforce + review", "documentar + proteger + revisar"),
        Step(language, "Integrate", "Integra", "Keep one comprehensible solution", "Mantén una solución comprensible",
            Pane("OrderFlow.cs", T(language, "Complete path", "Ruta completa"), """
                // HTTP request
                //   → CreateOrderEndpoint (presentation adapter)
                //   → CreateOrderUseCase (application policy)
                //   → Order (domain rules)
                //   → IOrderStore / IOrderSuggestionService (ports)
                //   → EF Core / configured AI provider (adapters)
                """),
            Pane("BoundaryDecision.cs", T(language, "Pragmatic rule", "Regla pragmática"), """
                bool requiresBoundary =
                    capability.IsVolatile
                    || capability.IsExternal
                    || capability.HasIndependentTests;

                // A separate project is optional; clear dependencies are required.
                """),
            "Use the minimum structure that keeps dependency rules visible", "Usa la estructura mínima que mantenga visibles las reglas de dependencia",
            "Middle demonstrates boundaries without turning every concept into a separate project or generic abstraction.", "Middle demuestra límites sin convertir cada concepto en un proyecto o abstracción genérica.",
            "clear ownership + minimal ceremony", "propiedad clara + ceremonia mínima"));

    private static ModuleLesson MaintainableAi(AppLanguage language) => Module("ia-integracion",
        Step(language, "Understand", "Entiende", "Treat the model as an external collaborator", "Trata el modelo como colaborador externo",
            Pane("IOrderSuggestionService.cs", T(language, "Provider-independent boundary", "Límite independiente del proveedor"), """
                public interface IOrderSuggestionService
                {
                    Task<OrderSuggestion> SuggestAsync(
                        OrderNote note,
                        CancellationToken token);
                }

                public sealed record OrderSuggestion(
                    string Category,
                    string Summary,
                    bool RequiresReview);
                """), null,
            "An LLM returns untrusted, nondeterministic data", "Un LLM devuelve datos no confiables y no deterministas",
            "The application owns the input limit, output schema, validation, timeout, fallback, and review policy.", "La aplicación posee límite de entrada, esquema de salida, validación, timeout, fallback y revisión.",
            "external model → validated suggestion", "modelo externo → sugerencia validada"),
        Step(language, "Compare", "Compara", "Direct SDK coupling versus an application port", "Acoplamiento al SDK frente a un puerto de aplicación",
            Bad("OrderAssistant.Before.cs", T(language, "Provider inside the use case", "Proveedor dentro del caso de uso"), """
                var client = new VendorChatClient(apiKey);
                string response = await client.CompleteAsync(customerNote);
                order.Category = response;
                """),
            Good("OrderAssistant.After.cs", T(language, "Replaceable capability", "Capacidad reemplazable"), """
                OrderSuggestion suggestion = await suggestionService.SuggestAsync(
                    OrderNote.Create(customerNote),
                    token);

                return validator.AcceptOrFallback(suggestion);
                """),
            "Provider SDK types stop at the infrastructure adapter", "Los tipos del SDK se detienen en el adaptador de infraestructura",
            "The use case depends on its own contract and never grants the model authority to mutate the order.", "El caso depende de su contrato y nunca concede al modelo autoridad para modificar la orden.",
            "own contract > vendor response", "contrato propio > respuesta del proveedor"),
        Step(language, "Design", "Diseña", "Constrain input, output, time, and failure", "Limita entrada, salida, tiempo y fallo",
            Pane("ConfiguredAiSuggestionService.cs", T(language, "Bounded call", "Llamada acotada"), """
                public async Task<OrderSuggestion> SuggestAsync(
                    OrderNote note,
                    CancellationToken token)
                {
                    using var timeout = CancellationTokenSource.CreateLinkedTokenSource(token);
                    timeout.CancelAfter(options.Timeout);

                    ProviderResult result = await provider.GenerateAsync(
                        promptFactory.Create(note.LimitedText),
                        timeout.Token);

                    return parser.ParseAndValidate(result.StructuredJson);
                }
                """),
            Pane("OrderSuggestionValidator.cs", T(language, "Allow-list validation", "Validación por lista permitida"), """
                public OrderSuggestion AcceptOrFallback(OrderSuggestion suggestion) =>
                    AllowedCategories.Contains(suggestion.Category)
                    && suggestion.Summary.Length <= 160
                        ? suggestion with { RequiresReview = true }
                        : OrderSuggestion.Fallback;
                """),
            "Structured output still requires validation", "La salida estructurada todavía requiere validación",
            "A schema improves parsing but cannot guarantee truth, permission, domain validity, or safe content.", "Un esquema mejora el parsing pero no garantiza verdad, permisos, validez de dominio ni contenido seguro.",
            "limit → request → parse → validate", "limitar → solicitar → parsear → validar"),
        Step(language, "Verify", "Comprueba", "Use deterministic fakes for application behavior", "Usa fakes deterministas para la conducta de aplicación",
            Pane("OrderSuggestionServiceTests.cs", T(language, "Timeout fallback", "Fallback por timeout"), """
                [Fact]
                public async Task ProviderTimeout_ReturnsDeterministicFallback()
                {
                    var provider = new TimeoutSuggestionProvider();
                    var service = CreateService(provider);

                    OrderSuggestion result = await service.SuggestAsync(note, default);

                    Assert.Equal(OrderSuggestion.Fallback, result);
                }
                """),
            Pane("ProviderContractTests.cs", T(language, "Adapter contract", "Contrato del adaptador"), """
                [Fact]
                public async Task ValidStructuredResponse_MapsToApplicationContract()
                {
                    provider.ReturnJson("{\"category\":\"delivery\",\"summary\":\"After 5 PM\"}");

                    OrderSuggestion result = await adapter.SuggestAsync(note, default);

                    Assert.Equal("delivery", result.Category);
                    Assert.True(result.RequiresReview);
                }
                """),
            "Test application policy separately from provider compatibility", "Prueba política de aplicación aparte de compatibilidad del proveedor",
            "Fakes make timeout and invalid-output paths deterministic; adapter contract tests verify provider translation.", "Los fakes vuelven deterministas timeout y salida inválida; las pruebas de adaptador verifican la traducción.",
            "fake policy tests + adapter contract tests", "pruebas de política con fake + contrato de adaptador"),
        Step(language, "Integrate", "Integra", "Configure real or demo capability without changing UI", "Configura capacidad real o demo sin cambiar la UI",
            Pane("Program.cs", T(language, "Configuration-driven provider", "Proveedor dirigido por configuración"), """
                if (configuration.GetValue<bool>("Demo:UseDeterministicAi"))
                {
                    services.AddScoped<IOrderSuggestionService, DeterministicSuggestionService>();
                }
                else
                {
                    services.AddScoped<IOrderSuggestionService, ConfiguredAiSuggestionService>();
                }
                """),
            Pane("CreateOrderUseCase.cs", T(language, "Unchanged application", "Aplicación sin cambios"), """
                OrderSuggestion suggestion = await suggestions.SuggestAsync(
                    command.Note,
                    token);

                return new CreateOrderResult.Created(
                    order,
                    suggestion with { RequiresReview = true });
                """),
            "Configuration chooses capability; policy remains stable", "La configuración elige capacidad; la política permanece estable",
            "GitHub Pages can simulate the same contract while the server build uses a configured adapter and protected credentials.", "GitHub Pages puede simular el mismo contrato mientras el servidor usa un adaptador configurado y credenciales protegidas.",
            "same port + environment-specific adapter", "mismo puerto + adaptador por ambiente"));
}
