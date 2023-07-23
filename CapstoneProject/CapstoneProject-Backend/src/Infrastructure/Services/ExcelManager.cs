using Application.Common.Interfaces;
using Application.Common.Models.Excel;
using ExcelMapper;
using Infrastructure.Common.Excel.ExcelMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ExcelManager : IExcelService
    {
        public List<ExcelOrderDto> ReadOrders(ExcelBase64Dto excelDto)
        {
            // We convert base64string to byte[]
            var fileBytes = Convert.FromBase64String(excelDto.File);

            using var stream = new MemoryStream(fileBytes);
            using var importer = new ExcelImporter(stream);

            importer.Configuration.RegisterClassMap<ExcelOrderDtoConfiguration>();

            ExcelSheet sheet = importer.ReadSheet();

            var orderDtos = sheet.ReadRows<ExcelOrderDto>().ToList();


            return orderDtos;
        }
    }
}
