using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Orders;

public enum LabRuntimeMode
{
    Server,
    StaticDemo
}

public sealed record LabHostingProfile(
    LabRuntimeMode Mode,
    bool HasRealHttpApi,
    bool HasDurablePersistence,
    bool HasRealAiProvider,
    LocalizedText RuntimeLabel,
    LocalizedText PersistenceStep,
    LocalizedText PersistenceDetail,
    LocalizedText StorageStep,
    LocalizedText StorageDetail,
    LocalizedText PersistingMessage,
    LocalizedText StoredMessage,
    LocalizedText InvalidMessage)
{
    public bool IsStaticDemo => Mode == LabRuntimeMode.StaticDemo;

    public static LabHostingProfile Server { get; } = new(
        Mode: LabRuntimeMode.Server,
        HasRealHttpApi: true,
        HasDurablePersistence: true,
        HasRealAiProvider: false,
        RuntimeLabel: new("SQLite · EF Core", "SQLite · EF Core"),
        PersistenceStep: new("EF Core", "EF Core"),
        PersistenceDetail: new("Save entity", "Guardar entidad"),
        StorageStep: new("SQLite", "SQLite"),
        StorageDetail: new("App_Data/orders.db", "App_Data/orders.db"),
        PersistingMessage: new("EF Core is persisting the order", "EF Core está persistiendo la orden"),
        StoredMessage: new("SQLite confirmed the write to orders.db", "SQLite confirmó la escritura en orders.db"),
        InvalidMessage: new("400 response received · SQLite did not change", "Respuesta 400 recibida · SQLite no cambió"));

    public static LabHostingProfile GitHubPages { get; } = new(
        Mode: LabRuntimeMode.StaticDemo,
        HasRealHttpApi: false,
        HasDurablePersistence: false,
        HasRealAiProvider: false,
        RuntimeLabel: new("Interactive simulation · GitHub Pages", "Simulación interactiva · GitHub Pages"),
        PersistenceStep: new("Service", "Servicio"),
        PersistenceDetail: new("Apply rules", "Aplicar reglas"),
        StorageStep: new("Memory", "Memoria"),
        StorageDetail: new("Browser session", "Sesión del navegador"),
        PersistingMessage: new("The service is creating the order in the browser", "El servicio está creando la orden en el navegador"),
        StoredMessage: new("The browser session confirmed the in-memory write", "La sesión del navegador confirmó la escritura en memoria"),
        InvalidMessage: new("Simulated 400 response · memory did not change", "Respuesta 400 simulada · la memoria no cambió"));
}
