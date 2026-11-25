using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

/// <summary>
/// Executable lesson content for the completed Middle modules. The shared
/// player owns presentation; this catalog owns bilingual engineering content.
/// </summary>
public static partial class MiddleLessonCatalog
{
    public static IReadOnlyList<string> ImplementedSlugs { get; } =
        [
            "solid",
            "inyeccion-dependencias",
            "strategy-patrones",
            "refactorizacion",
            "ef-core",
            "problem-details",
            "seguridad-jwt",
            "pruebas-integracion",
            "limites-arquitectura",
            "ia-integracion"
        ];

    public static bool TryGet(string slug, AppLanguage language, out ModuleLesson lesson)
    {
        lesson = slug.ToLowerInvariant() switch
        {
            "solid" => Solid(language),
            "inyeccion-dependencias" => DependencyInjection(language),
            "strategy-patrones" => Strategy(language),
            "refactorizacion" => SafeRefactoring(language),
            "ef-core" => EfCore(language),
            "problem-details" => ProblemDetails(language),
            "seguridad-jwt" => JwtAuthorization(language),
            "pruebas-integracion" => IntegrationTesting(language),
            "limites-arquitectura" => ArchitectureBoundaries(language),
            "ia-integracion" => MaintainableAi(language),
            _ => null!
        };

        if (lesson is not null) lesson = WithAdditionalCodeVariants(lesson, language);
        return lesson is not null;
    }

    public static ModuleLesson Get(string slug, AppLanguage language) =>
        TryGet(slug, language, out var lesson)
            ? lesson
            : throw new KeyNotFoundException($"Middle lesson '{slug}' has not been implemented.");

    private static ModuleLesson Solid(AppLanguage language)
    {
        var lesson = Module("solid",
        Step(language, "S · Single Responsibility", "S · Responsabilidad única", "Single Responsibility: one reason to change", "Responsabilidad única: una razón de cambio",
            Bad("OrderService.Before.cs", T(language, "Pricing and notification change together", "Precios y notificación cambian juntos"), """
                public sealed class OrderService
                {
                    public async Task<Order> CreateAsync(CreateOrderRequest request)
                    {
                        decimal total = request.Quantity * request.UnitPrice;
                        Order order = new(request.ProductCode, total);
                        await smtp.SendAsync(order);
                        return order;
                    }
                }
                """),
            Good("OrderService.After.cs", T(language, "The service only orchestrates the use case", "El servicio solo orquesta el caso de uso"), """
                public sealed class OrderService(
                    IOrderFactory factory,
                    INotifier notifier)
                {
                    public async Task<Order> CreateAsync(CreateOrderRequest request)
                    {
                        Order order = factory.Create(request);
                        await notifier.SendAsync(order);
                        return order;
                    }
                }
                """),
            "S — separate independent reasons to change", "S — separa razones independientes de cambio",
            "Pricing rules and email transport evolve for different owners. The application service coordinates them without implementing either detail.", "Las reglas de precio y el transporte de correo evolucionan por responsables distintos. El servicio de aplicación los coordina sin implementar esos detalles.",
            "SRP: pricing changes ≠ notification changes", "SRP: cambios de precio ≠ cambios de notificación"),
        Step(language, "O · Open/Closed", "O · Abierto/cerrado", "Open/Closed: extend behavior without editing stable policy", "Abierto/cerrado: extiende conducta sin editar la política estable",
            Bad("DiscountCalculator.Before.cs", T(language, "Every discount edits the switch", "Cada descuento modifica el switch"), """
                public decimal Apply(Order order, DiscountKind kind) => kind switch
                {
                    DiscountKind.Standard => order.Subtotal,
                    DiscountKind.Vip => order.Subtotal * 0.90m,
                    _ => throw new NotSupportedException()
                };
                """),
            Good("DiscountCalculator.After.cs", T(language, "New policies extend the variation", "Nuevas políticas extienden la variación"), """
                public interface IDiscountPolicy
                {
                    decimal Apply(Order order);
                }

                public sealed class VipDiscount : IDiscountPolicy
                {
                    public decimal Apply(Order order) => order.Subtotal * 0.90m;
                }
                """),
            "O — close stable orchestration; open the real variation", "O — cierra la orquestación estable; abre la variación real",
            "Add a policy when discounts vary independently. Do not create an abstraction for behavior that is unlikely to change.", "Agrega una política cuando los descuentos varíen de forma independiente. No crees una abstracción para conducta que probablemente no cambiará.",
            "OCP: add VipDiscount; do not edit the calculator", "OCP: agrega VipDiscount; no edites el calculador"),
        Step(language, "L · Liskov Substitution", "L · Sustitución de Liskov", "Liskov Substitution: every substitute honors the contract", "Sustitución de Liskov: cada sustituto respeta el contrato",
            Bad("ReadOnlyOrderStore.Before.cs", T(language, "The adapter rejects a promised operation", "El adaptador rechaza una operación prometida"), """
                public sealed class ReadOnlyOrderStore : IOrderStore
                {
                    public Task SaveAsync(Order order, CancellationToken token) =>
                        throw new NotSupportedException("Read only");
                }
                """),
            Good("InMemoryOrderStore.After.cs", T(language, "The substitute preserves expectations", "El sustituto conserva las expectativas"), """
                public sealed class InMemoryOrderStore : IOrderStore
                {
                    private readonly List<Order> orders = [];

                    public Task SaveAsync(Order order, CancellationToken token)
                    {
                        orders.Add(order);
                        return Task.CompletedTask;
                    }
                }
                """),
            "L — a replacement must not weaken the promise", "L — un reemplazo no debe debilitar la promesa",
            "If an adapter cannot save, it must not substitute IOrderStore. Give read-only behavior a different contract instead of throwing for valid use.", "Si un adaptador no puede guardar, no debe sustituir a IOrderStore. Da a la lectura un contrato distinto en lugar de lanzar una excepción ante un uso válido.",
            "LSP: same preconditions + same observable guarantees", "LSP: mismas precondiciones + mismas garantías observables"),
        Step(language, "I · Interface Segregation", "I · Segregación de interfaces", "Interface Segregation: consumers depend only on what they use", "Segregación de interfaces: cada consumidor depende solo de lo que usa",
            Bad("IOrderRepository.Before.cs", T(language, "One oversized contract", "Un contrato sobredimensionado"), """
                public interface IOrderRepository
                {
                    Task SaveAsync(Order order);
                    Task<Order?> FindAsync(Guid id);
                    Task DeleteAsync(Guid id);
                    Task<byte[]> ExportCsvAsync();
                }
                """),
            Good("OrderPorts.After.cs", T(language, "Focused consumer contracts", "Contratos enfocados al consumidor"), """
                public interface IOrderStore
                {
                    Task SaveAsync(Order order, CancellationToken token);
                }

                public interface IOrderReader
                {
                    Task<Order?> FindAsync(Guid id, CancellationToken token);
                }
                """),
            "I — shape interfaces around consumer needs", "I — diseña interfaces según las necesidades del consumidor",
            "The create-order use case should not depend on delete or CSV export. Focused ports prevent unrelated changes from spreading.", "El caso de crear una orden no debe depender de eliminar o exportar CSV. Los puertos enfocados evitan propagar cambios ajenos.",
            "ISP: writer → IOrderStore; query → IOrderReader", "ISP: escritura → IOrderStore; consulta → IOrderReader"),
        Step(language, "D · Dependency Inversion", "D · Inversión de dependencias", "Dependency Inversion: policy owns the abstraction", "Inversión de dependencias: la política posee la abstracción",
            Bad("CreateOrderUseCase.Before.cs", T(language, "Policy constructs infrastructure", "La política construye infraestructura"), """
                public sealed class CreateOrderUseCase
                {
                    private readonly SqliteOrderStore store = new("orders.db");

                    public Task ExecuteAsync(Order order, CancellationToken token) =>
                        store.SaveAsync(order, token);
                }
                """),
            Good("CreateOrderUseCase.After.cs", T(language, "Policy depends on its own port", "La política depende de su propio puerto"), """
                public sealed class CreateOrderUseCase(IOrderStore store)
                {
                    public Task ExecuteAsync(Order order, CancellationToken token) =>
                        store.SaveAsync(order, token);
                }

                builder.Services.AddScoped<IOrderStore, SqliteOrderStore>();
                """),
            "D — high-level policy must not know infrastructure details", "D — la política de alto nivel no debe conocer detalles de infraestructura",
            "The application defines IOrderStore; composition selects SQLite. Tests or another host can supply a different adapter without changing the use case.", "La aplicación define IOrderStore; la composición selecciona SQLite. Las pruebas u otro host pueden aportar otro adaptador sin modificar el caso de uso.",
            "DIP: use case → port ← SQLite adapter", "DIP: caso de uso → puerto ← adaptador SQLite"));

        return lesson with
        {
            Steps = lesson.Steps
                .Select((step, index) => step with
                {
                    CodeVariants =
                    [
                        ClassicSolidVariant(language, index),
                        new(
                            T(language, "Order API", "API de órdenes"),
                            step.Left,
                            step.Right)
                    ],
                    DefaultCodeVariantIndex = 1
                })
                .ToArray()
        };
    }

    private static ModuleLesson DependencyInjection(AppLanguage language) => Module("inyeccion-dependencias",
        Step(language, "Understand", "Entiende", "The container builds an object graph", "El contenedor construye un grafo de objetos",
            Pane("Program.cs", T(language, "Composition root", "Raíz de composición"), """
                builder.Services.AddScoped<IOrderStore, SqliteOrderStore>();
                builder.Services.AddScoped<INotifier, EmailNotifier>();
                builder.Services.AddScoped<OrderService>();
                """), null,
            "DI makes dependencies explicit and composes them at the edge", "DI hace explícitas las dependencias y las compone en el borde",
            "Business services receive capabilities; Program.cs chooses concrete adapters.", "Los servicios de negocio reciben capacidades; Program.cs elige adaptadores concretos.",
            "register → resolve → use", "registrar → resolver → usar"),
        Step(language, "Compare", "Compara", "Constructor injection versus service location", "Inyección por constructor frente a service locator",
            Bad("OrderService.Before.cs", T(language, "Hidden dependency", "Dependencia oculta"), """
                public sealed class OrderService(IServiceProvider services)
                {
                    public Task SaveAsync(Order order)
                    {
                        var store = services.GetRequiredService<IOrderStore>();
                        return store.SaveAsync(order, default);
                    }
                }
                """),
            Good("OrderService.After.cs", T(language, "Explicit dependency", "Dependencia explícita"), """
                public sealed class OrderService(IOrderStore store)
                {
                    public Task SaveAsync(Order order, CancellationToken token) =>
                        store.SaveAsync(order, token);
                }
                """),
            "A constructor documents what an object needs", "Un constructor documenta lo que un objeto necesita",
            "Service location hides requirements until runtime and makes focused tests harder.", "Service locator oculta requisitos hasta ejecución y dificulta pruebas enfocadas.",
            "explicit dependencies > ambient container", "dependencias explícitas > contenedor ambiental"),
        Step(language, "Design", "Diseña", "Choose lifetimes from ownership", "Elige ciclos de vida según propiedad",
            Pane("Program.cs", T(language, "Lifetime selection", "Selección de ciclo de vida"), """
                builder.Services.AddTransient<IClock, SystemClock>();
                builder.Services.AddScoped<OrderDbContext>();
                builder.Services.AddSingleton<CurrencyCatalog>();
                """),
            Pane("LifetimeRules.cs", T(language, "Ownership rule", "Regla de propiedad"), """
                // Transient: a new lightweight instance per resolution.
                // Scoped: shared inside one HTTP request.
                // Singleton: one thread-safe instance for the application.
                """),
            "Long-lived services must not capture shorter-lived state", "Los servicios largos no deben capturar estado de vida más corta",
            "A singleton holding a scoped DbContext creates stale state and unsafe sharing.", "Un singleton que conserva un DbContext scoped produce estado obsoleto y uso compartido inseguro.",
            "singleton ≠ scoped dependency", "singleton ≠ dependencia scoped"),
        Step(language, "Verify", "Comprueba", "Replace true boundaries in the test host", "Sustituye límites reales en el host de pruebas",
            Pane("OrderApiFactory.cs", T(language, "Test composition", "Composición de prueba"), """
                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll<INotifier>();
                    services.AddSingleton<INotifier, RecordingNotifier>();
                });
                """), null,
            "Tests may change composition without changing application code", "Las pruebas pueden cambiar composición sin modificar la aplicación",
            "Replace the external adapter while keeping the real endpoint, serialization, service, and validation flow.", "Sustituye el adaptador externo conservando endpoint, serialización, servicio y validación reales.",
            "same app + controlled adapter", "misma app + adaptador controlado"),
        Step(language, "Integrate", "Integra", "Detect a captive dependency before production", "Detecta una dependencia cautiva antes de producción",
            Bad("NotificationCache.cs", T(language, "Invalid ownership", "Propiedad inválida"), """
                public sealed class NotificationCache(OrderDbContext db)
                {
                    public OrderDbContext Database { get; } = db;
                }

                builder.Services.AddSingleton<NotificationCache>();
                builder.Services.AddScoped<OrderDbContext>();
                """),
            Good("NotificationLookup.cs", T(language, "Safe boundary", "Límite seguro"), """
                public sealed class NotificationLookup(
                    IDbContextFactory<OrderDbContext> factory)
                {
                    public async Task<Order?> FindAsync(Guid id)
                    {
                        await using var db = await factory.CreateDbContextAsync();
                        return await db.Orders.FindAsync(id);
                    }
                }
                """),
            "Lifetimes are correctness boundaries", "Los ciclos de vida son límites de corrección",
            "Use a factory when a longer-lived collaborator must create and own a short-lived resource explicitly.", "Usa una fábrica cuando un colaborador más largo deba crear y poseer explícitamente un recurso corto.",
            "factory creates and disposes the scope", "la fábrica crea y libera el alcance"));

    private static ModuleLesson Strategy(AppLanguage language) => Module("strategy-patrones",
        Step(language, "Understand", "Entiende", "A strategy names interchangeable behavior", "Una estrategia nombra comportamiento intercambiable",
            Pane("INotificationStrategy.cs", T(language, "Variation contract", "Contrato de variación"), """
                public interface INotificationStrategy
                {
                    string Channel { get; }
                    Task SendAsync(Order order, CancellationToken token);
                }
                """), null,
            "Strategy models one operation with multiple implementations", "Strategy modela una operación con varias implementaciones",
            "The orchestrator uses the capability without knowing email, SMS, or push details.", "El orquestador usa la capacidad sin conocer detalles de correo, SMS o push.",
            "stable operation + replaceable behavior", "operación estable + comportamiento reemplazable"),
        Step(language, "Compare", "Compara", "A small switch versus growing variation", "Un switch pequeño frente a variación creciente",
            Bad("OrderNotifier.Before.cs", T(language, "Growing conditional", "Condicional creciente"), """
                if (channel == "email") await SendEmailAsync(order);
                else if (channel == "sms") await SendSmsAsync(order);
                else if (channel == "push") await SendPushAsync(order);
                else throw new UnsupportedChannelException(channel);
                """),
            Good("OrderNotifier.After.cs", T(language, "Named strategy", "Estrategia con nombre"), """
                INotificationStrategy strategy = selector.Get(channel);
                await strategy.SendAsync(order, token);
                """),
            "A switch is acceptable while the variation is small and stable", "Un switch es aceptable mientras la variación sea pequeña y estable",
            "Strategy earns its cost when implementations evolve independently or carry different dependencies.", "Strategy justifica su costo cuando las implementaciones evolucionan por separado o tienen dependencias distintas.",
            "real variation first, pattern second", "primero variación real, después patrón"),
        Step(language, "Design", "Diseña", "Keep selection separate from execution", "Separa selección de ejecución",
            Pane("NotificationStrategySelector.cs", T(language, "Selector", "Selector"), """
                public sealed class NotificationStrategySelector(
                    IEnumerable<INotificationStrategy> strategies)
                {
                    private readonly IReadOnlyDictionary<string, INotificationStrategy> map =
                        strategies.ToDictionary(item => item.Channel);

                    public INotificationStrategy Get(string channel) =>
                        map.TryGetValue(channel, out var strategy)
                            ? strategy
                            : throw new UnsupportedChannelException(channel);
                }
                """), null,
            "The selector decides which behavior; the strategy executes it", "El selector decide cuál conducta; la estrategia la ejecuta",
            "Do not move the original conditional into a factory that also performs the work.", "No muevas el condicional original a una fábrica que también realiza el trabajo.",
            "select once → execute through contract", "seleccionar una vez → ejecutar por contrato"),
        Step(language, "Verify", "Comprueba", "Test each behavior and the selection rule", "Prueba cada conducta y la regla de selección",
            Pane("NotificationStrategySelectorTests.cs", T(language, "Focused tests", "Pruebas enfocadas"), """
                [Fact]
                public void Get_Email_ReturnsEmailStrategy()
                {
                    var email = new FakeStrategy("email");
                    var selector = new NotificationStrategySelector([email]);

                    Assert.Same(email, selector.Get("email"));
                }
                """),
            Pane("EmailNotificationStrategyTests.cs", T(language, "Behavior test", "Prueba de conducta"), """
                [Fact]
                public async Task Send_UsesOrderRecipient()
                {
                    await strategy.SendAsync(order, default);

                    Assert.Equal(order.Email, gateway.LastRecipient);
                }
                """),
            "Selection and behavior fail for different reasons", "Selección y conducta fallan por razones distintas",
            "Separate tests make the source of a regression clear without testing private implementation details.", "Pruebas separadas aclaran el origen de una regresión sin probar detalles privados.",
            "selector test + strategy test", "prueba de selector + prueba de estrategia"),
        Step(language, "Integrate", "Integra", "Add a channel through registration", "Agrega un canal mediante registro",
            Pane("Program.cs", T(language, "Open extension point", "Punto de extensión abierto"), """
                builder.Services.AddScoped<INotificationStrategy, EmailStrategy>();
                builder.Services.AddScoped<INotificationStrategy, SmsStrategy>();
                builder.Services.AddScoped<INotificationStrategy, PushStrategy>();
                """),
            Pane("OrderService.cs", T(language, "Unchanged orchestrator", "Orquestador sin cambios"), """
                public Task NotifyAsync(
                    Order order,
                    string channel,
                    CancellationToken token) =>
                    selector.Get(channel).SendAsync(order, token);
                """),
            "A new strategy should not require editing the orchestrator", "Una estrategia nueva no debe requerir modificar el orquestador",
            "Registration extends the available behavior while the application flow remains stable.", "El registro extiende la conducta disponible mientras el flujo de aplicación permanece estable.",
            "new implementation + registration", "nueva implementación + registro"));

    private static LessonCodeVariant ClassicSolidVariant(AppLanguage language, int index) => index switch
    {
        0 => new(
            T(language, "Classic example", "Ejemplo clásico"),
            Bad("Employee.Before.cs", T(language, "Three actors change one class", "Tres responsables modifican una clase"), ClassicCode(language,
                "Payroll, accounting, and database changes collide inside Employee.",
                "Nómina, contabilidad y base de datos cambian dentro de Employee.", """
                public sealed class Employee
                {
                    public Money CalculatePay() => Money.Zero;
                    public Report ReportHours() => new();
                    public Task SaveAsync() => Task.CompletedTask;
                }
                """)),
            Good("Employee.After.cs", T(language, "One reason to change per collaborator", "Una razón de cambio por colaborador"), ClassicCode(language,
                "Each actor changes an independent collaborator.",
                "Cada responsable modifica un colaborador independiente.", """
                public sealed class PayCalculator
                {
                    public Money Calculate(Employee employee) => Money.Zero;
                }

                public sealed class HoursReporter
                {
                    public Report Create(Employee employee) => new();
                }

                public sealed class EmployeeRepository
                {
                    public Task SaveAsync(Employee employee) => Task.CompletedTask;
                }
                """))),
        1 => new(
            T(language, "Classic example", "Ejemplo clásico"),
            Bad("AreaCalculator.Before.cs", T(language, "Every shape edits the calculator", "Cada figura modifica el calculador"), ClassicCode(language,
                "Adding Triangle requires changing this stable switch.",
                "Agregar Triangle obliga a modificar este switch estable.", """
                public sealed class AreaCalculator
                {
                    public decimal Area(Shape shape) => shape switch
                    {
                        Rectangle rectangle => rectangle.Width * rectangle.Height,
                        Circle circle => (decimal)Math.PI * circle.Radius * circle.Radius,
                        _ => throw new NotSupportedException()
                    };
                }
                """)),
            Good("Shapes.After.cs", T(language, "New shapes extend the contract", "Nuevas figuras extienden el contrato"), ClassicCode(language,
                "Triangle can be added without editing AreaCalculator.",
                "Triangle puede agregarse sin modificar AreaCalculator.", """
                public interface IShape
                {
                    decimal Area { get; }
                }

                public sealed record Rectangle(decimal Width, decimal Height) : IShape
                {
                    public decimal Area => Width * Height;
                }

                public sealed class AreaCalculator
                {
                    public decimal Total(IEnumerable<IShape> shapes) =>
                        shapes.Sum(shape => shape.Area);
                }
                """))),
        2 => new(
            T(language, "Classic example", "Ejemplo clásico"),
            Bad("RubberDuck.Before.cs", T(language, "A subtype breaks the flight promise", "Un subtipo rompe la promesa de vuelo"), ClassicCode(language,
                "RubberDuck compiles as Duck but cannot honor Fly().",
                "RubberDuck compila como Duck, pero no puede cumplir Fly().", """
                public abstract class Duck
                {
                    public abstract void Swim();
                    public abstract void Fly();
                }

                public sealed class RubberDuck : Duck
                {
                    public override void Swim() { }
                    public override void Fly() =>
                        throw new NotSupportedException();
                }
                """)),
            Good("DuckCapabilities.After.cs", T(language, "Each duck promises supported behavior", "Cada pato promete conducta compatible"), ClassicCode(language,
                "Only ducks that can really fly implement IFlyer.",
                "Solo los patos que realmente vuelan implementan IFlyer.", """
                public interface ISwimmer { void Swim(); }
                public interface IFlyer { void Fly(); }

                public sealed class MallardDuck : ISwimmer, IFlyer
                {
                    public void Swim() { }
                    public void Fly() { }
                }

                public sealed class RubberDuck : ISwimmer
                {
                    public void Swim() { }
                }
                """))),
        3 => new(
            T(language, "Classic example", "Ejemplo clásico"),
            Bad("BasicPrinter.Before.cs", T(language, "A small client receives a fat contract", "Un cliente pequeño recibe un contrato grande"), ClassicCode(language,
                "BasicPrinter is forced to reject Scan() and Fax().",
                "BasicPrinter debe rechazar Scan() y Fax().", """
                public interface IMultiFunctionDevice
                {
                    void Print(Document document);
                    void Scan(Document document);
                    void Fax(Document document);
                }

                public sealed class BasicPrinter : IMultiFunctionDevice
                {
                    public void Print(Document document) { }
                    public void Scan(Document document) => throw new NotSupportedException();
                    public void Fax(Document document) => throw new NotSupportedException();
                }
                """)),
            Good("DeviceCapabilities.After.cs", T(language, "Clients choose focused capabilities", "Los clientes eligen capacidades enfocadas"), ClassicCode(language,
                "BasicPrinter implements only the capability it supports.",
                "BasicPrinter implementa solo la capacidad compatible.", """
                public interface IPrinter { void Print(Document document); }
                public interface IScanner { void Scan(Document document); }
                public interface IFax { void Fax(Document document); }

                public sealed class BasicPrinter : IPrinter
                {
                    public void Print(Document document) { }
                }
                """))),
        4 => new(
            T(language, "Classic example", "Ejemplo clásico"),
            Bad("LightSwitch.Before.cs", T(language, "The switch owns one concrete device", "El interruptor controla un dispositivo concreto"), ClassicCode(language,
                "LightSwitch constructs LightBulb and cannot control a Fan.",
                "LightSwitch construye LightBulb y no puede controlar un Fan.", """
                public sealed class LightSwitch
                {
                    private readonly LightBulb bulb = new();

                    public void Toggle() => bulb.TurnOn();
                }
                """)),
            Good("LightSwitch.After.cs", T(language, "The switch depends on a capability", "El interruptor depende de una capacidad"), ClassicCode(language,
                "Composition supplies any device that honors ISwitchable.",
                "La composición aporta cualquier dispositivo que cumpla ISwitchable.", """
                public interface ISwitchable
                {
                    void TurnOn();
                }

                public sealed class LightSwitch(ISwitchable device)
                {
                    public void Toggle() => device.TurnOn();
                }

                ISwitchable device = new LightBulb();
                var control = new LightSwitch(device);
                """))),
        _ => throw new ArgumentOutOfRangeException(nameof(index))
    };

    private static ModuleLesson Module(string slug, params LessonPerspective[] steps) => new(slug, steps);

    private static LessonPerspective Step(
        AppLanguage language,
        string nameEnglish,
        string nameSpanish,
        string captionEnglish,
        string captionSpanish,
        LessonCodePane left,
        LessonCodePane? right,
        string hintTitleEnglish,
        string hintTitleSpanish,
        string hintBodyEnglish,
        string hintBodySpanish,
        string hintExampleEnglish,
        string hintExampleSpanish) =>
        new(
            T(language, nameEnglish, nameSpanish),
            T(language, nameEnglish, nameSpanish),
            T(language, captionEnglish, captionSpanish),
            left,
            right,
            T(language, hintTitleEnglish, hintTitleSpanish),
            T(language, hintBodyEnglish, hintBodySpanish),
            T(language, hintExampleEnglish, hintExampleSpanish));

    private static LessonCodePane Pane(string fileName, string label, string code) => new(fileName, label, code);
    private static LessonCodePane Bad(string fileName, string label, string code) => new(fileName, label, code, "bad-practice");
    private static LessonCodePane Good(string fileName, string label, string code) => new(fileName, label, code, "good-practice");
    private static string ClassicCode(AppLanguage language, string english, string spanish, string code) =>
        $"// {T(language, english, spanish)}\n{code}";
    private static string T(AppLanguage language, string english, string spanish) =>
        language == AppLanguage.Spanish ? spanish : english;
}
