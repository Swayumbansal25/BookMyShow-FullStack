using AutoMapper;
using BookMyShow.Application.DTOs.Core.Movies;
using BookMyShow.Core.Entities.Core;

namespace BookMyShow.Application.Mappings.Core
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<CreateMoviesDto, Movies>();
            CreateMap<UpdateMoviesDto, Movies>()
                .ForMember(x => x.MovieId, opt => opt.Ignore());
        }
    }
}
