using System;
using System.Threading.Tasks;

namespace EipqLibrary.Domain.Interfaces.EFInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IBookRepository BookRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IGroupRepository GroupRepository { get; }
        public IBookCreationRequestRepository BookCreationRequestRepository { get; }
        public IBookDeletionRequestRepository BookDeletionRequestRepository { get; }
        public IBookInstanceRepository BookInstanceRepository{ get; }
        public IReservationRepository ReservationRepository{ get; }
        public IProfessionRepository ProfessionRepository { get; }

        public Task SaveChangesAsync();
    }
}
