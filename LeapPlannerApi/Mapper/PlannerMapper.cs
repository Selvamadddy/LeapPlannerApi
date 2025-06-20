using AutoMapper;
using LeapPlannerApi.Entities.Planner;
using LeapPlannerApi.Model.Planner.model;

namespace LeapPlannerApi.Mapper
{
    public class PlannerMapper : Profile
    {
        public PlannerMapper()
        {
            CreateMap<Entities.Planner.PlannerDetail, Model.Planner.model.PlannerDetail>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
