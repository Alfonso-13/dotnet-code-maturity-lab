namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson LinqSpanish() => Module("linq",
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

                // Con EF Core, IQueryable compone expresiones traducibles a SQL.
                IQueryable<Order> databaseQuery = db.Orders
                    .AsNoTracking()
                    .Where(order => order.Status == OrderStatus.Created);
                List<Order> saved = await databaseQuery.ToListAsync();
                // Después de ToListAsync, IEnumerable recorre objetos en memoria.
                IEnumerable<Order> inMemory = saved.Where(order => order.Subtotal > 0);
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
            "Materializa cuando reutilizas el mismo resultado", "Evita enumerar repetidamente una consulta costosa o una fuente que puede cambiar.", "Where → ToList → reutilizar"));

    private static ModuleLesson LinqEnglish() => Module("linq",
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

                // With EF Core, IQueryable composes SQL-translatable expressions.
                IQueryable<Order> databaseQuery = db.Orders
                    .AsNoTracking()
                    .Where(order => order.Status == OrderStatus.Created);
                List<Order> saved = await databaseQuery.ToListAsync();
                // After ToListAsync, IEnumerable iterates in-memory objects.
                IEnumerable<Order> inMemory = saved.Where(order => order.Subtotal > 0);
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
            "Materialize when reusing the same result", "Avoid repeatedly enumerating an expensive query or a source that can change.", "Where → ToList → reuse"));
}
