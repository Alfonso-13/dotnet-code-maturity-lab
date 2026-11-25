namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson CollectionsAndGenericsSpanish() => Module("colecciones-y-genericos",
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
            "No existe una colección mejor para todo", "Array fija tamaño; List organiza secuencias; Dictionary busca por clave; HashSet garantiza unicidad; LinkedList requiere un patrón real de nodos.", "operación frecuente → colección"));

    private static ModuleLesson CollectionsAndGenericsEnglish() => Module("colecciones-y-genericos",
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
            "No collection is best for everything", "Array fixes size; List stores sequences; Dictionary finds by key; HashSet enforces uniqueness; LinkedList needs a node-based pattern.", "frequent operation → collection"));
}
