using LeapPlannerApi.Entities.Planner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.Planner
{
    public interface IPlannerService
    {
        Task<List<PlannerDetail>> GetAllPlanner(int userId);
    }
}
