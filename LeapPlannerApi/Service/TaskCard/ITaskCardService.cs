using LeapPlannerApi.Entities.TaskCard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.TaskCard
{
    public interface ITaskCardService
    {
        Task<List<TaskCardDetail>> GetAllTaskCard(int plannerId);
        Task<bool> AddTaskCard(int plannerId);
        Task<bool> UpdateTaskCard(int id, string name, string colour);
        Task<bool> DeleteTaskCard(int id);
    }
}
