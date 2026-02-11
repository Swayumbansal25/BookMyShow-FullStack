using AutoMapper;
using BookMyShow.Application.DTOs.Core.Users;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    /// <summary>
    /// AutoMapper profile for Users mappings
    /// </summary>
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            // DTO -> Entity
            CreateMap<CreateUsersDto, Users>();

            // DTO -> Entity (for updates)
            CreateMap<UpdateUsersDto, Users>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            // Optionally: Entity -> DTOs if you add response DTOs later
            // CreateMap<Users, UsersResponseDto>();
        }
    }
}
