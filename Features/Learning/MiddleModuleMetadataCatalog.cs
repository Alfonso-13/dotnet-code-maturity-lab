using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

/// <summary>
/// Route-independent labels for Middle modules. Keeping this copy outside the
/// Razor player lets both languages use the same lesson structure.
/// </summary>
public static class MiddleModuleMetadataCatalog
{
    private static readonly IReadOnlyDictionary<string, LocalizedMetadata> Modules =
        new Dictionary<string, LocalizedMetadata>(StringComparer.OrdinalIgnoreCase)
        {
            ["solid"] = Metadata(
                "Applied SOLID", "SOLID aplicado",
                "Applied SOLID: boundaries justified by change", "SOLID aplicado: límites justificados por el cambio"),
            ["inyeccion-dependencias"] = Metadata(
                "Dependency injection", "Inyección de dependencias",
                "Dependency injection: composition and lifetimes", "Inyección de dependencias: composición y ciclos de vida"),
            ["strategy-patrones"] = Metadata(
                "Strategy and patterns", "Strategy y patrones",
                "Strategy: model a real variation", "Strategy: modelar una variación real"),
            ["refactorizacion"] = Metadata(
                "Safe refactoring", "Refactorización segura",
                "Safe refactoring: change structure with evidence", "Refactorización segura: cambiar estructura con evidencia"),
            ["ef-core"] = Metadata(
                "EF Core persistence", "Persistencia con EF Core",
                "EF Core: intentional queries and transactions", "EF Core: consultas y transacciones intencionales"),
            ["problem-details"] = Metadata(
                "ProblemDetails", "ProblemDetails",
                "ProblemDetails: stable error contracts", "ProblemDetails: contratos de error estables"),
            ["seguridad-jwt"] = Metadata(
                "JWT and authorization", "JWT y autorización",
                "JWT, claims, and policy authorization", "JWT, claims y autorización por políticas"),
            ["pruebas-integracion"] = Metadata(
                "Integration testing", "Pruebas de integración",
                "Integration testing: verify real collaboration", "Pruebas de integración: comprobar colaboración real"),
            ["limites-arquitectura"] = Metadata(
                "Architecture boundaries", "Límites de arquitectura",
                "Architecture boundaries: protect policy from details", "Límites de arquitectura: proteger políticas de los detalles"),
            ["ia-integracion"] = Metadata(
                "Maintainable AI", "IA mantenible",
                "Maintainable AI integration", "Integración de IA mantenible")
        };

    public static ModuleMetadata Get(string slug, AppLanguage language)
    {
        var module = Modules[slug];
        return language == AppLanguage.Spanish
            ? new(module.ShortTitleSpanish, module.TitleSpanish, "DISEÑO MIDDLE")
            : new(module.ShortTitleEnglish, module.TitleEnglish, "MIDDLE DESIGN");
    }

    private static LocalizedMetadata Metadata(
        string shortTitleEnglish,
        string shortTitleSpanish,
        string titleEnglish,
        string titleSpanish) =>
        new(shortTitleEnglish, shortTitleSpanish, titleEnglish, titleSpanish);

    private sealed record LocalizedMetadata(
        string ShortTitleEnglish,
        string ShortTitleSpanish,
        string TitleEnglish,
        string TitleSpanish);
}
