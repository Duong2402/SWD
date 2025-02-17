using Domain.ValueObjects.Enums;

namespace Domain.Common.BaseEntities
{
    public class BaseProduct
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public string Vendors { get; set; }
        public string Type { get; set; }
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public DateTime? DateUpdate { get; set; }

        public StockStatusEnum StockStatus { get; set; }
    }
}
