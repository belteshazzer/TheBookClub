using AutoMapper;
using TheBookClub.Models.Dtos.AuthDtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map RegisterRequestDto to User
            CreateMap<RegisterRequestDto, User>().ReverseMap();
        }
    }
}