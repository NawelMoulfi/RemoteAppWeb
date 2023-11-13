
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Configuration;
using System.IO;

namespace RemoteApp.Data.Audit
{
    public class AuditContext2 : DbContext
    {
        public AuditContext2() {
        
        
        }
        public AuditContext2(DbContextOptions<AuditContext2> options) : base(options)
        {
        }

        public IConfiguration Configuration { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var config = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json")
                  .Build();

            var connString = config.GetConnectionString("AuditContext");


            optionsBuilder.UseSqlServer(
                connString,
                b => b.MigrationsAssembly("RemoteApp"));
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RemovePluralizingTableNameConvention();
        }

        public virtual DbSet<DbJsonAuditEntry> DbJsonAuditEntries { get; set; }
        public virtual DbSet<MvcActionAuditEntry> MvcActionAuditEntries { get; set; }
        public virtual DbSet<DbPrintAuditEntry> DbPrintAuditEntries { get; set; }
        public virtual DbSet<InstrumentServiceAudit> InstrumentServiceAudits { get; set; }
    }
}