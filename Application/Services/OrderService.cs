using Application.Common.Pagination;
using Application.DTO.Order;
using Application.Interfaces.Pagination;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Services
{
    public class OrderService
    {
        private readonly IGenericRepository<Order> _orderRepos;
        private readonly IGenericRepository<OrderItem> _itemRepository;
        private readonly IGenericRepository<Product> _product;
        private readonly UserManager<User> _user;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IGenericRepository<Order> orderRepos, IUnitOfWork unitOfWork,
            IGenericRepository<OrderItem> itemRepository, UserManager<User> user
            , IGenericRepository<Product> product)
        {
            _orderRepos = orderRepos;
            _itemRepository = itemRepository;
            _user = user;
            _unitOfWork = unitOfWork;
            _product = product;
        }


        public async Task<int> CreateOrder(OrderVM orderItems)
        {
            var user = await _user.FindByIdAsync(orderItems.UserId);
            if (user == null)
            {
                throw new ArgumentException("Id is not found");
            }

            if (orderItems == null || orderItems.Items.Any(x => x.Quantity <= 0))
            {
                throw new ArgumentException("List item is error");
            }

            double sum = 0;
            foreach (var item in orderItems.Items)
            {
                var product = await _product.GetByIdAsync(Guid.Parse(item.ProductId));
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {item.ProductId} not found");
                }
                sum += item.Quantity * product.Price;
            }

            var orderCreate = new Order
            {
                UserId = user.Id,
                TotalAmount = sum,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = user.Id,
                OrderDate = DateTime.UtcNow,
                Status = Domain.ValueObjects.Enums.OrderStatusEnum.InProgress,
                Id = Guid.NewGuid()
            };

            await _orderRepos.AddAsync(orderCreate);
            var saveCount = await _unitOfWork.SaveChangesAsync();
            foreach (var item in orderItems.Items)
            {
                var product = await _product.GetByIdAsync(Guid.Parse(item.ProductId));
                if (product != null)
                {
                    var itemOrder = new OrderItem
                    {
                        Id = new Guid(),
                        OrderId = orderCreate.Id,
                        ProductId = Guid.Parse(item.ProductId),
                        Quantity = item.Quantity,
                        Total = item.Quantity * product.Price,
                    };
                    await _itemRepository.AddAsync(itemOrder);
                    saveCount += await _unitOfWork.SaveChangesAsync();
                }
            }
            return saveCount == (orderItems.Items.Count + 1) ? saveCount : 0;
        }

        [HttpGet("filter")]
        public async Task<IPagedResult<Order>> GetAllOrder(
            [FromQuery] string? userId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {

            Guid? userGuid = null;
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out var parseId))
            {
                userGuid = parseId;
            }
            Expression<Func<Order, bool>> filter = f =>
                 userGuid == null || f.UserId == userGuid;

            var listOrder = await _orderRepos.FilterAll(filter, "Items,User");
            if (listOrder != null)
            {
                var result = PageMethod.ToPaginatedList<Order>(listOrder, page, pageSize);
                return result;
            }
            throw new ArgumentException("List item is error");
        }

        public async Task<Order> GetOrderItemsById(Guid orderId)
        {
            var order = await _orderRepos.GetDetailById(orderId, "Items.Product.Media");
            if (order == null)
            {
                throw new ArgumentException("Order is not found");
            }
            return (order);
        }

        public async Task<int> UpdateOrder(Guid orderId,int orderStatus)
        {
            var orderFind = await _orderRepos.GetByIdAsync(orderId);
            if (orderFind == null)
            {
                throw new ArgumentException("Order is not found");
            }
            
            orderFind.Status = (OrderStatusEnum) orderStatus;

            await _orderRepos.UpdateAsync(orderFind);
            var result = await _unitOfWork.SaveChangesAsync();
            Console.WriteLine($"After Update - OrderId: {orderId}, Status: {orderFind.Status}");
            return result;
        }

    }
}
