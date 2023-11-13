
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
using RemoteApp.Data.Models;

namespace RemoteApp.Data
{
    [AuditDbContext(Mode = AuditOptionMode.OptIn, IncludeEntityObjects = false, AuditEventType = "{database}_{context}")]
    public class ApplicationDbContext : AuditDbContext
    {

      

        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
           // var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            var connString = config.GetConnectionString("ApplicationDbContext");
       //
          //  optionsBuilder.UseSqlServer(
                connString,
              //  b => b.MigrationsAssembly("RemoteApp"));
            optionsBuilder.UseLazyLoadingProxies();
        }
       */
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
       // public DbSet<Customer> Customers { get; set; }
        //public DbSet<TicketType> TicketTypes { get; se
        //public DbSet<Permission> Permissions { get; set; }
        //public DbSet<Attachment> Attachments { get; set; 
        //public DbSet<ModuleAction> ModuleActions { get; set; }
        //public DbSet<ModuleActionRole> ModuleActionRoles { get; set; }
        //public DbSet<AttachTicket> TicketAttachments { get; set; }
        //public DbSet<Attach> Attachs { get; set; }
        //public DbSet<TicketAction> TicketActions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.RemoveCascadeDeleteConvention();
            modelBuilder.RemovePluralizingTableNameConvention();
          
            //modelBuilder.Entity("DataAccess.Models.Attach", b =>
            //{
            //    b.Property<Guid>("AttachID")
            //        .ValueGeneratedOnAdd()
            //        .HasColumnType("uniqueidentifier");

            //    b.Property<string>("AttachChemin")
            //        .HasMaxLength(250)
            //        .HasColumnType("nvarchar(250)");

            //    b.Property<DateTime>("AttachDate")
            //        .HasColumnType("datetime2");



            //    b.Property<string>("AttachNom")
            //        .IsRequired()
            //        .HasColumnType("nvarchar(max)");



            //    b.HasKey("AttachID");

            //    b.ToTable("Attach");


            //});

            //        modelBuilder
            //.Entity<AttachTicket>(
            //    eb =>
            //    {
            //        eb.HasNoKey();
            //          //eb.Ignore(c => c.PriceListe);

            //          eb.ToView(null);
            //          // eb.Property(v => v.ItemName).HasColumnName("Name");
            //      });
            //         modelBuilder
            // .Entity<MaterielRapport>(
            //     eb =>
            //     {
            //         eb.HasNoKey();
            //     });
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