using AutoMapper;
using LeapPlannerApi.Entities.Login;
using LeapPlannerApi.Model;
using LeapPlannerApi.Model.Login;

namespace LeapPlannerApi.Mapper
{
    public class LoginMapper : Profile
    {
        public LoginMapper()
        {
            CreateMap<RegisterUser, RegisterUserDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
            CreateMap<Login, LoginDto>();
        }
    }
}
