using Domain.Enums;

namespace Application.Features.Addresses.Queries.GetAll;

public class AddressGetAllDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }

    public int CountryId { get; set; }
    public string CountryName { get; set; }

    public int CityId { get; set; }
    public string CityName { get; set; }

    public string District { get; set; }
    public string PostCode { get; set; }

    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }

    public bool IsDeleted { get; set; }

    public AddressType AddressType { get; set; }
}