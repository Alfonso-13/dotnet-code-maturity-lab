namespace DotnetCodeMaturityLab.Tests.Ui;

public sealed class LearningPathAnimationContractTests
{
    private static readonly string RepositoryRoot = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "../../../../.."));

    [Fact]
    public void Rails_RunFromCenterToCenterBehindTheBadges()
    {
        string css = ReadAppCss();

        Assert.Contains("width: 33.333%", css);
        Assert.Contains(".rail-junior-middle { left: 16.666%; }", css);
        Assert.Contains(".rail-middle-senior { left: 50%; }", css);
    }

    [Fact]
    public void ReceiverGlow_StartsOnlyAfterItsRailFinishes()
    {
        string css = ReadAppCss();

        Assert.Contains("20% { opacity: 1; transform: scaleX(1); }", css);
        Assert.Contains("22% { opacity: .2; transform: scale(1); }", css);
        Assert.Contains("40% { opacity: 1; transform: scaleX(1); }", css);
        Assert.Contains("42% { opacity: .25; transform: scale(1); }", css);
    }

    [Fact]
    public void MobileUsesTheSameRailBeforeGlowSequence()
    {
        string css = ReadAppCss();

        Assert.Contains("@keyframes white-route-beam-mobile", css);
        Assert.Contains("20% { opacity: 1; transform: scaleY(1); }", css);
        Assert.Contains("@keyframes red-route-beam-mobile", css);
        Assert.Contains("40% { opacity: 1; transform: scaleY(1); }", css);
    }

    [Fact]
    public void CompletedRailsRemainVisibleUntilTheCycleRestarts()
    {
        string css = ReadAppCss();

        Assert.Contains("22%,99.99% { opacity: .32; transform: scaleX(1); }", css);
        Assert.Contains("22%,99.99% { opacity: .32; transform: scaleY(1); }", css);
        Assert.Contains("42%,99.99% { opacity: .32; transform: scaleX(1); }", css);
        Assert.Contains("42%,99.99% { opacity: .32; transform: scaleY(1); }", css);
    }

    private static string ReadAppCss()
    {
        string path = Path.Combine(RepositoryRoot, "wwwroot", "app.css");
        return File.ReadAllText(path);
    }
}
