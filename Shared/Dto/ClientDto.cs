using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class ClientDto
    {
        public int ClientId { set; get; }
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }
        [Required]
        public string Wilaya { set; get; }
        [Required]
        public string Adresse { set; get; }
        [Required]
        public string PID { set; get; }
        [Required]
        public string PhoneNumber { set; get; }
    }
}
