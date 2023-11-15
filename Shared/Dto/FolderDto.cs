using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class FolderDto
    {
        public int FolderId { get; set; }

        [Required]
        public string FolderName { get; set; }
       
        public int? ParentFolderId { get; set; }

        public string? ParentFolderFolderName { get; set; }
       
        public string? FolderDescription { get; set; }
        [Required]
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
