using Client.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DevExpress.Blazor;
using RemoteAppWeb.Services.Contracts;
using RemoteAppWeb.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using RemoteAppWeb.Areas.Identity;

//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7149/") });
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = BootstrapVersion.v5);

builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<IClientDataService, ClientDataService>();
builder.Services.AddScoped<IEntryDataService, EntryDataService>();
builder.Services.AddScoped<IFolderDataService, FolderDataService>();
builder.Services.AddScoped<IMaterielDataService, MaterielDataService>();
builder.Services.AddScoped<IRoleDataService, RoleDataService>();
builder.Services.AddTransient<CheckPermissionService>();
builder.Services.AddScoped<IRapportDataService, RapportDataService>();
builder.Services.AddScoped<IModuleActionDataService, ModuleActionDataService>();
builder.Services.AddScoped<IModuleActionRoleDataService, ModuleActionRoleDataService>();
builder.Services.AddScoped<IModuleDataService, ModuleDataService>();
builder.Services.AddScoped<IMaterielRapportDataService, MaterielRapportDataService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationCore();


await builder.Build().RunAsync();
