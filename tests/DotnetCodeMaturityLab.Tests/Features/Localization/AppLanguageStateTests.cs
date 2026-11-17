using DotnetCodeMaturityLab.Features.Localization;

namespace DotnetCodeMaturityLab.Tests.Features.Localization;

public sealed class AppLanguageStateTests
{
    [Fact]
    public void DefaultsToEnglish()
    {
        var state = new AppLanguageState();

        Assert.Equal(AppLanguage.English, state.Current);
        Assert.Equal("en", state.Code);
        Assert.Equal("Home", state.Text.Home);
    }

    [Theory]
    [InlineData("es", AppLanguage.Spanish, "Inicio")]
    [InlineData("ES", AppLanguage.Spanish, "Inicio")]
    [InlineData("en", AppLanguage.English, "Home")]
    [InlineData(null, AppLanguage.English, "Home")]
    [InlineData("unsupported", AppLanguage.English, "Home")]
    public void SelectsOnlySupportedLanguages(
        string? languageCode,
        AppLanguage expectedLanguage,
        string expectedHomeLabel)
    {
        var state = new AppLanguageState();

        state.Set(languageCode);

        Assert.Equal(expectedLanguage, state.Current);
        Assert.Equal(expectedHomeLabel, state.Text.Home);
    }
}
