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

        [HttpGet("orders/lookups")]
        public async Task<IActionResult> GetLookupsAsync()
        {
            var response = await _orderService.GetLookupsAsync();

            return Ok(response);
        }

        [HttpGet("orders/{id:int}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var order = await _orderService.GetOrderForEditAsync(id);

            if (order is null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderEditor request)
        {
            try
            {
                var id = await _orderService.CreateOrderAsync(request);

                return CreatedAtRoute("GetOrder", new { id }, new CreateOrderResponse { IdOrder = id });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("orders/{id:int}")]
        public async Task<IActionResult> UpdateOrderAsync(
            int id,
            [FromBody] OrderEditor request)
        {
            try
            {
                var updated = await _orderService.UpdateOrderAsync(id, request);

                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}