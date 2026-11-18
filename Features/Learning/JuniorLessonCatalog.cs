using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public sealed record JuniorCodePane(string FileName, string Label, string Code, string CssClass = "");

public sealed record JuniorLessonStep(
    string Name,
    string Label,
    string Caption,
    JuniorCodePane Left,
    JuniorCodePane? Right,
    string HintTitle,
    string HintBody,
    string HintExample);

public sealed record JuniorModuleLesson(string Slug, JuniorLessonStep[] Steps);

public static partial class JuniorLessonCatalog
{
    public static IReadOnlyDictionary<string, JuniorModuleLesson> Modules { get; } =
        new Dictionary<string, JuniorModuleLesson>(StringComparer.OrdinalIgnoreCase)
        {
            ["fundamentos-csharp"] = Module("fundamentos-csharp",
                Step("Entiende", "Entiende", "Tipos básicos dentro de una orden",
                    Pane("OrderData.cs", "Tipos y operadores", """
                        // string representa texto. Sin ? es un dato requerido.
                        string productCode = "MONITOR-24";

                        // int representa números enteros: 1, 2, 3...
                        int quantity = 2;

                        // decimal conserva la precisión necesaria para dinero.
                        decimal unitPrice = 3499.90m;

                        // El operador * combina los valores y produce un resultado.
                        decimal subtotal = quantity * unitPrice;
                        """), null,
                    "El tipo funciona como un contrato", "Antes de pensar en sintaxis, pregunta qué representa el dato, si puede faltar y qué operaciones admite.", "tipo nombre = valor;"),
                Step("Compara", "Compara", "El contrato incorrecto y el contrato explícito",
                    Bad("OrderData.cs", "Mala práctica", """
                        // ❌ El tipo promete texto, pero recibe ausencia.
                        string productCode = null;

                        // "" tampoco explica si el dato era opcional.
                        string notes = "";
                        """),
                    Good("OrderData.cs", "Buena práctica", """
                        // ✅ El código de producto es obligatorio.
                        string productCode = "MONITOR-24";

                        // ✅ El signo ? declara que notes puede no existir.
                        string? notes = null;
                        """),
                    "null y texto vacío no significan lo mismo", "Usa ? únicamente cuando la ausencia sea un estado válido del dominio. No lo agregues solo para silenciar advertencias.", "string requerido; | string? opcional;"),
                Step("Condiciones", "Condiciones", "Dos formas de elegir qué camino ejecutar",
                    Pane("IfElseRules.cs", "if / else", """
                        // if evalúa la primera condición.
                        if (quantity <= 0)
                        {
                            return "Cantidad inválida";
                        }
                        else if (quantity >= 10)
                        {
                            return "Aplicar descuento";
                        }
                        else
                        {
                            return "Procesar orden";
                        }
                        """),
                    Pane("SwitchRules.cs", "switch", """
                        // switch compara un valor contra casos conocidos.
                        switch (status)
                        {
                            case OrderStatus.Draft:
                                return "La orden puede editarse";
                            case OrderStatus.Confirmed:
                                return "La orden está confirmada";
                            default:
                                return "Estado no reconocido";
                        }
                        """),
                    "Elige la condición que mejor expresa la decisión", "Usa if cuando evalúas condiciones diferentes. Usa switch cuando comparas un mismo valor contra un conjunto claro de casos.", "if / else | switch"),
                Step("Ciclos", "Ciclos", "Recorrer elementos o repetir mientras exista una condición",
                    Pane("ForeachExample.cs", "foreach", """
                        // foreach recorre cada elemento sin usar un índice.
                        decimal total = 0m;

                        foreach (OrderItem item in orderItems)
                        {
                            total += item.Quantity * item.UnitPrice;
                        }
                        """),
                    Pane("ForWhileExample.cs", "for / while", """
                        // for resulta útil cuando necesitas el índice.
                        for (int index = 0; index < orderItems.Count; index++)
                        {
                            Console.WriteLine(orderItems[index]);
                        }

                        // while repite mientras la condición sea verdadera.
                        while (pendingOrders.Count > 0)
                        {
                            Order nextOrder = pendingOrders.Dequeue();
                            Process(nextOrder);
                        }
                        """),
                    "Elige el ciclo según cómo necesitas recorrer", "foreach prioriza claridad; for ofrece un índice; while repite mientras una condición permanezca verdadera.", "foreach | for | while"),
                Step("Integra", "Integra", "Compara todo lo revisado en el módulo",
                    Bad("OrderProcessor.Before.cs", "Código deficiente", """
                        // ❌ No expresa qué datos pueden faltar.
                        string productCode = null;
                        string notes = "";
                        int quantity = 0;
                        decimal unitPrice = 3499.90m;

                        // ❌ Calcula antes de validar.
                        decimal subtotal = quantity * unitPrice;

                        // ❌ Repite lógica sin explicar la intención.
                        for (int index = 0; index < orderItems.Count; index++)
                        {
                            subtotal += orderItems[index].UnitPrice;
                        }

                        if (quantity <= 0)
                        {
                            return "Cantidad inválida";
                        }
                        """),
                    Good("OrderProcessor.After.cs", "Código correcto", """
                        // ✅ El contrato distingue datos requeridos y opcionales.
                        string productCode = "MONITOR-24";
                        string? notes = null;
                        int quantity = 2;
                        decimal unitPrice = 3499.90m;

                        // ✅ La condición protege el flujo antes de calcular.
                        if (quantity <= 0)
                        {
                            return "Cantidad inválida";
                        }

                        // ✅ foreach comunica que recorremos todos los elementos.
                        decimal subtotal = 0m;

                        foreach (OrderItem item in orderItems)
                        {
                            subtotal += item.Quantity * item.UnitPrice;
                        }

                        return $"Orden {productCode}: {subtotal:C}";
                        """),
                    "El orden de las decisiones también importa", "Primero declara un contrato claro, después valida y finalmente calcula o recorre los elementos.", "contrato → validación → recorrido → resultado")),

            ["metodos-y-clases"] = Module("metodos-y-clases",
                Step("Entiende", "Entiende", "Una clase reúne estado y comportamiento",
                    Pane("Order.cs", "Clase", """
                        // Una clase reúne datos y operaciones relacionadas.
                        public sealed class Order
                        {
                            // La propiedad expone el dato; private set protege sus cambios.
                            public string ProductCode { get; }
                            public decimal Total { get; private set; }

                            // El constructor recibe lo necesario para crear la instancia.
                            public Order(string productCode)
                            {
                                ProductCode = productCode;
                            }

                            // El método expresa una operación válida de la orden.
                            public void Recalculate(int quantity, decimal unitPrice)
                            {
                                Total = quantity * unitPrice;
                            }
                        }
                        """), null,
                    "Una clase define qué puede hacer una instancia", "Las propiedades describen el estado y los métodos describen operaciones válidas sobre ese estado.", "clase = estado + comportamiento"),
                Step("Compara", "Compara", "Estado expuesto frente a estado protegido",
                    Bad("Order.Before.cs", "Mala práctica", """
                        // ❌ Cualquier consumidor puede dejar la orden inconsistente.
                        public sealed class Order
                        {
                            public string ProductCode { get; set; } = "";
                            public decimal Total { get; set; }
                        }

                        var order = new Order();
                        order.Total = -500m;
                        """),
                    Good("Order.After.cs", "Buena práctica", """
                        // ✅ La clase controla cómo nace y cómo cambia su estado.
                        public sealed class Order
                        {
                            public string ProductCode { get; }
                            public decimal Total { get; private set; }

                            public Order(string productCode)
                            {
                                ProductCode = productCode;
                            }
                        }
                        """),
                    "Encapsular no significa esconderlo todo", "Permite consultar lo necesario, pero limita los cambios para que la clase no acepte estados imposibles.", "get público | cambio controlado"),
                Step("Constructores", "Constructores", "Crear objetos válidos desde el primer momento",
                    Bad("Order.Empty.cs", "Sin garantías", """
                        // ❌ El objeto existe antes de tener los datos obligatorios.
                        public sealed class Order
                        {
                            public string ProductCode { get; set; } = "";
                        }

                        var order = new Order();
                        order.ProductCode = "MONITOR-24";
                        """),
                    Good("Order.Valid.cs", "Contrato explícito", """
                        // ✅ El constructor exige el dato para crear una orden válida.
                        public sealed class Order
                        {
                            public string ProductCode { get; }

                            public Order(string productCode)
                            {
                                ProductCode = productCode;
                            }
                        }

                        var order = new Order("MONITOR-24");
                        """),
                    "El constructor protege el punto de entrada", "Si un dato es obligatorio, recíbelo en el constructor en lugar de esperar que alguien lo asigne después.", "new Order(productCode)"),
                Step("Métodos", "Métodos", "void ejecuta una acción; return entrega un resultado",
                    Pane("OrderActions.cs", "void", """
                        // void comunica que el método no entrega un valor.
                        // Su efecto observable es el cambio de estado.
                        public void Confirm()
                        {
                            Status = OrderStatus.Confirmed;
                        }

                        order.Confirm();
                        """),
                    Pane("OrderCalculations.cs", "return", """
                        // decimal comunica qué resultado recibe el llamador.
                        public decimal CalculateSubtotal(int quantity, decimal unitPrice)
                        {
                            return quantity * unitPrice;
                        }

                        decimal subtotal = order.CalculateSubtotal(2, 3499.90m);
                        """),
                    "La firma del método comunica su resultado", "void no devuelve un valor. Un tipo como decimal declara que el llamador recibirá un resultado de ese tipo.", "void Confirm() | decimal Calculate()"),
                Step("Integra", "Integra", "Compara una clase anémica con una clase que protege sus reglas",
                    Bad("Order.Before.cs", "Código deficiente", """
                        // ❌ Solo almacena datos y permite cualquier modificación.
                        public sealed class Order
                        {
                            public string ProductCode { get; set; } = "";
                            public int Quantity { get; set; }
                            public decimal UnitPrice { get; set; }
                            public decimal Total { get; set; }
                            public string Status { get; set; } = "";
                        }

                        var order = new Order();
                        order.Quantity = -2;
                        order.Total = -6999.80m;
                        order.Status = "cualquier texto";
                        """),
                    Good("Order.After.cs", "Código correcto", """
                        // ✅ Constructor, propiedades y métodos forman un contrato claro.
                        public sealed class Order
                        {
                            public string ProductCode { get; }
                            public decimal Total { get; private set; }
                            public OrderStatus Status { get; private set; }

                            public Order(string productCode)
                            {
                                ProductCode = productCode;
                                Status = OrderStatus.Draft;
                            }

                            public void Recalculate(int quantity, decimal unitPrice)
                            {
                                if (quantity <= 0) return;
                                Total = quantity * unitPrice;
                            }

                            public void Confirm()
                            {
                                Status = OrderStatus.Confirmed;
                            }
                        }
                        """),
                    "El objeto debe proteger sus propias reglas", "Una clase útil no es solo una bolsa de propiedades: conserva juntas las reglas y los datos que protege.", "constructor → propiedades → métodos → estado válido")),

            ["modelos-y-memoria"] = Module("modelos-y-memoria",
                Step("Entiende", "Modelos", "Identidad, valor y estados permitidos",
                    Pane("OrderModels.cs", "class / record / enum", """
                        // class modela una entidad con identidad y ciclo de vida.
                        public sealed class Order
                        {
                            public Guid Id { get; init; }
                            public OrderStatus Status { get; private set; }
                        }

                        // record compara sus datos por valor.
                        public sealed record Money(decimal Amount, string Currency);

                        // enum limita los estados permitidos.
                        public enum OrderStatus
                        {
                            Draft,
                            Confirmed,
                            Cancelled
                        }
                        """), null,
                    "El tipo comunica la intención", "class, record, struct y enum no son intercambiables: cada uno expresa una semántica distinta.", "identidad | valor | conjunto cerrado"),
                Step("Compara", "Memoria", "Asignar una referencia no copia el objeto",
                    Bad("ReferenceCopy.cs", "Referencia compartida", """
                        var firstOrder = new Order();
                        var secondOrder = firstOrder;

                        // Ambas variables apuntan a la misma instancia.
                        secondOrder.Status = OrderStatus.Confirmed;

                        // firstOrder también observa Confirmed.
                        """),
                    Good("ValueCopy.cs", "Valor copiado", """
                        var firstMoney = new Money(100m, "MXN");
                        var secondMoney = firstMoney with { Amount = 200m };

                        // firstMoney conserva 100; secondMoney contiene 200.
                        """),
                    "Referencia y valor se comportan distinto", "Una class suele compartir la misma instancia; un record permite crear otro valor mediante with.", "class → referencia | record → valor"),
                Step("Contratos", "class, struct e interface", "Distinguir identidad, valor y capacidad",
                    Pane("OrderTypes.cs", "class / struct", """
                        // class: tipo por referencia con identidad y ciclo de vida.
                        public sealed class Order
                        {
                            public Guid Id { get; init; }
                            public OrderStatus Status { get; private set; }
                        }

                        // struct: valor pequeño; se copia al asignarlo.
                        public readonly struct Coordinates
                        {
                            public decimal Latitude { get; init; }
                            public decimal Longitude { get; init; }
                        }
                        """),
                    Pane("INotifier.cs", "interface", """
                        // interface: contrato sin decidir la implementación.
                        public interface INotifier
                        {
                            Task SendAsync(Order order);
                        }

                        // Una class o un struct puede implementar el contrato.
                        public sealed class EmailNotifier : INotifier
                        {
                            public Task SendAsync(Order order)
                                => emailClient.SendAsync(order.Id);
                        }
                        """),
                    "Son tipos con propósitos diferentes", "class representa identidad y comportamiento; struct representa un valor pequeño; interface declara una capacidad que otros tipos implementan.", "class → identidad | struct → valor | interface → contrato"),
                Step("Estados", "record y enum", "Evitar strings arbitrarios en el dominio",
                    Bad("OrderStrings.cs", "String libre", """
                        // Cualquier texto compila, incluso uno inválido.
                        string status = "confirmda";
                        string currency = "pesos";
                        """),
                    Good("OrderTypes.cs", "Tipos explícitos", """
                        OrderStatus status = OrderStatus.Confirmed;
                        Money total = new(3499.90m, "MXN");
                        """),
                    "Los tipos reducen estados inválidos", "enum limita opciones conocidas y record agrupa datos que forman un solo valor.", "string libre → error | tipo explícito → contrato"),
                Step("Integra", "Mapa de tipos", "Elegir cada tipo por el contrato que comunica",
                    Bad("OrderModels.Before.cs", "Código deficiente", """
                        public sealed class OrderData
                        {
                            public string Id { get; set; } = "";
                            public string Status { get; set; } = "";
                            public decimal Amount { get; set; }
                            public string Currency { get; set; } = "";
                        }
                        """),
                    Good("OrderModels.After.cs", "Código correcto", """
                        public enum OrderStatus { Draft, Confirmed, Cancelled }

                        public sealed record Money(decimal Amount, string Currency);

                        public readonly struct Coordinates
                        {
                            public decimal Latitude { get; init; }
                            public decimal Longitude { get; init; }
                        }

                        public interface IOrderNotifier
                        {
                            Task SendAsync(Order order);
                        }

                        public sealed class Order
                        {
                            public Guid Id { get; init; }
                            public OrderStatus Status { get; private set; }
                            public Money Total { get; private set; } = new(0m, "MXN");
                        }
                        """),
                    "Modela antes de optimizar", "Entidad, valor, estado cerrado y capacidad necesitan tipos diferentes aunque todos participen en el mismo flujo.", "class + struct + interface + record + enum")),

            ["poo"] = Module("poo",
                Step("Entiende", "POO", "Estado y comportamiento colaboran dentro de objetos",
                    Pane("Order.cs", "Encapsulación", """
                        public sealed class Order
                        {
                            public OrderStatus Status { get; private set; }

                            public void Confirm()
                            {
                                if (Status != OrderStatus.Draft) return;
                                Status = OrderStatus.Confirmed;
                            }
                        }
                        """), null,
                    "Encapsular mantiene juntas las reglas", "El objeto controla su estado y solo expone operaciones válidas para modificarlo.", "estado privado + operación pública"),
                Step("Compara", "Diseño", "Herencia forzada frente a composición",
                    Bad("EmailOrderService.cs", "Herencia rígida", """
                        public class OrderService { }

                        public sealed class EmailOrderService : OrderService
                        {
                            public void SendEmail() { }
                        }
                        """),
                    Good("OrderService.cs", "Composición", """
                        public sealed class OrderService(INotifier notifier)
                        {
                            public Task NotifyAsync(Order order)
                            {
                                return notifier.SendAsync(order);
                            }
                        }
                        """),
                    "Prefiere relaciones que expresen la realidad", "Una notificación es una capacidad colaboradora; no convierte al servicio en otro tipo de servicio.", "tiene un → composición | es un → herencia"),
                Step("Pilares", "Abstracción", "Exponer la capacidad sin revelar la implementación",
                    Pane("INotifier.cs", "Contrato", """
                        public interface INotifier
                        {
                            Task SendAsync(Order order);
                        }
                        """),
                    Pane("EmailNotifier.cs", "Implementación", """
                        public sealed class EmailNotifier : INotifier
                        {
                            public Task SendAsync(Order order)
                            {
                                return emailClient.SendAsync(order.Id);
                            }
                        }
                        """),
                    "Abstraer es mostrar lo esencial", "El consumidor conoce la capacidad SendAsync, no los detalles del proveedor de correo.", "interface → qué | clase → cómo"),
                Step("Polimorfismo", "Sustitución", "Mismo contrato, comportamientos diferentes",
                    Pane("EmailNotifier.cs", "Correo", """
                        INotifier notifier = new EmailNotifier(emailClient);
                        await notifier.SendAsync(order);
                        """),
                    Pane("SmsNotifier.cs", "SMS", """
                        INotifier notifier = new SmsNotifier(smsClient);
                        await notifier.SendAsync(order);
                        """),
                    "El consumidor usa el contrato", "El servicio puede recibir correo o SMS sin cambiar la operación que invoca.", "INotifier → Email | SMS"),
                Step("Integra", "POO aplicada", "Usar los cuatro pilares dentro de una colaboración",
                    Bad("OrderService.Before.cs", "Código deficiente", """
                        // Estado expuesto y responsabilidades mezcladas.
                        public sealed class Order
                        {
                            public string Status { get; set; } = "draft";
                        }

                        public sealed class OrderService
                        {
                            public async Task ProcessAsync(
                                Order order,
                                string channel)
                            {
                                order.Status = "confirmed";

                                if (channel == "email") await SendEmail(order);
                                if (channel == "sms") await SendSms(order);
                            }
                        }
                        """),
                    Good("OrderService.After.cs", "Código correcto", """
                        // ABSTRACCIÓN: expone una capacidad.
                        public interface INotifier
                        {
                            Task SendAsync(Order order);
                        }

                        // ENCAPSULACIÓN: la orden protege su estado.
                        public sealed class Order
                        {
                            public OrderStatus Status { get; private set; }

                            public void Confirm()
                            {
                                if (Status != OrderStatus.Draft) return;
                                Status = OrderStatus.Confirmed;
                            }
                        }

                        // POLIMORFISMO: EmailNotifier o SmsNotifier
                        // pueden ocupar el lugar de INotifier.

                        // COMPOSICIÓN: el servicio recibe la capacidad.
                        public sealed class OrderService(INotifier notifier)
                        {
                            public Task ProcessAsync(Order order)
                            {
                                order.Confirm();
                                return notifier.SendAsync(order);
                            }
                        }
                        """),
                    "POO protege reglas y permite sustituir conductas", "Los pilares no son definiciones aisladas: encapsulan el estado, abstraen capacidades y permiten polimorfismo mediante composición.", "Order + INotifier + implementación")),

            ["errores-y-validacion"] = Module("errores-y-validacion",
                Step("Entiende", "Errores", "Distinguir un dato inválido de un fallo inesperado",
                    Pane("CreateOrderService.cs", "Resultado esperado", """
                        if (request.Quantity <= 0)
                        {
                            return CreateOrderResult.Invalid(
                                "Quantity debe ser mayor que cero.");
                        }

                        return CreateOrderResult.Success(order);
                        """), null,
                    "Un error esperado debe poder corregirse", "La validación comunica al cliente qué dato debe cambiar sin lanzar una excepción inesperada.", "dato inválido → resultado de validación"),
                Step("Compara", "Clasificación", "Validación corregible frente a excepción",
                    Pane("Validation.cs", "Esperado", """
                        if (request.ProductCode == "")
                        {
                            return Result.Invalid("ProductCode es requerido");
                        }
                        """),
                    Pane("Repository.cs", "Inesperado", """
                        try
                        {
                            await repository.SaveAsync(order);
                        }
                        catch (DatabaseException exception)
                        {
                            logger.LogError(exception, "No se guardó la orden");
                            throw;
                        }
                        """),
                    "No uses excepciones para decisiones normales", "Una cantidad inválida es previsible; una dependencia caída impide continuar de forma excepcional.", "validación ≠ excepción"),
                Step("Validación", "Guard clauses", "Detener temprano antes de modificar estado",
                    Bad("OrderFlow.Before.cs", "Validación tardía", """
                        order.Total = request.Quantity * request.UnitPrice;
                        await repository.SaveAsync(order);

                        if (request.Quantity <= 0)
                        {
                            return Result.Invalid("Quantity inválida");
                        }
                        """),
                    Good("OrderFlow.After.cs", "Validación temprana", """
                        if (request.Quantity <= 0)
                        {
                            return Result.Invalid("Quantity inválida");
                        }

                        order.Recalculate(request.Quantity, request.UnitPrice);
                        await repository.SaveAsync(order);
                        """),
                    "Protege el flujo antes de actuar", "Una guard clause evita cálculos, escrituras o llamadas externas con datos inválidos.", "validar → calcular → persistir"),
                Step("Excepciones", "catch", "Capturar solo cuando puedes aportar valor",
                    Bad("Exceptions.Before.cs", "Error oculto", """
                        try
                        {
                            await repository.SaveAsync(order);
                        }
                        catch
                        {
                            // El fallo desaparece y el sistema continúa.
                        }
                        """),
                    Good("Exceptions.After.cs", "Contexto útil", """
                        try
                        {
                            await repository.SaveAsync(order);
                        }
                        catch (DatabaseException exception)
                        {
                            logger.LogError(exception, "Order {Id} falló", order.Id);
                            throw;
                        }
                        """),
                    "Un catch vacío destruye evidencia", "Captura para recuperar, traducir o agregar contexto; si no puedes hacerlo, deja que el fallo se propague.", "catch → recuperar | traducir | registrar"),
                Step("Integra", "Respuesta", "Comunicar errores de forma consistente",
                    Bad("OrderEndpoint.Before.cs", "Código deficiente", """
                        app.MapPost("/api/orders", async request =>
                        {
                            try { return await service.CreateAsync(request); }
                            catch { return "Algo salió mal"; }
                        });
                        """),
                    Good("OrderEndpoint.After.cs", "Código correcto", """
                        app.MapPost("/api/orders", async request =>
                        {
                            var result = await service.CreateAsync(request);

                            return result.IsSuccess
                                ? Results.Created($"/api/orders/{result.Order!.Id}", result.Order)
                                : Results.ValidationProblem(result.Errors);
                        });
                        """),
                    "El contrato de error también es parte de la API", "Los errores esperados necesitan estructura; los inesperados deben conservar diagnóstico y contexto.", "Result + ProblemDetails")),

            ["colecciones-y-genericos"] = Module("colecciones-y-genericos",
                Step("Entiende", "Colecciones", "Elegir según la operación más frecuente",
                    Pane("OrderCollections.cs", "List / Dictionary / HashSet", """
                        var orders = new List<Order>();
                        var ordersById = new Dictionary<Guid, Order>();
                        var productCodes = new HashSet<string>();

                        orders.Add(order);
                        ordersById[order.Id] = order;
                        productCodes.Add(order.ProductCode);
                        """), null,
                    "La colección expresa cómo usarás los datos", "List conserva una secuencia; Dictionary busca por clave; HashSet mantiene valores únicos.", "secuencia | clave | unicidad"),
                Step("Compara", "List vs LinkedList", "La estructura especializada necesita un motivo real",
                    Pane("ListExample.cs", "List<T>", """
                        var orders = new List<Order>();
                        orders.Add(order);

                        Order first = orders[0];

                        foreach (Order current in orders)
                        {
                            Console.WriteLine(current.Id);
                        }
                        """),
                    Pane("LinkedListExample.cs", "LinkedList<T>", """
                        var queue = new LinkedList<Order>();
                        LinkedListNode<Order> node = queue.AddLast(order);

                        // Resulta útil cuando ya conservas el nodo.
                        queue.AddAfter(node, nextOrder);
                        queue.Remove(node);
                        """),
                    "List es la opción general", "LinkedList solo destaca si ya tienes nodos y realizas inserciones o eliminaciones locales frecuentes.", "List por defecto | LinkedList con nodos"),
                Step("Operaciones", "Acceso", "Secuencia, clave y unicidad resuelven problemas distintos",
                    Pane("Sequence.cs", "Recorrer", """
                        foreach (Order order in orders)
                        {
                            Process(order);
                        }
                        """),
                    Pane("Lookup.cs", "Buscar", """
                        if (ordersById.TryGetValue(orderId, out Order? order))
                        {
                            Process(order);
                        }
                        """),
                    "La intención debe verse en la operación", "Recorrer todos los elementos y localizar uno por Id no son el mismo patrón de acceso.", "foreach → secuencia | key → Dictionary"),
                Step("Genéricos", "Reutilización", "Un algoritmo puede conservar seguridad de tipos",
                    Pane("Repository.cs", "Tipo genérico", """
                        public sealed class Repository<T>
                        {
                            private readonly List<T> items = [];

                            public void Add(T item)
                            {
                                items.Add(item);
                            }
                        }
                        """),
                    Pane("Usage.cs", "Tipos concretos", """
                        var orderRepository = new Repository<Order>();
                        orderRepository.Add(order);

                        var codeRepository = new Repository<string>();
                        codeRepository.Add("MONITOR-24");
                        """),
                    "T representa un tipo que se decide al usarlo", "Los genéricos reutilizan estructura sin convertir todo a object ni perder comprobación del compilador.", "Repository<T>"),
                Step("Integra", "Mapa de colecciones", "Resolver secuencia, búsqueda y unicidad sin una colección universal",
                    Bad("OrderStore.Before.cs", "Código deficiente", """
                        // Una LinkedList no mejora una búsqueda por Id.
                        var orders = new LinkedList<Order>();

                        Order? found = null;
                        foreach (Order order in orders)
                        {
                            if (order.Id == orderId) found = order;
                        }
                        """),
                    Good("OrderStore.After.cs", "Código correcto", """
                        // List: secuencia general, recorrido y acceso por índice.
                        var orders = new List<Order>();

                        // Dictionary: acceso frecuente mediante una clave.
                        var ordersById = new Dictionary<Guid, Order>();

                        // HashSet: códigos únicos sin duplicados.
                        var productCodes = new HashSet<string>();

                        orders.Add(order);
                        ordersById[order.Id] = order;
                        productCodes.Add(order.ProductCode);

                        bool exists = ordersById.TryGetValue(orderId, out Order? found);

                        // LinkedList solo aporta valor si conservamos el nodo
                        // y hacemos inserciones o eliminaciones locales.
                        """),
                    "No existe una colección mejor para todo", "Array fija tamaño; List organiza secuencias; Dictionary busca por clave; HashSet garantiza unicidad; LinkedList requiere un patrón real de nodos.", "operación frecuente → colección")),

            ["linq"] = Module("linq",
                Step("Entiende", "LINQ", "Consultar secuencias con operaciones encadenadas",
                    Pane("OrderQueries.cs", "Where / Select", """
                        var pendingTotals = orders
                            .Where(order => order.Status == OrderStatus.Pending)
                            .Select(order => order.Subtotal)
                            .ToList();

                        bool hasLargeOrder = orders.Any(
                            order => order.Subtotal > 10_000m);
                        """), null,
                    "LINQ describe la forma del resultado", "Where filtra, Select transforma, Any comprueba existencia y ToList materializa.", "fuente → filtro → transformación → resultado"),
                Step("Compara", "Estilo", "Recorrido imperativo frente a consulta declarativa",
                    Pane("Orders.Before.cs", "foreach", """
                        var totals = new List<decimal>();

                        foreach (Order order in orders)
                        {
                            if (order.Status == OrderStatus.Pending)
                            {
                                totals.Add(order.Subtotal);
                            }
                        }
                        """),
                    Pane("Orders.After.cs", "LINQ", """
                        var totals = orders
                            .Where(order => order.Status == OrderStatus.Pending)
                            .Select(order => order.Subtotal)
                            .ToList();
                        """),
                    "Elige la forma más legible", "foreach muestra cada paso; LINQ destaca la forma del resultado. Ninguno es automáticamente mejor en todos los casos.", "cómo recorrer | qué resultado"),
                Step("Lambdas", "Funciones", "Expresar una operación breve como argumento",
                    Pane("Filter.cs", "Predicado", """
                        // Recibe Order y devuelve bool.
                        Func<Order, bool> isPending =
                            order => order.Status == OrderStatus.Pending;

                        var pending = orders.Where(isPending);
                        """),
                    Pane("Projection.cs", "Transformación", """
                        // Recibe Order y devuelve decimal.
                        Func<Order, decimal> selectTotal =
                            order => order.Subtotal;

                        var totals = orders.Select(selectTotal);
                        """),
                    "La firma de la lambda depende del operador", "Where necesita una decisión booleana; Select necesita el valor que producirá por elemento.", "entrada => resultado"),
                Step("Ejecución", "Diferida", "La consulta se ejecuta al enumerarse",
                    Bad("Deferred.cs", "Consulta viva", """
                        var pending = orders.Where(order => order.IsPending);

                        orders.Add(new Order { IsPending = true });

                        // La nueva orden aparece al enumerar pending.
                        foreach (Order order in pending) { }
                        """),
                    Good("Materialized.cs", "Resultado fijado", """
                        var pending = orders
                            .Where(order => order.IsPending)
                            .ToList();

                        orders.Add(new Order { IsPending = true });

                        // pending conserva la fotografía anterior.
                        """),
                    "ToList marca el momento de ejecución", "Sin materializar, muchas consultas vuelven a leer la fuente cada vez que se enumeran.", "IEnumerable → diferido | ToList → ahora"),
                Step("Integra", "Consulta", "Evitar enumeraciones y transformaciones innecesarias",
                    Bad("OrderQueries.Before.cs", "Código deficiente", """
                        var pending = orders.Where(order => order.IsPending);

                        if (pending.Count() > 0)
                        {
                            decimal total = pending.Sum(order => order.Subtotal);
                            Console.WriteLine(pending.Count());
                        }
                        """),
                    Good("OrderQueries.After.cs", "Código correcto", """
                        var pending = orders
                            .Where(order => order.IsPending)
                            .ToList();

                        if (pending.Any())
                        {
                            decimal total = pending.Sum(order => order.Subtotal);
                            Console.WriteLine(pending.Count);
                        }
                        """),
                    "Materializa cuando reutilizas el mismo resultado", "Evita enumerar repetidamente una consulta costosa o una fuente que puede cambiar.", "Where → ToList → reutilizar")),

            ["task-async-await"] = Module("task-async-await",
                Step("Entiende", "Task", "Representar una operación que termina en el futuro",
                    Pane("OrderService.cs", "Task / Task<T>", """
                        // Task expone finalización y errores.
                        public Task SaveAsync(Order order)
                        {
                            return repository.SaveAsync(order);
                        }

                        // Task<T> además entrega un resultado.
                        public Task<Order?> GetAsync(Guid id)
                        {
                            return repository.GetAsync(id);
                        }
                        """), null,
                    "Task no significa hilo nuevo", "Task representa una operación; una espera de entrada o salida puede quedar pendiente sin ocupar un hilo.", "Task → finalización | Task<T> → resultado"),
                Step("Compara", "Firma", "async void frente a Task observable",
                    Bad("OrderEvents.cs", "async void", """
                        // El llamador no puede esperar ni observar errores.
                        public async void SaveOrder(Order order)
                        {
                            await repository.SaveAsync(order);
                        }
                        """),
                    Good("OrderService.cs", "Task", """
                        public async Task SaveOrderAsync(Order order)
                        {
                            await repository.SaveAsync(order);
                        }

                        await service.SaveOrderAsync(order);
                        """),
                    "Evita async void salvo eventos", "Task permite esperar la operación, propagar excepciones y componerla con otras tareas.", "async Task → operación observable"),
                Step("Await", "I/O", "Esperar sin bloquear el hilo",
                    Bad("Blocking.cs", "Bloqueo", """
                        Order order = repository
                            .GetAsync(orderId)
                            .Result;
                        """),
                    Good("Awaiting.cs", "await", """
                        Order? order = await repository
                            .GetAsync(orderId);

                        if (order is null) return;
                        """),
                    "await suspende la continuación", "No bloquees una operación asíncrona con Result o Wait; propaga async y await hasta el límite.", "async all the way"),
                Step("ValueTask", "Task vs ValueTask", "Elegir la opción simple antes de optimizar asignaciones",
                    Pane("OrderRepository.cs", "Task<T>", """
                        // Opción predeterminada: simple de consumir y combinar.
                        public Task<Order?> GetAsync(Guid id)
                        {
                            return database.FindOrderAsync(id);
                        }

                        Order? order = await repository.GetAsync(orderId);
                        """),
                    Pane("CachedOrderStore.cs", "ValueTask<T>", """
                        // Úsalo solo si medir demuestra que conviene y el
                        // resultado suele estar disponible sincrónicamente.
                        public ValueTask<Order?> GetCachedAsync(Guid id)
                        {
                            if (cache.TryGetValue(id, out Order? order))
                            {
                                return ValueTask.FromResult<Order?>(order);
                            }

                            return new(database.FindOrderAsync(id));
                        }
                        """),
                    "Task debe ser la elección habitual", "ValueTask es un struct que puede evitar una asignación en rutas muy frecuentes, pero agrega restricciones y complejidad. No lo uses sin medir.", "Task por defecto | ValueTask con evidencia"),
                Step("Integra", "Flujo async", "Conservar finalización, errores y cancelación",
                    Bad("OrderService.Before.cs", "Código deficiente", """
                        public async void Create(Order order)
                        {
                            Task.Run(() => repository.SaveAsync(order));
                        }
                        """),
                    Good("OrderService.After.cs", "Código correcto", """
                        public async Task CreateAsync(
                            Order order,
                            CancellationToken cancellationToken)
                        {
                            await repository.SaveAsync(
                                order,
                                cancellationToken);
                        }
                        """),
                    "No escondas la operación asíncrona", "Devuelve Task, usa await y propaga CancellationToken. ValueTask es una optimización puntual, no un reemplazo general.", "Task + await + CancellationToken")),

            ["pruebas-aaa"] = Module("pruebas-aaa",
                Step("Entiende", "AAA", "Preparar, ejecutar y verificar una conducta",
                    Pane("CreateOrderServiceTests.cs", "Arrange / Act / Assert", """
                        [Fact]
                        public async Task Create_WithZeroQuantity_ReturnsError()
                        {
                            // Arrange
                            var request = new CreateOrderRequest("MONITOR", 0, 50m);

                            // Act
                            CreateOrderResult result = await service.CreateAsync(request);

                            // Assert
                            Assert.Contains("Quantity", result.Errors);
                        }
                        """), null,
                    "AAA organiza el escenario", "Arrange prepara, Act ejecuta una sola conducta y Assert verifica el resultado observable.", "Arrange → Act → Assert"),
                Step("Compara", "Enfoque", "Probar comportamiento frente a detalles internos",
                    Bad("OrderTests.Before.cs", "Detalle privado", """
                        [Fact]
                        public void Confirm_CallsPrivateMethod()
                        {
                            // La prueba depende de cómo está implementada la clase.
                            Assert.True(CallPrivateValidation(order));
                        }
                        """),
                    Good("OrderTests.After.cs", "Resultado observable", """
                        [Fact]
                        public void Confirm_DraftOrder_ChangesStatus()
                        {
                            var order = new Order("MONITOR");

                            order.Confirm();

                            Assert.Equal(OrderStatus.Confirmed, order.Status);
                        }
                        """),
                    "La prueba debe sobrevivir una refactorización", "Comprueba lo que el consumidor puede observar, no los métodos privados usados para conseguirlo.", "entrada → conducta → resultado"),
                Step("Casos", "Límites", "Cubrir válido, inválido y frontera",
                    Pane("ValidCases.cs", "Casos válidos", """
                        [Theory]
                        [InlineData(1)]
                        [InlineData(10)]
                        public async Task Create_WithPositiveQuantity_Succeeds(int quantity)
                        {
                            CreateOrderResult result = await service.CreateAsync(Request(quantity));
                            Assert.True(result.IsSuccess);
                        }
                        """),
                    Pane("InvalidCases.cs", "Casos inválidos", """
                        [Theory]
                        [InlineData(0)]
                        [InlineData(-1)]
                        public async Task Create_WithNonPositiveQuantity_Fails(int quantity)
                        {
                            CreateOrderResult result = await service.CreateAsync(Request(quantity));
                            Assert.False(result.IsSuccess);
                        }
                        """),
                    "Los límites revelan errores frecuentes", "No pruebes solo el camino feliz: incluye cero, negativos y valores que cambian una regla.", "válido | inválido | frontera"),
                Step("Integración", "API real", "Comprobar que los componentes colaboran",
                    Pane("ServiceTest.cs", "Unitaria", """
                        [Fact]
                        public async Task Create_InvalidQuantity_ReturnsError()
                        {
                            var service = new CreateOrderService(fakeStore);
                            CreateOrderResult result = await service.CreateAsync(Request(0));
                            Assert.False(result.IsSuccess);
                        }
                        """),
                    Pane("ApiTest.cs", "Integración", """
                        [Fact]
                        public async Task PostOrder_ValidRequest_ReturnsCreated()
                        {
                            HttpResponseMessage response = await client.PostAsJsonAsync(
                                "/api/orders",
                                Request(2));

                            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                        }
                        """),
                    "El tipo de prueba depende del riesgo", "La unitaria enfoca una regla; la integración comprueba endpoint, serialización y colaboración real.", "unidad | integración"),
                Step("Integra", "Prueba útil", "Un nombre claro y una sola razón para fallar",
                    Bad("Tests.Before.cs", "Código deficiente", """
                        [Fact]
                        public async Task Test1()
                        {
                            Assert.NotNull(await service.CreateAsync(Request(2)));
                            Assert.NotNull(await service.CreateAsync(Request(0)));
                        }
                        """),
                    Good("Tests.After.cs", "Código correcto", """
                        [Fact]
                        public async Task Create_WithZeroQuantity_ReturnsQuantityError()
                        {
                            // Arrange
                            CreateOrderRequest request = Request(0);

                            // Act
                            CreateOrderResult result = await service.CreateAsync(request);

                            // Assert
                            Assert.Contains("Quantity", result.Errors);
                        }
                        """),
                    "La prueba explica la conducta", "Nombre, preparación, acción y aserción deben permitir entender el fallo sin depurar primero.", "escenario → acción → expectativa")),

            ["rest-http"] = Module("rest-http",
                Step("Entiende", "Contrato HTTP", "Ruta, verbo, DTO y respuesta forman la API",
                    Pane("OrderEndpoints.cs", "POST /api/orders", """
                        app.MapPost("/api/orders", async (CreateOrderRequest request) =>
                        {
                            CreateOrderResult result = await service.CreateAsync(request);

                            return result.IsSuccess
                                ? Results.Created(
                                    $"/api/orders/{result.Order!.Id}",
                                    result.Order)
                                : Results.ValidationProblem(result.Errors);
                        });
                        """), null,
                    "Una API es un contrato observable", "El cliente observa datos, rutas, verbos, códigos HTTP, encabezados y estructura de errores.", "request → endpoint → response"),
                Step("Compara", "DTO", "Contrato externo frente a entidad interna",
                    Bad("OrderEndpoint.Before.cs", "Entidad expuesta", """
                        app.MapPost("/api/orders", (Order order) =>
                        {
                            database.Orders.Add(order);
                            database.SaveChanges();
                            return order;
                        });
                        """),
                    Good("OrderEndpoint.After.cs", "DTO explícito", """
                        app.MapPost("/api/orders", async (CreateOrderRequest request) =>
                        {
                            CreateOrderResult result = await service.CreateAsync(request);
                            return Results.Created(
                                $"/api/orders/{result.Order!.Id}",
                                result.Order);
                        });
                        """),
                    "El cliente no debe controlar la entidad", "El DTO limita qué datos puede decidir el consumidor y evita acoplar el contrato a persistencia.", "DTO externo ≠ entidad interna"),
                Step("Métodos", "Métodos HTTP", "Expresar lectura, creación, reemplazo, cambio y eliminación",
                    Pane("OrderReads.http", "Consultar", """
                        // Consultar la colección.
                        GET /api/orders

                        // Consultar un recurso.
                        GET /api/orders/{id}

                        // HEAD consulta metadatos sin devolver el cuerpo.
                        HEAD /api/orders/{id}
                        """),
                    Pane("OrderWrites.http", "Modificar", """
                        // Crear un recurso.
                        POST /api/orders

                        // Reemplazar su representación completa.
                        PUT /api/orders/{id}

                        // Modificar solo algunos campos.
                        PATCH /api/orders/{id}

                        // Eliminar el recurso.
                        DELETE /api/orders/{id}
                        """),
                    "La ruta identifica; el método expresa la intención", "GET lee, POST crea, PUT reemplaza, PATCH modifica parcialmente y DELETE elimina. HEAD y OPTIONS son métodos complementarios.", "GET | POST | PUT | PATCH | DELETE"),
                Step("Respuestas", "Códigos HTTP", "Reconocer los resultados habituales de una API",
                    Pane("SuccessResults.cs", "2xx · éxito", """
                        // 200 OK: consulta o actualización con respuesta.
                        return Results.Ok(response);

                        // 201 Created: recurso creado y localizable.
                        return Results.Created(location, response);

                        // 202 Accepted: procesamiento iniciado.
                        return Results.Accepted(location);

                        // 204 No Content: operación exitosa sin cuerpo.
                        return Results.NoContent();
                        """),
                    Pane("ErrorResults.cs", "4xx / 5xx · error", """
                        return Results.BadRequest();           // 400 datos inválidos
                        return Results.Unauthorized();         // 401 sin autenticar
                        return Results.Forbid();               // 403 sin permiso
                        return Results.NotFound();             // 404 no existe
                        return Results.Conflict();             // 409 conflicto
                        return Results.UnprocessableEntity();  // 422 regla inválida
                        return Results.StatusCode(500);        // fallo interno
                        return Results.StatusCode(503);        // no disponible
                        """),
                    "El código HTTP resume el resultado", "El cliente debe distinguir éxito, autenticación, autorización, ausencia, conflicto, validación y fallos del servidor sin interpretar primero un texto libre.", "2xx éxito | 4xx cliente | 5xx servidor"),
                Step("Integra", "API de órdenes", "Combinar métodos, rutas, DTO y respuestas precisas",
                    Pane("OrderReadEndpoints.cs", "GET", """
                        app.MapGet("/api/orders", async () =>
                            Results.Ok(await store.GetAllAsync()));

                        app.MapGet("/api/orders/{id:guid}", async (Guid id) =>
                        {
                            Order? order = await store.FindAsync(id);

                            return order is null
                                ? Results.NotFound()
                                : Results.Ok(OrderResponse.From(order));
                        });
                        """),
                    Pane("OrderWriteEndpoints.cs", "POST / PUT / PATCH / DELETE", """
                        app.MapPost("/api/orders", (CreateOrderRequest request) =>
                            CreateOrder(request)); // 201 | 400 | 409

                        app.MapPut("/api/orders/{id:guid}",
                            (Guid id, ReplaceOrderRequest request) =>
                                ReplaceOrder(id, request)); // 200 | 400 | 404

                        app.MapPatch("/api/orders/{id:guid}",
                            (Guid id, UpdateOrderRequest request) =>
                                UpdateOrder(id, request)); // 200 | 400 | 404

                        app.MapDelete("/api/orders/{id:guid}",
                            (Guid id) => DeleteOrder(id)); // 204 | 404
                        """),
                    "El contrato completo se entiende sin abrir la base de datos", "Cada endpoint recibe un DTO, delega la regla y traduce el resultado usando el método y el código HTTP que mejor expresan lo ocurrido.", "ruta + método + DTO + status")),

            ["persistencia-ef-core"] = Module("persistencia-ef-core",
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
                    "Persistir no significa exponer la base de datos", "El dominio crea una orden válida; el store guarda; la consulta proyecta un DTO y el endpoint decide la respuesta HTTP.", "dominio → EF Core → DTO")),

            ["aspnet-core"] = Module("aspnet-core",
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
                    "Una API es más que sus endpoints", "El laboratorio es público: DI compone EF Core y el servicio, mientras el pipeline sirve la interfaz y publica los endpoints. Autenticación y autorización se agregan cuando exista identidad.", "DI + SQLite + middleware + endpoints")),

            ["ia-llm"] = Module("ia-llm",
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
                    "Integra mediante un contrato reemplazable", "Las pruebas usan un fake determinista. El laboratorio simula el proveedor y demuestra la frontera; una integración real se conecta desde configuración del servidor.", "LLM sugiere → código valida → persona revisa"))
        };

    public static JuniorModuleLesson Get(string slug) => Modules[slug];

    public static JuniorModuleLesson Get(string slug, AppLanguage language) =>
        language == AppLanguage.English && EnglishModules.TryGetValue(slug, out var module)
            ? module
            : Modules[slug];

    private static JuniorModuleLesson Module(string slug, params JuniorLessonStep[] steps) => new(slug, steps);

    private static JuniorLessonStep Step(string name, string label, string caption, JuniorCodePane left, JuniorCodePane? right, string hintTitle, string hintBody, string hintExample) =>
        new(name, label, caption, left, right, hintTitle, hintBody, hintExample);

    private static JuniorCodePane Pane(string fileName, string label, string code) => new(fileName, label, code);
    private static JuniorCodePane Bad(string fileName, string label, string code) => new(fileName, label, code, "bad-practice");
    private static JuniorCodePane Good(string fileName, string label, string code) => new(fileName, label, code, "good-practice");
}
