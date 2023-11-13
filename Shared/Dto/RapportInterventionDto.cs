using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class RapportInterventionDto
    {
        public long RapportId { get; set; }
        public DateTime? CreatedDate { get; set; }
        [Required]
        public long Num { set; get; }

        [Required]
        public Operation Operation { set; get; }
      

        public string CommentaireTraveaux { set; get; }

        public string AutreInformation { set; get; }
        public Logiciel Logiciel { set; get; }
        public string CreatedByUserUserNom { get; set; }
        public int CreatedByUserId { get; set; }

    }
    public enum Operation
    {
        InstallationLogiciel,
        MiseEnMarche,
        Assistance,
        InstallationAutomate,
        Formation,
    }

    public enum Logiciel
    {
        BmVie,
        BmLab,
    }
}
