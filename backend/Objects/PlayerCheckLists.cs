using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Objects
{
    public class PlayerCheckLists
    {
        public int? ID { get; set; }
        public int? PlayerID { get; set; }
        public int? CheckListTemplateID { get; set; }
        public int? IsMain { get; set; }
        public string DateAssigned { get; set; }
        public string DateCompleted { get; set; }
    }
}
