using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteAppDtos
{
    public class ModuleActionDto
    {
        public int ModuleActionID { get; set; }
        public int? ModuleID { get; set; }
        public string ModuleNom { get; set; }

        public string ModuleGroup { get; set; }
        public int Resource { get; set; }
        public int Action { get; set; }
        public string? ModuleActionNom { get; set; }
        public  ModuleDto Module { get; set; }
    }
}
