namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson ModelsAndMemorySpanish() => Module("modelos-y-memoria",
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

                // Boxing: object almacena una copia empaquetada del int.
                int quantity = 2;
                object boxed = quantity;
                quantity = 3; // boxed todavía contiene 2.
                int restored = (int)boxed; // Unboxing requiere el tipo correcto.
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
            "Modela antes de optimizar", "Entidad, valor, estado cerrado y capacidad necesitan tipos diferentes aunque todos participen en el mismo flujo.", "class + struct + interface + record + enum"));

    private static ModuleLesson ModelsAndMemoryEnglish() => Module("modelos-y-memoria",
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

                // Boxing: object holds a boxed copy of the int.
                int quantity = 2;
                object boxed = quantity;
                quantity = 3; // boxed still contains 2.
                int restored = (int)boxed; // Unboxing requires the matching type.
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
            "Model before optimizing", "Entity, value, closed state, and capability need different types even when they participate in the same flow.", "class + struct + interface + record + enum"));
}
