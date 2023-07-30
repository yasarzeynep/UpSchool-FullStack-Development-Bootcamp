using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetAll
{
    public class OrderGetAllQueryHandler : IRequestHandler<OrderGetAllQuery, List<OrderGetAllDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderGetAllQueryHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<OrderGetAllDto>> Handle(OrderGetAllQuery request, CancellationToken cancellationToken)
        {
            var orders = await _applicationDbContext.Orders
                .Where(x => !request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted.Value)
                .ToListAsync(cancellationToken);

            var orderDtos = orders.Select(MapOrderToDto).ToList();
            return orderDtos;
        }

        private OrderGetAllDto MapOrderToDto(Order order)
        {
            return new OrderGetAllDto
            {
                Id = order.Id,
                RequestedAmount = order.RequestedAmount,
                TotalFoundAmount = order.TotalFoundAmount,
                ProductCrawlType = order.ProductCrawlType,
                IsDeleted = order.IsDeleted,
            };
        }
    }


}
