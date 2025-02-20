using Domain.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartItem : BaseAuditableEntity
    {
        [ForeignKey(nameof(Cart))]
        public Guid CartId { get; set; }

        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public double Total {  get; set; }

        // Nav props
        public Cart Cart { get; set; }

        public Product Product { get; set; }
    }
}
