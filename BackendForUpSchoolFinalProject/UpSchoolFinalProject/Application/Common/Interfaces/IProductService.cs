using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IProductService
    {
        Task<Guid> AddProduct(Product product);
        Task<List<Product>> GetProducts();
    }
}
