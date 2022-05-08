using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupCreationRequest, Group>();
            CreateMap<Group, GroupModel>()
                .ForMember(d => d.CreationDate, opts => opts.MapFrom(s => s.CreationDate.ToShortDateString()))
                .ForMember(d => d.GraduationDate, opts => opts.MapFrom(s => s.GraduationDate.ToShortDateString()))
                .ForMember(d => d.CreationYear, opts => opts.MapFrom(s => s.CreationDate.Year))
                .ForMember(d => d.GraduationYear, opts => opts.MapFrom(s => s.GraduationDate.Year));
        }
    }
}
