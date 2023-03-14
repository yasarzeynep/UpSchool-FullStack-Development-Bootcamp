using Domain.Common;
using MediatR;

namespace Application.Features.Addresses.Queries.GetById;

public class AddressGetByIdQuery : IRequest<AddressGetByIdDto>
{
    public int Id { get; set; }
}