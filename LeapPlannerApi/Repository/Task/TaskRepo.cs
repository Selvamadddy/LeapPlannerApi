using Dapper;
using LeapPlannerApi.Entities.Task;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.Task
{
    public class TaskRepo : ITaskRepo
    {
        private readonly DapperContext _context;
        public TaskRepo(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<TaskDetail>> GetAllTask(int taskCardId)
        {
            List<TaskDetail> response = new();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.QueryAsync<TaskDetail>("GetAllTask", new { taskCardId = taskCardId }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data?.ToList();
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> AddTask(int taskCardId)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("AddTask", new { taskCardId = taskCardId }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data > 0;
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> UpdateTask(int id, string text, bool isChecked)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("UpdateTask", new { id = id, text = text, isChecked = isChecked }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data > 0;
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> DeleteTask(int id)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("DeleteTask", new { id = id }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data > 0;
                }
            }
            catch
            {
                throw;
            }
            return response;
        }
    }
}
