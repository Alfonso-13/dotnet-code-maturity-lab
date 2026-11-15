namespace DotnetCodeMaturityLab.Features.Orders;

public sealed class Order
{
    private Order()
    {
        ProductCode = string.Empty;
    }

    internal Order(Guid id, string productCode, int quantity, decimal unitPrice)
    {
        Id = id;
        ProductCode = productCode;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Subtotal = quantity * unitPrice;
        Status = OrderStatus.Created;
    }

    public Guid Id { get; private set; }

    public string ProductCode { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal Subtotal { get; private set; }

    public OrderStatus Status { get; private set; }
}

public enum OrderStatus
{
    Created
}
