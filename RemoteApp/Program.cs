using RemoteApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using RemoteApp.Services.Contracts;
using RemoteApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RemoteApp;
/*var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7023/") });*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7023/"); // Set the base address of your API
    client.Timeout = TimeSpan.FromMinutes(5); // Set the timeout duration as needed

});

builder.Services.AddDbContextPool<ApplicationDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")).EnableSensitiveDataLogging()
    );//Add-Migration InitialCreate and update-database*/
builder.Services.AddDbContextPool<AuditContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("AuditContext")).EnableSensitiveDataLogging()
    );//Add-Migration InitialCreate and update-database*/

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


//IMaterielRapportDataService
builder.Services.AddScoped<IMaterielRapportDataService, MaterielRapportDataService>();


builder.Services.AddBlazoredToast();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();





builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();
//var app;
//
try
{
    var app = builder.Build();
    // ... the rest of your code
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseStaticFiles();

    app.UseRouting();

    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    app.Run();
}
catch (AggregateException aggregateException)
{
   
    foreach (var innerException in aggregateException.InnerExceptions)
    {
        // Handle or log the individual exceptions
        Console.WriteLine($"Inner Exception: {innerException.Message}");
    }
}
//var app = builder.Build();

// Configure the HTTP request pipeline.
