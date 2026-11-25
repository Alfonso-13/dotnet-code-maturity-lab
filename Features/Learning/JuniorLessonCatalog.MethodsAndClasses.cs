namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson MethodsAndClassesSpanish() => Module("metodos-y-clases",
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
            "El objeto debe proteger sus propias reglas", "Una clase útil no es solo una bolsa de propiedades: conserva juntas las reglas y los datos que protege.", "constructor → propiedades → métodos → estado válido"));

    private static ModuleLesson MethodsAndClassesEnglish() => Module("metodos-y-clases",
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
            "The object should protect its own rules", "A useful class is not merely a bag of properties: it keeps together the rules and data it protects.", "constructor → properties → methods → valid state"));
}
