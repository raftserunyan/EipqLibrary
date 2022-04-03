using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(ICollection<BookModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bookService.GetAllAsync());
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(BookModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(int bookId)
        {
            return Ok(await _bookService.GetByIdAsync(bookId));
        }

        //[HttpPost]
        //[ProducesResponseType((int)HttpStatusCode.Created)]
        //public async Task<IActionResult> Post([FromBody] BookAdditionRequest bookCreationRequest)
        //{
        //    int entityId = await _bookService.CreateBook(bookCreationRequest);
        //    return CreatedAtAction(nameof(GetById), new { bookId = entityId }, new { bookId = entityId });
        //}
    }
}
