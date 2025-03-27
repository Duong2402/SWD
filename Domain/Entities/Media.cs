using Domain.Common.BaseEntities;

namespace Domain.Entities
{
    public class Media : BaseAuditableEntity
    {
        public string Url { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
