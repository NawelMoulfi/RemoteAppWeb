using System.ComponentModel.DataAnnotations;

namespace Shared.Dto
{
    public class ModuleDto
    {

        public int ModuleID { get; set; }

        [Required]
        public string ModuleNom { get; set; }

        public string ModuleGroup { get; set; }

      //  public List<ModuleActionDto>? ModuleActions { get; set; }

    }
}