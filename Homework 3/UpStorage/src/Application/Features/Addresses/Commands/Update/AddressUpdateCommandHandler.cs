using System.Runtime.Intrinsics.X86;
using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Commands.Update;

public class AddressUpdateCommandHandler : IRequestHandler<AddressUpdateCommand, Response<int>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public AddressUpdateCommandHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Response<int>> Handle(AddressUpdateCommand request, CancellationToken cancellationToken)
    {

        var updatedAddress = await _applicationDbContext.Addresses.Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (updatedAddress == null)
        {
            return new Response<int>($"The address named {request.Name} was not found", request.Id);
        }
        else
        {
            updatedAddress.Name = request.Name;
            updatedAddress.AddressLine1 = request.AddressLine1;
            updatedAddress.AddressLine2 = request.AddressLine2;
            updatedAddress.District = request.District;
            updatedAddress.AddressType = request.AddressType;
            updatedAddress.PostCode = request.PostCode;
            updatedAddress.ModifiedOn = DateTimeOffset.Now;
            updatedAddress.ModifiedByUserId = null;


            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<int>("The address updated successfully.", request.Id);
        }






    }
}
