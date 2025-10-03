using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CIPP.Frontend.WASM;
using CIPP.Frontend.WASM.Modules.Authentication;
using CIPP.Frontend.WASM.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddCippAuthentication(builder.Configuration);

await builder.Build().RunAsync();
