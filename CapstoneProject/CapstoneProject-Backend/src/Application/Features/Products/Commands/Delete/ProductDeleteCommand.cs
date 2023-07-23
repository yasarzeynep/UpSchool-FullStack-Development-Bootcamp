using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands.Delete
{
    public class ProductDeleteCommand : IRequest<Response<int>>
    {
        public Guid Id { get; set; }
    }
}
