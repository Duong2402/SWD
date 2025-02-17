using Domain.Common.BaseEntities;

namespace Domain.Entities
{
    public class Figure : BaseProduct
    {
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
