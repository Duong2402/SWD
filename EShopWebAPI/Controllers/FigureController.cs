using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShopWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FigureController(ProductServices productServices) : ControllerBase
    {
        private readonly ProductServices _productServices = productServices;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productServices.GetAllAsync();

            return new JsonResult(Ok(result));
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterProduct(
            [FromQuery] string? name,
            [FromQuery] string? type,
            [FromQuery] string? category,
            [FromQuery] decimal? min,
            [FromQuery] decimal? max,
            [FromQuery] int page,
            [FromQuery] int pageSize = 10)
        {
           var products = await _productServices.Filter(name, type, category, min, max, page, pageSize);
            if(products == null )
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(int page, int size = 10)
        {
            var result = await _productServices.GetPagedAsync(page, size);

            return new JsonResult(Ok(result));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            var result = await _productServices.GetByIdAsync(productId);

            if (result == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            try
            {
                await _productServices.CreateFigureAsync(dto);
            }
            catch (ValidationException ex)
            {
                return new JsonResult(BadRequest($"{ex.GetType().Name}: {ex.Message}"));
            }

            return new JsonResult(Created());
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> Update(Guid postId, [FromBody] ProductCreateDto dto)
        {
            try
            {
                await _productServices.UpdateFigureAsync(postId, dto);
            }
            catch (KeyNotFoundException ex)
            {
                return new JsonResult(NotFound($"{ex.GetType().Name}: {ex.Message}"));
            }

            return new JsonResult(Accepted(dto));

        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> Delete(Guid postId)
        {
            try
            {
                await _productServices.DeleteFigureAsync(postId);
            }
            catch (KeyNotFoundException ex)
            {
                return new JsonResult(NotFound($"{ex.GetType().Name}: {ex.Message}"));
            }

            return new JsonResult(NoContent());
        }

       
    }
}
