using DotnetCodeMaturityLab;
using DotnetCodeMaturityLab.Features.Orders;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<IOrderStore, InMemoryOrderStore>();
builder.Services.AddScoped<CreateOrderService>();
builder.Services.AddSingleton(LabHostingProfile.GitHubPages);

await builder.Build().RunAsync();
