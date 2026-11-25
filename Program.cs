using DotnetCodeMaturityLab.Components;
using DotnetCodeMaturityLab.Features.AI;
using DotnetCodeMaturityLab.Features.Configuration;
using DotnetCodeMaturityLab.Features.Localization;
using DotnetCodeMaturityLab.Features.Orders;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton(SiteOptions.Default);
builder.Services.AddScoped<AppLanguageState>();
var sqliteDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(sqliteDirectory);
// Keep development keys beside the local database so the educational app does not
// depend on a machine-specific user-profile key store.
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(sqliteDirectory, "keys")));
var sqliteConnectionBuilder = new SqliteConnectionStringBuilder(
    builder.Configuration.GetConnectionString("Orders")
    ?? "Data Source=App_Data/orders.db");
if (!Path.IsPathRooted(sqliteConnectionBuilder.DataSource))
{
    // Resolve relative database paths against the application root. Launchers and test
    // hosts may use a different working directory and must not create a second database.
    sqliteConnectionBuilder.DataSource = Path.Combine(
        builder.Environment.ContentRootPath,
        sqliteConnectionBuilder.DataSource);
}
builder.Services.AddDbContextFactory<OrderDbContext>(options =>
    options.UseSqlite(sqliteConnectionBuilder.ConnectionString));
builder.Services.AddScoped<IOrderStore, SqliteOrderStore>();
builder.Services.AddScoped<CreateOrderService>();
var aiProvider = builder.Configuration["AI:Provider"] ?? "Simulated";
if (!aiProvider.Equals("Simulated", StringComparison.OrdinalIgnoreCase)
    && !aiProvider.Equals("Gemini", StringComparison.OrdinalIgnoreCase))
    throw new InvalidOperationException("AI:Provider must be Simulated or Gemini.");
var realAi = aiProvider.Equals("Gemini", StringComparison.OrdinalIgnoreCase);
if (realAi && string.IsNullOrWhiteSpace(builder.Configuration["AI:ApiKey"]))
    throw new InvalidOperationException("AI:ApiKey is required when AI:Provider is Gemini.");
if (realAi)
{
    // The API key travels in a custom header, so redirects must never forward it
    // to a different host chosen by a provider response.
    builder.Services.AddHttpClient<IAiOrderSuggestionService, GeminiAiOrderSuggestionService>()
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { AllowAutoRedirect = false });
}
else
{
    builder.Services.AddSingleton<IAiOrderSuggestionService, SimulatedAiOrderSuggestionService>();
}
builder.Services.AddSingleton(LabHostingProfile.Server with { HasRealAiProvider = realAi });
builder.Services.AddScoped<IOrderLabTransport, ServerOrderLabTransport>();
builder.Services.AddScoped<IMiddleLabEvidenceService, ServerMiddleLabEvidenceService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    // EnsureCreated keeps the local learning setup self-contained. A production service
    // would use reviewed EF Core migrations instead of creating its schema at startup.
    var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<OrderDbContext>>();
    await using var database = await contextFactory.CreateDbContextAsync();
    await database.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
// The local HTTP-only launcher has no HTTPS endpoint to redirect to. Keep
// redirection on by default outside Development; the HTTPS launch profile opts in.
if (app.Configuration.GetValue("HttpsRedirection:Enabled", !app.Environment.IsDevelopment()))
    app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapOrderEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public partial class Program { }
