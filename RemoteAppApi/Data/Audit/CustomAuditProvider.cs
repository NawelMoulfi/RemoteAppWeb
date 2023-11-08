using Audit.Core;
using Audit.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using RemoteApp.Data.Models;
using RemoteApp.Data.Audit;

namespace RemoteApp.Data.Audit
{
    public class CustomAuditProvider : AuditDataProvider
    {
        private IHttpContextAccessor _httpContextAccessor;

        private ILogger<CustomAuditProvider> _logger;
        public static Dictionary<string, string> RelatedEntitiesMapping = new Dictionary<string, string>
        {
            {nameof(Role),null},
            {nameof(User),null},
            {nameof(Folder),null},
            {nameof(Entry),"FolderId"},
        };

        public override object InsertEvent(AuditEvent auditEvent)
        {
            //var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            Task.Run(() =>
            {
                try
                {
                    if (auditEvent is AuditEventEntityFramework)
                        InsertEvent((AuditEventEntityFramework)auditEvent);
                    else if (auditEvent.EventType == "Login" )
                        InsertMvcActionAuditEntry(auditEvent);
                    //else if (auditEvent is AuditEventMvcAction)
                    //    InsertEvent((AuditEventMvcAction)auditEvent);
                    //else if (auditEvent.EventType == "PrintAuditEntry")
                    //    InsertPrintAuditEntry(auditEvent);
                    //else if (auditEvent.EventType == "InstrumentServiceAudit")
                    //    InsertInstrumentServiceAudit(auditEvent);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                    throw new Exception($"audit problem : {e.ToString()}");
                    //Hubs.NotificationHub.Alert_Error(Helper.Error(e).Data);
                }
            });
            return null;
        }

        public CustomAuditProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            //_logger = logger;
        }
        /**/
        public void InsertEvent(AuditEventEntityFramework auditEvent)
        {
            var auditDb = new AuditContext2();
            var userName = "";
            var userId = "";
            var machineName = GetMachineName();
            if (_httpContextAccessor.HttpContext?.User?.Identity != null)
            {
                userName = _httpContextAccessor.HttpContext.User.Identity.Name;
                userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            }

            foreach (var entry in auditEvent.EntityFrameworkEvent.Entries)
            {
             

                entry.CustomFields.Add("AuditDate", DateTime.Now);
                entry.CustomFields.Add("AuditUserName", userName);
                entry.CustomFields.Add("AuditUserId", userId);

                var relatedEntityIdObject = !RelatedEntitiesMapping.ContainsKey(entry.Table) || (RelatedEntitiesMapping[entry.Table] == null) || !entry.ColumnValues.ContainsKey(RelatedEntitiesMapping[entry.Table])
                    ? null : entry.ColumnValues[RelatedEntitiesMapping[entry.Table]];
                var relatedEntityId = relatedEntityIdObject == null ? (long?)null
                    : (long)Convert.ChangeType(relatedEntityIdObject, typeof(long));
                var entityId = (long)Convert.ChangeType(entry.PrimaryKey.Values.ElementAt(0), typeof(long));
                var r = auditDb.DbJsonAuditEntries.Add(new DbJsonAuditEntry
                {
                    Type = entry.Table,
                    EntityId = entityId,
                    RelatedEntityId = relatedEntityId,
                    Date = DateTime.Now,
                    MachineName = machineName,
                    UserId = Convert.ToInt32(userId),
                    JsonContent = entry.ToJson(),
                });
                auditDb.SaveChangesAsync();
            }
        }



        private void InsertMvcActionAuditEntry(AuditEvent auditEvent)
        {
            var auditDb = new AuditContext2();
            var userName = "";
            var userId = 0;
            var machineName = auditEvent.CustomFields["MachineName"]?.ToString();
            if (_httpContextAccessor.HttpContext?.User?.Identity != null)
            {
                userName = _httpContextAccessor.HttpContext.User.Identity.Name;
                Int32.TryParse(_httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out userId);

            }

            auditEvent.CustomFields.Add("UserName", userName);

            auditDb.MvcActionAuditEntries.Add(new MvcActionAuditEntry
            {
                UserId = Convert.ToInt32(userId),
                JsonContent = auditEvent.ToJson(),
                Date = DateTime.Now,
                MachineName = machineName,
                Type = auditEvent.EventType,


            });
            auditDb.SaveChanges();
        }
        /**/

        public string GetMachineName()
        {

            string machineName = "";
            try
            {
                var address = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (address != null)
                {
                    var hostEntry = System.Net.Dns.GetHostEntry(address);
                    if (hostEntry != null && !hostEntry.HostName.Trim().ToLower().Contains("domain.name"))
                    {
                        machineName = hostEntry.HostName;
                    }
                }

                return machineName;
            }
            catch (Exception e)
            {

                return "";
            }


        }


        public override void ReplaceEvent(object eventId, AuditEvent auditEvent)
        {
            //if (auditEvent is AuditEventEntityFramework)
            //{
            //    _provider1.ReplaceEvent(eventId, auditEvent);
            //}
            //else if (auditEvent is AuditEventMvcAction)
            //{
            //    _provider2.ReplaceEvent(eventId, auditEvent);
            //}
        }

    }
}