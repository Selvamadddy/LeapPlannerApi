using LeapPlannerApi.Entities.Task;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.Task
{
    public interface ITaskRepo
    {
        Task<List<TaskDetail>> GetAllTask(int taskCardId);
        Task<bool> UpdateTask(int id, string text, bool isChecked);
        Task<bool> AddTask(int taskCardId);
        Task<bool> DeleteTask(int id);
    }
}
