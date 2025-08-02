using LeapPlannerApi.Model.Common;
using System.Collections.Generic;

namespace LeapPlannerApi.Model.TaskCard
{
    public class GetAllTaskCardsReply : ReplyBase
    {
        public List<TaskCardDetails> TaskCards { get; set; }
    }
}
