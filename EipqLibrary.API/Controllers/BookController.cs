using EipqLibrary.Domain.Core.AggregatedEntities;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedData<BookModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] PageInfo pageInfo, [FromQuery] int? categoryId, [FromQuery] string author)
        {
            return Ok(await _bookService.GetAllAsync(pageInfo, categoryId, author));
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int bookId)
        {
            return Ok(await _bookService.GetByIdAsync(bookId));
        }
    }
}
