using HockeyPool.Components;
using HockeyPool.Configuration;
using HockeyPool.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddHockeyPoolAuthentication();
builder.Services.AddHockeyPoolDatabase(builder.Configuration);
builder.Services.AddHockeyPoolIdentity();
builder.Services.AddHockeyPoolAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ClipboardService>();

var app = builder.Build();

app.SetupHttpPipeline();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

await app.SetupHockeyPoolDBAsync();

app.Run();

