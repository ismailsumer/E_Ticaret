using ETicaretAPI.DTOs;
using ETicaretAPI.Services;
using Microsoft.AspNetCore.Mvc;
using ETicaretAPI.Wrappers;
using Microsoft.AspNetCore.Authorization;

namespace ETicaretAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1, 
            [FromQuery] int size = 10, 
            [FromQuery] string? search = null, 
            [FromQuery] int? categoryId = null, 
            [FromQuery] int? brandId = null)
        {
            var result = await _productService.GetAllProductsAsync(page, size, search, categoryId, brandId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto productDto)
        {
            var result = await _productService.AddProductAsync(productDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, ProductUpdateDto productDto)
        {
            var result = await _productService.UpdateProductAsync(id, productDto);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost("{id}/upload-image")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            var result = await _productService.UploadImageAsync(id, file);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}