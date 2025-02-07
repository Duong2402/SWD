using Domain.ValueObjects.Enums;

namespace Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } 
        public StockStatusEnum StockStatus { get; set; }
    }
}
