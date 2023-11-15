using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class MaterielRapportDto
    {
        public long MaterielRapportId { set; get; }
        public long Nombre { set; get; }
        public long MaterielId { set; get; }
        public string? MaterielCode { set; get; }
        public string? MaterielPiece { set; get; }
        public long RapportInterventionId { get; set; }
        public DateTime? RapportInterventionCreatedDate { get; set; }
        public long RapportInterventionNum { set; get; }
    }
}
