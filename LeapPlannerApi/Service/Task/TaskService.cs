using LeapPlannerApi.Entities.Task;
using LeapPlannerApi.Repository.Task;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.Task
{
    public class TaskService : ITaskService
    {
        private ITaskRepo _taskRepo;

        public TaskService(ITaskRepo taskRepo)
        {
            _taskRepo = taskRepo;
        }

        public async Task<List<TaskDetail>> GetAllTask(int taskCardId)
        {
            List<TaskDetail> tasks = new();
            try
            {
                tasks = await _taskRepo.GetAllTask(taskCardId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tasks;
        }

        public async Task<bool> AddTask(int taskcardId)
        {
            bool response = false;
            try
            {
                response = await _taskRepo.AddTask(taskcardId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> UpdateTask(int id, string text, bool isChecked)
        {
            bool response = false;
            try
            {
                response = await _taskRepo.UpdateTask(id, text ?? "", isChecked);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> DeleteTask(int id)
        {
            bool response = false;
            try
            {
                response = await _taskRepo.DeleteTask(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}

