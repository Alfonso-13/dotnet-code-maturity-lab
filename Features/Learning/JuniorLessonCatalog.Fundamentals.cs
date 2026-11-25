namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson FundamentalsSpanish() => Module("fundamentos-csharp",
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
            "El orden de las decisiones también importa", "Primero declara un contrato claro, después valida y finalmente calcula o recorre los elementos.", "contrato → validación → recorrido → resultado"));

    private static ModuleLesson FundamentalsEnglish() => Module("fundamentos-csharp",
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
            "The order of decisions matters too", "First declare a clear contract, then validate, and finally calculate or traverse the elements.", "contract → validation → traversal → result"));
}
