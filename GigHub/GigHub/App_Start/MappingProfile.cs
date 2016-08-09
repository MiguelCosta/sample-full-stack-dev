using AutoMapper;
using GigHub.Core.Dtos;
using GigHub.Core.Models;

namespace GigHub.App_Start
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Mapping Models to Dto
        /// </summary>
        /// <see cref="https://github.com/AutoMapper/AutoMapper/wiki/Configuration"/>
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<Gig, GigDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<Notification, NotificationDto>();
        }
    }
}
