using AutoMapper;
using GigHub.Controllers.Api;
using GigHub.Dtos;
using GigHub.Models;

namespace GigHub.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, UserDto>();
            Mapper.CreateMap<Gig, GigDto>();
            Mapper.CreateMap<Notification, NotificationDto>();
        }
    }
}