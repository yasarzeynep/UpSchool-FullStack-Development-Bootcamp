using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.Update
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, Response<int>>
    {

        private readonly IApplicationDbContext _applicationDbContext;

        public ProductUpdateCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<int>> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products
                        .Where(x => x.Id == request.Id)
                        .SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                throw new Exception("The product was not found.");
            }

            // Ürünün güncellenen özellikleri atanır.
            product.Name = request.Name;
            product.Picture = request.Picture;
            product.Price = request.Price;
            product.SalePrice = request.SalePrice;

            // Değişiklikleri veritabanına kaydetmek için SaveChangesAsync() metodu çağrılır.
            _applicationDbContext.Products.Update(product);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            // İşlem başarılı ise yanıt olarak bir Response nesnesi döndürülür.
            return new Response<int>($"The product with ID {product.Id} was successfully updated");

        }
    }
}
