using Application.DTO.BaseDTO;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController(CartServices cartService, ProductServices figureServices) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCart(Guid userId)
        {
            var cart = await cartService.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                await cartService.CreateCartAsync(cart);
            }
            var cartItems = cart.CartItems.Select(ci => new
            {
                ci.CartId,
                ci.ProductId,
                ProductName = ci.Product.Name,
                ProductPrice = ci.Product.Price,
                ci.Quantity,
                ci.Total,
            }).ToList();
            return Ok(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartDto cartDTO)
        {
            var product = await figureServices.GetByIdAsync(cartDTO.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            var cart = await cartService.GetCartByUserIdAsync(cartDTO.UserId);
            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = cartDTO.UserId,
                    CartItems = new List<CartItem>()
                };
                await cartService.CreateCartAsync(cart);
            }
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == product.Id);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = cartDTO.ProductId,
                    Quantity = cartDTO.Quantity,
                    Total = product.Price * cartDTO.Quantity
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += cartDTO.Quantity;
                cartItem.Total = product.Price * cartItem.Quantity;
            }
            await cartService.UpdateCartAsync(cartItem);
            return Ok(cart);

        }
        [HttpDelete]
        public async Task<IActionResult> RemoveFromCart(Guid cartId, Guid productId)
        {
            try
            {
                await cartService.DeleteCartItemAsync(cartId, productId);
            }
            catch (KeyNotFoundException ex)
            {
                return new JsonResult(NotFound($"{ex.GetType().Name}: {ex.Message}"));
            }
            return new JsonResult(NoContent());
        }
        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(CartDto dto)
        {
            var product = await figureServices.GetByIdAsync(dto.ProductId);
            if (product == null)
            {
                return NotFound();
            }
            var cart = await cartService.GetCartByUserIdAsync(dto.UserId);
            if (cart == null)
            {
                return NotFound();
            }
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == product.Id);
            if (cartItem != null)
            {
                cartItem.Quantity = dto.Quantity;
                cartItem.Total = product.Price * cartItem.Quantity;
                await cartService.UpdateCartAsync(cartItem);
            }
            return Ok(cart);
        }
    }
}
