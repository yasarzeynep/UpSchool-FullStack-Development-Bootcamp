﻿using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Update
{
    public class OrderUpdateCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }
        public string RequestedAmount { get; set; }
        public int TotalFoundAmount { get; set; }

    }
}
