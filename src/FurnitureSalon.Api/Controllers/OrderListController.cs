using FurnitureSalon.Api.Interfaces;
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
    }
}