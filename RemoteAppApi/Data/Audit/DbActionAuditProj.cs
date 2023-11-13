using Audit.EntityFramework;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace RemoteApp.Data.Audit
{
    public class DbActionAuditProj
    {
        //[Inject] ILogger<DbAuditProj<T>> _logger { get; set; }
        public DbActionAuditProj(MvcActionAuditEntry d, ApplicationDbContext db , bool checkColumns = false)
        {
            //_logger = logger;
            Db = db ?? new ApplicationDbContext();

            EventEntry = EventEntry.FromJson(d.JsonContent);
            AuditDate = d.Date ;
            AuditUserName = Db.Users.Find(d.UserId)?.UserNomPrenom ;
            AuditMachineName = d.MachineName;
            Id = d.Id;
            Type = d.Type;

        }
        public long? Id { get; set; }
        public ApplicationDbContext Db { get; set; }
        public EventEntry EventEntry { get; set; }

        public DateTime? AuditDate { get; set; }
        public string AuditUserName { get; set; }
        public string AuditMachineName { get; set; }
        public string Type { get; set; }


    }

    //     

    //#if DEBUG
    //            throw new Exception("Property type not expected !!");
    //#else
    //            return null;
    //#endif
    //        }


}