using AutoMapper;
using EipqLibrary.Domain.Core.DomainModels;
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

        public async Task<Reservation> CreateAsync(ReservationCreationRequest request, User user)
        {
            //Check if the book being requested exists
            var book = await _uow.BookRepository.GetByIdWithIncludeAsync(request.BookId, x => x.Instances);
            EnsureExists(book, $"A book with ID {request.BookId} does not exist");

            //Keep only date part of the DateTimes
            request.ReturnDate = request.ReturnDate.DropTimePart();
            request.BorrowingDate = request.BorrowingDate.DropTimePart();

            //Check if there'll be available book for borrowing for the requested time
            int availableBookInstanceId = -1;

            bool canBeReserved = IsThereFreeBookForTheInterval(book, request, ref availableBookInstanceId);
            if (!canBeReserved)
            {
                throw BadRequest($"There'll be no free instances of \"{book.Author} - {book.Name}\" for the specified time interval");
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

        private bool IsThereFreeBookForTheInterval(Book book, ReservationCreationRequest request, ref int bookInstanceId)
        {
            foreach (var instance in book.Instances)
            {
                var reservations = instance.GetActiveReservations().OrderBy(x => x.ExpectedBorrowingDate);

                var lastReturnDate = DateTime.Now;
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
            }

            return false;
        }
    }
}

