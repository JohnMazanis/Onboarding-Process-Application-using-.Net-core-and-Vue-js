using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Objects
{
    public class CheckListTemplateQuestionsAndAnswers
    {
        public int? ID { get; set; }
        public int? ChecklistTemplateID { get; set; }
        public string ControlTypeDescr { get; set; }
        public string Descr { get; set; }
        public int? ControlTypeID { get; set; }
        public int? Points { get; set; }
        public int? AchievementID { get; set; }
        public string Answer { get; set; }
        public bool IsSubmitted { get; set; }
        public int? AnswerID { get; set; }
        public int? PlayerID { get; set; }
        public int? QuestionAnswerID { get; set; }
    }
}
