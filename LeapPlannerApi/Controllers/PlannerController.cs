using AutoMapper;
using LeapPlannerApi.Model.Common;
using LeapPlannerApi.Model.Planner;
using LeapPlannerApi.Model.Planner.model;
using LeapPlannerApi.Service.Common;
using LeapPlannerApi.Service.Planner;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Controllers
{
    [ApiController]
    public class PlannerController : ControllerBase
    {
        private IPlannerService _plannerService;
        private readonly IMapper _mapper;
        private ITokenService _tokenService;

        public PlannerController(IPlannerService plannerService, IMapper mapper, ITokenService tokenService)
        {
            _plannerService = plannerService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [Route("api/planner/GetAllPlanners")]
        [HttpGet]
        public async Task<GetAllPlannersReply> GetAllPlanners()
        {
            GetAllPlannersReply reply = new GetAllPlannersReply();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var response = await _plannerService.GetAllPlanner(email);
                        reply = ResponseService.UpdateSucess<GetAllPlannersReply>();
                        if (response != null && response.Count > 0)
                        {                
                            reply.Planners = _mapper.Map<List<PlannerDetail>>(response);
                        }
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<GetAllPlannersReply>(ErrorMessage.BadRequest.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<GetAllPlannersReply>(ErrorMessage.UnAuthorized.ToString());
                }
            }
            catch
            {
                reply = ResponseService.UpdateErrorMessage<GetAllPlannersReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/planner/AddPlanner")]
        [HttpPost]
        public async Task<ReplyBase> AddPlanner(AddPlanner addPlanner)
        {
            ReplyBase reply = new ReplyBase();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var response = await _plannerService.AddPlanner(email, addPlanner.Name ?? "");            
                        if (response)
                        {
                            reply = ResponseService.UpdateSucess<ReplyBase>();
                        }
                        else
                        {
                            reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.InvalidProcess.ToString());
                        }
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.BadRequest.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.UnAuthorized.ToString());
                }
            }
            catch
            {
                reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/planner/UpdatePlanner")]
        [HttpPut]
        public async Task<ReplyBase> UpdatePlanner(UpdatePlanner updatePlanner)
        {
            ReplyBase reply = new ReplyBase();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && updatePlanner.Id > 0)
                    {
                        var response = await _plannerService.UpdatePlanner(updatePlanner.Id, updatePlanner.Name, email);
                        if (response)
                        {
                            reply = ResponseService.UpdateSucess<ReplyBase>();
                        }
                        else
                        {
                            reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.InvalidProcess.ToString());
                        }
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.BadRequest.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.UnAuthorized.ToString());
                }
            }
            catch
            {
                reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/planner/DeletePlanner")]
        [HttpDelete]
        public async Task<ReplyBase> DeletePlanner(DeletePlanner deletePlanner)
        {
            ReplyBase reply = new ReplyBase();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && deletePlanner.Id > 0)
                    {
                        var response = await _plannerService.DeletePlanner(deletePlanner.Id, email);
                        if (response)
                        {
                            reply = ResponseService.UpdateSucess<ReplyBase>();
                        }
                        else
                        {
                            reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.InvalidProcess.ToString());
                        }
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.BadRequest.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.UnAuthorized.ToString());
                }
            }
            catch
            {
                reply = ResponseService.UpdateErrorMessage<ReplyBase>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

    }
}
