namespace DotnetCodeMaturityLab.Features.Orders;

public sealed class CreateOrderService(IOrderStore store)
{
    public async Task<CreateOrderResult> CreateAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);

        if (errors.Count > 0)
        {
            return CreateOrderResult.Failure(errors);
        }

        var order = new Order(
            Guid.NewGuid(),
            request.ProductCode!.Trim(),
            request.Quantity,
            request.UnitPrice);

        await store.AddAsync(order, cancellationToken);

        return CreateOrderResult.Success(order);
    }

    private static Dictionary<string, string[]> Validate(CreateOrderRequest request)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.ProductCode))
        {
            errors[nameof(request.ProductCode)] = ["El código del producto es obligatorio."];
        }

        if (request.Quantity <= 0)
        {
            errors[nameof(request.Quantity)] = ["La cantidad debe ser mayor que cero."];
        }

        if (request.UnitPrice <= 0)
        {
            errors[nameof(request.UnitPrice)] = ["El precio unitario debe ser mayor que cero."];
        }

        return errors;
    }
}
