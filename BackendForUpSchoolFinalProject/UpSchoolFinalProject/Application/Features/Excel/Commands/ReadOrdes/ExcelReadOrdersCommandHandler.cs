using Application.Common.Interfaces;
using Application.Common.Models.Excel;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Excel.Commands.ReadOrdes
{
    public class ExcelReadOrdersCommandHandler : IRequestHandler<ExcelReadOrdersCommand, Response<int>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IExcelService _excelService;
        public ExcelReadOrdersCommandHandler(IApplicationDbContext applicationDbContext, IExcelService excelService)
        {
            _applicationDbContext = applicationDbContext;
            _excelService = excelService;
        }

        public Task<Response<int>> Handle(ExcelReadOrdersCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            //order mı? product mı? hangisi
            //var orderDtos = _excelService.ReadOrders(MapCommandToExcelBase64Dto(request));

            //var cities = orderDtos.Select(x => x.MapToCity()).ToList();

            //await _applicationDbContext.Cities.AddRangeAsync(cities, cancellationToken);

            //await _applicationDbContext.SaveChangesAsync(cancellationToken);

            //return new Response<int>($"{orders.Count} orders were added to the db successfully.", orders.Count);
        }
        private ExcelBase64Dto MapCommandToExcelBase64Dto(ExcelReadOrdersCommand command)
        {
            return new ExcelBase64Dto()
            {
                File = command.ExcelBase64File
            };
        }
    }
}

