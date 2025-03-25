using Domain.Common.BaseEntities;
using Domain.ValueObjects.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Order : BaseAuditableEntity
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.InProgress;

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public double TotalAmount { get; set; } = 0;

        public string Note { get; set; } = String.Empty;
        public ICollection<OrderItem> Items { get; set;}
    }
}
