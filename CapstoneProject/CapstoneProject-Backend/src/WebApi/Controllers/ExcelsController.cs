using Application.Features.Excel.Commands.ReadOrdes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelsController : ApiControllerBase
    {
        [HttpPost("ReadOrders")]
        public async Task<IActionResult> ReadCitiesAsync(ExcelReadOrdersCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
