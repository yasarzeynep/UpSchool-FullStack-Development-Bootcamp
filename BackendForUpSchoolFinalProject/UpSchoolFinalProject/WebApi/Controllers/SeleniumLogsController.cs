using Application.Common.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi.Hubs;

namespace UpSchool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeleniumLogsController : ControllerBase
    {
        private readonly IHubContext<SeleniumLogHub> _seleniumLogHubContext; //Hangi hub metodu tetikleyeceksen onu yazıyoruz

        public SeleniumLogsController(IHubContext<SeleniumLogHub> seleniumLogHubContext)
        {
            _seleniumLogHubContext = seleniumLogHubContext;
        }

        [HttpPost]
        public async Task<IActionResult> SendLogNotificationAsync(SendLogNotificationApiDto logNotificationApiDto)
        {
            await _seleniumLogHubContext.Clients.AllExcept(logNotificationApiDto.ConnectionId)
                .SendAsync("NewSeleniumLogAdded", logNotificationApiDto.Log);

            return Ok();
        }
    }
}

