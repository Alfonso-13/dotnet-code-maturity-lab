namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    public static IReadOnlyDictionary<string, ModuleLesson> EnglishModules { get; } =
        new Dictionary<string, ModuleLesson>(StringComparer.OrdinalIgnoreCase)
        {
            ["fundamentos-csharp"] = Module("fundamentos-csharp",
                Step("Understand", "Understand", "Basic types inside an order",
                    Pane("OrderData.cs", "Types and operators", """
                        // string represents text. Without ?, the value is required.
                        string productCode = "MONITOR-24";

                        // int represents whole numbers: 1, 2, 3...
                        int quantity = 2;

                        // decimal preserves the precision needed for money.
                        decimal unitPrice = 3499.90m;

                        // The * operator combines values and produces a result.
                        decimal subtotal = quantity * unitPrice;
                        """), null,
                    "A type works as a contract", "Before thinking about syntax, ask what the value represents, whether it can be absent, and which operations it supports.", "type name = value;"),
                Step("Compare", "Compare", "The incorrect contract and the explicit contract",
                    Bad("OrderData.cs", "Bad practice", """
                        // ❌ The type promises text but receives absence.
                        string productCode = null;

                        // "" does not explain whether the value was optional either.
                        string notes = "";
                        """),
                    Good("OrderData.cs", "Good practice", """
                        // ✅ The product code is required.
                        string productCode = "MONITOR-24";

                        // ✅ The ? declares that notes may be absent.
                        string? notes = null;
                        """),
                    "null and empty text do not mean the same thing", "Use ? only when absence is a valid domain state. Do not add it merely to silence warnings.", "string required; | string? optional;"),
                Step("Conditions", "Conditions", "Two ways to choose which path to execute",
                    Pane("IfElseRules.cs", "if / else", """
                        // if evaluates the first condition.
                        if (quantity <= 0)
                        {
                            return "Invalid quantity";
                        }
                        else if (quantity >= 10)
                        {
                            return "Apply discount";
                        }
                        else
                        {
                            return "Process order";
                        }
                        """),
                    Pane("SwitchRules.cs", "switch", """
                        // switch compares one value against known cases.
                        switch (status)
                        {
                            case OrderStatus.Draft:
                                return "The order can be edited";
                            case OrderStatus.Confirmed:
                                return "The order is confirmed";
                            default:
                                return "Unknown status";
                        }
                        """),
                    "Choose the condition that best expresses the decision", "Use if when evaluating different conditions. Use switch when comparing one value against a clear set of cases.", "if / else | switch"),
                Step("Loops", "Loops", "Traverse elements or repeat while a condition holds",
                    Pane("ForeachExample.cs", "foreach", """
                        // foreach visits each element without using an index.
                        decimal total = 0m;

                        foreach (OrderItem item in orderItems)
                        {
                            total += item.Quantity * item.UnitPrice;
                        }
                        """),
                    Pane("ForWhileExample.cs", "for / while", """
                        // for is useful when you need the index.
                        for (int index = 0; index < orderItems.Count; index++)
                        {
                            Console.WriteLine(orderItems[index]);
                        }

                        // while repeats as long as the condition remains true.
                        while (pendingOrders.Count > 0)
                        {
                            Order nextOrder = pendingOrders.Dequeue();
                            Process(nextOrder);
                        }
                        """),
                    "Choose the loop according to how you need to traverse", "foreach favors clarity; for provides an index; while repeats while a condition remains true.", "foreach | for | while"),
                Step("Integrate", "Integrate", "Compare everything reviewed in the module",
                    Bad("OrderProcessor.Before.cs", "Deficient code", """
                        // ❌ It does not express which values may be absent.
                        string productCode = null;
                        string notes = "";
                        int quantity = 0;
                        decimal unitPrice = 3499.90m;

                        // ❌ It calculates before validating.
                        decimal subtotal = quantity * unitPrice;

                        // ❌ It repeats logic without expressing intent.
                        for (int index = 0; index < orderItems.Count; index++)
                        {
                            subtotal += orderItems[index].UnitPrice;
                        }

                        if (quantity <= 0)
                        {
                            return "Invalid quantity";
                        }
                        """),
                    Good("OrderProcessor.After.cs", "Correct code", """
                        // ✅ The contract distinguishes required and optional values.
                        string productCode = "MONITOR-24";
                        string? notes = null;
                        int quantity = 2;
                        decimal unitPrice = 3499.90m;

                        // ✅ The condition protects the flow before calculating.
                        if (quantity <= 0)
                        {
                            return "Invalid quantity";
                        }

                        // ✅ foreach communicates that every element is visited.
                        decimal subtotal = 0m;

                        foreach (OrderItem item in orderItems)
                        {
                            subtotal += item.Quantity * item.UnitPrice;
                        }

                        return $"Order {productCode}: {subtotal:C}";
                        """),
                    "The order of decisions matters too", "First declare a clear contract, then validate, and finally calculate or traverse the elements.", "contract → validation → traversal → result")),

            ["metodos-y-clases"] = Module("metodos-y-clases",
                Step("Understand", "Understand", "A class brings state and behavior together",
                    Pane("Order.cs", "Class", """
                        // A class brings related data and operations together.
                        public sealed class Order
                        {
                            // The property exposes data; private set protects changes.
                            public string ProductCode { get; }
                            public decimal Total { get; private set; }

                            // The constructor receives what the instance requires.
                            public Order(string productCode)
                            {
                                ProductCode = productCode;
                            }

                            // The method expresses a valid order operation.
                            public void Recalculate(int quantity, decimal unitPrice)
                            {
                                Total = quantity * unitPrice;
                            }
                        }
                        """), null,
                    "A class defines what an instance can do", "Properties describe state and methods describe valid operations over that state.", "class = state + behavior"),
                Step("Compare", "Compare", "Exposed state versus protected state",
                    Bad("Order.Before.cs", "Bad practice", """
                        // ❌ Any consumer can leave the order inconsistent.
                        public sealed class Order
                        {
                            public string ProductCode { get; set; } = "";
                            public decimal Total { get; set; }
                        }

                        var order = new Order();
                        order.Total = -500m;
                        """),
                    Good("Order.After.cs", "Good practice", """
                        // ✅ The class controls how its state starts and changes.
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
                    "Encapsulation does not mean hiding everything", "Allow consumers to read what they need, but limit changes so the class cannot accept impossible states.", "public get | controlled change"),
                Step("Constructors", "Constructors", "Create valid objects from the start",
                    Bad("Order.Empty.cs", "No guarantees", """
                        // ❌ The object exists before it has the required data.
                        public sealed class Order
                        {
                            public string ProductCode { get; set; } = "";
                        }

                        var order = new Order();
                        order.ProductCode = "MONITOR-24";
                        """),
                    Good("Order.Valid.cs", "Explicit contract", """
                        // ✅ The constructor requires the data for a valid order.
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
                    "The constructor protects the entry point", "If a value is required, receive it in the constructor instead of waiting for someone to assign it later.", "new Order(productCode)"),
                Step("Methods", "Methods", "void performs an action; return provides a result",
                    Pane("OrderActions.cs", "void", """
                        // void communicates that the method returns no value.
                        // Its observable effect is the state change.
                        public void Confirm()
                        {
                            Status = OrderStatus.Confirmed;
                        }

                        order.Confirm();
                        """),
                    Pane("OrderCalculations.cs", "return", """
                        // decimal communicates the result received by the caller.
                        public decimal CalculateSubtotal(int quantity, decimal unitPrice)
                        {
                            return quantity * unitPrice;
                        }

                        decimal subtotal = order.CalculateSubtotal(2, 3499.90m);
                        """),
                    "The method signature communicates its result", "void returns no value. A type such as decimal declares that the caller receives a value of that type.", "void Confirm() | decimal Calculate()"),
                Step("Integrate", "Integrate", "Compare an anemic class with one that protects its rules",
                    Bad("Order.Before.cs", "Deficient code", """
                        // ❌ It only stores data and allows any modification.
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
                        order.Status = "any text";
                        """),
                    Good("Order.After.cs", "Correct code", """
                        // ✅ Constructor, properties, and methods form a clear contract.
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
                    "The object should protect its own rules", "A useful class is not merely a bag of properties: it keeps together the rules and data it protects.", "constructor → properties → methods → valid state")),

            ["modelos-y-memoria"] = Module("modelos-y-memoria",
                Step("Understand", "Models", "Identity, value, and permitted states",
                    Pane("OrderModels.cs", "class / record / enum", """
                        // class models an entity with identity and a lifecycle.
                        public sealed class Order
                        {
                            public Guid Id { get; init; }
                            public OrderStatus Status { get; private set; }
                        }

                        // record compares its data by value.
                        public sealed record Money(decimal Amount, string Currency);

                        // enum limits the permitted states.
                        public enum OrderStatus
                        {
                            Draft,
                            Confirmed,
                            Cancelled
                        }
                        """), null,
                    "The type communicates intent", "class, record, struct, and enum are not interchangeable: each one expresses different semantics.", "identity | value | closed set"),
                Step("Compare", "Memory", "Assigning a reference does not copy the object",
                    Bad("ReferenceCopy.cs", "Shared reference", """
                        var firstOrder = new Order();
                        var secondOrder = firstOrder;

                        // Both variables point to the same instance.
                        secondOrder.Status = OrderStatus.Confirmed;

                        // firstOrder also observes Confirmed.
                        """),
                    Good("ValueCopy.cs", "Copied value", """
                        var firstMoney = new Money(100m, "MXN");
                        var secondMoney = firstMoney with { Amount = 200m };

                        // firstMoney keeps 100; secondMoney contains 200.
                        """),
                    "References and values behave differently", "A class usually shares the same instance; a record can create another value with with.", "class → reference | record → value"),
                Step("Contracts", "class, struct, and interface", "Distinguish identity, value, and capability",
                    Pane("OrderTypes.cs", "class / struct", """
                        // class: reference type with identity and a lifecycle.
                        public sealed class Order
                        {
                            public Guid Id { get; init; }
                            public OrderStatus Status { get; private set; }
                        }

                        // struct: small value; assignment copies it.
                        public readonly struct Coordinates
                        {
                            public decimal Latitude { get; init; }
                            public decimal Longitude { get; init; }
                        }
                        """),
                    Pane("INotifier.cs", "interface", """
                        // interface: a contract that does not choose the implementation.
                        public interface INotifier
                        {
                            Task SendAsync(Order order);
                        }

                        // A class or struct can implement the contract.
                        public sealed class EmailNotifier : INotifier
                        {
                            public Task SendAsync(Order order)
                                => emailClient.SendAsync(order.Id);
                        }
                        """),
                    "They are types with different purposes", "class represents identity and behavior; struct represents a small value; interface declares a capability implemented by other types.", "class → identity | struct → value | interface → contract"),
                Step("States", "record and enum", "Avoid arbitrary strings in the domain",
                    Bad("OrderStrings.cs", "Free-form string", """
                        // Any text compiles, including an invalid value.
                        string status = "confirmd";
                        string currency = "pesos";
                        """),
                    Good("OrderTypes.cs", "Explicit types", """
                        OrderStatus status = OrderStatus.Confirmed;
                        Money total = new(3499.90m, "MXN");
                        """),
                    "Types reduce invalid states", "enum limits known options and record groups data that forms one value.", "free-form string → error | explicit type → contract"),
                Step("Integrate", "Type map", "Choose each type for the contract it communicates",
                    Bad("OrderModels.Before.cs", "Deficient code", """
                        public sealed class OrderData
                        {
                            public string Id { get; set; } = "";
                            public string Status { get; set; } = "";
                            public decimal Amount { get; set; }
                            public string Currency { get; set; } = "";
                        }
                        """),
                    Good("OrderModels.After.cs", "Correct code", """
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
                    "Model before optimizing", "Entity, value, closed state, and capability need different types even when they participate in the same flow.", "class + struct + interface + record + enum")),

            ["poo"] = Module("poo",
                Step("Understand", "OOP", "State and behavior collaborate inside objects",
                    Pane("Order.cs", "Encapsulation", """
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
                    "Encapsulation keeps rules together", "The object controls its state and exposes only valid operations for changing it.", "private state + public operation"),
                Step("Compare", "Design", "Forced inheritance versus composition",
                    Bad("EmailOrderService.cs", "Rigid inheritance", """
                        public class OrderService { }

                        public sealed class EmailOrderService : OrderService
                        {
                            public void SendEmail() { }
                        }
                        """),
                    Good("OrderService.cs", "Composition", """
                        public sealed class OrderService(INotifier notifier)
                        {
                            public Task NotifyAsync(Order order)
                            {
                                return notifier.SendAsync(order);
                            }
                        }
                        """),
                    "Prefer relationships that express reality", "Notification is a collaborating capability; it does not turn the service into a different kind of service.", "has a → composition | is a → inheritance"),
                Step("Pillars", "Abstraction", "Expose the capability without revealing the implementation",
                    Pane("INotifier.cs", "Contract", """
                        public interface INotifier
                        {
                            Task SendAsync(Order order);
                        }
                        """),
                    Pane("EmailNotifier.cs", "Implementation", """
                        public sealed class EmailNotifier : INotifier
                        {
                            public Task SendAsync(Order order)
                            {
                                return emailClient.SendAsync(order.Id);
                            }
                        }
                        """),
                    "Abstraction shows what is essential", "The consumer knows the SendAsync capability, not the email provider details.", "interface → what | class → how"),
                Step("Polymorphism", "Substitution", "Same contract, different behaviors",
                    Pane("EmailNotifier.cs", "Email", """
                        INotifier notifier = new EmailNotifier(emailClient);
                        await notifier.SendAsync(order);
                        """),
                    Pane("SmsNotifier.cs", "SMS", """
                        INotifier notifier = new SmsNotifier(smsClient);
                        await notifier.SendAsync(order);
                        """),
                    "The consumer uses the contract", "The service can receive email or SMS without changing the operation it invokes.", "INotifier → Email | SMS"),
                Step("Integrate", "Applied OOP", "Use all four pillars within one collaboration",
                    Bad("OrderService.Before.cs", "Deficient code", """
                        // Exposed state and mixed responsibilities.
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
                    Good("OrderService.After.cs", "Correct code", """
                        // ABSTRACTION: exposes a capability.
                        public interface INotifier
                        {
                            Task SendAsync(Order order);
                        }

                        // ENCAPSULATION: the order protects its state.
                        public sealed class Order
                        {
                            public OrderStatus Status { get; private set; }

                            public void Confirm()
                            {
                                if (Status != OrderStatus.Draft) return;
                                Status = OrderStatus.Confirmed;
                            }
                        }

                        // POLYMORPHISM: EmailNotifier or SmsNotifier
                        // can take the place of INotifier.

                        // COMPOSITION: the service receives the capability.
                        public sealed class OrderService(INotifier notifier)
                        {
                            public Task ProcessAsync(Order order)
                            {
                                order.Confirm();
                                return notifier.SendAsync(order);
                            }
                        }
                        """),
                    "OOP protects rules and allows behavior substitution", "The pillars are not isolated definitions: they encapsulate state, abstract capabilities, and enable polymorphism through composition.", "Order + INotifier + implementation")),

            ["errores-y-validacion"] = Module("errores-y-validacion",
                Step("Understand", "Errors", "Separate invalid input from an unexpected failure",
                    Pane("CreateOrderService.cs", "Expected result", """
                        if (request.Quantity <= 0)
                        {
                            return CreateOrderResult.Invalid(
                                "Quantity must be greater than zero.");
                        }

                        return CreateOrderResult.Success(order);
                        """), null,
                    "An expected error must be actionable", "Validation tells the client what to change without throwing an unexpected exception.", "invalid input → validation result"),
                Step("Compare", "Classification", "Recoverable validation versus exception",
                    Pane("Validation.cs", "Expected", """
                        if (request.ProductCode == "")
                        {
                            return Result.Invalid("ProductCode is required");
                        }
                        """),
                    Pane("Repository.cs", "Unexpected", """
                        try
                        {
                            await repository.SaveAsync(order);
                        }
                        catch (DatabaseException exception)
                        {
                            logger.LogError(exception, "The order was not saved");
                            throw;
                        }
                        """),
                    "Do not use exceptions for normal decisions", "Invalid input is predictable; an unavailable dependency prevents the operation from continuing.", "validation ≠ exception"),
                Step("Validate", "Guard clauses", "Stop early before changing state",
                    Bad("OrderFlow.Before.cs", "Late validation", """
                        order.Total = request.Quantity * request.UnitPrice;
                        await repository.SaveAsync(order);

                        if (request.Quantity <= 0)
                        {
                            return Result.Invalid("Invalid quantity");
                        }
                        """),
                    Good("OrderFlow.After.cs", "Early validation", """
                        if (request.Quantity <= 0)
                        {
                            return Result.Invalid("Invalid quantity");
                        }

                        order.Recalculate(request.Quantity, request.UnitPrice);
                        await repository.SaveAsync(order);
                        """),
                    "Protect the flow before acting", "A guard clause prevents calculations, writes, or external calls with invalid data.", "validate → calculate → persist"),
                Step("Exceptions", "catch", "Catch only when you can add value",
                    Bad("Exceptions.Before.cs", "Hidden failure", """
                        try
                        {
                            await repository.SaveAsync(order);
                        }
                        catch
                        {
                            // The failure disappears and execution continues.
                        }
                        """),
                    Good("Exceptions.After.cs", "Useful context", """
                        try
                        {
                            await repository.SaveAsync(order);
                        }
                        catch (DatabaseException exception)
                        {
                            logger.LogError(exception, "Order {Id} failed", order.Id);
                            throw;
                        }
                        """),
                    "An empty catch destroys evidence", "Catch to recover, translate, or add context. Otherwise, let the failure propagate.", "catch → recover | translate | log"),
                Step("Integrate", "Response", "Return errors consistently",
                    Bad("OrderEndpoint.Before.cs", "Deficient code", """
                        app.MapPost("/api/orders", async request =>
                        {
                            try { return await service.CreateAsync(request); }
                            catch { return "Something went wrong"; }
                        });
                        """),
                    Good("OrderEndpoint.After.cs", "Correct code", """
                        app.MapPost("/api/orders", async request =>
                        {
                            var result = await service.CreateAsync(request);

                            return result.IsSuccess
                                ? Results.Created($"/api/orders/{result.Order!.Id}", result.Order)
                                : Results.ValidationProblem(result.Errors);
                        });
                        """),
                    "The error contract is part of the API", "Expected errors need structure; unexpected failures must preserve diagnostics and context.", "Result + ProblemDetails")),

            ["colecciones-y-genericos"] = Module("colecciones-y-genericos",
                Step("Understand", "Collections", "Choose by the operation you perform most often",
                    Pane("OrderCollections.cs", "List / Dictionary / HashSet", """
                        var orders = new List<Order>();
                        var ordersById = new Dictionary<Guid, Order>();
                        var productCodes = new HashSet<string>();

                        orders.Add(order);
                        ordersById[order.Id] = order;
                        productCodes.Add(order.ProductCode);
                        """), null,
                    "The collection communicates how data is used", "List keeps a sequence, Dictionary looks up by key, and HashSet keeps unique values.", "sequence | key | uniqueness"),
                Step("Compare", "List vs LinkedList", "A specialized structure needs a real reason",
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

                        // Useful when the node is already available.
                        queue.AddAfter(node, nextOrder);
                        queue.Remove(node);
                        """),
                    "List is the general-purpose choice", "LinkedList stands out only when you retain nodes and frequently insert or remove near them.", "List by default | LinkedList with nodes"),
                Step("Operations", "Access", "Sequence, key, and uniqueness solve different problems",
                    Pane("Sequence.cs", "Iterate", """
                        foreach (Order order in orders)
                        {
                            Process(order);
                        }
                        """),
                    Pane("Lookup.cs", "Find", """
                        if (ordersById.TryGetValue(orderId, out Order? order))
                        {
                            Process(order);
                        }
                        """),
                    "The operation should reveal the intention", "Iterating every item and locating one by Id are different access patterns.", "foreach → sequence | key → Dictionary"),
                Step("Generics", "Reuse", "One algorithm can preserve type safety",
                    Pane("Repository.cs", "Generic type", """
                        public sealed class Repository<T>
                        {
                            private readonly List<T> items = [];

                            public void Add(T item)
                            {
                                items.Add(item);
                            }
                        }
                        """),
                    Pane("Usage.cs", "Concrete types", """
                        var orderRepository = new Repository<Order>();
                        orderRepository.Add(order);

                        var codeRepository = new Repository<string>();
                        codeRepository.Add("MONITOR-24");
                        """),
                    "T is selected when the type is used", "Generics reuse structure without converting everything to object or losing compiler checks.", "Repository<T>"),
                Step("Integrate", "Collection map", "Solve sequence, lookup, and uniqueness without one universal collection",
                    Bad("OrderStore.Before.cs", "Deficient code", """
                        // LinkedList does not improve lookup by Id.
                        var orders = new LinkedList<Order>();

                        Order? found = null;
                        foreach (Order order in orders)
                        {
                            if (order.Id == orderId) found = order;
                        }
                        """),
                    Good("OrderStore.After.cs", "Correct code", """
                        // List: general sequence and index access.
                        var orders = new List<Order>();

                        // Dictionary: frequent access through a key.
                        var ordersById = new Dictionary<Guid, Order>();

                        // HashSet: unique product codes.
                        var productCodes = new HashSet<string>();

                        orders.Add(order);
                        ordersById[order.Id] = order;
                        productCodes.Add(order.ProductCode);

                        bool exists = ordersById.TryGetValue(orderId, out Order? found);
                        """),
                    "No collection is best for everything", "Array fixes size; List stores sequences; Dictionary finds by key; HashSet enforces uniqueness; LinkedList needs a node-based pattern.", "frequent operation → collection")),

            ["linq"] = Module("linq",
                Step("Understand", "LINQ", "Query sequences with chained operations",
                    Pane("OrderQueries.cs", "Where / Select", """
                        var pendingTotals = orders
                            .Where(order => order.Status == OrderStatus.Pending)
                            .Select(order => order.Subtotal)
                            .ToList();

                        bool hasLargeOrder = orders.Any(
                            order => order.Subtotal > 10_000m);
                        """), null,
                    "LINQ describes the shape of the result", "Where filters, Select transforms, Any checks existence, and ToList materializes.", "source → filter → projection → result"),
                Step("Compare", "Style", "Imperative loop versus declarative query",
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
                    "Choose the most readable form", "foreach exposes each step; LINQ emphasizes the result. Neither is automatically better in every case.", "how to iterate | what to produce"),
                Step("Lambdas", "Functions", "Pass a short operation as an argument",
                    Pane("Filter.cs", "Predicate", """
                        // Receives Order and returns bool.
                        Func<Order, bool> isPending =
                            order => order.Status == OrderStatus.Pending;

                        var pending = orders.Where(isPending);
                        """),
                    Pane("Projection.cs", "Projection", """
                        // Receives Order and returns decimal.
                        Func<Order, decimal> selectTotal =
                            order => order.Subtotal;

                        var totals = orders.Select(selectTotal);
                        """),
                    "The operator determines the lambda signature", "Where needs a boolean decision; Select needs the value produced for each element.", "input => result"),
                Step("Execution", "Deferred", "The query runs when it is enumerated",
                    Bad("Deferred.cs", "Live query", """
                        var pending = orders.Where(order => order.IsPending);
                        orders.Add(new Order { IsPending = true });

                        // The new order appears when pending is enumerated.
                        foreach (Order order in pending) { }
                        """),
                    Good("Materialized.cs", "Fixed result", """
                        var pending = orders
                            .Where(order => order.IsPending)
                            .ToList();

                        orders.Add(new Order { IsPending = true });
                        // pending keeps the earlier snapshot.
                        """),
                    "ToList marks the execution point", "Without materialization, many queries read the source again each time they are enumerated.", "IEnumerable → deferred | ToList → now"),
                Step("Integrate", "Query", "Avoid repeated enumeration and unnecessary transformations",
                    Bad("OrderQueries.Before.cs", "Deficient code", """
                        var pending = orders.Where(order => order.IsPending);

                        if (pending.Count() > 0)
                        {
                            decimal total = pending.Sum(order => order.Subtotal);
                            Console.WriteLine(pending.Count());
                        }
                        """),
                    Good("OrderQueries.After.cs", "Correct code", """
                        var pending = orders
                            .Where(order => order.IsPending)
                            .ToList();

                        if (pending.Any())
                        {
                            decimal total = pending.Sum(order => order.Subtotal);
                            Console.WriteLine(pending.Count);
                        }
                        """),
                    "Materialize when reusing the same result", "Avoid repeatedly enumerating an expensive query or a source that can change.", "Where → ToList → reuse")),

            ["task-async-await"] = Module("task-async-await",
                Step("Understand", "Task", "Represent an operation that completes in the future",
                    Pane("OrderService.cs", "Task / Task<T>", """
                        // Task exposes completion and errors.
                        public Task SaveAsync(Order order)
                        {
                            return repository.SaveAsync(order);
                        }

                        // Task<T> also provides a result.
                        public Task<Order?> GetAsync(Guid id)
                        {
                            return repository.GetAsync(id);
                        }
                        """), null,
                    "Task does not mean a new thread", "Task represents an operation; I/O can remain pending without occupying a thread.", "Task → completion | Task<T> → result"),
                Step("Compare", "Signature", "async void versus observable Task",
                    Bad("OrderEvents.cs", "async void", """
                        // The caller cannot await it or observe errors.
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
                    "Avoid async void except for events", "Task allows callers to await completion, propagate exceptions, and compose operations.", "async Task → observable operation"),
                Step("Await", "I/O", "Wait without blocking the thread",
                    Bad("Blocking.cs", "Blocking", """
                        Order order = repository
                            .GetAsync(orderId)
                            .Result;
                        """),
                    Good("Awaiting.cs", "Awaiting", """
                        Order? order = await repository
                            .GetAsync(orderId, cancellationToken);
                        """),
                    "Await keeps the asynchronous flow", ".Result and .Wait() block and can cause deadlocks; await resumes when the operation completes.", "start → await → resume"),
                Step("ValueTask", "Return type", "Use ValueTask only for measured hot paths",
                    Pane("TaskExample.cs", "Task<T>", """
                        public Task<Order?> GetAsync(Guid id)
                        {
                            return repository.GetAsync(id);
                        }
                        """),
                    Pane("ValueTaskExample.cs", "ValueTask<T>", """
                        public ValueTask<Order?> FindCachedAsync(Guid id)
                        {
                            if (cache.TryGetValue(id, out Order? order))
                            {
                                return ValueTask.FromResult<Order?>(order);
                            }

                            return new ValueTask<Order?>(LoadAsync(id));
                        }
                        """),
                    "Task is the default", "ValueTask can reduce allocations when completion is frequently synchronous, but adds usage constraints and complexity.", "Task by default | ValueTask after measuring"),
                Step("Integrate", "Cancellation", "Propagate cancellation through the whole operation",
                    Bad("OrderFlow.Before.cs", "Cancellation lost", """
                        public async Task<Order> CreateAsync(CreateOrderRequest request)
                        {
                            await repository.SaveAsync(Create(request));
                            return order;
                        }
                        """),
                    Good("OrderFlow.After.cs", "Cancellation propagated", """
                        public async Task<Order> CreateAsync(
                            CreateOrderRequest request,
                            CancellationToken cancellationToken)
                        {
                            Order order = Create(request);
                            await repository.SaveAsync(order, cancellationToken);
                            return order;
                        }
                        """),
                    "Cancellation is part of the async contract", "Pass the token to every operation that supports it so abandoned requests stop unnecessary work.", "request → service → repository")),

            ["pruebas-aaa"] = Module("pruebas-aaa",
                Step("Understand", "AAA", "Arrange, act, and assert one observable behavior",
                    Pane("OrderTests.cs", "Arrange / Act / Assert", """
                        [Fact]
                        public void Confirm_DraftOrder_ChangesStatus()
                        {
                            // Arrange
                            var order = new Order("MONITOR");

                            // Act
                            order.Confirm();

                            // Assert
                            Assert.Equal(OrderStatus.Confirmed, order.Status);
                        }
                        """), null,
                    "A test tells a behavioral story", "AAA separates setup, the action under test, and the observable expectation.", "arrange → act → assert"),
                Step("Compare", "Coupling", "Test behavior instead of implementation details",
                    Bad("OrderTests.Before.cs", "Brittle test", """
                        [Fact]
                        public void Confirm_CallsPrivateMethod()
                        {
                            Assert.True(InvokePrivate(order, "CanConfirm"));
                        }
                        """),
                    Good("OrderTests.After.cs", "Observable result", """
                        [Fact]
                        public void Confirm_DraftOrder_ChangesStatus()
                        {
                            var order = new Order("MONITOR");
                            order.Confirm();
                            Assert.Equal(OrderStatus.Confirmed, order.Status);
                        }
                        """),
                    "Tests should survive refactoring", "Assert what a consumer can observe, not which private method produced it.", "input → behavior → result"),
                Step("Cases", "Boundaries", "Cover valid, invalid, and boundary values",
                    Pane("ValidCases.cs", "Valid cases", """
                        [Theory]
                        [InlineData(1)]
                        [InlineData(10)]
                        public async Task Create_WithPositiveQuantity_Succeeds(int quantity)
                        {
                            CreateOrderResult result = await service.CreateAsync(Request(quantity));
                            Assert.True(result.IsSuccess);
                        }
                        """),
                    Pane("InvalidCases.cs", "Invalid cases", """
                        [Theory]
                        [InlineData(0)]
                        [InlineData(-1)]
                        public async Task Create_WithNonPositiveQuantity_Fails(int quantity)
                        {
                            CreateOrderResult result = await service.CreateAsync(Request(quantity));
                            Assert.False(result.IsSuccess);
                        }
                        """),
                    "Boundaries expose common defects", "Do not test only the happy path. Include zero, negatives, and values where rules change.", "valid | invalid | boundary"),
                Step("Integration", "Real API", "Verify that components collaborate",
                    Pane("ServiceTest.cs", "Unit", """
                        [Fact]
                        public async Task Create_InvalidQuantity_ReturnsError()
                        {
                            var service = new CreateOrderService(fakeStore);
                            CreateOrderResult result = await service.CreateAsync(Request(0));
                            Assert.False(result.IsSuccess);
                        }
                        """),
                    Pane("ApiTest.cs", "Integration", """
                        [Fact]
                        public async Task PostOrder_ValidRequest_ReturnsCreated()
                        {
                            HttpResponseMessage response = await client.PostAsJsonAsync(
                                "/api/orders",
                                Request(2));

                            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                        }
                        """),
                    "Choose the test type by risk", "A unit test focuses on one rule; an integration test verifies the endpoint, serialization, and real collaboration.", "unit | integration"),
                Step("Integrate", "Useful test", "Give the test one clear reason to fail",
                    Bad("Tests.Before.cs", "Deficient code", """
                        [Fact]
                        public async Task Test1()
                        {
                            Assert.NotNull(await service.CreateAsync(Request(2)));
                            Assert.NotNull(await service.CreateAsync(Request(0)));
                        }
                        """),
                    Good("Tests.After.cs", "Correct code", """
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
                    "The test explains the behavior", "Its name, setup, action, and assertion should explain a failure before debugging.", "scenario → action → expectation")),

            ["rest-http"] = Module("rest-http",
                Step("Understand", "HTTP contract", "Route, verb, DTO, and response form the API",
                    Pane("OrderEndpoints.cs", "POST /api/orders", """
                        app.MapPost("/api/orders", async (CreateOrderRequest request) =>
                        {
                            CreateOrderResult result = await service.CreateAsync(request);

                            return result.IsSuccess
                                ? Results.Created($"/api/orders/{result.Order!.Id}", result.Order)
                                : Results.ValidationProblem(result.Errors);
                        });
                        """), null,
                    "An API is an observable contract", "Clients observe data, routes, verbs, HTTP status codes, headers, and error structures.", "request → endpoint → response"),
                Step("Compare", "DTO", "External contract versus internal entity",
                    Bad("OrderEndpoint.Before.cs", "Exposed entity", """
                        app.MapPost("/api/orders", (Order order) =>
                        {
                            database.Orders.Add(order);
                            database.SaveChanges();
                            return order;
                        });
                        """),
                    Good("OrderEndpoint.After.cs", "Explicit DTO", """
                        app.MapPost("/api/orders", async (CreateOrderRequest request) =>
                        {
                            CreateOrderResult result = await service.CreateAsync(request);
                            return Results.Created(
                                $"/api/orders/{result.Order!.Id}",
                                result.Order);
                        });
                        """),
                    "The client must not control the entity", "A DTO limits what consumers can decide and keeps persistence details out of the public contract.", "external DTO ≠ internal entity"),
                Step("Methods", "HTTP methods", "Express read, create, replace, update, and delete",
                    Pane("OrderReads.http", "Read", """
                        // Read the collection.
                        GET /api/orders

                        // Read one resource.
                        GET /api/orders/{id}

                        // Read metadata without the response body.
                        HEAD /api/orders/{id}
                        """),
                    Pane("OrderWrites.http", "Write", """
                        POST /api/orders        // Create
                        PUT /api/orders/{id}    // Replace
                        PATCH /api/orders/{id}  // Partially update
                        DELETE /api/orders/{id} // Delete
                        """),
                    "The route identifies; the method states the intention", "GET reads, POST creates, PUT replaces, PATCH updates partially, and DELETE removes.", "GET | POST | PUT | PATCH | DELETE"),
                Step("Responses", "HTTP status", "Recognize common API outcomes",
                    Pane("SuccessResults.cs", "2xx · success", """
                        return Results.Ok(response);        // 200
                        return Results.Created(location, response); // 201
                        return Results.Accepted(location); // 202
                        return Results.NoContent();         // 204
                        """),
                    Pane("ErrorResults.cs", "4xx / 5xx · error", """
                        return Results.BadRequest();          // 400 invalid input
                        return Results.Unauthorized();        // 401 unauthenticated
                        return Results.Forbid();              // 403 forbidden
                        return Results.NotFound();            // 404 missing
                        return Results.Conflict();            // 409 conflict
                        return Results.UnprocessableEntity(); // 422 invalid rule
                        return Results.StatusCode(500);       // internal failure
                        return Results.StatusCode(503);       // unavailable
                        """),
                    "The status code summarizes the outcome", "Clients should distinguish success, authentication, authorization, absence, conflicts, validation, and server failures without parsing free text.", "2xx success | 4xx client | 5xx server"),
                Step("Integrate", "Order API", "Combine methods, routes, DTOs, and precise responses",
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
                            (Guid id, ReplaceOrderRequest request) => ReplaceOrder(id, request));

                        app.MapPatch("/api/orders/{id:guid}",
                            (Guid id, UpdateOrderRequest request) => UpdateOrder(id, request));

                        app.MapDelete("/api/orders/{id:guid}",
                            (Guid id) => DeleteOrder(id));
                        """),
                    "The full contract is understandable without opening the database", "Each endpoint receives a DTO, delegates rules, and translates the result into the method and status that best describe it.", "route + method + DTO + status")),

            ["persistencia-ef-core"] = Module("persistencia-ef-core",
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
                    "Persistence does not mean exposing the database", "The domain creates a valid order, the store saves it, the query projects a DTO, and the endpoint selects the HTTP response.", "domain → EF Core → DTO")),

            ["aspnet-core"] = Module("aspnet-core",
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
                    "The module closes with observable behavior", "A request crosses the HTTP contract, domain rules, EF Core store, and response mapping while each layer remains testable.", "HTTP → service → EF Core → HTTP")),

            ["ia-llm"] = Module("ia-llm",
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
                    "Integrate through a replaceable contract", "Tests use a deterministic fake. The laboratory simulates the provider and demonstrates the boundary; a real integration connects through server configuration.", "LLM suggests → code validates → human reviews"))
        };
}
