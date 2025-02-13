using Application.DTO;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShopWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FigureController(FigureServices figureServices) : ControllerBase
    {
        private readonly FigureServices _figureServices = figureServices;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _figureServices.GetAllAsync();

            return new JsonResult(Ok(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(int page, int size = 10)
        {
            var result = await _figureServices.GetPagedAsync(page, size);

            return new JsonResult(Ok(result));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            var result = await _figureServices.GetByIdAsync(productId);

            if (result == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FigureDto dto)
        {
            try
            {
                await _figureServices.CreateFigureAsync(dto);
            }
            catch (ValidationException ex)
            {
                return new JsonResult(BadRequest($"{ex.GetType().Name}: {ex.Message}"));
            }

            return new JsonResult(Created());
        }

        [HttpPut("{postId}")]
        public async Task<IActionResult> Update(Guid postId, [FromBody] FigureDto dto)
        {
            try
            {
                await _figureServices.UpdateFigureAsync(postId, dto);
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
                await _figureServices.DeleteFigureAsync(postId);
            }
            catch (KeyNotFoundException ex)
            {
                return new JsonResult(NotFound($"{ex.GetType().Name}: {ex.Message}"));
            }

            return new JsonResult(NoContent());
        }
    }
}
