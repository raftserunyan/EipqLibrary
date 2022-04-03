using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookModel>()
                .ForMember(d => d.BookId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.BookAvailability, opt => opt.MapFrom(s => s.Availability.ToString()))
                .ForMember(d => d.DeletionReason, opt => opt.MapFrom(s => s.DeletionReason.ToString()));

            CreateMap<BookCreationRequest, Book>();
        }
    }
}
