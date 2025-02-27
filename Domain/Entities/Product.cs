﻿using Domain.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Product : BaseAuditableEntity {
        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }

        public string Name {  get; set; }

        public string Description { get; set; }

        public ICollection<Media>? Media { get; set; }

        public double Price { get; set; }

        public int StockQuantity {  get; set; }

        // Nav props
        public Category Category { get; set; }
    }
}
