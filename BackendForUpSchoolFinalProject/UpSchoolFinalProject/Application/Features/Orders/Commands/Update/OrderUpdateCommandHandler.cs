using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Update
{
    public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand, Response<Guid>>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public OrderUpdateCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Response<Guid>> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var order = await _applicationDbContext.Orders.FindAsync(request.Id);

            if (order == null)
            {
                return new Response<Guid>("Order not found.");
            }

            // Değiştirilen değerleri doğrudan güncelle.
            order.TotalFoundAmount = request.TotalFoundAmount;

            if (request.RequestedAmount.ToLower() == "all")
            {
                // all ise , tüm miktarı güncelle
                order.RequestedAmount = order.TotalFoundAmount;
            }
            else if (int.TryParse(request.RequestedAmount, out int requestedAmount))
            {
                // eğer sayı belirtildiyse, sayıyı güncelle
                order.RequestedAmount = requestedAmount;
            }
            else
            {
                return new Response<Guid>("Error:Invalid requested amount.");
            }

            // Değişiklikleri veritabanına kaydetmek için SaveChangesAsync() metodu çağrılır.
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return new Response<Guid>(order.Id);
        }
    }


}
