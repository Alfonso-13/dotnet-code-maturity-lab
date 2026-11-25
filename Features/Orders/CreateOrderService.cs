using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Orders;

public sealed class CreateOrderService(IOrderStore store)
{
    public async Task<CreateOrderResult> CreateAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
        => await CreateAsync(request, AppLanguage.English, cancellationToken);

    public async Task<CreateOrderResult> CreateAsync(
        CreateOrderRequest request,
        AppLanguage language,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request, language);

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

    private static Dictionary<string, string[]> Validate(CreateOrderRequest request, AppLanguage language)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(request.ProductCode))
        {
            errors[nameof(request.ProductCode)] = [language == AppLanguage.Spanish
                ? "El código del producto es obligatorio."
                : "The product code is required."];
        }
        else if (request.ProductCode.Trim().Length > 80)
        {
            // Keep the public contract aligned with OrderDbContext's column limit.
            errors[nameof(request.ProductCode)] = [language == AppLanguage.Spanish
                ? "El código del producto no puede superar 80 caracteres."
                : "The product code cannot exceed 80 characters."];
        }

        if (request.Quantity <= 0)
        {
            errors[nameof(request.Quantity)] = [language == AppLanguage.Spanish
                ? "La cantidad debe ser mayor que cero."
                : "The quantity must be greater than zero."];
        }

        if (request.UnitPrice <= 0)
        {
            errors[nameof(request.UnitPrice)] = [language == AppLanguage.Spanish
                ? "El precio unitario debe ser mayor que cero."
                : "The unit price must be greater than zero."];
        }
        else if (request.Quantity > 0 && request.UnitPrice > decimal.MaxValue / request.Quantity)
        {
            errors[nameof(request.UnitPrice)] = [language == AppLanguage.Spanish
                ? "El subtotal supera el rango admitido."
                : "The subtotal exceeds the supported range."];
        }

        return errors;
    }
}
