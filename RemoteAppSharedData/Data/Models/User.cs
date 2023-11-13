using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RemoteApp.Data.Models
{

    [AuditInclude]
    public class User : EntityBase
    {
        private string _userPassword;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [NotMapped]
        public bool PasswordChanged { get; set; }

        [Required]
        public string UserLogin { get; set; }

       
        public string UserPassword
        {
            get { return _userPassword; }
            set { Set(ref _userPassword, value); }
        }

        public string UserNom { get; set; }

        public string UserPrenom { get; set; }

        [NotMapped]
        public string UserNomPrenom => string.Format("{0} {1}", UserNom, UserPrenom);

        public UserStatus UserStatus { get; set; }

        [NotMapped]
        public int UserStatusInt => (int)UserStatus;


        public string UserPhone { get; set; }

       
        public string UserEmail { get; set; }
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }

        
      
       public virtual Role? Role { get; set; }

        public int UserMaxCapacity { get; set; }

        public static implicit operator User(HttpResponseMessage v)
        {
            throw new NotImplementedException();
        }
    }

    public enum UserStatus
    {
        Enabled,
        Desabled
    }
}
