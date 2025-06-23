using LeapPlannerApi.Entities.Planner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.Planner
{
    public interface IPlannerService
    {
        Task<List<PlannerDetail>> GetAllPlanner(string email);
        Task<bool> AddPlanner(string email, string name);
        Task<bool> UpdatePlanner(int plannerId, string updatedName, string email);
        Task<bool> DeletePlanner(int plannerId, string email);
    }
}
