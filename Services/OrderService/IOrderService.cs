using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.OrderService
{
    public interface IOrderService
    {
        Task<Orders> CreateOrder(OrderDto orderDto);
        Task<Orders> GetOrderById(Guid orderId);
        Task<IEnumerable<Orders>> GetAllOrders();
        Task<IEnumerable<Orders>> GetOrdersByUserId(Guid userId);
        Task<Orders> UpdateOrder(Guid id, OrderDto orderDto);
        Task<bool> DeleteOrder(Guid orderId);
        Task<bool> SoftDeleteOrder(Guid orderId);
    }
}