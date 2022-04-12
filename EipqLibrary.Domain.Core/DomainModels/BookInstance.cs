﻿using EipqLibrary.Domain.Core.DomainModels.Common;
using EipqLibrary.Domain.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace EipqLibrary.Domain.Core.DomainModels
{
    public class BookInstance : BaseEntity
    {
        public ICollection<Reservation> Borrowings { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public IEnumerable<Reservation> GetActiveReservations()
        {
            return Borrowings.Where(x => x.Status != ReservationStatus.Returned && x.Status != ReservationStatus.Cancelled);
        }
    }
}