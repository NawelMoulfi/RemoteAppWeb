
using Microsoft.EntityFrameworkCore;

using Audit.EntityFramework;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data.Models;

namespace RemoteApp.Data
{
    [AuditDbContext(Mode = AuditOptionMode.OptIn, IncludeEntityObjects = false, AuditEventType = "{database}_{context}")]
    public class ApplicationDbContext : AuditDbContext
    {

      

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

      
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<ModuleAction> ModuleActions { get; set; }
        public DbSet<ModuleActionRole> ModuleActionRoles { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<RapportIntervention> RapportInterventions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Materiel> Materiels { get; set; }
        public DbSet<MaterielRapport> MaterielRapports { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RemoveCascadeDeleteConvention();
            modelBuilder.RemovePluralizingTableNameConvention();
         
        }

    }
    public static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.BaseType == null)
                    entity.SetTableName(entity.DisplayName());
            }
        }



        public static void RemoveCascadeDeleteConvention(this ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);



            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }



    }
}