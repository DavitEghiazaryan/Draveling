using System.Globalization;
using Draveling.API;
using Draveling.API.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddLocalization();
builder.Services.AddScoped<AppLanguage>();

var host = builder.Build();

// The language comes from the URL prefix (/hy/..., /ru/...). index.html passes the same
// value to the runtime at startup so the matching resources are loaded; this keeps the
// .NET culture in sync with it.
var language = host.Services.GetRequiredService<AppLanguage>().Code;
var culture = CultureInfo.GetCultureInfo(language);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
