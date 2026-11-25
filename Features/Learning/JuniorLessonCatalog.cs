using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Features.Learning;

public static partial class JuniorLessonCatalog
{
    // Keep each module's two languages registered together so a new topic cannot
    // appear in one language without the other.
    private static readonly IReadOnlyDictionary<string, (ModuleLesson English, ModuleLesson Spanish)> Content =
        new Dictionary<string, (ModuleLesson English, ModuleLesson Spanish)>(StringComparer.OrdinalIgnoreCase)
        {
            ["fundamentos-csharp"] = (FundamentalsEnglish(), FundamentalsSpanish()),
            ["metodos-y-clases"] = (MethodsAndClassesEnglish(), MethodsAndClassesSpanish()),
            ["modelos-y-memoria"] = (ModelsAndMemoryEnglish(), ModelsAndMemorySpanish()),
            ["poo"] = (OopEnglish(), OopSpanish()),
            ["errores-y-validacion"] = (ErrorsAndValidationEnglish(), ErrorsAndValidationSpanish()),
            ["colecciones-y-genericos"] = (CollectionsAndGenericsEnglish(), CollectionsAndGenericsSpanish()),
            ["linq"] = (LinqEnglish(), LinqSpanish()),
            ["task-async-await"] = (TaskAsyncEnglish(), TaskAsyncSpanish()),
            ["pruebas-aaa"] = (TestingAaaEnglish(), TestingAaaSpanish()),
            ["rest-http"] = (RestHttpEnglish(), RestHttpSpanish()),
            ["persistencia-ef-core"] = (PersistenceEfCoreEnglish(), PersistenceEfCoreSpanish()),
            ["aspnet-core"] = (AspNetCoreEnglish(), AspNetCoreSpanish()),
            ["ia-llm"] = (AiLlmEnglish(), AiLlmSpanish())
        };

    public static ModuleLesson Get(string slug) => Get(slug, AppLanguage.Spanish);

    public static ModuleLesson Get(string slug, AppLanguage language)
    {
        var content = Content[slug];
        var module = language == AppLanguage.English ? content.English : content.Spanish;
        return WithCodeVariants(module, language);
    }

    private static ModuleLesson Module(string slug, params LessonPerspective[] steps) => new(slug, steps);

    private static LessonPerspective Step(string name, string label, string caption, LessonCodePane left, LessonCodePane? right, string hintTitle, string hintBody, string hintExample) =>
        new(name, label, caption, left, right, hintTitle, hintBody, hintExample);

    private static LessonCodePane Pane(string fileName, string label, string code) => new(fileName, label, code);
    private static LessonCodePane Bad(string fileName, string label, string code) => new(fileName, label, code, "bad-practice");
    private static LessonCodePane Good(string fileName, string label, string code) => new(fileName, label, code, "good-practice");
}
