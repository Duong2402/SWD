﻿using Application.DTO;
using Application.DTO.ProductDTO;
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
            [FromQuery] string? category,
            [FromQuery] double? min,
            [FromQuery] double? max,
            [FromQuery] int page,
            [FromQuery] int pageSize = 10)
        {
           var products = await _productServices.Filter(name,category, min, max, page, pageSize);
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

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetDetailById(Guid productId)
        {
            var result = await _productServices.FigureDetailDTO(productId);

            if (result == null)
            {
                return (NotFound());
            }

            return (Ok(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
        public async Task<IActionResult> Update(Guid postId, [FromForm] ProductCreateDto dto)
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

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            try
            {
                var categories = await _productServices.CategoryList();

                if (categories == null || !categories.Any())
                {
                    return NotFound("No categories found.");
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CategoryList API: {ex.Message}");

                return StatusCode(500, "An error occurred while retrieving categories.");
            }
        }



    }
}
