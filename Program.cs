// Ignore Spelling: apikey authorization initialize supabase

using FamilySweepstake;
using FamilySweepstake.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var supabaseUrl = builder.Configuration["Supabase:Url"];
var supabaseKey = builder.Configuration["Supabase:Key"];

builder.Services.AddMudServices(cfg =>
{
    cfg.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.TopRight;
    cfg.SnackbarConfiguration.PreventDuplicates = true;
    cfg.SnackbarConfiguration.NewestOnTop = true;
    cfg.SnackbarConfiguration.ShowCloseIcon = true;
    cfg.SnackbarConfiguration.VisibleStateDuration = 5000;
    cfg.SnackbarConfiguration.HideTransitionDuration = 500;
    cfg.SnackbarConfiguration.ShowTransitionDuration = 500;
    cfg.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
});

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Register caches (CHANGED: all scoped)
builder.Services.AddScoped<TournamentCache>();
builder.Services.AddScoped<FamilyMemberCache>();
builder.Services.AddScoped<TeamCache>();
builder.Services.AddScoped<TeamOwnershipCache>();
builder.Services.AddScoped<CacheService>();

// Register Supabase tournament service (scoped)
builder.Services.AddScoped<ITournamentService>(sp =>
{
    var http = new HttpClient { BaseAddress = new Uri($"{supabaseUrl}/rest/v1/") };
    http.DefaultRequestHeaders.Add("apikey", supabaseKey);
    http.DefaultRequestHeaders.Add("Authorization", $"Bearer {supabaseKey}");

    return new SupabaseTournamentService(
        http,
        sp.GetRequiredService<TournamentCache>()
    );
});

var app = builder.Build();

// Initialize cache service (valid in WASM because scoped == singleton)
var cache = app.Services.GetRequiredService<CacheService>();
await cache.InitializeAsync();

await app.RunAsync();
