namespace DotnetCodeMaturityLab.Features.Orders;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/orders")
            .WithTags("Orders");

        group.MapPost("/", CreateOrder)
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        group.MapGet("/", GetOrders)
            .WithName("GetOrders")
            .Produces<IReadOnlyCollection<CreateOrderResponse>>();

        group.MapGet("/{id:guid}", GetOrder)
            .WithName("GetOrderById")
            .Produces<CreateOrderResponse>()
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }

    private static async Task<IResult> CreateOrder(
        CreateOrderRequest request,
        CreateOrderService service,
        CancellationToken cancellationToken)
    {
        var result = await service.CreateAsync(request, cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.ValidationProblem(result.Errors);
        }

        var response = CreateOrderResponse.FromOrder(result.Order!);

        return Results.Created($"/api/orders/{response.Id}", response);
    }

    private static async Task<IResult> GetOrders(
        IOrderStore store,
        CancellationToken cancellationToken) =>
        Results.Ok((await store.GetAllAsync(cancellationToken)).Select(CreateOrderResponse.FromOrder));

    private static async Task<IResult> GetOrder(
        Guid id,
        IOrderStore store,
        CancellationToken cancellationToken)
    {
        var order = await store.FindAsync(id, cancellationToken);

        return order is null
            ? Results.NotFound()
            : Results.Ok(CreateOrderResponse.FromOrder(order));
    }
}
