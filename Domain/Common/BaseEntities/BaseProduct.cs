using Domain.ValueObjects.Enums;

namespace Domain.Common.BaseEntities
{
    public class BaseProduct
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Decimal? Price { get; set; }
        public StockStatusEnum StockStatus { get; set; }
    }
}
