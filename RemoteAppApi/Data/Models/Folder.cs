using Audit.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemoteApp.Data.Models
{

    [AuditInclude]
    public class Folder : EntityBase
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FolderId { get; set; }

        [Required]
        public string FolderName { get; set; }

        public int? ParentFolderId { get; set; }

        [ForeignKey("ParentFolderId")]
      // public virtual Folder ParentFolder { get; set; }

        public string FolderDescription { get; set; }
        public FolderStatus FolderStatus { get; set; }

        [NotMapped]
        public int FolderStatusInt => (int)FolderStatus;
    }

    public enum FolderStatus
    {
        Enabled,
        Desabled
    }
}
