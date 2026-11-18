namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record VisualIdea(string Label, string Value, string Description);

public sealed record ChallengeOption(string Text, bool IsCorrect, string Feedback);

public sealed record LabTopic(
    string Slug,
    string ShortTitle,
    string Title,
    string Level,
    string Block,
    string Summary,
    string Definition,
    string[] KeyIdeas,
    VisualIdea Left,
    VisualIdea Right,
    string Decision,
    string CodeFile,
    string Code,
    string ChallengeQuestion,
    ChallengeOption[] ChallengeOptions,
    string InterviewQuestion,
    string InterviewAnswer);

public static class JuniorCurriculum
{
    public const int FundamentalsCount = 9;

    public static IReadOnlyList<LabTopic> Topics { get; } =
    [
        Topic(
            "fundamentos-csharp", "Fundamentos de C#", "Fundamentos de C#: datos, decisiones y repetición",
            "Cada dato tiene un contrato y cada condición decide cómo continúa el programa.",
            "Aprende a elegir tipos, representar valores opcionales y detener temprano un flujo inválido antes de estudiar estructuras más grandes.",
            ["string y string? expresan contratos distintos.", "decimal es apropiado para importes monetarios.", "if y switch seleccionan caminos; los ciclos recorren o repiten."],
            new("Dato requerido", "string productCode", "Debe existir para crear la orden."),
            new("Dato opcional", "string? notes", "La ausencia tiene un significado válido."),
            "Primero modela el dato; después expresa las decisiones con el flujo más legible.",
            "OrderRules.cs",
            "decimal subtotal = quantity * unitPrice;\n\nif (quantity <= 0)\n{\n    return \"Cantidad inválida\";\n}\n\nforeach (var item in orderItems)\n{\n    subtotal += item.Price;\n}",
            "Si quantity vale 0, ¿qué debe ocurrir antes de calcular la orden?",
            [
                new("Continuar y guardar la orden", false, "Guardar primero permitiría un estado inválido."),
                new("Entrar a la validación y detener el flujo", true, "Correcto: el guard clause evita continuar con datos inválidos."),
                new("Convertir 0 en 1 automáticamente", false, "El servidor no debe cambiar silenciosamente la intención del cliente.")
            ],
            "¿Qué diferencia existe entre if, switch y foreach?",
            "if evalúa condiciones; switch selecciona entre casos; foreach recorre elementos. La elección depende de la intención que deseas expresar."),

        Topic(
            "metodos-y-clases", "Métodos y clases", "Métodos, constructores y propiedades",
            "Una clase protege estado; sus métodos exponen operaciones válidas.",
            "Un constructor crea una instancia válida. Las propiedades controlan acceso al estado y los modificadores public, private, protected e internal definen quién puede usar cada miembro. return entrega un resultado; void no devuelve valor.",
            ["El constructor establece datos obligatorios.", "private set evita cambios arbitrarios desde fuera.", "static pertenece al tipo; readonly evita reasignación después de construir."],
            new("Estado expuesto", "public decimal Total { get; set; }", "Cualquier consumidor puede sobrescribirlo."),
            new("Estado protegido", "public decimal Total { get; private set; }", "La clase controla sus cambios."),
            "Expón la mínima superficie necesaria y conserva dentro de la clase las reglas que protegen su estado.",
            "Order.cs",
            "public sealed class Order\n{\n    public string ProductCode { get; }\n    public decimal Total { get; private set; }\n\n    public Order(string productCode)\n    {\n        ProductCode = productCode;\n    }\n\n    public void Recalculate(int quantity, decimal price)\n        => Total = quantity * price;\n}",
            "¿Por qué Total tiene private set?",
            [
                new("Para que solo Order controle cómo cambia", true, "Correcto: la propiedad se consulta desde fuera, pero la regla de cambio queda encapsulada."),
                new("Porque private hace más rápida la aplicación", false, "La visibilidad expresa diseño y seguridad del estado, no una optimización automática."),
                new("Para impedir que las pruebas lean Total", false, "El getter sigue siendo público; las pruebas pueden observar el resultado.")
            ],
            "¿Qué diferencia hay entre constructor, método y propiedad?",
            "El constructor crea una instancia; un método ejecuta una conducta; una propiedad expone un valor mediante un contrato de acceso."),

        Topic(
            "modelos-y-memoria", "Modelos y memoria", "Valor, referencia, class, struct, record y enum",
            "La forma de un modelo comunica identidad, valor y estados permitidos.",
            "class es un tipo por referencia y suele modelar identidad; struct es un valor que se copia completo; interface declara un contrato; record facilita igualdad por datos; enum limita un conjunto de opciones con nombre.",
            ["Asignar una class copia la referencia, no el objeto.", "struct representa valores pequeños e interface expresa capacidades.", "record y enum reducen estados ambiguos."],
            new("Identidad", "class Order", "La orden sigue siendo la misma aunque cambie su estado."),
            new("Valor", "record Money", "Dos valores equivalentes se comparan por sus datos."),
            "Elige por significado y mutabilidad; optimiza memoria solo después de medir.",
            "OrderModels.cs",
            "public enum OrderStatus\n{\n    Draft, Confirmed, Cancelled\n}\n\npublic sealed class Order\n{\n    public Guid Id { get; init; }\n    public OrderStatus Status { get; private set; }\n}\n\npublic sealed record CreateOrderRequest(\n    string ProductCode, int Quantity, decimal UnitPrice);",
            "Si secondOrder = firstOrder y cambias la instancia, ¿qué observa firstOrder?",
            [
                new("Nada, porque class se copia completa", false, "Una class no se copia completa con una asignación."),
                new("El mismo cambio, porque ambas variables apuntan a la instancia", true, "Correcto: se copió la referencia al mismo objeto."),
                new("Un error de compilación", false, "Asignar referencias del mismo tipo es válido.")
            ],
            "¿Qué diferencia existe entre class, struct e interface?",
            "class representa una instancia por referencia con identidad y comportamiento; struct representa un valor que se copia; interface declara un contrato que una class o struct puede implementar."),

        Topic(
            "poo", "POO", "Programación orientada a objetos",
            "POO modela un problema mediante objetos que reúnen estado, comportamiento y colaboraciones.",
            "Abstracción modela lo relevante; encapsulación protege el estado; herencia especializa cuando existe sustitución; polimorfismo permite usar implementaciones mediante un contrato. La composición combina colaboradores sin forzar una jerarquía.",
            ["Una interface describe una capacidad.", "Polimorfismo permite sustituir implementaciones.", "Composición suele reducir el acoplamiento frente a herencias innecesarias."],
            new("Herencia rígida", "EmailOrderService : OrderService", "Mezcla variaciones con la jerarquía."),
            new("Composición", "OrderService(INotifier)", "El servicio recibe la capacidad que necesita."),
            "Usa herencia cuando el subtipo pueda sustituir al tipo base; usa composición para combinar capacidades independientes.",
            "Notifications.cs",
            "public interface INotifier\n{\n    Task SendAsync(Order order);\n}\n\npublic sealed class OrderService(INotifier notifier)\n{\n    public Task NotifyAsync(Order order)\n        => notifier.SendAsync(order);\n}",
            "Debes soportar correo y SMS sin cambiar OrderService. ¿Qué opción expresa mejor la variación?",
            [
                new("Agregar if por cada proveedor dentro del servicio", false, "El servicio crecería con cada nueva variante."),
                new("Usar INotifier e inyectar la implementación", true, "Correcto: el servicio depende de una capacidad y usa polimorfismo."),
                new("Convertir todos los métodos en static", false, "static no modela variantes ni reduce el acoplamiento.")
            ],
            "¿Cuáles son los pilares de POO y dónde aparece composición?",
            "Encapsulación, abstracción, herencia y polimorfismo son los pilares habituales. Composición es una relación entre objetos y suele ser preferible para combinar conductas."),

        Topic(
            "errores-y-validacion", "Errores y validación", "Validación, resultados y excepciones",
            "Un dato inválido esperado no es igual a un fallo inesperado.",
            "La validación devuelve errores corregibles. Una excepción interrumpe el flujo cuando el nivel actual no puede continuar. catch debe recuperar, traducir o agregar contexto; nunca ocultar silenciosamente.",
            ["Valida antes de modificar estado.", "No uses excepciones como flujo normal.", "ProblemDetails ofrece errores HTTP con estructura consistente."],
            new("Resultado esperado", "Quantity = 0", "El cliente puede corregir la solicitud."),
            new("Fallo inesperado", "Base de datos no disponible", "La operación no puede continuar normalmente."),
            "Representa errores esperados como resultados y reserva excepciones para fallos excepcionales.",
            "CreateOrderService.cs",
            "if (request.Quantity <= 0)\n{\n    return CreateOrderResult.Invalid(\n        \"Quantity debe ser mayor que cero.\");\n}\n\ntry\n{\n    await repository.SaveAsync(order);\n}\ncatch (DatabaseException exception)\n{\n    logger.LogError(exception, \"No se guardó la orden\");\n    throw;\n}",
            "Quantity es 0. ¿Qué respuesta representa mejor el caso?",
            [
                new("Un error de validación corregible", true, "Correcto: es un dato inválido previsto por el contrato."),
                new("Una excepción no controlada", false, "El caso es esperado y debe comunicarse de forma estructurada."),
                new("201 Created con una advertencia", false, "No debe crearse un recurso que viola las reglas.")
            ],
            "¿Cuándo capturarías una excepción?",
            "Cuando puedas recuperar la operación, traducir el error a otro nivel o agregar contexto útil antes de relanzar."),

        Topic(
            "colecciones-y-genericos", "Colecciones", "Colecciones, genéricos y complejidad",
            "La colección correcta depende de cómo se consultan y modifican los datos.",
            "Los genéricos permiten escribir tipos reutilizables como List<T>. Array tiene tamaño fijo; List mantiene una secuencia; Dictionary accede por clave; HashSet conserva valores únicos; LinkedList conecta nodos para cambios locales.",
            ["List es la opción general para secuencias.", "Dictionary y HashSet expresan clave y unicidad.", "LinkedList conviene cuando conservas el nodo y el patrón lo justifica."],
            new("Recorrido ordenado", "List<Order>", "Buena localidad de memoria y acceso por índice."),
            new("Búsqueda por Id", "Dictionary<Guid, Order>", "Expresa acceso mediante una clave única."),
            "Selecciona por las operaciones frecuentes y mide antes de elegir una estructura especializada.",
            "OrderStore.cs",
            "var orders = new List<Order>();\nvar byId = new Dictionary<Guid, Order>();\nvar productCodes = new HashSet<string>();\n\norders.Add(order);\nbyId[order.Id] = order;\nproductCodes.Add(order.ProductCode);",
            "Necesitas encontrar una orden repetidamente por Id. ¿Qué colección expresa mejor la intención?",
            [
                new("Dictionary<Guid, Order>", true, "Correcto: el Id actúa como clave de acceso."),
                new("LinkedList<Order>", false, "Aún tendrías que recorrer nodos para encontrar el Id."),
                new("HashSet<decimal>", false, "No representa órdenes ni usa el Id como clave.")
            ],
            "¿Por qué List suele preferirse sobre LinkedList?",
            "List ofrece acceso por índice, recorrido eficiente y buena localidad de memoria. LinkedList destaca cuando ya conservas el nodo y haces cambios locales frecuentes."),

        Topic(
            "linq", "LINQ", "LINQ y expresiones lambda",
            "LINQ significa Language Integrated Query: consulta integrada en el lenguaje.",
            "LINQ usa operadores y lambdas para filtrar, transformar, ordenar y agrupar fuentes de datos. Muchas consultas son diferidas: se ejecutan al enumerarse o materializarse.",
            ["Where filtra; Select transforma.", "Any comprueba existencia.", "ToList materializa el resultado en ese momento."],
            new("Imperativo", "foreach + if", "Describe paso por paso el recorrido."),
            new("Declarativo", "Where + Select", "Describe la forma del resultado."),
            "Usa LINQ cuando mejora la lectura y evita enumerar accidentalmente la misma consulta varias veces.",
            "OrderQueries.cs",
            "var pendingTotals = orders\n    .Where(order => order.Status == OrderStatus.Pending)\n    .Select(order => order.Subtotal)\n    .ToList();\n\nbool hasLargeOrder = orders.Any(\n    order => order.Subtotal > 10_000m);",
            "¿Qué operación ejecuta y conserva inmediatamente la consulta?",
            [
                new("Where", false, "Where normalmente conserva ejecución diferida."),
                new("Select", false, "Select normalmente conserva ejecución diferida."),
                new("ToList", true, "Correcto: ToList enumera y materializa el resultado.")
            ],
            "¿Qué significa ejecución diferida en LINQ?",
            "La consulta se construye primero y se ejecuta al enumerarla. Materializar fija el resultado de ese momento."),

        Topic(
            "task-async-await", "Task y async", "Task, Task<T>, async y await",
            "Task representa una operación que puede terminar en el futuro; no representa necesariamente un hilo.",
            "Task permite observar finalización y errores; Task<T> además entrega un valor. ValueTask<T> es una optimización puntual para resultados frecuentemente síncronos. await suspende la continuación sin bloquear el hilo durante una espera de entrada o salida.",
            ["Evita async void salvo controladores de eventos.", "Usa Task por defecto y ValueTask solo después de medir.", "CancellationToken permite solicitar cancelación cooperativa."],
            new("Sin resultado", "Task SaveAsync()", "Expone finalización y errores."),
            new("Con resultado", "Task<Order> GetAsync()", "Entrega una Order al completar."),
            "Usa asincronía para esperas de I/O y no envuelvas automáticamente cada operación en Task.Run.",
            "OrderService.cs",
            "public async Task<Order> CreateAsync(\n    CreateOrderRequest request,\n    CancellationToken cancellationToken)\n{\n    var order = new Order(request.ProductCode);\n    await repository.SaveAsync(order, cancellationToken);\n    return order;\n}",
            "¿Crear un Task significa crear siempre un hilo nuevo?",
            [
                new("Sí, un Task es exactamente un Thread", false, "Task abstrae una operación; no es sinónimo de Thread."),
                new("No, una espera de I/O puede no ocupar un hilo", true, "Correcto: la operación puede quedar pendiente hasta recibir la respuesta."),
                new("Solo cuando Task devuelve un valor", false, "Task y Task<T> se diferencian por el resultado, no por crear hilos.")
            ],
            "¿Cuál es la diferencia entre Task<T> y ValueTask<T>?",
            "Task<T> es la opción general y fácil de componer; ValueTask<T> es un struct que puede evitar una asignación cuando el resultado suele estar disponible sincrónicamente, pero agrega restricciones y debe justificarse con medición."),

        Topic(
            "pruebas-aaa", "Pruebas y AAA", "Pruebas unitarias, integración y AAA",
            "Una prueba útil comprueba una conducta observable y falla por una razón comprensible.",
            "Una prueba unitaria aísla una unidad de conducta; una de integración comprueba colaboración real entre componentes. AAA organiza cada escenario: Arrange prepara, Act ejecuta y Assert verifica.",
            ["Incluye casos válidos, inválidos y límites.", "Prueba resultados, no detalles privados.", "Un mock es útil en un límite, no una obligación en cada prueba."],
            new("Unitaria", "await CreateAsync(request)", "Rápida y enfocada en reglas del servicio."),
            new("Integración", "POST /api/orders", "Comprueba endpoint, serialización y respuesta HTTP."),
            "El tipo de prueba depende del riesgo; combina pruebas unitarias e integración sin duplicar todos los escenarios.",
            "CreateOrderServiceTests.cs",
            "[Fact]\npublic async Task Create_WithZeroQuantity_ReturnsError()\n{\n    // Arrange\n    var request = new CreateOrderRequest(\"MONITOR\", 0, 50m);\n\n    // Act\n    var result = await service.CreateAsync(request);\n\n    // Assert\n    Assert.Contains(\"Quantity\", result.Errors);\n}",
            "¿En qué parte de AAA se construye CreateOrderRequest?",
            [
                new("Arrange", true, "Correcto: ahí se prepara el escenario y sus datos."),
                new("Act", false, "Act debe ejecutar la conducta que se está probando."),
                new("Assert", false, "Assert comprueba el resultado observado.")
            ],
            "¿Cuál es la diferencia entre prueba unitaria e integración?",
            "La unitaria enfoca una conducta con dependencias controladas; la integración verifica que componentes reales colaboren correctamente."),

        Topic(
            "rest-http", "REST y HTTP", "REST, DTO, rutas y códigos HTTP",
            "Una API es un contrato observable, no solo un método que devuelve JSON.",
            "Un DTO define datos de entrada o salida. La ruta identifica recursos; GET, POST, PUT, PATCH y DELETE expresan la operación; el código HTTP comunica el resultado. La validación se devuelve con una estructura consistente como ProblemDetails.",
            ["GET consulta; POST crea; PUT reemplaza; PATCH modifica y DELETE elimina.", "200, 201 y 204 comunican éxitos distintos.", "400, 401, 403, 404, 409, 422, 500 y 503 explican familias frecuentes de error."],
            new("Entrada", "CreateOrderRequest", "Datos que el cliente puede decidir."),
            new("Salida", "CreateOrderResponse", "Datos que el servidor garantiza."),
            "Diseña el contrato antes del endpoint y no expongas directamente detalles internos de persistencia.",
            "OrderEndpoints.cs",
            "app.MapPost(\"/api/orders\", async (CreateOrderRequest request) =>\n{\n    var result = await service.CreateAsync(request);\n\n    return result.IsSuccess\n        ? Results.Created($\"/api/orders/{result.Order!.Id}\", result.Order)\n        : Results.ValidationProblem(result.Errors);\n});",
            "La orden fue creada y puede consultarse en /api/orders/42. ¿Qué respuesta corresponde?",
            [
                new("200 sin encabezados", false, "200 puede contener un resultado, pero no comunica tan precisamente la creación."),
                new("201 Created con Location", true, "Correcto: comunica creación e indica la ubicación del nuevo recurso."),
                new("204 No Content", false, "204 no comunica el recurso creado ni su ubicación.")
            ],
            "¿Qué métodos y códigos HTTP usarías para el ciclo de vida de una orden?",
            "GET consulta; POST crea y devuelve 201; PUT reemplaza; PATCH modifica parcialmente; DELETE elimina y puede devolver 204. Los resultados inválidos, ausentes o en conflicto se expresan con 400, 404 o 409 según el caso.",
            block: "API y persistencia"),

        Topic(
            "persistencia-ef-core", "SQL y EF Core", "Persistencia relacional con SQL y Entity Framework Core",
            "Persistir una orden requiere modelar relaciones, consultar con intención y controlar cuándo se guardan los cambios.",
            "Una base relacional organiza datos mediante tablas, claves primarias y foráneas. EF Core traduce entidades y consultas LINQ mediante DbContext, DbSet y migraciones sin eliminar la necesidad de comprender SQL.",
            ["La clave primaria identifica y la clave foránea relaciona.", "DbContext representa una unidad de trabajo de corta duración.", "AsNoTracking evita seguimiento cuando la consulta es solo lectura."],
            new("Modelo actual", "Orders · PK Id", "El laboratorio guarda una fila por orden; una colección futura se relacionaría mediante una clave foránea."),
            new("Código .NET", "DbSet<Order>", "EF Core traduce operaciones de entidades a la base de datos."),
            "Diseña primero los datos y sus relaciones; después usa EF Core para expresar consultas y cambios con claridad.",
            "OrderDbContext.cs",
            "public sealed class OrderDbContext(DbContextOptions<OrderDbContext> options)\n    : DbContext(options)\n{\n    public DbSet<Order> Orders => Set<Order>();\n\n    protected override void OnModelCreating(ModelBuilder modelBuilder)\n    {\n        modelBuilder.Entity<Order>().HasKey(order => order.Id);\n    }\n}",
            "Necesitas consultar una orden para mostrarla sin modificarla. ¿Qué opción evita seguimiento innecesario?",
            [
                new("Usar AsNoTracking", true, "Correcto: expresa que la consulta es de solo lectura y evita mantener cambios de la entidad."),
                new("Llamar SaveChanges después de consultar", false, "Una lectura no necesita guardar cambios."),
                new("Crear un DbContext global", false, "DbContext debe tener una vida corta y no compartirse globalmente.")
            ],
            "¿Qué diferencia existe entre una clave primaria y una clave foránea?",
            "La clave primaria identifica de forma única una fila; la clave foránea referencia la clave de otra tabla y expresa la relación entre ambas.",
            block: "API y persistencia"),

        Topic(
            "aspnet-core", "ASP.NET Core", "Fundamentos de una aplicación ASP.NET Core",
            "Una API también depende de su composición: servicios, configuración, middleware, logs y seguridad básica.",
            "Program.cs registra dependencias y construye el pipeline HTTP. La inyección de dependencias entrega colaboradores; el middleware procesa cada solicitud en orden; configuración y logging permiten adaptar y observar la aplicación.",
            ["Scoped suele coincidir con una solicitud HTTP.", "El orden del middleware modifica el comportamiento.", "Autenticación identifica; autorización decide permisos."],
            new("Servicios", "AddScoped<IOrderStore, SqliteOrderStore>()", "Registra la abstracción con la implementación que usa el laboratorio."),
            new("Pipeline", "UseAuthentication → UseAuthorization", "Cada middleware participa en la solicitud."),
            "Mantén Program.cs como punto de composición: registra capacidades, ordena el pipeline y deja las reglas dentro de servicios comprobables.",
            "Program.cs",
            "builder.Services.AddDbContextFactory<OrderDbContext>(options =>\n    options.UseSqlite(connectionString));\nbuilder.Services.AddScoped<IOrderStore, SqliteOrderStore>();\nbuilder.Services.AddScoped<CreateOrderService>();\n\nvar app = builder.Build();\napp.UseHttpsRedirection();\napp.MapOrderEndpoints();\napp.Run();",
            "¿Qué diferencia existe entre autenticación y autorización?",
            [
                new("Autenticación identifica y autorización evalúa permisos", true, "Correcto: primero se establece quién realiza la solicitud y después qué puede hacer."),
                new("Son dos nombres para validar DTO", false, "La validación de datos es otro límite de la aplicación."),
                new("Autorización siempre ocurre antes de autenticar", false, "Para evaluar permisos normalmente primero necesitas una identidad.")
            ],
            "¿Qué diferencias existen entre Transient, Scoped y Singleton?",
            "Transient crea una instancia por resolución; Scoped comparte una instancia dentro del alcance, normalmente la solicitud; Singleton conserva una instancia durante la vida de la aplicación.",
            block: "Aplicación ASP.NET Core"),

        Topic(
            "ia-llm", "IA y LLM", "Fundamentos de IA generativa y LLM",
            "Un LLM propone texto probable; la aplicación conserva la responsabilidad sobre reglas, permisos y datos.",
            "Un modelo de lenguaje recibe instrucciones y contexto para generar una salida no determinista. La aplicación debe llamar al proveedor desde el servidor, proteger credenciales, limitar los datos enviados, validar la respuesta y ofrecer un fallback.",
            ["El prompt no reemplaza una regla de negocio.", "Las claves del proveedor permanecen en el servidor.", "Toda salida del modelo se trata como dato externo no confiable."],
            new("Asistente", "LLM → sugerencia", "Resume o clasifica información sin decidir por el dominio."),
            new("Aplicación", "validar → aceptar/rechazar", "Comprueba estructura, límites y permisos antes de usar la salida."),
            "Usa IA para asistir tareas ambiguas; conserva las decisiones deterministas en código y exige revisión cuando el impacto lo requiera.",
            "OrderAiAssistant.cs",
            "AiSuggestion suggestion = await assistant.SuggestAsync(\n    request.CustomerNote,\n    cancellationToken);\n\nif (!AllowedCategories.Contains(suggestion.Category))\n{\n    return AiSuggestion.Fallback;\n}\n\n// La sugerencia no modifica la orden automáticamente.\nreturn suggestion with { RequiresReview = true };",
            "¿Qué debes hacer antes de usar una respuesta generada por un LLM?",
            [
                new("Validar su estructura y reglas antes de aplicarla", true, "Correcto: la salida del modelo es entrada externa y puede ser incorrecta o inesperada."),
                new("Confiar en ella porque el proveedor ya entrenó el modelo", false, "El entrenamiento no garantiza exactitud ni cumplimiento de tus reglas."),
                new("Guardar la clave del proveedor en el navegador", false, "Una clave en el cliente queda expuesta y puede ser utilizada por terceros.")
            ],
            "¿Qué diferencia existe entre una regla de negocio y una sugerencia de un LLM?",
            "La regla de negocio debe ser determinista y verificable; la sugerencia de un LLM es probabilística, puede variar y debe validarse antes de influir en el sistema.",
            block: "IA aplicada"),

        Topic(
            "crear-orden", "Crear una orden", "Laboratorio: crear una orden",
            "Integra C#, POO, validación, asincronía, pruebas, persistencia y HTTP en un flujo funcional.",
            "El cliente envía ProductCode, Quantity y UnitPrice. El servidor valida, genera Id, calcula Subtotal, persiste la orden y responde con un contrato HTTP preciso.",
            ["El servidor controla identidad y cálculos.", "EF Core conserva la orden en SQLite.", "Las pruebas confirman las reglas y el endpoint."],
            new("Solicitud válida", "POST → 201", "Se crea el recurso y se devuelve Location."),
            new("Solicitud inválida", "POST → 400", "ProblemDetails describe los campos incorrectos."),
            "Prueba primero el camino correcto y después los límites del contrato.",
            "CreateOrderService.cs",
            "var result = await service.CreateAsync(new CreateOrderRequest(\n    ProductCode: \"MONITOR-24\",\n    Quantity: 2,\n    UnitPrice: 3499.90m));\n\n// La orden queda persistida en SQLite.\n// Subtotal calculado por el servidor: 6999.80",
            "¿Qué dato NO debería aceptar el contrato de creación?",
            [
                new("ProductCode", false, "El cliente debe identificar qué producto desea ordenar."),
                new("Quantity", false, "La cantidad forma parte de la decisión del cliente."),
                new("Subtotal", true, "Correcto: el servidor debe calcularlo para proteger la regla.")
            ],
            "¿Qué falta antes de considerar esta API lista para producción?",
            "Idempotencia, concurrencia, resiliencia, observabilidad avanzada y seguridad completa; esos riesgos pertenecen a niveles posteriores.",
            block: "Laboratorio integrador")
    ];

    public static int IndexOf(string slug)
    {
        for (var index = 0; index < Topics.Count; index++)
        {
            if (Topics[index].Slug.Equals(slug, StringComparison.OrdinalIgnoreCase)) return index;
        }

        return -1;
    }

    private static LabTopic Topic(
        string slug,
        string shortTitle,
        string title,
        string summary,
        string definition,
        string[] keyIdeas,
        VisualIdea left,
        VisualIdea right,
        string decision,
        string codeFile,
        string code,
        string challengeQuestion,
        ChallengeOption[] challengeOptions,
        string interviewQuestion,
        string interviewAnswer,
        string block = "Fundamentos de C#") =>
        new(slug, shortTitle, title, "Junior", block, summary, definition, keyIdeas, left, right, decision, codeFile, code,
            challengeQuestion, challengeOptions, interviewQuestion, interviewAnswer);
}
