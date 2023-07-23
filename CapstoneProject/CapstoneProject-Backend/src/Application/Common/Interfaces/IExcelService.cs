using Application.Common.Models.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IExcelService
    {
        // Task<List<ExcelOrderDto>> SaveExcelAsync(ExcelOrderDto saveDto, CancellationToken cancellationToken);
        List<ExcelOrderDto> ReadOrders(ExcelBase64Dto excelDto);
    }
}
 