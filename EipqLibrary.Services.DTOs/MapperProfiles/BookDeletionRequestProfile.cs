using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class BookDeletionRequestProfile : Profile
    {
        public BookDeletionRequestProfile()
        {
            CreateMap<BookDeletionRequestDto, BookDeletionRequest>();
            CreateMap<BookDeletionRequest, BookDeletionRequestModel>();
        }
    }
}
