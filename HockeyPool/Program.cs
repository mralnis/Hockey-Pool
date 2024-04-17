using HockeyPool.Components;
using HockeyPool.Configuration;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddHockeyPoolAuthentication();
builder.Services.AddHockeyPoolDatabase(builder.Configuration);
builder.Services.AddHockeyPoolIdentity();
builder.Services.AddHockeyPoolAuthorization();

var app = builder.Build();

app.SetupHttpPipeline();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.SetupHockeyPoolDB();

app.Run();

