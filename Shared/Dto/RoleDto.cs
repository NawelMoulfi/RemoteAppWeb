using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class RoleDto
    {
        
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }

       
       
    }
}

