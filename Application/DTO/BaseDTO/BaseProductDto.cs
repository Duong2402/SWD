using Domain.ValueObjects.Enums;

namespace Application.DTO.BaseDTO
{
    public class BaseProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<MediaDto>? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}
