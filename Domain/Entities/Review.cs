using Domain.Common.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review : BaseAuditableEntity
    {
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }

        public double Rating { get; set; }

        public string Comment { get; set; }

        // Navigation properties
        public User User { get; set; }
        
        public Product Product { get; set; }

    }
}
