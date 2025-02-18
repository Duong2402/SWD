using Domain.ValueObjects.Enums;

namespace Application.DTO.BaseDTO
{
    public class BaseProductDto
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Vendors { get; set; }
        public string? Type { get; set; }
        public decimal Price { get; set; }
        public StockStatusEnum StockStatus { get; set; }
    }
}
