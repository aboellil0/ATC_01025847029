using Blazored.LocalStorage;
using EventBookingSystem.Webpage;
using EventBookingSystem.Webpage.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IClientAuthService, ClientAuthService>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddHttpClient<EventService>(client =>
{
    // This should match the URL shown in Postman
    client.BaseAddress = new Uri("https://localhost:7050/");
});


//builder.Services.AddOidcAuthentication(options =>
//{
//    // Configure your authentication provider options here.  
//    // For more information, see https://aka.ms/blazor-standalone-auth  
//    builder.Configuration.Bind("Local", options.ProviderOptions);
//});
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationHeaderHandler>();

await builder.Build().RunAsync();
