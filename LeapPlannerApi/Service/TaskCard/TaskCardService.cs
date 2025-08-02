using LeapPlannerApi.Entities.TaskCard;
using LeapPlannerApi.Repository.TaskCard;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.TaskCard
{
    public class TaskCardService : ITaskCardService
    {
        private ITaskCardRepo _taskCardRepo;

        public TaskCardService(ITaskCardRepo taskCardRepo)
        {
            _taskCardRepo = taskCardRepo;
        }

        public async Task<List<TaskCardDetail>> GetAllTaskCard(int plannerId)
        {
            List<TaskCardDetail> taskCards = new();
            try
            {
                taskCards = await _taskCardRepo.GetAllTaskCard(plannerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taskCards;
        }

        public async Task<bool> AddTaskCard(int plannerId)
        {
            bool response = false;
            try
            {
                response = await _taskCardRepo.AddTaskCard(plannerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> UpdateTaskCard(int id, string name, string colour)
        {
            bool response = false;
            try
            {
                response = await _taskCardRepo.UpdateTaskCard(id, colour ?? "", name ?? "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> DeleteTaskCard(int id)
        {
            bool response = false;
            try
            {
                response = await _taskCardRepo.DeleteTaskCard(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
