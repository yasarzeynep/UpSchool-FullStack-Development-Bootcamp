using Domain.Common;
using MediatR;

namespace Application.Features.Addresses.Commands.HardDelete;

public class AddressHardDeleteCommand : IRequest<Response<int>>
{
    public int Id { get; set; }

}