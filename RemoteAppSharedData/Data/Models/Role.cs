
using Audit.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemoteApp.Data.Models
{
    [AuditInclude]
    public class Role : EntityBase
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

        [NotMapped]
        public virtual List<ModuleAction> ? ModuleActions  { get; set; }
    
     ///   public virtual List<ModuleActionRole> ?  ModuleActionRoles { get; set; }
    }
}
