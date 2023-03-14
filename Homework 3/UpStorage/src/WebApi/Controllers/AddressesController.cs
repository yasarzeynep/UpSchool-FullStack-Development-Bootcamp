using Application.Features.Addresses.Commands.Add;
using Application.Features.Addresses.Commands.Delete;
using Application.Features.Addresses.Commands.HardDelete;
using Application.Features.Addresses.Commands.Update;
using Application.Features.Addresses.Queries.GetAll;
using Application.Features.Addresses.Queries.GetById;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AddressesController : ApiControllerBase
{
    [HttpPost("Add")]
    public async Task<IActionResult> AddAddressesAsync(AddressAddCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAllAsync(AddressGetAllQuery query)
    {
        return Ok(await Mediator.Send(query));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        return Ok(await Mediator.Send(new AddressGetByIdQuery { Id = id }));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateByIdAsync(int id)
    {
        return Ok(Mediator.Send(new AddressUpdateCommand { Id = id }));
    }

    [HttpDelete("SoftDelete")]
    public async Task<IActionResult> DeleteByIdAsync(int id)
    {
        return Ok(Mediator.Send(new AddressDeleteCommand { Id = id }));
    }


    [HttpDelete("HardDelete")]
    public async Task<IActionResult> HardDeleteByIdAsync(int id)
    {
        return Ok(Mediator.Send(new AddressHardDeleteCommand { Id = id }));
    }


}