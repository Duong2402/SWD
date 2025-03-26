using Microsoft.AspNetCore.Http;

namespace Application.DTO
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public double Price {  get; set; }
        public int StockQuantity { get; set; }
        public List<IFormFile>? Media { get; set; }
        public string CategoryId { get; set; }
    }
}
