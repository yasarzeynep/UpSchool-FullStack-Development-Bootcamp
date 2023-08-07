using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.Delete
{
    public class ProductDeleteCommandHandler : IRequestHandler<ProductDeleteCommand, Response<int>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public ProductDeleteCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Response<int>> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            // Veritabanından ürünü alır.
            var product = await _applicationDbContext.Products
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                throw new Exception("The product was not found.");
            }

            // Ürünün IsDeleted özelliği true olarak işaretlenir (silinmiş olarak işaretlenir).
            product.IsDeleted = true;

            // Değişiklikleri veritabanına kaydetmek için Update() metodu kullanılır.
            _applicationDbContext.Products.Update(product);

            // Değişiklikleri veritabanına kaydetmek için SaveChangesAsync() metodu çağrılır.
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            // İşlem başarılı ise yanıt olarak bir Response nesnesi döndürülür.
            return new Response<int>( $"The product with ID {product.Id} was successfully deleted.");
        }

      
    }
    }

