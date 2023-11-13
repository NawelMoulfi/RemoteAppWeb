using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class ModuleActionDto
    {
        public int ModuleActionID { get; set; }
        public int? ModuleID { get; set; }
        public string ModuleModuleNom { get; set; }

        public string ModuleModuleGroup { get; set; }
        public Resource Resource { get; set; }
        public Action Action { get; set; }
        public string? ModuleActionNom { get; set; }
      

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
