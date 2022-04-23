using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Domain.Core.Enums;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [Route("api/BookCreationRequests")]
    [ApiController]
    public class BookCreationRequestController : ControllerBase
    {
        private readonly IBookCreationRequestService _bookCreationRequestService;
        private readonly IBookService _bookService;

        public BookCreationRequestController(IBookCreationRequestService bookCreationRequestService,
                                             IBookService bookService)
        {
            _bookCreationRequestService = bookCreationRequestService;
            _bookService = bookService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] BookAdditionRequest bookCreationRequest)
        {
            var existingBook = await _bookService.GetByNameAndAuthorAsync(bookCreationRequest.Name, bookCreationRequest.Author);
            if (existingBook != null)
            {
                if (!bookCreationRequest.OverrideExistingValues)
                {
                    dynamic o = new ExpandoObject();
                    bool isCompatibleWithExisting = true;

                    if (!String.IsNullOrEmpty(existingBook.Description) && existingBook.Description != bookCreationRequest.Description)
                    {
                        o.CurrentDescription = existingBook.Description;
                        isCompatibleWithExisting = false;
                    }
                    if (existingBook.PagesCount != null && existingBook.PagesCount != bookCreationRequest.PagesCount)
                    {
                        o.CurrentPagesCount = existingBook.PagesCount;
                        isCompatibleWithExisting = false;
                    }
                    if (existingBook.ProductionYear != bookCreationRequest.ProductionYear)
                    {
                        o.CurrentProductionYear = existingBook.ProductionYear;
                        isCompatibleWithExisting = false;
                    }
                    if (existingBook.Category.Id != bookCreationRequest.CategoryId)
                    {
                        o.CurrentCategory = existingBook.Category.Name;
                        isCompatibleWithExisting = false;
                    }

                    if (!isCompatibleWithExisting)
                    {
                        o.Message = $"A book with the name '{existingBook.Name}' and author '{existingBook.Author}' already exists. " +
                                    $"Do you want to override the values of these fields?";

                        return BadRequest(o);
                    }
                }
            }

            int entityId = await _bookCreationRequestService.CreateBookAdditionRequestAsync(bookCreationRequest);
            return Ok(new { bookCreationRequestId = entityId });
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedData<BookCreationRequestModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] PageInfo pageInfo, [FromQuery] BCRSortOption sort, [FromQuery] BookCreationRequestStatus? status)
        {
            var requests = await _bookCreationRequestService.GetAllAsync(pageInfo, sort, status);
            return Ok(requests);
        }

        [HttpGet("{requestId}")]
        [ProducesResponseType(typeof(BookCreationRequestModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int requestId)
        {
            var request = await _bookCreationRequestService.GetByIdAsync(requestId);
            return Ok(request);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] UpdateBookCreationRequest updateRequest)
        {
            var updatedEntity = await _bookCreationRequestService.UpdateAsync(updateRequest);
            return Ok(updatedEntity);
        }

        [HttpDelete("{requestId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int requestId)
        {
            await _bookCreationRequestService.DeleteAsync(requestId);
            return Ok();
        }

        [HttpPost("{requestId}/confirm")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Confirm(int requestId, [FromBody] BookManipulationAccountantMessage accountantNote)
        {
            var accountantAction = new BookCreationRequestAccountantAction
            {
                RequestId = requestId,
                AccountantActionResult = Domain.Core.Enums.BookCreationRequestStatus.Approved,
                AccountantMessage = accountantNote.AccountantMessage
            };

            await _bookCreationRequestService.AddAccountantAction(accountantAction);
            return Ok();
        }

        [HttpPost("{requestId}/reject")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Reject(int requestId, [FromBody] BookManipulationAccountantMessage accountantNote)
        {
            var accountantAction = new BookCreationRequestAccountantAction
            {
                RequestId = requestId,
                AccountantActionResult = Domain.Core.Enums.BookCreationRequestStatus.Rejected,
                AccountantMessage = accountantNote.AccountantMessage
            };

            await _bookCreationRequestService.AddAccountantAction(accountantAction);
            return Ok();
        }
    }
}
