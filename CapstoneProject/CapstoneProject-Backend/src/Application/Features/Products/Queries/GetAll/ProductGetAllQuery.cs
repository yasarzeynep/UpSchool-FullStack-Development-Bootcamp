using MediatR;

namespace Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllQuery : IRequest<List<ProductGetAllDto>>
    {
        public int OrderId { get; set; }
        public bool? IsDeleted { get; set; }

        public ProductGetAllQuery(int orderId, bool? isDeleted)
        {
            OrderId = orderId;
            IsDeleted = isDeleted;
        }
    }
}

