namespace DotnetCodeMaturityLab.Features.Localization;

public enum AppLanguage
{
    English,
    Spanish
}

public sealed class AppLanguageState
{
    public const string StorageKey = "dotnet-code-maturity-lab-language";

    public AppLanguage Current { get; private set; } = AppLanguage.English;

    public UiText Text => UiTextCatalog.Get(Current);

    public bool Set(string? languageCode)
    {
        var next = languageCode?.Equals("es", StringComparison.OrdinalIgnoreCase) == true
            ? AppLanguage.Spanish
            : AppLanguage.English;

        if (next == Current) return false;
        Current = next;
        return true;
    }

    public string Code => Current == AppLanguage.Spanish ? "es" : "en";
}

public sealed record UiText(
    string SkipToContent,
    string HomeAriaLabel,
    string ProductTagline,
    string OpenLab,
    string ViewOnGitHub,
    string MainNavigation,
    string ToggleNavigation,
    string ExploreLab,
    string Explore,
    string Home,
    string Roadmap,
    string Laboratories,
    string PrepareForInterview,
    string DotNetHistory,
    string DotNetEcosystem,
    string DesignAndOperate,
    string Architecture,
    string AzureAndScale,
    string Glossary,
    string InProgress,
    string EducationalProjectBy,
    string BuiltWith,
    string UnexpectedError,
    string Reload,
    string LanguageLabel);

internal static class UiTextCatalog
{
    private static readonly UiText English = new(
        "Skip to content",
        "Go to home page",
        "Interactive laboratory",
        "Open laboratory",
        "View on GitHub",
        "Main navigation",
        "Show navigation",
        "Explore laboratory",
        "Explore",
        "Home",
        "Maturity roadmap",
        "Laboratories",
        "Prepare for interview",
        ".NET history",
        ".NET ecosystem",
        "Design and operate",
        "Architecture",
        "Azure and scale",
        "Glossary",
        "In progress",
        "An educational project by",
        "Built with .NET 10 and Blazor",
        "An unexpected error occurred.",
        "Reload",
        "Language");

    private static readonly UiText Spanish = new(
        "Saltar al contenido",
        "Ir al inicio",
        "Laboratorio interactivo",
        "Laboratorio abierto",
        "Ver en GitHub",
        "Navegación principal",
        "Mostrar navegación",
        "Explorar laboratorio",
        "Explorar",
        "Inicio",
        "Ruta de madurez",
        "Laboratorios",
        "Prepárate para la entrevista",
        "Historia de .NET",
        "Ecosistema .NET",
        "Diseñar y operar",
        "Arquitectura",
        "Azure y escala",
        "Glosario",
        "En proceso",
        "Un proyecto educativo de",
        "Construido con .NET 10 y Blazor",
        "Ocurrió un error inesperado.",
        "Recargar",
        "Idioma");

    public static UiText Get(AppLanguage language) =>
        language == AppLanguage.Spanish ? Spanish : English;
}
