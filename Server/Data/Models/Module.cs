using Audit.EntityFramework;
using Shared.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RemoteApp.Data.Models
{

    [AuditInclude]
    public class Module
    {
        public int ModuleID { get; set; }

        [Required]
        public string ModuleNom { get; set; }

        public string ModuleGroup { get; set; }

        public virtual List<ModuleAction>? ModuleActions { get; set; }

       
    }

    public class ModuleAction
    {
        public int ModuleActionID { get; set; }
        public int? ModuleID { get; set; }
        public Resource Resource { get; set; }
        public Shared.Dto.Action Action { get; set; }
        public string? ModuleActionNom { get; set; }
        [JsonIgnore]
        public virtual Module? Module { get; set; }
    }
   
}
