namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson TaskAsyncSpanish() => Module("task-async-await",
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
            "No escondas la operación asíncrona", "Devuelve Task, usa await y propaga CancellationToken. ValueTask es una optimización puntual, no un reemplazo general.", "Task + await + CancellationToken"));

    private static ModuleLesson TaskAsyncEnglish() => Module("task-async-await",
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
            "Cancellation is part of the async contract", "Pass the token to every operation that supports it so abandoned requests stop unnecessary work.", "request → service → repository"));
}
