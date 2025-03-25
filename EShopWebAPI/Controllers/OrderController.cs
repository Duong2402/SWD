using Application.Common.Pagination;
using Application.DTO.Order;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EShopWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _order;

        public OrderController(OrderService order)
        {
            _order = order;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderVM orders)
        {
            try
            {
                var result = await _order.CreateOrder(orders);
                if (result > 0)
                {
                    return Ok(new { Message = "Order created successfully", OrderId = result });
                }
                return BadRequest(new { Message = "Failed to create order" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetAllOrder(
            [FromQuery] string? userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _order.GetAllOrder(userId, page, pageSize);

                if (result == null || !result.Items.Any())
                {
                    return NotFound(new { Message = "No orders found" });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpGet("Detail/{orderId}")]
        public async Task<IActionResult> GetOrderItemsById(Guid orderId)
        {
            try
            {
                var order = await _order.GetOrderItemsById(orderId);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateOrder([FromQuery] Guid orderId, [FromBody] int orderStatus)
        {
            try
            {
                var result = await _order.UpdateOrder(orderId,orderStatus);
                if (result > 0)
                {
                    return Ok(new { Message = "Order updated successfully" });
                }
                return BadRequest(new { Message = "Failed to update order" });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Error = ex.Message });
            }
        }



    }
}
