using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteAppDtos
{
    public class ModuleActionRoleDto
    {
        public int ModuleActionRoleId { get; set; }
      
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int ModuleActionId { get; set; }
        public int? ModuleID { get; set; }
        public string ModuleNom { get; set; }

        public string ModuleGroup { get; set; }
        public int Resource { get; set; }
        public int Action { get; set; }
        public string? ModuleActionNom { get; set; }
        public ModuleDto Module { get; set; }
        public  RoleDto Role { get; set; }
        public  ModuleActionDto ModuleAction { get; set; }
    }
}
