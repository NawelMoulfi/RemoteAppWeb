using Audit.EntityFramework;
using RemoteApp.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteApp.Data.Models
{

    [AuditInclude]
    public class ModuleActionRole
    {
        public int ModuleActionRoleId { get; set; }
        [Column("Role_RoleID")]
        public int RoleId { get; set; }
        [Column("ModuleAction_ModuleActionID")]
        public int ModuleActionId { get; set; }
        public virtual Role? Role { get; set; }
       public virtual ModuleAction? ModuleAction { get; set; }
    }
}
