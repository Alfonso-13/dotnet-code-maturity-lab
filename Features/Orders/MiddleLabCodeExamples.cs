using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Orders;

/// <summary>Illustrative capstone code, separate from the running order API.</summary>
public static class MiddleLabCodeExamples
{
    public static string Before(AppLanguage language) =>
        language == AppLanguage.Spanish ? BeforeCodeEs : BeforeCodeEn;

    public static string After(AppLanguage language) =>
        language == AppLanguage.Spanish ? AfterCodeEs : AfterCodeEn;
    private const string BeforeCodeEn = """
        public sealed class OrderService(
            OrderDbContext db,
            EmailClient email,
            VendorAiClient ai)
        {
            public async Task<Order> CreateAsync(CreateOrderRequest request)
            {
                // Validation, persistence, notification, and provider details
                // change together inside one service.
                Order order = MapAndCalculate(request);
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                await email.SendAsync(order);
                order.Category = await ai.ClassifyAsync(request.Note);
                return order;
            }
        }
        """;

    private const string BeforeCodeEs = """
        public sealed class OrderService(
            OrderDbContext db,
            EmailClient email,
            VendorAiClient ai)
        {
            public async Task<Order> CreateAsync(CreateOrderRequest request)
            {
                // Validación, persistencia, notificación y proveedor
                // cambian juntos dentro de un solo servicio.
                Order order = MapAndCalculate(request);
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                await email.SendAsync(order);
                order.Category = await ai.ClassifyAsync(request.Note);
                return order;
            }
        }
        """;

    private const string AfterCodeEn = """
        public sealed class CreateOrderUseCase(
            IOrderFactory factory,
            IOrderStore store,
            INotificationStrategySelector notifications,
            IOrderSuggestionService suggestions)
        {
            public async Task<CreateOrderResult> ExecuteAsync(
                CreateOrderCommand command,
                CancellationToken token)
            {
                Order order = factory.Create(command);
                await store.SaveAsync(order, token);
                await notifications.Get(command.Channel).SendAsync(order, token);

                OrderSuggestion suggestion = await suggestions.SuggestAsync(
                    command.Note,
                    token);

                return new CreateOrderResult.Created(order, suggestion);
            }
        }
        """;

    private const string AfterCodeEs = """
        public sealed class CreateOrderUseCase(
            IOrderFactory factory,
            IOrderStore store,
            INotificationStrategySelector notifications,
            IOrderSuggestionService suggestions)
        {
            public async Task<CreateOrderResult> ExecuteAsync(
                CreateOrderCommand command,
                CancellationToken token)
            {
                Order order = factory.Create(command);
                await store.SaveAsync(order, token);
                await notifications.Get(command.Channel).SendAsync(order, token);

                OrderSuggestion suggestion = await suggestions.SuggestAsync(
                    command.Note,
                    token);

                return new CreateOrderResult.Created(order, suggestion);
            }
        }
        """;
}