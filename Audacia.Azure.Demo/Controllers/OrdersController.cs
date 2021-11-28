using System.Threading.Tasks;
using Audacia.Azure.Demo.Models.Requests.Queue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Audacia.Azure.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            return Ok();
        }

        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] AddQueueRequest request)
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put()
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] object request)
        {
            return Ok();
        }
    }
}