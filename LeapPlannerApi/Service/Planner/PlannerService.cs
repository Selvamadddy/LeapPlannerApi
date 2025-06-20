using LeapPlannerApi.Entities.Planner;
using LeapPlannerApi.Repository.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.Planner
{
    public class PlannerService : IPlannerService
    {
        private IPlannerRepo _plannerRepo;

        public PlannerService(IPlannerRepo plannerRepo)
        {
            _plannerRepo = plannerRepo;
        }

        public async Task<List<PlannerDetail>> GetAllPlanner(int userId)
        {
            List<PlannerDetail> userDetails = new List<PlannerDetail>();
            try
            {
                userDetails = await _plannerRepo.GetAllPlanner(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userDetails;
        }
    }
}
