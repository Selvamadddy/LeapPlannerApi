using LeapPlannerApi.Entities.TaskCard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.TaskCard
{
    public interface ITaskCardRepo
    {
        Task<List<TaskCardDetail>> GetAllTaskCard(int plannerId);
        Task<bool> AddTaskCard(int plannerId);
        Task<bool> UpdateTaskCard(int id, string colour, string name);
        Task<bool> DeleteTaskCard(int id);
    }
}
