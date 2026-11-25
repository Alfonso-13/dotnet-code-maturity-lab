namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson RestHttpSpanish() => Module("rest-http",
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
            "El contrato completo se entiende sin abrir la base de datos", "Cada endpoint recibe un DTO, delega la regla y traduce el resultado usando el método y el código HTTP que mejor expresan lo ocurrido.", "ruta + método + DTO + status"));

    private static ModuleLesson RestHttpEnglish() => Module("rest-http",
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
            "The full contract is understandable without opening the database", "Each endpoint receives a DTO, delegates rules, and translates the result into the method and status that best describe it.", "route + method + DTO + status"));
}
