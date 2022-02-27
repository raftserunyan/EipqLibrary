using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookModel>()
                .ForMember(d => d.BookId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.DeletionReason, opt => opt.MapFrom(s => s.DeletionReason.ToString()));

            CreateMap<BookCreationRequest, Book>();
        }
    }
}
