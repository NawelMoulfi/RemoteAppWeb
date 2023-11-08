using System.ComponentModel.DataAnnotations.Schema;

namespace RemoteApp.Data.Models;

public class Materiel
{
    public long MaterielId { set; get; }
    public string Code { set; get; }
    public string  Piece { set; get; }
}

public class MaterielRapport
{
    public long MaterielRapportId { set; get; }
    public long MaterielId { set; get; }
    [ForeignKey("MaterielId")]
    public virtual  Materiel? Materiel { set; get; }
    
    public long RapportInterventionId { set; get; }
    [ForeignKey("RapportInterventionId")]
    public virtual  RapportIntervention? RapportIntervention { set; get; }
    
    public long Nombre { set; get; }
}