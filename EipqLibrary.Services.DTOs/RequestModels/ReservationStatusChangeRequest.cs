using EipqLibrary.Domain.Core.Enums;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    public class ReservationStatusChangeRequest
    {
        public ReservationStatus Status { get; set; }
    }
}
