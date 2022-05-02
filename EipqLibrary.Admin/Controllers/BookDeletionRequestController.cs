using AutoMapper;
using EipqLibrary.Admin.Attributes;
using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.Constants.Admins;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
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

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian)]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateDeletionRequest(BookDeletionRequestDto deletionRequest)
        {
            var request = await _bookDeletionService.CreateAsync(deletionRequest);

            var requestModel = _mapper.Map<BookDeletionRequestModel>(request);
            return Ok(requestModel);
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Accountant)]
        [HttpPost("{requestId}/confirm")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Confirm(int requestId, [FromBody] BookManipulationAccountantMessage accountantNote)
        {
            var accountantAction = new BookDeletionRequestAccountantAction
            {
                RequestId = requestId,
                AccountantActionResult = Domain.Core.Enums.BookDeletionRequestStatus.Approved,
                AccountantMessage = accountantNote.AccountantMessage
            };

            await _bookDeletionService.AddAccountantAction(accountantAction);
            return Ok();
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Accountant)]
        [HttpPost("{requestId}/reject")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Reject(int requestId, [FromBody] BookManipulationAccountantMessage accountantNote)
        {
            var accountantAction = new BookDeletionRequestAccountantAction
            {
                RequestId = requestId,
                AccountantActionResult = Domain.Core.Enums.BookDeletionRequestStatus.Rejected,
                AccountantMessage = accountantNote.AccountantMessage
            };

            await _bookDeletionService.AddAccountantAction(accountantAction);
            return Ok();
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian, AdminRoleNames.Accountant)]
        [HttpGet]
        [ProducesResponseType(typeof(PagedData<BookDeletionRequestModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] PageInfo pageInfo, [FromQuery] BDRSortOption sort, [FromQuery] BookDeletionRequestStatus? status)
        {
            var requests = await _bookDeletionService.GetAllAsync(pageInfo, sort, status);
            return Ok(requests);
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian, AdminRoleNames.Accountant)]
        [HttpGet("{requestId}")]
        [ProducesResponseType(typeof(BookDeletionRequestModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int requestId)
        {
            var request = await _bookDeletionService.GetByIdAsync(requestId);
            return Ok(request);
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian)]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateBookDeletionRequest updateRequest)
        {
            var updatedEntity = await _bookDeletionService.UpdateAsync(updateRequest);
            return Ok(updatedEntity);
        }

        [AuthorizeRoles(AdminRoleNames.SuperAdmin, AdminRoleNames.Librarian)]
        [HttpDelete("{requestId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int requestId)
        {
            await _bookDeletionService.DeleteAsync(requestId);
            return Ok();
        }
    }
}
