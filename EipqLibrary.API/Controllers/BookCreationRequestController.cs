using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCreationRequestController : ControllerBase
    {
        private readonly IBookCreationRequestService _bookCreationRequestService;

        public BookCreationRequestController(IBookCreationRequestService bookCreationRequestService)
        {
            _bookCreationRequestService = bookCreationRequestService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] BookAdditionRequest bookCreationRequest)
        {
            int entityId = await _bookCreationRequestService.CreateBookAdditionRequestAsync(bookCreationRequest);
            return Ok(new { bookCreationRequestId = entityId });
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _bookCreationRequestService.GetAllAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery]int id)
        {
            var request = await _bookCreationRequestService.GetByIdAsync(id);
            return Ok(request);
        }
    }
}
