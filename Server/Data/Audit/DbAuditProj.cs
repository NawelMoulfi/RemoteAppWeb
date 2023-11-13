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
using RemoteApp.Data.Models;

namespace RemoteApp.Data.Audit
{
    public class DbAuditProj<T> where T : new()
    {
        //[Inject] ILogger<DbAuditProj<T>> _logger { get; set; }
        public DbAuditProj(DbJsonAuditEntry d, ApplicationDbContext db , bool checkColumns = false)
        {
            //_logger = logger;
            Db = db ?? new ApplicationDbContext();
            
            EventEntry = EventEntry.FromJson(d.JsonContent);
            AuditDate = d.Date ?? Convert.ToDateTime(EventEntry.CustomFields["AuditDate"].ToString());
            AuditUserName = Db.Users.Find(d.UserId)?.UserNomPrenom ?? EventEntry.CustomFields["AuditUserName"].ToString();
            AuditMachineName = d.MachineName;
            ChangedColumns = EventEntry.Changes?.Select(r => r.ColumnName).ToList() ?? new List<string>();
              // ChangedColumns = EventEntry.Changes?.Where( c => c.NewValue?.ToString() != c.OriginalValue?.ToString())?.Select(r => r.ColumnName).ToList() ?? new List<string>();
            Id = d.Id;
            EntityId = d.EntityId;
            Entity = new T();
            foreach (var tuple in EventEntry.ColumnValues)
            {
                var type = Entity.GetType();
                var propertyInfo = type.GetProperty(tuple.Key) ?? type.GetProperties().FirstOrDefault(p => p.GetCustomAttribute<ColumnAttribute>()?.Name == tuple.Key);
                if (propertyInfo != null && checkColumns)
                {
                    var attName = propertyInfo.GetCustomAttribute<ColumnAttribute>()?.Name;
                    var i = ChangedColumns?.IndexOf(attName) ?? -1;
                    if (i >= 0)
                        ChangedColumns[i] = propertyInfo.Name;
                }
                TrySetPropertyValue(Entity, propertyInfo, tuple.Value?.ToString());
            }
            TrySetNavigationProperties();
        }
        public long? Id { get; set; }
        public ApplicationDbContext Db { get; set; }
        //public ILogger<DbAuditProj<T>> _logger;
        public long? EntityId { get; set; }
        public T Entity { get; set; }
        public EventEntry EventEntry { get; set; }
        public List<string> ChangedColumns { get; set; }
        // public DbAuditInfo AuditInfo { get; set; }
        public string Action => EventEntry.Action == "Update" ? "Modification"
                                : EventEntry.Action == "Insert" ? "Ajout"
                                : EventEntry.Action == "Delete" ? "Suppression" : "";
        // public DateTime AuditDate => (DateTime)EventEntry.CustomFields["AuditDate"];
        //public DateTime AuditDate => Convert.ToDateTime(EventEntry.CustomFields["AuditDate"].ToString());
        public DateTime? AuditDate { get; set; }
        //public string AuditUserName => (string)EventEntry.CustomFields["AuditUserName"];
        //public string AuditUserName => EventEntry.CustomFields["AuditUserName"].ToString();
        public string AuditUserName { get; set; }
        public string AuditMachineName { get; set; }
        public void TrySetPropertyValue(T entity, PropertyInfo p, object value)
        {
            if (p == null) return;
            try
            {
                var propertyType = p.PropertyType;
                var targetType = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? Nullable.GetUnderlyingType(propertyType) : propertyType;
                value = value == null ? value
                    : targetType.IsEnum ? Enum.Parse(targetType, value.ToString())
                    : targetType == typeof(byte[]) ? Convert.FromBase64String(value.ToString())
                    : value;
                p.SetValue(entity, value == null ? value : Convert.ChangeType(value, targetType));
            }
            catch (Exception exception)
            {
                //_logger.LogError(exception.ToString());
            }
        }

        public void TrySetNavigationProperties()
        {
            var propertyInfos = Entity.GetType().GetProperties();
            var vp = propertyInfos.Where(p => p.GetGetMethod().IsVirtual).ToList();
            foreach (var p in vp)
            {
                try
                {
                    var nameFk = p.Name.ToLower() + "id";
                    var fKPropertyInfo = propertyInfos.FirstOrDefault(p2 => p2.Name.ToLower() == nameFk);
                    if (fKPropertyInfo == null)
                    {
                        nameFk = p.GetCustomAttribute<ForeignKeyAttribute>()?.Name.ToLower();
                        fKPropertyInfo = propertyInfos.FirstOrDefault(p2 => p2.Name.ToLower() == nameFk);
                    }
                    var forginKey = fKPropertyInfo?.GetValue(Entity);
                    if (forginKey == null)
                        continue;
                    // var db = new ClinModel();
                    var pk = GetPrimaryKeys(p.PropertyType, Db).FirstOrDefault();
                    var t = p.PropertyType;
                    var query = $"select * from  [dbo].[{p.PropertyType.Name}] where {pk} = {forginKey}";
                    var vertual = GetVirtual(Db, p.PropertyType, query);

                  
                    p.SetValue(Entity, vertual);
                    //var enumerable = Db..FromSqlRaw(p.PropertyType.ToString(), query, new object[0]);
                    //var enumerable = Db.Database.SqlQuery(p.PropertyType, $"select * from {p.PropertyType.Name} where {pk} = {forginKey}", new object[0]).GetEnumerator();
                    //if (enumerable.MoveNext())
                    //    p.SetValue(Entity, enumerable.Current);
                }
                catch (Exception exception)
                {
                    //_logger.LogError(exception.ToString());
                }
            }
        }

//        private object GetVirtual(ClinModel db, Type pPropertyType, string query)
//        {
//            //if(pPropertyType == typeof(Patient))
//            //    return Db.Patients.FromSqlRaw(pPropertyType.ToString(), query).FirstOrDefault();
//            if (pPropertyType == typeof(Patient))
//                return Db.Patients.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if (pPropertyType == typeof(Analysis))
//                return Db.Analysis.FromSqlRaw(pPropertyType.ToString(), query).FirstOrDefault();
//            if(pPropertyType == typeof(Medicament))
//                return Db.Medicaments.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(Sig))
//                return Db.Sigs.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(Icd10Code))
//                return Db.Icd10Code.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(ActionProcedureType))
//                return Db.ActionProcedureTypes.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(ActionProcedure))
//                return Db.ActionProcedures.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(Priority))
//                return Db.Priorities.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(Service))
//                return Db.Services.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(User)) 
//                return Db.User.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(ImageRequestStatus)) 
//                return Db.ImageRequestStatus.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(MedecinAdressant)) 
//                return Db.MedecinAdressants.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(Procedure)) 
//                return Db.Procedures.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(Measurement)) 
//                return Db.Measurements.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(AnalysisType)) 
//                return Db.AnalysisType.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(AllergyStatus)) 
//                return Db.AllergyStatus.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(SnomedConcept)) 
//                return Db.SnomedConcepts.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(AllergyCriticality)) 
//                return Db.AllergyCriticalities.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(FamilyRelationship)) 
//                return Db.FamilyRelationship.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(SocialHistory)) 
//                return Db.SocialHistories.FromSqlRaw(query).AsEnumerable().FirstOrDefault();
//            if(pPropertyType == typeof(ProcedureType)) 
//                return Db.ProcedureTypes.FromSqlRaw(query).AsEnumerable().FirstOrDefault();

//            if (pPropertyType == typeof(RdvMotif))
//                return Db.RdvMotif.FromSqlRaw(query).AsEnumerable().FirstOrDefault();


//#if DEBUG
//            throw new Exception("Property type not expected !!");
//#else
//            return null;
//#endif
//        }

        private object GetVirtual(ApplicationDbContext db, Type pPropertyType, string query)
        {
               if (pPropertyType == typeof(Role))
                return Db.Roles.FromSqlRaw(query).AsEnumerable().FirstOrDefault();

            if (pPropertyType == typeof(User))
                return Db.Users.FromSqlRaw(query).AsEnumerable().FirstOrDefault();

            if (pPropertyType == typeof(Folder))
                return Db.Folders.FromSqlRaw(query).AsEnumerable().FirstOrDefault();

            if (pPropertyType == typeof(Entry))
                return Db.Entries.FromSqlRaw(query).AsEnumerable().FirstOrDefault();

            return null;
        }

        public static List<string> GetPrimaryKeys(Type type, DbContext db)
        {
            var keyNames = db.Model.FindEntityType(type).FindPrimaryKey().Properties
                .Select(x => x.Name).ToList();
            return keyNames;
        /*    return ((System.Data.Entity.Infrastructure.IObjectContextAdapter)db)
                .ObjectContext.MetadataWorkspace.GetItem<System.Data.Entity.Core.Metadata.Edm.EntityType>(type.FullName, System.Data.Entity.Core.Metadata.Edm.DataSpace.OSpace)
                .KeyProperties.Select(k => k.Name).ToList();*/
        }
    }
}