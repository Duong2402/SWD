using Domain.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Order
{
    public class OrderUpdateVM
    {
        public OrderStatusEnum OrderStatus { get; set; }
    }
}
