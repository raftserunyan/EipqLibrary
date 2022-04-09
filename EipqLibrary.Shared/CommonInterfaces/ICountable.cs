using System;

namespace EipqLibrary.Shared.CommonInterfaces
{
    public interface ICountable
    {
        public int Quantity { get; set; }
        public int AvailableForBorrowingCount { get; set; }
        public int AvailableForUsingInLibraryCount { get; set; }
    }
}
