using LeapPlannerApi.Model.Common;
using LeapPlannerApi.Model.Planner.model;
using System.Collections.Generic;

namespace LeapPlannerApi.Model.Planner
{
    public class GetAllPlannersReply : ReplyBase
    {
        public List<PlannerDetail> Planners { get; set; }
    }
}
