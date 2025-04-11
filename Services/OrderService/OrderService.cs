using AutoMapper;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;

namespace TheBookClub.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Orders> _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepository<Orders> orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Orders> CreateOrder(OrderDto orderDto)
        {
            var order = _mapper.Map<Orders>(orderDto);
            await _orderRepository.AddAsync(order);
            return order;
        }

        public async Task<Orders> GetOrderById(Guid orderId)
        {
            return await _orderRepository.GetByIdAsync(orderId);
        }

        public async Task<IEnumerable<Orders>> GetOrdersByUserId(Guid userId)
        {
            return await _orderRepository.GetByConditionAsync(o => o.UserId == userId);
        }

        public async Task<IEnumerable<Orders>> GetAllOrders()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Orders> UpdateOrder(Guid id, OrderDto orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            _mapper.Map(orderDto, order);
            await _orderRepository.UpdateAsync(order);
            return order;
        }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
            return await _orderRepository.DeleteAsync(orderId);
        }

        public async Task<bool> SoftDeleteOrder(Guid orderId)
        {
            return await _orderRepository.SoftDeleteAsync(orderId);
        }
    }
}