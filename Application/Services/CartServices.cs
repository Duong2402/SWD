using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CartServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbContext _context;
        private readonly DbSet<Cart> _dbSetCart;
        private readonly DbSet<CartItem> _dbSetCartItems;
        private readonly IGenericRepository<Cart> _cartRepository;
        private readonly IGenericRepository<CartItem> _cartItemsRepository;

        private const string URLImage = "https://localhost:7241/images/";

        public CartServices(IUnitOfWork unitOfWork, UserManager<User> userManager, DbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _dbSetCart = context.Set<Cart>();
            _cartRepository = unitOfWork.GetRepository<Cart>();
            _cartItemsRepository = unitOfWork.GetRepository<CartItem>();
        }
        public async Task<Cart> GetCartByUserIdAsync(Guid userId)
        {
            return await _dbSetCart.Include(c => c.CartItems)
                                   .ThenInclude(ci => ci.Product)
                                   .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<int> CreateCartAsync(Cart cart)
        {
            await _cartRepository.AddAsync(cart);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> UpdateCartAsync(CartItem cartItem)
        {
            var existingCartItem = await _cartItemsRepository.GetByConditionAsync(ci => ci.CartId == cartItem.CartId && ci.ProductId == cartItem.ProductId);
            if (existingCartItem != null)
            {

                existingCartItem.Quantity = cartItem.Quantity;
                existingCartItem.Total = cartItem.Total;
                await _cartItemsRepository.UpdateAsync(existingCartItem);
            }
            else
            {
                await _cartItemsRepository.AddAsync(cartItem);
            }

            return await _unitOfWork.SaveChangesAsync();
        }
        public async Task<int> DeleteCartItemAsync(Guid cartId, Guid productId)
        {
            await _cartItemsRepository.DeleteByConditionAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
            return await _unitOfWork.SaveChangesAsync();
        }
    }

}
