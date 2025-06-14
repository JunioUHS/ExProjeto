using AutoMapper;
using ExProjetoAPI.DTOs;
using ExProjetoAPI.Models;

namespace ExProjetoAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserDto, ApplicationUser>().ReverseMap();
        }
    }
}