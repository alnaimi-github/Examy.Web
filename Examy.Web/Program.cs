using Examy.Shared;
using Examy.Shared.Components.Auth;
using Examy.Web;
using Examy.Web.Apis;
using Examy.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;
using AppState = Examy.Shared.AppState;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<QuizAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<QuizAuthStateProvider>());
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();

builder.Services.AddSingleton<IAppState, AppState>().AddSingleton<IStorageService, StorageService>();
builder.Services.AddSingleton<QuizState>();

ConfigureRefit(builder.Services);

var app = builder.Build();

// Initialize the authentication state provider
var authStateProvider = app.Services.GetRequiredService<QuizAuthStateProvider>();
await authStateProvider.InitializeAsync();

await app.RunAsync();

static void ConfigureRefit(IServiceCollection services)
{
    const string ApiBaseUrl = "https://localhost:7270";
    services.AddRefitClient<IAuthApi>()
        .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<ICategoryApi>(GetRefitSettings)
         .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<IQuizApi>(GetRefitSettings)
         .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<IAdminApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    services.AddRefitClient<IStudentQuizApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    static void SetHttpClient(HttpClient httpClient)
    => httpClient.BaseAddress = new Uri(ApiBaseUrl);

    static RefitSettings GetRefitSettings(IServiceProvider sp)
    {
        var authStateProvider = sp.GetRequiredService<QuizAuthStateProvider>();
        return new RefitSettings
        {
            AuthorizationHeaderValueGetter = (_, __)
            => Task.FromResult(
                authStateProvider.User?.Token ?? "")
        };
    }
}