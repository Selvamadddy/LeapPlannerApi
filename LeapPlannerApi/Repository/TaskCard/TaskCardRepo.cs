using Dapper;
using LeapPlannerApi.Entities.TaskCard;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.TaskCard
{
    public class TaskCardRepo : ITaskCardRepo
    {
        private readonly DapperContext _context;
        public TaskCardRepo(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<TaskCardDetail>> GetAllTaskCard(int plannerId)
        {
            List<TaskCardDetail> response = new();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.QueryAsync<TaskCardDetail>("GetPlannerTaskCards", new { plannerId = plannerId }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data?.ToList();
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> AddTaskCard(int plannerId)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("AddTaskCard", new { plannerId }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data > 0;
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> UpdateTaskCard(int id, string colour, string name)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("UpdateTaskCard", new { id=id, name=name, colour = colour }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data > 0;
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> DeleteTaskCard(int id)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("DeleteTaskCard", new { id = id }, commandType: System.Data.CommandType.StoredProcedure);
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
