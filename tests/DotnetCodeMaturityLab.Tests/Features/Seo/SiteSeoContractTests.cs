using System.Xml.Linq;
using DotnetCodeMaturityLab.Features.Navigation;
using DotnetCodeMaturityLab.Features.Seo;

namespace DotnetCodeMaturityLab.Tests.Features.Seo;

public sealed class SiteSeoContractTests
{
    private static readonly string RepositoryRoot = FindRepositoryRoot();

    [Fact]
    public void HostDocuments_ExposeTheSharedSearchAndSocialIdentity()
    {
        var serverDocument = ReadRepositoryFile("Components", "App.razor");
        var pagesDocument = ReadRepositoryFile("GitHubPages", "wwwroot", "index.html");

        Assert.Contains("@SiteSeo.Title", serverDocument, StringComparison.Ordinal);
        Assert.Contains("@SiteSeo.Description", serverDocument, StringComparison.Ordinal);
        Assert.Contains("@SiteSeo.SocialImageUrl", serverDocument, StringComparison.Ordinal);
        Assert.Contains("summary_large_image", serverDocument, StringComparison.Ordinal);

        Assert.Contains(SiteSeo.Title, pagesDocument, StringComparison.Ordinal);
        Assert.Contains(SiteSeo.Description, pagesDocument, StringComparison.Ordinal);
        Assert.Contains(SiteSeo.PublicUrl, pagesDocument, StringComparison.Ordinal);
        Assert.Contains(SiteSeo.SocialImageUrl, pagesDocument, StringComparison.Ordinal);
        Assert.Contains("summary_large_image", pagesDocument, StringComparison.Ordinal);
        Assert.Contains($"content=\"{SiteSeo.SocialImageWidth}\"", pagesDocument, StringComparison.Ordinal);
        Assert.Contains($"content=\"{SiteSeo.SocialImageHeight}\"", pagesDocument, StringComparison.Ordinal);
    }

    [Fact]
    public void DiscoveryFiles_UseCanonicalEnglishFirstRoutes()
    {
        var robots = ReadRepositoryFile("wwwroot", "robots.txt");
        var sitemapPath = Path.Combine(RepositoryRoot, "wwwroot", "sitemap.xml");
        var sitemap = XDocument.Load(sitemapPath);
        XNamespace sitemapNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var locations = sitemap
            .Descendants(sitemapNamespace + "loc")
            .Select(element => element.Value)
            .ToHashSet(StringComparer.Ordinal);

        Assert.Contains($"Sitemap: {SiteSeo.PublicUrl}sitemap.xml", robots, StringComparison.Ordinal);
        Assert.Contains(SiteSeo.PublicUrl, locations);
        Assert.Contains($"{SiteSeo.PublicUrl}{AppRoutes.Roadmap}", locations);
        Assert.Contains($"{SiteSeo.PublicUrl}{AppRoutes.Labs}", locations);
        Assert.Contains($"{SiteSeo.PublicUrl}{AppRoutes.Interview}", locations);

        foreach (var canonicalSlug in AppRoutes.JuniorModuleSlugs.Values)
        {
            Assert.Contains($"{SiteSeo.PublicUrl}learn/junior/{canonicalSlug}", locations);
        }

        Assert.DoesNotContain(locations, location => location.Contains("fundamentos-csharp", StringComparison.Ordinal));
    }

    [Fact]
    public void SocialPreview_HasTheDeclaredOpenGraphDimensions()
    {
        var imagePath = Path.Combine(
            RepositoryRoot,
            "wwwroot",
            "images",
            "dotnet-code-maturity-lab-social.jpg");

        var (width, height) = ReadJpegDimensions(imagePath);

        Assert.Equal(SiteSeo.SocialImageWidth, width);
        Assert.Equal(SiteSeo.SocialImageHeight, height);
    }

    private static string ReadRepositoryFile(params string[] segments) =>
        File.ReadAllText(Path.Combine([RepositoryRoot, .. segments]));

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "DotnetCodeMaturityLab.slnx")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate the repository root from the test output directory.");
    }

    private static (int Width, int Height) ReadJpegDimensions(string imagePath)
    {
        using var stream = File.OpenRead(imagePath);
        using var reader = new BinaryReader(stream);

        if (reader.ReadByte() != 0xFF || reader.ReadByte() != 0xD8)
        {
            throw new InvalidDataException("The social preview is not a JPEG image.");
        }

        while (stream.Position < stream.Length)
        {
            if (reader.ReadByte() != 0xFF)
            {
                continue;
            }

            byte marker;
            do
            {
                marker = reader.ReadByte();
            }
            while (marker == 0xFF);

            if (marker is 0xD8 or 0xD9)
            {
                continue;
            }

            var segmentLength = ReadBigEndianUInt16(reader);
            if (segmentLength < 2)
            {
                throw new InvalidDataException("The social preview contains an invalid JPEG segment.");
            }

            // Start-of-frame segments contain the encoded dimensions. Reading the JPEG
            // header avoids platform-specific image libraries in Linux CI runners.
            if (marker is >= 0xC0 and <= 0xC3)
            {
                _ = reader.ReadByte();
                var height = ReadBigEndianUInt16(reader);
                var width = ReadBigEndianUInt16(reader);
                return (width, height);
            }

            stream.Seek(segmentLength - 2, SeekOrigin.Current);
        }

        throw new InvalidDataException("The social preview does not contain JPEG dimensions.");
    }

    private static ushort ReadBigEndianUInt16(BinaryReader reader)
    {
        var high = reader.ReadByte();
        var low = reader.ReadByte();
        return (ushort)((high << 8) | low);
    }
}
