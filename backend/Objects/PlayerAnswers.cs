using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Objects
{
    public class PlayerAnswers
    {
        public int ChecklistTemplateQuestionID { get; set; }
        public string Answer { get; set; }
        public bool IsAnswered { get; set; }
        public int PlayerID { get; set; }
        public int CheckListTemplateID { get; set; }
    }
}
