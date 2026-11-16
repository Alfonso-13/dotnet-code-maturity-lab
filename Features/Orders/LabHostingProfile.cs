namespace DotnetCodeMaturityLab.Features.Orders;

public sealed record LabHostingProfile(
    bool IsStaticDemo,
    string RuntimeLabel,
    string PersistenceStep,
    string PersistenceDetail,
    string StorageStep,
    string StorageDetail,
    string PersistingMessage,
    string StoredMessage,
    string InvalidMessage)
{
    public static LabHostingProfile Server { get; } = new(
        IsStaticDemo: false,
        RuntimeLabel: "SQLite · EF Core",
        PersistenceStep: "EF Core",
        PersistenceDetail: "Guardar entidad",
        StorageStep: "SQLite",
        StorageDetail: "App_Data/orders.db",
        PersistingMessage: "EF Core está persistiendo la orden",
        StoredMessage: "SQLite confirmó la escritura en orders.db",
        InvalidMessage: "Respuesta 400 recibida · SQLite no cambió");

    public static LabHostingProfile GitHubPages { get; } = new(
        IsStaticDemo: true,
        RuntimeLabel: "Demo local · GitHub Pages",
        PersistenceStep: "Servicio",
        PersistenceDetail: "Aplicar reglas",
        StorageStep: "Memoria",
        StorageDetail: "Sesión del navegador",
        PersistingMessage: "El servicio está creando la orden en el navegador",
        StoredMessage: "La sesión local confirmó la escritura en memoria",
        InvalidMessage: "Respuesta 400 simulada · la memoria no cambió");
}
