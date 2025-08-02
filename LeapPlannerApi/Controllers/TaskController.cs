using AutoMapper;
using LeapPlannerApi.Model.Common;
using LeapPlannerApi.Model.Task;
using LeapPlannerApi.Service.Common;
using LeapPlannerApi.Service.Task;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeapPlannerApi.Controllers
{
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService _taskService;
        private readonly IMapper _mapper;
        private ITokenService _tokenService;

        public TaskController(ITaskService taskService, IMapper mapper, ITokenService tokenService)
        {
            _taskService = taskService;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [Route("api/task/GetAllTask")]
        [HttpPost]
        public async Task<GetAllTaskReply> GetAllTask(GetAllTask getAllTasks)
        {
            GetAllTaskReply reply = new GetAllTaskReply();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && getAllTasks.TaskCardId > 0)
                    {
                        var response = await _taskService.GetAllTask(getAllTasks.TaskCardId);
                        reply = ResponseService.UpdateSucess<GetAllTaskReply>();
                        if (response != null && response.Count > 0)
                        {
                            reply.Tasks = _mapper.Map<List<TaskDetails>>(response);
                        }
                    }
                    else
                    {
                        reply = ResponseService.UpdateErrorMessage<GetAllTaskReply>(ErrorMessage.BadRequest.ToString());
                    }
                }
                else
                {
                    reply = ResponseService.UpdateErrorMessage<GetAllTaskReply>(ErrorMessage.UnAuthorized.ToString());
                }
            }
            catch
            {
                reply = ResponseService.UpdateErrorMessage<GetAllTaskReply>(ErrorMessage.InternalServerError.ToString());
            }
            return reply;
        }

        [Route("api/task/AddTask")]
        [HttpPost]
        public async Task<ReplyBase> AddTask(GetAllTask addTask)
        {
            ReplyBase reply = new();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && addTask.TaskCardId > 0)
                    {
                        var response = await _taskService.AddTask(addTask.TaskCardId);

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

        [Route("api/task/UpdateTask")]
        [HttpPut]
        public async Task<ReplyBase> UpdateTask(UpdateTask updateTask)
        {
            ReplyBase reply = new();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && updateTask.Id > 0)
                    {
                        var response = await _taskService.UpdateTask(updateTask.Id, updateTask.Text, updateTask.IsChecked);

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

        [Route("api/task/DeleteTask")]
        [HttpDelete]
        public async Task<ReplyBase> DeleteTask(DeleteTask deleteTask)
        {
            ReplyBase reply = new();
            try
            {
                (bool isAuthorizedUser, string email) = _tokenService.ValidateToken(Request.Headers);
                if (isAuthorizedUser)
                {
                    if (!string.IsNullOrEmpty(email) && deleteTask.Id > 0)
                    {
                        var response = await _taskService.DeleteTask(deleteTask.Id);

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
