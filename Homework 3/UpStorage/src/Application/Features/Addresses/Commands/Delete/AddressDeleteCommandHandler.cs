using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Commands.Delete;

public class AddressDeleteCommandHandler : IRequestHandler<AddressDeleteCommand, Response<int>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public AddressDeleteCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Response<int>> Handle(AddressDeleteCommand request, CancellationToken cancellationToken)
    {
        var address = await _applicationDbContext.Addresses.Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (address == null) return new Response<int>("The address can not be found");

        if (address.IsDeleted == false) return new Response<int>($"The address has been already deleted!");
        _applicationDbContext.Addresses.Update(address = new Domain.Entities.Address()
        {
            IsDeleted = true,
            DeletedOn = DateTimeOffset.UtcNow,
            DeletedByUserId = null
        });

        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return new Response<int>($"{address.Name} address has been successfully deleted.");
    }
}

