using Domain.Common.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Category Category { get; set; }
    }
}
