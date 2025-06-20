using LeapPlannerApi.Entities.Planner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.Planner
{
    public interface IPlannerRepo
    {
        Task<List<PlannerDetail>> GetAllPlanner(int userId);
    }
}
