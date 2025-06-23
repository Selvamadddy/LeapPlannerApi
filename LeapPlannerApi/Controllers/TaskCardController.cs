using AutoMapper;
using LeapPlannerApi.Model.Common;
using LeapPlannerApi.Model.TaskCard;
using LeapPlannerApi.Service.Common;
using LeapPlannerApi.Service.TaskCard;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Controllers
{
    [ApiController]
    public class TaskCardController : ControllerBase
    {
        private ITaskCardService _taskCardService;
        private readonly IMapper _mapper;
        private ITokenService _tokenService;

        public TaskCardController(ITaskCardService taskCardService, IMapper mapper, ITokenService tokenService)
        {
            _taskCardService = taskCardService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [Route("api/taskcard/GetAllTaskCards")]
        [HttpPost]
        public async Task<GetAllTaskCardsReply> GetAllTaskCards(GetAllTaskCards getAllTaskCards)
        {
            GetAllTaskCardsReply reply = new GetAllTaskCardsReply();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && getAllTaskCards.PlannerId > 0)
                    {
                        var response = await _taskCardService.GetAllTaskCard(getAllTaskCards.PlannerId);
                        reply = ResponseService.UpdateSucess<GetAllTaskCardsReply>();
                        if (response != null && response.Count > 0)
                        {
                            reply.TaskCards = _mapper.Map<List<TaskCardDetails>>(response);
                        }
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<GetAllTaskCardsReply>(ErrorMessage.BadRequest.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<GetAllTaskCardsReply>(ErrorMessage.UnAuthorized.ToString());
                }
            }
            catch
            {
                reply = ResponseService.UpdateErrorMessage<GetAllTaskCardsReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/taskcard/AddTaskCard")]
        [HttpPost]
        public async Task<ReplyBase> AddTaskCard(GetAllTaskCards addTaskCard)
        {
            ReplyBase reply = new();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && addTaskCard.PlannerId > 0)
                    {
                        var response = await _taskCardService.AddTaskCard(addTaskCard.PlannerId);
                        
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

        [Route("api/taskcard/UpdateTaskCard")]
        [HttpPut]
        public async Task<UpdateTaskCardReply> UpdateTaskCard(UpdateTaskCard updateTaskCard)
        {
            UpdateTaskCardReply reply = new();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && updateTaskCard.Id > 0)
                    {
                        var response = await _taskCardService.UpdateTaskCard(updateTaskCard.Id, updateTaskCard.Name, updateTaskCard.Colour);

                        if (response)
                        {
                            reply = ResponseService.UpdateSucess<UpdateTaskCardReply>();
                        }
                        else
                        {
                            reply = ResponseService.UpdateErrorMessage<UpdateTaskCardReply>(ErrorMessage.InvalidProcess.ToString());
                        }
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<UpdateTaskCardReply>(ErrorMessage.BadRequest.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<UpdateTaskCardReply>(ErrorMessage.UnAuthorized.ToString());
                }
            }
            catch
            {
                reply = ResponseService.UpdateErrorMessage<UpdateTaskCardReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/taskcard/DeleteTaskCard")]
        [HttpDelete]
        public async Task<ReplyBase> DeleteTaskCard(DeleteTaskCard deleteTaskCard)
        {
            ReplyBase reply = new();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && deleteTaskCard.Id > 0)
                    {
                        var response = await _taskCardService.DeleteTaskCard(deleteTaskCard.Id);

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
