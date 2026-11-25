namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson AspNetCoreSpanish() => Module("aspnet-core",
        Step("Entiende", "Composición", "Registrar servicios y construir el pipeline HTTP",
            Pane("Program.cs", "Punto de entrada", """
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                builder.Services.AddDbContextFactory<OrderDbContext>(options =>
                    options.UseSqlite(connectionString));
                builder.Services.AddScoped<IOrderStore, SqliteOrderStore>();
                builder.Services.AddScoped<CreateOrderService>();

                WebApplication app = builder.Build();

                app.UseExceptionHandler();
                app.MapOrderEndpoints();
                app.Run();
                """), null,
            "Program.cs conecta la aplicación", "Registra capacidades, construye el pipeline y publica endpoints; las reglas de negocio permanecen en servicios comprobables.", "services → app → middleware → endpoints"),
        Step("Lifetimes", "Lifetimes de DI", "Elegir cuánto vive una dependencia",
            Pane("ServiceLifetimes.cs", "Transient / Scoped", """
                // Nueva instancia cada vez que se solicita.
                builder.Services.AddTransient<IClock, SystemClock>();

                // Una instancia dentro de la solicitud HTTP.
                builder.Services.AddScoped<IOrderStore, SqliteOrderStore>();
                builder.Services.AddDbContextFactory<OrderDbContext>();
                """),
            Pane("SingletonService.cs", "Singleton", """
                // Una instancia durante la vida de la aplicación.
                builder.Services.AddSingleton<IAppMetadata, AppMetadata>();

                // Un Singleton no debe capturar un servicio Scoped.
                // DbContext tampoco debe registrarse como Singleton.
                """),
            "El lifetime forma parte del contrato operativo", "Transient crea por resolución; Scoped comparte por solicitud; Singleton comparte globalmente y exige seguridad ante concurrencia.", "Transient | Scoped | Singleton"),
        Step("Pipeline", "Middleware", "El orden determina qué ocurre con cada solicitud",
            Bad("Pipeline.Before.cs", "Orden incorrecto", """
                app.MapOrderEndpoints();

                // Estos middleware quedaron después de los endpoints.
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseExceptionHandler();
                """),
            Good("Pipeline.After.cs", "Orden explícito", """
                app.UseExceptionHandler();
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapOrderEndpoints();
                """),
            "El pipeline se ejecuta en orden", "Manejo de errores, HTTPS, identidad y permisos deben participar antes de ejecutar el endpoint protegido.", "request → middleware → endpoint → response"),
        Step("Operación", "Configuración, logs y seguridad", "Adaptar y observar sin escribir secretos en el código",
            Pane("OrderOptions.cs", "Configuración y logs", """
                string connectionString = builder.Configuration
                    .GetConnectionString("Orders")
                    ?? throw new InvalidOperationException(
                        "Falta ConnectionStrings:Orders");

                logger.LogInformation(
                    "Creando orden para {ProductCode}",
                    request.ProductCode);
                """),
            Pane("Security.cs", "Identidad y permisos", """
                // Autenticación: ¿quién realiza la solicitud?
                builder.Services.AddAuthentication()
                    .AddJwtBearer();

                // Autorización: ¿qué puede hacer esa identidad?
                builder.Services.AddAuthorization();

                orders.MapPost("/", CreateOrder)
                    .RequireAuthorization("orders.write");
                """),
            "Configura fuera del código y registra contexto", "No guardes secretos en el repositorio. Los logs usan propiedades estructuradas; autenticación identifica y autorización evalúa permisos.", "configuración + logging + auth"),
        Step("Integra", "Aplicación mínima", "Componer el laboratorio público con SQLite",
            Pane("ServiceRegistration.cs", "Servicios", """
                builder.Services.AddDbContextFactory<OrderDbContext>(options =>
                    options.UseSqlite(connectionString));
                builder.Services.AddScoped<IOrderStore, SqliteOrderStore>();
                builder.Services.AddScoped<CreateOrderService>();
                """),
            Pane("HttpPipeline.cs", "Pipeline", """
                WebApplication app = builder.Build();

                app.UseHttpsRedirection();
                app.UseStaticFiles();
                app.UseAntiforgery();

                app.MapOrderEndpoints();
                app.Run();
                """),
            "Una API es más que sus endpoints", "El laboratorio es público: DI compone EF Core y el servicio, mientras el pipeline sirve la interfaz y publica los endpoints. Autenticación y autorización se agregan cuando exista identidad.", "DI + SQLite + middleware + endpoints"));

    private static ModuleLesson AspNetCoreEnglish() => Module("aspnet-core",
        Step("Understand", "Composition", "Register services and build the HTTP pipeline",
            Pane("Program.cs", "Entry point", """
                WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

                builder.Services.AddDbContextFactory<OrderDbContext>(options =>
                    options.UseSqlite(connectionString));
                builder.Services.AddScoped<IOrderStore, SqliteOrderStore>();
                builder.Services.AddScoped<CreateOrderService>();

                WebApplication app = builder.Build();
                app.UseExceptionHandler();
                app.MapOrderEndpoints();
                app.Run();
                """), null,
            "Program.cs is the composition root", "The container creates dependencies; the pipeline defines how requests reach endpoints.", "services → pipeline → endpoints"),
        Step("Compare", "Dependency injection", "Receive collaborators instead of constructing them",
            Bad("CreateOrderService.Before.cs", "Hard dependency", """
                public sealed class CreateOrderService
                {
                    private readonly SqliteOrderStore store = new();
                }
                """),
            Good("CreateOrderService.After.cs", "Injected contract", """
                public sealed class CreateOrderService(IOrderStore store)
                {
                    public Task AddAsync(Order order, CancellationToken token)
                    {
                        return store.AddAsync(order, token);
                    }
                }
                """),
            "Dependencies should be visible", "Constructor injection exposes what the service needs and allows tests to replace infrastructure.", "contract → constructor → service"),
        Step("Pipeline", "Middleware", "Order cross-cutting HTTP behavior deliberately",
            Pane("Pipeline.cs", "Middleware order", """
                app.UseExceptionHandler();
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();

                app.MapOrderEndpoints();
                """),
            Pane("RequestFlow.cs", "Request flow", """
                // Each middleware can act before and after the next one.
                app.Use(async (context, next) =>
                {
                    logger.LogInformation("Request {Path}", context.Request.Path);
                    await next(context);
                });
                """),
            "Middleware order changes behavior", "Exception handling must wrap later work; authentication must identify a user before authorization evaluates access.", "request → middleware → endpoint → response"),
        Step("Configuration", "Environments", "Keep environment values outside source code",
            Bad("Configuration.Before.cs", "Hard-coded secret", """
                string connectionString =
                    "Data Source=orders.db;Password=secret";
                """),
            Good("Configuration.After.cs", "Configuration source", """
                string connectionString = builder.Configuration
                    .GetConnectionString("Orders")
                    ?? throw new InvalidOperationException(
                        "Connection string 'Orders' was not configured.");
                """),
            "Configuration changes by environment", "Use appsettings, environment variables, or secret stores; never commit credentials.", "code + environment configuration"),
        Step("Integrate", "Junior API", "Connect validation, persistence, and HTTP in one testable flow",
            Pane("OrderEndpoints.cs", "Endpoint", """
                group.MapPost("/", async (
                    CreateOrderRequest request,
                    CreateOrderService service,
                    CancellationToken token) =>
                {
                    CreateOrderResult result = await service.CreateAsync(request, token);

                    return result.IsSuccess
                        ? Results.Created($"/api/orders/{result.Order!.Id}", result.Order)
                        : Results.ValidationProblem(result.Errors);
                });
                """),
            Pane("OrderApiTests.cs", "HTTP integration test", """
                [Fact]
                public async Task PostOrder_ValidRequest_ReturnsCreated()
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(
                        "/api/orders",
                        new CreateOrderRequest("MONITOR-24", 2, 3499.90m));

                    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                }
                """),
            "The module closes with observable behavior", "A request crosses the HTTP contract, domain rules, EF Core store, and response mapping while each layer remains testable.", "HTTP → service → EF Core → HTTP"));
}
