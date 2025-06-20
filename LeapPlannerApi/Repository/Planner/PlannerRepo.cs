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
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
