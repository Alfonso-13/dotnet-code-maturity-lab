namespace DotnetCodeMaturityLab.Features.Orders;

public sealed record CreateOrderResponse(
    Guid Id,
    string ProductCode,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal,
    string Status)
{
    public static CreateOrderResponse FromOrder(Order order) => new(
        order.Id,
        order.ProductCode,
        order.Quantity,
        order.UnitPrice,
        order.Subtotal,
        order.Status.ToString());
}
