using Audit.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RemoteApp.Data.Models;

[AuditInclude]
public class RapportIntervention : EntityBase
{
    public RapportIntervention()
    {
        CreatedDate = DateTime.Now;
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RapportId { get; set; }
    public DateTime? CreatedDate { get; set; }
    
    [Required]
    public Operation Operation { set; get; }
    
    public string CommentaireTraveaux { set; get; }
    
    public string AutreInformation { set; get; }
    [Required]
    public long Num { set; get; }
    
    [Required]
    public Logiciel Logiciel { set; get; }
    
   [Required]
   [ForeignKey("CreatedByUserId")]
   public int CreatedByUserId { set; get; }
 public virtual  User ? CreatedByUser { set; get; }
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
