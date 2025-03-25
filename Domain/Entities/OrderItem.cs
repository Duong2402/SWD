using Domain.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem : BaseAuditableEntity
    {
        [ForeignKey(nameof(Order))]
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public double Total {  get; set; }

        // Nav props
        [JsonIgnore]
        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
