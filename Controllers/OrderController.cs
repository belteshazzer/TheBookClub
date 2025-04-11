using Microsoft.AspNetCore.Mvc;
using TheBookClub.Models.Dtos;
using TheBookClub.Services.OrderService;
using RLIMS.Common;

namespace TheBookClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = orders,
                Message = "Orders retrieved successfully."
            });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = orders,
                Message = "Orders retrieved successfully."
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Order not found."
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = order,
                Message = "Order retrieved successfully."
            });
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid order data."
                });
            }

            var order = await _orderService.CreateOrder(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, new ApiResponse
            {
                StatusCode = 201,
                Data = order,
                Message = "Order created successfully."
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid order data."
                });
            }

            var order = await _orderService.UpdateOrder(id, orderDto);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = order,
                Message = "Order updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var result = await _orderService.DeleteOrder(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Order deleted successfully." : "Failed to delete order."
            });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteOrder(Guid id)
        {
            var result = await _orderService.SoftDeleteOrder(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Order soft deleted successfully." : "Failed to soft delete order."
            });
        }
    }
}