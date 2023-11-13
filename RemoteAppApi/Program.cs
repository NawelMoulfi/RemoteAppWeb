using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteAppApi.Repositories.Contracts;
using RemoteAppApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.WithOrigins("https://localhost:7087");
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7087")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


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
var app = builder.Build();

// Configure the HTTP request pipeline.

//string RemoteAppConn = builder.Configuration.GetConnectionString("ApplicationDbContext");
//string RemoteAppAuditConn = builder.Configuration.GetConnectionString("AuditContext");

//builder.Services.AddHttpClient();

//services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

/*builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(RemoteAppConn, b => b.MigrationsAssembly("RemoteApp"));
}
);
builder.Services.AddDbContext<AuditContext>(options =>
{
    options.UseSqlServer(
        RemoteAppAuditConn, b => b.MigrationsAssembly("RemoteApp"));
}
);*/

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
