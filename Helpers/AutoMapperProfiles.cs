using AutoMapper;
using PizzaOrder.Dtos;
using PizzaOrder.Models;

namespace PizzaOrder.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // user
            CreateMap<User, UserForAddDto>().ReverseMap();
            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailsDto>();
            CreateMap<User, UserForAddDto>();
            CreateMap<User, UserForLoginDto>();
   
            
          
        }
    }
}
