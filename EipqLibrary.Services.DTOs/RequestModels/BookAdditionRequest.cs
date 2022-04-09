using EipqLibrary.Shared.CommonInterfaces;
using EipqLibrary.Shared.Utils.Attributes;
using System.ComponentModel.DataAnnotations;

namespace EipqLibrary.Services.DTOs.RequestModels
{
    [QuantityEqualsBorrowableAndLibraryOnlySum]
    public class BookAdditionRequest : ICountable
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public int ProductionYear { get; set; }
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int? PagesCount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableForBorrowingCount { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int AvailableForUsingInLibraryCount { get; set; }

        //TODO
        //Check on bug for usinginlibrarycount
        //Add filtering to bookcreationrequests

        [Required]
        public int CategoryId { get; set; }

        public bool OverrideExistingValues { get; set; } = false;
    }
}
