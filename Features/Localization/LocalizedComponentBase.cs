using Microsoft.AspNetCore.Components;

namespace DotnetCodeMaturityLab.Features.Localization;

public abstract class LocalizedComponentBase : ComponentBase
{
    [CascadingParameter]
    protected AppLanguageState Language { get; set; } = new();

    protected string T(string english, string spanish) =>
        Language.Current == AppLanguage.Spanish ? spanish : english;
}
