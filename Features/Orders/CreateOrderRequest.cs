namespace DotnetCodeMaturityLab.Features.Orders;

public sealed record CreateOrderRequest(
    string? ProductCode,
    int Quantity,
    decimal UnitPrice);
