using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Orders;

/// <summary>Separates the local HTTP request from the Pages-only in-memory demonstration.</summary>
public interface IOrderLabTransport
{
    Task<OrderLabSubmission> SubmitAsync(
        CreateOrderRequest request,
        AppLanguage language,
        CancellationToken cancellationToken = default);
}

public sealed record OrderLabSubmission(
    int StatusCode,
    string ResponseJson,
    CreateOrderResponse? Created);
