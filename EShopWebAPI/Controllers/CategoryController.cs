using Application.DTO.BaseDTO;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Category> _repository;
        private readonly IUnitOfWork _unit;

        public CategoryController(IGenericRepository<Category> repository, IUnitOfWork unit)
        {
            _repository = repository;
            _unit = unit;
        }

        // Lấy tất cả danh mục
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _repository.GetAllAsync();
            return Ok(categories);
        }

        // Lấy danh mục theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(category);
        }

        // Thêm danh mục mới (Sửa lỗi CreatedAtAction)
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(CategoryCreateDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(new { message = "Invalid category data" });
            }

            var category = new Category
            {
                Id = Guid.NewGuid(), // Tạo ID mới
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            await _repository.AddAsync(category);
            await _unit.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // Cập nhật danh mục
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, CategoryCreateDto updatedCategory)
        {

            var existingCategory = await _repository.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;

            await _repository.UpdateAsync(existingCategory);
            await _unit.SaveChangesAsync();

            return NoContent();
        }

        // Xóa danh mục
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            await _repository.DeleteAsync(id);
            await _unit.SaveChangesAsync();
            return NoContent();
        }
    }
}
