namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson ErrorsAndValidationSpanish() => Module("errores-y-validacion",
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
            "El contrato de error también es parte de la API", "Los errores esperados necesitan estructura; los inesperados deben conservar diagnóstico y contexto.", "Result + ProblemDetails"));

    private static ModuleLesson ErrorsAndValidationEnglish() => Module("errores-y-validacion",
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
            "The error contract is part of the API", "Expected errors need structure; unexpected failures must preserve diagnostics and context.", "Result + ProblemDetails"));
}
