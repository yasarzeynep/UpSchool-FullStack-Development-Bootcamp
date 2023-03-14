using Application.Common.Interfaces;
using Application.Features.Addresses.Commands.Delete;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Commands.HardDelete;

public class AddressDeleteCommandHandler : IRequestHandler<AddressDeleteCommand, Response<int>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public AddressDeleteCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Response<int>> Handle(AddressDeleteCommand request, CancellationToken cancellationToken)
    {
        var deletedAddress = await _applicationDbContext.Addresses.Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (deletedAddress == null) return new Response<int>("The address can not be found");

        _applicationDbContext.Addresses.Remove(deletedAddress);

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new Response<int>($"{deletedAddress.Name} address has been successfully removed from database.");
    }
}