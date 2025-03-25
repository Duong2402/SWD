using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.BaseDTO
{
    public class CartDto
    {
        public Guid UserId { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

    }
}
