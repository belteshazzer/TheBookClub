using AutoMapper;
using TheBookClub.Models.Dtos.AuthDtos;
using TheBookClub.Models.Entities;
using TheBookClub.Models.Dtos;

namespace TheBookClub.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map RegisterRequestDto to User
            CreateMap<RegisterRequestDto, User>().ReverseMap();
            CreateMap<BookDto, Book>().ReverseMap();
            CreateMap<BookmarkDto, Bookmark>().ReverseMap();
            CreateMap<ReviewDto, Review>().ReverseMap();
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<OrderDto, Orders>().ReverseMap();
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<AuthorDto, Author>().ReverseMap();
        }
    }
}