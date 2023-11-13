using Audit.EntityFramework;
using Shared.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RemoteApp.Data.Models
{
    [AuditInclude]
    public class Entry : EntityBase
    {
        public Entry()
            {
            CreatedDate = DateTime.Now;
            }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EntryId { get; set; }

        [Required]
        public string EntryName { get; set; }
        public EntryType EntryType { get; set; }
        public int EntryTypeInt => (int)EntryType;
        public string ID { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
      //  public virtual User CreatedByUser { get; set; }

        public string Address { get; set; }
        public string URL { get; set; }

        public int FolderId { get; set; }

        [ForeignKey("FolderId")]
        public virtual Folder? Folder { get; set; }

        public string Description { get; set; }
        public EntryStatus EntryStatus { get; set; }
        public int EntryStatusInt => (int)EntryStatus;

        public string Command { get; set; }

    }

   
}
