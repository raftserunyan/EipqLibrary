using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.RequestModels;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IReservationService
    {
        Task<Reservation> CreateAsync(ReservationCreationRequest request, User user);
    }
}
