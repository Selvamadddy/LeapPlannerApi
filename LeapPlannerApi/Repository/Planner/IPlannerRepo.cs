using LeapPlannerApi.Entities.Planner;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.Planner
{
    public interface IPlannerRepo
    {
        Task<List<PlannerDetail>> GetAllPlanner(int userId);
        Task<bool> AddPlanner(int userId, string name);
        Task<bool> UpdatePlannerName(int id, string updatedName, int userId);
        Task<bool> DeletePlanner(int id, int userId);
    }
}
