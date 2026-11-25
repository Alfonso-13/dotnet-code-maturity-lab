namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson PersistenceEfCoreSpanish() => Module("persistencia-ef-core",
        Step("Entiende", "Modelo relacional", "Comprender cómo crecería el modelo mediante claves",
            Pane("OrderWithItems.Future.cs", "Evolución PK / FK", """
                // El laboratorio actual guarda una fila por orden.
                // Si la orden incorpora renglones, el modelo puede crecer así:
                public sealed class Order
                {
                    // PK: identifica una orden de forma única.
                    public Guid Id { get; private set; }
                    public List<OrderItem> Items { get; } = [];
                }

                public sealed class OrderItem
                {
                    public Guid Id { get; private set; }

                    // FK: relaciona el renglón con su orden.
                    public Guid OrderId { get; private set; }
                    public string ProductCode { get; private set; } = "";
                }
                """), null,
            "Las relaciones también forman parte del modelo", "Una clave primaria identifica; una clave foránea conecta tablas sin duplicar toda la información de la orden.", "Order 1 → N OrderItems"),
        Step("Compara", "Persistencia", "Guardar entidades sin mezclar infraestructura con el endpoint",
            Bad("OrderEndpoint.Before.cs", "Acceso directo", """
                app.MapPost("/api/orders", async (Order order) =>
                {
                    database.Orders.Add(order);
                    await database.SaveChangesAsync();
                    return Results.Ok(order);
                });
                """),
            Good("SqliteOrderStore.cs", "Repositorio explícito", """
                public sealed class SqliteOrderStore(
                    IDbContextFactory<OrderDbContext> contextFactory)
                    : IOrderStore
                {
                    public async Task AddAsync(
                        Order order,
                        CancellationToken cancellationToken)
                    {
                        await using OrderDbContext database =
                            await contextFactory.CreateDbContextAsync(
                                cancellationToken);

                        database.Orders.Add(order);
                        await database.SaveChangesAsync(cancellationToken);
                    }
                }
                """),
            "El endpoint no necesita conocer EF Core", "La infraestructura persiste; el endpoint conserva el contrato HTTP y el servicio conserva las reglas de la orden.", "endpoint → servicio → store → DbContext"),
        Step("DbContext", "Contexto y migraciones", "Definir el modelo que EF Core conservará",
            Pane("OrderDbContext.cs", "DbContext", """
                public sealed class OrderDbContext(
                    DbContextOptions<OrderDbContext> options)
                    : DbContext(options)
                {
                    public DbSet<Order> Orders => Set<Order>();
                    protected override void OnModelCreating(
                        ModelBuilder modelBuilder)
                    {
                        modelBuilder.Entity<Order>()
                            .HasKey(order => order.Id);
                    }
                }
                """),
            Pane("SchemaLifecycle.cs", "Evolución del esquema", """
                // El laboratorio crea su base local con EnsureCreated.
                await database.Database.EnsureCreatedAsync();

                // En un sistema desplegado usa migraciones revisadas:
                // dotnet ef migrations add CreateOrders
                // dotnet ef database update
                """),
            "El modelo y el esquema evolucionan juntos", "DbContext configura entidades y relaciones; una migración describe cómo llevar la base de datos de una versión a otra.", "modelo → migración → base de datos"),
        Step("Consultas", "Tracking", "Consultar para leer o para modificar",
            Pane("OrderQueries.cs", "Solo lectura", """
                OrderResponse? response = await database.Orders
                    .AsNoTracking()
                    .Where(order => order.Id == orderId)
                    .Select(order => OrderResponse.From(order))
                    .SingleOrDefaultAsync(cancellationToken);
                """),
            Pane("OrderCommands.cs", "Modificar", """
                Order? order = await database.Orders
                    .SingleOrDefaultAsync(
                        current => current.Id == orderId,
                        cancellationToken);

                if (order is null) return;

                order.Confirm();
                await database.SaveChangesAsync(cancellationToken);
                """),
            "No todas las consultas necesitan seguimiento", "AsNoTracking expresa lectura; una entidad seguida permite detectar cambios y persistirlos mediante SaveChangesAsync.", "lectura → AsNoTracking | cambio → tracking"),
        Step("Integra", "Flujo persistente", "Crear y consultar una orden sin filtrar EF Core al contrato HTTP",
            Pane("CreateOrderFlow.cs", "Escritura", """
                public async Task<Order> CreateAsync(
                    CreateOrderRequest request,
                    CancellationToken cancellationToken)
                {
                    Order order = Order.Create(
                        request.ProductCode,
                        request.Quantity,
                        request.UnitPrice);

                    await store.AddAsync(order, cancellationToken);
                    return order;
                }
                """),
            Pane("GetOrderFlow.cs", "Lectura", """
                public Task<OrderResponse?> GetAsync(
                    Guid id,
                    CancellationToken cancellationToken)
                {
                    return database.Orders
                        .AsNoTracking()
                        .Where(order => order.Id == id)
                        .Select(order => OrderResponse.From(order))
                        .SingleOrDefaultAsync(cancellationToken);
                }
                """),
            "Persistir no significa exponer la base de datos", "El dominio crea una orden válida; el store guarda; la consulta proyecta un DTO y el endpoint decide la respuesta HTTP.", "dominio → EF Core → DTO"));

    private static ModuleLesson PersistenceEfCoreEnglish() => Module("persistencia-ef-core",
        Step("Understand", "Relational model", "Grow the model through keys and relationships",
            Pane("OrderWithItems.Future.cs", "PK / FK evolution", """
                // The current lab stores one row per order.
                public sealed class Order
                {
                    // PK: uniquely identifies an order.
                    public Guid Id { get; private set; }
                    public List<OrderItem> Items { get; } = [];
                }

                public sealed class OrderItem
                {
                    public Guid Id { get; private set; }

                    // FK: links this item to its order.
                    public Guid OrderId { get; private set; }
                    public string ProductCode { get; private set; } = "";
                }
                """), null,
            "Relationships are part of the model", "A primary key identifies a row; a foreign key connects tables without duplicating the whole order.", "Order 1 → N OrderItems"),
        Step("Compare", "Persistence", "Store entities without mixing infrastructure into the endpoint",
            Bad("OrderEndpoint.Before.cs", "Direct access", """
                app.MapPost("/api/orders", async (Order order) =>
                {
                    database.Orders.Add(order);
                    await database.SaveChangesAsync();
                    return Results.Ok(order);
                });
                """),
            Good("SqliteOrderStore.cs", "Explicit repository", """
                public sealed class SqliteOrderStore(
                    IDbContextFactory<OrderDbContext> contextFactory) : IOrderStore
                {
                    public async Task AddAsync(
                        Order order,
                        CancellationToken cancellationToken)
                    {
                        await using OrderDbContext database =
                            await contextFactory.CreateDbContextAsync(cancellationToken);

                        database.Orders.Add(order);
                        await database.SaveChangesAsync(cancellationToken);
                    }
                }
                """),
            "The endpoint does not need to know EF Core", "Infrastructure persists, the endpoint owns HTTP, and the service protects order rules.", "endpoint → service → store → DbContext"),
        Step("DbContext", "Context and migrations", "Define the model EF Core will preserve",
            Pane("OrderDbContext.cs", "DbContext", """
                public sealed class OrderDbContext(
                    DbContextOptions<OrderDbContext> options) : DbContext(options)
                {
                    public DbSet<Order> Orders => Set<Order>();

                    protected override void OnModelCreating(ModelBuilder modelBuilder)
                    {
                        modelBuilder.Entity<Order>().HasKey(order => order.Id);
                    }
                }
                """),
            Pane("SchemaLifecycle.cs", "Schema evolution", """
                // The lab creates its local database directly.
                await database.Database.EnsureCreatedAsync();

                // A deployed system uses reviewed migrations:
                // dotnet ef migrations add CreateOrders
                // dotnet ef database update
                """),
            "The model and schema evolve together", "DbContext configures entities and relationships; a migration describes how the database moves between versions.", "model → migration → database"),
        Step("Queries", "Tracking", "Query to read or query to change",
            Pane("OrderQueries.cs", "Read only", """
                OrderResponse? response = await database.Orders
                    .AsNoTracking()
                    .Where(order => order.Id == orderId)
                    .Select(order => OrderResponse.From(order))
                    .SingleOrDefaultAsync(cancellationToken);
                """),
            Pane("OrderCommands.cs", "Modify", """
                Order? order = await database.Orders
                    .SingleOrDefaultAsync(
                        current => current.Id == orderId,
                        cancellationToken);

                if (order is null) return;
                order.Confirm();
                await database.SaveChangesAsync(cancellationToken);
                """),
            "Not every query needs tracking", "AsNoTracking expresses reading; a tracked entity lets EF detect changes for SaveChangesAsync.", "read → AsNoTracking | change → tracking"),
        Step("Integrate", "Persistent flow", "Create and read an order without leaking EF Core into HTTP",
            Pane("CreateOrderFlow.cs", "Write", """
                public async Task<Order> CreateAsync(
                    CreateOrderRequest request,
                    CancellationToken cancellationToken)
                {
                    Order order = Order.Create(
                        request.ProductCode,
                        request.Quantity,
                        request.UnitPrice);

                    await store.AddAsync(order, cancellationToken);
                    return order;
                }
                """),
            Pane("GetOrderFlow.cs", "Read", """
                public Task<OrderResponse?> GetAsync(
                    Guid id,
                    CancellationToken cancellationToken)
                {
                    return database.Orders
                        .AsNoTracking()
                        .Where(order => order.Id == id)
                        .Select(order => OrderResponse.From(order))
                        .SingleOrDefaultAsync(cancellationToken);
                }
                """),
            "Persistence does not mean exposing the database", "The domain creates a valid order, the store saves it, the query projects a DTO, and the endpoint selects the HTTP response.", "domain → EF Core → DTO"));
}
