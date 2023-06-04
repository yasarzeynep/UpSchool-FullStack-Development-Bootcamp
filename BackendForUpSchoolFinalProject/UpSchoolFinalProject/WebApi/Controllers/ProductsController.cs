using Application.Features.Products.Commands.Add;
using Application.Features.Products.Queries.GetAll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ApiControllerBase
    {

        [HttpPost] //Added
        public async Task<IActionResult> AddAsync(ProductAddCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet] //ıd, data 
        public async Task<IActionResult> GetAllAsync(ProductGetAllQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

    }
}
