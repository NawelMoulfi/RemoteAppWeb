

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.Configuration;
using System.IO;

namespace RemoteApp.Data
{
    public class AuditContext : DbContext
    {
     public AuditContext()
    {


    }
    public AuditContext(DbContextOptions<AuditContext> options) : base(options)
    {
    }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.RemovePluralizingTableNameConvention();
    }

    
}
}
