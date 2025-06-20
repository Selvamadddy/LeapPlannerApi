using AutoMapper;
using LeapPlannerApi.Model.Planner;
using LeapPlannerApi.Model.Planner.model;
using LeapPlannerApi.Service.Common;
using LeapPlannerApi.Service.Planner;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LeapPlannerApi.Controllers
{
    [ApiController]
    public class PlannerController : ControllerBase
    {
        private IPlannerService _plannerService;
        private readonly IMapper _mapper;

        public PlannerController(IPlannerService plannerService, IMapper mapper)
        {
            _plannerService = plannerService;
            _mapper = mapper;
        }

        [Route("api/planner/GetAllPlanners")]
        [HttpPost]
        public async Task<GetAllPlannersReply> GetAllPlanners(GetAllPlanners request)
        {
            GetAllPlannersReply reply = new GetAllPlannersReply();
            try
            {
                if (request.UserId > 0)
                {
                    var response = await _plannerService.GetAllPlanner(request.UserId);
                    if (response != null && response.Count > 0)
                    {
                        reply = ResponseService.UpdateSucess<GetAllPlannersReply>();
                        reply.Planners = _mapper.Map<List<PlannerDetail>>(response);
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<GetAllPlannersReply>(ErrorMessage.InvalidProcess.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<GetAllPlannersReply>(ErrorMessage.BadRequest.ToString());
                }
            }
            catch (Exception ex)
            {
                reply = ResponseService.UpdateErrorMessage<GetAllPlannersReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }
    }


    #region Validator
   
    #endregion Validator
}
