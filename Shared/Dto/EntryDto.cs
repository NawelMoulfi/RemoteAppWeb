using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class EntryDto
    {
        public long EntryId { get; set; }

        [Required]
        public string EntryName { get; set; }
        [Required]
        public EntryType EntryType { get; set; }
        public int EntryTypeInt => (int)EntryType;
        public string? ID { get; set; }
 
        public string? Password { get; set; }
        [Required]
        public int? CreatedByUserId { get; set; }
        public string? CreatedByUserUserNom { get; set; }

        public string? CreatedByUserUserPrenom { get; set; }
        [NotMapped]
        public string? CreatedByUserUserNomPrenom => string.Format("{0} {1}", CreatedByUserUserNom, CreatedByUserUserPrenom);
        public string? Address { get; set; }
        public string  ? URL { get; set; }
        public string? Description { get; set; }
        public string? Command { get; set; }
        [Required]
        public EntryStatus EntryStatus { get; set; }
        public int EntryStatusInt => (int)EntryStatus;
        [Required]
        public int? FolderId { get; set; }

        
        public string? FolderFolderName { get; set; }

    }
    public enum EntryType
    {
        AnyDesk,
        Rustdesk,
        Password,
        ILO,
        Automate,
        Convertisseur
    }

    public enum EntryStatus
    {
        Enabled,
        Desabled,
    }
}
