using Microsoft.AspNetCore.Mvc;
using PostMortem.APPLICATION.Implementations;
using PostMortemTests.Models;
using PostMortemTests.Services;

namespace PostMortemTests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(IOrderService _orderService) : ControllerBase
    {
        [HttpPost]
        [ServiceFilter(typeof(StructuredLoggingAttribute))]
        public async Task<IActionResult> Order([FromBody] OrderDTO orderDto)
        {
            try
            {
                await _orderService.PlaceOrder(orderDto);

                return Ok("Order placed successfully!");
            }
            catch
            {
                return BadRequest("Order could not be placed!");
            }
        }
    }
}
