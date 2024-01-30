using Data.MongoCollections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("api/fileupload")]
        public async Task<ActionResult> UploadFileAsync([FromForm] List<IFormFile> file)
        {
            if (file == null)
            {
                return BadRequest();
            }
            try
            {
                await _orderService.PostFileAsync(file);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] DateTime from, DateTime to)
        {
            try
            {
                var result = await _orderService.GetShippers(from, to);
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
