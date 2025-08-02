using Dapper;
using LeapPlannerApi.Entities.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.Planner
{
    public class PlannerRepo : IPlannerRepo
    {
        private readonly DapperContext _context;
        public PlannerRepo(DapperContext context)
        {
            _context = context;
        }

        public async Task<List<PlannerDetail>> GetAllPlanner(int userId)
        {
            List<PlannerDetail> response = new List<PlannerDetail>();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.QueryAsync<PlannerDetail> ("GetUserPlanners", new { userId = userId }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data.ToList();
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> AddPlanner(int userId, string name)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("AddPlanner", new { userId = userId, name = name }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data > 0;
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> UpdatePlannerName(int id, string updatedName, int userId)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("UpdatePlannersName", new { id = id, name = updatedName, userId = userId }, commandType: System.Data.CommandType.StoredProcedure);
                    response = data > 0;
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> DeletePlanner(int id, int userId)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var data = await connection.ExecuteAsync("DeletePlanners", new { id = id, userId = userId }, commandType: System.Data.CommandType.StoredProcedure);
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
