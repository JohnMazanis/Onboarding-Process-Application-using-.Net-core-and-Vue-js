using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Objects
{
    public class CheckListTemplateItem
    {
        public int? ID { get; set; }
        public string Descr { get; set; }
        public int? Category { get; set; }
        public bool Active { get; set; }
        public List<CheckListTemplateQuestions> Questions { get; set; }
    }
}
