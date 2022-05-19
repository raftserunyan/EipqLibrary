using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.Utils.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class ReservationService : BaseService, IReservationService
    {
        private const byte daysUntilReturnDate = 2;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ReservationService(IUnitOfWork uow,
                                  IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CancelReservationForAdminAsync(int reservationId)
        {
            var reservation = await _uow.ReservationRepository.GetByIdAsync(reservationId);
            
            if (reservation.Status == ReservationStatus.Borrowed ||
                reservation.Status == ReservationStatus.Returned ||
                reservation.Status == ReservationStatus.Cancelled)
            {
                throw BadRequest("Դուք կարող եք չեղարկել միայն դեռ չհաստատված ամրագրումները");
            }

            reservation.CancellationDate = DateTime.Now;
            reservation.Status = ReservationStatus.Cancelled;
            await _uow.SaveChangesAsync();
        }

        public async Task CancelReservationForStudentAsync(int reservationId, string userId)
        {
            var reservation = await _uow.ReservationRepository.GetByIdAsync(reservationId);
            if (reservation.UserId != userId)
            {
                throw new UnauthorizedAccessException("Դուք կարող եք չեղարկել միայն ձեր ամրագրումները");
            }
            if (reservation.Status == ReservationStatus.Borrowed ||
                reservation.Status == ReservationStatus.Returned ||
                reservation.Status == ReservationStatus.Cancelled)
            {
                throw BadRequest("Դուք կարող եք չեղարկել միայն դեռ չհաստատված ամրագրումները");
            }

            reservation.CancellationDate = DateTime.Now;
            reservation.Status = ReservationStatus.Cancelled;
            await _uow.SaveChangesAsync();
        }

        public async Task ChangeReservationStatusAsync(int reservationId, ReservationStatusChangeRequest changes)
        {
            var reservation = await _uow.ReservationRepository.GetByIdAsync(reservationId);

            switch (changes.Status)
            {                
                case ReservationStatus.Cancelled:
                    CancelReservation(reservation);
                    break;
                case ReservationStatus.Borrowed:
                    BorrowReservation(reservation);
                    break;
                case ReservationStatus.Returned:
                    ReturnReservation(reservation);
                    break;
                case ReservationStatus.Reserved:
                    throw BadRequest("Դուք չեք կարող կարգավիճակը դնել նորից 'Ամրագրված'");
                    break;
                default:
                    break;
            }

            await _uow.SaveChangesAsync();
        }

        public async Task<Reservation> CreateAsync(ReservationCreationRequest request, User user)
        {
            //Check if the book being requested exists
            var book = await _uow.BookRepository.GetByIdWithInstancesAndReservationsAsync(request.BookId);
            EnsureExists(book, $"Նշված գիրքը չի գտնվել․ Id = {request.BookId}");

            //Keep only date part of the DateTimes
            request.ReturnDate = request.ReturnDate.DropTimePart();
            request.BorrowingDate = request.BorrowingDate.DropTimePart();

            //Check if there'll be available book for borrowing for the requested time
            int availableBookInstanceId = -1;
            DateTime? suggestedTime = null;

            bool canBeReserved = IsThereFreeBookForTheInterval(book, request, ref availableBookInstanceId, ref suggestedTime);
            if (!canBeReserved)
            {
                var message = $"Նշված ժամանակահատվածի համար չկա հասանելի օրինակ \"{book.Author} - {book.Name}\" գրքից։ ";
                if (suggestedTime != null)
                {
                    message += $"Առաջարկվող ժամանակահատված՝ սկսած {suggestedTime.Value.ToShortDateString()}-ից";
                }

                throw BadRequest(message);
            }
            
            //Save the reservation
            var availableBookInstance = await _uow.BookInstanceRepository.GetByIdAsync(availableBookInstanceId);

            var newReservation = _mapper.Map<Reservation>(request);
            newReservation.UserId = user.Id;
            newReservation.BookInstanceId = availableBookInstance.Id;
            newReservation.BookName = book.Name;
            newReservation.BookAuthor = book.Author;

            availableBookInstance.Borrowings.Add(newReservation);
            await _uow.SaveChangesAsync();

            return newReservation;
        }

        public async Task<PagedData<Reservation>> GetAllAsync(PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status)
        {
            var pagedReservations = await _uow.ReservationRepository.GetAllAsync(pageInfo, reservationSort, status);
            EnsureExists(pagedReservations);

            return pagedReservations;
        }

        public async Task<PagedData<Reservation>> GetAllReservationsPagedAsync(PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status)
        {
            var pagedReservations = await _uow.ReservationRepository.GetAllReservationsFilteredAndPagedAsync(pageInfo, reservationSort, status);
            EnsureExists(pagedReservations);

            return pagedReservations;
        }

        public async Task<PagedData<Reservation>> GetReservationsByUserIdAsync(string userId, PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status)
        {
            var pagedReservations = await _uow.ReservationRepository.GetReservationsByUserIdAsync(userId, pageInfo, reservationSort, status);
            EnsureExists(pagedReservations);

            return pagedReservations;
        }

        public async Task<PagedData<Reservation>> GetSoonEndingReservationsPagedAsync(PageInfo pageInfo)
        {
            var reservationsPaged = await _uow.ReservationRepository.GetSoonEndingReservationsPagedAsync(pageInfo, daysUntilReturnDate);

            return reservationsPaged;
        }

        // Private methods
        private void BorrowReservation(Reservation reservation)
        {
            if (reservation.Status == ReservationStatus.Returned ||
                reservation.Status == ReservationStatus.Cancelled ||
                reservation.Status == ReservationStatus.Borrowed)
            {
                throw BadRequest("Կարելի է վերցնել միայն 'Ամրագրված' կարգավիճակով ամրագրումները");
            }

            reservation.Status = ReservationStatus.Borrowed;
            reservation.ActualBorrowingDate = DateTime.Now;
        }
        private void CancelReservation(Reservation reservation)
        {
            if (reservation.Status == ReservationStatus.Returned ||
                reservation.Status == ReservationStatus.Cancelled ||
                reservation.Status == ReservationStatus.Borrowed)
            {
                throw BadRequest("Կարելի է չեղարկել միայն 'Ամրագրված' կարգավիճակով ամրագրումները");
            }

            reservation.Status = ReservationStatus.Cancelled;
            reservation.CancellationDate = DateTime.Now;
        }
        private void ReturnReservation(Reservation reservation)
        {
            if (reservation.Status == ReservationStatus.Reserved ||
                reservation.Status == ReservationStatus.Cancelled ||
                reservation.Status == ReservationStatus.Returned)
            {
                throw BadRequest("Կարելի է վերադարձնել միայն 'Վերցված' կարգավիճակով ամրագրումները");
            }

            reservation.Status = ReservationStatus.Returned;
            reservation.ActualReturnDate = DateTime.Now;
        }

        private bool IsThereFreeBookForTheInterval(Book book, ReservationCreationRequest request, ref int bookInstanceId, ref DateTime? suggestedTime)
        {
            foreach (var instance in book.Instances)
            {
                var reservations = instance.GetActiveReservations().OrderBy(x => x.ExpectedBorrowingDate);

                var lastReturnDate = DateTime.Now.DropTimePart();
                //Check if the new reservation can fit between any two of the existing reservations
                foreach (var reservation in reservations)
                {
                    if (request.BorrowingDate >= lastReturnDate && request.ReturnDate <= reservation.ExpectedBorrowingDate)
                    {
                        bookInstanceId = instance.Id;
                        return true;
                    }
                    lastReturnDate = reservation.ExpectedReturnDate;
                }
                //Check if the new reservation is after all existing reservations
                if (request.BorrowingDate >= lastReturnDate)
                {
                    bookInstanceId = instance.Id;
                    return true;
                }

                if (suggestedTime != null)
                {
                    if (lastReturnDate < suggestedTime.Value)
                    {
                        suggestedTime = lastReturnDate;
                    }
                }
                else
                {
                    suggestedTime = lastReturnDate;
                }
            }

            return false;
        }
    }
}

