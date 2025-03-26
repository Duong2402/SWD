using System.ComponentModel.DataAnnotations;

namespace Application.DTO.BaseDTO
{
    public class CartDto
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

    }
}
