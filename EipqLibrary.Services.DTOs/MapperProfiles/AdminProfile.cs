using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<AdminCreationRequest, AdminCreationDto>().ReverseMap();
            CreateMap<AdminCreationDto, AdminUser>().ReverseMap();
            CreateMap<AdminUser, AdminUserModel>().ReverseMap();
            CreateMap<AdminDeletionRequest, AdminDeletionDto>().ReverseMap();
            CreateMap<AdminChangeStatusRequest, AdminChangeStatusDto>().ReverseMap();

            CreateMap<AdminUser, AdminInfo>()
                .ForMember(x => x.Name, opts => opts.MapFrom(a => a.FirstName + " " + a.LastName));
        }
    }
}
