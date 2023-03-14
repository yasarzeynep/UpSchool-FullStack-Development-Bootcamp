using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Addresses.Queries.GetAll;

public class AddressGetAllQueryHandler : IRequestHandler<AddressGetAllQuery, List<AddressGetAllDto>>
{
    private readonly IApplicationDbContext _applicationDbContext;

    public AddressGetAllQueryHandler(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<AddressGetAllDto>> Handle(AddressGetAllQuery query, CancellationToken cancellationToken)
    {
        var addressQuery = _applicationDbContext.Addresses.AsQueryable();

        addressQuery = addressQuery.Where(x => x.UserId == query.UserId);

        addressQuery = addressQuery.Include(x => x.City);
        addressQuery = addressQuery.Include(x => x.Country);


        if (query.IsDeleted.HasValue)
        {
            addressQuery = addressQuery.Where(x => x.IsDeleted == query.IsDeleted.Value);
        }

        var addresses = await addressQuery.ToListAsync(cancellationToken);

        var adressDtos = MapAddressesToGetAllDtos(addresses);

        return adressDtos.ToList();
    }

    private IEnumerable<AddressGetAllDto> MapAddressesToGetAllDtos(List<Domain.Entities.Address> addresses)
    {
        List<AddressGetAllDto> addressGetAllDtos = new List<AddressGetAllDto>();

        foreach (var address in addresses)
        {

            yield return new AddressGetAllDto()
            {
                Id = address.Id,
                UserId = address.UserId,
                Name = address.Name,
                CountryId = address.CountryId,
                CountryName = address.Country.Name,
                CityId = address.CityId,
                CityName = address.City.Name,
                District = address.District,
                PostCode = address.PostCode,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                IsDeleted = address.IsDeleted,
                AddressType = address.AddressType
            };
        }
    }

}
