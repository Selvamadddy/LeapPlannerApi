using AutoMapper;

namespace LeapPlannerApi.Mapper
{
    public class TaskMapper : Profile
    {
        public TaskMapper()
        {
            CreateMap<Entities.Task.TaskDetail, Model.Task.TaskDetails>();
        }
    }
}
