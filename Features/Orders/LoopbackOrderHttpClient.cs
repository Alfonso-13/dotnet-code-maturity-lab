using Microsoft.AspNetCore.Components;

namespace DotnetCodeMaturityLab.Features.Orders;

/// <summary>
/// A server-side laboratory must not turn the request-derived browser origin into
/// an arbitrary outbound destination. Only the local listener may be contacted.
/// </summary>
public static class LoopbackOrderHttpClient
{
    public static HttpClient Create(NavigationManager navigation)
    {
        var origin = new Uri(navigation.BaseUri);
        if (!origin.IsLoopback || origin.Scheme is not ("http" or "https"))
            throw new InvalidOperationException("The local API check requires a loopback URL. Open the app through start-local.cmd.");

        return new HttpClient(new HttpClientHandler { AllowAutoRedirect = false })
        {
            BaseAddress = origin,
            Timeout = TimeSpan.FromSeconds(15)
        };
    }
}
