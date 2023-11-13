using Audit.EntityFramework;
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
        public Action Action { get; set; }
        public string? ModuleActionNom { get; set; }
        [JsonIgnore]
        public virtual Module? Module { get; set; }

        //[NotMapped]
        //public virtual List<Role> Roles
        //{
        //    get
        //    {
        //        return ModuleActionRoles.Select(r => r.Role).ToList();
        //    }
        //}
        //public virtual List<ModuleActionRole> ModuleActionRoles { get; set; }
    }
    public enum Resource
    {
        [Display(Name = "Utilisateurs")]
        [Description("Utilisateurs")]
        Utilisateurs = 1,

        [Display(Name = "Roles")]
        [Description("Roles")]
        Roles = 2,

        [Display(Name = "Dossiers")]
        [Description("Dossiers")]
        Folders = 3,

        [Display(Name = "Entrées")]
        [Description("Entrées")]
        Entries = 4,

    }

    public enum Action
    {
        [Display(Name = "Lecture")]
        [Description("Lecture")]
        Lecture = 1,

        [Display(Name = "Creation")]
        [Description("Creation")]
        Creation = 2,

        [Display(Name = "Modification")]
        [Description("Modification")]
        Modification = 3,

        [Display(Name = "Suppression")]
        [Description("Suppression")]
        Suppression = 4,

    }
}
