using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
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
        }
    }
}
