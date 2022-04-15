using AutoMapper;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Domain.Interfaces.EFInterfaces;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using EipqLibrary.Shared.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EipqLibrary.Infrastructure.Business.Services
{
    public class ReservationService : BaseService, IReservationService
    {
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
                throw BadRequest("You can cancel only the requests which are not borrowed or cancelled yet");
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
                throw new UnauthorizedAccessException("You can only cancel your reservations");
            }
            if (reservation.Status == ReservationStatus.Borrowed ||
                reservation.Status == ReservationStatus.Returned ||
                reservation.Status == ReservationStatus.Cancelled)
            {
                throw BadRequest("You can cancel only the requests which are not borrowed or cancelled yet");
            }

            reservation.CancellationDate = DateTime.Now;
            reservation.Status = ReservationStatus.Cancelled;
            await _uow.SaveChangesAsync();
        }

        public async Task<Reservation> CreateAsync(ReservationCreationRequest request, User user)
        {
            //Check if the book being requested exists
            var book = await _uow.BookRepository.GetByIdWithInstancesAndReservationsAsync(request.BookId);
            EnsureExists(book, $"A book with ID {request.BookId} does not exist");

            //Keep only date part of the DateTimes
            request.ReturnDate = request.ReturnDate.DropTimePart();
            request.BorrowingDate = request.BorrowingDate.DropTimePart();

            //Check if there'll be available book for borrowing for the requested time
            int availableBookInstanceId = -1;
            DateTime? suggestedTime = null;

            bool canBeReserved = IsThereFreeBookForTheInterval(book, request, ref availableBookInstanceId, ref suggestedTime);
            if (!canBeReserved)
            {
                var message = $"There'll be no free instances of \"{book.Author} - {book.Name}\" for the specified time interval.";
                if (suggestedTime != null)
                {
                    message += $"Suggested time: starting from {suggestedTime.Value.ToShortDateString()}";
                }

                throw BadRequest(message);
            }
            
            //Save the reservation
            var availableBookInstance = await _uow.BookInstanceRepository.GetByIdAsync(availableBookInstanceId);

            var newReservation = _mapper.Map<Reservation>(request);
            newReservation.UserId = user.Id;
            newReservation.BookInstanceId = availableBookInstance.Id;

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

        public async Task<PagedData<Reservation>> GetMyReservationsAsync(PageInfo pageInfo, ReservationSortOption reservationSort, ReservationStatus? status, string userId)
        {
            var pagedReservations = await _uow.ReservationRepository.GetMyReservationsAsync(pageInfo, reservationSort, userId, status);
            EnsureExists(pagedReservations);

            return pagedReservations;
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

