using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Features.OrderEvents.Queries.GetAll
{
    public class OrderEventGetAllQuery : IRequest<List<OrderEventGetAllDto>>
    {
        //public Guid OrderId { get; set; }

        //public OrderEventGetAllQuery(Guid orderId)
        //{
        //    OrderId = orderId;
        //}
        public OrderEventGetAllQuery(bool? isDeleted)
        {
            IsDeleted = isDeleted;
        }
        public bool? IsDeleted { get; set; }
    }
}

