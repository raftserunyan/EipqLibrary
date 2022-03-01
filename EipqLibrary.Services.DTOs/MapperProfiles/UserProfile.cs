using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegistrationRequest, User>().ReverseMap();
            CreateMap<UserCreationDto, User>().ReverseMap();
            CreateMap<RegistrationRequest, UserCreationDto>().ReverseMap();
            CreateMap<User, UpdateUserStatusDto>().ReverseMap();
        }
    }
}
