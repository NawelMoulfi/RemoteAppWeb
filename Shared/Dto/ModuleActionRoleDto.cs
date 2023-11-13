using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class ModuleActionRoleDto
    {
        public int ModuleActionRoleId { get; set; }
      
        public int RoleId { get; set; }
        public string RoleRoleName { get; set; }
        public int ModuleActionId { get; set; }
        public int ModuleActionModuleID { get; set; }
        public string ModuleActionModuleModuleNom { get; set; }

        public string ModuleActionModuleModuleGroup { get; set; }
        public Resource ModuleActionResource { get; set; }
        /// <summary>
        ///  public int ResourceInt { get; set; }
        /// </summary>
        public Action ModuleActionAction { get; set; }
        public string? ModuleActionModuleActionNom { get; set; }
       
    }
}
