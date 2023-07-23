using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Excel.Commands.ReadOrdes
{
    public class ExcelReadOrdersCommand : IRequest<Response<int>>
    {
        public string ExcelBase64File { get; set; }


    }
}
