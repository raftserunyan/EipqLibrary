using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;

namespace EipqLibrary.Services.DTOs.MapperProfiles
{
    public class ReservationProfile : Profile
    {
        public ReservationProfile()
        {
            CreateMap<ReservationCreationRequest, Reservation>()
                .ForMember(d => d.ExpectedBorrowingDate, opts => opts.MapFrom(s => s.BorrowingDate))
                .ForMember(d => d.ExpectedReturnDate, opts => opts.MapFrom(s => s.ReturnDate));

            CreateMap<Reservation, ReservationModel>()
                .ForMember(d => d.ExpectedBorrowingDate, opts => opts.MapFrom(s => s.ExpectedBorrowingDate.ToShortDateString()))
                .ForMember(d => d.ExpectedReturnDate, opts => opts.MapFrom(s => s.ExpectedReturnDate.ToShortDateString()))
                .ForMember(d => d.Book, opts => opts.MapFrom(s => s.BookInstance.Book));
            CreateMap<PagedData<Reservation>, PagedData<ReservationModel>>();
        }
    }
}
