using LeapPlannerApi.Model.Common;
using System.Collections.Generic;

namespace LeapPlannerApi.Model.Task
{
    public class GetAllTaskReply : ReplyBase
    {
        public List<TaskDetails> Tasks { get; set; }
    }
}
