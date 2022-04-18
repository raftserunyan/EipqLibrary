using AutoMapper;
using EipqLibrary.Admin.Attributes;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    //[AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian)]
    [Route("api/bookDeletionRequests")]
    [ApiController]
    public class BookDeletionRequestController : ControllerBase
    {
        private readonly IBookDeletionService _bookDeletionService;
        private readonly IMapper _mapper;

        public BookDeletionRequestController(IBookDeletionService bookDeletionService,
                                             IMapper mapper)
        {
            _bookDeletionService = bookDeletionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeletionRequest(BookDeletionRequestDto deletionRequest)
        {
            var request = await _bookDeletionService.CreateAsync(deletionRequest);

            var requestModel = _mapper.Map<BookDeletionRequestModel>(request);
            return Ok(requestModel);
        }
    }
}
