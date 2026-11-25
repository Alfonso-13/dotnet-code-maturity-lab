namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    private static ModuleLesson OopSpanish() => Module("poo",
        Step("Entiende", "POO", "Estado y comportamiento colaboran dentro de objetos",
            Pane("Order.cs", "Encapsulación", """
                public sealed class Order
                {
                    public OrderStatus Status { get; private set; }

                    public void Confirm()
                    {
                        if (Status != OrderStatus.Draft) return;
                        Status = OrderStatus.Confirmed;
                    }
                }
                """), null,
            "Encapsular mantiene juntas las reglas", "El objeto controla su estado y solo expone operaciones válidas para modificarlo.", "estado privado + operación pública"),
        Step("Compara", "Diseño", "Herencia forzada frente a composición",
            Bad("EmailOrderService.cs", "Herencia rígida", """
                public class OrderService { }

                public sealed class EmailOrderService : OrderService
                {
                    public void SendEmail() { }
                }
                """),
            Good("OrderService.cs", "Composición", """
                public sealed class OrderService(INotifier notifier)
                {
                    public Task NotifyAsync(Order order)
                    {
                        return notifier.SendAsync(order);
                    }
                }
                """),
            "Prefiere relaciones que expresen la realidad", "Una notificación es una capacidad colaboradora; no convierte al servicio en otro tipo de servicio.", "tiene un → composición | es un → herencia"),
        Step("Pilares", "Abstracción", "Exponer la capacidad sin revelar la implementación",
            Pane("INotifier.cs", "Contrato", """
                public interface INotifier
                {
                    Task SendAsync(Order order);
                }
                """),
            Pane("EmailNotifier.cs", "Implementación", """
                public sealed class EmailNotifier : INotifier
                {
                    public Task SendAsync(Order order)
                    {
                        return emailClient.SendAsync(order.Id);
                    }
                }
                """),
            "Abstraer es mostrar lo esencial", "El consumidor conoce la capacidad SendAsync, no los detalles del proveedor de correo.", "interface → qué | clase → cómo"),
        Step("Polimorfismo", "Sustitución", "Mismo contrato, comportamientos diferentes",
            Pane("EmailNotifier.cs", "Correo", """
                INotifier notifier = new EmailNotifier(emailClient);
                await notifier.SendAsync(order);
                """),
            Pane("SmsNotifier.cs", "SMS", """
                INotifier notifier = new SmsNotifier(smsClient);
                await notifier.SendAsync(order);
                """),
            "El consumidor usa el contrato", "El servicio puede recibir correo o SMS sin cambiar la operación que invoca.", "INotifier → Email | SMS"),
        Step("Integra", "POO aplicada", "Usar los cuatro pilares dentro de una colaboración",
            Bad("OrderService.Before.cs", "Código deficiente", """
                // Estado expuesto y responsabilidades mezcladas.
                public sealed class Order
                {
                    public string Status { get; set; } = "draft";
                }

                public sealed class OrderService
                {
                    public async Task ProcessAsync(
                        Order order,
                        string channel)
                    {
                        order.Status = "confirmed";

                        if (channel == "email") await SendEmail(order);
                        if (channel == "sms") await SendSms(order);
                    }
                }
                """),
            Good("OrderService.After.cs", "Código correcto", """
                // ABSTRACCIÓN: expone una capacidad.
                public interface INotifier
                {
                    Task SendAsync(Order order);
                }

                // ENCAPSULACIÓN: la orden protege su estado.
                public sealed class Order
                {
                    public OrderStatus Status { get; private set; }

                    public void Confirm()
                    {
                        if (Status != OrderStatus.Draft) return;
                        Status = OrderStatus.Confirmed;
                    }
                }

                // POLIMORFISMO: EmailNotifier o SmsNotifier
                // pueden ocupar el lugar de INotifier.

                // COMPOSICIÓN: el servicio recibe la capacidad.
                public sealed class OrderService(INotifier notifier)
                {
                    public Task ProcessAsync(Order order)
                    {
                        order.Confirm();
                        return notifier.SendAsync(order);
                    }
                }
                """),
            "POO protege reglas y permite sustituir conductas", "Los pilares no son definiciones aisladas: encapsulan el estado, abstraen capacidades y permiten polimorfismo mediante composición.", "Order + INotifier + implementación"));

    private static ModuleLesson OopEnglish() => Module("poo",
        Step("Understand", "OOP", "State and behavior collaborate inside objects",
            Pane("Order.cs", "Encapsulation", """
                public sealed class Order
                {
                    public OrderStatus Status { get; private set; }

                    public void Confirm()
                    {
                        if (Status != OrderStatus.Draft) return;
                        Status = OrderStatus.Confirmed;
                    }
                }
                """), null,
            "Encapsulation keeps rules together", "The object controls its state and exposes only valid operations for changing it.", "private state + public operation"),
        Step("Compare", "Design", "Forced inheritance versus composition",
            Bad("EmailOrderService.cs", "Rigid inheritance", """
                public class OrderService { }

                public sealed class EmailOrderService : OrderService
                {
                    public void SendEmail() { }
                }
                """),
            Good("OrderService.cs", "Composition", """
                public sealed class OrderService(INotifier notifier)
                {
                    public Task NotifyAsync(Order order)
                    {
                        return notifier.SendAsync(order);
                    }
                }
                """),
            "Prefer relationships that express reality", "Notification is a collaborating capability; it does not turn the service into a different kind of service.", "has a → composition | is a → inheritance"),
        Step("Pillars", "Abstraction", "Expose the capability without revealing the implementation",
            Pane("INotifier.cs", "Contract", """
                public interface INotifier
                {
                    Task SendAsync(Order order);
                }
                """),
            Pane("EmailNotifier.cs", "Implementation", """
                public sealed class EmailNotifier : INotifier
                {
                    public Task SendAsync(Order order)
                    {
                        return emailClient.SendAsync(order.Id);
                    }
                }
                """),
            "Abstraction shows what is essential", "The consumer knows the SendAsync capability, not the email provider details.", "interface → what | class → how"),
        Step("Polymorphism", "Substitution", "Same contract, different behaviors",
            Pane("EmailNotifier.cs", "Email", """
                INotifier notifier = new EmailNotifier(emailClient);
                await notifier.SendAsync(order);
                """),
            Pane("SmsNotifier.cs", "SMS", """
                INotifier notifier = new SmsNotifier(smsClient);
                await notifier.SendAsync(order);
                """),
            "The consumer uses the contract", "The service can receive email or SMS without changing the operation it invokes.", "INotifier → Email | SMS"),
        Step("Integrate", "Applied OOP", "Use all four pillars within one collaboration",
            Bad("OrderService.Before.cs", "Deficient code", """
                // Exposed state and mixed responsibilities.
                public sealed class Order
                {
                    public string Status { get; set; } = "draft";
                }

                public sealed class OrderService
                {
                    public async Task ProcessAsync(
                        Order order,
                        string channel)
                    {
                        order.Status = "confirmed";

                        if (channel == "email") await SendEmail(order);
                        if (channel == "sms") await SendSms(order);
                    }
                }
                """),
            Good("OrderService.After.cs", "Correct code", """
                // ABSTRACTION: exposes a capability.
                public interface INotifier
                {
                    Task SendAsync(Order order);
                }

                // ENCAPSULATION: the order protects its state.
                public sealed class Order
                {
                    public OrderStatus Status { get; private set; }

                    public void Confirm()
                    {
                        if (Status != OrderStatus.Draft) return;
                        Status = OrderStatus.Confirmed;
                    }
                }

                // POLYMORPHISM: EmailNotifier or SmsNotifier
                // can take the place of INotifier.

                // COMPOSITION: the service receives the capability.
                public sealed class OrderService(INotifier notifier)
                {
                    public Task ProcessAsync(Order order)
                    {
                        order.Confirm();
                        return notifier.SendAsync(order);
                    }
                }
                """),
            "OOP protects rules and allows behavior substitution", "The pillars are not isolated definitions: they encapsulate state, abstract capabilities, and enable polymorphism through composition.", "Order + INotifier + implementation"));
}
