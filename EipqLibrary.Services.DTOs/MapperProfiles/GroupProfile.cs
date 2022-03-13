using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Collections.Generic;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupCreationRequest, Group>();
            CreateMap<Group, GroupModel>();
            //CreateMap<List<Group>, List<GroupModel>>();
        }
    }
}
