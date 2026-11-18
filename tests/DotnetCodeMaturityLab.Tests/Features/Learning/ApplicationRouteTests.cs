using DotnetCodeMaturityLab.Features.Navigation;

namespace DotnetCodeMaturityLab.Tests.Features.Learning;

public sealed class ApplicationRouteTests
{
    [Fact]
    public void CanonicalRoutes_AreEnglishAndHostRelative()
    {
        var routes = new[]
        {
            AppRoutes.Roadmap,
            AppRoutes.Labs,
            AppRoutes.DotNetHistory,
            AppRoutes.DotNetEcosystem,
            AppRoutes.Interview,
            AppRoutes.InterviewFor("junior"),
            AppRoutes.Lab("junior"),
            AppRoutes.ComingSoon("architecture")
        }.Concat(AppRoutes.JuniorModuleSlugs.Keys.Select(AppRoutes.LearnJunior));

        Assert.All(routes, route =>
        {
            Assert.False(route.StartsWith('/'), $"{route} must remain relative to the configured base path.");
            Assert.DoesNotContain("aprender", route, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("laboratorio", route, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("historia", route, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("ecosistema", route, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("en-preparacion", route, StringComparison.OrdinalIgnoreCase);
        });
    }

    [Fact]
    public void JuniorModuleSlugs_RoundTripBetweenInternalIdsAndCanonicalRoutes()
    {
        foreach (var pair in AppRoutes.JuniorModuleSlugs)
        {
            Assert.Equal(pair.Value, AppRoutes.CanonicalModuleSlug(pair.Key));
            Assert.Equal(pair.Key, AppRoutes.InternalModuleSlug(pair.Value));
        }
    }

}
