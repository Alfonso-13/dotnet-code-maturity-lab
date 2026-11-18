namespace DotnetCodeMaturityLab.Features.Localization;

public sealed record LocalizedText(string English, string Spanish)
{
    public string Get(AppLanguage language) =>
        language == AppLanguage.Spanish ? Spanish : English;
}
