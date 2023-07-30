using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.Add
{
    public class ProductAddCommandHandler : IRequestHandler<ProductAddCommand, Response<int>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductAddCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<int>> Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            var product = new Product()
            {
                Name = request.Name,
                OrderId = Guid.Parse(request.OrderId),
                Price = request.Price,
                Picture = request.Picture,
                IsOnSale = request.IsOnSale,
                SalePrice = request.SalePrice,
                // CreatedOn = DateTimeOffset.Now,
                IsDeleted = false
            };

            await _applicationDbContext.Products.AddAsync(product, cancellationToken);

            return new Response<int>("Congratulations!Product was successfully added.");
        }
    }
}
