using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    class BookCreationRequestProfile : Profile
    {
        public BookCreationRequestProfile()
        {
            CreateMap<BookAdditionRequest, BookCreationRequest>();
            CreateMap<BookCreationRequest, BookCreationRequestModel>();
        }
    }
}
