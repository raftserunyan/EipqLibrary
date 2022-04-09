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
                .ForMember(d => d.BookId, opt => opt.MapFrom(s => s.Id));

            CreateMap<BookUpdateRequest, Book>()
                .ForMember(d => d.TotalCount, opt => opt.MapFrom(s => s.Quantity));
        }
    }
}
