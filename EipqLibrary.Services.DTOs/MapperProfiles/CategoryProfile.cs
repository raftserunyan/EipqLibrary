using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
            CreateMap<Category, CategoryCreationRequest>().ReverseMap();
            CreateMap<CategoryUpdateRequest, Category>();
        }
    }
}
