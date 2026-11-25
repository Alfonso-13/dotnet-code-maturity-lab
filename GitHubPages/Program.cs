using DotnetCodeMaturityLab;
using DotnetCodeMaturityLab.Features.AI;
using DotnetCodeMaturityLab.Features.Configuration;
using DotnetCodeMaturityLab.Features.Localization;
using DotnetCodeMaturityLab.Features.Orders;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(SiteOptions.Default);
builder.Services.AddScoped<AppLanguageState>();
builder.Services.AddSingleton<IOrderStore, InMemoryOrderStore>();
builder.Services.AddScoped<CreateOrderService>();
builder.Services.AddSingleton<IAiOrderSuggestionService, SimulatedAiOrderSuggestionService>();
builder.Services.AddSingleton(LabHostingProfile.GitHubPages);
builder.Services.AddScoped<IOrderLabTransport, StaticOrderLabTransport>();
builder.Services.AddScoped<IMiddleLabEvidenceService, StaticMiddleLabEvidenceService>();

await builder.Build().RunAsync();
