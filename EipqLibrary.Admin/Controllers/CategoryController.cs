using EipqLibrary.Services.DTOs.Models;
using EipqLibrary.Services.DTOs.RequestModels;
using EipqLibrary.Services.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace EipqLibrary.Admin.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPut]
        [ProducesResponseType(typeof(CategoryModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] CategoryUpdateRequest categoryUpdateRequest)
        {
            var updatedCategory = await _categoryService.UpdateAsync(categoryUpdateRequest);
            return Ok(updatedCategory);
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] CategoryCreationRequest categoryCreationRequest)
        {
            int entityId = await _categoryService.CreateCategory(categoryCreationRequest);
            return Ok(new { categoryId = entityId });
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(int categoryId)
        {
            await _categoryService.DeleteAsync(categoryId);
            return Ok();
        }
    }
}
