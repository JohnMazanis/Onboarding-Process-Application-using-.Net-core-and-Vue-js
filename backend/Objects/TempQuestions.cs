using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Objects
{
    public class TempQuestions
    {
        public int? ID { get; set; }
        public int? CheckListTemplateID { get; set; }
        public string Descr { get; set; }
        public int? ControlTypeID { get; set; }
        public int? Points { get; set; }
        public int? AchievementID { get; set; }
    }
}
