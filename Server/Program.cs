using RemoteApp.Data;
using RemoteAppApi.Repositories.Contracts;
using RemoteAppApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7001")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// Add services to the container.
/*builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

// Set supported cultures
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("us_english");
    options.SupportedCultures = new List<CultureInfo> { new CultureInfo("us_english") };
    options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("us_english") };
});*/


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextPool<ApplicationDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")).EnableSensitiveDataLogging()
    );//Add-Migration InitialCreate and update-database*/
builder.Services.AddDbContextPool<AuditContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("AuditContext")).EnableSensitiveDataLogging()
    );//Add-Migration InitialCreate and update-database*/
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();

builder.Services.AddScoped<IEntryRepository, EntryRepository>();
builder.Services.AddScoped<IMaterielRepository, MaterielRepository>();
builder.Services.AddScoped<IMaterielRapportRepository, MaterielRapportRepository>();

builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<IModuleActionRepository, ModuleActionRepository>();
builder.Services.AddScoped<IModuleActionRoleRepository, ModuleActionRoleRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRapportInterventionRepository, RapportInterventionRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
