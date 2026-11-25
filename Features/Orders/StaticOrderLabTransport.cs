using System.Text.Json;
using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Orders;

/// <summary>Pages runs the same domain rules locally but does not claim an HTTP request.</summary>
public sealed class StaticOrderLabTransport(CreateOrderService service) : IOrderLabTransport
{
    public async Task<OrderLabSubmission> SubmitAsync(
        CreateOrderRequest request, AppLanguage language, CancellationToken cancellationToken = default)
    {
        var result = await service.CreateAsync(request, language, cancellationToken);
        if (result.IsSuccess)
        {
            var created = CreateOrderResponse.FromOrder(result.Order!);
            return new(201, JsonSerializer.Serialize(created, new JsonSerializerOptions(JsonSerializerDefaults.Web)), created);
        }

        return new(400, JsonSerializer.Serialize(new
        {
            title = language == AppLanguage.Spanish
                ? "Ocurrieron uno o más errores de validación."
                : "One or more validation errors occurred.",
            status = 400,
            errors = result.Errors
        }), null);
    }
}
