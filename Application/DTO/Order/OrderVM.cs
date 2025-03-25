using Domain.Entities;
using Domain.ValueObjects.Enums;
using System.Net;

namespace Application.DTO.Order
{
    public class OrderVM
    {
        public string UserId { get; set; }
        public List<OrderItemsVM> Items { get; set; }
    }
}
