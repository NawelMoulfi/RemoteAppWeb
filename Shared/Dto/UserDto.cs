using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }

        [Required]
        public string UserLogin { get; set; }

        public bool PasswordChanged { get; set; }
        public string UserPassword { get; set; }
        public string UserNom { get; set; }

        public string UserPrenom { get; set; }

        [NotMapped]
        public string UserNomPrenom => string.Format("{0} {1}", UserNom, UserPrenom);

        public UserStatus UserStatus { get; set; }

        [NotMapped]
        public int UserStatusInt => (int)UserStatus;
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public int RoleId { get; set; }
        public string RoleRoleName { get; set; }
        public int UserMaxCapacity { get; set; }
    }
    public enum UserStatus
    {
        Enabled,
        Desabled
    }
}
