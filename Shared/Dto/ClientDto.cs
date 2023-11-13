using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    public class ClientDto
    {
        public int ClientId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Wilaya { set; get; }
        public string Adresse { set; get; }
        public string PID { set; get; }
        public string PhoneNumber { set; get; }
    }
}
