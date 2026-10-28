using FurnitureSalon.Api.Interfaces;
using FurnitureSalon.Core.DTO.Order;
using Microsoft.AspNetCore.Mvc;

namespace FurnitureSalon.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class OrderListController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderListController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var response = await _orderService.GetAllOrdersAsync();

            return Ok(response);
        }

        [HttpPost("search/orders")]
        public async Task<IActionResult> SearchOrdersAsync([FromBody] OrderSearchRequest searchRequest)
        {
            var response = await _orderService.SearchOrdersAsync(searchRequest);

            return Ok(response);
        }
    }
}