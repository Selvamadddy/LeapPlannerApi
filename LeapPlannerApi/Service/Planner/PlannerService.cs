using LeapPlannerApi.Entities.Planner;
using LeapPlannerApi.Repository.Login;
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
        private ILoginRepo _loginRepo;

        public PlannerService(IPlannerRepo plannerRepo, ILoginRepo loginRepo)
        {
            _plannerRepo = plannerRepo;
            _loginRepo = loginRepo;
        }

        public async Task<List<PlannerDetail>> GetAllPlanner(string email)
        {
            List<PlannerDetail> planners = new List<PlannerDetail>();
            try
            {
                var userDetails = await _loginRepo.GetUserDetails(email);
                if (userDetails != null)
                {
                    planners = await _plannerRepo.GetAllPlanner(userDetails.Id);
                }              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return planners;
        }

        public async Task<bool> AddPlanner(string email, string name)
        {
            var response = false;
            try
            {
                var userDetails = await _loginRepo.GetUserDetails(email);
                if (userDetails != null)
                {
                    response = await _plannerRepo.AddPlanner(userDetails.Id, name);
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> UpdatePlanner(int plannerId, string updatedName, string email)
        {
            var response = false;
            try
            {
                var userDetails = await _loginRepo.GetUserDetails(email);
                if (userDetails != null)
                {
                    response = await _plannerRepo.UpdatePlannerName(plannerId, updatedName, userDetails.Id);
                }
            }
            catch
            {
                throw;
            }
            return response;
        }

        public async Task<bool> DeletePlanner(int plannerId, string email)
        {
            var response = false;
            try
            {
                var userDetails = await _loginRepo.GetUserDetails(email);
                if (userDetails != null)
                {
                    response = await _plannerRepo.DeletePlanner(plannerId, userDetails.Id);
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
