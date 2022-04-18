using EipqLibrary.Domain.Core.DomainModels;
using EipqLibrary.Services.DTOs.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EipqLibrary.Services.Interfaces.ServiceInterfaces
{
    public interface IBookDeletionService
    {
        Task<BookDeletionRequest> CreateAsync(BookDeletionRequestDto requestDto);
    }
}
