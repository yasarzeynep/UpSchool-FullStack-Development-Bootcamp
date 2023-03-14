using MediatR;

namespace Application.Features.Addresses.Queries.GetAll;

public class AddressGetAllQuery : IRequest<List<AddressGetAllDto>>
{
    public int UserId { get; set; }
    public bool? IsDeleted { get; set; }

    public AddressGetAllQuery(int userId, bool? isIsDeleted)
    {
        UserId = userId;

        IsDeleted = isIsDeleted;
    }
}
