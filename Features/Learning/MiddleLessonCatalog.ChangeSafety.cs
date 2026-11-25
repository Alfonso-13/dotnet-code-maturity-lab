using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class MiddleLessonCatalog
{
    private static ModuleLesson SafeRefactoring(AppLanguage language) => Module("refactorizacion",
        Step(language, "Understand", "Entiende", "Describe behavior before changing structure", "Describe la conducta antes de cambiar la estructura",
            Pane("OrderService.CharacterizationTests.cs", T(language, "Existing behavior", "Conducta existente"), """
                [Fact]
                public async Task Create_ValidRequest_PersistsAndReturnsOrder()
                {
                    Order result = await service.CreateAsync(ValidRequest, default);

                    Assert.Equal("MONITOR-24", result.ProductCode);
                    Assert.Equal(6999.80m, result.Subtotal);
                    Assert.Same(result, store.SavedOrder);
                }
                """), null,
            "Characterization tests record what the system does today", "Las pruebas de caracterización registran lo que el sistema hace hoy",
            "They create a safety net before deciding whether every current behavior is desirable.", "Crean una red de seguridad antes de decidir si toda conducta actual es deseable.",
            "observe → record → refactor", "observar → registrar → refactorizar"),
        Step(language, "Compare", "Compara", "Rewrite versus incremental refactoring", "Reescritura frente a refactorización incremental",
            Bad("OrderService.Rewrite.cs", T(language, "Large rewrite", "Reescritura grande"), """
                // Validation, mapping, persistence, and responses change together.
                public Task<OrderResponse> CreateV2Async(NewRequest request)
                {
                    throw new NotImplementedException();
                }
                """),
            Good("OrderService.Refactor.cs", T(language, "One safe step", "Un paso seguro"), """
                public Task<Order> CreateAsync(
                    CreateOrderRequest request,
                    CancellationToken token)
                {
                    Order order = orderFactory.Create(request);
                    return SaveAndReturnAsync(order, token);
                }
                """),
            "Change one dimension at a time", "Cambia una dimensión a la vez",
            "A small extraction can be reviewed and reverted without mixing a new contract with a new design.", "Una extracción pequeña puede revisarse y revertirse sin mezclar un contrato nuevo con un diseño nuevo.",
            "structure change ≠ behavior change", "cambio estructural ≠ cambio de conducta"),
        Step(language, "Design", "Diseña", "Create a seam around volatile work", "Crea un seam alrededor del trabajo volátil",
            Pane("IOrderFactory.cs", T(language, "Extraction seam", "Seam de extracción"), """
                public interface IOrderFactory
                {
                    Order Create(CreateOrderRequest request);
                }
                """),
            Pane("OrderFactory.cs", T(language, "Moved decision", "Decisión movida"), """
                public sealed class OrderFactory(IPricePolicy prices) : IOrderFactory
                {
                    public Order Create(CreateOrderRequest request) =>
                        Order.Create(
                            request.ProductCode,
                            request.Quantity,
                            prices.UnitPriceFor(request.ProductCode));
                }
                """),
            "Extract a coherent decision, not arbitrary lines", "Extrae una decisión coherente, no líneas arbitrarias",
            "The seam is useful because pricing and construction can change independently from orchestration.", "El seam es útil porque precio y construcción pueden cambiar aparte de la orquestación.",
            "volatile decision → explicit seam", "decisión volátil → seam explícito"),
        Step(language, "Verify", "Comprueba", "Keep tests green after every transformation", "Mantén verdes las pruebas después de cada transformación",
            Pane("RefactoringLoop.cs", T(language, "Short feedback loop", "Ciclo corto de retroalimentación"), """
                // 1. Start from a green test suite.
                // 2. Apply one structural transformation.
                // 3. Compile and run focused tests.
                // 4. Run the complete suite.
                // 5. Commit a reviewable change.
                """),
            Pane("OrderFactoryTests.cs", T(language, "Focused decision test", "Prueba enfocada de decisión"), """
                [Fact]
                public void Create_UsesPricePolicy()
                {
                    Order order = factory.Create(ValidRequest);

                    Assert.Equal(6999.80m, order.Subtotal);
                }
                """),
            "A green suite is evidence, not a guarantee", "Una suite verde es evidencia, no garantía",
            "Combine focused tests with integration coverage for the observable boundary being preserved.", "Combina pruebas enfocadas con cobertura de integración para el límite observable que se conserva.",
            "small step → focused tests → full suite", "paso pequeño → pruebas enfocadas → suite completa"),
        Step(language, "Integrate", "Integra", "Show the dependency improvement without changing HTTP", "Muestra la mejora de dependencias sin cambiar HTTP",
            Pane("OrderService.After.cs", T(language, "Refactored use case", "Caso de uso refactorizado"), """
                public sealed class OrderService(
                    IOrderFactory factory,
                    IOrderStore store,
                    INotifier notifier)
                {
                    public async Task<Order> CreateAsync(
                        CreateOrderRequest request,
                        CancellationToken token)
                    {
                        Order order = factory.Create(request);
                        await store.SaveAsync(order, token);
                        await notifier.SendAsync(order, token);
                        return order;
                    }
                }
                """),
            Pane("CreateOrderContractTests.cs", T(language, "Preserved contract", "Contrato conservado"), """
                [Fact]
                public async Task PostOrder_ReturnsCreatedWithLocation()
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        ValidRequest);

                    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                    Assert.NotNull(response.Headers.Location);
                }
                """),
            "The endpoint stays stable while dependencies become explicit", "El endpoint permanece estable mientras las dependencias se vuelven explícitas",
            "The final evidence includes focused design tests and the original HTTP contract test.", "La evidencia final incluye pruebas enfocadas de diseño y la prueba original del contrato HTTP.",
            "cleaner graph + same response", "grafo más limpio + misma respuesta"));

    private static ModuleLesson EfCore(AppLanguage language) => Module("ef-core",
        Step(language, "Understand", "Entiende", "A LINQ query becomes SQL and tracked state", "Una consulta LINQ se convierte en SQL y estado rastreado",
            Pane("OrderQueries.cs", T(language, "Intentional read", "Lectura intencional"), """
                OrderSummary? order = await db.Orders
                    .AsNoTracking()
                    .Where(item => item.Id == orderId)
                    .Select(item => new OrderSummary(
                        item.Id,
                        item.ProductCode,
                        item.Subtotal))
                    .SingleOrDefaultAsync(token);
                """), null,
            "Query shape determines data and tracking cost", "La forma de consulta determina datos y costo de tracking",
            "Project read models and use AsNoTracking when no entity changes will be saved.", "Proyecta modelos de lectura y usa AsNoTracking cuando no guardarás cambios de entidades.",
            "filter → project → execute", "filtrar → proyectar → ejecutar"),
        Step(language, "Compare", "Compara", "Loading a graph versus projecting what is needed", "Cargar un grafo frente a proyectar lo necesario",
            Bad("OrderQueries.Before.cs", T(language, "Overloaded entity graph", "Grafo de entidades excesivo"), """
                Order order = await db.Orders
                    .Include(item => item.Lines)
                    .Include(item => item.Customer)
                    .SingleAsync(item => item.Id == orderId, token);

                return MapSummary(order);
                """),
            Good("OrderQueries.After.cs", T(language, "Database projection", "Proyección en base de datos"), """
                return await db.Orders
                    .AsNoTracking()
                    .Where(item => item.Id == orderId)
                    .Select(item => new OrderSummary(
                        item.Id,
                        item.Customer.Name,
                        item.Lines.Sum(line => line.Quantity * line.UnitPrice)))
                    .SingleAsync(token);
                """),
            "Load entities to change them; project data to read it", "Carga entidades para cambiarlas; proyecta datos para leerlos",
            "A projection lets the database return only the columns and aggregate required by the use case.", "Una proyección permite que la base devuelva solo columnas y agregados requeridos por el caso.",
            "command model ≠ read model", "modelo de comando ≠ modelo de lectura"),
        Step(language, "Design", "Diseña", "Define one transaction boundary for the use case", "Define un límite transaccional para el caso de uso",
            Pane("OrderUnitOfWork.cs", T(language, "Atomic change", "Cambio atómico"), """
                await using IDbContextTransaction transaction =
                    await db.Database.BeginTransactionAsync(token);

                db.Orders.Add(order);
                db.InventoryReservations.Add(reservation);
                await db.SaveChangesAsync(token);

                await transaction.CommitAsync(token);
                """),
            Pane("OrderConfiguration.cs", T(language, "Explicit relationship", "Relación explícita"), """
                builder.HasMany(order => order.Lines)
                    .WithOne()
                    .HasForeignKey(line => line.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Supports the actual pending-orders query shape.
                builder.HasIndex(order => new
                {
                    order.Status,
                    order.CreatedAt
                });
                """),
            "A transaction protects one consistency boundary", "Una transacción protege un límite de consistencia",
            "Keep the transaction as short as possible and do not hold it while calling external services.", "Mantén la transacción lo más corta posible y no la conserves mientras llamas servicios externos.",
            "local state changes → one commit", "cambios de estado local → un commit"),
        Step(language, "Verify", "Comprueba", "Detect N+1 behavior through command evidence", "Detecta N+1 mediante evidencia de comandos",
            Bad("OrderReport.Before.cs", T(language, "Query per order", "Consulta por orden"), """
                foreach (Order order in await db.Orders.ToListAsync(token))
                {
                    order.Lines = await db.OrderLines
                        .Where(line => line.OrderId == order.Id)
                        .ToListAsync(token);
                }
                """),
            Good("OrderReport.After.cs", T(language, "Single projection", "Proyección única"), """
                IQueryable<OrderReport> query = db.Orders
                    .AsNoTracking()
                    .Select(order => new OrderReport(
                        order.Id,
                        order.Lines.Count,
                        order.Lines.Sum(line => line.Quantity * line.UnitPrice)));

                // Inspect generated SQL, then use the database EXPLAIN tool
                // to verify index usage and detect scans with realistic data.
                string sql = query.ToQueryString();
                OrderReport[] report = await query.ToArrayAsync(token);
                """),
            "Count database round trips, not only returned rows", "Cuenta viajes a base de datos, no solo filas devueltas",
            "Integration tests can capture commands and fail when one query becomes one plus N queries.", "Las pruebas de integración pueden capturar comandos y fallar cuando una consulta se vuelve una más N.",
            "1 query expected, N+1 rejected", "1 consulta esperada, N+1 rechazada"),
        Step(language, "Integrate", "Integra", "Persist and query the order through verified mappings", "Persiste y consulta la orden mediante mapeos verificados",
            Pane("CreateOrderPersistenceTests.cs", T(language, "SQLite integration", "Integración SQLite"), """
                [Fact]
                public async Task SaveOrder_PersistsLinesAndCalculatedSubtotal()
                {
                    await store.SaveAsync(order, default);

                    OrderSummary? saved = await queries.FindAsync(order.Id, default);

                    Assert.Equal(2, saved!.LineCount);
                    Assert.Equal(order.Subtotal, saved.Subtotal);
                }
                """),
            Pane("AddOrderStatusIndex.cs", T(language, "Reviewed migration", "Migración revisada"), """
                public partial class AddOrderStatusIndex : Migration
                {
                    protected override void Up(MigrationBuilder migration) =>
                        migration.CreateIndex(
                            name: "IX_Orders_Status_CreatedAt",
                            table: "Orders",
                            columns: ["Status", "CreatedAt"]);

                    protected override void Down(MigrationBuilder migration) =>
                        migration.DropIndex(
                            name: "IX_Orders_Status_CreatedAt",
                            table: "Orders");
                }
                """),
            "EF Core does not replace database reasoning", "EF Core no reemplaza el razonamiento de base de datos",
            "The migration makes the index reviewable, while the integration test proves mappings, generated SQL behavior, and stored results together.", "La migración vuelve revisable el índice, mientras la prueba de integración comprueba mapeos, conducta SQL y resultados guardados en conjunto.",
            "query shape + migration + plan + observable result", "forma de consulta + migración + plan + resultado observable"));

    private static ModuleLesson ProblemDetails(AppLanguage language) => Module("problem-details",
        Step(language, "Understand", "Entiende", "Classify failures before mapping HTTP", "Clasifica fallos antes de mapear HTTP",
            Pane("CreateOrderResult.cs", T(language, "Application outcomes", "Resultados de aplicación"), """
                public abstract record CreateOrderResult
                {
                    public sealed record Created(Order Order) : CreateOrderResult;
                    public sealed record Invalid(IDictionary<string, string[]> Errors) : CreateOrderResult;
                    public sealed record Conflict(string Code) : CreateOrderResult;
                }
                """), null,
            "Expected outcomes are not unexpected exceptions", "Los resultados esperados no son excepciones inesperadas",
            "Validation and conflict are part of the use-case contract; unavailable infrastructure is a failure outside it.", "Validación y conflicto forman parte del contrato; infraestructura no disponible es un fallo externo.",
            "outcome first → HTTP mapping second", "primero resultado → después mapeo HTTP"),
        Step(language, "Compare", "Compara", "Ad-hoc errors versus a stable contract", "Errores improvisados frente a un contrato estable",
            Bad("OrderEndpoints.Before.cs", T(language, "Inconsistent JSON", "JSON inconsistente"), """
                return Results.BadRequest(new
                {
                    error = "Invalid order",
                    exception = exception.ToString()
                });
                """),
            Good("OrderEndpoints.After.cs", T(language, "ProblemDetails", "ProblemDetails"), """
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Order conflict",
                    type: "https://httpstatuses.com/409",
                    extensions: new Dictionary<string, object?>
                    {
                        ["code"] = conflict.Code
                    });
                """),
            "Clients need stable fields; operators need internal diagnostics", "Los clientes necesitan campos estables; operación necesita diagnóstico interno",
            "Return a safe problem contract and log the exception with a correlation identifier on the server.", "Devuelve un contrato seguro y registra la excepción con un identificador de correlación en el servidor.",
            "safe response + internal log", "respuesta segura + log interno"),
        Step(language, "Design", "Diseña", "Translate once at the HTTP boundary", "Traduce una vez en el límite HTTP",
            Pane("CreateOrderHttpMapper.cs", T(language, "Result mapping", "Mapeo de resultados"), """
                public static IResult ToHttpResult(this CreateOrderResult result) =>
                    result switch
                    {
                        CreateOrderResult.Created created =>
                            Results.Created($"/api/orders/{created.Order.Id}", created.Order),
                        CreateOrderResult.Invalid invalid =>
                            Results.ValidationProblem(invalid.Errors),
                        CreateOrderResult.Conflict conflict =>
                            OrderProblems.Conflict(conflict.Code),
                        _ => throw new UnreachableException()
                    };
                """), null,
            "Transport translation belongs at the transport boundary", "La traducción de transporte pertenece al límite de transporte",
            "The application result stays independent from HTTP while every endpoint reuses the same mapper.", "El resultado de aplicación permanece independiente de HTTP mientras cada endpoint reutiliza el mismo mapper.",
            "application result → one HTTP mapper", "resultado de aplicación → un mapper HTTP"),
        Step(language, "Verify", "Comprueba", "Assert the complete error contract", "Comprueba el contrato de error completo",
            Pane("OrderProblemDetailsTests.cs", T(language, "Contract assertion", "Aserción de contrato"), """
                [Fact]
                public async Task DuplicateOrder_ReturnsConflictProblem()
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        DuplicateRequest);

                    Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
                    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                    Assert.Equal("Order conflict", problem!.Title);
                    Assert.False(problem.Extensions.ContainsKey("stackTrace"));
                }
                """), null,
            "A status code alone is not a contract test", "Un código de estado por sí solo no es una prueba de contrato",
            "Assert media type, status, title, type, safe extensions, and absence of sensitive diagnostics.", "Comprueba media type, status, title, type, extensiones seguras y ausencia de diagnóstico sensible.",
            "status + shape + safety", "estado + forma + seguridad"),
        Step(language, "Integrate", "Integra", "Handle unexpected exceptions centrally", "Maneja excepciones inesperadas de forma centralizada",
            Pane("Program.cs", T(language, "Exception handler", "Manejador de excepciones"), """
                builder.Services.AddProblemDetails();
                builder.Services.AddExceptionHandler<ApiExceptionHandler>();

                var app = builder.Build();
                app.UseExceptionHandler();
                """),
            Pane("ApiExceptionHandler.cs", T(language, "Safe 500 response", "Respuesta 500 segura"), """
                public async ValueTask<bool> TryHandleAsync(
                    HttpContext context,
                    Exception exception,
                    CancellationToken token)
                {
                    logger.LogError(exception, "Unhandled request failure");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await problemDetails.WriteAsync(new() { HttpContext = context });
                    return true;
                }
                """),
            "Central handling keeps failures consistent and observable", "El manejo central mantiene fallos consistentes y observables",
            "Expected results still map explicitly; the exception handler is the final safe boundary for unexpected failures.", "Los resultados esperados siguen mapeándose explícitamente; el manejador es el límite final para fallos inesperados.",
            "expected result mapping + unexpected exception handler", "mapeo de resultados esperados + manejador inesperado"));
}
