namespace DotnetCodeMaturityLab.Features.Orders;

public sealed class CreateOrderResult
{
    private CreateOrderResult(
        Order? order,
        IReadOnlyDictionary<string, string[]> errors)
    {
        Order = order;
        Errors = errors;
    }

    public Order? Order { get; }

    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public bool IsSuccess => Order is not null;

    public static CreateOrderResult Success(Order order) =>
        new(order, new Dictionary<string, string[]>());

    public static CreateOrderResult Failure(Dictionary<string, string[]> errors) =>
        new(null, errors);
}
