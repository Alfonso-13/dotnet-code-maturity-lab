using DotnetCodeMaturityLab.Components;
using DotnetCodeMaturityLab.Features.Orders;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
var sqliteDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(sqliteDirectory);
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(sqliteDirectory, "keys")));
var sqliteConnectionBuilder = new SqliteConnectionStringBuilder(
    builder.Configuration.GetConnectionString("Orders")
    ?? "Data Source=App_Data/orders.db");
if (!Path.IsPathRooted(sqliteConnectionBuilder.DataSource))
{
    sqliteConnectionBuilder.DataSource = Path.Combine(
        builder.Environment.ContentRootPath,
        sqliteConnectionBuilder.DataSource);
}
builder.Services.AddDbContextFactory<OrderDbContext>(options =>
    options.UseSqlite(sqliteConnectionBuilder.ConnectionString));
builder.Services.AddScoped<IOrderStore, SqliteOrderStore>();
builder.Services.AddScoped<CreateOrderService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
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
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapOrderEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

public partial class Program { }
