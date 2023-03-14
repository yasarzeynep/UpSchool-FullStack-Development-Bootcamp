using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Queries.GetById;

public class AddressGetByIdQueryHandler : IRequestHandler<AddressGetByIdQuery, AddressGetByIdDto>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public AddressGetByIdQueryHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<AddressGetByIdDto> Handle(AddressGetByIdQuery request, CancellationToken cancellationToken)
    {

        var address = await _applicationDbContext.Addresses.Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return new AddressGetByIdDto()
        {
            Name = address.Name,
            District = address.District,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            PostCode = address.PostCode
        };

    }
}