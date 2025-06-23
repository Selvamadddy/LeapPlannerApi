using AutoMapper;

namespace LeapPlannerApi.Mapper
{
    public class TaskCardMapper : Profile
    {
        public TaskCardMapper()
        {
            CreateMap<Entities.TaskCard.TaskCardDetail, Model.TaskCard.TaskCardDetails>();
        }
    }
}
